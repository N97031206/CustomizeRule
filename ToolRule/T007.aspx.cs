
//  資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  
//  版權所有、禁止複製
//
//  模組：Tool
//  程式命名：ToolType.aspx
//
//  說明：

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
using System.IO;
using System.Security.Principal;
using System.Web;

namespace CustomizeRule.ToolRule
{
    /// <summary>
    /// ToolType 的摘要描述。
    /// </summary>
    public partial class T007 : CimesBasePage
    {
        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        #region PROPERTY
        /// <summary>
        /// 儲存路徑資料
        /// </summary>
        private List<WpcExClassItemInfo> _SavePicturedData
        {
            get { return this["_SavePicturedData"] as List<WpcExClassItemInfo>; }
            set { this["_SavePicturedData"] = value; }
        }

        /// <summary>
        /// 選擇的刀具型態
        /// </summary>
        private string _ToolType
        {
            get { return this["_ToolType"] as string; }
            set { this["_ToolType"] = value; }
        }

        /// <summary>
        /// 原始的刀壽設定清單
        /// </summary>
        private List<CSTToolTypeLifeInfo> _SourceToolTypeLifes
        {
            get { return this["_SourceToolTypeLifes"] as List<CSTToolTypeLifeInfo>; }
            set { this["_SourceToolTypeLifes"] = value; }
        }

        /// <summary>
        /// 修改過的刀壽設定清單
        /// </summary>
        private List<CSTToolTypeLifeInfo> _ModifyToolTypeLifes
        {
            get { return this["_ModifyToolTypeLifes"] as List<CSTToolTypeLifeInfo>; }
            set { this["_ModifyToolTypeLifes"] = value; }
        }

        /// <summary>
        /// 原始的刀具類型圖檔
        /// </summary>
        private List<CSTToolTypePictureInfo> _SourceToolTypePictures
        {
            get { return this["_SourceToolTypePictures"] as List<CSTToolTypePictureInfo>; }
            set { this["_SourceToolTypePictures"] = value; }
        }

        /// <summary>
        /// 修改過的刀具類型圖檔
        /// </summary>
        private List<CSTToolTypePictureInfo> _ModifyToolTypePictures
        {
            get { return this["_ModifyToolTypePictures"] as List<CSTToolTypePictureInfo>; }
            set { this["_ModifyToolTypePictures"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }

        }

        private List<HttpPostedFile> _HttpPostedFiles
        {
            get { return this["_HttpPostedFiles"] as List<HttpPostedFile>; }
            set { this["_HttpPostedFiles"] = value; }
        }

        private SystemAttribute _SystemAttribute
        {
            get { return SystemAttribute1 as SystemAttribute; }

        }

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

        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Check User Right ,防止User直接打網址進入
                //ID = "ToolTypeR4";
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

            //取得供應商清單
            GetSuppliers();

            _ToolType = "";
        }

        protected override void OnInit(EventArgs e)
        {
            CurrentUpdatePanel = UpdatePanel1;
            base.OnInit(e);
        }

