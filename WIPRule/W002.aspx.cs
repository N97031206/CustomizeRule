/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：Keith

功能說明：提供批號執行出站、下機、紀錄不良作業。
		提供不良品送待判站待檢。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/07/23      Keith       初版

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.CustomizeInfo.ExtendInfo;
using CustomizeRule.RuleUtility;
using System.Data;

namespace CustomizeRule.WIPRule
{
    public partial class W002 : CustomizeRuleBasePage
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
        /// 選擇的Componet資料
        /// </summary>
        private ComponentInfo _SelectedComponetData
        {
            get { return this["_SelectedComponetData"] as ComponentInfo; }
            set { this["_SelectedComponetData"] = value; }
        }

        /// <summary>
        /// 料號資料
        /// </summary>
        private DeviceVersionInfoEx _DeviceData
        {
            get { return this["_DeviceData"] as DeviceVersionInfoEx; }
            set { this["_DeviceData"] = value; }
        }

        [Serializable]
        public class DefectGridData
        {
            /// <summary>
            /// 子單元名稱
            /// </summary>
            public string ComponentID { get; set; }

            /// <summary>
            /// 批號
            /// </summary>
            public string Lot { get; set; }

            /// <summary>
            /// 原因碼
            /// </summary>
            public string DefectReason { get; set; }

            /// <summary>
            /// 原因碼SID
            /// </summary>
            public string DefectID { get; set; }

            /// <summary>
            /// 原因說明
            /// </summary>
            public string DefectDesc { get; set; }

            /// <summary>
            /// 子單元SID
            /// </summary>
            public string WIPComponentSID { get; set; }

            /// <summary>
            /// 子單元數量
            /// </summary>
            public int ComponentQty { get; set; }
        }

        /// <summary>
        /// 介面清單顯示資料
        /// </summary>
        private List<DefectGridData> _DefectGridData
        {
            get { return this["_DefectGridData"] as List<DefectGridData>; }
            set { this["_DefectGridData"] = value; }
        }

