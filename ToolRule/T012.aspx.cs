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
    public partial class T012 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 刀具零組件資料
        /// </summary>
        private List<ToolInfoEx> _ToolGroupData
        {
            get { return this["_ToolGroupData"] as List<ToolInfoEx>; }
            set { this["_ToolGroupData"] = value; }
        }

        /// <summary>
        /// 刀具資料
        /// </summary>
        private ToolInfo _ToolData
        {
            get { return this["_ToolData"] as ToolInfo; }
            set { this["_ToolData"] = value; }
        }
        /// <summary>
        ///刀具歸還原因
        /// </summary>
        private ReasonCategoryInfo _ReasonCategory
        {
            get { return this["_ReasonCategory"] as ReasonCategoryInfo; }
            set { this["_ReasonCategory"] = value; }
        }
        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    //第一次開啟頁面
                    ClearField();
                    AjaxFocus(ttbToolName);
                    LoadReason();
                }
                else
                {
                    gvQuery.SetDataSource(_ToolGroupData, true);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void LoadReason()
        {
            _ReasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "刀具歸還");
            if (_ReasonCategory == null)
            {
                throw new CimesException(RuleMessage.Error.C00053("CustomizeReason", "刀具歸還"));
            }
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearField()
        {
            _ToolData = null;

            ttbToolName.Text = "";

            _ToolGroupData = new List<ToolInfoEx>();
            gvQuery.SetDataSource(_ToolGroupData, true);

            btnOK.Enabled = false;

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
                _ToolData = ToolInfo.GetToolByName(ttbToolName.Text);

                # region 確認刀具零組件是否存在
                if (_ToolData == null)
                {
                    // [00030]{0}：{1}不存在!
                    throw new Exception(TextMessage.Error.T00030(lblToolName.Text, ttbToolName.Text));
                }
                #endregion

                #region 搜尋相同 GroupID 的刀具
                if (_ToolData["GROUPID"].ToString() == null || _ToolData["GROUPID"].ToString() == "")
                {
                    // [C10131]刀具零組件：{0}沒有刀具群組編號資訊!
                    //throw new Exception(RuleMessage.Error.C10131(ttbToolName.Text));

                    _ToolGroupData = new List<ToolInfoEx>();
                    _ToolGroupData.Add(_ToolData.ChangeTo<ToolInfoEx>());
                }
                else
                {
                    _ToolGroupData = ToolInfoEx.GetToolListByGroupID(_ToolData["GROUPID"].ToString());
                }

                foreach(var tool in _ToolGroupData)
                {
                    //尚未確認刀具在產線的判斷

                    #region 確認刀具零組件是否啟用
                    if (tool.UsingStatus == UsingStatus.Disable)
                    {
                        //刀具零組件：{0}已停用，如需使用，請至"刀具零組件進料作業"啟用!!
                        throw new Exception(RuleMessage.Error.C10128(tool.ToolName));
                    }
                    #endregion

                    #region 確認刀具零組件的LOCATION是否為Hub
                    if (tool.Location != "Hub")
                    {
                        //{0}: {1} 狀態不為: {2}，不可使用!
                        throw new Exception(RuleMessage.Error.C00006(lblToolName.Text, tool.ToolName, "Hub"));
                    }
                    #endregion


                    #region 驗證模治具是否在別的機台上，如是要報錯
                    var equipToolDataList = EquipToolInfo.GetByToolName(tool.ToolName);

                    if (equipToolDataList.Count > 0)
                    {
                        //[C10138]刀具零組件：{0}已在機台：{1}上，不可執行刀具零組件歸還!
                        throw new Exception(RuleMessage.Error.C10138(tool.ToolName, equipToolDataList[0].EquipmentName));
                    }
                    #endregion

                }

                #endregion

                gvQuery.SetDataSource(_ToolGroupData, true);
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

                using (var cts = CimesTransactionScope.Create())
                {
                    _ToolGroupData.ForEach(tool =>
                    {
                        // 清空GROUPID
                        // Location寫為"Warehouse"
                        // 確認後寫入 Comment
                        var toolInfo = ToolInfo.GetToolByName(tool.ToolName).ChangeTo<ToolInfoEx>();
                        var modifyAttrList = new List<ModifyAttributeInfo>();

                        //因刀具報表需求，所以在歸還時要AddComment，並將使用次數記錄在UDC07
                        var toolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(toolInfo.ToolName);
                        var toolLife = toolLifeList.Find(p => p.Head == toolInfo.Head);
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("USERDEFINECOL07", toolLife.UseCount));
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("USERDEFINECOL08", ""));
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("GROUPID", "")); 
                        modifyAttrList.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("LOCATION", "Warehouse"));
                        TMSTransaction.ModifyToolMultipleAttribute(toolInfo, modifyAttrList, txnStamp);

                        txnStamp.Remark = _ReasonCategory.Reason;
                        txnStamp.CategoryReasonCode = _ReasonCategory;
                        txnStamp.Description = "";
                        TMSTransaction.AddToolComment(toolInfo, txnStamp);
                    });

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
                // 返回在製品查詢頁面
                ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}