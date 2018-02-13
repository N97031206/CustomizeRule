/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tool
作者：Nick

功能說明：包裝作業。包裝方式分為：標準、混和及不卡控。
依照方式不同，執行不一樣的邏輯。

------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/10/12      Nick       初版
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
using Ares.Cimes.IntelliService.Web.UI;
namespace CustomizeRule.ToolRule
{
    public partial class T011 : CustomizeRuleBasePage
    {
        private EquipmentInfo _EquipmentInfo
        {
            get
            {
                return (EquipmentInfo)this["_EquipmentInfo"];
            }
            set
            {
                this["_EquipmentInfo"] = value;
            }
        }

        private ToolInfo _CurrentToolInfo
        {
            get
            {
                return (ToolInfo)this["_CurrentToolInfo"];
            }
            set
            {
                this["_CurrentToolInfo"] = value;
            }
        }

        private List<CSTToolLifeInfo> _CurrentToolLifeList
        {
            get
            {
                return (List<CSTToolLifeInfo>)this["_CurrentToolLifeList"];
            }
            set
            {
                this["_CurrentToolLifeList"] = value;
            }
        }

        private List<CSTToolLifeInfo> _TakeToolLifeList
        {
            get
            {
                return (List<CSTToolLifeInfo>)this["_TakeToolLifeList"];
            }
            set
            {
                this["_TakeToolLifeList"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ClearField();

                    LoadDefaultControl();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void LoadDefaultControl()
        {           
            ttbTakeUserName.Text = UserProfileInfo.GetInfoByID(Page.User.Identity.Name).UserName;
            ttbTakeDate.Text = DBCenter.GetSystemTime().Substring(0, 10);

            #region 取得Rule的Reason                        
            RuleReasonInfo ruleRsn = RuleReasonInfo.GetRuleReasonByOperationRuleControlName("ALL", this.ProgramRight, "Default");
            if (ruleRsn == null)
            {
                throw new RuleCimesException(TextMessage.Error.T00641(this.ProgramRight, "ALL"));
            }
            csReason.Setup(ruleRsn.RuleName, ruleRsn.OperationName, ruleRsn.ControlName, ReasonMode.Category);
            #endregion
        }

        private void ClearField()
        {
            ttbTakeQuantity.Text = "0";
            ttbMillName.Text = "";
            ddlMillHeader.Items.Clear();
            ttbEquipment.Text = "";
            ttbIdentity.Text = "";

            csReason.ClearSelection();
            gvMillHeader.SetDataSource(null, true);
            
            ClearData();
        }

        private void ClearData()
        {
            _EquipmentInfo = null;
            _CurrentToolInfo = null;
            _CurrentToolLifeList = null;
            _TakeToolLifeList = null;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ddlMillHeader.Must(lblMillHeader);

                if (_TakeToolLifeList == null)
                {
                    _TakeToolLifeList = new List<CSTToolLifeInfo>();
                }

                // 檢查刀具是否重複
                if (_TakeToolLifeList.Find(p => p.ToolName == _CurrentToolInfo.ToolName) != null)
                {
                    AjaxFocus(ttbMillName);
                    // [00033]刀具名稱:{1}重複！
                    throw new RuleCimesException(TextMessage.Error.T00033(lblMillName.Text, _CurrentToolInfo.ToolName));
                }

                var additem = _CurrentToolLifeList.Find(p => p.ID == ddlMillHeader.SelectedValue);
                additem.Operation = ddlOperation.SelectedValue;
                _TakeToolLifeList.Add(additem);

                gvMillHeader.SetDataSource(_TakeToolLifeList, true);

                ttbMillName.Text = "";
                AjaxFocus(ttbMillName);
                ddlMillHeader.Items.Clear();

                ttbTakeQuantity.Text = _TakeToolLifeList.Count.ToString();
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
                ttbEquipment.Must(lblEquipment);
                ddlOperation.Must(lblOperation);
                csReason.Must(lblTakeReason);

                if (_TakeToolLifeList.Count == 0)
                {
                    // [00824]請新增[刀頭]！
                    throw new RuleCimesException(TextMessage.Error.T00824(lblMillHeader.Text));
                }

                string groupid = DBCenter.GetSystemID();
                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    _TakeToolLifeList.ForEach(tool =>
                    {
                        // 變更GROUPID與HEAD系統屬性
                        // HEAD紀錄目前刀具使用的刀頭
                        // GROUPID紀錄目前綁定的刀具
                        var toolInfo = ToolInfo.GetToolByName(tool.ToolName);
                        var modifyAttrList = new List<ModifyAttributeInfo>();
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("GROUPID", groupid));
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("HEAD", tool.Head));
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("LOCATION", "Hub"));
                        //因刀具報表需求，所以在領用時要AddComment，並將刀面及使用次數記錄在UDC06 & UDC07
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("USERDEFINECOL06", tool.Head));
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("USERDEFINECOL07", tool.UseCount));
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("USERDEFINECOL08", tool.Operation));
                        TMSTransaction.ModifyToolMultipleAttribute(toolInfo, modifyAttrList, txnStamp);                        

