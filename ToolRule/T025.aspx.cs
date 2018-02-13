/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：CheerHuang

功能說明：模治具型態維護

------------------------------------------------------------------
*   日期            作者            變更內容
*   2017/11/28      CheerHuang       初版
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Web.UI;
using ciMes.Web.Common;
using ciMes.Web.Common.UserControl;

namespace ciMes.Tool.Admin
{
    /// <summary>
    /// ToolType 的摘要描述。
    /// </summary>
    public partial class T025 : CimesBasePage
    {
        #region property

        protected string DelConfirmStr = "";

        string sql;
        bool DelFlag = false;

        /// <summary>
        /// 指定項目清單的排序方向。
        /// </summary>
        SortDirection GridViewSortDirection
        {
            get
            {
                if (this["GridViewSortDirection"] != null)
                {
                    return (SortDirection)this["GridViewSortDirection"];
                }
                else
                {
                    return SortDirection.Ascending;
                }
            }
            set
            {
                this["GridViewSortDirection"] = value;
            }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }

        }

        private AttributeSetupGrid uscAttributeSetupGrid
        {
            get
            {
                return (AttributeSetupGrid)AttributeSetupGrid1;
            }
        }

        List<ToolTypeInfo> ToolTypeList
        {
            get
            {
                return (List<ToolTypeInfo>)this["ToolTypeList"];
            }
            set
            {
                this["ToolTypeList"] = value;
            }
        }
        private SystemAttribute _SystemAttribute
        {
            get { return SystemAttribute1 as SystemAttribute; }

        }

        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Check User Right ,防止User直接打網址進入
                if (!UserProfileInfo.CheckUserRight(User.Identity.Name, _ProgramInformationBlock.ProgramRight))
                {
                    HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                    return;
                }
                // 修改警告訊息多國語言化
                DelConfirmStr = TextMessage.Error.T00771();
                ttbToolType.Attributes["onkeypress"] = " if (window.event.keyCode==13) {btnQuery.click();return false; }";

