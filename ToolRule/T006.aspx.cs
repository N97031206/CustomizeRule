/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：keith

功能說明：提供模具降模紀錄。

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
    public partial class T006 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 模具資料
        /// </summary>
        private ToolInfo _ToolData
        {
            get { return this["_ToolData"] as ToolInfo; }
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
            ttbToolDescr.Text = "";
            ttbToolName.Text = "";
            ttbToolType.Text = "";

            btnOK.Enabled = false;
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
                _ToolData = ToolInfo.GetToolByName(ttbToolName.Text);

                #region 驗證模具號正確性
                if (_ToolData == null)
                {
                    //模具：{0}不存在，請確認資料正確性
                    throw new Exception(RuleMessage.Error.C10016(ttbToolName.Text));
                }
                #endregion

                #region 驗證模具啟用狀態
                if (_ToolData.UsingStatus == UsingStatus.Disable)
                {
                    //模具：{0}已停用，如需使用，請至"配件資料維護"啟用!!
                    throw new Exception(RuleMessage.Error.C10017(ttbToolName.Text));
                }
                #endregion

                #region 驗證模具狀態是否可使用，僅WaitPM可執行
                if (_ToolData.CurrentState != "WaitPM")
                {
                    //模具狀態為{0}，不可進行降模作業!!
                    throw new Exception(RuleMessage.Error.C10034(_ToolData.CurrentState));
                }
                #endregion

                #region 驗證模具儲位是否可使用，僅Warehouse可執行
                if (_ToolData.Location != "Warehouse")
                {
                    //模具儲位為{0}，不在庫房，不可進行降模作業!!
                    throw new Exception(RuleMessage.Error.C10035(_ToolData.Location));
                }
                #endregion

                ttbToolDescr.Text = _ToolData.Description;
                ttbToolType.Text = _ToolData.ToolType;

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

                //確認模具是否有輸入
                ttbToolName.Must(lblToolName);

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 變更配件狀態為IDLE
                    var newStateInfo = ToolStateInfo.GetToolStateByState("IDLE");
                    if (newStateInfo == null)
                    {
                        // 模具狀態：{0}不存在!!
                        throw new Exception(RuleMessage.Error.C10027("IDLE"));
                    }

                    TMSTransaction.ChangeToolState(_ToolData, newStateInfo, txnStamp);
                    #endregion

                    //將配件上的鉗修次數重置成0 (MES_TOOL_MAST. USERDEFINECOL01)
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL01", "0", txnStamp);

                    //將配件上的降模次數重置成0 (MES_TOOL_MAST. USERDEFINECOL02)
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL02", "0", txnStamp);

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