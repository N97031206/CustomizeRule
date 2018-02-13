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
using ciMesRule.UserControl;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService;

namespace CustomizeRule.WIPRule
{
    public partial class W036 : CimesRuleBasePage
    {
        // 宣告一 ReportDocument 物件,用來接Crystal Report產生的rpd檔
        ReportDocument rptdoc = new ReportDocument();

        private LotInfoEx lotData
        {
            get
            {
                return (LotInfoEx)this["lotData"];
            }
            set
            {
                this["lotData"] = value;
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

                    string lotID = GetLotCookie();
                    if (!lotID.IsNullOrTrimEmpty())
                    {
                        LoadControlByLot(lotID);
                    }
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
        private void LoadControlByLot(string LotID)
        {
            // 清除欄位資訊
            ClearField();

            // 一定在MES_WIP_LOT_NONACTIVE因為已經入庫
            var lsLotData = InfoCenter.GetBySQL<LotInfoEx>("SELECT * FROM MES_WIP_LOT_NONACTIVE WHERE LOT = #[STRING]", LotID);

            if (lsLotData == null)
            {               
                lsLotData = InfoCenter.GetBySQL<LotInfoEx>("SELECT * FROM MES_WIP_LOT_NONACTIVE WHERE INVNO = #[STRING]", LotID);
            }

            // 若該批號無資料可顯示,離開程式並顯示訊息
            if (lsLotData == null)
            {
                btnPrint.Enabled = false;
                throw new Exception(TextMessage.Error.T00060(LotID));
            }

            btnPrint.Enabled = true;

            lotData = lsLotData;
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
                if (lotData == null)
                {
                    throw new Exception(TextMessage.Error.T00060(""));
                }
                _dsReport.Tables.Clear();

                // 取得Report資料
                _dsReport = GetRunCardDataSource(lotData);
                _dsReport.AcceptChanges();

                if (_dsReport.Tables.Count > 0)
                {
                    string sPrintProgram = "/CustomizeRule/WIPRule/W036View.aspx";
                    string sHost = Request.Url.Host;
                    string sApplicationPath = Request.ApplicationPath;
                    string ReportPath = "http://" + sHost + sApplicationPath + sPrintProgram;
                    Session["W036View"] = _dsReport;
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

        protected void ttbWOLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ttbWOLot.Must(lblLot);
                ttbWOLot.Text = ttbWOLot.Text.Trim();
                LoadControlByLot(ttbWOLot.Text);
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
        private DataSet GetRunCardDataSource(LotInfoEx LotData)
        {
            string sql = @"SELECT * FROM MES_WIP_LOT_NONACTIVE WHERE INVNO = #[STRING]";
            sql = string.Format(sql);
            SqlAgent sa = SQLCenter.Parse(sql, LotData.InventoryNo);

            var lsInvLot = InfoCenter.GetList<LotInfoEx>(sa);
            if (lsInvLot.Count == 0 )
            {
                throw new Exception(TextMessage.Error.T00060("InventoryNO:" + LotData.InventoryNo));
            }

            string sBoxInString = "";
            lsInvLot.ForEach(p => {
                sBoxInString += "'" + p.Lot + "',";
            });
            sBoxInString = sBoxInString.EndsWith(",") ? sBoxInString.Substring(0, sBoxInString.Length - 1) : sBoxInString;

            var dtInvData = DBCenter.GetDataTable(@"
                SELECT WO,INVLOT,SUM(QUANTITY) QUANTITY,DEVICE FROM MES_WIP_LOT_NONACTIVE WHERE LOT IN (
                SELECT CURRENTLOT FROM MES_WIP_COMP_NONACTIVE  WHERE COMPONENTID IN (
                SELECT COMPONENTID FROM CST_WIP_PACK 
                INNER JOIN CST_WIP_PACK_DATA ON (CST_WIP_PACK.WIP_PACK_SID = CST_WIP_PACK_DATA.WIP_PACK_SID) WHERE 
                BOXNO IN (" + sBoxInString + "))) GROUP BY WO,INVLOT,DEVICE");
            dtInvData.TableName = "INVData";

            #region 定義 LOTDATA 資料表
            //DataTable dtInvData = lsInvLot.CopyDataToTable("INVData");
            dtInvData.Columns.Add("INVDate");
            dtInvData.Columns.Add("ITEM");            
            dtInvData.Columns.Add("DeviceDescr");            
            dtInvData.Columns.Add("Remark");
            dtInvData.Columns.Add("Loaction");
            dtInvData.Columns.Add("Factory");
            #endregion

            for (int i = 0; i < dtInvData.Rows.Count; i++)
            {                
                dtInvData.Rows[i]["INVDate"] = lsInvLot[0]["USERDEFINECOL17"].ToCimesString();
                dtInvData.Rows[i]["ITEM"] = (i + 1).ToCimesString();
                dtInvData.Rows[i]["Remark"] = lsInvLot[0]["USERDEFINECOL18"].ToCimesString();
                dtInvData.Rows[i]["Loaction"] = lsInvLot[0]["LOCATION"].ToCimesString();
                dtInvData.Rows[i]["Factory"] = lsInvLot[0]["FACTORY"].ToCimesString();

                var DeviceData = DeviceVersionInfoEx.GetActiveDeviceVersion(dtInvData.Rows[i]["DEVICE"].ToString()).ChangeTo<DeviceVersionInfoEx>();
                if (DeviceData != null)
                {
                    dtInvData.Rows[i]["DeviceDescr"] = DeviceData.Description;
                }
            }

            DataSet dsReportData = new DataSet();
            dsReportData.Tables.Add(dtInvData);

            return dsReportData;
        }
    }
}