                //修改多國語言資源檔讀取方式
                if (!IsPostBack)
                {
                    ToolTypeList = null;
                    UIControl();
                    QueryData();
                    ttbToolType.Focus();
                }
                else
                {
                    gvQuery.DataSource = ToolTypeList;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        private void UIControl()
        {
            var tabPMCounter = rtsDetail.FindTabByValue("PMCounter");
            if (CimesGenuine.RegisterLicense.PM.AccessAuthority == EAccessAuthority.FullAccess)
            {
                if (tabPMCounter != null)
                    tabPMCounter.Visible = true;
            }
            else
            {
                if (tabPMCounter != null)
                    tabPMCounter.Visible = false;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            CurrentUpdatePanel = UpdatePanel1;
            base.OnInit(e);
        }

        private void QueryData()
        {
            //若有輸入查詢條件,且使用Like方式Query資料
            sql = "SELECT * FROM MES_TOOL_TYPE WHERE TOOLCLASS = 'DIE'  ";
            if (ttbToolType.Text.Trim() != "")
            {
                sql += " AND UPPER(TYPE) LIKE UPPER(#[STRING]) ORDER BY TYPE";
                ToolTypeList = InfoCenter.GetList<ToolTypeInfo>(sql, ttbToolType.Text.Trim() + "%");
            }
            else
            {
                sql += " ORDER BY TYPE";
                ToolTypeList = InfoCenter.GetList<ToolTypeInfo>(sql);
            }

            if (ToolTypeList.Count == 0)    //資料找不到
            {
                gvQuery.SetDataSource(null, true);
                throw new Exception(TextMessage.Error.T00060(""));
            }
            gvQuery.SetDataSource(ToolTypeList, true);
            gvQuery.EditItemIndex = -1;
            gvQuery.CurrentPageIndex = 0;
            uscAttributeSetupGrid.ClearField();
        }

        protected void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                QueryData();
                DisableSubControl();
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        protected void gvQuery_CancelCommand(object source, GridViewCancelEditEventArgs e)
        {
            try
            {
                if (gvQuery.EditItemIndex == -1)
                    return;

                removeListAddandDeletRow();
                gvQuery.SetDataSource(ToolTypeList);

                gvQuery.EditItemIndex = -1;
                gvQuery.DataBind();
                DisableSubControl();
                uscAttributeSetupGrid.ClearField();
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        protected void gvQuery_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int iIndex = gvQuery.Rows[e.RowIndex].DataItemIndex;
                
                //避免新增未按下儲存就按其他資料的編輯

                string SID, Tag;
                if (ToolTypeList[iIndex].ActiveFlag == "T")     //ActiveFlag的判斷,若曾經啟用過則不可刪除
                {
                    removeListAddandDeletRow();
                    gvQuery.SetDataSource(ToolTypeList);
                    gvQuery.DataBind();
                    throw new Exception(TextMessage.Error.T00714());
                }
                else
                {
                    //  Delete Data 	
                    SID = ToolTypeList[iIndex].ID;
                    //避免若新增模式時,按下刪除的button,不作動作
                    if (SID.Length == 0)
                        return;

                    using (CimesTransactionScope cts = CimesTransactionScope.Create())
                    {
                        if (ToolTypeList[iIndex].DeleteFromDB() == 0)
                            throw new Exception(TextMessage.Error.T00710(""));

                        AttributeAttributeInfo.DeleteByObjectSIDAndDataClass(SID, "ToolTypeAttribute", this.User.Identity.Name, DBCenter.GetSystemTime());

                        cts.Complete();
                    }
                    _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00349(ToolTypeList[iIndex].Type));

                    ToolTypeList.RemoveAt(iIndex);

                    removeListAddandDeletRow();
                    gvQuery.SetDataSource(ToolTypeList);

                    gvQuery.EditItemIndex = -1;
                    if (gvQuery.CurrentPageIndex > ((ToolTypeList.Count - 1) / gvQuery.PageSize))
                        gvQuery.CurrentPageIndex = (ToolTypeList.Count - 1) / gvQuery.PageSize;
                    gvQuery.DataBind();
                    DisableSubControl();
                    uscAttributeSetupGrid.ClearField();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        protected void gvQuery_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                //避免新增未按下儲存就按其他資料的編輯
                if (gvQuery.EditItemIndex == -1)
                    return;

                int index = gvQuery.Rows[e.NewEditIndex].DataItemIndex;

                if (ToolTypeList[0].InfoState == InfoState.NewCreate)   //若為新增模式,則按下取消,將該筆刪除
                {
                    ToolTypeList[0].SetInfoStateToDeleted();
                    //ToolTypeList.RemoveAt(0);
                    DelFlag = true;
                }
                gvQuery.EditItemIndex = -1;
                gvQuery.DataBind();
                DisableSubControl();
                uscAttributeSetupGrid.ClearField();
               
                gvQuery.EditItemIndex = e.NewEditIndex;

                gvQuery.DataBind();

                uscAttributeSetupGrid.BindData("ToolTypeAttribute", ToolTypeList[index].ID);

                //顯示系統屬性
                _SystemAttribute.LoadSystemAttribute(ToolTypeList[index], "ToolDieTypeSystemAttribute");

                EnableSubControl();
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }



        void EnableSubControl()
        {
            foreach (CimesTabItem tab in rtsDetail.Tabs)
            {
                tab.Enabled = true;
            }
            foreach (CimesPageView view in rmpDetail.PageViews)
            {
                view.Enabled = true;
            }
            rtsDetail.PurgeToSetByIndex(0);
        }

        void DisableSubControl()
        {
            foreach (CimesTabItem tab in rtsDetail.Tabs)
            {
                tab.Enabled = false;
            }
            foreach (CimesPageView view in rmpDetail.PageViews)
            {
                view.Enabled = false;
            }
            rtsDetail.PurgeToSetByIndex(0);
        }

        protected void gvQuery_RowCreated(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                    if (ToolTypeList[e.Row.DataItemIndex].InfoState == InfoState.Unchanged)
                    {
                        btnDelete.Attributes["OnMouseOut"] = "bDelete=false;";
                        btnDelete.Attributes["OnClick"] = "bDelete=true; DelKey='" + (ToolTypeList[e.Row.DataItemIndex].Type) + "';";
                    }

                    if (ToolTypeList[e.Row.DataItemIndex].ActiveFlag.ToUpper() == "T")
                    {
                        btnDelete.Visible = false;
                    }

                    if (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Alternate | DataControlRowState.Edit))
                    {
                        e.Row.Cells[1].Width = new Unit("150px");
                        ((Button)e.Row.FindControl("btnUpdate")).Attributes["id"] = "btnSave";
                        for (int i = 2; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].Attributes["onkeypress"] = " if (window.event.keyCode==13) {btnSave.click();return false; }";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        protected void gvQuery_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var ToolTypeInfo1 = ToolTypeList[e.Row.DataItemIndex];
                    e.Row.Visible = !(ToolTypeInfo1.InfoState == InfoState.Deleted); //刪除狀態先隱藏，換頁、刪除、儲存或取消時再移掉

                    //編輯模式
                    if (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Alternate | DataControlRowState.Edit))
                    {
                        TextBox ttbType = (TextBox)e.Row.FindControl("ttbType");
                        RadioButton rbtEnable = (RadioButton)e.Row.FindControl("rbEnable");
                        RadioButton rbtDisable = (RadioButton)e.Row.FindControl("rbDisable");
                        TextBox ttbDescr = (TextBox)e.Row.FindControl("ttbDescription");

                        if (ToolTypeList[e.Row.DataItemIndex].InfoState == InfoState.NewCreate) //新增模式
                            ttbType.Enabled = true;
                        else
                            ttbType.Enabled = false;
                        if (ToolTypeList[e.Row.DataItemIndex].Status == "Enable")
                            rbtEnable.Checked = true;
                        else
                            rbtDisable.Checked = true;
                        ttbType.Text = ToolTypeList[e.Row.DataItemIndex].Type;
                        ttbDescr.Text = ToolTypeList[e.Row.DataItemIndex].Description;
                    }
                    else     //查詢模式
                    {
                        Label lblType = (Label)e.Row.FindControl("lblType");
                        Label lblDescription = (Label)e.Row.FindControl("lblDescription");
                        Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                        lblType.Text = ToolTypeList[e.Row.DataItemIndex].Type;
                        if (ToolTypeList[e.Row.DataItemIndex].Status == "Enable")
                            lblStatus.Text = GetUIResource("Enable");
                        else
                            lblStatus.Text = GetUIResource("Disable");

                        lblDescription.Text = ToolTypeList[e.Row.DataItemIndex].Description;
                    }
                }
            }

            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        protected void gvQuery_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int index = gvQuery.Rows[e.RowIndex].DataItemIndex;
                var Mod = ToolTypeList[index].InfoState;
                string ToolType = ((TextBox)gvQuery.Rows[e.RowIndex].FindControl("ttbType")).Text;
                if (Mod == InfoState.NewCreate)         //表示為新增模式
                {
                    if (ToolType.Length == 0 || ToolType.IndexOf(" ") != -1)   //檢查名稱必需輸入,且字串中不可有空白
                        throw new Exception(TextMessage.Error.T00053(GetUIResource("ToolType")));

                    InsertToolType(e.RowIndex);
                }
                else
                {
                    UpdateToolType(e.RowIndex);
                }
                //儲存完後將應該刪除的info移掉
                removeListAddandDeletRow();
                //-----------------------------------------------------------
                gvQuery.EditItemIndex = -1;
                gvQuery.SetDataSource(ToolTypeList, true);
                uscAttributeSetupGrid.ClearField();
                DisableSubControl();
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        protected void removeListAddandDeletRow()
        {
            for (int j = ToolTypeList.Count - 1; j >= 0; j--)
            {
                if (ToolTypeList[j].InfoState == InfoState.Deleted || ToolTypeList[j].InfoState == InfoState.NewCreate)
                {
                    ToolTypeList.RemoveAt(j);
                }
            }
        }
        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ToolTypeList == null)
                {
                    ToolTypeList = new List<ToolTypeInfo>();
                }
                //若第一筆資料模式仍是新增,則不再新增空白列
                if (ToolTypeList.Count > 0)
                    if (ToolTypeList[0].InfoState == InfoState.NewCreate) return;

