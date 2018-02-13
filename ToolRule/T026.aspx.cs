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

namespace ciMes.Tool.Admin
{
    public partial class T026 : CimesBasePage
    {
        #region PROPERTY
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
        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }

        }

        private SystemAttribute _SystemAttribute
        {
            get { return SystemAttribute1 as SystemAttribute; }

        }

        #endregion

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
                this.ID = "T026";
                if (!IsPostBack)
                {
                    // Check User Right ,防止User直接打網址進入
                    if (!UserProfileInfo.CheckUserRight(User.Identity.Name, ProgramRight))
                    {
                        HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                    }

                    AllTools = new List<ToolInfoEx>();

                    QueryAllTool();
                    UIControl();
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
            var allToolTypeData = ToolTypeInfoEx.GetToolTypeByToolClass("DIE").OrderBy(p => p.Type).ToList();
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

        private void QueryAllTool()
        {
            AllTools = ToolInfoEx.GetToolByToolClass("DIE");
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

                if (CurrentToolData .ActiveFlag == "T")     //ActiveFlag的判斷,若曾經啟用過則不可刪除
                {
                    throw new Exception(TextMessage.Error.T00714());
                }
                
                using (CimesTransactionScope cts = CimesTransactionScope.Create())
                {
                    CurrentToolData.DeleteFromDB();
                    AttributeAttributeInfo.DeleteByObjectSIDAndDataClass(CurrentToolData.ID, "ToolAttribute", this.User.Identity.Name, DBCenter.GetSystemTime());
                    LogCenter.LogToDB(CurrentToolData, LogCenter.LogIndicator.Create(ActionType.Remove, User.Identity.Name, DBCenter.GetSystemTime()));
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
            ttbToolName.ReadOnly = true;
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

            var li = ddlType.Items.FindByValue(CurrentToolData.ToolTypeSID);
            if (li != null)
            {
                ddlType.ClearSelection();
                li.Selected = true;
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
            //UserDefineColumnSet1.SetControl(BindType.Tool, CurrentToolData);
            btnSave.Visible = true;
            btnCancel.Visible = true;
            Action = "U";

            rtsDetail.PurgeToSetByIndex(0);

            //顯示系統屬性
            _SystemAttribute.LoadSystemAttribute(CurrentToolData, "ToolSystemAttribute");
        }

        protected void gvToolList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                ClearData();
                EnableTab();
                CurrentToolData = AllTools[gvToolList.Rows[e.NewSelectedIndex].DataItemIndex];
                BindDataByCurrentToolData();
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
                ToolAttributeSetup.BindData("ToolCutterAttribute", CurrentToolData.ID);
                PMCounterSetupInterface.BindUserControl(PMSType.ToolID, CurrentToolData.ID);
                //UserDefineColumnSet1.SetControl(BindType.Tool, null);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                Action = "A";
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
                ttbToolName.Must(lblToolName);
                ddlType.Must(lblType);

                if (!rbtDisable.Checked && !rbtEnable.Checked)
                {
                    throw new CimesException(TextMessage.Error.T00841(lblStates.Text));
                }
         

                CurrentToolData.ToolName = ttbToolName.Text.Trim();
                if (CurrentToolData.InfoState == InfoState.NewCreate)
                {
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
                    CurrentToolData.Location = "Warehouse";
                    CurrentToolData.ToolClass = "DIE";
                }

                CurrentToolData.UsingStatus = (rbtEnable.Checked ? UsingStatus.Enable : UsingStatus.Disable);
                CurrentToolData.ToolTypeSID = ddlType.SelectedItem.Value;
                CurrentToolData.ToolType = ddlType.SelectedItem.Text;
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
                    //UserDefineColumnSet1.ModifyInfoRecordWithoutUpdate(toolData);
                    if (toolData.InfoState == InfoState.NewCreate)
                    {
                        toolData.InsertImmediately(this.User.Identity.Name, DBCenter.GetSystemTime());
                        LogCenter.LogToDB(toolData, LogCenter.LogIndicator.Default);

                        if (PMSSetupInfo.GetSetupData("TOOL", "Type", toolData.ToolType).Count == 0)
                            bCheckPMSSetup = false;
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

                    ToolAttributeSetup.ExcuteTransaction(toolData.ID);

                    if (bCheckPMSSetup == true)
                        PMCounterSetupInterface.SyncSettingToDB(toolData.ID, "ToolID", txnStamp);
                    cts.Complete();
                }

                CurrentToolData = (ToolInfoEx)toolData.DeepCopy();
                BindDataByCurrentToolData();
                gvToolList.ResetCloneDataSource();
                //QueryAllTool();
                //001 
                AllTools = ToolInfoEx.GetToolByToolClass("DIE");

                int index = AllTools.FindIndex(p => p.ID == CurrentToolData.ID);

                //int pageIndex = index / gvToolList.PageSize;
                int selectedIndex = index % gvToolList.PageSize;

                //換頁要放在bind資料前
                //指定選擇行放在bind資料後
                gvToolList.PageIndex = gvToolList.CurrentPageIndex;
                gvToolList.SetDataSource(AllTools, true);
                gvToolList.SelectedIndex = selectedIndex;

                DisableTab();

                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00083(""));
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
        }

        private void ClearData()
        {
            btnSave.Visible = false;
            btnCancel.Visible = false;
            ttbToolName.ReadOnly = false;
            rbtEnable.Checked = false;
            rbtDisable.Checked = true;
            ttbToolName.Text = "";
            ttbDescr.Text = "";
            ddlState.ClearSelection();
            ddlType.ClearSelection();
            gvToolList.SelectedIndex = -1;
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlType.SelectedItem != null && ddlType.SelectedItem.Text != "")
                {
                    // 依據對應的型態資料，帶出預設的記數資料
                    //PMCounterSetupInterface.SetDefaultDataByType(PMSType.ToolType, ddlType.SelectedItem.Value);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}