        /// <summary>
        /// 顯示畫面的資料表
        /// </summary>
        private DataTable _DispatchReportTable
        {
            get { return this["_DispatchReportTable"] as DataTable; }
            set { this["_DispatchReportTable"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
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
                    gvDefect.Initialize();

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

        private void ClearField()
        {
            //初始化
            ttbLot.Text = "";
            ttbEquip.Text = "";
            ttbQty.Text = "";
            ttbDevice.Text = "";
            ttbRoute.Text = "";
            ttbOperation.Text = "";
            ttbComponentId.Text = "";
            ttbDefectDesc.Text = "";
            ttbDefectQty.Text = "";
            ttbDeviceDescr.Text = "";

            ddlComponentId.Items.Clear();
            ddlDefectReason.Items.Clear();

            _LotData = null;
            _SelectedComponetData = null;
            _DeviceData = null;

            _DispatchReportTable = new DataTable();

            _DefectGridData = new List<DefectGridData>();

            btnOK.Enabled = false;
            ddlComponentId.Enabled = true;
            ttbComponentId.Enabled = true;

            gvDefect.SetDataSource(_DispatchReportTable, true);
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

                ttbLot.Must(lblLot);

                #region 清除介面資料
                ttbEquip.Text = "";
                ttbQty.Text = "";
                ttbDevice.Text = "";
                ttbRoute.Text = "";
                ttbOperation.Text = "";
                ttbComponentId.Text = "";
                ttbDefectDesc.Text = "";
                ttbDefectQty.Text = "";
                ttbDeviceDescr.Text = "";

                ddlComponentId.Items.Clear();
                ddlDefectReason.Items.Clear();
                ddlComponentId.Enabled = true;
                ttbComponentId.Enabled = true;

                _LotData = null;
                _SelectedComponetData = null;
                _DeviceData = null;

                _DispatchReportTable = new DataTable();

                _DefectGridData = new List<DefectGridData>();

                btnOK.Enabled = false;

                gvDefect.SetDataSource(_DispatchReportTable, true);
                #endregion

                _LotData = LotInfoEx.GetLotByLot(ttbLot.Text).ChangeTo<LotInfoEx>();

                //驗證批號存在性。(批號不存在)
                if (_LotData != null)
                {
                    //驗證批號狀態，Run的批號。(狀態為XX，不可執行)
                    if (_LotData.Status == LotDefaultStatus.Run.ToString())
                    {
                        //驗證currentRule。
                        if (_LotData.CurrentRuleName != _ProgramInformationBlock.ProgramRight)
                        {
                            //該批號作業為{0}，不為此功能，請遵循作業規範
                            throw new Exception(RuleMessage.Error.C10004(_LotData.CurrentRuleName));
                        }

                        var operData = OperationInfo.GetOperationByName(_LotData.OperationName).ChangeTo<OperationInfoEx>();
                        if(operData.GetDMC.ToBool())
                        {
                            var compList = ComponentInfo.GetLotAllComponents(_LotData.Lot).ChangeTo<ComponentInfoEx>();
                            foreach(var compData in compList)
                            {
                                if (compData.DMC.IsNullOrTrimEmpty())
                                    throw new CimesException(RuleMessage.Error.C00040(compData.ComponentID));
                            }
                        }

                        _DeviceData = DeviceVersionInfoEx.GetDataByDeviceName(_LotData.DeviceName);

                        ttbQty.Text = _LotData.Quantity.ToString();
                        ttbDevice.Text = _LotData.DeviceName;
                        ttbEquip.Text = _LotData.ResourceName;
                        ttbDeviceDescr.Text = _DeviceData.Description;
                        ttbRoute.Text = _LotData.RouteName;
                        ttbOperation.Text = _LotData.OperationName;

                        InitialDropDownList();

                        if (_DeviceData.ProdType == CustomizeFunction.ProdType.S.ToCimesString())
                        {
                            ttbDefectQty.Enabled = false;
                            ttbDefectQty.Text = "1";
                        }
                        else if (_DeviceData.ProdType == CustomizeFunction.ProdType.G.ToCimesString() || 
                                 _DeviceData.ProdType == CustomizeFunction.ProdType.W.ToCimesString() ||
                                 _DeviceData.ProdType == CustomizeFunction.ProdType.B.ToCimesString())
                        {
                            ttbDefectQty.Enabled = true;
                            ttbDefectQty.Text = "1";

                            ddlComponentId.Enabled = false;
                            ddlComponentId.SelectedIndex = -1;
                            ttbComponentId.Enabled = false;
                        }
                        else
                        {
                            //[00031]{0}：{1}的系統屬性：{2} 未設定，請洽IT人員！
                            throw new Exception(TextMessage.Error.T00031(lblDevice.Text, _DeviceData.DeviceName, _DeviceData.ProdType));
                        }
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
        /// 初始化下拉式選項(不良原因碼及工件序號)
        /// </summary>
        void InitialDropDownList()
        {
            ddlDefectReason.Items.Clear();
            List<BusinessReason> reason = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(_LotData.CurrentRuleName, _LotData.OperationName, "Default", ReasonMode.Category);
            if (reason.Count > 0)
            {
                ddlDefectReason.DataSource = reason;
                ddlDefectReason.DataTextField = "ReasonDescription";
                ddlDefectReason.DataValueField = "ReasonCategorySID";
                ddlDefectReason.DataBind();

                if (ddlDefectReason.Items.Count != 1)
                    ddlDefectReason.Items.Insert(0, "");
                else
                {
                    ddlDefectReason.SelectedIndex = 0;
                }
            }
            else
            {
                //[00641]規則:{0} 工作站:{1} 使用的原因碼未設定，請洽IT人員!
                throw new Exception(TextMessage.Error.T00641(_LotData.CurrentRuleName, _LotData.OperationName));
            }

            ddlComponentId.Items.Clear();
            List<ComponentInfo> components = ComponentInfo.GetLotAllComponents(_LotData);
            if (components.Count > 0)
            {
                ddlComponentId.DataSource = components;
                ddlComponentId.DataTextField = "COMPONENTID";
                ddlComponentId.DataValueField = "WIPComponentSID";
                ddlComponentId.DataBind();

                if (ddlComponentId.Items.Count != 1)
                    ddlComponentId.Items.Insert(0, "");
                else
                {
                    ddlComponentId.SelectedIndex = 0;
                    ddlComponentId_SelectedIndexChanged(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 子單元切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlComponentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlComponentId.SelectedIndex >= 0)
                {
                    ttbComponentId.Text = ddlComponentId.SelectedItem.Text;
                    ttbComponentId_TextChanged(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 輸入不良說明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbComponentId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _SelectedComponetData = null;

                if (ttbComponentId.Text.IsNullOrEmpty())
                {
                    throw new Exception(TextMessage.Error.T00826(lblComponentId.Text));
                }

                //驗證component合法性。
                //找不到component。(序號：xxx不存在。)
                //輸入的component不屬於這個批號。(序號：xxx所屬批號為xxx，請確認資料正確性!!)
                var component = ComponentInfoEx.GetComponentByComponentID(ttbComponentId.Text);
                if (component != null)
                {
                    if (_LotData.Lot != component.CurrentLot)
                    {
                        //序號：{0}所屬批號為{1}，請確認資料正確性!
                        throw new Exception(RuleMessage.Error.C10010(component.ComponentID, component.CurrentLot));
                    }
                    _SelectedComponetData = component;
                }
                else
                {
                    //序號：{0}不存在
                    throw new Exception(RuleMessage.Error.C10009(ttbComponentId.Text));
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //檢查不良原因是否有選擇
                ddlDefectReason.Must(lblDefectReason);

                if (_DeviceData.ProdType == CustomizeFunction.ProdType.S.ToCimesString())
                {
                    //確認生產編號是否有輸入(TextBox)。(請輸入生產編號!!)
                    if (!ttbComponentId.Text.IsNullOrEmpty())
                    {
                        //檢查新增的序號是否存在於不良清單裡。(序號：xxx已存在於不良清單裡，請確認資料正確性!!!)
                        foreach (DefectGridData data in _DefectGridData)
                        {
                            if (data.ComponentID == ttbComponentId.Text)
                            {
                                //序號：{0}已存在於不良清單裡，請確認資料正確性!
                                throw new Exception(RuleMessage.Error.C10011(ttbComponentId.Text));
                            }
                        }
                    }
                    else
                    {
                        //請輸入生產編號!!
                        throw new Exception(RuleMessage.Error.C10012());
                    }

                    //檢查完成
                    DefectGridData rowData = new DefectGridData();
                    rowData.ComponentID = ttbComponentId.Text;
                    rowData.Lot = _LotData.Lot;
                    rowData.DefectReason = ddlDefectReason.SelectedItem.Text;
                    rowData.DefectID = ddlDefectReason.SelectedItem.Value;
                    rowData.DefectDesc = ttbDefectDesc.Text;
                    rowData.WIPComponentSID = _SelectedComponetData.WIPComponentSID;
                    rowData.ComponentQty = _SelectedComponetData.ComponentQuantity.ToInt();

                    _DefectGridData.Add(rowData);

                }
                else if (_DeviceData.ProdType == CustomizeFunction.ProdType.G.ToCimesString() || 
                         _DeviceData.ProdType == CustomizeFunction.ProdType.W.ToCimesString() ||
                         _DeviceData.ProdType == CustomizeFunction.ProdType.B.ToCimesString())
                {
                    //驗證不良數量數字合法性。(生產數量輸入的資料格式錯誤，請輸入正整數！)
                    if (!ttbDefectQty.Text.IsNullOrEmpty())
                    {
                        int qty;
                        if (int.TryParse(ttbDefectQty.Text, out qty))
                        {
                            //輸入的數量不可為0。(不良數量不可為0)
                            if (qty > 0)
                            {
                                //輸入的數量 + 不良清單數量，不可大於批號數量。(不良數量不可大於批號數量!)
                                if (qty + _DefectGridData.Count() > _LotData.Quantity)
                                {
                                    //不良數量不可大於批號數量!
                                    throw new Exception(RuleMessage.Error.C10015());
                                }

                                //依照輸入的數量，逆排未指定的component，再依數量將component加入不良清單
                                List<ComponentInfo> components = ComponentInfo.GetLotAllComponents(_LotData);
                                components.Reverse();
                                int component_index = 0;
                                while (qty > 0)
                                {
                                    DefectGridData rowData = new DefectGridData();
                                    DefectGridData check_com;
                                    do
                                    {
                                        check_com = _DefectGridData.Find(p => p.ComponentID == components[component_index].ComponentID);
                                        if (check_com != null)
                                        {
                                            component_index++;
                                        }
                                    } while (check_com != null);


                                    rowData.ComponentID = components[component_index].ComponentID;
                                    rowData.Lot = _LotData.Lot;
                                    rowData.DefectReason = ddlDefectReason.SelectedItem.Text;
                                    rowData.DefectID = ddlDefectReason.SelectedItem.Value;
                                    rowData.DefectDesc = ttbDefectDesc.Text;
                                    rowData.WIPComponentSID = components[component_index].WIPComponentSID;
                                    rowData.ComponentQty = components[component_index].ComponentQuantity.ToInt();

                                    _DefectGridData.Add(rowData);

                                    qty -= components[component_index].ComponentQuantity.ToInt();
                                }

                            }
                            else
                            {
                                //不良數量不可為0
                                throw new Exception(RuleMessage.Error.C10013());
                            }
                        }
                        else
                        {
                            //[00012][{0}]輸入的資料格式錯誤，請輸入整數！
                            throw new Exception(TextMessage.Error.T00012(ttbDefectQty.Text));
                        }
                    }
                }

                //清除資料
                _DispatchReportTable = new DataTable();

                _DispatchReportTable.Columns.Add("ComponentID");
                _DispatchReportTable.Columns.Add("Lot");
                _DispatchReportTable.Columns.Add("DefectReason");
                _DispatchReportTable.Columns.Add("DefectDesc");
                _DispatchReportTable.Columns.Add("WIPComponentSID");

                _DefectGridData.ForEach(dispatch =>
                {
                    DataRow dr = _DispatchReportTable.NewRow();

                    dr["ComponentID"] = dispatch.ComponentID;
                    dr["Lot"] = dispatch.Lot;
                    dr["DefectReason"] = dispatch.DefectReason;
                    dr["DefectDesc"] = dispatch.DefectDesc;
                    dr["WIPComponentSID"] = dispatch;

                    _DispatchReportTable.Rows.Add(dr);
                });

                gvDefect.SetDataSource(_DispatchReportTable, true);
            }
            catch (Exception ex)
            {
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
                /*  1.	如有不良數量，先確認有待判站。
                2.	待判站找尋方式：到系統資料設定(MES_WPC_EXTENDITEM)裡找尋屬性SAIJudgeOperation，依照process找出待判站的名稱。如找不到要報錯。(找不到待判站資訊，請至系統資料維護增加資訊，屬性：xxxx。)
                    拿MES_WIP_LOT_PROCESS 跟系統資料設定裡的REMARK01比對後，找出工作站點(REMARK02)
                3.	將批號出站。(CheckOut)
                4.	將批號下機。(EQPRemoveLot)
                5.	判定機台容量如果為0，將機台狀態變更為IDLE。(ChangeStaus)
                    機台容量判定方式：MES_EQP_EQP. CAPACITY = 0。
                6.	將CST_WIP_RESERVE_CHECKIN_INFO搬移至CST_WIP_RESERVE_CHECKIN_INFO_LOG。
                    資料條件：Lot + Operation
                7.	如有不良數量，拆一個不良批號，將批號送到待判站。(Defect、SplitLot、ReassignOperation)
                8.	將原批號派送至下一規則。(Dispatch)
                */

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    //取得批號資料
                    var txnLotData = _LotData;

                    //取得批號上機台資料
                    var equipmentLotData = EquipmentLotInfo.GetEquipmentLotByLot(txnLotData.Lot);

                    //如果批號上機台是有資料，表示必須執行機台相關的交易
                    if (equipmentLotData != null)
                    {
                        var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("IDLE");

                        var equipment = EquipmentInfo.GetEquipmentByName(txnLotData.ResourceName);
                        if (equipment == null)
                        {
                            //[00885]機台{0}不存在！
                            throw new Exception(TextMessage.Error.T00885(txnLotData.ResourceName));
                        }

                        //機台移批號
                        EMSTransaction.RemoveLotFromEquipment(txnLotData, equipment, txnStamp);

                        //確認機台的最大容量是否為零，如果不是，則執行變更機台狀態
                        if (equipment.Capacity == 0)
                        {
                            //變更機台狀態 Run=>IDLE
                            EMSTransaction.ChangeState(equipment, newStateInfo, txnStamp);
                        }
                    }

                    //更新預約資料
                    UpdateReserveCheckInData(txnLotData, txnLotData.OperationName, txnStamp);

                    //批號出站
                    WIPTransaction.CheckOut(txnLotData, txnStamp);

                    //如果有不良數量，則拆號及送待判工作站
                    if (_DefectGridData != null && _DefectGridData.Count > 0)
                    {
                        SplitDefectLotList(txnLotData, _DefectGridData, txnStamp);
                    }

                    //將批號Dispatch到下一規則
                    WIPTransaction.DispatchLot(txnLotData, txnStamp);

                    //檢查是否還有預約工作站資料
                    CheckNextReserveCheckIn(txnLotData, txnStamp);

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
        /// 取得Naming
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="user"></param>
        /// <param name="info"></param>
        /// <param name="lstArgs"></param>
        /// <returns></returns>
        public Pair<string[], List<SqlAgent>> GetNamingRule(string ruleName, string user, InfoBase info = null, List<string> lstArgs = null)
        {
            var naming = NamingIDGenerator.GetRule(ruleName);
            if (naming == null)
            {
                throw new Exception(TextMessage.Error.T00437(ruleName));   //[00437]找不到可產生的序號，請至命名規則維護設定，規則名稱：{0}!!!
            }

            if (lstArgs == null)
            {
                lstArgs = new List<string>();
            }

            if (info != null)
            {
                var result = naming.GenerateNextIDs(1, info, lstArgs.ToArray(), user);
                return new Pair<string[], List<SqlAgent>>(result.First, result.Second);
            }
            else
            {
                var result = naming.GenerateNextIDs(1, lstArgs.ToArray(), user);
                return new Pair<string[], List<SqlAgent>>(result.First, result.Second);
            }
        }

        /// <summary>
        /// 依據傳入批號資料及工作站來更新預約資料
        /// </summary>
        /// <param name="txnLotData"></param>
        /// <param name="operationName"></param>
        /// <param name="txnStamp"></param>
        public void UpdateReserveCheckInData(LotInfoEx txnLotData, string operationName, TransactionStamp txnStamp)
        {
            //取得預約資料
            var WIPReserveList = CSTWIPReserveCheckInInfo.GetDataByLotAndOper(txnLotData.Lot, operationName);
            WIPReserveList.ForEach(reserveData =>
            {
                //記錄出站時間
                reserveData.OutTime = DBCenter.GetSystemTime();

                //記錄LOG
                CSTWIPReserveCheckInLogInfo reserveDataLog = InfoCenter.Create<CSTWIPReserveCheckInLogInfo>();
                reserveDataLog = reserveData.Fill<CSTWIPReserveCheckInLogInfo>();
                reserveDataLog.LinkSID = txnStamp.LinkSID;

                reserveData.DeleteFromDB();
                reserveDataLog.InsertToDB();
            });
        }

        /// <summary>
        /// 確認出站後是否有下一站的預約紀錄，如果有，則將批號進站至下一個工作站
        /// </summary>
        /// <param name="txnLotData">機加批號</param>
        /// <param name="txnStamp"></param>
        public void CheckNextReserveCheckIn(LotInfoEx txnLotData, TransactionStamp txnStamp)
        {
            //取得預約工作站資料
            var WIPNextReserveList = CSTWIPReserveCheckInInfo.GetDataByLot(txnLotData.Lot);
            if (WIPNextReserveList.Count > 0)
            {
                //下一個預約工作站資料
                var reserveData = WIPNextReserveList[0];

                //將預約的進站時間回寫到hist的Remark01
                txnStamp.Remark01 = reserveData.InTime;

                //取得機台資料
                var equipData = EquipmentInfo.GetEquipmentByName(reserveData.Equipment);
                if (equipData == null)
                {
                    //[00885]機台{0}不存在！
                    throw new Exception(TextMessage.Error.T00885(reserveData.Equipment));
                }

                //取得機台狀態資料
                var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("RUN");

                if (equipData.CurrentState != "RUN")
                {
                    //更新機台狀態
                    EMSTransaction.ChangeState(equipData, newStateInfo, txnStamp);
                }

                //批號上機台
                EMSTxn.Default.AddLotToEquipment(txnLotData, equipData, txnStamp);

                //批號進站
                WIPTransaction.CheckIn(txnLotData, equipData.EquipmentName, "", "", LotDefaultStatus.Run.ToString(), txnStamp);

                //將批號Dispatch到下一規則
                WIPTransaction.DispatchLot(txnLotData, txnStamp);
            }
        }

        /// <summary>
        /// 鍛造出站有不良數量直接拆批及送待判工作站
        /// </summary>
        /// <param name="txnLotData">機加批號</param>
        /// <param name="defectGridDataList">不良清單</param>
        /// <param name="txnStamp"></param>
        public void SplitDefectLotList(LotInfoEx txnLotData, List<DefectGridData> defectGridDataList, TransactionStamp txnStamp)
        {
            //待判工作站點名稱
            string judgeOperationName = "";

            //確認是否有不良清單，如果有不良清單的話，則要取得待判工作站資料
            if (defectGridDataList.Count > 0)
            {
                //在系統資料維護裡，取得此批號對應製程(CPC/CPF)的待判工作站名稱
                List<WpcExClassItemInfo> operationList = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIJudgeOperation");
                WpcExClassItemInfo judgeOperationData = operationList.Find(p => p.Remark01 == txnLotData.Process);
                if (judgeOperationData == null)
                {
                    //找不到待判站資訊，請至系統資料維護增加資訊，屬性：{0}
                    throw new Exception(RuleMessage.Error.C10014(txnLotData.Process));
                }

                //取得待判工作站名稱
                judgeOperationName = judgeOperationData.Remark02;
            }

            //處理不良批號
            defectGridDataList.ForEach(defectGridData =>
            {
                //取得原因碼資訊
                var reasonData = InfoCenter.GetBySID<ReasonCategoryInfo>(defectGridData.DefectID);

                //取得批號子單元資訊
                var component = ComponentInfo.GetComponentByComponentID(defectGridData.ComponentID);

                //取得不良子批批號名稱
                var splitLotNaming = GetNamingRule("SplitLot", txnStamp.UserID, txnLotData);

                //批號拆子批
                var splitLot = SplitLotInfo.CreateSplitLotByLotAndQuantity(txnLotData.Lot, splitLotNaming.First[0], new List<ComponentInfo>() { component }, reasonData, reasonData.Description);
                WIPTxn.Default.SplitLot(txnLotData, splitLot, WIPTxn.SplitIndicator.Create(null, null, null, TerminateBehavior.NoTerminate), txnStamp);

                if (splitLotNaming.Second != null && splitLotNaming.Second.Count != 0)
                {
                    DBCenter.ExecuteSQL(splitLotNaming.Second);
                }

                //註記不良
                var compDefect = ComponentDefectObject.Create(component, component.ComponentQuantity, 0, reasonData, defectGridData.DefectDesc);
                WIPTransaction.DefectComponent(splitLot, new List<ComponentDefectObject>() { compDefect }, WIPTransaction.DefectIndicator.Create(), txnStamp);

                #region 送至待判工作站

                //取得目前批號的流程線上版本
                RouteVersionInfo RouteVersion = RouteVersionInfo.GetRouteActiveVersion(txnLotData.RouteName);

                //以目前工作站名稱去查詢在所有流程中的序號
                var routeOperation = RouteOperationInfo.GetRouteAllOperations(RouteVersion).Find(p => p.OperationName == judgeOperationName);

                //以目前工作站名稱去查詢在所有流程中的序號
                var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");

                ////將批號的UDC01註記不良批
                //WIPTransaction.ModifyLotSystemAttribute(splitLot, "USERDEFINECOL01", "Y", txnStamp);

                ////將批號的UDC02註記工作站序號
                //WIPTransaction.ModifyLotSystemAttribute(splitLot, "USERDEFINECOL02", operationSequence, txnStamp);

                ////將批號的UDC03註記工作站名稱
                //WIPTransaction.ModifyLotSystemAttribute(splitLot, "USERDEFINECOL03", operationName, txnStamp);

                var modifyAttrList = new List<ModifyLotAttributeInfo>();

                //將批號的UDC01註記不良批
                modifyAttrList.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL01", "Y"));

                //將批號的UDC02註記工作站序號
                modifyAttrList.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL02", splitLot.OperationSequence));

                //將批號的UDC03註記工作站名稱
                modifyAttrList.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL03", splitLot.OperationName));

                WIPTransaction.ModifyLotMultipleAttribute(splitLot, modifyAttrList, txnStamp);

                WIPTransaction.ReassignOperation(splitLot, routeOperation, reasonCategory, reasonCategory.Description, txnStamp);

                #endregion
            });
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDefect_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = gvDefect.Rows[e.RowIndex].DataItemIndex;

                var removeData = _DefectGridData[index];

                _DefectGridData.Remove(removeData);

                //清除資料
                _DispatchReportTable = new DataTable();

                _DispatchReportTable.Columns.Add("ComponentID");
                _DispatchReportTable.Columns.Add("Lot");
                _DispatchReportTable.Columns.Add("DefectReason");
                _DispatchReportTable.Columns.Add("DefectDesc");
                _DispatchReportTable.Columns.Add("WIPComponentSID");

                _DefectGridData.ForEach(dispatch =>
                {
                    DataRow dr = _DispatchReportTable.NewRow();

                    dr["ComponentID"] = dispatch.ComponentID;
                    dr["Lot"] = dispatch.Lot;
                    dr["DefectReason"] = dispatch.DefectReason;
                    dr["DefectDesc"] = dispatch.DefectDesc;
                    dr["WIPComponentSID"] = dispatch;

                    _DispatchReportTable.Rows.Add(dr);
                });

                gvDefect.SetDataSource(_DispatchReportTable, true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}