        private void QueryData()
        {
            //若有輸入查詢條件,且使用Like方式Query資料
            sql = "SELECT * FROM MES_TOOL_TYPE WHERE TOOLCLASS = 'CUTTER' ";
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

                //取得路徑對應清單
                _SavePicturedData = GetExtendItemListByClass("SAIToolTypeImage");

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
                _SystemAttribute.LoadSystemAttribute(ToolTypeList[index], "ToolCutterTypeSystemAttribute");

                EnableSubControl();

                _ToolType = ToolTypeList[index].Type;

                //初始化刀壽及圖檔設定資料
                LoadControlDefaultToolLifeAndPicture(_ToolType);
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

                //確認刀壽是否有設定
                if (_ModifyToolTypeLifes.Count == 0)
                {
                    //刀壽設定分頁中，請新增一筆資料!
                    throw new Exception(RuleMessage.Error.C10158());
                }

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

                //取得路徑對應清單
                _SavePicturedData = GetExtendItemListByClass("SAIToolTypeImage");

                //新增一筆空白在第一列
                gvQuery.CurrentPageIndex = 0;

                ToolTypeList.Insert(0, InfoCenter.Create<ToolTypeInfo>());
                gvQuery.SetDataSource(ToolTypeList, true);
                gvQuery.EditItemIndex = 0;
                gvQuery.DataBind();

                uscAttributeSetupGrid.BindData("ToolTypeAttribute", "");

                //顯示系統屬性
                _SystemAttribute.LoadSystemAttribute(ToolTypeList[0], "ToolCutterTypeSystemAttribute");

                EnableSubControl();

                //初始化刀壽及圖檔設定資料
                LoadControlDefaultToolLifeAndPicture("");
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
            toolTypeData["TOOLCLASS"] = "CUTTER";
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

                //儲存刀壽
                SaveToolLife(sToolType, txnStamp.UserID, txnStamp.RecordTime);

                //儲存刀具圖檔
                SaveToolPicture(sToolType, txnStamp.UserID, txnStamp.RecordTime);

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

                //處理刀壽
                SaveToolLife(sToolType, txnStamp.UserID, txnStamp.RecordTime);

                //儲存刀具圖檔
                SaveToolPicture(sToolType, txnStamp.UserID, txnStamp.RecordTime);

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
                //若為第一筆為新增模式,則按下該排序按鈕
                //if (ToolTypeList[0].InfoState == InfoState.NewCreate)
                //{
                //    ToolTypeList.RemoveAt(0);
                //}
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

        /// <summary>
        /// 新增一筆刀壽設定資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToolLifeAdd_Click(object sender, System.EventArgs e)
        {
            try
            {

                //確認供應商是否有選擇
                ddlSupplier.Must(lblSupplier);

                //確認刀壽是否為正整數
                ttbToolLifeCount.MustInt(lblToolLifeCount);
                int lifeCount = Convert.ToInt32(ttbToolLifeCount.Text);
                if (lifeCount <= 0)
                {
                    //[00916]輸入值必須為數字且必須大於等於0!!
                    throw new Exception(TextMessage.Error.T00916());
                }

                //確認供應商資料是否已經新增
                _ModifyToolTypeLifes.ForEach(toolTypeLife =>
                {
                    if (toolTypeLife.Supplier == ddlSupplier.SelectedItem.Text)
                    {
                        //供應商:{0} 資料已存在!
                        throw new Exception(RuleMessage.Error.C10120(ddlSupplier.SelectedItem.Text));
                    }
                });

                //新增一筆刀壽資料
                var newToolLife = InfoCenter.Create<CSTToolTypeLifeInfo>();
                newToolLife.Supplier = ddlSupplier.SelectedItem.Text;
                newToolLife.Life = Convert.ToDecimal(ttbToolLifeCount.Text);
                newToolLife.ToolType = _ToolType;

                _ModifyToolTypeLifes.Insert(0, newToolLife);

                gvToolLife.SetDataSource(_ModifyToolTypeLifes, true);

                //清除資料
                ddlSupplier.ClearSelection();
                ttbToolLifeCount.Text = "";
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        /// <summary>
        /// 儲存一筆刀壽設定資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToolLifeSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                ////取得登入者資訊
                //var recordTime = DBCenter.GetSystemTime();
                //var userID = User.Identity.Name;

                //using (var cts = CimesTransactionScope.Create())
                //{
                //    //新增刀壽設定清單
                //    _ModifyToolTypeLifes.ForEach(toolTypeLife =>
                //    {
                //        //註記刀壽設定LOG的狀態
                //        ActionType actionType = new ActionType();

                //        if (toolTypeLife.InfoState == InfoState.NewCreate)
                //        {
                //            //新增資料
                //            toolTypeLife.InsertToDB(userID, recordTime);
                //            actionType = ActionType.Add;
                //        }
                //        else if (toolTypeLife.InfoState == InfoState.Modified)
                //        {
                //            //更改資料
                //            toolTypeLife.UpdateToDB(userID, recordTime);
                //            actionType = ActionType.Set;
                //            _SourceToolTypeLifes.Remove(toolTypeLife);
                //        }
                //        else if (toolTypeLife.InfoState == InfoState.Unchanged)
                //        {
                //            _SourceToolTypeLifes.Remove(toolTypeLife);
                //        }

                //        //紀錄歷史紀錄[CST_TOOL_TYPE_LIFE_LOG]
                //        if (actionType != ActionType.None)
                //        {
                //            LogCenter.LogToDB(toolTypeLife, LogCenter.LogIndicator.Create(actionType, userID, recordTime));
                //        }
                //    });

                //    //例外處理資料刪除的部份
                //    _SourceToolTypeLifes.ForEach(toolTypeLife =>
                //    {
                //        toolTypeLife.DeleteFromDB();
                //        LogCenter.LogToDB(toolTypeLife, LogCenter.LogIndicator.Create(ActionType.Remove, userID, recordTime));
                //    });

                //    cts.Complete();
                //}

                ////[00083]{0}儲存成功！
                //_ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00083(""));
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        /// <summary>
        /// 儲存刀壽
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="recordTime"></param>
        private void SaveToolLife(string toolType, string userID, string recordTime)
        {
            //例外處理資料刪除的部份
            _SourceToolTypeLifes.ForEach(toolTypeLife =>
            {
                toolTypeLife.DeleteFromDB();

                LogCenter.LogToDB(toolTypeLife, LogCenter.LogIndicator.Create(ActionType.Remove, userID, recordTime));
            });

            //新增刀壽設定清單
            _ModifyToolTypeLifes.ForEach(toolTypeLife =>
            {
                //註記刀壽設定LOG的狀態
                ActionType actionType = new ActionType();

                if (toolTypeLife.InfoState == InfoState.NewCreate)
                {
                    //新增資料
                    toolTypeLife.ToolType = toolType;
                    toolTypeLife.InsertToDB(userID, recordTime);
                    actionType = ActionType.Add;

                    //更新刀具已設定的刀壽次數
                    UpdateToolLifes(toolTypeLife, userID, recordTime);
                }
                else if (toolTypeLife.InfoState == InfoState.Modified)
                {
                    //更改資料
                    toolTypeLife.ToolType = toolType;
                    toolTypeLife.UpdateToDB(userID, recordTime);
                    actionType = ActionType.Set;
                    _SourceToolTypeLifes.Remove(toolTypeLife);

                    //更新刀具已設定的刀壽次數
                    UpdateToolLifes(toolTypeLife, userID, recordTime);
                }
                else if (toolTypeLife.InfoState == InfoState.Unchanged)
                {
                    _SourceToolTypeLifes.Remove(toolTypeLife);
                }

                //紀錄歷史紀錄[CST_TOOL_TYPE_LIFE_LOG]
                if (actionType != ActionType.None)
                {
                    LogCenter.LogToDB(toolTypeLife, LogCenter.LogIndicator.Create(actionType, userID, recordTime));
                }
            });
        }

        /// <summary>
        /// 更新刀具已設定的刀壽次數
        /// </summary>
        /// <param name="toolTypeLife"></param>
        /// <param name="userID"></param>
        /// <param name="recordTime"></param>
        private void UpdateToolLifes(CSTToolTypeLifeInfo toolTypeLife, string userID, string recordTime)
        {
            //依據刀具型態及供應商取得有刀具清單
            var toolList = ToolInfoEx.GetToolByToolTypeAndVendor(toolTypeLife.ToolType, toolTypeLife.Supplier);

            //執行更新每支刀具對應的基本刀壽次數
            toolList.ForEach(tool =>
            {
                //取得刀具基本刀壽資料
                var toolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(tool.ToolName);

                //取得此刀具所在的機台清單
                var equipToolList = EquipToolInfo.GetByToolName(tool.ToolName);

                //如果此刀具沒有在任何機台上，則進行修改基本刀壽次數
                if (equipToolList.Count == 0)
                {
                    toolLifeList.ForEach(toolLife =>
                    {
                    //確認基本刀壽次數是否一致，如果不一致，則直接更新資料
                    if (toolLife.Life != toolTypeLife.Life)
                        {
                        //更新基本刀壽次數
                        toolLife.Life = toolTypeLife.Life;
                            toolLife.UpdateToDB(userID, recordTime);

                        //記錄LOG
                        LogCenter.LogToDB(toolLife, LogCenter.LogIndicator.Create(ActionType.Set, userID, recordTime));
                        }
                    });
                }
            });
        }

        /// <summary>
        /// 取消刀壽設定資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToolLifeCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                //清除資料
                _ModifyToolTypeLifes = new List<CSTToolTypeLifeInfo>();

                _SourceToolTypeLifes.ForEach(toolTypeLife =>
                {
                    _ModifyToolTypeLifes.Add(toolTypeLife);
                });

                gvToolLife.SetDataSource(_ModifyToolTypeLifes, true);
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        /// <summary>
        /// 刪除刀壽設定資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvToolLife_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = gvToolLife.Rows[e.RowIndex].DataItemIndex;

                var removeData = _ModifyToolTypeLifes[index];

                _ModifyToolTypeLifes.Remove(removeData);

                gvToolLife.SetDataSource(_ModifyToolTypeLifes, true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 取得刀壽設定清單
        /// </summary>
        /// <param name="toolType"></param>
        private void GetToolTypeLifes(string toolType)
        {
            //清除資料
            _SourceToolTypeLifes = new List<CSTToolTypeLifeInfo>();
            _ModifyToolTypeLifes = new List<CSTToolTypeLifeInfo>();

            //取得刀壽設定清單
            _SourceToolTypeLifes = CSTToolTypeLifeInfo.GetDataListByToolType(toolType);

            _SourceToolTypeLifes.ForEach(toolTypeLife =>
            {
                _ModifyToolTypeLifes.Add(toolTypeLife);
            });

            gvToolLife.SetDataSource(_ModifyToolTypeLifes, true);
        }

        /// <summary>
        /// 取得供應商清單
        /// </summary>
        private void GetSuppliers()
        {
            var suppliers = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIToolSupplier");
            ddlSupplier.DataSource = suppliers;
            ddlSupplier.DataTextField = "Remark01";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, "");

            if (suppliers.Count == 0)
            {
                //[00555]查無資料，請至系統資料維護新增類別{0}、項目{1}!
                throw new Exception(TextMessage.Error.T00555("SAIToolSupplier", ""));
            }
        }

        /// <summary>
        /// 初始化刀壽及圖檔設定資料
        /// </summary>
        /// <param name="toolType"></param>
        private void LoadControlDefaultToolLifeAndPicture(string toolType)
        {
            //清除資料
            ddlSupplier.ClearSelection();
            ttbToolLifeCount.Text = "";

            //取得刀壽設定清單
            GetToolTypeLifes(toolType);

            //取得刀具圖檔
            GetToolTypeImages(toolType);
        }

        /// <summary>
        /// 儲存刀具圖檔資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUploadPicture_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!FileUpload1.HasFile)
                {
                    // [00854]請選擇檔案!!
                    throw new CimesException(TextMessage.Error.T00854());
                }

                string fileExt = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                //if (fileExt != ".jpeg" && fileExt != ".jpg" && fileExt != ".png" && fileExt != ".bmp")
                //{
                //    //僅接受格式為.jpg /.jpeg/.png/.bmp的檔案！
                //    throw new CimesException(RuleMessage.Error.C10182());
                //}

                string fileName = FileUpload1.PostedFile.FileName;

                WindowsImpersonationContext impersonationContext = null;

                //儲存資料夾
                string saveDirPath = _SavePicturedData[0].Remark01;

                //使用者帳號
                string userName = _SavePicturedData[0].Remark02;

                //使用者密碼
                string password = _SavePicturedData[0].Remark03;

                //實際儲存檔案路徑
                string saveFilePath = saveDirPath + "\\" + _ToolType + "\\" + fileName;

                //如果使用者帳號不為空白才執行切換帳號功能
                if (userName.IsNullOrTrimEmpty() == false)
                {
                    //切換使用者
                    var tokenHandle = Impersonate(userName, password, "domain");
                    WindowsIdentity windowsIdentity = new WindowsIdentity(tokenHandle);
                    impersonationContext = windowsIdentity.Impersonate();
                }

                //確認檔案在清單中是否已存在
                foreach (var toolTypePicture in _ModifyToolTypePictures)
                {
                    if (toolTypePicture.FileName == saveFilePath)
                    {
                        //檔案名稱:{0} 已存在!
                        throw new Exception(RuleMessage.Error.C10119(fileName));
                    }
                }

                //判斷儲存路徑是否存在
                if (Directory.Exists(saveDirPath + "\\" + _ToolType) == false)
                {
                    //如果不存在，則新增一個資料夾路徑
                    Directory.CreateDirectory(saveDirPath + "\\" + _ToolType);
                }

                //儲存檔案
                FileUpload1.SaveAs(saveFilePath);

                if (impersonationContext != null)
                {
                    //切換回原身分
                    impersonationContext.Undo();
                }

                //新增一筆刀壽資料
                var newToolPicture = InfoCenter.Create<CSTToolTypePictureInfo>();
                newToolPicture.ToolType = _ToolType;
                newToolPicture.FileName = saveFilePath;

                _ModifyToolTypePictures.Insert(0, newToolPicture);

                _HttpPostedFiles.Insert(0, FileUpload1.PostedFile);

                //顯示資料
                gvToolTypeImage.SetDataSource(_ModifyToolTypePictures, true);
            }
            catch (Exception ex)
            {
                HandleError(ex, CurrentUpdatePanel, string.Empty, MessageShowOptions.OnLabel);
            }
        }

        /// <summary>
        /// 儲存刀具圖檔
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="recordTime"></param>
        private void SaveToolPicture(string toolType, string userID, string recordTime)
        {
            _ModifyToolTypePictures.ForEach(toolTypePicture =>
            {
                //註記圖檔LOG的狀態
                ActionType actionType = new ActionType();

                if (toolTypePicture.InfoState == InfoState.NewCreate)
                {
                    //新增資料
                    toolTypePicture.ToolType = toolType;
                    toolTypePicture.InsertToDB(userID, recordTime);
                    actionType = ActionType.Add;
                }
                else if (toolTypePicture.InfoState == InfoState.Modified)
                {
                    //更改資料
                    toolTypePicture.ToolType = toolType;
                    toolTypePicture.UpdateToDB(userID, recordTime);
                    actionType = ActionType.Set;
                    _SourceToolTypePictures.Remove(toolTypePicture);
                }
                else if (toolTypePicture.InfoState == InfoState.Unchanged)
                {
                    _SourceToolTypePictures.Remove(toolTypePicture);
                }

                //紀錄歷史紀錄[CST_TOOL_TYPE_PICT_LOG]
                if (actionType != ActionType.None)
                {
                    LogCenter.LogToDB(toolTypePicture, LogCenter.LogIndicator.Create(actionType, userID, recordTime));
                }
            });

            //例外處理資料刪除的部份
            _SourceToolTypePictures.ForEach(toolTypePicture =>
            {
                toolTypePicture.DeleteFromDB();
                LogCenter.LogToDB(toolTypePicture, LogCenter.LogIndicator.Create(ActionType.Remove, userID, recordTime));
            });
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
        /// //取得刀具圖檔
        /// </summary>
        /// <param name="toolType"></param>
        private void GetToolTypeImages(string toolType)
        {
            //清除資料
            _SourceToolTypePictures = new List<CSTToolTypePictureInfo>();
            _ModifyToolTypePictures = new List<CSTToolTypePictureInfo>();
            _HttpPostedFiles = new List<HttpPostedFile>();

            //取得刀具圖檔清單
            _SourceToolTypePictures = CSTToolTypePictureInfo.GetDataListByToolType(toolType);

            _SourceToolTypePictures.ForEach(toolTypePicture =>
            {
                _HttpPostedFiles.Add(FileUpload1.PostedFile);
                _ModifyToolTypePictures.Add(toolTypePicture);
            });

            gvToolTypeImage.SetDataSource(_ModifyToolTypePictures, true);
        }

        /// <summary>
        /// 刪除圖檔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvToolTypeImage_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = gvToolTypeImage.Rows[e.RowIndex].DataItemIndex;

                _ModifyToolTypePictures.RemoveAt(index);

                gvToolTypeImage.SetDataSource(_ModifyToolTypePictures, true);

            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvToolTypeImage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                var btnOpenFile = e.Row.FindControl("btnOpenFile") as LinkButton;
                string filePath = _ModifyToolTypePictures[e.Row.DataItemIndex].FileName;
                var text = filePath.Replace(_SavePicturedData[0].Remark01 + "\\" + _ModifyToolTypePictures[e.Row.DataItemIndex].ToolType + "\\", "");

                btnOpenFile.Text = text;
           
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        ///// <summary>
        ///// 點擊圖檔
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ibtnToolType_Click(object sender, ImageClickEventArgs e)
        //{
        //    var ibtnToolType = ((ImageButton)sender);

        //    //打開文件
        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
        //        Session.SessionID, "window.open('" + ibtnToolType.ImageUrl + "','_blank','resizable,scrollbars=no,menubar=no,toolbar=no,location=no,status=no',false);", true);
        //}

        /// <summary>
        /// 開啟檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOpenFile_Click(object sender, EventArgs e)
        {
            var row = ((LinkButton)sender).Parent.Parent as GridViewRow;

            string url = "";
            string filePath = _ModifyToolTypePictures[row.DataItemIndex].FileName;
            string sHost = Request.Url.Host;
            string sApplicationPath = Request.ApplicationPath;
            string picturePath = "http://" + sHost + sApplicationPath + "/ToolImage";
            url = filePath.Replace(_SavePicturedData[0].Remark01, picturePath).Replace(@"\", "/");

            //打開文件
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                Session.SessionID, "window.open('" + url + "','_blank','resizable,scrollbars=no,menubar=no,toolbar=no,location=no,status=no',false);", true);
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
    }
}
