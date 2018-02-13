using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using ciMes.Web.Common;
using ciMes.Web.Common.UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.ToolRule
{
    public partial class T008Report : CimesBasePage
    {
        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        /// <summary>
        /// 儲存路徑資料
        /// </summary>
        private List<WpcExClassItemInfo> _SaveReportData
        {
            get { return this["_SaveReportData"] as List<WpcExClassItemInfo>; }
            set { this["_SaveReportData"] = value; }
        }

        /// <summary>
        /// 刀具檢驗清單
        /// </summary>
        private List<CSTToolReportInfo> _ToolReports
        {
            get { return this["_ToolReports"] as List<CSTToolReportInfo>; }
            set { this["_ToolReports"] = value; }
        }

        /// <summary>
        /// 傳入的ToolName
        /// </summary>
        private string _CurrentToolName
        {
            get
            {
                if (Request["ToolName"].IsNullOrTrimEmpty())
                {
                    throw new Exception(TextMessage.Error.T00045(GetUIResource("ToolName")));
                }

                return Request["ToolName"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    gvToolReport.Initialize();

                    //取得路徑對應清單
                    _SaveReportData = GetExtendItemListByClass("SAIToolInspection");

                    GetToolReport(_CurrentToolName);
                }
                else
                {

                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        /// <summary>
        /// //取得刀具檢驗資料
        /// </summary>
        /// <param name="toolName"></param>
        private void GetToolReport(string toolName)
        {
            //清除資料
            _ToolReports = new List<CSTToolReportInfo>();

            //取得刀具檢驗清單
            _ToolReports = InfoCenter.GetList<CSTToolReportInfo>("SELECT * FROM CST_TOOL_REPORT WHERE TOOLNAME = #[STRING] ORDER BY UPDATETIME DESC", toolName);

            gvToolReport.SetDataSource(_ToolReports, true);
        }

        protected void gvToolReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                //var lblFileName = e.Row.FindControl("lblFileName") as Label;
                var lblUpdateTime = e.Row.FindControl("lblUpdateTime") as Label;
                var btnOpenFile = e.Row.FindControl("btnOpenFile") as LinkButton;

                string filePath = _ToolReports[e.Row.DataItemIndex].FileName;
                string updateTime = _ToolReports[e.Row.DataItemIndex].UpdateTime;

                //string sHost = Request.Url.Host;
                //string sApplicationPath = Request.ApplicationPath;
                //string picturePath = "http://" + sHost + sApplicationPath + "/ToolReport";
                //var url = filePath.Replace(_SaveReportData[0].Remark01, picturePath).Replace(@"\", "/");

                var text = filePath.Replace(_SaveReportData[0].Remark01 + "\\" + _CurrentToolName + "\\", "");

                //lblFileName.Text = text;
                lblUpdateTime.Text = updateTime;
                btnOpenFile.Text = text;
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        ///// <summary>
        ///// 點擊
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnOpen_Click(object sender, EventArgs e)
        //{
        //    var row = ((Button)sender).Parent.Parent as GridViewRow;

        //    string url = "";
        //    string filePath = _ToolReports[row.DataItemIndex].FileName;
        //    string sHost = Request.Url.Host;
        //    string sApplicationPath = Request.ApplicationPath;
        //    string picturePath = "http://" + sHost + sApplicationPath + "/ToolReport";
        //    url = filePath.Replace(_SaveReportData[0].Remark01, picturePath).Replace(@"\", "/");

        //    //打開文件
        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
        //        Session.SessionID, "window.open('" + url + "','_blank','resizable,scrollbars=no,menubar=no,toolbar=no,location=no,status=no',false);", true);
        //}

        /// <summary>
        /// 開啟檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOpenFile_Click(object sender, EventArgs e)
        {
            var row = ((LinkButton)sender).Parent.Parent as GridViewRow;

            string url = "";
            string filePath = _ToolReports[row.DataItemIndex].FileName;
            string sHost = Request.Url.Host;
            string sApplicationPath = Request.ApplicationPath;
            string picturePath = "http://" + sHost + sApplicationPath + "/ToolReport";
            url = filePath.Replace(_SaveReportData[0].Remark01, picturePath).Replace(@"\", "/");

            //打開文件
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                Session.SessionID, "window.open('" + url + "','_blank','resizable,scrollbars=no,menubar=no,toolbar=no,location=no,status=no',false);", true);
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

        /// <summary>
        /// 離開此畫面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenDialog", "javascript:window.parent.closeMasterWindow();", true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 刪除檢驗資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvToolReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = gvToolReport.Rows[e.RowIndex].DataItemIndex;

                var removeData = _ToolReports[index];

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    removeData.DeleteFromDB();

                    LogCenter.LogToDB(removeData, LogCenter.LogIndicator.Create(ActionType.Remove, txnStamp.UserID, txnStamp.RecordTime));

                    cts.Complete();
                }

                //重新取得資料
                GetToolReport(_CurrentToolName);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}