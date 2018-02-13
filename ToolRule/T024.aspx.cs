/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：kmchen

功能說明：變更刀具維修的回廠日期。

------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/11/27      kmchen       初版
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
    public partial class T024: CustomizeRuleBasePage
    {
        /// <summary>
        /// 刀具零組件資料
        /// </summary>
        private ToolInfoEx _ToolData
        {
            get { return this["_ToolData"] as ToolInfoEx; }
            set { this["_ToolData"] = value; }
        }

        private CSTToolRepairInfo _ToolRepairData
        {
            get { return this["_ToolRepairData"] as CSTToolRepairInfo; }
            set { this["_ToolRepairData"] = value; }
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
            ttbBackDate.Text = "";
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

                //確認刀具零組件狀態是否可使用，僅REPAIR可執行
                if (_ToolData.CurrentState != "REPAIR")
                {
                    //刀具零組件狀態為{0}，不可執行此功能!!
                    throw new Exception(RuleMessage.Error.C10129(_ToolData.CurrentState));
                }

                //萬一在維修客製表找不到資料，就先報錯
                _ToolRepairData = CSTToolRepairInfo.GetDataByToolName(_ToolData.ToolName);
                if (_ToolRepairData == null)
                {
                    throw new CimesException(RuleMessage.Error.C00046(_ToolData.ToolName));
                }

                //預計回廠日期
                ttbBackDate.Text = _ToolData.UserDefineColumn04;

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

                //確認是否有選擇變更回廠日
                var date = ttbDate.MustDate(lblDate);

                //取得系統時間
                var sysDate = Convert.ToDateTime(DBCenter.GetSystemDateTime().ToString("yyyy/MM/dd"));

                //確認預定回廠日是否大於等於當天日期
                if (date < sysDate)
                {
                    //變更回廠日必須大於等於{0} !
                    throw new Exception(RuleMessage.Error.C10170(sysDate.ToString("yyyy/MM/dd")));
                }

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 更新[CST_TOOL_REPAIR]的預計回廠日
                    _ToolRepairData.EstimateDateOfReturn = ttbDate.Text;
                    _ToolRepairData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

                    //新增一筆[CST_TOOL_REPAIR_LOG]
                    LogCenter.LogToDB(_ToolRepairData, LogCenter.LogIndicator.Create(ActionType.Set, txnStamp.UserID, txnStamp.RecordTime));

                    #endregion

                    //預定回廠日
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL04", ttbDate.Text, txnStamp);

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