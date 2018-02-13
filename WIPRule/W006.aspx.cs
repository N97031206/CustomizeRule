/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：

說明：列印系統之標準流程卡。
      提供使用者選取生產批之後, 依據該生產批所設定之流程, 列印流程卡.
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
using CustomizeRule.RuleUtility;

namespace CustomizeRule.WIPRule
{
    public partial class W006 : CimesRuleBasePage
    {
        // 宣告一 ReportDocument 物件,用來接Crystal Report產生的rpd檔
        ReportDocument rptdoc = new ReportDocument();

        private LotInfoEx _LotData
        {
            get
            {
                return (LotInfoEx)this["_LotData"];
            }
            set
            {
                this["_LotData"] = value;
            }
        }


        // 套用Report Template的路徑
        //private string mReportTemplateName = "";

        // 輸出Report 的路徑及檔案名稱
        //string mExportFileName = "";

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
                // 匯出檔名的設定
                //mExportFileName = Request.PhysicalApplicationPath + @"\\PDF\\" + ProgramInformationBlock1.ProgramRight + Session.SessionID + ".pdf";

                //mReportTemplateName = Request.PhysicalApplicationPath + @"\\CustomizeRule\\WIPRule\\RPT\\W006_rpt.rpt";

                // 第一次進入網頁
                if (!IsPostBack)
                {
                    // 顯示畫面各欄位的Default Value
                    LoadControlDefault();
                    AjaxFocus(ttbWOLot);
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

            _LotData = null;
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
            if(cbxWO.Checked)
            {
                _LotData = LotInfoEx.GetLotByWorkOrderLot(LotID);
            }

            if (cbxLot.Checked)
            {
                _LotData = LotInfoEx.GetLotByLot(LotID);
            }

            if (cbxSN.Checked)
            {
                var lot = CustomizeFunction.ConvertDMCCode(LotID);

                var compInfo = ComponentInfoEx.GetComponentByComponentID(lot);
                if(compInfo == null)
                {
                    var compList = ComponentInfoEx.GetComponentByDMCCode(lot);
                    if (compList.Count != 0)
                        compInfo = compList[0];
                }

                if (compInfo != null)
                {
                    _LotData = LotInfo.GetLotByLot(compInfo.CurrentLot).ChangeTo<LotInfoEx>();
                }
            }

            // 若該批號無資料可顯示,離開程式並顯示訊息
            if (_LotData == null)
            {
                btnPrint.Enabled = false;
                AjaxFocus(ttbWOLot);
                throw new Exception(TextMessage.Error.T00060(LotID));
            }

            btnPrint.Enabled = true;
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
                if (_LotData == null)
                {
                    throw new Exception(TextMessage.Error.T00060(""));
                }
                _dsReport.Tables.Clear();

                // 取得Report資料
                List<LotInfo> lotDataList = new List<LotInfo>();
                lotDataList.Add(_LotData);
                DataView dvRepot = GetRunCardDataSource(lotDataList);
                _dsReport.Tables.Add(dvRepot.Table.Copy());

                _dsReport.Tables.Add(GetLotTitle());
                _dsReport.AcceptChanges();

                if (_dsReport.Tables.Count > 0)
                {
                    string sPrintProgram = "/CustomizeRule/WIPRule/W006View.aspx";
                    string sHost = Request.Url.Host;
                    string sApplicationPath = Request.ApplicationPath;
                    string ReportPath = "http://" + sHost + sApplicationPath + sPrintProgram;
                    Session["W006View"] = _dsReport;
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

                //一定要選擇一種查詢模式
                if(!cbxLot.Checked && !cbxSN.Checked && !cbxWO.Checked)
                {
                    AjaxFocus(ttbWOLot);
                    throw new CimesException(RuleMessage.Error.C00047());
                }
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
        private DataView GetRunCardDataSource(List<LotInfo> LotDataList)
        {
            string sql = "";

            DataTable dtReportData = new DataTable();
            dtReportData.Columns.Add("OPERSEQ");
            dtReportData.Columns.Add("OPERNO");
            dtReportData.Columns.Add("Operation");
            dtReportData.Columns.Add("OperationDescr");
            dtReportData.Columns.Add("RECIPEID");
            dtReportData.Columns.Add("EDC");
            dtReportData.Columns.Add("ERP_Route_LTEXT");
            dtReportData.Columns.Add("ERP_Route_LTXA1");

            LotDataList.ForEach(p =>
            {
                var workOrder = WorkOrderInfo.GetWorkOrderByWorkOrder(p.WorkOrder);
                var lstCstRouteInfo = CSTWPCWorkOrderRouteInfo.GetDataByWorkOrder(workOrder.WorkOrder);

                #region RouteOperationInfo
                sql = @" SELECT O.OPERATIONNO, O.DESCR, ROUTEOPER.* 
                       FROM MES_PRC_ROUTE_OPER ROUTEOPER ,MES_PRC_ROUTE_VER ROUTEVER ,MES_PRC_ROUTE ROUTE ,MES_PRC_OPER O
                      WHERE ROUTE.PRC_ROUTE_SID = ROUTEVER.PRC_ROUTE_SID 
                        AND ROUTEVER.PRC_ROUTE_VER_SID = ROUTEOPER.PRC_ROUTE_VER_SID
                        AND ROUTEOPER.OPERNAME = O.OPERATION
                        AND ROUTEVER.ROUTE = #[STRING]
                        AND ROUTEVER.VERSION = #[DECIMAL]
                      ORDER BY OPERSEQ";

                List<RouteOperationInfo> routeOpers = InfoCenter.GetList<RouteOperationInfo>(sql, p.RouteName, p.RouteVersion);
                #endregion

                #region 工作站
                routeOpers.ForEach(oper =>
                {
                    DataRow dr = dtReportData.NewRow();
                    dr["OPERSEQ"] = oper.OperationSequence;
                    dr["Operation"] = oper.OperationName;
                    dr["OPERNO"] = oper["OPERATIONNO"].ToString();
                    dr["OperationDescr"] = oper["DESCR"].ToString();

                    var cstOperDataInfo = CSTPRCOperationMappingDataInfo.GetOperationMappingDataByMESOperation(oper.OperationName);
                    if (cstOperDataInfo != null)
                    {
                        var cstOperInfo = InfoCenter.GetBySID<CSTPRCOperationMappingInfo>(cstOperDataInfo.PRCOperationMappingSID);
                        if (cstOperInfo != null)
                        {
                            var cstRouteInfo = lstCstRouteInfo.Find(route => route.ARBPL == cstOperInfo.ERPOperation);
                            if (cstRouteInfo != null)
                            {
                                dr["ERP_Route_LTEXT"] = cstRouteInfo["LTEXT"].ToString();
                                dr["ERP_Route_LTXA1"] = cstRouteInfo["LTXA1"].ToString();
                            }
                        }
                    }

                    dtReportData.Rows.Add(dr);
                });
                #endregion
            });

            dtReportData.AcceptChanges();
            DataView dvReportData = new DataView(dtReportData);

            dvReportData.Sort = "OPERSEQ";
            dvReportData.Table.TableName = "MES_OPER_RECIPE";

            return dvReportData;
        }

        private DataTable GetLotTitle()
        {
            DataTable dt = _LotData.CopyDataToTable("MES_WIP_LOT");
            dt.Columns.Add("DeviceCode");//料號的2,3碼
            dt.Columns.Add("LotTypeDescr");
            dt.Columns.Add("WOQuantity");
            dt.Columns.Add("DeviceDescr");
            dt.Columns.Add("DeviceBPNo");
            dt.Columns.Add("DeviceBPRev");
            dt.Columns.Add("ProductionDate");//預計生產日期
            dt.Columns.Add("ScheduleDate");//預計完成日期
            dt.Columns.Add("BOM_MATNR");//投入料號
            dt.Columns.Add("Device_PLMNO");//PLM 編號
            dt.Columns.Add("Device_PLMVR");//PLM 版本

            if(cbxWO.Checked)
            {
                var woLot = CSTWorkOrderLotInfo.GetWorkOrderLotDataByWorkOrderLot(ttbWOLot.Text.Trim());

                dt.Rows[0]["WOLOT"] = woLot.WOLOT;
                dt.Rows[0]["INVLOT"] = woLot.INVLOT;
                dt.Rows[0]["MATERIALLOT"] = woLot.MATERIALLOT;
                dt.Rows[0]["Quantity"] = woLot.Quantity;
                dt.Rows[0]["LOT"] = woLot.WOLOT;

                var cstWOBOMInfo = CSTWPCWorkOrderBOMInfo.GetDataByWorkOrder(woLot.WorkOrder).Find(p => p["SORTF"].ToString() == "1");
                if (cstWOBOMInfo != null)
                {
                    dt.Rows[0]["BOM_MATNR"] = cstWOBOMInfo["MATNR"].ToString();
                }

                dt.Rows[0]["DeviceCode"] = _LotData.DeviceName.Substring(1, 2);

                var lsLotType = WpcExClassItemInfo.GetExClassItemInfo("LotType", _LotData.LotType);
                dt.Rows[0]["LotTypeDescr"] = lsLotType.Count > 0 ? lsLotType[0].Remark02 : "";

                var WOData = WorkOrderInfo.GetWorkOrderByWorkOrder(woLot.WorkOrder);
                dt.Rows[0]["WOQuantity"] = WOData.Quantity;
                dt.Rows[0]["ProductionDate"] = WOData["ProductionDate"].ToCimesString();
                dt.Rows[0]["ScheduleDate"] = WOData.ScheduleDate;

                var DeviceData = DeviceVersionInfoEx.GetActiveDeviceVersion(_LotData.DeviceName).ChangeTo<DeviceVersionInfoEx>();
                dt.Rows[0]["DeviceDescr"] = DeviceData.Description;
                dt.Rows[0]["DeviceBPNo"] = DeviceData["BPNO"].ToString();
                dt.Rows[0]["DeviceBPRev"] = DeviceData["BPREV"].ToString();
                dt.Rows[0]["Device_PLMVR"] = DeviceData["PLMVR"].ToString();
                dt.Rows[0]["Device_PLMNO"] = DeviceData["PLMNO"].ToString();
            }
            if(cbxSN.Checked || cbxLot.Checked)
            {
                dt.Rows[0]["WOLOT"] = _LotData.WorkOrderLot;
                dt.Rows[0]["INVLOT"] = _LotData.InventoryLot;
                dt.Rows[0]["MATERIALLOT"] = _LotData.MaterialLot;
                dt.Rows[0]["Quantity"] = _LotData.Quantity;
                dt.Rows[0]["LOT"] = _LotData.Lot;

                dt.Rows[0]["DeviceCode"] = _LotData.DeviceName.Substring(1, 2);
                var lsLotType = WpcExClassItemInfo.GetExClassItemInfo("LotType", _LotData.LotType);
                dt.Rows[0]["LotTypeDescr"] = lsLotType.Count > 0 ? lsLotType[0].Remark02 : "";

                var WOData = WorkOrderInfo.GetWorkOrderByWorkOrder(_LotData.WorkOrder);
                dt.Rows[0]["WOQuantity"] = WOData.Quantity;
                dt.Rows[0]["ProductionDate"] = WOData["ProductionDate"].ToCimesString();
                dt.Rows[0]["ScheduleDate"] = WOData.ScheduleDate;

                var DeviceData = DeviceVersionInfoEx.GetActiveDeviceVersion(_LotData.DeviceName).ChangeTo<DeviceVersionInfoEx>();
                dt.Rows[0]["DeviceDescr"] = DeviceData.Description;
                dt.Rows[0]["DeviceBPNo"] = DeviceData["BPNO"].ToString();
                dt.Rows[0]["DeviceBPRev"] = DeviceData["BPREV"].ToString();
                dt.Rows[0]["Device_PLMVR"] = DeviceData["PLMVR"].ToString();
                dt.Rows[0]["Device_PLMNO"] = DeviceData["PLMNO"].ToString();

                var cstWOBOMInfo = CSTWPCWorkOrderBOMInfo.GetDataByWorkOrder(WOData.WorkOrder).Find(p => p["SORTF"].ToString() == "1");
                if (cstWOBOMInfo != null)
                {
                    dt.Rows[0]["BOM_MATNR"] = cstWOBOMInfo["MATNR"].ToString();
                }
            }

            return dt;
        }
    }
}
