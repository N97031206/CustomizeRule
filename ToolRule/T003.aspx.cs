/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：keith

功能說明：提供模具上機台功能。

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
    public partial class T003 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 模具資料
        /// </summary>
        private ToolInfo _ToolData
        {
            get { return this["_ToolData"] as ToolInfo; }
            set { this["_ToolData"] = value; }
        }

        /// <summary>
        /// 機台資料
        /// </summary>
        private EquipmentInfo _EquipData
        {
            get { return this["_EquipData"] as EquipmentInfo; }
            set { this["_EquipData"] = value; }
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
            _EquipData = null;
            ttbEquip.Text = "";
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
                _ToolData = ToolInfo.GetToolByName(ttbToolName.Text.Trim());

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

                if (equipToolDataList.Count > 0)
                {
                    //模治具：{0}已在機台：{1}上，不可再上機!!
                    throw new Exception(RuleMessage.Error.C10023(ttbToolName.Text, equipToolDataList[0].EquipmentName));
                }
                #endregion

                #region 驗證模治具儲位是否可上機，僅Hub可執行
                if (_ToolData.Location != "Hub")
                {
                    //模治具儲位為{0}，尚未領用，不可執行上機!!
                    throw new Exception(RuleMessage.Error.C10024(_ToolData.Location));
                }
                #endregion

                #region 驗證模治具狀態是否可使用，僅IDLE可執行
                if (_ToolData.CurrentState != "IDLE")
                {
                    //模治具狀態為{0}，不可執行此功能!!
                    throw new Exception(RuleMessage.Error.C10022(_ToolData.CurrentState));
                }
                #endregion

                ttbToolDescr.Text = _ToolData.Description;
                ttbToolType.Text = _ToolData.ToolType;

                if (_EquipData != null)
                {
                    btnOK.Enabled = true;
                }

                AjaxFocus(ttbEquip);
            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbToolName);
                HandleError(ex);
            }
        }

        /// <summary>
        /// 輸入機台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbEquip_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _EquipData = EquipmentInfo.GetEquipmentByName(ttbEquip.Text);

                #region 驗證機台正確性
                if (_EquipData == null)
                {
                    //[00885]機台{0}不存在！
                    throw new Exception(TextMessage.Error.T00885(ttbEquip.Text));
                }
                #endregion

                #region 驗證機台啟用狀態
                if (_EquipData.UsingStatus == UsingStatus.Disable)
                {
                    //機台：{0}已停用，如需使用，請至"機台資料維護"啟用!!
                    throw new Exception(RuleMessage.Error.C10025(_EquipData.EquipmentName));
                }
                #endregion

                #region 驗證機台狀態，僅IDLE、RUN可使用
                if (_EquipData.CurrentState != "IDLE" && _EquipData.CurrentState != "RUN")
                {
                    //機台狀態為{0}，不可執行此功能!!
                    throw new Exception(RuleMessage.Error.C10026(_EquipData.CurrentState));
                }
                #endregion

                if (_ToolData != null)
                {
                    btnOK.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ttbEquip.Text = "";
                _EquipData = null;
                AjaxFocus(ttbEquip);
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

                //確認機台是否有輸入
                ttbEquip.Must(lblEquip);

                using (var cts = CimesTransactionScope.Create())
                {
                    //配件上機台
                    TMSTxn.Default.AddToolToEquipment(new List<ToolInfo>() { _ToolData }, _EquipData, txnStamp);
                    
                    var newStateInfo = ToolStateInfo.GetToolStateByState("USED");
                    if (newStateInfo == null)
                    {
                        // 模治具狀態：{0}不存在!!
                        throw new Exception(RuleMessage.Error.C10032("USED"));
                    }

                    //變更配件狀態為USED
                    TMSTransaction.ChangeToolState(_ToolData, newStateInfo, txnStamp);
                    
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