                //新增一筆空白在第一列

                gvQuery.CurrentPageIndex = 0;

                ToolTypeList.Insert(0, InfoCenter.Create<ToolTypeInfo>());
                gvQuery.SetDataSource(ToolTypeList, true);
                gvQuery.EditItemIndex = 0;
                gvQuery.DataBind();

                uscAttributeSetupGrid.BindData("ToolTypeAttribute", "");

                //顯示系統屬性
                _SystemAttribute.LoadSystemAttribute(ToolTypeList[0], "ToolDieTypeSystemAttribute");

                EnableSubControl();
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        private void InsertToolType(int rowIndex)
        {
            int index = gvQuery.Rows[rowIndex].DataItemIndex;
            ArrayList SaveList = new ArrayList();
            string sToolType = ((TextBox)gvQuery.Rows[rowIndex].FindControl("ttbType")).Text;
            RadioButton rbtEnable = (RadioButton)gvQuery.Rows[rowIndex].FindControl("rbEnable");
            TextBox ttbDescr = (TextBox)gvQuery.Rows[rowIndex].FindControl("ttbDescription");

            //檢查是否有相同名稱存在的資料
            sql = "SELECT 1 FROM MES_TOOL_TYPE WHERE TYPE = #[STRING]";
            DataView dvCheck = DBCenter.GetDataTable(sql, sToolType).DefaultView;// Query.DoQuery(sql);
            if (dvCheck != null && dvCheck.Count > 0)
                throw new Exception(TextMessage.Error.T00710(sToolType));

            uscAttributeSetupGrid.ValidateCheck();
            //新增資料庫
            ToolTypeInfo toolTypeData = InfoCenter.Create<ToolTypeInfo>();

            toolTypeData.Type = sToolType;
            toolTypeData.Description = ttbDescr.Text;
            toolTypeData["TOOLCLASS"] = "DIE";
            toolTypeData["UPDATETIME"] = DBCenter.GetSystemTime();
            toolTypeData["USERID"] = this.User.Identity.Name;
            if (rbtEnable.Checked == true)
            {
                toolTypeData.Status = "Enable";
                toolTypeData.ActiveFlag = "T";
            }
            else
            {
                toolTypeData.Status = "Disable";
                toolTypeData.ActiveFlag = "F";
            }

            //儲存系統屬性
            _SystemAttribute.SaveSystemAttribute(toolTypeData);
            TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, this.ApplicationName);
            using (CimesTransactionScope cts = CimesTransactionScope.Create())
            {
                toolTypeData.InsertToDB();
                uscAttributeSetupGrid.ExcuteTransaction(toolTypeData.ID);
                cts.Complete();
            }

