/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：Keith

功能說明：提供批號執行進站、上機作業
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/07/23      Keith       初版
*/

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.CustomizeInfo.ExtendInfo;
using CustomizeRule.RuleUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.WIPRule
{
    public partial class W001 : CustomizeRuleBasePage
    {
        /// <summary>
        /// LOT資料
        /// </summary>
        private LotInfoEx _LotData
        {
            get { return this["_LotData"] as LotInfoEx; }
            set { this["_LotData"] = value; }
        }

        /// <summary>
        /// 預約單資料
        /// </summary>
        private CSTWIPReserveCheckInInfo _WIPReserveCheckInData
        {
            get { return this["_WIPReserveCheckInData"] as CSTWIPReserveCheckInInfo; }
            set { this["_WIPReserveCheckInData"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            ttbLot.Text = "";
            ttbQty.Text = "";
            ttbDevice.Text = "";
            ttbRoute.Text = "";
            ttbOperation.Text = "";
            ttbDeviceDescr.Text = "";

            _LotData = null;
            _WIPReserveCheckInData = null;

            ddlEquip.Items.Clear();
            ddlEquip.Enabled = false;
            btnOK.Enabled = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
                /* 1.	驗證批號存在性。(批號不存在)
                 * 2.	驗證批號狀態，只能輸入Wait或Run的批號。(狀態為XX，不可執行)
                 *   數量(Label)：帶出批號數量。
                 *   料號(Label)：帶出批號所屬料號。
                 *   料號描述(Label)：帶出料號說明。
                 *   流程(Label)：帶出批號所屬流程。
                 *   工作站(Label)：帶出批號所屬工作站。
                 */

                #region 清除介面資料
                ttbQty.Text = "";
                ttbDevice.Text = "";
                ttbRoute.Text = "";
                ttbOperation.Text = "";
                ttbDeviceDescr.Text = "";

                _LotData = null;
                _WIPReserveCheckInData = null;

                ddlEquip.Items.Clear();
                ddlEquip.Enabled = false;
                btnOK.Enabled = false;
                #endregion

                ttbLot.Must(lblLot);

                _LotData = LotInfoEx.GetLotByLot(ttbLot.Text);

                if (_LotData != null)
                {
                    if (_LotData.Status == LotDefaultStatus.Wait.ToString() || _LotData.Status == LotDefaultStatus.Run.ToString())
                    {
                        // 取得預約工作站資料
                        GetReserveCheckInData();

                        //取得可使用機台清單
                        GetEquipmentList(_LotData.OperationName);

                        //顯示介面欄位內容
                        var device = DeviceInfo.GetDeviceByName(_LotData.DeviceName);
                        ttbQty.Text = _LotData.Quantity.ToString();
                        ttbDevice.Text = _LotData.DeviceName;
                        ttbRoute.Text = _LotData.RouteName;
                        ttbOperation.Text = _LotData.OperationName;
                        ttbDeviceDescr.Text = device.Description;

                        AjaxFocus(ddlEquip);
                    }
                    else
                    {
                        //狀態為{0}，不可執行
                        throw new Exception(RuleMessage.Error.C10003(_LotData.Status));
                    }
                }
                else
                {
                    //[00030]{0}：{1}不存在!
                    throw new Exception(TextMessage.Error.T00030(lblLot.Text, ttbLot.Text));
                }

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
        /// 取得預約工作站資料
        /// </summary>
        private void GetReserveCheckInData()
        {
            //如果批號狀態為Wait，表示第一次進站
            if (_LotData.Status == LotDefaultStatus.Wait.ToString())
            {
                //檢查批號目前規則是否與使用程式名稱一樣
                if (_LotData.CurrentRuleName != ProgramRight)
                {
                    //該批號作業為{0}，不為此功能，請遵循作業規範
                    throw new Exception(RuleMessage.Error.C10004(_LotData.CurrentRuleName));
                }
            }
            else if (_LotData.Status == LotDefaultStatus.Run.ToString())
            {
                var reserveFlag = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIReserve").Find(p=>p.Remark01 == _LotData.Process);

                //如果批號狀態為Run，且有設定允許預約進站，則開始取得預約站點資訊
                if (reserveFlag != null)
                {
                    //取得預約工作站的序號
                    var nextOperationSequence = CustomizeFunction.GetAppointmentOperationSequence(_LotData.Lot, ProgramRight);

                    //取得預約工作站名稱
                    var operationName = CustomizeFunction.GetAppointmentOperationName(_LotData, nextOperationSequence);

                    //取得預約工作站的第一個RuleName
                    var firstRuleName = CustomizeFunction.GetFirstOperationTypeRuleByOperationName(_LotData, operationName, ProgramRight);

                    //新增一筆預約資料
                    _WIPReserveCheckInData = InfoCenter.Create<CSTWIPReserveCheckInInfo>();
                    _WIPReserveCheckInData.Lot = _LotData.Lot;
                    _WIPReserveCheckInData.OperationName = operationName;
                    _WIPReserveCheckInData.RuleName = firstRuleName;
                } 
                else
                {
                    throw new CimesException(TextMessage.Error.T00356(_LotData.Lot));
                }
            }
        }

        /// <summary>
        /// 取得可使用機台清單
        /// </summary>
        /// <param name="lot"></param>
        public void GetEquipmentList(string operationName)
        {
            //清除機台清單
            ddlEquip.Items.Clear();
            ddlEquip.Enabled = false;

            //取得可用的所有機台
            var equipmentList = CustomizeFunction.GetEquipmentListByOperationName(operationName);

            //如果取得機台資料大於零，則將機台清單傳至下拉式元件
            if (equipmentList.Count > 0)
            {
                ddlEquip.Enabled = true;
                ddlEquip.DataSource = equipmentList;
                ddlEquip.DataTextField = "EquipmentName";
                ddlEquip.DataValueField = "EquipmentSID";
                ddlEquip.DataBind();

                if (ddlEquip.Items.Count != 1)
                    ddlEquip.Items.Insert(0, "");
                else
                {
                    ddlEquip.SelectedIndex = 0;
                }
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
                /*  1.	檢查畫面上所有輸入欄位是否都已輸入。
                 *  2.	如為當站進站(批號狀態為Wait)，則將批號進站併上機台。(EquipmentAddLot、CheckIn、Dispatch)。
                 *  3.	如為預約進站，則將預約資訊塞入客製表，做批號備註。(AddComment)
                 *  4.	清空畫面，游標停在機加批號。
                 */
                string equipmentName = "";
                EquipmentInfo equipData = null;
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                //檢查機台是否有輸入
                if (ttbLot.Text.IsNullOrEmpty() || _LotData == null)
                {
                    AjaxFocus(ttbLot);
                    throw new Exception(TextMessage.Error.T00030(GetUIResource("LotCheckInLot"), ""));
                }

                //如果機台下拉式清單Enabled為TRUE，則必須檢查是否有選擇機台
                if (ddlEquip.Enabled)
                {
                    ddlEquip.Must(lblEquip);

                    //取得機台資訊
                    equipData = EquipmentInfo.GetEquipmentByID(ddlEquip.SelectedValue);

                    //註記機台名稱
                    equipmentName = equipData.EquipmentName;
                }

                using (var cts = CimesTransactionScope.Create())
                {
                    //批號狀態為Wait時，表示目前要執行進站
                    if (_LotData.Status == LotDefaultStatus.Wait.ToString())
                    {
                        CheckBom(_LotData.OperationName, equipData, _LotData);

                        if (equipData != null)
                        {
                            //批號上機台
                            EMSTxn.Default.AddLotToEquipment(_LotData, equipData, txnStamp);
                            
                            //取得機台狀態資料
                            var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("RUN");

                            if (equipData.CurrentState != "RUN")
                            {
                                //更新機台狀態
                                EMSTransaction.ChangeState(equipData, newStateInfo, txnStamp);
                            }
                        }

                        //批號進站
                        WIPTransaction.CheckIn(_LotData, equipmentName, "", "", LotDefaultStatus.Run.ToString(), txnStamp);

                        //將批號Dispatch到下一規則
                        WIPTransaction.DispatchLot(_LotData, txnStamp);
                    }
                    else
                    {
                        CheckBom(_WIPReserveCheckInData.OperationName, equipData, _LotData);

                        //執行預約進站功能
                        _WIPReserveCheckInData.Equipment = equipmentName;
                        _WIPReserveCheckInData.InTime = DBCenter.GetSystemTime();
                        _WIPReserveCheckInData.InsertToDB();

                        //做批號備註。(AddComment)
                        var reason = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "ReserveLotCheckIn");
                        var massage = "";

                        WIPTransaction.AddLotComment(_LotData, reason, massage, txnStamp);
                    }
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

        /// <summary>
        /// 確認預約工作站是否有開啟物料檢查功能
        /// </summary>
        private void CheckBom(string opeartoinName, EquipmentInfo equipData, LotInfo lotData)
        {
            //確認工作站是否有開啟物料檢查功能，如果機台資料為NULL，則直接跳過這個檢查機制
            var operationData = OperationInfo.GetOperationByName(opeartoinName).ChangeTo<OperationInfoEx>();
            if (operationData.CheckBOM == "Y" && equipData != null)
            {
                #region 先比對BOM表的料是否都有上機
                //取得客製表[CST_WPC_BOM]工單對應華司料(SORTF = 2)的資料
                var WOBomList = CSTWPCWorkOrderBOMInfo.GetDataByWorkOrderAndSORTF(_LotData.WorkOrder, "2");

                //取得目前選定的機台上所有的物料資料
                var eqpMLotList = EquipmentMaterialLotInfo.GetEquipmentMaterialLotByEquipment(equipData.EquipmentName);

                //確認目前選定的機台是否已經有上物料
                WOBomList.ForEach(WOBom =>
                {
                    bool isFind = false;

                    foreach (var eqpMLot in eqpMLotList)
                    {
                        //取得料編號資料
                        var materialLotData = MaterialLotInfo.GetMaterialLotByMaterialLot(eqpMLot.MaterialLot);
                        if (WOBom.MATNR == materialLotData.MaterialNO)
                        {
                            //註記有找到對應的物料編號
                            isFind = true;
                            break;
                        }
                    }

                    if (isFind == false)
                    {
                        //機台:{0} 沒有上物料編號{1}的資料！
                        throw new Exception(RuleMessage.Error.C10145(equipData.EquipmentName, WOBom.MATNR));
                    }

                });

                #endregion

                #region 依照料號的孔位設定，確認孔位都有華司資料
                var deviceVersion = DeviceVersionInfo.GetLotCurrentDeviceVersion(lotData).ChangeTo<DeviceVersionInfoEx>();

                if (deviceVersion.PushLocation.ToString().IsNullOrEmpty())
                {
                    //[00031]{0}：{1}的系統屬性：{2} 未設定，請洽IT人員！
                    throw new CimesException(TextMessage.Error.T00031(GetUIResource("Device"), deviceVersion.DeviceName, "PUSH_LOCATION"));                   
                }
                var lstPushLocation = deviceVersion["PUSH_LOCATION"].ToString().Split(',');

                //先取得機台上的物料批資訊
                List<MaterialLotInfo> eqpMaterialLotList = new List<MaterialLotInfo>();
                eqpMLotList.ForEach(p =>
                {
                    var mLot = MaterialLotInfo.GetMaterialLotByMaterialLot(p.MaterialLot);
                    eqpMaterialLotList.Add(mLot);
                });

                for (int i=0; i < lstPushLocation.Length; i++)
                {
                    var mlotData = eqpMaterialLotList.Find(p => p.Location == lstPushLocation[i]);
                    if(mlotData == null)
                    {
                        throw new CimesException(RuleMessage.Error.C00041(equipData.EquipmentName, lstPushLocation[i]));
                    }
                }
                #endregion
            }
        }
    }
}