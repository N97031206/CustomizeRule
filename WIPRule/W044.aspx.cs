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
using CustomizeRule.RuleUtility;
using Ares.Cimes.IntelliService.DbTableSchema;
using ciMes.Web.Common.UserControl;

namespace CustomizeRule.WIPRule
{
    public partial class W044 : CustomizeRuleBasePage
    {
        private String _JudgeOperationName
        {
            get
            {
                return (String)this["_JudgeOperationName"];
            }
            set
            {
                this["_JudgeOperationName"] = value;
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
        private ComponentInfoEx _ComponentInfo
        {
            get
            {
                return (ComponentInfoEx)this["ComponentInfo"];
            }
            set
            {
                this["ComponentInfo"] = value;
            }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
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
            ttbDefectDesc.Text = "";

            ddlDefectReason.Items.Clear();
            ddlOperation.Items.Clear();                      
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearData()
        {
            _ComponentInfo = null;
            ProcessLotData = null;            
            _ProdType = null;
            _JudgeOperationName = null;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // 檢查權限
                if (!IsPostBack)
                {
                    if (!UserProfileInfo.CheckUserRight(User.Identity.Name, ProgramRight))
                    {
                        HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                        return;
                    }
                    LoadDefaultControl();
                    AjaxFocus(ttbWOLot);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void LoadDefaultControl()
        {
            ttbWorkpiece.ReadOnly = true;
            ttbDefectDesc.ReadOnly = true;
            ddlOperation.Enabled = false;
            ddlDefectReason.Enabled = false;
            btnOK.Enabled = false;
        }

        protected void ttbWOLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                /***************************************************************************************
                 * 依照PRODTYP不同，做不同畫面的處置：
                 * S：有序號，一律刷入DMC處理。
                 * G：僅有鍛造批，選擇要判定的工作站，系統找出該工作站所有的COMPONENT，順排取最後一個
                 * W：沒有刻字，選擇要判定的工作站，系統找出該工作站所有的COMPONENT，順排取最後一個
                 * B：有刻字，但序號無意義。
                 *    在刻字站前因為未刻字所以選擇要判定的工作站，系統找出該工作站所有的COMPONENT，順排取最後一個
                 *    刻字後，直接刷入DMC處理          
                 **********************************************************/
                // 清除資料與使用者介面                
                ClearField();

                LoadDefaultControl();

                string sWOLot = ttbWOLot.Text.Trim();
                if (sWOLot.IsNullOrEmpty())
                {
                    return;
                }

                #region 找出料號型態
                // 小工單號找批號，此處找批號只是為了找出料號，也順便可以確認線上還有批號可以做送待判這件事
                ProcessLotData = LotInfoEx.GetLotByWorkOrderLot(sWOLot);
                if (ProcessLotData == null)
                {
                    ProcessLotData = LotInfoEx.GetLotByLot(sWOLot);
                }

                if (ProcessLotData == null)
                {
                    ttbWOLot.Text = "";
                    AjaxFocus(ttbWOLot);
                    throw new RuleCimesException(TextMessage.Error.T00030(GetUIResource("WOLot"), sWOLot));
                }

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
                #endregion

                #region 取得工作站
                ddlOperation.Items.Clear();
                var lstLotRouteOperation = RouteOperationInfo.GetLotDefaultOperations(ProcessLotData).OrderBy(p => p.OperationSequence).ToList();
                lstLotRouteOperation.ForEach(oper => {
                    ddlOperation.Items.Add(new ListItem(oper.OperationName + "[" + oper.OperationSequence +"]", oper.OperationName));
                });
                ddlOperation.Items.Insert(0, "");
                #endregion

                #region 原因碼選項
                ddlDefectReason.Items.Clear();
                List<BusinessReason> reason = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(ProgramRight, ProcessLotData.OperationName, "Default", ReasonMode.Category);
                if (reason.Count == 0)
                {
                    reason = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(ProgramRight, "ALL", "Default", ReasonMode.Category);
                }

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
                    throw new Exception(TextMessage.Error.T00641(ProgramRight, ProcessLotData.OperationName));
                }
                #endregion

                #region 依照PRODTYPE處理介面
                ttbDefectDesc.ReadOnly = false;
                ddlDefectReason.Enabled = true;

                if (_ProdType == CustomizeFunction.ProdType.S.ToCimesString())
                {
                    ddlOperation.Enabled = false;
                    ttbWorkpiece.ReadOnly = false;
                }

                if (_ProdType == CustomizeFunction.ProdType.W.ToCimesString() || _ProdType == CustomizeFunction.ProdType.G.ToCimesString())
                {
                    ttbWorkpiece.ReadOnly = true;
                    ddlOperation.Enabled = true;
                }

                if (_ProdType == CustomizeFunction.ProdType.B.ToCimesString())
                {
                    ttbWorkpiece.ReadOnly = false;
                    ddlOperation.Enabled = true;
                }
                #endregion

                #region 找出待判站
                //在系統資料維護裡，取得此批號對應製程(CPC/CPF)的待判工作站名稱
                List<WpcExClassItemInfo> operationList = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIJudgeOperation");
                WpcExClassItemInfo judgeOperationData = operationList.Find(p => p.Remark01 == ProcessLotData.Process);
                if (judgeOperationData == null)
                {
                    //找不到待判站資訊，請至系統資料維護增加資訊，屬性：{0}
                    throw new Exception(RuleMessage.Error.C10014(ProcessLotData.Process));
                }

                //取得待判工作站名稱
                _JudgeOperationName = judgeOperationData.Remark02;

                #endregion

                if (ttbWorkpiece.Enabled)
                    AjaxFocus(ttbWorkpiece);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ttbWorkpiece_TextChanged(object sender, EventArgs e)
        {
            try
            {
                btnOK.Enabled = false;

                if (ttbWorkpiece.Text.Trim().IsNullOrEmpty())
                {
                    if (_ProdType == CustomizeFunction.ProdType.B.ToCimesString())
                    {
                        ddlOperation.Enabled = true;
                        ddlOperation.ClearSelection();
                    }
                    return;
                }

                string componentid = CustomizeFunction.ConvertDMCCode(ttbWorkpiece.Text.Trim());

                //僅有B跟S型態會刷DMC，故只需針對兩種CASE處理
                if (_ProdType == CustomizeFunction.ProdType.B.ToCimesString())
                {
                    _ComponentInfo = ComponentInfoEx.GetOneComponentByDMCCode(componentid);
                    if (_ComponentInfo == null)
                    {
                        throw new RuleCimesException(RuleMessage.Error.C00049(ttbWorkpiece.Text));
                    }
                }
                else if (_ProdType == CustomizeFunction.ProdType.S.ToCimesString())
                {
                    _ComponentInfo = ComponentInfoEx.GetComponentByComponentID(componentid);

                    if (_ComponentInfo == null)
                    {
                        throw new RuleCimesException(RuleMessage.Error.C10047(ttbWorkpiece.Text));
                    }
                }

                // 找不到工件
                if (_ComponentInfo == null)
                {
                    throw new RuleCimesException(RuleMessage.Error.C10047(ttbWorkpiece.Text));
                }

                ddlOperation.Enabled = false;

                ProcessLotData = LotInfoEx.GetLotByLot(_ComponentInfo.CurrentLot);
                if (ProcessLotData == null)
                {
                    AjaxFocus(ttbWorkpiece);
                    throw new RuleCimesException(TextMessage.Error.T00030(lblWOLot.Text + "(" + ttbWOLot.Text.Trim() + ")" + lblttbWorkpiece.Text + "(" + componentid + ")", GetUIResource("Lot")), ttbWorkpiece);
                }

                if (ProcessLotData.OperationName == _JudgeOperationName)
                {
                    AjaxFocus(ttbWorkpiece);
                    throw new RuleCimesException(RuleMessage.Error.C10175(_JudgeOperationName), ttbWorkpiece);
                }

                var item = ddlOperation.Items.FindByValue(ProcessLotData.OperationName);
                if (item != null)
                {
                    ddlOperation.ClearSelection();
                    item.Selected = true;
                }

                btnOK.Enabled = true;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                AjaxFocus(ttbWorkpiece);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // 子單元不存在拋錯
                if (_ComponentInfo == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Component")));
                }
                // 批號不存在拋錯
                if (ProcessLotData == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Lot")));
                }
                //  批號狀態必須為Wait
                if (ProcessLotData.Status != "Wait")
                {
                    throw new RuleCimesException(TextMessage.Error.T00424());
                }
                // 原因碼
                ddlDefectReason.Must(lblDefectReason);                

                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    //取得原因碼資訊
                    var reasonData = InfoCenter.GetBySID<ReasonCategoryInfo>(ddlDefectReason.SelectedValue);                    

                    //取得不良子批批號名稱
                    var splitLotNaming = GetNamingRule("SplitLot", txnStamp.UserID, ProcessLotData);

                    //批號拆子批
                    var splitLot = SplitLotInfo.CreateSplitLotByLotAndQuantity(ProcessLotData.Lot, splitLotNaming.First[0], new List<ComponentInfo>() { _ComponentInfo }, reasonData, reasonData.Description);
                    WIPTxn.Default.SplitLot(ProcessLotData, splitLot, WIPTxn.SplitIndicator.Create(), txnStamp);

                    if (splitLotNaming.Second != null && splitLotNaming.Second.Count != 0)
                    {
                        DBCenter.ExecuteSQL(splitLotNaming.Second);
                    }

                    //註記不良
                    var compDefect = ComponentDefectObject.Create(_ComponentInfo, _ComponentInfo.ComponentQuantity, 0, reasonData, ttbDefectDesc.Text.Trim());
                    WIPTransaction.DefectComponent(splitLot, new List<ComponentDefectObject>() { compDefect }, WIPTransaction.DefectIndicator.Create(), txnStamp);

                    #region 送至待判工作站

                    //取得目前批號的流程線上版本
                    RouteVersionInfo RouteVersion = RouteVersionInfo.GetRouteActiveVersion(ProcessLotData.RouteName);

                    //以目前工作站名稱去查詢在所有流程中的序號
                    var routeOperation = RouteOperationInfo.GetRouteAllOperations(RouteVersion).Find(p => p.OperationName == _JudgeOperationName);

                    //以目前工作站名稱去查詢在所有流程中的序號
                    var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");                    

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

                    cts.Complete();
                }

                LoadDefaultControl();
                ClearField();
                ttbWOLot.Text = "";
                AjaxFocus(ttbWOLot);

                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00614(""));
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnOK.Enabled = false;
                if (ddlOperation.SelectedValue.IsNullOrEmpty())
                {
                    if (_ProdType == CustomizeFunction.ProdType.B.ToCimesString())
                    {
                        ttbWorkpiece.ReadOnly = false;
                        ttbWorkpiece.Text = "";
                    }
                    return;
                }
                ttbWorkpiece.ReadOnly = true;
                
                //用小工單號找出所有的component，逆排後找出第一個符合選擇站點的component為送待判的對象
                var compList = ComponentInfoEx.GetAllComponentByWOLot(ttbWOLot.Text.Trim()).OrderByDescending(p => p.ComponentID).ToList();

                ProcessLotData = null;
                foreach (var p in compList)
                {
                    var tempLot = LotInfoEx.GetLotByLot(p.CurrentLot);
                    
                    //第一個找到站點符合的component就不用再找了
                    if (tempLot.OperationName == ddlOperation.SelectedValue)
                    {
                        ProcessLotData = tempLot;
                        break;
                    }
                }

                if (ProcessLotData == null)
                {
                    AjaxFocus(ddlOperation);
                    ttbWOLot.Text = string.Empty;
                    throw new RuleCimesException(RuleMessage.Error.C00050(ttbWOLot.Text, ddlOperation.SelectedValue));
                }

                // 取得子單元
                var lstComponents = ComponentInfo.GetLotAllComponents(ProcessLotData).OrderByDescending(p => p.ComponentID).ToList();
                _ComponentInfo = lstComponents.Count == 0 ? null : lstComponents[0].ChangeTo<ComponentInfoEx>();
                // 判斷子單元是否存在
                if (_ComponentInfo == null)
                {
                    throw new CimesException(RuleMessage.Error.C00039(ProcessLotData.Lot));
                }
                ttbWorkpiece.ReadOnly = true;
                ttbWorkpiece.Text = _ComponentInfo.ComponentID;                
                btnOK.Enabled = true;
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}