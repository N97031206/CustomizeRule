/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：

說明：列印包裝清單。
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
    public partial class W035 : CimesRuleBasePage
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
                AjaxFocus(ttbWOLot);
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

            var lsLotData = LotInfoEx.GetLotByLot(LotID).ChangeTo<LotInfoEx>();

            //若在製找不到，換到MES_WIP_LOT_NONACTIVE找
            if (lsLotData == null)
            {
                string sql = @"SELECT * FROM MES_WIP_LOT_NONACTIVE WHERE LOT = #[STRING]";
                SqlAgent sa = SQLCenter.Parse(sql, LotID);
                lsLotData = InfoCenter.GetBySQL<LotInfoEx>(sa);
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
                    string sPrintProgram = "/CustomizeRule/WIPRule/W035View.aspx";
                    string sHost = Request.Url.Host;
                    string sApplicationPath = Request.ApplicationPath;
                    string ReportPath = "http://" + sHost + sApplicationPath + sPrintProgram;
                    Session["W035View"] = _dsReport;
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
            string sql = "";
            string sTableName = "MES_WIP_COMP";

            if (LotData.Status == "Finished")
            {
                sTableName = "MES_WIP_COMP_NONACTIVE";
            }

            #region 定義 LOTDATA 資料表
            DataTable dtLotData = lotData.CopyDataToTable("LOTDATA");
            dtLotData.Columns.Add("CartonNo");
            dtLotData.Columns.Add("DeviceDescr1");
            dtLotData.Columns.Add("DeviceDescr2");
            dtLotData.Columns.Add("CustomerNo1");
            dtLotData.Columns.Add("CustomerNo2");
            dtLotData.Columns.Add("Device1");
            dtLotData.Columns.Add("Device2");
            dtLotData.Columns.Add("Quantity1");
            dtLotData.Columns.Add("Quantity2");
            dtLotData.Columns.Add("Remark");
            dtLotData.Columns.Add("Inspectors");
            dtLotData.Columns.Add("Packers");
            dtLotData.Columns.Add("InspectionDate");
            #endregion

            #region 定義 COMPDATA 資料表
            DataTable dtCompData = new DataTable("COMPDATA");
            dtCompData.Columns.Add("ComponentID1");
            dtCompData.Columns.Add("ComponentID2");
            dtCompData.Columns.Add("Quantity1");
            dtCompData.Columns.Add("Quantity2");
            #endregion

            dtLotData.Rows[0]["CartonNo"] = LotData.Lot;
            dtLotData.Rows[0]["Remark"] = "";

            var packInfo = CSTWIPPackInfo.GetPackInfoByBoxNo(LotData.Lot);
            dtLotData.Rows[0]["Inspectors"] = packInfo.INSPUSER;
            dtLotData.Rows[0]["Packers"] = packInfo.UserID;
            dtLotData.Rows[0]["InspectionDate"] = packInfo.UpdateTime.Substring(0,10).Replace("/", "-");

            sql = @"SELECT DEVICE,COUNT(*) QTY 
                      FROM CST_WIP_PACK 
                     INNER JOIN CST_WIP_PACK_DATA ON CST_WIP_PACK.WIP_PACK_SID = CST_WIP_PACK_DATA.WIP_PACK_SID
                     WHERE BOXNO = #[STRING]
                     GROUP BY DEVICE";

            DataTable dtData = DBCenter.GetDataTable(sql, LotData.Lot);
            int iIndex = 1;
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                if (i >= 2)
                    break;

                iIndex = i + 1;
                dtLotData.Rows[0]["Quantity" + iIndex.ToCimesString()] = dtData.Rows[i]["QTY"].ToCimesString();

                var DeviceData = DeviceVersionInfoEx.GetActiveDeviceVersion(dtData.Rows[i]["DEVICE"].ToCimesString()).ChangeTo<DeviceVersionInfoEx>();
                if (DeviceData != null)
                {
                    dtLotData.Rows[0]["DeviceDescr" + iIndex.ToCimesString()] = DeviceData.Description;
                    dtLotData.Rows[0]["CustomerNo" + iIndex.ToCimesString()] = DeviceData["CustomerNo"].ToCimesString();
                    dtLotData.Rows[0]["Device" + iIndex.ToCimesString()] = DeviceData.DeviceName;
                }

                #region 入庫批號

                sql = @"SELECT DMC,COUNT(*) COMPONENTQTY 
                          FROM CST_WIP_PACK 
                         INNER JOIN CST_WIP_PACK_DATA ON CST_WIP_PACK.WIP_PACK_SID = CST_WIP_PACK_DATA.WIP_PACK_SID
                         WHERE BOXNO = #[STRING]
                           AND DEVICE = #[STRING]
                         GROUP BY DMC 
                         ORDER BY DMC ";

                var dt = DBCenter.GetDataTable(sql, LotData.Lot, DeviceData.DeviceName);
                if (dt.Rows.Count > dtCompData.Rows.Count)
                {
                    int iRowCount = dt.Rows.Count - dtCompData.Rows.Count;
                    for (int j = 0; j < iRowCount; j++)
                    {
                        DataRow dr = dtCompData.NewRow();
                        dtCompData.Rows.Add(dr);
                    }
                }
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    dtCompData.Rows[k]["ComponentID" + iIndex.ToCimesString()] = dt.Rows[k]["DMC"].ToString();
                    dtCompData.Rows[k]["Quantity" + iIndex.ToCimesString()] = dt.Rows[k]["COMPONENTQTY"].ToString();
                }        
                #endregion
            }

            dtCompData.AcceptChanges();
            DataSet dsReportData = new DataSet();
            dsReportData.Tables.Add(dtLotData);
            dsReportData.Tables.Add(dtCompData);

            return dsReportData;
        }
    }
}
