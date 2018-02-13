/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：提供人員進行維修作業紀錄。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/08/03      keith       初版
*/

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.RuleUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.WIPRule
{
    public partial class W013 : CustomizeRuleBasePage
    {
        /// <summary>
        /// LOT資料
        /// </summary>
        private LotInfo _LotData
        {
            get { return this["_LotData"] as LotInfo; }
            set { this["_LotData"] = value; }
        }

        /// <summary>
        /// 客製資料
        /// </summary>
        private CSTWIPDefectJudgementInfo _DefectJudgementData
        {
            get { return this["_DefectJudgementData"] as CSTWIPDefectJudgementInfo; }
            set { this["_DefectJudgementData"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            ttbItemSN.Text = "";
            ttbRepairDescr.Text = "";
            ttbLot.Text = "";
            ttbJudgeReason.Text = "";

            ddlRepairReasonCode.Items.Clear();

            _LotData = null;

            rdbOK.Checked = false;
            rdbNG.Checked = false;

            btnOK.Enabled = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!UserProfileInfo.CheckUserRight(User.Identity.Name, ProgramRight))
                {
                    HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                    return;
                }

                if (!IsPostBack)
                {
                    //第一次開啟頁面
                    ClearField();
                    AjaxFocus(ttbLot);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 輸入機加批號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                #region 驗證批號存在性
                _LotData = LotInfoEx.GetLotByLot(ttbLot.Text);

                if (_LotData == null)
                {
                    //[00030]{0}：{1}不存在!
                    throw new Exception(TextMessage.Error.T00030(lblLot.Text, ttbLot.Text));
                }
                #endregion

                #region 驗證批號狀態，Run的批號
                if (_LotData.Status != LotDefaultStatus.Run.ToString())
                {
                    //狀態為{0}，不可執行
                    throw new Exception(RuleMessage.Error.C10003(_LotData.Status));
                }
                #endregion

                #region 驗證currentRule
                if (_LotData.CurrentRuleName != _ProgramInformationBlock.ProgramRight)
                {
                    //該批號作業為XXXX，不為此功能，請遵循作業規範
                    throw new Exception(RuleMessage.Error.C10004(_LotData.CurrentRuleName));
                }
                #endregion

                #region 維修原因碼(DropDownList)：依照原因碼工作站設定，帶出原因碼
                ddlRepairReasonCode.Items.Clear();

                List<BusinessReason> reasonList = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(_LotData.CurrentRuleName, _LotData.OperationName, "Default", ReasonMode.Category);

                if (reasonList.Count > 0)
                {
                    ddlRepairReasonCode.DataSource = reasonList;
                    ddlRepairReasonCode.DataTextField = "ReasonDescription";
                    ddlRepairReasonCode.DataValueField = "ReasonCategorySID";
                    ddlRepairReasonCode.DataBind();

                    if (ddlRepairReasonCode.Items.Count != 1)
                        ddlRepairReasonCode.Items.Insert(0, "");
                    else
                    {
                        ddlRepairReasonCode.SelectedIndex = 0;
                    }
                }
                else
                {
                    //[00641]規則:{0} 工作站:{1} 使用的原因碼未設定，請洽IT人員!
                    throw new Exception(TextMessage.Error.T00641(ProgramRight, _LotData.OperationName));
                }
                #endregion

                //取得工件序號
                var componentDataList = ComponentInfoEx.GetDataByCurrentLot(_LotData.Lot);
                if (componentDataList.Count > 0)
                {
                    ttbItemSN.Text = componentDataList[0].ComponentID;
                }

                //取得送待判原因
                var lotDefectData = LotDefectInfoEx.GetDataByLotAndComponentID(_LotData.Lot, ttbItemSN.Text);
                if (lotDefectData != null)
                {
                     _DefectJudgementData = CSTWIPDefectJudgementInfo.GetDataByWIPDefectSID(lotDefectData.DefectSID);

                    if (_DefectJudgementData != null)
                    {
                        ttbJudgeReason.Text = _DefectJudgementData.Reason;
                    }
                }

                ttbRepairDescr.Text = "";
                btnOK.Enabled = true;
            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbLot);
                HandleError(ex);
            }
        }

        /// <summary>
        /// 確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                //檢查維修原因是否有選擇
                ddlRepairReasonCode.Must(lblJudgeReason);

                #region 檢查維修結果是否有選擇
                if (rdbOK.Checked == false && rdbNG.Checked == false)
                {
                    throw new Exception(TextMessage.Error.T00841(lblRepairResult.Text));
                }
                #endregion

                using (var cts = CimesTransactionScope.Create())
                {
                    //選取的原因碼
                    var reasonCategory = InfoCenter.GetBySID<ReasonCategoryInfo>(ddlRepairReasonCode.SelectedValue);

                    #region 紀錄維修結束 To do...
                    List<WIPRepairFinishInfo> repairFinishDatas = new List<WIPRepairFinishInfo>();

                    var repairData = WIPRepairInfo.GetRepairByLotAndReason(_LotData.Lot, _DefectJudgementData.Reason);
                    
                    var repairFinishData = WIPRepairFinishInfo.CreateInfo(repairData);

                    repairFinishData.ActionCategory = reasonCategory.Category;
                    repairFinishData.ActionReasonCode = reasonCategory.Reason;
                    repairFinishData.ActionReasonSID = reasonCategory.ReasonSID;
                    repairFinishData.Result = rdbOK.Checked ? "OK" : "NG";
                    repairFinishDatas.Add(repairFinishData);

                    var issueOperation = OperationInfo.GetOperationByName(_LotData.OperationName);

                    WIPTransaction.RepairEnd(_LotData, repairFinishDatas, issueOperation, txnStamp);

                    #endregion

                    #region 2017/10/26 跟詩涵確認，無論維修結果如何，一律送回待判站由品保決定處理結果

                    /*
                    #region OK：依照批號的UDC02(工作站序號)+UDC03(工作站名稱)找出預設流程的下一站點，將批號跳站至該站

                    if (rdbOK.Checked)
                    {
                        //取得流程線上版本 取得目前Lot的所有流程(所有工作站)
                        RouteVersionInfo RouteVersion = RouteVersionInfo.GetRouteActiveVersion(_LotData.RouteName);

                        //以此工作站名稱去查詢在流程中的序號
                        var routeOperation = RouteOperationInfo.GetRouteAllOperations(RouteVersion).Find(p => p.OperationName == _LotData.UserDefineColumn03);

                        var lastOperationSeq = string.Format("{0:000}", (Convert.ToDecimal(routeOperation.OperationSequence) + 1));

                        //下一個工作站 用LOT和流程中下一站的序號去查出下一個工作站資訊
                        var NextRouteOperation = RouteOperationInfo.GetRouteOperationByLotSequence(_LotData, lastOperationSeq);

                        if (NextRouteOperation == null)
                        {
                            //批號：{0}已無下個工作站點，請確認[流程設定]
                            throw new Exception(RuleMessage.Error.C10008(_LotData.Lot));
                        }

                        //將批號的UDC02清空
                        WIPTransaction.ModifyLotSystemAttribute(_LotData, "USERDEFINECOL02", "", txnStamp);

                        //將批號的UDC03清空
                        WIPTransaction.ModifyLotSystemAttribute(_LotData, "USERDEFINECOL03", "", txnStamp);

                        //執行出站
                        WIPTransaction.CheckOut(_LotData, txnStamp);

                        WIPTransaction.ReassignOperation(_LotData, NextRouteOperation, reasonCategory, ttbRepairDescr.Text, txnStamp);
                    }
                    #endregion

                    #region NG：送到下一站
                    if (rdbNG.Checked)
                    {
                        //執行出站
                        WIPTransaction.CheckOut(_LotData, txnStamp);

                        WIPTransaction.DispatchLot(_LotData, txnStamp);
                    }
                    #endregion

                    */
                    #endregion

                    //執行出站
                    WIPTransaction.CheckOut(_LotData, txnStamp);

                    WIPTransaction.DispatchLot(_LotData, txnStamp);


                    cts.Complete();
                }

                ClearField();
                AjaxFocus(ttbLot);

                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00614(""));

            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 離開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
    }
}