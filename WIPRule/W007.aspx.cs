/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：Nick

功能說明：提供批號進行中心孔量測作業。

------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/07/23      Nick       初版
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

using ciMes.Web.Common;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using CustomizeRule.RuleUtility;
using Ares.Cimes.IntelliService.DbTableSchema;

namespace CustomizeRule.WIPRule
{
    public partial class W007 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 批號全域變數
        /// </summary>
        public LotInfoEx ProcessLotData
        {
            get
            {
                return this["ProcessLotData"] as LotInfoEx;
            }
            protected set
            {
                this["ProcessLotData"] = value;
            }
        }

        /// <summary>
        /// 子單元全域變數
        /// </summary>
        private ComponentInfo ComponentInfo
        {
            get
            {
                return (ComponentInfo)this["ComponentInfo"];
            }
            set
            {
                this["ComponentInfo"] = value;
            }
        }

        /// <summary>
        /// 輸入的RUNCARD批號
        /// </summary>
        private List<ComponentInfo> _ComponentList
        {
            get
            {
                return (List<ComponentInfo>)this["_ComponentList"];
            }
            set
            {
                this["_ComponentList"] = value;
            }
        }

        /// <summary>
        /// 中心孔量測的上下限設定
        /// </summary>
        private WpcExClassItemInfo SAICenterHole
        {
            get
            {
                return (WpcExClassItemInfo)this["SAICenterHole"];
            }
            set
            {
                this["SAICenterHole"] = value;
            }
        }

        /// <summary>
        /// 小工單號的PRODTYPE
        /// </summary>
        private String _ProdType
        {
            get
            {
                return (String)this["_ProdType"];
            }
            set
            {
                this["_ProdType"] = value;
            }
        }

