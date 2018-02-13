/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：kmchen

功能說明：刀具領用時會決定當次使用刀面，若作業過程中需要變更刀面，則使用該功能。

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
    public partial class T020 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 刀具零組件資料
        /// </summary>
        private ToolInfoEx _ToolData
        {
            get { return this["_ToolData"] as ToolInfoEx; }
            set { this["_ToolData"] = value; }
        }

        /// <summary>
        /// 刀面資料清單
        /// </summary>
        private List<CSTToolLifeInfo> _ToolLifeList
        {
            get { return this["_ToolLifeList"] as List<CSTToolLifeInfo>; }
            set { this["_ToolLifeList"] = value; }
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

                    gvToolHead.Initialize();
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
            _ToolLifeList = new List<CSTToolLifeInfo>();

            ttbToolName.Text = "";
            ttbMainHead.Text = "";

            btnOK.Enabled = false;

            gvToolHead.SetDataSource(_ToolLifeList, true);
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
                _ToolData = ToolInfo.GetToolByName(ttbToolName.Text.Trim()).ChangeTo<ToolInfoEx>();

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

                //確認刀具零組件是否為一面刀頭
                var toolType = ToolTypeInfo.GetToolTypeByType(_ToolData.ToolType).ChangeTo<ToolTypeInfoEx>();
                if (toolType.SideCount == "1")
                {
                    //刀具零組件：{0} 只有一個刀面，不能更換刀面!!
                    throw new Exception(RuleMessage.Error.C10153(ttbToolName.Text));
                }

                //確認刀具零組件是否已經下機
                var equipTools = EquipToolInfo.GetByToolName(_ToolData.ToolName);
                if (equipTools.Count > 0)
                {
                    //刀具零組件：{0} 已在機台({1})上，請先執行刀具下機!!
                    throw new Exception(RuleMessage.Error.C10152(ttbToolName.Text, equipTools[0].EquipmentName));
                }

                ////確認刀具零組件的LOCATION是否在Warehouse
                //if (_ToolData.Location != "Warehouse")
                //{
                //    //刀具零組件:{0} 不在庫房，不可執行此功能!!
                //    throw new Exception(RuleMessage.Error.C10127(ttbToolName.Text));
                //}

                //確認刀具零組件狀態是否可使用，僅IDLE可執行
                if (_ToolData.CurrentState != "IDLE")
                {
                    //刀具零組件狀態為{0}，不可執行此功能!!
                    throw new Exception(RuleMessage.Error.C10129(_ToolData.CurrentState));
                }

                //取得刀面資料清單
                var toolLifeListTmp = CSTToolLifeInfo.GetToolLifeByToolNmae(_ToolData.ToolName);
                if (toolLifeListTmp.Count == 0)
                {
                    //刀具零組件:{0} 無刀面設定資料!!
                    throw new Exception(RuleMessage.Error.C10130(_ToolData.ToolName));
                }

                //資料過濾目前設定的刀面
                _ToolLifeList = toolLifeListTmp.FindAll(p => p.Head != _ToolData.Head);

                ttbMainHead.Text = _ToolData.Head;

                gvToolHead.SetDataSource(_ToolLifeList, true);

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

                List<decimal> lsLife = new List<decimal>();

                //確認是否輸入刀具零組件
                ttbToolName.Must(lblToolName);

                #region 確認是否有勾選任一刀面設定
                string mainHead = "";
                for (int i = 0; i < gvToolHead.Rows.Count; i++)
                {
                    CheckBox ckbSelect = gvToolHead.Rows[i].FindControl("ckbSelect") as CheckBox;

                    if (ckbSelect.Checked == true)
                    {
                        mainHead = _ToolLifeList[i].Head;
                    }
                }

                if (mainHead.IsNullOrTrimEmpty())
                {
                    //[00841]請選擇一個 {0}!
                    throw new Exception(TextMessage.Error.T00841(lblMainHead.Text));
                }
                #endregion

                using (var cts = CimesTransactionScope.Create())
                {
                    //變更主刀面
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "HEAD", mainHead, txnStamp);

                    //因刀具報表需求，所以在送修時要將使用次數記錄在UDC07  
                    var toolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(_ToolData.ToolName);
                    var toolLifeData = toolLifeList.Find(p => p.Head == _ToolData.Head);
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL06", _ToolData.Head, txnStamp);
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL07", toolLifeData.UseCount.ToCimesString(), txnStamp);

                    //註記原因碼
                    var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "刀具換面");
                    if (reasonCategory == null)
                    {
                        throw new CimesException(RuleMessage.Error.C00053("CustomizeReason", "刀具換面"));
                    }
                    txnStamp.CategoryReasonCode = reasonCategory;
                    txnStamp.Remark = reasonCategory.Reason;
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

        /// <summary>
        /// DataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvToolHead_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox ttbChangeLife = e.Row.FindControl("ckbSelect") as CheckBox;

                    //绑定選中的CheckBox ClientID
                    ttbChangeLife.Attributes.Add("onclick", "javascript:Change(" + ttbChangeLife.ClientID + ")");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}