            _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00695(sToolType));
            ToolTypeList[index] = toolTypeData;
        }

        private void UpdateToolType(int rowIndex)
        {
            int index = gvQuery.Rows[rowIndex].DataItemIndex;
            string sToolType = ((TextBox)gvQuery.Rows[rowIndex].FindControl("ttbType")).Text;
            RadioButton rbtEnable = (RadioButton)gvQuery.Rows[rowIndex].FindControl("rbEnable");
            TextBox ttbDescr = (TextBox)gvQuery.Rows[rowIndex].FindControl("ttbDescription");
            uscAttributeSetupGrid.ValidateCheck();

            var toolTypeData = (ToolTypeInfo)ToolTypeList[index].DeepCopy();
            toolTypeData.Description = ttbDescr.Text;
            toolTypeData["UPDATETIME"] = DBCenter.GetSystemTime();
            toolTypeData["USERID"] = Page.User.Identity.Name;
            if (rbtEnable.Checked == true)
            {
                toolTypeData.Status = "Enable";
                toolTypeData.ActiveFlag = "T";
            }
            else
            {
                toolTypeData.Status = "Disable";
            }

            //儲存系統屬性
            _SystemAttribute.SaveSystemAttribute(toolTypeData);
            TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, this.ApplicationName);
            using (CimesTransactionScope cts = CimesTransactionScope.Create())
            {
                if (toolTypeData.UpdateToDB() == 0)
                {
                    throw new Exception(TextMessage.Error.T00747(""));
                }

                uscAttributeSetupGrid.ExcuteTransaction(toolTypeData.ID, toolTypeData.UserID, toolTypeData.UpdateTime);

                cts.Complete();
            }

            _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00083(sToolType));
            ToolTypeList[index] = toolTypeData;
        }

        protected void gvQuery_PageIndexChanging(object source, GridViewPageEventArgs e)
        {
            try
            {
                //將應該刪除的info移掉
                removeListAddandDeletRow();
                gvQuery.SetDataSource(ToolTypeList);
                gvQuery.EditItemIndex = -1;
                if (e.NewPageIndex > ((ToolTypeList.Count - 1) / gvQuery.PageSize))
                    gvQuery.CurrentPageIndex = (ToolTypeList.Count - 1) / gvQuery.PageSize;
                else
                    gvQuery.CurrentPageIndex = e.NewPageIndex;

                gvQuery.DataBind();
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        protected void gvQuery_PreRender(object sender, System.EventArgs e)
        {
            try
            {
                ShowPageTotal();
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        private void ShowPageTotal()
        {
            if (gvQuery.Items.Count == 0)
                return;
            TableRow dgPager = new TableRow();

            int PagerIndex = ((Table)gvQuery.Controls[0]).Rows.Count - 1;
            if (PagerIndex > 0)
            {
                dgPager = ((Table)gvQuery.Controls[0]).Rows[PagerIndex];
                Label lblCount = new Label();
                Literal ltrSpace = new Literal();
                ltrSpace.Text = " ";
                lblCount.Text = " Total : " + Convert.ToString(gvQuery.PageCount) + " Page(s)";
                lblCount.ForeColor = Color.Black;
                dgPager.Cells[0].Controls.Add(ltrSpace);
                dgPager.Cells[0].Controls.Add(lblCount);
                dgPager.Cells[0].Attributes["colspan"] = gvQuery.Columns.Count.ToString();
            }
        }

        protected void gvQuery_SortCommand(object source, Ares.Cimes.IntelliService.Web.UI.CimesSortedArgs e)
        {
            try
            {               
                removeListAddandDeletRow();
                gvQuery.SetDataSource(ToolTypeList);
                gvQuery.EditItemIndex = -1;

                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    GridViewSortDirection = SortDirection.Descending;
                    ToolTypeList = ToolTypeList.OrderByDescending(p => p[e.CimesSortField].ToString()).ToList();
                }
                else
                {
                    GridViewSortDirection = SortDirection.Ascending;
                    ToolTypeList = ToolTypeList.OrderBy(p => p[e.CimesSortField].ToString()).ToList();
                }

                gvQuery.DataBind();
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }
        protected void gvQuery_DataSourceChanged(object sender, Ares.Cimes.IntelliService.Web.UI.CimesDataSourceEventArgs e)
        {

        }
    }
}