        private String _CenterHoleFlag
        {
            get
            {
                return (String)this["_CenterHoleFlag"];
            }
            set
            {
                this["_CenterHoleFlag"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 檢查權限
                if (!IsPostBack)
                {
                    if (!UserProfileInfo.CheckUserRight(User.Identity.Name, ProgramRight))
                    {
                        HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                        return;
                    }

                    AjaxFocus(ttbWOLot);
                }
                else
                {
                    AjaxFocus(ttbWorkpiece);
                }
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
            ttbWorkpiece.Text = "";
            ttbMaterialLot.Text = "";
            ttbWorkpieceSerialNumber.Text = "";
            ttbDeviceName.Text = "";

            ttbDeviceDescr.Text = "";
            ttbOperation.Text = "";
            ttbRouteName.Text = "";
            ttbTemperature.Text = "";
            ttbTemperature.ReadOnly = true;

            gvComponentEDC.DataSource = null;
            gvComponentEDC.DataBind();
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearData()
        {
            ComponentInfo = null;
            ProcessLotData = null;
            SAICenterHole = null;
            _ProdType = null;
            _CenterHoleFlag = null;
        }

        protected void ttbWorkpiece_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string inputObject = ttbWorkpiece.Text.Trim();
                if (inputObject.IsNullOrEmpty())
                {
                    ClearUI();
                    return;
                }
                //轉換字串最後"."的字串
                inputObject = CustomizeFunction.ConvertDMCCode(inputObject);
                //DMCCode有刻字有SN
                if (_ProdType == CustomizeFunction.ProdType.S.ToCimesString())
                {
                    ComponentInfo = _ComponentList.Find(p => p.ComponentID == inputObject);
                    if (ComponentInfo == null)
                    {
                        //工件{0}不屬於Runcard {1}，請確認!!
                        throw new CimesException(RuleMessage.Error.C00030(inputObject, ttbWOLot.Text));
                    }
                    #region 以ComponentID找出ComponentInfo
                    //ComponentInfo = ComponentInfoEx.GetComponentByComponentID(inputObject);

                    //if (ComponentInfo != null)
                    //{
                    //    ProcessLotData = LotInfo.GetLotByLot(ComponentInfo.CurrentLot);
                    //}
                    #endregion
                }

                //DMCCode有刻字無SN                
                if (_ProdType == CustomizeFunction.ProdType.G.ToCimesString())
                {
                    #region 以MLot,WOLot找出ComponentInfo
                    // 以物料批找出批號
                    var lstLots = LotInfoEx.GetLotByMaterialLotAndWOLot(inputObject, ProcessLotData.WorkOrderLot);
                    if (lstLots.Count > 1)
                    {
                        throw new RuleCimesException(RuleMessage.Error.C10040(inputObject));
                    }
                    // 若物料批找不到批號則以WOLot找出批號
                    if (lstLots.Count == 0)
                    {
                        //[00030]{0}：{1}不存在!
                        throw new RuleCimesException(TextMessage.Error.T00030(lblWOLot.Text + "(" + ttbWOLot.Text + ")," + lblMaterialLot.Text + "(" + inputObject + ")", GetUIResource("Lot")));
                    }

                    ProcessLotData = lstLots[0];

                    // 取得所有子單元，並取得沒有做過中心孔量測的批號，以ComponentID排序
                    var lstComponents = ComponentInfo.GetLotAllComponents(ProcessLotData).FindAll(p => p["CENTER_HOLE_FLAG"].ToString() == "N").OrderBy(p => p.ComponentID).ToList();
                    ComponentInfo = lstComponents.Count == 0 ? null : lstComponents[0];

                    if (ComponentInfo == null)
                    {
                        throw new CimesException(RuleMessage.Error.C00039(ProcessLotData.Lot));
                    }
                    #endregion
                }

                //DMCCode有刻字無意義,或是沒有刻DMCCODE，WOLOT是唯一所以可以直接找到批號
                if (_ProdType == CustomizeFunction.ProdType.B.ToCimesString() || _ProdType == CustomizeFunction.ProdType.W.ToCimesString())
                {
                    #region 以小工單號找出批號
                    // 以小工單號找出批號
                    //ProcessLotData = LotInfoEx.GetLotByWorkOrderLot(ttbWOLot.Text.Trim());
                    ProcessLotData = LotInfoEx.GetLotByLot(ttbWOLot.Text.Trim());
                    if (ProcessLotData == null)
                    {
                        throw new RuleCimesException(RuleMessage.Error.C10040(inputObject));
                    }

                    var lstComponents = ComponentInfo.GetLotAllComponents(ProcessLotData).ChangeTo<ComponentInfoEx>();

                    var lstComponentTemp = lstComponents.Find(p => p.DMC == inputObject);
                    if(lstComponentTemp != null)
                    {
                        throw new CimesException(RuleMessage.Error.C00052(inputObject));
                    }

                    // 取得所有子單元，並取得沒有做過中心孔量測的批號，以ComponentID排序
                    lstComponents = lstComponents.FindAll(p=> p["CENTER_HOLE_FLAG"].ToString() == "N").OrderBy(p => p.ComponentID).ToList();

                    ComponentInfo = lstComponents.Count == 0 ? null : lstComponents[0];

                    if (ComponentInfo == null)
                    {
                        throw new CimesException(RuleMessage.Error.C00039(ProcessLotData.Lot));
                    }
                    #endregion
                }

                // 找不到工件
                if (ComponentInfo == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Workpiece")));
                }

                // 找不到批號
                if (ProcessLotData == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Lot")));
                }

                if (ComponentInfo["CENTER_HOLE_FLAG"].ToString() != "N")
                {
                    throw new RuleCimesException(RuleMessage.Error.C10049());
                }

                //批號檢查狀態
                if (ProcessLotData.Status != LotDefaultStatus.Run.ToCimesString())
                {
                    //[01203]批號狀態不正確, 應為 {0} !
                    throw new Exception(TextMessage.Error.T01203("Run"));
                }

                // 顯示機加批號資訊
                //ttbWOLot.Text = ProcessLotData["WOLOT"].ToString();
                // 顯示鍛造批號資訊
                ttbMaterialLot.Text = ProcessLotData["MATERIALLOT"].ToString();
                // 顯示工件序號資訊
                ttbWorkpieceSerialNumber.Text = ComponentInfo.ComponentID;
                // 顯示料號資訊
                ttbDeviceName.Text = ProcessLotData.DeviceName;

                var deviceInfo = DeviceInfo.GetDeviceByName(ProcessLotData.DeviceName);
                // 顯示機加批號資訊
                ttbDeviceDescr.Text = deviceInfo.Description;
                // 顯示工作站資訊
                ttbOperation.Text = ProcessLotData.OperationName;
                // 顯示流程名稱資訊
                ttbRouteName.Text = ProcessLotData.RouteName;

                if (_CenterHoleFlag.ToBool())
                {
                    // 取得中心孔量測設定值
                    var lstSAICenterHolde = WpcExClassItemInfo.GetExClassItemInfo("SAICenterHole", ProcessLotData.DeviceName);
                    if (lstSAICenterHolde.Count == 0)
                    {
                        lstSAICenterHolde = WpcExClassItemInfo.GetExClassItemInfo("SAICenterHole", "ALL");
                    }
                    // 若找不到中心孔量測需拋錯
                    if (lstSAICenterHolde.Count == 0)
                    {
                        throw new RuleCimesException(TextMessage.Error.T00555("SAICenterHole", ProcessLotData.DeviceName + "," + "ALL"));
                    }

                    SAICenterHole = lstSAICenterHolde[0];
                    // 設定中心孔量測的DataTable資料
                    DataTable dtEmpty = new DataTable();
                    dtEmpty.Columns.Add("ITEM", typeof(int));
                    dtEmpty.Columns.Add("EDC", typeof(String));

                    for (int i = 0; i < SAICenterHole.Remark04.ToDecimal(0); i++)
                    {
                        DataRow dr = dtEmpty.NewRow();
                        dr["ITEM"] = i + 1;
                        dtEmpty.Rows.Add(dr);
                    }
                    // 將產生的資料表顯示在畫面上
                    gvComponentEDC.DataSource = dtEmpty;
                    gvComponentEDC.DataBind();

                    ttbTemperature.ReadOnly = false;
                }
                else
                {
                    btnOK_Click(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                ttbWorkpiece.Text = "";
                ClearUI();
                HandleError(ex, ttbWorkpiece.ClientID);
            }
        }

        protected void gvComponentEDC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //顯示量測規格於表頭
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[1].Text = GetUIResource("UPSPEC") + ":" + SAICenterHole.Remark02 + "<br>"
                        + GetUIResource("LOWSPEC") + ":" + SAICenterHole.Remark03 + "<br>";
                }

                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                DataRowView dr = e.Row.DataItem as DataRowView;
                // 顯示項次
                var lblItem = e.Row.FindControl("lblItem") as Label;
                lblItem.Text = dr["ITEM"].ToString();

                var ttbEDC = e.Row.FindControl("ttbEDC") as TextBox;
                ttbEDC.Attributes.Add("EDCFlag", "Y");
                if (e.Row.RowIndex == 0)
                {
                    AjaxFocus(ttbEDC);
                }

                // 註冊前端Javascript
                string validateString = "javascript:ValidateInputNumber(this,{0},{1});";
                validateString = string.Format(validateString, SAICenterHole.Remark03, SAICenterHole.Remark02);
                ttbEDC.Attributes.Add("onblur", validateString);
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
                // 子單元不存在拋錯
                if (ComponentInfo == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Component")));
                }
                // 批號不存在拋錯
                if (ProcessLotData == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Lot")));
                }
                // 定義交易戳記
                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    bool inSpec = true;
                    if (_CenterHoleFlag != "N")
                    {
                        gvComponentEDC.Rows.LoopDo<GridViewRow>((p, i) => {
                            // 取得TextBox Control
                            var ttbEDC = p.FindControl("ttbEDC") as TextBox;
                            if (ttbEDC.Text.IsNullOrEmpty())
                            {
                                throw new RuleCimesException(TextMessage.Error.T00043(GetUIResource("CenterHoleData")));
                            }
                            // 是否符合規格判斷
                            var measureVal = ttbEDC.Text.ToDecimal();
                            if (measureVal > SAICenterHole.Remark02.ToDecimal())
                            {
                                inSpec = false;
                            }

                            if (measureVal < SAICenterHole.Remark03.ToDecimal())
                            {
                                inSpec = false;
                            }
                            // 將量測資料記錄到客製表
                            var edcCompInfo = InfoCenter.Create<CSTEDCComponentInfo>();
                            edcCompInfo.ComponentID = ComponentInfo.ComponentID;
                            edcCompInfo.Data = measureVal;
                            edcCompInfo.UpSpecification = SAICenterHole.Remark02.ToDecimal();
                            edcCompInfo.LowSpecification = SAICenterHole.Remark03.ToDecimal();
                            edcCompInfo.INSPEC = inSpec == true ? "OK" : "NG";
                            edcCompInfo.Lot = ProcessLotData.Lot;
                            edcCompInfo["LINKSID"] = txnStamp.LinkSID;
                            edcCompInfo["PARAMETER"] = "SC" + (i + 1).ToString();
                            edcCompInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                        });
                    }

                    if (!ttbTemperature.Text.IsNullOrEmpty())
                    {
                        // 溫度必須是數值
                        ttbTemperature.MustDecimal(lblTemperature);
                        // 將量測資料記錄到客製表
                        var edcCompInfo = InfoCenter.Create<CSTEDCComponentInfo>();
                        edcCompInfo.ComponentID = ComponentInfo.ComponentID;
                        edcCompInfo.Data = ttbTemperature.Text.ToDecimal();
                        edcCompInfo.Lot = ProcessLotData.Lot;
                        edcCompInfo["LINKSID"] = txnStamp.LinkSID;
                        edcCompInfo["PARAMETER"] = "Temperature";
                        edcCompInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }

                    var lstCompAttr = new List<ModifyAttributeInfo>();
                    // 紀錄子單元量測時間
                    lstCompAttr.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("CENTER_HOLE_TIME", txnStamp.RecordTime));
                    lstCompAttr.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("DMC", ttbWorkpiece.Text.Trim()));
                    if (inSpec)
                    {
                        // 修改子單元系統屬性
                        lstCompAttr.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("CENTER_HOLE_FLAG", "OK"));
                        WIPTransaction.ModifyLotComponentMultipleAttribute(ProcessLotData, ComponentInfo, lstCompAttr, txnStamp);
                    }
                    else
                    {
                        // 修改子單元系統屬性
                        lstCompAttr.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("CENTER_HOLE_FLAG", "NG"));
                        WIPTransaction.ModifyLotComponentMultipleAttribute(ProcessLotData, ComponentInfo, lstCompAttr, txnStamp);

                        #region 若不在規格範圍內則拆批至待判站點
                        var lstSourceLot = new List<LotInfo>();
                        List<SqlAgent> splitLotArchiSQLList = new List<SqlAgent>();
                        List<ComponentInfo> lsComponentDatas = new List<ComponentInfo>();
                        lsComponentDatas.Add(ComponentInfo);

                        var generator = NamingIDGenerator.GetRule("SplitLot");
                        if (generator == null)
                        {
                            //WRN-00411,找不到可產生的序號，請至命名規則維護設定，規則名稱：{0}!!!
                            throw new Exception(TextMessage.Error.T00437("SplitLot"));
                        }
                        var serialData = generator.GenerateNextIDs(1, ProcessLotData, new string[] { }, User.Identity.Name);
                        splitLotArchiSQLList = serialData.Second;
                        var splitLotID = serialData.First[0];

                        var reasonCategoryInfo = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");
                        SplitLotInfo splitLotData = SplitLotInfo.CreateSplitLotByLotAndQuantity(ProcessLotData.Lot, splitLotID, lsComponentDatas, reasonCategoryInfo, "EDC");

                        WIPTxn.SplitIndicator splitInd = WIPTxn.SplitIndicator.Create();
                        WIPTxn.Default.SplitLot(ProcessLotData, splitLotData, splitInd, txnStamp);

                        //若子單元為自動產生，更新序號至DB
                        if (splitLotArchiSQLList != null && splitLotArchiSQLList.Count > 0)
                        {
                            DBCenter.ExecuteSQL(splitLotArchiSQLList);
                        }
                        #endregion

                        var splitLotInfo = LotInfo.GetLotByLot(splitLotID);

                        // 新增Defect
                        List<ComponentDefectObject> lstCompDefectObj = new List<ComponentDefectObject>();
                        var reason = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "MeasureNG_MES");
                        lstCompDefectObj.Add(ComponentDefectObject.Create(ComponentInfo, 1, 0, reason, "CENTER_HOLE_FLAG:NG"));
                        WIPTransaction.DefectComponent(splitLotInfo, lstCompDefectObj, WIPTransaction.DefectIndicator.Create(), txnStamp);

                        #region ReassignOperation                        
                        // 取得待判站點設定
                        var saiJudgeOperation = WpcExClassItemInfo.GetExClassItemInfo("SAIJudgeOperation", splitLotInfo["PROCESS"].ToString());
                        if (saiJudgeOperation.Count == 0)
                        {
                            throw new RuleCimesException(TextMessage.Error.T00555("SAIJudgeOperation", splitLotInfo["PROCESS"].ToString()));
                        }
                        var reassignOperationInfo = RouteOperationInfo.GetLotAllRouteOperations(splitLotInfo).Find(oper => oper.OperationName == saiJudgeOperation[0].Remark02);
                        if (reassignOperationInfo == null)
                        {
                            throw new RuleCimesException(RuleMessage.Error.C10050());
                        }

                        var lstLotAttr = new List<ModifyLotAttributeInfo>();
                        // 修改批號系統屬性
                        lstLotAttr.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL01", "Y"));
                        lstLotAttr.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL02", splitLotInfo.OperationSequence));
                        lstLotAttr.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL03", splitLotInfo.OperationName));
                        WIPTransaction.ModifyLotMultipleAttribute(splitLotInfo, lstLotAttr, txnStamp);
                        WIPTransaction.ReassignOperation(splitLotInfo, reassignOperationInfo, reasonCategoryInfo, "EDCSplitReassignOperation", txnStamp);
                        WIPTransaction.ResetLotRule(splitLotInfo, txnStamp);
                        #endregion
                    }
                    cts.Complete();
                }

                ClearUI();
                AjaxFocus(ttbWorkpiece);
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
                // 返回在製品查詢頁面
                ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ttbWOLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                /****************************************************************
                 * 1. 依照工作站點及料號的設定，判斷批號是否可執行中心孔資訊收集。
                 * 2. 部分料號就算不收中心孔資訊，但需要收集DMC
                 * 3. 若不須收集中心孔，且prodType = 'W'，連DMC都不須執行
                 ****************************************************************/
                ttbWorkpiece.ReadOnly = true;
                ttbTemperature.ReadOnly = true;
                // 清除資料與使用者介面
                ClearField();

                string sWOLot = ttbWOLot.Text.Trim();
                if (sWOLot.IsNullOrEmpty())
                {
                    return;
                }

                // 小工單號找批號
                //var lotInfo = LotInfoEx.GetLotByWorkOrderLot(sWOLot);
                ProcessLotData = LotInfoEx.GetLotByLot(sWOLot);

                if (ProcessLotData == null)
                {
                    ttbWOLot.Text = "";
                    AjaxFocus(ttbWOLot);
                    throw new RuleCimesException(TextMessage.Error.T00030(lblWOLot.Text + "(" + sWOLot + ")", GetUIResource("Lot")));
                }

                _ComponentList = ComponentInfo.GetLotAllComponents(ProcessLotData);

                //取得小工單號的料號版本
                var deviceVersionInfo = DeviceVersionInfo.GetLotCurrentDeviceVersion(ProcessLotData).ChangeTo<DeviceVersionInfoEx>();
                if (deviceVersionInfo == null)
                {
                    throw new CimesException(TextMessage.Error.T00537(ProcessLotData.DeviceName));
                }

                if (deviceVersionInfo.ProdType.IsNullOrTrimEmpty())
                {
                    ttbWOLot.Text = "";
                    AjaxFocus(ttbWOLot);
                    throw new RuleCimesException(TextMessage.Error.T00031(GetUIResource("Device"), deviceVersionInfo.DeviceName, "PRODTYPE"));
                }
                _ProdType = deviceVersionInfo.ProdType;
                _CenterHoleFlag = deviceVersionInfo.CenterHoleFlag;

                //確認執行的工作站是否需要收集中心孔
                var operData = OperationInfo.GetOperationByName(ProcessLotData.OperationName).ChangeTo<OperationInfoEx>();
                if (operData == null)
                {
                    throw new CimesException(TextMessage.Error.T00171(ProcessLotData.OperationName));
                }

                if (!operData.CenterHoleFlag.ToBool() && !operData.GetDMC.ToBool())
                {
                    AjaxFocus(ttbWOLot);
                    throw new CimesException(RuleMessage.Error.C00042(ProcessLotData.OperationName));
                }

                //若不須收集中心孔，且prodType = 'W'，連DMC都不須執行
                if (!_CenterHoleFlag.ToBool() && deviceVersionInfo.ProdType == CustomizeFunction.ProdType.W.ToCimesString())
                {
                    AjaxFocus(ttbWOLot);
                    throw new CimesException(RuleMessage.Error.C00043(ProcessLotData.DeviceName));
                }

                if (_CenterHoleFlag.ToBool())
                {
                    gvComponentEDC.Visible = true;
                }
                else
                {
                    gvComponentEDC.Visible = false;
                }
                ttbWorkpiece.ReadOnly = false;
                AjaxFocus(ttbWorkpiece);

            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}