                        txnStamp.Remark = "刀具領用";
                        var reasonCategory = InfoCenter.GetBySID<ReasonCategoryInfo>(csReason.SelectedValue);
                        txnStamp.CategoryReasonCode = reasonCategory;
                        txnStamp.Description = "";
                        TMSTransaction.AddToolComment(toolInfo, txnStamp);
                    });

                    // 寫入CSTToolIssue客製表
                    var toolIssueInfo = InfoCenter.Create<CSTToolIssueInfo>();
                    toolIssueInfo.TOTALQTY = _TakeToolLifeList.Count;
                    toolIssueInfo.Reason = csReason.GetBusinessReason().ReasonCode;
                    toolIssueInfo.EquipmentName = _EquipmentInfo == null ? "" : _EquipmentInfo.EquipmentName;
                    toolIssueInfo.LinkSID = txnStamp.LinkSID;
                    toolIssueInfo.Action = "領用";
                    toolIssueInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    cts.Complete();
                }

                ClearField();
                AjaxFocus(ttbMillName);

                ((ProgramInformationBlock)ProgramInformationBlock1).ShowMessage(TextMessage.Hint.T00057("領用"));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
        
        protected void gvMillHeader_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                var dataItem = (CSTToolLifeInfo)e.Row.DataItem;
                Label lblOperation = (Label)e.Row.FindControl("lblOperation");
                lblOperation.Text = dataItem.Operation;
            }
            catch (Exception E)
            {
                HandleError(E);
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

        protected void ttbEquipment_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ttbEquipment.Text.Trim().IsNullOrTrimEmpty())
                {
                    return;
                }

                string sEquipment = ttbEquipment.Text.Trim();
                _EquipmentInfo = EquipmentInfo.GetEquipmentByName(sEquipment);
                if (_EquipmentInfo == null)
                {
                    ttbEquipment.Text = "";
                    AjaxFocus(ttbEquipment);
                    throw new RuleCimesException(TextMessage.Error.T00030(lblEquipment.Text, sEquipment));
                }

                ddlOperation.Items.Clear();
                var lstOepration = DBCenter.GetStringList(SQLCenter.Parse(
                    "SELECT DISTINCT OPERATION FROM CST_TOOL_DEVICE_DETAIL WHERE EQP=#[STRING] AND OPERATION IS NOT NULL", sEquipment));
                lstOepration.ForEach(p =>
                {
                    ddlOperation.Items.Add(p);
                });
                if (lstOepration.Count > 1)
                {
                    ddlOperation.Items.Insert(0, "");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ttbMillName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ddlMillHeader.Items.Clear();

                if (ttbMillName.Text.Trim().IsNullOrTrimEmpty())
                {
                    ttbMillName.Text = string.Empty;
                    return;
                }

                string sToolName = ttbMillName.Text.Trim();
                _CurrentToolInfo = ToolInfo.GetToolByName(sToolName);
                if (_CurrentToolInfo == null)
                {                    
                    throw new RuleCimesException(TextMessage.Error.T00030(lblMillName.Text, sToolName));
                }

                #region 依照刀具目前儲位及狀態做基礎判斷
                if (_CurrentToolInfo.Location != "Warehouse")
                {
                    // 刀具:{0} 位置:{1} 不可領用 !
                    throw new RuleCimesException(RuleMessage.Error.C10124(sToolName, _CurrentToolInfo.Location));
                }

                if (_CurrentToolInfo.CurrentState.ToUpper() == "SCRAP")
                {
                    throw new CimesException(RuleMessage.Error.C00044(_CurrentToolInfo.ToolName));
                }

                if (_CurrentToolInfo.CurrentState.ToUpper() == "REPAIR")
                {
                    throw new CimesException(RuleMessage.Error.C00045(_CurrentToolInfo.ToolName));
                }

                if (_CurrentToolInfo.UsingStatus.ToCimesString().ToUpper() != "ENABLE")
                {
                    //T01677, {0}:{1}未啟用，不可使用!
                    throw new CimesException(TextMessage.Error.T01677(GetUIResource("MillNumber"), _CurrentToolInfo.ToolName));
                }
                #endregion

                ttbIdentity.Text = _CurrentToolInfo["IDENTITY"].ToString();

                _CurrentToolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(_CurrentToolInfo.ToolName);
                if (_CurrentToolLifeList.Count == 0)
                {
                    // 刀具:{0}找不到刀頭資料 !
                    throw new RuleCimesException(RuleMessage.Error.C10123(sToolName));
                }
                
                _CurrentToolLifeList.ForEach(toollife =>
                {
                    ddlMillHeader.Items.Add(new ListItem(toollife.Head, toollife.ID));
                });

                //如果只有單頭，就預帶刀面數
                if (ddlMillHeader.Items.Count != 1)
                    ddlMillHeader.Items.Insert(0, "");
                else
                {
                    ddlMillHeader.SelectedIndex = 0;
                }

                AjaxFocus(ddlMillHeader.ClientID);
            }
            catch (Exception ex)
            {                
                HandleError(ex, ttbMillName.ClientID);
            }
        }

        protected void gvMillHeader_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {                
                int index = gvMillHeader.Rows[e.RowIndex].DataItemIndex;
                _TakeToolLifeList.RemoveAt(index);

                gvMillHeader.SetDataSource(_TakeToolLifeList, true);
                ttbTakeQuantity.Text = _TakeToolLifeList.Count.ToString();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}