/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：Nick

功能說明：提供批號進行拆包作業，還原批號與批號對應的子單元。

------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/11/21      Nick       初版
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ciMes.Web.Common;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.CustomizeInfo.ExtendInfo;
using CustomizeRule.RuleUtility;

namespace CustomizeRule.WIPRule
{
    public partial class W043 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 批號全域變數
        /// </summary>
        private LotNonActiveInfo _ProcessLot
        {
            get
            {
                return (LotNonActiveInfo)this["_ProcessLot"];
            }
            set
            {
                this["_ProcessLot"] = value;
            }
        }

        /// <summary>
        /// 子單元清單全域變數 
        /// </summary>
        private List<ComponentNonactiveInfoEx> _ComponentList
        {
            get
            {
                return (List<ComponentNonactiveInfoEx>)this["_ComponentList"];
            }
            set
            {
                this["_ComponentList"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ClearField();

                    // 檢查權限
                    if (!UserProfileInfo.CheckUserRight(User.Identity.Name, ProgramRight))
                    {
                        HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 清除資料與使用者介面
        /// </summary>
        private void ClearField()
        {
            ClearData();
            ClearUI();
        }

        /// <summary>
        /// 清除使用者介面
        /// </summary>
        private void ClearUI()
        {
            gvComponent.SetDataSource(null, true);
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearData()
        {
            _ProcessLot = null;
            _ComponentList = null;
        }

        protected void ttbBoxNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // 清除資料與使用者介面
                ClearField();

                var convertInfo = WIPConvertInfoEx.GetWIPConvertInfoByTargetLot(ttbBoxNo.Text.Trim());
                if (convertInfo == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00030(lblUnPackingInventoryLot.Text, ttbBoxNo.Text.Trim()), ttbBoxNo);
                }

                // 取得批號
                _ProcessLot = LotNonActiveInfo.GetLotNonActiveByLot(ttbBoxNo.Text.Trim());
                
                // 若批號不存在拋錯
                if (_ProcessLot == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(lblUnPackingInventoryLot.Text), ttbBoxNo);
                }

                if (_ProcessLot.Status != "Finished")
                {
                    // [01203]批號狀態不正確, 應為 {0} !
                    throw new RuleCimesException(TextMessage.Error.T01203("Finished"), ttbBoxNo);
                }

                if (_ProcessLot.OperationName != "入庫")
                {
                    //[00361]批號({ 0})所在工作站({ 1}) 與({ 2}) 不同 !
                    throw new RuleCimesException(TextMessage.Error.T00361(_ProcessLot.Lot, _ProcessLot.OperationName, "入庫"), ttbBoxNo);
                }
                
                _ComponentList = ComponentNonactiveInfoEx.GetDataByInvBoxNo(_ProcessLot.Lot);

                // 資料繫結
                gvComponent.SetDataSource(_ComponentList, true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ProcessLot == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00826(lblUnPackingInventoryLot.Text));
                }

                // 定義交易戳記
                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    // 將入庫批取消完工
                    WIPTransaction.UndoFinish(_ProcessLot, txnStamp);
                    var unPackingLot = LotInfo.GetLotByLot(ttbBoxNo.Text.Trim());
                    // 取得拆包Component
                    var lstUnPackingComp = unPackingLot.GetLotAllComponents();
                    // 將轉為子單元的批號還原，例如可使用於拆包
                    var txn = WIPTxn.Default.ConvertToLot(unPackingLot, lstUnPackingComp, txnStamp);
                    txn.LotList.ForEach(lot => {
                        //寫入WMSMaster & Detail
                        var wmsMaster = CSTWMSMastInfo.GetMaterialLotDataByMaterialLot(lot["INVLOT"].ToString());
                        if (wmsMaster == null)
                        {
                            wmsMaster = InfoCenter.Create<CSTWMSMastInfo>();
                            wmsMaster.Lot = lot["INVLOT"].ToString();
                            wmsMaster.DeviceName = lot.DeviceName;
                            wmsMaster.RuleName = txnStamp.RuleName;
                            wmsMaster.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                        }
                        //理論上只會有一個Comp
                        lot.GetLotAllComponents().ForEach(comp => {
                            var wmsDetail = InfoCenter.Create<CSTWMSDetailInfo>();
                            wmsDetail.Lot = lot["INVLOT"].ToString();
                            wmsDetail.ComponentID = comp.ComponentID;
                            wmsDetail.Quantity = comp.ComponentQuantity;
                            wmsDetail.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                        });
                        WIPTransaction.TerminateLot(lot, txnStamp);
                    });                    

                    cts.Complete();
                }
                // 返回定義的預設網頁
                ReturnToPortal();
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
                // 返回定義的預設網頁
                ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}