/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：kmchen

功能說明：當刀具零組件損耗刀一定程度時，需做維修保養方能繼續使用，則需執行維修作業來做維修紀錄。

------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/10/18      kmchen       初版
*/

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule;
using CustomizeRule.RuleUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.ToolRule
{
    public partial class T017 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 刀具零組件資料
        /// </summary>
        private ToolInfoEx _ToolData
        {
            get { return this["_ToolData"] as ToolInfoEx; }
            set { this["_ToolData"] = value; }
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
                    ClearField();
                    AjaxFocus(ttbToolName);
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
        /// 清除資料
        /// </summary>
        private void ClearField()
        {
            _ToolData = null;
            ttbToolName.Text = "";
            ttbDate.Text = "";
            btnOK.Enabled = false;
            ttbRepairPart.Text = "";
            ddlReason.Items.Clear();
        }

        /// <summary>
        /// 輸入刀具零組件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbToolName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _ToolData = ToolInfo.GetToolByName(ttbToolName.Text).ChangeTo<ToolInfoEx>();

                //確認刀具零組件是否存在
                if (_ToolData == null)
                {
                    // [00030]{0}：{1}不存在!
                    throw new Exception(TextMessage.Error.T00030(lblToolName.Text, ttbToolName.Text));
                }

                //確認刀具零組件是否啟用
                if (_ToolData.UsingStatus == UsingStatus.Disable)
                {
                    //刀具零組件：{0}已停用，如需使用，請至"刀具零組件進料作業"啟用!!
                    throw new Exception(RuleMessage.Error.C10128(ttbToolName.Text));
                }

                //確認刀具零組件的LOCATION是否在Warehouse
                if (_ToolData.Location != "Warehouse")
                {
                    //刀具零組件:{0} 不在庫房，不可執行此功能!!
                    throw new Exception(RuleMessage.Error.C10127(ttbToolName.Text));
                }

                //確認刀具零組件狀態是否可使用，僅IDLE可執行
                if (_ToolData.CurrentState != "IDLE")
                {
                    //刀具零組件狀態為{0}，不可執行此功能!!
                    throw new Exception(RuleMessage.Error.C10129(_ToolData.CurrentState));
                }

                #region 下機原因碼(DropDownList)：依照原因碼工作站設定，帶出原因碼
                ddlReason.Items.Clear();

                List<BusinessReason> reasonList = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(ProgramRight, "ALL", "Default", ReasonMode.Category);

                if (reasonList.Count > 0)
                {
                    ddlReason.DataSource = reasonList;
                    ddlReason.DataTextField = "ReasonDescription";
                    ddlReason.DataValueField = "ReasonCategorySID";
                    ddlReason.DataBind();

                    if (ddlReason.Items.Count != 1)
                        ddlReason.Items.Insert(0, "");
                    else
                    {
                        ddlReason.SelectedIndex = 0;
                    }
                }
                else
                {
                    //[00641]規則:{0} 工作站:{1} 使用的原因碼未設定，請洽IT人員!
                    throw new Exception(TextMessage.Error.T00641(ProgramRight, "ALL"));
                }
                #endregion

                btnOK.Enabled = true;
            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbToolName);
                HandleError(ex);
            }
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                //確認是否輸入刀具零組件
                ttbToolName.Must(lblToolName);

                //確認是否選擇維修原因
                ddlReason.Must(lblReason);

                //確認是否有選擇預定回廠日
                var date = ttbDate.MustDate(lblDate);

                //取得系統時間
                var sysDate = Convert.ToDateTime(DBCenter.GetSystemDateTime().ToString("yyyy/MM/dd"));

                //確認預定回廠日是否大於等於當天日期
                if (date < sysDate)
                {
                    //預定回廠日必須大於等於{0} !
                    throw new Exception(RuleMessage.Error.C10159(sysDate.ToString("yyyy/MM/dd")));
                }

                using (var cts = CimesTransactionScope.Create())
                {
                    //取得原因碼資料
                    var reasonCategory = InfoCenter.GetBySID<ReasonCategoryInfo>(ddlReason.SelectedValue);

                    //取得刀面資料清單
                    var toolLifes = CSTToolLifeInfo.GetToolLifeByToolNmae(_ToolData.ToolName);
                    toolLifes.Sort(data => data.Head);

                    //新增一筆[CST_TOOL_REPAIR]
                    var newToolRePair = InfoCenter.Create<CSTToolRepairInfo>();
                    newToolRePair.EstimateDateOfReturn = ttbDate.Text;
                    newToolRePair.Parts = ttbRepairPart.Text;
                    newToolRePair.Reason = reasonCategory.Reason;
                    newToolRePair.ReasonCategory = reasonCategory.Category;
                    newToolRePair.RepairTime = txnStamp.RecordTime;
                    newToolRePair.RepairUser = txnStamp.UserID;
                    newToolRePair.ToolName = _ToolData.ToolName;
                    newToolRePair.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    //新增一筆[CST_TOOL_REPAIR_LOG]
                    LogCenter.LogToDB(newToolRePair, LogCenter.LogIndicator.Create(ActionType.Add, txnStamp.UserID, txnStamp.RecordTime));

                    //新增[CST_TOOL_REPAIR_LIFE]
                    toolLifes.ForEach(toolLife =>
                    {
                        var newToolRePairLife = InfoCenter.Create<CSTToolRepairLifeInfo>();
                        newToolRePairLife.Head = toolLife.Head;
                        newToolRePairLife.Life = toolLife.UseCount.ToString();
                        newToolRePairLife.ToolRepairSID = newToolRePair.ToolRepairSID;
                        newToolRePairLife.InsertToDB();
                    });

                    var newStateInfo = ToolStateInfo.GetToolStateByState("REPAIR");
                    if (newStateInfo == null)
                    {
                        //刀具零組件狀態: {0}不存在，請至配件狀態維護新增此狀態!!
                        throw new Exception(RuleMessage.Error.C10149("REPAIR"));
                    }

                    //因刀具報表需求，所以在送修時要將使用次數記錄在UDC07                    
                    var toolLifeData = toolLifes.Find(p => p.Head == _ToolData.Head);
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL07", toolLifeData.UseCount.ToCimesString(), txnStamp);

                    //變更狀態為REPAIR
                    TMSTransaction.ChangeToolState(_ToolData, newStateInfo, txnStamp);

                    int maintainCount = _ToolData.MaintainCount;

                    //維修次數+1
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "MAINTAINCOUNT", (maintainCount + 1).ToString(), txnStamp);

                    //預定回廠日
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL04", ttbDate.Text, txnStamp);

                    //註記原因碼
                    txnStamp.CategoryReasonCode = reasonCategory;
                    txnStamp.Description = "";

                    //備註
                    TMSTransaction.AddToolComment(_ToolData, txnStamp);

                    cts.Complete();
                }

                ClearField();

                AjaxFocus(ttbToolName);

                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00614(""));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //回到模具查詢
                Response.Redirect(ciMes.Security.UserSetting.GetPreviousListPage(this), false);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}