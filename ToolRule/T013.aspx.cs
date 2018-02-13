
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
    public partial class T013 : CustomizeRuleBasePage
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
        /// 機台資料
        /// </summary>
        private EquipmentInfo _EquipData
        {
            get { return this["_EquipData"] as EquipmentInfo; }
            set { this["_EquipData"] = value; }
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
                    AjaxFocus(ttbEquipment);                    
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

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearField()
        {
            _EquipData = null;
            _ToolData = null;

            ttbEquipment.Text = "";
            ttbToolName.Text = "";

            _ToolGroupData = new List<ToolInfoEx>();
            gvQuery.SetDataSource(_ToolGroupData, true);

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
                _EquipData = null;
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

                //尚未定義機台為何狀態不可執行刀具上機
                //..
                //..

                AjaxFocus(ttbToolName);
            }
            catch (Exception ex)
            {
                _EquipData = null;
                ttbEquipment.Text = "";
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
                _ToolData = ToolInfo.GetToolByName(ttbToolName.Text.Trim());

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
                    // [C10131]刀具零組件：{0}未領用、不能上機!
                    throw new Exception(RuleMessage.Error.C10131(ttbToolName.Text));
                }
                #endregion

                # region 搜尋相同 GroupID 的刀具
                _ToolGroupData = ToolInfoEx.GetToolListByGroupID(_ToolData["GROUPID"].ToString());
                
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

                    //確認刀具零組件狀態是否可使用，僅IDLE可執行
                    if (_ToolData.CurrentState != "IDLE")
                    {
                        //刀具零組件狀態為{0}，不可執行此功能!!
                        throw new Exception(RuleMessage.Error.C10129(_ToolData.CurrentState));
                    }

                    #region 驗證模治具是否在別的機台上，如是要報錯
                    var equipToolDataList = EquipToolInfo.GetByToolName(tool.ToolName);

                    if (equipToolDataList.Count > 0)
                    {
                        //[C10132]刀具零組件：{0}已在機台：{1}上，不可再上機!
                        throw new Exception(RuleMessage.Error.C10132(tool.ToolName, equipToolDataList[0].EquipmentName));
                    }
                    #endregion

                }

                #endregion

                gvQuery.SetDataSource(_ToolGroupData, true);
                btnOK.Enabled = true;
                ttbToolName.Text = "";
                AjaxFocus(ttbToolName);
            }
            catch (Exception ex)
            {
                _ToolData = null;
                ttbToolName.Text = "";
                _ToolGroupData = new List<ToolInfoEx>();
                gvQuery.SetDataSource(_ToolGroupData, true);

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

                //確認是否輸入機台
                ttbEquipment.Must(lblEquipment);

                //確認是否輸入刀具零組件
                if (_ToolGroupData.Count == 0)
                {
                    throw new Exception(TextMessage.Error.T00826(GetUIResource("ToolParts")));
                }

                using (var cts = CimesTransactionScope.Create())
                {
                    _ToolGroupData.ChangeTo<ToolInfo>();

                    _ToolGroupData.ForEach(toolData =>
                    {
                        //取得刀具狀態資料
                        var newStateInfo = ToolStateInfo.GetToolStateByState("USED");
                        if (newStateInfo == null)
                        {
                            //刀具零組件狀態: {0}不存在，請至配件狀態維護新增此狀態!!
                            throw new Exception(RuleMessage.Error.C10149("USED"));
                        }

                        //因刀具報表需求，所以在上機時要將當下的使用次數記錄在UDC07
                        var toolLifeList = CSTToolLifeInfo.GetToolLifeByToolNmae(toolData.ToolName);
                        var toolLife = toolLifeList.Find(p => p.Head == toolData.Head);
                        TMSTransaction.ModifyToolSystemAttribute(toolData, "USERDEFINECOL07", toolLife.UseCount.ToCimesString(), txnStamp);

                        //變更配件狀態為USED
                        TMSTransaction.ChangeToolState(toolData, newStateInfo, txnStamp);
                        
                        //變更模具上的儲位為機台編號
                        TMSTransaction.ModifyToolSystemAttribute(toolData, "Location", _EquipData.EquipmentName, txnStamp);

                        if (toolData.Identity == CustomizeFunction.ToolIdentity.新品.ToString())
                        {
                            //變更IDENTITY為堪用
                            TMSTransaction.ModifyToolSystemAttribute(toolData, "IDENTITY", CustomizeFunction.ToolIdentity.堪用.ToString(), txnStamp);
                        }
                    });

                    //配件上機台
                    TMSTxn.Default.AddToolToEquipment(_ToolGroupData.ChangeTo<ToolInfo>(), _EquipData, txnStamp);

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