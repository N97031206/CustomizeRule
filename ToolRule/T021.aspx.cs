
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
    public partial class T021 : CustomizeRuleBasePage
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

                #region 確認刀具零組件是否有 GROUPID
                if (_ToolData["GROUPID"].ToString() == null || _ToolData["GROUPID"].ToString() == "")
                {
                    // [C10131]刀具零組件：{0}沒有刀具群組編號資訊!
                    throw new Exception(RuleMessage.Error.C10131(ttbToolName.Text));
                }
                #endregion

                # region 搜尋相同 GroupID 的刀具
                _ToolGroupData = ToolInfoEx.GetToolListByGroupID(_ToolData["GROUPID"].ToString());

                foreach (var tool in _ToolGroupData)
                {
                    #region 確認刀具零組件是否啟用
                    if (tool.UsingStatus == UsingStatus.Disable)
                    {
                        //刀具零組件：{0}已停用，如需使用，請至"刀具零組件進料作業"啟用!!
                        throw new Exception(RuleMessage.Error.C10128(tool.ToolName));
                    }
                    #endregion

                    #region 驗證刀具是否在機台上，如是要報錯
                    var equipToolDataList = EquipToolInfo.GetByToolName(tool.ToolName);

                    if (equipToolDataList.Count > 0)
                    {
                        //[C10135]刀具零組件：{0}已在機台：{1}上，不可拆群組!
                        throw new Exception(RuleMessage.Error.C10135(tool.ToolName, equipToolDataList[0].EquipmentName));
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
        /// 全勾選/全不勾選觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ckbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox ckbSelectAll = gvQuery.HeaderRow.FindControl("ckbSelectAll") as CheckBox;
                foreach (GridViewRow gvRow in gvQuery.Rows)
                {
                    CheckBox ckbSelect = gvRow.FindControl("ckbSelect") as CheckBox;
                    ckbSelect.Checked = ckbSelectAll.Checked;
                }
            }
            catch (Exception ex)
            {
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
                List<ToolInfo> checkedToolDataList = new List<ToolInfo>(); //勾選的刀具列表

                //確認是否輸入刀具零組件
                ttbToolName.Must(lblToolName);

                #region 判斷此Group刀具是否全選
                var groupData = ToolInfoEx.GetToolListByGroupID(_ToolData["GROUPID"].ToString());

                //搜尋勾選刀具
                for (int i = 0; i < gvQuery.Rows.Count; i++)
                {
                    var thisCheckBox = (CheckBox)gvQuery.Rows[i].FindControl("ckbSelect");

                    //勾選的刀具準備產生新群組
                    if (thisCheckBox.Checked)
                    {
                        //#region 確認刀具零組件是否啟用
                        //if (_ToolGroupData[i].UsingStatus == UsingStatus.Disable)
                        //{
                        //    //刀具零組件：{0}已停用，如需使用，請至"刀具零組件進料作業"啟用!!
                        //    throw new Exception(RuleMessage.Error.C10128(_ToolGroupData[i].ToolName));
                        //}
                        //#endregion

                        //#region 驗證刀具是否在機台上，如是要報錯
                        //var equipToolDataList = EquipToolInfo.GetByToolName(_ToolGroupData[i].ToolName);

                        //if (equipToolDataList.Count > 0)
                        //{
                        //    //[C10135]刀具零組件：{0}已在機台：{1}上，不可拆群組!
                        //    throw new Exception(RuleMessage.Error.C10135(_ToolGroupData[i].ToolName, equipToolDataList[0].EquipmentName));
                        //}
                        //#endregion

                        checkedToolDataList.Add(_ToolGroupData[i]);
                    }
                }

                //確認是否有勾選資料
                if(checkedToolDataList.Count == 0)
                {
                    // [00816]請至少選取一個{0}！
                    throw new Exception(TextMessage.Error.T00816(GetUIResource("ToolName")));
                }

                //檢查不能全部勾選
                if (groupData.Count == checkedToolDataList.Count)
                {
                    //[C10136] 刀具零組件已被全部勾選，無法拆群組!
                    throw new Exception(RuleMessage.Error.C10136());
                }

                #endregion

                using (var cts = CimesTransactionScope.Create())
                {
                    string groupid = DBCenter.GetSystemID();

                    foreach (var tool in checkedToolDataList)
                    {
                        //變更刀具 GROUPID 產生新群組
                        TMSTransaction.ModifyToolSystemAttribute(tool, "GROUPID", groupid, txnStamp);
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