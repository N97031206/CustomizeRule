/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：

說明：供其他程式來呼叫列印流程卡
------------------------------------------------------------------

*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

using ciMes.Web.Common;
using Ares.Cimes.IntelliService;

namespace ciMesRule.WIPRule
{
    public partial class W006View : CimesRuleBasePage
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rptdoc;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RrintToPDF();
            }
            catch (Exception E)
            {
                ShowMessage(E.Message);
            }

            finally
            {
                #region 刪除舊有XLS、PDF檔案(24個小時前的資料)
                DateTime dtTime = DateTime.Now;
                dtTime = dtTime.AddHours(-24);
                DirectoryInfo di = new DirectoryInfo(ConfigurationSettings.AppSettings["reportPath"]);
                if (di != null)
                {
                    foreach (FileSystemInfo fsi in di.GetFiles())
                    {
                        if (fsi.CreationTime < dtTime)
                        {
                            switch (fsi.Extension.ToUpper())
                            {
                                case ".PDF":
                                case ".XLS":
                                    fsi.Delete();
                                    break;
                            }
                        }
                    }
                }
                #endregion
            }
        }

        protected override void VCFailure(object sender, EventArgs e)
        {

        }

        protected override void VCSuccess(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 產生PDF檔案
        /// </summary>
        private void RrintToPDF()
        {

            // 匯出檔名的設定
            string sReportFileName = @"\rpt\W006_rpt.rpt";
            string sMapPath = Server.MapPath("");
            string sHost = Request.Url.Host;
            string sApplicationPath = Request.ApplicationPath;
            string reportID = DBCenter.GetSystemID();

            string sExportFileName = Server.MapPath("../../") + @"PDF\W006_rpt" + reportID + ".pdf";
            string strPath = sMapPath + sReportFileName;
            string ReportPath = "http://" + sHost + sApplicationPath + "/PDF/W006_rpt" + reportID + ".pdf";

            DataSet dsLot = (DataSet)Session["W006View"];

            rptdoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            rptdoc.Load(strPath);
            rptdoc.SetDataSource(dsLot);

            // 設定匯出路徑及檔名
            DiskFileDestinationOptions df = new DiskFileDestinationOptions();
            df.DiskFileName = sExportFileName;
            rptdoc.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            rptdoc.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
            rptdoc.ExportOptions.DestinationOptions = df;
            rptdoc.Export();
            rptdoc.Dispose();

            //打開文件
            Response.Write("<script>window.open('" + ReportPath + "','_blank','resizable,scrollbars=no,menubar=no,toolbar=no,location=no,status=no',false);</script> ");

            Response.Write("<script>window.close();</script>");

        }

        private void ShowMessage(string message)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), Session.SessionID, "alert('" + message + "');", true);
        }
    }
}
