
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
    public partial class T014 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 刀具零組件資料
        /// </summary>
        private List<ToolInfoEx> _ToolList
        {
            get { return this["_ToolList"] as List<ToolInfoEx>; }
            set { this["_ToolList"] = value; }
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

                if (!IsPostBack)
                {
                    //第一次開啟頁面
                    ClearField();
                    AjaxFocus(ttbEquipment);
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
            _EquipData = null;

            ttbEquipment.Text = "";
            ttbToolName.Text = "";

            _ToolList = new List<ToolInfoEx>();
            gvQuery.SetDataSource(_ToolList, true);

            btnOK.Enabled = false;

        }

        /// <summary>
        /// 輸入機台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbEquipment_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //清除資料
                _EquipData = null;
                ttbToolName.Text = "";
                _ToolList = new List<ToolInfoEx>();
                gvQuery.SetDataSource(_ToolList, true);

                //確認機台編號是否有輸入
                ttbEquipment.Must(lblEquipment);

                _EquipData = EquipmentInfo.GetEquipmentByName(ttbEquipment.Text);

                #region 驗證機台正確性
                if (_EquipData == null)
                {
                    //[00885]機台{0}不存在！
                    throw new Exception(TextMessage.Error.T00885(ttbEquipment.Text));
                }
                #endregion

                #region 驗證機台啟用狀態
                if (_EquipData.UsingStatus == UsingStatus.Disable)
                {
                    //機台：{0}已停用，如需使用，請至"機台資料維護"啟用!!
                    throw new Exception(RuleMessage.Error.C10025(_EquipData.EquipmentName));
                }
                #endregion

                #region 取得已上機的刀具資料
                var equipTools = EquipToolInfo.GetByEquipmentName(_EquipData.EquipmentName);
                if (equipTools.Count == 0)
                {
                    //機台編號:{0} 沒有刀具上機資料!!
                    throw new Exception(RuleMessage.Error.C10147(_EquipData.EquipmentName));
                }

                equipTools.ForEach(equipTool =>
                {
                    //取得刀具資料
                    var toolData = ToolInfo.GetToolByName(equipTool.ToolName).ChangeTo<ToolInfoEx>();

                    //確認刀具零組件是否存在
                    if (toolData == null)
                    {
                        // [00030]{0}：{1}不存在!
                        throw new Exception(TextMessage.Error.T00030(lblToolName.Text, ttbToolName.Text));
                    }

                    //確認刀具零組件狀態是否可使用，僅USED可執行
                    if (toolData.CurrentState != "USED")
                    {
                        //刀具零組件狀態為{0}，不可執行此功能!!
                        throw new Exception(RuleMessage.Error.C10129(toolData.CurrentState));
                    }

                    #region 確認刀具零組件是否啟用
                    if (toolData.UsingStatus == UsingStatus.Disable)
                    {
                        //刀具零組件：{0}已停用，如需使用，請至"刀具零組件進料作業"啟用!!
                        throw new Exception(RuleMessage.Error.C10128(toolData.ToolName));
                    }
                    #endregion

                    _ToolList.Add(toolData);
                });

                #endregion

                gvQuery.SetDataSource(_ToolList, true);
                btnOK.Enabled = true;

                AjaxFocus(ttbToolName);
            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbEquipment);
                HandleError(ex);
            }
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
                //確認機台編號是否有輸入
                ttbEquipment.Must(lblEquipment);

                //確認是否有輸入刀具名稱
                ttbToolName.Must(lblToolName);

                //確認輸入的刀具名稱是否存在於已上機清單內 
                var index = _ToolList.FindIndex(toolData => toolData.ToolName == ttbToolName.Text);
                if (index == -1)
                {
                    //刀具零組件:{0} 不在上機清單內!!
                    throw new Exception(RuleMessage.Error.C10148(ttbToolName.Text));
                }

                //勾選輸入的刀具項目
                var ckbSelect = (CheckBox)gvQuery.Rows[index].FindControl("ckbSelect");
                ckbSelect.Checked = true;

                ttbToolName.Text = "";
            }
            catch (Exception ex)
            {
                ttbToolName.Text = "";
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
                //確認是否輸入機台
                ttbEquipment.Must(lblEquipment);

                int checkedCount = 0;

                #region 確認是否有勾選資料
                for (int i = 0; i < gvQuery.Rows.Count; i++)
                {
                    var thisCheckBox = (CheckBox)gvQuery.Rows[i].FindControl("ckbSelect");
                    if (thisCheckBox.Checked)
                    {
                        checkedCount++;
                    }
                }

                if(checkedCount == 0)
                {
                    // [00816]請至少選取一個{0}！
                    throw new Exception(TextMessage.Error.T00816(GetUIResource("ToolName")));
                }
                #endregion

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    //新的群組ID
                    string newGroupID = DBCenter.GetSystemID();

                    for (int i = 0; i < gvQuery.Rows.Count; i++)
                    {
                        var thisCheckBox = (CheckBox)gvQuery.Rows[i].FindControl("ckbSelect");

                        //只有勾選的刀具才要下機
                        if (thisCheckBox.Checked)
                        {
                            //因刀具報表需求，所以在下機時要將當下的使用次數記錄在UDC07
                            var toolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(_ToolList[i].ToolName);
                            var toolLife = toolLifeList.Find(p => p.Head == _ToolList[i].Head);
                            TMSTransaction.ModifyToolSystemAttribute(_ToolList[i], "USERDEFINECOL07", toolLife.UseCount.ToCimesString(), txnStamp);

                            //因為下機後在tool_hist會沒有機台資訊，所以在下機前把機台資訊記到remark(刀具報表需要用到)
                            txnStamp.Remark = _EquipData.EquipmentName;
                            //刀具下機台
                            TMSTxn.Default.RemoveToolFromEquipment(_ToolList[i], _EquipData, txnStamp);

                            //變更刀具 GROUPID
                            TMSTransaction.ModifyToolSystemAttribute(_ToolList[i], "GROUPID", newGroupID, txnStamp);

                            //變更儲位為Hub
                            TMSTransaction.ModifyToolSystemAttribute(_ToolList[i], "Location", "Hub", txnStamp);

                            //取得刀具狀態資料
                            var newStateInfo = ToolStateInfo.GetToolStateByState("IDLE");
                            if (newStateInfo == null)
                            {
                                //刀具零組件狀態: {0}不存在，請至配件狀態維護新增此狀態!!
                                throw new Exception(RuleMessage.Error.C10149("IDLE"));
                            }

                            //變更配件狀態為USED
                            TMSTransaction.ChangeToolState(_ToolList[i], newStateInfo, txnStamp);
                        }
                    }

                    //依據機台編號資料刪除[CST_EQP_TOOL_RELEASE]
                    var lstEqpToolRelease = CSTEquipmentToolReleaseInfo.GetDataByEquipment(_EquipData.EquipmentName);
                    lstEqpToolRelease.ForEach(p => {
                        p.DeleteFromDB();
                        LogCenter.LogToDB(p, LogCenter.LogIndicator.Create(ActionType.Remove, txnStamp.UserID, txnStamp.RecordTime));
                    });

                    //依據機台編號資料刪除[CST_EQP_DEVICE_RELEASE]
                    var lstEqpDeviceRelease = CSTEquipmentDeviceReleaseInfo.GetDataByEquipment(_EquipData.EquipmentName);
                    lstEqpDeviceRelease.ForEach(p => {
                        p.DeleteFromDB();
                        LogCenter.LogToDB(p, LogCenter.LogIndicator.Create(ActionType.Remove, txnStamp.UserID, txnStamp.RecordTime));
                    });

                    cts.Complete();
                }

                ClearField();

                AjaxFocus(ttbEquipment);

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