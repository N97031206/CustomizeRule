/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：keith

功能說明：提供模具下機台功能，並記錄下機原因。

------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/07/31      keith       初版
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
    public partial class T004 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 模具資料
        /// </summary>
        private ToolInfo _ToolData
        {
            get { return this["_ToolData"] as ToolInfo; }
            set { this["_ToolData"] = value; }
        }

        private string _ToolChangeState
        {
            get { return this["_ToolChangeState"] as string; }
            set { this["_ToolChangeState"] = value; }
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
                    if (Request["ChangeState"] != null)
                    {
                        _ToolChangeState = Request["ChangeState"];
                    }
                    else
                    {
                        //網頁沒有設定更變狀態!!
                        HintAndRedirect(RuleMessage.Error.C10028());
                        return;
                    }

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
            ttbCheckOutDescr.Text = "";
            ttbToolDescr.Text = "";
            ttbToolName.Text = "";
            ttbToolType.Text = "";
            ttbEquip.Text = "";

            btnOK.Enabled = false;

            ddlCheckOutReasonCode.Items.Clear();
        }

        /// <summary>
        /// 輸入模具編號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbToolName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string equipmentName = "";

                _ToolData = ToolInfo.GetToolByName(ttbToolName.Text);

                #region 驗證模治具號正確性
                if (_ToolData == null)
                {
                    //模治具：{0}不存在，請確認資料正確性
                    throw new Exception(RuleMessage.Error.C10029(ttbToolName.Text));
                }
                #endregion

                #region 驗證模治具啟用狀態
                if (_ToolData.UsingStatus == UsingStatus.Disable)
                {
                    //模治具：{0}已停用，如需使用，請至"配件資料維護"啟用!!
                    throw new Exception(RuleMessage.Error.C10030(ttbToolName.Text));
                }
                #endregion

                #region 驗證模治具是否在別的機台上，如是要報錯
                var equipToolDataList = EquipToolInfo.GetByToolName(_ToolData.ToolName);

                if (equipToolDataList.Count == 0)
                {
                    //模治具：{0}不在機台上，不須下機!!
                    throw new Exception(RuleMessage.Error.C10031(ttbToolName.Text));
                }

                equipmentName = equipToolDataList[0].EquipmentName;
                #endregion

                #region 下機原因碼(DropDownList)：依照原因碼工作站設定，帶出原因碼
                ddlCheckOutReasonCode.Items.Clear();

                List<BusinessReason> reasonList = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(ProgramRight, "ALL", "Default", ReasonMode.Category);

                if (reasonList.Count > 0)
                {
                    ddlCheckOutReasonCode.DataSource = reasonList;
                    ddlCheckOutReasonCode.DataTextField = "ReasonDescription";
                    ddlCheckOutReasonCode.DataValueField = "ReasonCategorySID";
                    ddlCheckOutReasonCode.DataBind();

                    if (ddlCheckOutReasonCode.Items.Count != 1)
                        ddlCheckOutReasonCode.Items.Insert(0, "");
                    else
                    {
                        ddlCheckOutReasonCode.SelectedIndex = 0;
                    }
                }
                else
                {
                    //[00641]規則:{0} 工作站:{1} 使用的原因碼未設定，請洽IT人員!
                    throw new Exception(TextMessage.Error.T00641(ProgramRight, "ALL"));
                }
                #endregion

                ttbToolDescr.Text = _ToolData.Description;
                ttbToolType.Text = _ToolData.ToolType;
                ttbEquip.Text = equipmentName;

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

                //確認是否輸入模治具
                ttbToolName.Must(lblToolName);

                //確認是否選擇下機原因
                ddlCheckOutReasonCode.Must(lblCheckOutReasonCode);

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 確認機台資料
                    var equipData = EquipmentInfo.GetEquipmentByName(ttbEquip.Text);

                    if (equipData == null)
                    {
                        //[00885]機台{0}不存在！
                        throw new Exception(TextMessage.Error.T00885(ttbEquip.Text));
                    }
                    #endregion

                    //配件下機台
                    TMSTxn.Default.RemoveToolFromEquipment(_ToolData, equipData, txnStamp);

                    ////配件記錄下機原因。
                    //TMSTransaction.AddToolComment(_ToolData, txnStamp);

                    //註記原因碼
                    var reasonCategory = InfoCenter.GetBySID<ReasonCategoryInfo>(ddlCheckOutReasonCode.SelectedValue);
                    txnStamp.CategoryReasonCode = reasonCategory;
                    txnStamp.Description = ttbCheckOutDescr.Text;

                    #region 變更配件狀態為WaitPM或IDLE(依權限傳入的參數而定)
                    var newStateInfo = ToolStateInfo.GetToolStateByState(_ToolChangeState);
                    if (newStateInfo == null)
                    {
                        //模治具狀態：{0}不存在!!
                        throw new Exception(RuleMessage.Error.C10032(_ToolChangeState));
                    }

                    TMSTransaction.ChangeToolState(_ToolData, newStateInfo, txnStamp);
                    #endregion

                    

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