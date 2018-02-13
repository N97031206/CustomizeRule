/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：keith

功能說明：提供模具領用功能，變更模具現在所屬儲位(location)。

------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/07/31      keith       初版
*/
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using ciMes.Web.Common.UserControl;
using CustomizeRule.RuleUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ares.Cimes.IntelliService.Transaction;
using CustomizeRule;

namespace CustomizeRule.ToolRule
{
    public partial class T001 : CustomizeRuleBasePage
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
                    gvChangeWO.Initialize();
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
            ttbChangeLine.Text = "";
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

                #region 驗證模具儲位是否可領用，僅Warehouse可執行
                if (_ToolData.Location != "Warehouse")
                {
                    //模具儲位為{0}，不在庫房，不須領用!!
                    throw new Exception(RuleMessage.Error.C10018(_ToolData.Location));
                }
                #endregion

                #region 驗證模具狀態是否可使用，僅IDLE可執行
                if (_ToolData.CurrentState != "IDLE")
                {
                    if (_ToolData.CurrentState == "WaitPM")
                    {
                        //模具狀態為{0}，請進行降模或鉗修保養!!
                        throw new Exception(RuleMessage.Error.C10019(_ToolData.CurrentState));
                    }
                    else
                    {
                        //模具狀態為{0}，不可執行此功能!!
                        throw new Exception(RuleMessage.Error.C10020(_ToolData.CurrentState));
                    }
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
        /// 輸入換線線別
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbChangeLine_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //To do..
                // gvChangeWO.SetDataSource();
            }
            catch (Exception ex)
            {
                AjaxFocus(ttbChangeLine);
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
                    //變更模具上的儲位為Hub
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "Location", "Hub", txnStamp);

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