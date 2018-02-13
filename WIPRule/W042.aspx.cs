/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：nick

功能說明：提供併批功能
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/11/15      nick       初版
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.RuleUtility;
using ciMes.Web.Common;

namespace CustomizeRule.WIPRule
{
    public partial class W042 : CustomizeRuleBasePage
    {
        private List<LotInfoEx> _LotDatas
        {
            get { return this["_LotDatas"] as List<LotInfoEx>; }
            set { this["_LotDatas"] = value; }
        }
        private string _JudgeOperation
        {
            get{return (String)this["_JudgeOperation"]; }
            set{this["_JudgeOperation"] = value;}
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ClearField();
                }
                AjaxFocus(ttbWorkOrderLot);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ClearField()
        {
            _LotDatas = new List<LotInfoEx>();
            _JudgeOperation = string.Empty;
            gvWorkpiece.SetDataSource(null, true);
            btnOK.Enabled = false;
        }


        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "ManualMerge");
                if (reasonCategory == null)
                {
                    //[00030]{0}：{1}不存在!
                    throw new CimesException(TextMessage.Error.T00030(GetUIResource("ReasonCode"), "CustomizeReason- ManualMerge"));
                }

                var baseLot = _LotDatas.Find(p => p.Status == "WaitMerge");
                // 定義交易戳記
                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {          
                    //拆批
                    var split = SplitLotInfo.CreateSplitLotByLotAndQuantity(baseLot.Lot, baseLot.WorkOrderLot, 0, 0, reasonCategory, "");
                    var splitIndicator = WIPTxn.SplitIndicator.Create();
                    WIPTxn.Default.SplitLot(baseLot, split, splitIndicator, txnStamp);

                    //再取一次批號資訊
                    var newLot = LotInfo.GetLotByLot(split.Lot);
                    //併批與子單元
                    List<MergeLotInfo> mergeLotList = new List<MergeLotInfo>();
                    _LotDatas.ForEach(mergelot =>
                    {
                        var lstCompData = mergelot.GetLotAllComponents();
                        var mergeLot = MergeLotInfo.GetMergeLotByLotAndQuantity(mergelot.Lot, lstCompData, reasonCategory, "");
                        mergeLotList.Add(mergeLot);
                    });
                    WIPTransaction.MergeLot(newLot, mergeLotList, txnStamp);
                    //再取一次批號資訊
                    var newMergeLot = LotInfo.GetLotByLot(split.Lot);

                    //將批號狀態變更為wait
                    WIPTransaction.ModifyLotSystemAttribute(newMergeLot, "STATUS", LotDefaultStatus.Wait.ToCimesString(), txnStamp);
                    //將COMPLOT、PROCESS_EQUIP欄位清空，因為這個時間點這個欄位已經沒意義了
                    WIPTransaction.ModifyLotSystemAttribute(newMergeLot, "COMPLOT", string.Empty, txnStamp);
                    WIPTransaction.ModifyLotSystemAttribute(newMergeLot, "PROCESS_EQUIP", string.Empty, txnStamp);

                    //Dispatch到下一站
                    WIPTransaction.DispatchLot(newMergeLot, txnStamp);
                    cts.Complete();
                }

                ClearField();
                ttbWorkOrderLot.Text = "";
                AjaxFocus(ttbWorkOrderLot);
                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00614(""));
            }                
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //返回在製品查詢頁面
                ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ttbWorkOrderLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClearField();
                string workorderLot = ttbWorkOrderLot.Text.Trim();
                if (workorderLot.IsNullOrEmpty())
                {
                    return;
                }

                _LotDatas = LotInfoEx.GetLotListByWorkOrderLot(workorderLot);
                if (_LotDatas.Count == 0)
                {
                    // [00060] 工件清單沒有資料可顯示！
                    throw new RuleCimesException(TextMessage.Error.T00060(lblLotList.Text));
                }

                var judgeOperation = WpcExClassItemInfo.GetInfoByClass("SAIJudgeOperation").Find(p => p.Remark01 == _LotDatas[0].Process);
                if (judgeOperation == null)
                {
                    //找不到待判站資訊，請至系統資料維護增加資訊，屬性：{0}
                    throw new Exception(RuleMessage.Error.C10014(_LotDatas[0].Process));
                }
                _JudgeOperation = judgeOperation.Remark02;

                _LotDatas = _LotDatas.OrderBy(p => p.OperationSequence).ThenBy(p=>p.ComponentLot).ToList();

                var lotTemp = _LotDatas.FindAll(p => p.OperationName != _JudgeOperation);

                var enabled = true;
                lotTemp.ForEach(lot => {
                    if (lot.Status != "WaitMerge")
                    {
                        enabled = false;
                    }
                });
                btnOK.Enabled = enabled;
                //gvWorkpiece.SetDataSource(_LotDatas, true);
                gvWorkpiece.DataSource = _LotDatas;
                gvWorkpiece.DataBind();

            }
            catch (Exception ex)
            {
                AjaxFocus(ttbWorkOrderLot);
                HandleError(ex);
            }
        }

        protected void gvWorkpiece_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                var data = e.Row.DataItem as LotInfoEx;
                if (data.Status != "WaitMerge" && data.OperationName != _JudgeOperation)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}