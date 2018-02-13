/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：kmchen

功能說明：設定刀具型態時會決定每支刀的刀壽，但有時實際作業才發現刀具的使用次數高於設定值，可透過該作業調整該把刀具的刀壽。

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
    public partial class T019 : CustomizeRuleBasePage
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

                ////確認刀具零組件的LOCATION是否在Warehouse
                //if (_ToolData.Location != "Warehouse")
                //{
                //    //刀具零組件:{0} 不在庫房，不可執行此功能!!
                //    throw new Exception(RuleMessage.Error.C10127(ttbToolName.Text));
                //}

                ////確認刀具零組件狀態是否可使用，僅IDLE可執行
                //if (_ToolData.CurrentState != "IDLE")
                //{
                //    //刀具零組件狀態為{0}，不可執行此功能!!
                //    throw new Exception(RuleMessage.Error.C10129(_ToolData.CurrentState));
                //}

                //取得刀面資料清單
                _ToolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(_ToolData.ToolName);
                if (_ToolLifeList.Count == 0)
                {
                    //刀具零組件:{0} 無刀面設定資料!!
                    throw new Exception(RuleMessage.Error.C10130(_ToolData.ToolName));
                }

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

                //確認變更刀壽次數資料是否正確
                for (int i = 0; i < gvToolHead.Rows.Count; i++)
                {
                    TextBox ttbChangeLife = gvToolHead.Rows[i].FindControl("ttbChangeLife") as TextBox;

                    //確認刀壽是否為正整數
                    ttbChangeLife.MustInt(GetUIResource("ChangeLife"));
                    int lifeCount = Convert.ToInt32(ttbChangeLife.Text);
                    if (lifeCount <= 0)
                    {
                        AjaxFocus(ttbChangeLife);

                        //[00916]輸入值必須為數字且必須大於等於0!!
                        throw new Exception(TextMessage.Error.T00916());
                    }

                    lsLife.Add(lifeCount.ToCimesDecimal());
                }

                using (var cts = CimesTransactionScope.Create())
                {
                    for (int i = 0; i < gvToolHead.Rows.Count; i++)
                    {
                        //原刀壽次數
                        string life = _ToolLifeList[i].Life.ToString();

                        //更新刀壽資料
                        _ToolLifeList[i].Life = lsLife[i];
                        _ToolLifeList[i].UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

                        //紀錄歷史紀錄[CST_TOOL_TYPE_LIFE_LOG]
                        LogCenter.LogToDB(_ToolLifeList[i], LogCenter.LogIndicator.Create(ActionType.Set, txnStamp.UserID, txnStamp.RecordTime));

                        //註記原因碼
                        var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "AlterToolLife");
                        txnStamp.CategoryReasonCode = reasonCategory;
                        //txnStamp.Description = string.Format("刀具零組件[{0}]，刀面[{1}]，原刀壽次數[{2}]，設定刀壽次數[{3}]",
                        //    _ToolData.ToolName, _ToolLifeList[i].Head, life, _ToolLifeList[i].Life.ToString());

                        //備註
                        TMSTransaction.AddToolComment(_ToolData, txnStamp);
                    }
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
                    TextBox ttbChangeLife = e.Row.FindControl("ttbChangeLife") as TextBox;

                    ttbChangeLife.Text = _ToolLifeList[e.Row.DataItemIndex].Life.ToString();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}