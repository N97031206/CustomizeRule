//  SEQ  DATE       AUTHOR       MESSAGE
//===================================================
//  001  20120330  Ryan WU  修正儲存後gridview update切換到新增tool當頁並且將該筆新紀錄以選取方式呈現
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Web.UI;
using ciMes.PMS.UserControl;
using ciMes.PMS.Utility;
using ciMes.Web.Common;
using ciMes.Web.Common.UserControl;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Security.Principal;
using CustomizeRule.RuleUtility;
using Ares.Cimes.IntelliService.Transaction;

namespace CustomizeRule.ToolRule
{
    public partial class T008 : CimesBasePage
    {
        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        /// <summary>
        /// 紀錄命名規則的SQL清單
        /// </summary>
        private List<SqlAgent> ExecuteNamingSQLList
        {
            get { return this["ExecuteNamingSQLList"] as List<SqlAgent>; }
            set { this["ExecuteNamingSQLList"] = value; }
        }

        /// <summary>
        /// 儲存檢驗報告路徑資料
        /// </summary>
        private List<WpcExClassItemInfo> _SaveInspectionData
        {
            get { return this["_SaveInspectionData"] as List<WpcExClassItemInfo>; }
            set { this["_SaveInspectionData"] = value; }
        }

        /// <summary>
        /// 刀具進料原因
        /// </summary>
        private ReasonCategoryInfo _ReasonCategory
        {
            get { return this["_ReasonCategory"] as ReasonCategoryInfo; }
            set { this["_ReasonCategory"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }

        }

        private SystemAttribute _SystemAttribute
        {
            get { return SystemAttribute1 as SystemAttribute; }

        }

        private AttributeSetupGrid _AttributeSetupGrid
        {
            get { return AttributeSetupGrid1 as AttributeSetupGrid; }

        }

        //private UserDefineColumnSet _UserDefineColumnSet
        //{
        //    get { return UserDefineColumnSet1 as UserDefineColumnSet; }

        //}

        /// <summary>
        /// 刀具檢驗報告
        /// </summary>
        private List<CSTToolReportInfo> _ToolReports
        {
            get { return this["_ToolReports"] as List<CSTToolReportInfo>; }
            set { this["_ToolReports"] = value; }
        }

        List<ToolInfoEx> AllTools
        {
            get
            {
                return (List<ToolInfoEx>)this["AllTools"];
            }
            set
            {
                this["AllTools"] = value;
            }
        }

        AttributeSetupGrid ToolAttributeSetup
        {
            get
            {
                return (AttributeSetupGrid)AttributeSetupGrid1;
            }
        }

        PMCounterSetup PMCounterSetupInterface
        {
            get
            {
                return (PMCounterSetup)PMCounterSetup1;
            }
        }

        String Action
        {
            get
            {
                return (String)this["Action"];
            }
            set
            {
                this["Action"] = value;
            }
        }

        ToolInfoEx CurrentToolData
        {
            get
            {
                return (ToolInfoEx)this["CurrentToolData"];
            }
            set
            {
                this["CurrentToolData"] = value;
            }
        }

        private DataSet _dsReport
        {
            get
            {
                return (DataSet)this["_dsReport"];
            }
            set
            {
                this["_dsReport"] = value;
            }

        }
        protected override void OnInit(EventArgs e)
        {
            CurrentUpdatePanel = UpdatePanel1;
            PMCounterSetupInterface.ControlException += new PMCounterSetup.ErrorEventHandler(PMCounterSetupInterface_ControlException);
            gvToolList.ClearCimesFiltering += new Ares.Cimes.IntelliService.Web.UI.CimesGridView.ClearCimesFilteringEventHandler(gvToolList_ClearCimesFiltering);
            base.OnInit(e);
        }

        void gvToolList_ClearCimesFiltering()
        {
            try
            {
                gvToolList.ResetFilterData();
                QueryAllTool();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        void PMCounterSetupInterface_ControlException(object sender, Exception e)
        {
            HandleError(e, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
        }

        void PMModule_ControlException(object sender, Exception e)
        {
            HandleError(e, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.ID = "ToolMaster";
                if (!IsPostBack)
                {
                    // Check User Right ,防止User直接打網址進入
                    if (!UserProfileInfo.CheckUserRight(User.Identity.Name, ProgramRight))
                    {
                        HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                    }

                    AllTools = new List<ToolInfoEx>();
                    _ToolReports = new List<CSTToolReportInfo>();
                    _dsReport = new DataSet();
                    QueryAllTool();
                    UIControl();
                    LoadReason();
                }
                else
                {
                    gvToolList.DataSource = AllTools;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        private void UIControl()
        {
            ddlType.Items.Clear();
            var allToolTypeData = ToolTypeInfoEx.GetToolTypeByToolClass("CUTTER").OrderBy(p => p.Type).ToList();
            allToolTypeData.FindAll(p => p.Status.ToBool()).ForEach(p => ddlType.Items.Add(new ListItem(p.Type, p.ID)));
            ddlType.Items.Insert(0, "");

            ddlState.Items.Clear();
            var allToolStateData = ToolStateInfo.GetAllToolStates();
            allToolStateData.FindAll(p => p.Status.ToBool()).ForEach(p => ddlState.Items.Add(new ListItem(p.State, p.ID)));
            ddlState.Items.Insert(0, "");

            var tabPM = rtsDetail.FindTabByValue("PM");
            var tabPMCounter = rtsDetail.FindTabByValue("PMCounter");
            if (CimesGenuine.RegisterLicense.PM.AccessAuthority == EAccessAuthority.FullAccess)
            {
                if (tabPM != null)
                    //tabPM.Visible = true;
                    if (tabPMCounter != null)
                        tabPMCounter.Visible = true;
            }
            else
            {
                if (tabPM != null)
                    //tabPM.Visible = false;
                    if (tabPMCounter != null)
                        tabPMCounter.Visible = false;
            }
        }
        private void LoadReason()
        {
            _ReasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "刀具進料");
            if (_ReasonCategory == null)
            {
                throw new CimesException(RuleMessage.Error.C00053("CustomizeReason", "刀具進料"));
            }
        }

        private void QueryAllTool()
        {
            AllTools = ToolInfoEx.GetToolByToolClass("CUTTER");
            AllTools = AllTools.OrderBy(p => p.ToolName).ToList();
            BindGridView();
        }

        private void BindGridView()
        {
            gvToolList.DataSource = AllTools;
            gvToolList.DataBind();

            ClearFieldAndDisableTab();
        }

        protected void gvToolList_CimesSorted(object sender, Ares.Cimes.IntelliService.Web.UI.CimesSortedArgs e)
        {
            try
            {
                ClearFieldAndDisableTab();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvToolList_CimesFiltered(object sender, Ares.Cimes.IntelliService.Web.UI.CimesFilteredArgs e)
        {
            try
            {
                ClearFieldAndDisableTab();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvToolList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                CurrentToolData = AllTools[gvToolList.Rows[e.RowIndex].DataItemIndex];

                if (CurrentToolData.ActiveFlag == "T")     //ActiveFlag的判斷,若曾經啟用過則不可刪除
                {
                    throw new Exception(TextMessage.Error.T00714());
                }

                using (CimesTransactionScope cts = CimesTransactionScope.Create())
                {
                    var userID = User.Identity.Name;
                    var updateTime = DBCenter.GetSystemTime();

                    //刪除[CST_TOOL_LIFE]資料
                   var toolLifes =  CSTToolLifeInfo.GetToolLifeByToolNmae(CurrentToolData.ToolName);
                    toolLifes.ForEach(toolLife =>
                    {
                        toolLife.DeleteFromDB();
                        LogCenter.LogToDB(toolLife, LogCenter.LogIndicator.Create(ActionType.Remove, userID, updateTime));
                    });
                    CurrentToolData.DeleteFromDB();
                    AttributeAttributeInfo.DeleteByObjectSIDAndDataClass(CurrentToolData.ID, "ToolAttribute", userID, updateTime);
                    LogCenter.LogToDB(CurrentToolData, LogCenter.LogIndicator.Create(ActionType.Remove, userID, updateTime));
                    cts.Complete();
                }
                CurrentToolData = null;
                gvToolList.ResetCloneDataSource();
                QueryAllTool();
                //BindGridView();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        void BindDataByCurrentToolData()
        {
            ttbToolName.Text = CurrentToolData.ToolName;
            // ttbToolName.ReadOnly = true;
            if (CurrentToolData.UsingStatus == UsingStatus.Enable)
            {
                rbtEnable.Checked = true;
                rbtDisable.Checked = false;
            }
            else
            {
                rbtEnable.Checked = false;
                rbtDisable.Checked = true;
            }

            //採購產編
            ttbPurchasingProd.Text = CurrentToolData.Fortune;

            ddlType.Enabled = false;
            var li = ddlType.Items.FindByValue(CurrentToolData.ToolTypeSID);
            if (li != null)
            {
                ddlType.ClearSelection();
                li.Selected = true;

                btnPictureQuery.Enabled = true;

                //取得供應商清單
                GetSuppliers(li.Text);

                ddlSupplier.Enabled = false;
                var vendorItem = ddlSupplier.Items.FindByText(CurrentToolData.Vendor);
                if (vendorItem != null)
                {
                    ddlSupplier.ClearSelection();
                    vendorItem.Selected = true;
                }

                //取得此刀具型態的資料
                var toolType = ToolTypeInfo.GetToolTypeByType(li.Text).ChangeTo<ToolTypeInfoEx>();
                ttbToolTypeSpec.Text = toolType.Specification;
                ttbUnit.Text = toolType.Department;
            }

            li = ddlState.Items.FindByValue(CurrentToolData.ToolStateSID);
            if (li != null)
            {
                ddlState.ClearSelection();
                li.Selected = true;
            }

            DropDownList ddlCountType = (DropDownList)FindControl("PMCounterSetup1$ddlCountType");
            if (ddlCountType != null)
            {
                li = ddlCountType.Items.FindByValue(CurrentToolData.CountType);
                if (li != null)
                {
                    ddlCountType.ClearSelection();
                    li.Selected = true;
                }
            }

            TextBox ttbCountRatio = (TextBox)FindControl("PMCounterSetup1$ttbCountRatio");
            if (ttbCountRatio != null)
                ttbCountRatio.Text = CurrentToolData.CountRatio.ToString();

            TextBox ttbTotalCount = (TextBox)FindControl("PMCounterSetup1$ttbTotalCount");
            if (ttbTotalCount != null)
                ttbTotalCount.Text = CurrentToolData.TotalCount.ToString();

            TextBox ttbScrapCount = (TextBox)FindControl("PMCounterSetup1$ttbScrapCount");
            if (ttbScrapCount != null)
                ttbScrapCount.Text = CurrentToolData.ScrapCount.ToString();

            ttbDescr.Text = CurrentToolData.Description;
            ToolAttributeSetup.BindData("ToolAttribute", CurrentToolData.ID);
            //PMCounterSetupInterface.BindUserControl(PMSType.ToolID, CurrentToolData.ID);
            //_UserDefineColumnSet.SetControl(BindType.Tool, CurrentToolData);
            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnInspectionQuery.Visible = true;
            btnPictureQuery.Visible = true;
            btnPrint.Visible = true;
            Action = "U";

            rtsDetail.PurgeToSetByIndex(0);

            //顯示系統屬性
            _SystemAttribute.LoadSystemAttribute(CurrentToolData, "ToolSystemAttribute");

            //取得檢驗報告清單
            _ToolReports = InfoCenter.GetList<CSTToolReportInfo>("SELECT * FROM CST_TOOL_REPORT WHERE TOOLNAME = #[STRING]", CurrentToolData.ToolName);
            if (_ToolReports.Count > 0)
            {
                btnInspectionQuery.Enabled = true;
            }
        }

        protected void gvToolList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                ClearData();
                EnableTab();
                CurrentToolData = AllTools[gvToolList.Rows[e.NewSelectedIndex].DataItemIndex];
                BindDataByCurrentToolData();

                //取得路徑對應清單
                _SaveInspectionData = GetExtendItemListByClass("SAIToolInspection");


            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvToolList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvToolList.DataSource = AllTools;
                gvToolList.PageIndex = e.NewPageIndex;
                gvToolList.SelectedIndex = -1;
                gvToolList.DataBind();

                ClearFieldAndDisableTab();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvToolList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ToolInfo currentTool = (ToolInfo)e.Row.DataItem;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvToolList_DataSourceChanged(object sender, Ares.Cimes.IntelliService.Web.UI.CimesDataSourceEventArgs e)
        {
            try
            {
                AllTools = (e.CurrentDataSource as IList).NewList(p => (ToolInfoEx)p);
                //gvToolList.DataSource = AllTools;
                //gvToolList.SelectedIndex = -1;
                //gvToolList.DataBind();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                UIControl();

                CurrentToolData = InfoCenter.Create<ToolInfoEx>();
                ClearData();
                EnableTab();
                ddlState.ClearSelection();
                var li = ddlState.Items.FindByText("IDLE");
                if (li == null)
                {
                    throw new CimesException(GetUIResource("ToolInitialStateUnSet"));
                }
                li.Selected = true;
                ToolAttributeSetup.BindData("ToolAttribute", CurrentToolData.ID);
                PMCounterSetupInterface.BindUserControl(PMSType.ToolID, CurrentToolData.ID);
                //_UserDefineColumnSet.SetControl(BindType.Tool, null);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnPrint.Visible = true;
                btnInspectionQuery.Visible = true;
                btnPictureQuery.Visible = true;
                Action = "A";

                //取得路徑對應清單
                _SaveInspectionData = GetExtendItemListByClass("SAIToolInspection");

                //清除檢驗報告清單
                _ToolReports = new List<CSTToolReportInfo>();

                btnUpload.Enabled = false;
                FileUpload1.Enabled = false;
                btnPrint.Enabled = false;
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region 檢查輸入資料
                ddlType.Must(lblType);

                ttbToolName.Must(lblToolName);

                //確認採購產編是否有輸入
                ttbPurchasingProd.Must(lblPurchasingProd);

                //確認供應商是否有選擇
                ddlSupplier.Must(lblSupplier);

                //如果刀具數量有顯示出來，則確認數值必須為正整數
                if (ttbToolQty.Enabled)
                {
                   var toolQty = ttbToolQty.MustInt(lblToolQty);
                    if (toolQty <= 0)
                    {
                        AjaxFocus(ttbToolQty);

                        //[01397]數量必須大於零！
                        throw new Exception(TextMessage.Error.T01397());
                    }

                    //如果數量大於1，則進行多次建立資料
                    if (toolQty > 1)
                    {
                        MultiToolData(toolQty);
                        return;
                    }
                }

                if (!rbtDisable.Checked && !rbtEnable.Checked)
                {
                    throw new CimesException(TextMessage.Error.T00841(lblStates.Text));
                }

                CurrentToolData.ToolName = ttbToolName.Text.Trim();

                //品名規格
                CurrentToolData.Specification = ttbToolTypeSpec.Text;

                //單位別
                CurrentToolData.Department = ttbUnit.Text;

                //供應商
                CurrentToolData.Vendor = ddlSupplier.SelectedItem.Text;

                //採購產編
                CurrentToolData.Fortune = ttbPurchasingProd.Text;

                //註記為庫房
                CurrentToolData.Location = "Warehouse";

                if (CurrentToolData.InfoState == InfoState.NewCreate)
                {
                    //取得刀具型態資料
                    var toolTypeData = ToolTypeInfo.GetToolTypeByType(ddlType.SelectedItem.Text).ChangeTo<ToolTypeInfoEx>();

                    //確認檢驗報告是否需要上傳
                    if (toolTypeData.InspectionFlag == "Y")
                    {
                        if (_ToolReports.Count == 0)
                        {
                            //刀具類型:{0} 必須上傳檢驗報告資料 !
                            throw new Exception(RuleMessage.Error.C10126(toolTypeData.Type));
                        }
                    }

                    //刀面
                    CurrentToolData.Head = "1";
                    //身份
                    CurrentToolData.Identity = CustomizeFunction.ToolIdentity.新品.ToString();
                    //維修次數
                    CurrentToolData.MaintainCount = 0;

                    if (ToolInfo.GetToolByName(CurrentToolData.ToolName) != null)
                    {
                        throw new CimesException(TextMessage.Error.T00710(CurrentToolData.ToolName));
                    }
                }

                ToolAttributeSetup.ValidateCheck();
                #endregion

                //儲存系統屬性
                _SystemAttribute.SaveSystemAttribute(CurrentToolData);

                //預設ActiveFlag = "F"
                if (CurrentToolData.InfoState == InfoState.NewCreate)
                {
                    CurrentToolData.ActiveFlag = "F";
                }

                CurrentToolData.UsingStatus = (rbtEnable.Checked ? UsingStatus.Enable : UsingStatus.Disable);
                CurrentToolData.ToolTypeSID = ddlType.SelectedItem.Value;
                CurrentToolData.ToolType = ddlType.SelectedItem.Text;
                CurrentToolData.ToolClass = "CUTTER";
                if (rbtEnable.Checked)
                {
                    //若啟用，則ActiveFlag=T
                    CurrentToolData.ActiveFlag = "T";
                }
                if (ddlState.SelectedItem != null && !ddlState.SelectedItem.Value.IsNullOrTrimEmpty())
                {
                    CurrentToolData.CurrentState = ddlState.SelectedItem.Text;
                    CurrentToolData.ToolStateSID = ddlState.SelectedItem.Value;
                }
                CurrentToolData.Description = ttbDescr.Text.Trim();

                #region 將使用次數寫至ToolMaster
                DropDownList ddlCountType = (DropDownList)FindControl("PMCounterSetup1$ddlCountType");
                if (ddlCountType != null)
                    CurrentToolData.CountType = ddlCountType.Text;

                TextBox ttbCountRatio = (TextBox)FindControl("PMCounterSetup1$ttbCountRatio");
                if (ttbCountRatio != null)
                {
                    CheckTextBox(ttbCountRatio, GetUIResource("CountRatio"), CheckDataType.GreaterEqualZeroDecimal);
                    CurrentToolData.CountRatio = ttbCountRatio.Text.ToDecimal();
                }

                TextBox ttbTotalCount = (TextBox)FindControl("PMCounterSetup1$ttbTotalCount");
                if (ttbTotalCount != null)
                {
                    CheckTextBox(ttbTotalCount, GetUIResource("TotalCount"), CheckDataType.GreaterEqualZeroDecimal);
                    CurrentToolData.TotalCount = ttbTotalCount.Text.ToDecimal();
                }

                TextBox ttbScrapCount = (TextBox)FindControl("PMCounterSetup1$ttbScrapCount");
                if (ttbScrapCount != null)
                {
                    CheckTextBox(ttbScrapCount, GetUIResource("ScrapCount"), CheckDataType.GreaterEqualZeroDecimal);
                    CurrentToolData.ScrapCount = ttbScrapCount.Text.ToDecimal();
                }
                #endregion

                ToolInfo toolData = (ToolInfo)CurrentToolData.DeepCopy();
                bool bCheckPMSSetup = true;
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, this.ApplicationName);
                using (CimesTransactionScope cts = CimesTransactionScope.Create())
                {
                    //_UserDefineColumnSet.ModifyInfoRecordWithoutUpdate(toolData);
                    if (toolData.InfoState == InfoState.NewCreate)
                    {
                        toolData.InsertImmediately(this.User.Identity.Name, DBCenter.GetSystemTime());
                        LogCenter.LogToDB(toolData, LogCenter.LogIndicator.Default);

                        if (PMSSetupInfo.GetSetupData("TOOL", "Type", toolData.ToolType).Count == 0)
                            bCheckPMSSetup = false;

                        #region 新增[CST_TOOL_LIFE]資料
                        //取得刀具型態資料
                        var toolTypeData = ToolTypeInfo.GetToolTypeByType(ddlType.SelectedItem.Text).ChangeTo<ToolTypeInfoEx>();

                        //取得刀壽設定資料
                        var toolTypeLifeData = InfoCenter.GetBySID<CSTToolTypeLifeInfo>(ddlSupplier.SelectedValue);

                        //取得刀面數
                        int sideCount = 0;
                        if (int.TryParse(toolTypeData.SideCount, out sideCount) == false)
                        {
                            //刀具類型:{0}，刀面數量({1})設定有誤，請至刀具類型維護修改此數量!!
                            throw new Exception(RuleMessage.Error.C10151(toolTypeData.Type, toolTypeData.SideCount));
                        }

                        //依據刀具型態設定的刀面數來新增資料
                        for (int i = 0; i < sideCount; i++)
                        {
                            var newToolLife = InfoCenter.Create<CSTToolLifeInfo>();

                            //刀具名稱
                            newToolLife.ToolName = CurrentToolData.ToolName;
                            //刀壽次數
                            newToolLife.Life = toolTypeLifeData.Life;
                            //刀面
                            newToolLife.Head = (i + 1).ToString();

                            newToolLife.UseCount = 0;

                            newToolLife.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                            LogCenter.LogToDB(newToolLife, LogCenter.LogIndicator.Create(ActionType.Add, txnStamp.UserID, txnStamp.RecordTime));
                        }
                        #endregion

                        #region 更新命名規則
                        if (ExecuteNamingSQLList != null && ExecuteNamingSQLList.Count > 0)
                        {
                            DBCenter.ExecuteSQL(ExecuteNamingSQLList);
                        }
                        #endregion
                    }
                    else
                    {
                        if (toolData.UpdateImmediately(this.User.Identity.Name, DBCenter.GetSystemTime()) != 1)
                        {
                            throw new CimesException(TextMessage.Error.T00747(""));
                        }
                        LogCenter.LogToDB(toolData, LogCenter.LogIndicator.Create(ActionType.Set));

                        if (PMSSetupInfo.GetSetupData("TOOL", "Type", toolData.ToolType).Count == 0 && PMSSetupInfo.GetSetupData("TOOL", "ID", toolData.ToolName).Count == 0)
                            bCheckPMSSetup = false;

                    }

                    #region 新增/更新檢驗報告[CST_TOOL_REPORT]
                    _ToolReports.ForEach(toolReport =>
                    {
                        //註記檢驗報告LOG的狀態
                        ActionType actionType = new ActionType();

                        if (toolReport.InfoState == InfoState.NewCreate)
                        {
                            toolReport.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                            actionType = ActionType.Add;
                        }
                        else if (toolReport.InfoState == InfoState.Modified)
                        {
                            //更改資料
                            toolReport.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                            actionType = ActionType.Set;
                        }
                        else if (toolReport.InfoState == InfoState.Unchanged)
                        {

                        }

                        //紀錄歷史紀錄[CST_TOOL_REPORT_LOG]
                        if (actionType != ActionType.None)
                        {
                            LogCenter.LogToDB(toolReport, LogCenter.LogIndicator.Create(actionType, txnStamp.UserID, txnStamp.RecordTime));
                        }
                    });
                    #endregion

                    ToolAttributeSetup.ExcuteTransaction(toolData.ID);

                    if (bCheckPMSSetup == true)
                        PMCounterSetupInterface.SyncSettingToDB(toolData.ID, "ToolID", txnStamp);

                    //因刀具報表需求，所以在新進料時要AddComment，並將刀面及使用次數記錄在UDC06 & UDC07
                    if (CurrentToolData.InfoState == InfoState.NewCreate)
                    {
                        var toolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(CurrentToolData.ToolName);
                        var toolLife = toolLifeList.Find(p => p.Head == "1");

                        TMSTransaction.ModifyToolSystemAttribute(toolData, "USERDEFINECOL06", "1", txnStamp);
                        TMSTransaction.ModifyToolSystemAttribute(toolData, "USERDEFINECOL07", toolLife.UseCount.ToCimesString(), txnStamp);

                        if (_ReasonCategory == null)
                        {
                            throw new CimesException(RuleMessage.Error.C00053("CustomizeReason", "刀具進料"));
                        }

                        txnStamp.Remark = _ReasonCategory.Reason;
                        txnStamp.CategoryReasonCode = _ReasonCategory;
                        txnStamp.Description = "";
                        TMSTransaction.AddToolComment(toolData, txnStamp);
                    }

                    cts.Complete();
                }

                CurrentToolData = (ToolInfoEx)toolData.DeepCopy();
                BindDataByCurrentToolData();
                gvToolList.ResetCloneDataSource();
                //QueryAllTool();
                //001 
                AllTools = ToolInfoEx.GetToolByToolClass("CUTTER").OrderBy(p=>p.ToolName).ToList();
                    
                int index = AllTools.FindIndex(p => p.ID == CurrentToolData.ID);

                //int pageIndex = index / gvToolList.PageSize;
                int selectedIndex = index % gvToolList.PageSize;

                //換頁要放在bind資料前
                //指定選擇行放在bind資料後
                gvToolList.PageIndex = gvToolList.CurrentPageIndex;
                gvToolList.SetDataSource(AllTools, true);
                gvToolList.SelectedIndex = selectedIndex;

                // DisableTab();

                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00083(""));

                ClearFieldAndDisableTab();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void MultiToolData(int count)
        {
            if (!rbtDisable.Checked && !rbtEnable.Checked)
            {
                throw new CimesException(TextMessage.Error.T00841(lblStates.Text));
            }

            ExecuteNamingSQLList.Clear();

            //取得刀具名稱
            var naming = GetNamingRule("ToolNo", User.Identity.Name, count, null, new List<string>() { ddlType.SelectedItem.Text });

            TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, this.ApplicationName);
            using (CimesTransactionScope cts = CimesTransactionScope.Create())
            {

                for (int namingCount = 0; namingCount < count; namingCount++)
                {
                    var newToolData = InfoCenter.Create<ToolInfoEx>();
                    newToolData.ToolName = naming[namingCount];

                    //品名規格
                    newToolData.Specification = ttbToolTypeSpec.Text;

                    //單位別
                    newToolData.Department = ttbUnit.Text;

                    //供應商
                    newToolData.Vendor = ddlSupplier.SelectedItem.Text;

                    //採購產編
                    newToolData.Fortune = ttbPurchasingProd.Text;

                    //註記為庫房
                    newToolData.Location = "Warehouse";

                    newToolData.ToolClass = "CUTTER";

                    if (newToolData.InfoState == InfoState.NewCreate)
                    {
                        //取得刀具型態資料
                        var toolTypeData = ToolTypeInfo.GetToolTypeByType(ddlType.SelectedItem.Text).ChangeTo<ToolTypeInfoEx>();

                        //確認檢驗報告是否需要上傳
                        if (toolTypeData.InspectionFlag == "Y")
                        {
                            if (_ToolReports.Count == 0)
                            {
                                //刀具類型:{0} 必須上傳檢驗報告資料 !
                                throw new Exception(RuleMessage.Error.C10126(toolTypeData.Type));
                            }
                        }

                        //刀面
                        newToolData.Head = "1";
                        //身份
                        newToolData.Identity = CustomizeFunction.ToolIdentity.新品.ToString();
                        //維修次數
                        newToolData.MaintainCount = 0;

                        if (ToolInfo.GetToolByName(newToolData.ToolName) != null)
                        {
                            throw new CimesException(TextMessage.Error.T00710(newToolData.ToolName));
                        }
                    }

                    ToolAttributeSetup.ValidateCheck();

                    //儲存系統屬性
                    _SystemAttribute.SaveSystemAttribute(newToolData);

                    //預設ActiveFlag = "F"
                    if (newToolData.InfoState == InfoState.NewCreate)
                    {
                        newToolData.ActiveFlag = "F";
                    }

                    newToolData.UsingStatus = (rbtEnable.Checked ? UsingStatus.Enable : UsingStatus.Disable);
                    newToolData.ToolTypeSID = ddlType.SelectedItem.Value;
                    newToolData.ToolType = ddlType.SelectedItem.Text;
                    if (rbtEnable.Checked)
                    {
                        //若啟用，則ActiveFlag=T
                        newToolData.ActiveFlag = "T";
                    }
                    if (ddlState.SelectedItem != null && !ddlState.SelectedItem.Value.IsNullOrTrimEmpty())
                    {
                        newToolData.CurrentState = ddlState.SelectedItem.Text;
                        newToolData.ToolStateSID = ddlState.SelectedItem.Value;
                    }
                    newToolData.Description = ttbDescr.Text.Trim();

                    #region 將使用次數寫至ToolMaster
                    DropDownList ddlCountType = (DropDownList)FindControl("PMCounterSetup1$ddlCountType");
                    if (ddlCountType != null)
                        newToolData.CountType = ddlCountType.Text;

                    TextBox ttbCountRatio = (TextBox)FindControl("PMCounterSetup1$ttbCountRatio");
                    if (ttbCountRatio != null)
                    {
                        CheckTextBox(ttbCountRatio, GetUIResource("CountRatio"), CheckDataType.GreaterEqualZeroDecimal);
                        newToolData.CountRatio = ttbCountRatio.Text.ToDecimal();
                    }

                    TextBox ttbTotalCount = (TextBox)FindControl("PMCounterSetup1$ttbTotalCount");
                    if (ttbTotalCount != null)
                    {
                        CheckTextBox(ttbTotalCount, GetUIResource("TotalCount"), CheckDataType.GreaterEqualZeroDecimal);
                        newToolData.TotalCount = ttbTotalCount.Text.ToDecimal();
                    }

                    TextBox ttbScrapCount = (TextBox)FindControl("PMCounterSetup1$ttbScrapCount");
                    if (ttbScrapCount != null)
                    {
                        CheckTextBox(ttbScrapCount, GetUIResource("ScrapCount"), CheckDataType.GreaterEqualZeroDecimal);
                        newToolData.ScrapCount = ttbScrapCount.Text.ToDecimal();
                    }
                    #endregion

                    ToolInfo toolData = (ToolInfo)newToolData.DeepCopy();
                    bool bCheckPMSSetup = true;

                    //_UserDefineColumnSet.ModifyInfoRecordWithoutUpdate(toolData);
                    if (toolData.InfoState == InfoState.NewCreate)
                    {
                        toolData.InsertImmediately(this.User.Identity.Name, DBCenter.GetSystemTime());
                        LogCenter.LogToDB(toolData, LogCenter.LogIndicator.Default);

                        if (PMSSetupInfo.GetSetupData("TOOL", "Type", toolData.ToolType).Count == 0)
                            bCheckPMSSetup = false;

                        #region 新增[CST_TOOL_LIFE]資料
                        //取得刀具型態資料
                        var toolTypeData = ToolTypeInfo.GetToolTypeByType(ddlType.SelectedItem.Text).ChangeTo<ToolTypeInfoEx>();

                        //取得刀壽設定資料
                        var toolTypeLifeData = InfoCenter.GetBySID<CSTToolTypeLifeInfo>(ddlSupplier.SelectedValue);

                        //取得刀面數
                        int sideCount = 0;
                        if (int.TryParse(toolTypeData.SideCount, out sideCount) == false)
                        {
                            //刀具類型:{0}，刀面數量({1})設定有誤，請至刀具類型維護修改此數量!!
                            throw new Exception(RuleMessage.Error.C10151(toolTypeData.Type, toolTypeData.SideCount));
                        }

                        //依據刀具型態設定的刀面數來新增資料
                        for (int i = 0; i < sideCount; i++)
                        {
                            var newToolLife = InfoCenter.Create<CSTToolLifeInfo>();

                            //刀具名稱
                            newToolLife.ToolName = newToolData.ToolName;
                            //刀壽次數
                            newToolLife.Life = toolTypeLifeData.Life;
                            //刀面
                            newToolLife.Head = (i + 1).ToString();

                            newToolLife.UseCount = 0;

                            newToolLife.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                            LogCenter.LogToDB(newToolLife, LogCenter.LogIndicator.Create(ActionType.Add, txnStamp.UserID, txnStamp.RecordTime));
                        }
                        #endregion
                    }
                    else
                    {
                        if (toolData.UpdateImmediately(this.User.Identity.Name, DBCenter.GetSystemTime()) != 1)
                        {
                            throw new CimesException(TextMessage.Error.T00747(""));
                        }
                        LogCenter.LogToDB(toolData, LogCenter.LogIndicator.Create(ActionType.Set));

                        if (PMSSetupInfo.GetSetupData("TOOL", "Type", toolData.ToolType).Count == 0 && PMSSetupInfo.GetSetupData("TOOL", "ID", toolData.ToolName).Count == 0)
                            bCheckPMSSetup = false;

                    }

                    #region 新增/更新檢驗報告[CST_TOOL_REPORT] (一筆以上的刀具資料不可新增檢驗報告)
                    //_ToolReports.ForEach(toolReport =>
                    //{
                    //    //註記檢驗報告LOG的狀態
                    //    ActionType actionType = new ActionType();

                    //    if (toolReport.InfoState == InfoState.NewCreate)
                    //    {
                    //        toolReport.ToolName = newToolData.ToolName;
                    //        toolReport.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    //        actionType = ActionType.Add;
                    //    }
                    //    else if (toolReport.InfoState == InfoState.Modified)
                    //    {
                    //        //更改資料
                    //        toolReport.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    //        actionType = ActionType.Set;
                    //    }
                    //    else if (toolReport.InfoState == InfoState.Unchanged)
                    //    {

                    //    }

                    //    //紀錄歷史紀錄[CST_TOOL_REPORT_LOG]
                    //    if (actionType != ActionType.None)
                    //    {
                    //        LogCenter.LogToDB(toolReport, LogCenter.LogIndicator.Create(actionType, txnStamp.UserID, txnStamp.RecordTime));
                    //    }
                    //});
                    #endregion

                    ToolAttributeSetup.ExcuteTransaction(toolData.ID);

                    if (bCheckPMSSetup == true)
                        PMCounterSetupInterface.SyncSettingToDB(toolData.ID, "ToolID", txnStamp);

                    if (newToolData.InfoState == InfoState.NewCreate)
                    {
                        var toolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(newToolData.ToolName);
                        var toolLife = toolLifeList.Find(p => p.Head == "1");

                        TMSTransaction.ModifyToolSystemAttribute(toolData, "USERDEFINECOL06", "1", txnStamp);
                        TMSTransaction.ModifyToolSystemAttribute(toolData, "USERDEFINECOL07", toolLife.UseCount.ToCimesString(), txnStamp);

                        txnStamp.Remark = _ReasonCategory.Reason;
                        txnStamp.CategoryReasonCode = _ReasonCategory;
                        txnStamp.Description = "";
                        TMSTransaction.AddToolComment(toolData, txnStamp);
                    }

                    if (namingCount == count - 1)
                    {
                        CurrentToolData = (ToolInfoEx)toolData.DeepCopy();
                    }
                }

                #region 更新命名規則
                if (ExecuteNamingSQLList != null && ExecuteNamingSQLList.Count > 0)
                {
                    DBCenter.ExecuteSQL(ExecuteNamingSQLList);
                }
                #endregion

                cts.Complete();
            }

           
            BindDataByCurrentToolData();
            gvToolList.ResetCloneDataSource();
            //QueryAllTool();
            //001 
            AllTools = InfoCenter.GetList<ToolInfoEx>("SELECT * FROM MES_TOOL_MAST ORDER BY TOOLNAME");

            int index = AllTools.FindIndex(p => p.ID == CurrentToolData.ID);

            //int pageIndex = index / gvToolList.PageSize;
            int selectedIndex = index % gvToolList.PageSize;

            //換頁要放在bind資料前
            //指定選擇行放在bind資料後
            gvToolList.PageIndex = gvToolList.CurrentPageIndex;
            gvToolList.SetDataSource(AllTools, true);
            gvToolList.SelectedIndex = selectedIndex;

            _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00083(""));

            ClearFieldAndDisableTab();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFieldAndDisableTab();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ClearFieldAndDisableTab()
        {
            DisableTab();
            ClearData();
        }

        private void DisableTab()
        {
            foreach (CimesTabItem tab in rtsDetail.Tabs)
            {
                tab.Enabled = false;
            }
            foreach (CimesPageView view in rmpDetail.PageViews)
            {
                view.Enabled = false;
            }

            FileUpload1.Enabled = false;
            btnUpload.Enabled = false;
            btnPrint.Enabled = false;
        }

        private void EnableTab()
        {
            foreach (CimesTabItem tab in rtsDetail.Tabs)
            {
                tab.Enabled = true;
            }
            foreach (CimesPageView view in rmpDetail.PageViews)
            {
                view.Enabled = true;
            }

            FileUpload1.Enabled = true;
            btnUpload.Enabled = true;
            btnPrint.Enabled = true;
        }

        private void ClearData()
        {
            hlShowFile.NavigateUrl = "";
            hlShowFile.Text = "";

            ttbToolQty.Text = "";
            //ttbToolQty.Visible = false;
            //lblToolQty.Visible = false;
            ttbToolQty.Enabled = false;

            btnInspectionQuery.Visible = false;
            btnPictureQuery.Visible = false;

            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnPrint.Visible = false;
            //ttbToolName.ReadOnly = false;
            rbtEnable.Checked = false;
            rbtDisable.Checked = true;
            ttbToolName.Text = "";
            ttbDescr.Text = "";
            ddlState.ClearSelection();
            ddlType.ClearSelection();
            gvToolList.SelectedIndex = -1;

            ddlType.Enabled = true;
            ddlSupplier.Enabled = true;
            ddlSupplier.ClearSelection();

            btnInspectionQuery.Enabled = false;
            btnPictureQuery.Enabled = false;
            ttbToolTypeSpec.Text = "";
            ttbUnit.Text = "";
            ttbPurchasingProd.Text = "";

            ExecuteNamingSQLList = new List<SqlAgent>();

            lblInspectionReport.ForeColor = System.Drawing.Color.Black;
            if (_dsReport != null)
                _dsReport.Tables.Clear();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlType.SelectedItem != null && ddlType.SelectedItem.Text != "")
                {
                    // 依據對應的型態資料，帶出預設的記數資料
                    //PMCounterSetupInterface.SetDefaultDataByType(PMSType.ToolType, ddlType.SelectedItem.Value);

                    //取得供應商清單
                    GetSuppliers(ddlType.SelectedItem.Text);

                    //取得此刀具型態的資料
                    var toolType = ToolTypeInfo.GetToolTypeByType(ddlType.SelectedItem.Text).ChangeTo<ToolTypeInfoEx>();
                    ttbToolTypeSpec.Text = toolType.Specification;
                    ttbUnit.Text = toolType.Department;

                    //取得刀具圖檔清單
                    var toolTypePictures = CSTToolTypePictureInfo.GetDataListByToolType(toolType.Type);
                    if (toolTypePictures.Count > 0)
                    {
                        btnPictureQuery.Enabled = true;
                    }
                    else
                    {
                        btnPictureQuery.Enabled = false;
                    }
                    
                    btnUpload.Enabled = true;
                    FileUpload1.Enabled = true;

                    if (toolType.InspectionFlag == "Y")
                    {
                        lblInspectionReport.ForeColor = System.Drawing.Color.Red;
                        //ttbToolQty.Visible = false;
                        //lblToolQty.Visible = false;
                        ttbToolQty.ReadOnly = true;
                        ttbToolQty.Enabled = false;
                        ttbToolQty.Text = "1";
                    }
                    else
                    {
                        lblInspectionReport.ForeColor = System.Drawing.Color.Black;
                        //ttbToolQty.Visible = true;
                        //lblToolQty.Visible = true;
                        ttbToolQty.ReadOnly = false;
                        ttbToolQty.Enabled = true;
                        ttbToolQty.Text = "1";
                    }

                    ExecuteNamingSQLList.Clear();

                    //取得刀具名稱
                    var naming = GetNamingRule("ToolNo", User.Identity.Name, 1, null, new List<string>() { toolType.Type });
                    ttbToolName.Text = naming[0];
                }
                else
                {
                    ttbToolName.Text = "";

                    ddlSupplier.Items.Clear();

                    ExecuteNamingSQLList.Clear();
                    btnPictureQuery.Enabled = false;
                    btnUpload.Enabled = false;
                    FileUpload1.Enabled = false;

                    lblInspectionReport.ForeColor = System.Drawing.Color.Black;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 上傳檢驗報告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //如果刀具數量有顯示出來，則確認數值必須為正整數
                if (ttbToolQty.Enabled)
                {
                    var toolQty = ttbToolQty.MustInt(lblToolQty);
                    if (toolQty <= 0)
                    {
                        AjaxFocus(ttbToolQty);

                        //[01397]數量必須大於零！
                        throw new Exception(TextMessage.Error.T01397());
                    }

                    if (toolQty > 1)
                    {
                        //刀具數量必須等於1，才可執行上傳檢驗報 !
                        throw new Exception(RuleMessage.Error.C10169());
                    }
                }

                string toolName = "";

                if (CurrentToolData.InfoState == InfoState.NewCreate)
                {
                    ddlType.Must(lblType);
                    ttbToolName.Must(lblToolName);

                }

                toolName = ttbToolName.Text;

                if (!FileUpload1.HasFile)
                {
                    //[00854]請選擇檔案!!
                    throw new CimesException(TextMessage.Error.T00854());
                }

                //string fileName = DBCenter.GetSystemDateTime().ToString("yyyyMMddHHmmss") + "_" + FileUpload1.PostedFile.FileName;
                var splitString = FileUpload1.PostedFile.FileName.Split('.');

                //取得檔案名稱
                string sourceFileName = "";
                for (int i = 0; i < splitString.Length - 1; i++)
                {
                    sourceFileName += splitString[i];
                }

                //取得副檔名
                string extension = splitString[splitString.Length - 1];

                string fileName = sourceFileName + "_" + DBCenter.GetSystemDateTime().ToString("HHmmss") + "." + extension;

                WindowsImpersonationContext impersonationContext = null;

                //儲存資料夾
                string saveDirPath = _SaveInspectionData[0].Remark01;

                //使用者帳號
                string userName = _SaveInspectionData[0].Remark02;

                //使用者密碼
                string password = _SaveInspectionData[0].Remark03;

                //實際儲存檔案路徑
                string saveFilePath = saveDirPath + "\\" + toolName + "\\" + fileName;

                //如果使用者帳號不為空白才執行切換帳號功能
                if (userName.IsNullOrTrimEmpty() == false)
                {
                    //切換使用者
                    var tokenHandle = Impersonate(userName, password, "domain");
                    WindowsIdentity windowsIdentity = new WindowsIdentity(tokenHandle);
                    impersonationContext = windowsIdentity.Impersonate();
                }

                //判斷儲存路徑是否存在
                if (Directory.Exists(saveDirPath + "\\" + toolName) == false)
                {
                    //如果不存在，則新增一個資料夾路徑
                    Directory.CreateDirectory(saveDirPath + "\\" + toolName);
                }

                //儲存檔案
                FileUpload1.SaveAs(saveFilePath);

                if (impersonationContext != null)
                {
                    //切換回原身分
                    impersonationContext.Undo();
                }

                //新增一筆檢驗報告資料
                var toolReport = InfoCenter.Create<CSTToolReportInfo>();
                toolReport.ToolName = toolName;
                toolReport.FileName = saveFilePath;

                _ToolReports.Add(toolReport);

                //檔案名稱:{0}已上傳完成 !
                _ProgramInformationBlock.ShowMessage(RuleMessage.Hint.C10125(fileName));

                //依據檔案名稱，顯示檔案名稱的連結
                BindFileData(toolReport.FileName, fileName);

            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 檢驗報告查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInspectionQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //SecurityRightSID是為了讓Popup的視窗，ProgramInfoBlock的Caption能顯示主視窗的標題
                string src = string.Format("T008Report.aspx?SecurityRightSID={0}&ToolName={1}",
                    ProgramRightSID, CurrentToolData.ToolName);

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenDialog", String.Format("OpenjQueryDialog('{0}',1060,600);", src), true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 圖面查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPictureQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //SecurityRightSID是為了讓Popup的視窗，ProgramInfoBlock的Caption能顯示主視窗的標題
                string src = string.Format("T008Picture.aspx?SecurityRightSID={0}&ToolType={1}",
                    ProgramRightSID, ddlType.SelectedItem.Text);

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenDialog", String.Format("OpenjQueryDialog('{0}',1060,600);", src), true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 取得供應商清單
        /// </summary>
        /// <param name="toolType"></param>
        private void GetSuppliers(string toolType)
        {
            ddlSupplier.Items.Clear();
            var suppliers = CSTToolTypeLifeInfo.GetDataListByToolType(toolType).ToList();
            suppliers.ForEach(p => ddlSupplier.Items.Add(new ListItem(p.Supplier, p.ID)));
            if (suppliers.Count > 0)
            {
                ddlSupplier.Items.Insert(0, "");
            }

            ////TO DO ClassName待確認
            //var suppliers = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SQM_SupplierMaterialCustAsignType");
            //ddlSupplier.DataSource = suppliers;
            //ddlSupplier.DataTextField = "Remark01";
            //ddlSupplier.DataBind();
            //ddlSupplier.Items.Insert(0, "");

            //if (suppliers.Count == 0)
            //{
            //    //[00555]查無資料，請至系統資料維護新增類別{0}、項目{1}!
            //    throw new Exception(TextMessage.Error.T00555("SQM_SupplierMaterialCustAsignType", ""));
            //}
        }

        /// <summary>
        /// 取得系統控制設定的類別資料
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private List<WpcExClassItemInfo> GetExtendItemListByClass(string className)
        {
            var dataList = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks(className);

            if (dataList.Count == 0)
            {
                // [00466]系統控制設定，類別：[{0}]，項目：[{1}]未設定！
                throw new Exception(TextMessage.Error.T00466(className, ""));
            }

            return dataList;
        }

        /// <summary>
        /// 切換使用者權限
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private IntPtr Impersonate(string userName, string password, string domain)
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
            IntPtr tokenHandle = new IntPtr(0);
            tokenHandle = IntPtr.Zero;

            bool returnValue = LogonUser(userName, domain, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref tokenHandle);

            if (returnValue == false)
            {
                //切換使用者權限失敗(使用者:{0})！
                throw new Exception(RuleMessage.Error.C10072(userName));
            }

            return tokenHandle;
        }

        /// <summary>
        /// 取得Naming
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="user"></param>
        /// <param name="info"></param>
        /// <param name="lstArgs"></param>
        /// <returns></returns>
        private List<string> GetNamingRule(string ruleName, string user, int count, InfoBase info = null, List<string> lstArgs = null)
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
                var result = naming.GenerateNextIDs(count, info, lstArgs.ToArray(), user);
                ExecuteNamingSQLList = result.Second;
                return result.First.ToList();
            }
            else
            {
                var result = naming.GenerateNextIDs(count, lstArgs.ToArray(), user);
                ExecuteNamingSQLList = result.Second;
                return result.First.ToList();
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentToolData == null)
                {
                    throw new Exception(TextMessage.Error.T00060(""));
                }
                _dsReport.Tables.Clear();

                // 取得Report資料
                _dsReport = GetRunCardDataSource(CurrentToolData);
                _dsReport.AcceptChanges();

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    var printLotInfo = InfoCenter.Create<CSTToolLabelPrintLogInfo>();
                    printLotInfo.ToolName = CurrentToolData.ToolName;
                    printLotInfo.RuleName = ProgramRight;
                    printLotInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    cts.Complete();
                }

                if (_dsReport.Tables.Count > 0)
                {
                    string sPrintProgram = "/CustomizeRule/ToolRule/T022View.aspx";
                    string sHost = Request.Url.Host;
                    string sApplicationPath = Request.ApplicationPath;
                    string ReportPath = "http://" + sHost + sApplicationPath + sPrintProgram;
                    Session["T022View"] = _dsReport;
                    //開啟查詢工單頁面
                    string openPrintWindow = "window.open('" + ReportPath + "','pop','resizable: yes; status: no; scrollbars:no; menubar:no;toolbar:no;location:no;dialogLeft:10px;dialogTop:10px;dialogHeight:10px;dialogWidth:10px',false);";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), openPrintWindow, true);
                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        private DataSet GetRunCardDataSource(ToolInfo toolData)
        {
            #region 定義 LOTDATA 資料表
            DataTable dtToolData = toolData.CopyDataToTable("TOOLData");
            dtToolData.Columns.Add("ToolTypeDescr");
            #endregion            
            var toolTypeInfo = ToolTypeInfo.GetToolTypeByType(toolData.ToolType);
            dtToolData.Rows[0]["ToolTypeDescr"] = toolTypeInfo.Description;
            DataSet dsReportData = new DataSet();
            dsReportData.Tables.Add(dtToolData);

            return dsReportData;
        }

        /// <summary>
        /// 依據檔案名稱，顯示檔案名稱的連結
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="fileName">檔案名稱</param>
        public void BindFileData(string filePath, string fileName)
        {
            //取得路徑對應清單
            var _SaveReportData = GetExtendItemListByClass("SAIToolInspection");

            string sHost = Request.Url.Host;
            string sApplicationPath = Request.ApplicationPath;
            string picturePath = "http://" + sHost + sApplicationPath + "/ToolReport";
            string urlPasth = filePath.Replace(_SaveReportData[0].Remark01, picturePath).Replace(@"\", "/");

            //顯示已上傳的檔案名稱
            string viewFileName = fileName;

            //限制顯示的檔案名稱長度，如果長度大於20字，則20字後面代...
            if (viewFileName.Length > 20)
            {
                viewFileName = fileName.Substring(0, 20) + "...";
            }
            hlShowFile.NavigateUrl = urlPasth;
            hlShowFile.Text = viewFileName;
        }
    }
}
