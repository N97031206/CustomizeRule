/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：

說明：列印入庫清單。
------------------------------------------------------------------

*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using CrystalDecisions.CrystalReports.Engine;
using ciMes.Web.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;

namespace CustomizeRule.ToolRule
{
    public partial class T022 : CimesRuleBasePage
    {
        // 宣告一 ReportDocument 物件,用來接Crystal Report產生的rpd檔
        ReportDocument rptdoc = new ReportDocument();

        private ToolInfo toolData
        {
            get
            {
                return (ToolInfo)this["toolData"];
            }
            set
            {
                this["toolData"] = value;
            }
        }

        private DataSet _dsReport
        {
            get
            {
                return (DataSet)this["_dsReport"];
            }
            set
            {
                this["_dsReport"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // 第一次進入網頁
                if (!IsPostBack)
                {
                    // 顯示畫面各欄位的Default Value
                    LoadControlDefault();
                    AjaxFocus(ttbToolName);
                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected override void VCFailure(object sender, EventArgs e)
        {
        }

        protected override void VCSuccess(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CurrentUpdatePanel = this.UpdatePanel1;
        }

        /// <summary>
        /// 清除畫面欄位
        /// </summary>
        private void ClearField()
        {
            btnPrint.Enabled = false;
            if (_dsReport != null)
                _dsReport.Tables.Clear();
        }

        /// <summary>
        /// 載入與 Uesr 輸入資料無關的欄位資料
        /// </summary>
        private void LoadControlDefault()
        {
            // 無此相關欄位
            btnPrint.Enabled = false;

            _dsReport = new DataSet();
        }

        /// <summary>
        /// 依照輸入LotID取得相關欄位資料及相關Button設定
        /// </summary>
        /// <param name="LotID">批號名稱</param>
        private void LoadControlByLot(string toolname)
        {
            // 清除欄位資訊
            ClearField();

            var toolInfo = ToolInfo.GetToolByName(toolname);


            // 若該批號無資料可顯示,離開程式並顯示訊息
            if (toolInfo == null)
            {
                btnPrint.Enabled = false;
                AjaxFocus(ttbToolName);
                throw new Exception(TextMessage.Error.T00060(toolname));
            }

            btnPrint.Enabled = true;

            toolData = toolInfo;
        }

        /// <summary>
        /// 系統事件 : btnPrint Click時觸發
        /// 將產生的ReportDocument 匯出成為PDF格式的檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolData == null)
                {
                    throw new Exception(TextMessage.Error.T00060(""));
                }
                _dsReport.Tables.Clear();

                // 取得Report資料
                _dsReport = GetRunCardDataSource(toolData);
                _dsReport.AcceptChanges();

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    var printLotInfo = InfoCenter.Create<CSTToolLabelPrintLogInfo>();
                    printLotInfo.ToolName = toolData.ToolName;
                    printLotInfo.RuleName = ProgramRight;
                    printLotInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    cts.Complete();
                }

                if (_dsReport.Tables.Count > 0)
                {
                    string sPrintProgram = "/CustomizeRule/ToolRule/T022View.aspx";
                    string sHost = Request.Url.Host;
                    string sApplicationPath = Request.ApplicationPath;
                    string ReportPath = "http://" + sHost + sApplicationPath + sPrintProgram;
                    Session["T022View"] = _dsReport;
                    //開啟查詢工單頁面
                    string openPrintWindow = "window.open('" + ReportPath + "','pop','resizable: yes; status: no; scrollbars:no; menubar:no;toolbar:no;location:no;dialogLeft:10px;dialogTop:10px;dialogHeight:10px;dialogWidth:10px',false);";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), openPrintWindow, true);
                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                // 執行完畢之後, 回到前一次進入此規則的查詢畫面
                Response.Redirect(ciMes.Security.UserSetting.GetPreviousListPage(this));
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void ttbToolName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ttbToolName.Must(lblMillName);
                ttbToolName.Text = ttbToolName.Text.Trim();
                LoadControlByLot(ttbToolName.Text);
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        /// <summary>
        /// 取的runcard的資料來源
        /// </summary>
        /// <param name="LotDataList"></param>
        /// <returns></returns>
        private DataSet GetRunCardDataSource(ToolInfo toolData)
        {
            #region 定義 LOTDATA 資料表
            DataTable dtToolData = toolData.CopyDataToTable("TOOLData");
            dtToolData.Columns.Add("ToolTypeDescr");
            #endregion            
            var toolTypeInfo = ToolTypeInfo.GetToolTypeByType(toolData.ToolType);
            dtToolData.Rows[0]["ToolTypeDescr"] = toolTypeInfo.Description;
            DataSet dsReportData = new DataSet();
            dsReportData.Tables.Add(dtToolData);

            return dsReportData;
        }
    }
}
