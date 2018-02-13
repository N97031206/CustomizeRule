/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：Tooling
作者：kmchen

功能說明：當刀具零組件送修回廠時，則需執行回廠作業來做回廠紀錄。

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
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.ToolRule
{
    public partial class T018 : CustomizeRuleBasePage
    {
        #region property
        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        /// <summary>
        /// 儲存檢驗報告路徑資料
        /// </summary>
        private List<WpcExClassItemInfo> _SaveInspectionData
        {
            get { return this["_SaveInspectionData"] as List<WpcExClassItemInfo>; }
            set { this["_SaveInspectionData"] = value; }
        }

        /// <summary>
        /// 刀具檢驗報告
        /// </summary>
        private List<CSTToolReportInfo> _ToolReports
        {
            get { return this["_ToolReports"] as List<CSTToolReportInfo>; }
            set { this["_ToolReports"] = value; }
        }

        /// <summary>
        /// 刀具零組件資料
        /// </summary>
        private ToolInfoEx _ToolData
        {
            get { return this["_ToolData"] as ToolInfoEx; }
            set { this["_ToolData"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private ToolTypeInfoEx _ToolType
        {
            get { return this["_ToolType"] as ToolTypeInfoEx;}
            set { this["_ToolType"] = value; }
        }
        #endregion
        
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

                    //取得路徑對應清單
                    _SaveInspectionData = GetExtendItemListByClass("SAIToolInspection");
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
            ttbToolType.Text = "";

            _ToolReports = new List<CSTToolReportInfo>();

            btnOK.Enabled = false;

            hlShowFile.NavigateUrl = "";
            hlShowFile.Text = "";
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

                //確認刀具零組件狀態是否可使用，僅REPAIR可執行
                if (_ToolData.CurrentState != "REPAIR")
                {
                    //刀具零組件狀態為{0}，不可執行此功能!!
                    throw new Exception(RuleMessage.Error.C10129(_ToolData.CurrentState));
                }

                //取得刀具型態資料
                _ToolType = ToolTypeInfo.GetToolTypeByType(_ToolData.ToolType).ChangeTo<ToolTypeInfoEx>();

                ttbToolType.Text = _ToolType.Type;

                if(_ToolType.InspectionFlag.ToBool())
                {
                    lblInspectionReport.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblInspectionReport.ForeColor = System.Drawing.Color.Black;
                }

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

                //確認檢驗報告是否需要上傳
                if (_ToolType.InspectionFlag == "Y")
                {
                    if (_ToolReports.Count == 0)
                    {
                        //刀具類型:{0} 必須上傳檢驗報告資料 !
                        throw new Exception(RuleMessage.Error.C10126(_ToolType.Type));
                    }
                }

                using (var cts = CimesTransactionScope.Create())
                {

                    #region 更新[CST_TOOL_REPAIR]的實際回廠日
                    var toolRepair = CSTToolRepairInfo.GetDataByToolName(_ToolData.ToolName);
                    toolRepair.ActualDateOfReturn = txnStamp.RecordTime.Substring(0, 10);
                    toolRepair.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

                    //新增一筆[CST_TOOL_REPAIR_LOG]
                    LogCenter.LogToDB(toolRepair, LogCenter.LogIndicator.Create(ActionType.Set, txnStamp.UserID, txnStamp.RecordTime));

                    #endregion

                    #region 將刀面使用次數歸零
                    //取得刀面資料清單
                    var toolLifes = CSTToolLifeInfo.GetToolLifeByToolNmae(_ToolData.ToolName);
                    toolLifes.ForEach(toolLife =>
                    {
                        toolLife.UseCount = 0;
                        toolLife.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    });
                    #endregion

                    var newStateInfo = ToolStateInfo.GetToolStateByState("IDLE");
                    if (newStateInfo == null)
                    {
                        //刀具零組件狀態: {0}不存在，請至配件狀態維護新增此狀態!!
                        throw new Exception(RuleMessage.Error.C10149("IDLE"));
                    }

                    //因刀具報表需求，所以在送修時要將使用次數記錄在UDC07                    
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL07", "0", txnStamp);

                    //變更狀態為IDLE
                    TMSTransaction.ChangeToolState(_ToolData, newStateInfo, txnStamp);

                    //變更LOCATION為Warehouse
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "LOCATION", "Warehouse", txnStamp);

                    //變更IDENTITY為維修品
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "IDENTITY", "維修品", txnStamp);

                    //清空預定回廠日
                    TMSTransaction.ModifyToolSystemAttribute(_ToolData, "USERDEFINECOL04", "", txnStamp);

                    //新增檢驗報告[CST_TOOL_REPORT]
                    _ToolReports.ForEach(toolReport =>
                    {
                        toolReport.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                        LogCenter.LogToDB(toolReport, LogCenter.LogIndicator.Create(ActionType.Add, txnStamp.UserID, txnStamp.RecordTime));
                    });

                    //註記原因碼
                    var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "ToolReturn");
                    txnStamp.CategoryReasonCode = reasonCategory;
                    txnStamp.Remark = reasonCategory.Reason;
                    //txnStamp.Description = string.Format("刀具零組件[{0}]，維修回廠", _ToolData.ToolName);

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
        /// 上傳檢驗報告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string toolName = "";

                toolName = ttbToolName.Text;

                if (!FileUpload1.HasFile)
                {
                    //[00854]請選擇檔案!!
                    throw new CimesException(TextMessage.Error.T00854());
                }

                var splitString = FileUpload1.PostedFile.FileName.Split('.');
                string fileName = splitString[0] + "_" + DBCenter.GetSystemDateTime().ToString("HHmmss") + "." + splitString[1];

                WindowsImpersonationContext impersonationContext = null;

                //儲存資料夾
                string saveDirPath = _SaveInspectionData[0].Remark01;

                //使用者帳號
                string userName = _SaveInspectionData[0].Remark02;

                //使用者密碼
                string password = _SaveInspectionData[0].Remark03;

                //實際儲存檔案路徑
                string saveFilePath = saveDirPath + "\\" + toolName + "\\" + fileName;

                //如果使用者帳號不為空白才執行切換帳號功能
                if (userName.IsNullOrTrimEmpty() == false)
                {
                    //切換使用者
                    var tokenHandle = Impersonate(userName, password, "domain");
                    WindowsIdentity windowsIdentity = new WindowsIdentity(tokenHandle);
                    impersonationContext = windowsIdentity.Impersonate();
                }

                //判斷儲存路徑是否存在
                if (Directory.Exists(saveDirPath + "\\" + toolName) == false)
                {
                    //如果不存在，則新增一個資料夾路徑
                    Directory.CreateDirectory(saveDirPath + "\\" + toolName);
                }

                //儲存檔案
                FileUpload1.SaveAs(saveFilePath);

                if (impersonationContext != null)
                {
                    //切換回原身分
                    impersonationContext.Undo();
                }

                //新增一筆檢驗報告資料
                var toolReport = InfoCenter.Create<CSTToolReportInfo>();
                toolReport.ToolName = toolName;
                toolReport.FileName = saveFilePath;

                _ToolReports.Add(toolReport);

                //檔案名稱:{0}已上傳完成 !
                _ProgramInformationBlock.ShowMessage(RuleMessage.Hint.C10125(fileName));

                //依據檔案名稱，顯示檔案名稱的連結
                BindFileData(toolReport.FileName, fileName);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 依據檔案名稱，顯示檔案名稱的連結
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="fileName">檔案名稱</param>
        public void BindFileData(string filePath, string fileName)
        {
            //取得路徑對應清單
            var _SaveReportData = GetExtendItemListByClass("SAIToolInspection");

            string sHost = Request.Url.Host;
            string sApplicationPath = Request.ApplicationPath;
            string picturePath = "http://" + sHost + sApplicationPath + "/ToolReport";
            string urlPasth = filePath.Replace(_SaveReportData[0].Remark01, picturePath).Replace(@"\", "/");

            hlShowFile.NavigateUrl = urlPasth;
            hlShowFile.Text = fileName;
        }

        /// <summary>
        /// 切換使用者權限
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private IntPtr Impersonate(string userName, string password, string domain)
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
            IntPtr tokenHandle = new IntPtr(0);
            tokenHandle = IntPtr.Zero;

            bool returnValue = LogonUser(userName, domain, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref tokenHandle);

            if (returnValue == false)
            {
                //切換使用者權限失敗(使用者:{0})！
                throw new Exception(RuleMessage.Error.C10072(userName));
            }

            return tokenHandle;
        }

        /// <summary>
        /// 取得系統控制設定的類別資料
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private List<WpcExClassItemInfo> GetExtendItemListByClass(string className)
        {
            var dataList = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks(className);

            if (dataList.Count == 0)
            {
                // [00466]系統控制設定，類別：[{0}]，項目：[{1}]未設定！
                throw new Exception(TextMessage.Error.T00466(className, ""));
            }

            return dataList;
        }
    }
}