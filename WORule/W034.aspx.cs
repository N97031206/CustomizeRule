
//  SEQ  DATE       AUTHOR       MESSAGE
//===================================================
//  001  20120326  BRYAN HUANG 修正在進行發放的時候，會出現unique constrain(WPC_WO_SID)的錯誤, 由原先的InsertToDB 修改為UpdateToDB
//  002  20120327  Ryan WU  修正儲存後gridview update切換到新增工單當頁並且將該筆新紀錄以選取方式呈現, 取消按鈕LoadData function 刪除
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using ciMes.Web.Common;
using Ares.Cimes.IntelliService.Web.UI;
using ciMes.Web.Common.UserControl;
using System.Web.UI;

namespace CustomizeRule.WORule
{
    public partial class W034 : CimesBasePage
    {
        #region Property
        /// <summary>
        /// 目前畫面上的工單info清單
        /// </summary>
        private List<WorkOrderInfo> _WODataList
        {
            get
            {
                return (List<WorkOrderInfo>)this["_WODataList"];
            }
            set { this["_WODataList"] = value; }
        }

        private List<CSTWorkOrderLotInfo> _WorkOrderLotList
        {
            get
            {
                return (List<CSTWorkOrderLotInfo>)this["_WorkOrderLotList"];
            }
            set { this["_WorkOrderLotList"] = value; }
        }

        

        /// <summary>
        /// 目前選取的工單info
        /// </summary>
        protected WorkOrderInfo _CurrentWorkOrder
        {
            get { return (WorkOrderInfo)this["_CurrentWorkOrder"]; }
            set { this["_CurrentWorkOrder"] = value; }
        }

        private SortContent _WOSort
        {
            get
            {
                if (this["_WOSort"] != null)
                    return (SortContent)this["_WOSort"];
                else
                {
                    this["_WOSort"] = new SortContent("WO", SortTypes.ASC);
                    return (SortContent)this["_WOSort"];
                }
            }
            set
            {
                this["_WOSort"] = value;
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

        private List<AttributeInstanceInfo> _LotTemplateAttrInstancies
        {
            get
            {
                return (List<AttributeInstanceInfo>)this["_LotTemplateAttrInstancies"];
            }
            set { this["_LotTemplateAttrInstancies"] = value; }
        }

        private List<AttributeAttributeInfo> _LotTemplateAttrs
        {
            get
            {
                return (List<AttributeAttributeInfo>)this["_LotTemplateAttrs"];
            }
            set { this["_LotTemplateAttrs"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }
        private string OpenPrintWindow;
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
                    ClearFiled();

                    //載入Control的預設資料
                    LoadControlDefault();
                    QueryData("", "");
                    LoadControlButton();
                }
                else
                {
                    gvQuery.DataSource = _WODataList;
                    gvLotTemplate.DataSource = _LotTemplateAttrInstancies;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void QueryData(string sWO, string sFlag)
        {
            ClearWOField();

            List<object> lstPara = new List<object>();
            string sql = " SELECT A.* FROM MES_WPC_WO A "
              + " WHERE 1 = 1 ";

            if (!sWO.IsNullOrTrimEmpty())
            {
                sql += " AND WO LIKE #[STRING]";
                lstPara.Add(sWO + "%");
            }
            if (!sFlag.IsNullOrTrimEmpty())
            {
                sql += " AND FLAG = #[STRING]";
                lstPara.Add(sFlag);
            }
            sql += " ORDER BY WO ";
            SqlAgent sa = SQLCenter.Parse(sql, lstPara.ToArray());
            _WODataList = InfoCenter.GetList<WorkOrderInfo>(sa);

            gvQuery.PageIndex = 0;
            gvQuery.SelectedIndex = -1;
            gvQuery.SetDataSource(_WODataList);
            gvQuery.DataBind();

            if (_WODataList.Count == 0)
            {
                throw new RuleCimesException(TextMessage.Error.T00060(""));
            }
        }

        protected void gvQuery_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                switch (e.Row.RowType)
                {
                    case DataControlRowType.Header:
                        break;
                    case DataControlRowType.DataRow:
                        //if (_WODataList == null || _WODataList.Count == 0) return;
                        //int dataItemIndex = e.Row.DataItemIndex;
                        //Button btnDelete = (Button)e.Row.Cells[0].Controls[0];
                        //btnDelete.Attributes["OnMouseOut"] = "bDelete=false;";
                        //btnDelete.Attributes["OnClick"] = "bDelete=true; DelKey='" + _WODataList[dataItemIndex]["WO"].ToString() + "';";

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 載入畫面預設的controls項目
        /// </summary>
        private void LoadControlDefault()
        {
            _dsReport = new DataSet();
            #region 客戶名稱
            List<CustomerInfo> customers = CustomerInfo.GetAllCustomers();
            ddlCustomer.DataSource = customers;
            ddlCustomer.DataTextField = "CUSTOMER";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, "");

            if (customers.Count == 0)
                throw new RuleCimesException(TextMessage.Warning.T00060(lblCustomer.Text));
            #endregion

            #region 產品碼
            List<ProductInfo> products = ProductInfo.GetAllProducts();
            ddlProduct.DataSource = products;
            ddlProduct.DataTextField = "ProductName";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, "");

            if (products.Count == 0)
                throw new RuleCimesException(TextMessage.Warning.T00060(lblCustomer.Text));
            #endregion

            #region 單位/第二單位
            List<WpcExClassItemInfo> units = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("Unit");
            ddlUnit.DataSource = units;
            ddlUnit.DataTextField = "Remark01";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, "");

            ddlSUnit.DataSource = units;
            ddlSUnit.DataTextField = "Remark01";
            ddlSUnit.DataBind();
            ddlSUnit.Items.Insert(0, "");

            if (units.Count == 0)
                throw new RuleCimesException(TextMessage.Error.T00555("Unit", ""));
            #endregion

            #region 批號型態
            List<WpcExClassItemInfo> LotTypes = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("LotType");
            ddlLotType.DataSource = LotTypes;
            ddlLotType.DataTextField = "Remark01";
            ddlLotType.DataBind();
            ddlLotType.Items.Insert(0, "");

            if (LotTypes.Count == 0)
                throw new RuleCimesException(TextMessage.Error.T00555("LotType", ""));
            #endregion

            #region 負責人
            List<UserProfileInfo> users = InfoCenter.GetList<UserProfileInfo>("SELECT * FROM MES_SEC_USER_PRFL ORDER BY SEC_USER_PRFL_SID");
            ddlOwner.DataSource = users;
            ddlOwner.DataTextField = "UserProfileSID";
            ddlOwner.DataBind();
            ddlOwner.Items.Insert(0, "");
            #endregion

            //以下內容沒有取得資料只顯示錯誤，但不影響往下執行
            string ex = "";

            #region 優先等級
            List<WpcExClassItemInfo> Prioritys = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("Priority");
            ddlPriority.DataSource = Prioritys;
            ddlPriority.DataTextField = "Remark01";
            ddlPriority.DataBind();

            if (Prioritys.Count == 0)
                ex = TextMessage.Error.T00555("Priority", "");
            #endregion

            #region 生產等級
            List<WpcExClassItemInfo> Criticals = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("Critical");
            ddlCritical.DataSource = Criticals;
            ddlCritical.DataTextField = "Remark01";
            ddlCritical.DataBind();
            ddlCritical.Items.Insert(0, "");

            if (Criticals.Count == 0)
                ex += TextMessage.Error.T00555("Critical", "");
            #endregion

            #region 批號範本
            string sql = "SELECT TEMPLATE,WIP_TEMP_SID FROM MES_WIP_TEMP WHERE STATUS='Enable' ORDER BY 1 ";
            ddlLotTemplate.DataSource = DBCenter.GetDataTable(sql);
            ddlLotTemplate.DataTextField = "TEMPLATE";
            ddlLotTemplate.DataValueField = "WIP_TEMP_SID";
            ddlLotTemplate.DataBind();
            ddlLotTemplate.Items.Insert(0, "");
            #endregion

            //若有錯誤訊息，只顯示於Label
            if (!ex.IsNullOrTrimEmpty())
                _ProgramInformationBlock.ShowMessage(ex, MessageShowOptions.OnLabel);
        }

        private void LoadControlButton()
        {
            //btnAdd.Visible = true;

            if (_CurrentWorkOrder == null)
            {
                btnRelease.Visible = false;
                btnUnRelease.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
            else
            {
                if (_CurrentWorkOrder.InfoState == InfoState.NewCreate)
                {
                    #region btnAdd: btnReset, btnAdd, btnRelease, btnUnRelease disable. btnSave enable
                    btnRelease.Visible = false;
                    btnUnRelease.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    #endregion
                }
                else
                {
                    btnAdd.Enabled = true;
                    switch (_CurrentWorkOrder.Flag)
                    {
                        case "":
                            btnRelease.Visible = false;
                            btnUnRelease.Visible = false;
                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            break;
                        case "Created":
                            #region 狀態是Created
                            btnRelease.Visible = false;
                            btnUnRelease.Visible = false;
                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            #endregion
                            break;
                        case "Release":
                            #region 狀態是Release
                            btnUnRelease.Visible = true;
                            btnRelease.Visible = false;
                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            #endregion
                            break;
                        case "UnRelease":
                            #region 狀態是UnRelease
                            btnSave.Visible = true;
                            btnRelease.Visible = true;
                            btnUnRelease.Visible = false;
                            btnCancel.Visible = true;

                            //002 判斷是否分過批
                            if (DBCenter.GetDataTable("SELECT * FROM MES_WPC_WO_PART WHERE WPC_WO_SID = #[STRING]", ttbWO.Text.Trim()).Rows.Count > 0)
                            {
                                btnSave.Visible = false;
                            }
                            #endregion
                            break;
                    }
                }
            }
        }

        private void BindGvData()
        {
            gvQuery.DataSource = _WODataList;
            gvQuery.DataBind();
        }

        private void BindGvData(int pageIndex, int rowIndex)
        {
            //換頁要放在bind資料前
            //指定選擇行放在bind資料後
            gvQuery.PageIndex = pageIndex;
            gvQuery.DataSource = _WODataList;
            gvQuery.DataBind();
            gvQuery.SelectedIndex = rowIndex;
        }

        protected void gvQuery_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvQuery.SelectedIndex = -1;
                gvQuery.PageIndex = e.NewPageIndex;
                gvQuery.DataBind();

                btnCancel_Click(btnCancel, null);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvQuery_RowClicked(object sender, Ares.Cimes.IntelliService.Web.UI.CimesGridViewRowClickedEventArgs e)
        {
            try
            {
                gvQuery.SelectedIndex = e.SelectIndex;
                gvQuery.DataBind();

                GridViewSelectEventArgs evn = new GridViewSelectEventArgs(e.SelectIndex);
                gvQuery_SelectedIndexChanging(gvQuery, evn);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvQuery_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                switch (e.Row.RowType)
                {
                    case DataControlRowType.DataRow:
                        switch (e.Row.RowState)
                        {
                            case DataControlRowState.Normal:
                            case DataControlRowState.Alternate:
                                //string strType = e.Row.Cells[3].Text;
                                //if (strType == "Enable")
                                //    e.Row.Cells[3].Text = GetUIResource("Enable");

                                //if (strType == "Disable")
                                //    e.Row.Cells[3].Text = GetUIResource("Disable");

                                break;
                            case DataControlRowState.Edit:
                            case DataControlRowState.Alternate | DataControlRowState.Edit:
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvQuery_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int dataItemIndex = gvQuery.Rows[e.RowIndex].DataItemIndex;

                if (_WODataList[dataItemIndex].Flag != "UnRelease")
                    throw new RuleCimesException(TextMessage.Error.T00190());

                TransactionStamp txnStamp = new TransactionStamp(_ProgramInformationBlock.UserID, _ProgramInformationBlock.ProgramRight, _ProgramInformationBlock.ProgramRight, ApplicationName);
                using (CimesTransactionScope cts = CimesTransactionScope.Create())
                {
                    //寫入Hist
                    WorkOrderHistoryInfo woHistData = InfoCenter.Create<WorkOrderHistoryInfo>();
                    _WODataList[dataItemIndex].Fill<WorkOrderHistoryInfo>(woHistData);
                    woHistData.InsertImmediately(txnStamp.UserID, txnStamp.RecordTime);

                    _WODataList[dataItemIndex].DeleteFromDB();

                    _WODataList.RemoveAt(dataItemIndex);

                    cts.Complete();
                }

                gvQuery.SetDataSource(_WODataList);
                gvQuery.DataBind();

                ClearWOField();

                LoadControlButton();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvQuery_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                int dataItemIndex = gvQuery.Rows[e.NewSelectedIndex].DataItemIndex;
                _CurrentWorkOrder = _WODataList[dataItemIndex];
                LoadControlByWO(_CurrentWorkOrder);
                //取得小工單資料清單
                _WorkOrderLotList = CSTWorkOrderLotInfo.GetWorkOrderLotDataByWorkOrder(_CurrentWorkOrder.WorkOrder);
                _WorkOrderLotList.ForEach(p =>
                {
                    if(p.CreateFlag == "Y")
                    {
                        p.CreateFlag = "已領料";
                    }
                    else
                    {
                        p.CreateFlag = "未領料";
                    }
                });
                gvWOLotTemplate.DataSource = _WorkOrderLotList;
                gvWOLotTemplate.DataBind();
                EnableTab();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
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
                Button btnPrint = (Button)sender;
                TableCell tc = (TableCell)btnPrint.Parent;
                GridViewRow gvRow = (GridViewRow)tc.Parent;
                int dataIndex = gvRow.DataItemIndex;
                if (_WorkOrderLotList.Count == 0)
                {
                    throw new Exception(TextMessage.Error.T00060(""));
                }
                _dsReport.Tables.Clear();
                var woLot = _WorkOrderLotList[dataIndex];
                var wo = WorkOrderInfo.GetWorkOrderByWorkOrder(woLot.WorkOrder);
                List<CSTWorkOrderLotInfo> woLotList = new List<CSTWorkOrderLotInfo>();
                woLotList.Add(woLot);
                //列印報表
                PrintReport(woLotList, wo);
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        private void PrintReport(List<CSTWorkOrderLotInfo> woLotList, WorkOrderInfo wo)
        {
            
            woLotList.ForEach(woLot =>
            {
                var dsReport = new DataSet();
                dsReport.Tables.Clear();
                // 取得Report資料
                DataView dvRepot = GetRunCardDataSource(woLot, wo);
                dsReport.Tables.Add(dvRepot.Table.Copy());

                dsReport.Tables.Add(GetLotTitle(wo, woLot));
                dsReport.AcceptChanges();

                if (dsReport.Tables.Count > 0)
                {
                    var sid = DBCenter.GetSystemID();
                    string sPrintProgram = "/CustomizeRule/WIPRule/W034View.aspx";
                    string sHost = Request.Url.Host;
                    string sApplicationPath = Request.ApplicationPath;
                    string ReportPath = "http://" + sHost + sApplicationPath + sPrintProgram + "?sid=" + sid;
                    Session[sid] = dsReport;
                    //開啟查詢工單頁面
                    //string openPrintWindow = "window.open('" + ReportPath + "','_blank ','resizable: yes; status: no; scrollbars:no; menubar:no;toolbar:no;location:no;dialogLeft:10px;dialogTop:10px;dialogHeight:10px;dialogWidth:10px',false);";
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), openPrintWindow, true);
                    //匯出檔案
                    OpenPrintWindow += @"window.open('{0}','_blank','resizable: yes; status: no; scrollbars:no; menubar:no;toolbar:no;location:no;dialogLeft:10px;dialogTop:10px;dialogHeight:10px;dialogWidth:10px',false);";
                    OpenPrintWindow = string.Format(OpenPrintWindow, ReportPath);
                }
            });

            //打開文件-->可以應付兩種PDF
            if (!OpenPrintWindow.IsNullOrTrimEmpty())
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), OpenPrintWindow, true);
                // {0}成功！
                //_ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00057(GetUIResource("Print")));
            }
        }

        /// <summary>
        /// 取的runcard的資料來源
        /// </summary>
        /// <param name="LotDataList"></param>
        /// <returns></returns>
        private DataView GetRunCardDataSource(CSTWorkOrderLotInfo woLot, WorkOrderInfo workOrder)
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


            #region RouteOperationInfo
            sql = @" SELECT O.OPERATIONNO, O.DESCR, ROUTEOPER.* 
                       FROM MES_PRC_ROUTE_OPER ROUTEOPER ,MES_PRC_ROUTE_VER ROUTEVER ,MES_PRC_ROUTE ROUTE ,MES_PRC_OPER O
                      WHERE ROUTE.PRC_ROUTE_SID = ROUTEVER.PRC_ROUTE_SID 
                        AND ROUTEVER.PRC_ROUTE_VER_SID = ROUTEOPER.PRC_ROUTE_VER_SID
                        AND ROUTEOPER.OPERNAME = O.OPERATION
                        AND ROUTEVER.ROUTE = #[STRING]
                        AND ROUTEVER.VERSION = #[DECIMAL]
                      ORDER BY OPERSEQ";

            List<RouteOperationInfo> routeOpers = InfoCenter.GetList<RouteOperationInfo>(sql, workOrder.RouteName, workOrder.RouteVersion);
            #endregion

            var lstCstRouteInfo = CSTWPCWorkOrderRouteInfo.GetDataByWorkOrder(workOrder.WorkOrder);
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
                        var cstRouteInfo = lstCstRouteInfo.Find(p => p.ARBPL == cstOperInfo.ERPOperation);
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

            dtReportData.AcceptChanges();
            DataView dvReportData = new DataView(dtReportData);

            dvReportData.Sort = "OPERSEQ";
            dvReportData.Table.TableName = "MES_OPER_RECIPE";

            return dvReportData;
        }

        private DataTable GetLotTitle(WorkOrderInfo workOrder, CSTWorkOrderLotInfo woLot)
        {
            var lotData = InfoCenter.Create<LotInfo>();
            lotData = workOrder.Fill<LotInfo>();
            DataTable dt = lotData.CopyDataToTable("MES_WIP_LOT");
            dt.Columns.Add("DeviceCode");//料號的2,3碼
            dt.Columns.Add("LotTypeDescr");
            dt.Columns.Add("WOQuantity");
            dt.Columns.Add("DeviceDescr");
            dt.Columns.Add("DeviceBPNo");
            dt.Columns.Add("DeviceBPRev");
            dt.Columns.Add("ProductionDate");//預計生產日期
            dt.Columns.Add("ScheduleDate");//預計完成日期
            dt.Columns.Add("BOM_MATNR");
            dt.Columns.Add("Device_PLMVR");
            dt.Columns.Add("Device_PLMNO");
            //dt.Columns.Add("LotQuantity");

            dt.Rows[0]["WOLOT"] = woLot.WOLOT;
            dt.Rows[0]["INVLOT"] = woLot.INVLOT;
            dt.Rows[0]["MATERIALLOT"] = woLot.MATERIALLOT;
            dt.Rows[0]["Quantity"] = woLot.Quantity;
            dt.Rows[0]["LOT"] = woLot.WOLOT;

            var cstWOBOMInfo = CSTWPCWorkOrderBOMInfo.GetDataByWorkOrder(workOrder.WorkOrder).Find(p => p["SORTF"].ToString() == "1");
            if (cstWOBOMInfo != null)
            {
                dt.Rows[0]["BOM_MATNR"] = cstWOBOMInfo["MATNR"].ToString();
            }

            dt.Rows[0]["DeviceCode"] = workOrder.DeviceName.Substring(1, 2);

            var lsLotType = WpcExClassItemInfo.GetExClassItemInfo("LotType", workOrder.LotType);
            dt.Rows[0]["LotTypeDescr"] = lsLotType.Count > 0 ? lsLotType[0].Remark02 : "";

            var WOData = WorkOrderInfo.GetWorkOrderByWorkOrder(workOrder.WorkOrder);
            dt.Rows[0]["WOQuantity"] = WOData.Quantity;
            dt.Rows[0]["ProductionDate"] = WOData["ProductionDate"].ToCimesString();
            dt.Rows[0]["ScheduleDate"] = WOData.ScheduleDate;

            var DeviceData = DeviceVersionInfoEx.GetActiveDeviceVersion(workOrder.DeviceName).ChangeTo<DeviceVersionInfoEx>();
            dt.Rows[0]["DeviceDescr"] = DeviceData.Description;
            dt.Rows[0]["DeviceBPNo"] = DeviceData["BPNO"].ToString();
            dt.Rows[0]["DeviceBPRev"] = DeviceData["BPREV"].ToString();
            dt.Rows[0]["Device_PLMVR"] = DeviceData["PLMVR"].ToString();
            dt.Rows[0]["Device_PLMNO"] = DeviceData["PLMNO"].ToString();
            
            return dt;
        }

        /// <summary>
        /// 系統事件 : btnAllPrint Click時觸發
        /// 將產生的ReportDocument 匯出成為PDF格式的檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAllPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (_WorkOrderLotList.Count == 0)
                {
                    throw new Exception(TextMessage.Error.T00060(""));
                }
                
                var wo = WorkOrderInfo.GetWorkOrderByWorkOrder(_WorkOrderLotList[0].WorkOrder);
                
                //列印報表
                PrintReport(_WorkOrderLotList, wo);
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        

        private void EnableTab()
        {
            foreach (CimesTabItem tab in rtsDetail.Tabs)
            {
                tab.Enabled = true;
            }
            foreach (CimesPageView view in rmpDetail.PageViews)
            {
                view.Enabled = true;
            }
        }

        private void LoadControlByWO(WorkOrderInfo woInfo)
        {
            if (woInfo == null)
                return;

            ListItem itemp;

            //工單編號
            ttbWO.Text = woInfo.WorkOrder;
            //訂單編號
            ttbSO.Text = woInfo.SO;

            if (woInfo.InfoState != InfoState.NewCreate)
            {
                //數量
                ttbQuantity.Text = woInfo.Quantity.ToCimesString();
                //第二數量
                ttbSQuantity.Text = woInfo.SecondQuantity.ToCimesString();
            }
            //工廠完工日
            ttbCommitDate.Text = woInfo.CommitDate;
            //客戶到期日
            ttbDueDate.Text = woInfo.DueDate;
            //預定交貨日
            ttbScheduleDate.Text = woInfo.ScheduleDate;
            //狀態
            lblShowFlag.Text = woInfo.Flag;
            //備註
            ttbRemark.Text = woInfo.Description;

            #region 批號型態
            itemp = ddlLotType.Items.FindByText(woInfo.LotType);
            if (itemp != null)
            {
                ddlLotType.SelectedIndex = ddlLotType.Items.IndexOf(itemp);
            }
            else
            {
                itemp = new ListItem(woInfo.LotType);
                ddlLotType.Items.Insert(0, itemp);
            }
            #endregion

            #region 批號範本
            itemp = ddlLotTemplate.Items.FindByText(woInfo.Template);
            if (itemp != null)
            {
                ddlLotTemplate.SelectedIndex = ddlLotTemplate.Items.IndexOf(itemp);

                LoadLotTemplate(woInfo.Template);
            }
            else
            {
                itemp = new ListItem(woInfo.Template);
                ddlLotTemplate.Items.Insert(0, itemp);
            }
            #endregion

            #region 第一單位
            itemp = ddlUnit.Items.FindByText(woInfo.Unit);
            if (itemp != null)
            {
                ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(itemp);
            }
            else
            {
                itemp = new ListItem(woInfo.Unit);
                ddlUnit.Items.Insert(0, itemp);
            }
            #endregion

            #region 第二單位
            itemp = ddlSUnit.Items.FindByText(woInfo.SecondUnit);
            if (itemp != null)
            {
                ddlSUnit.SelectedIndex = ddlSUnit.Items.IndexOf(itemp);
            }
            else
            {
                itemp = new ListItem(woInfo.SecondUnit);
                ddlSUnit.Items.Insert(0, itemp);
            }
            #endregion

            #region 客戶名稱
            itemp = ddlCustomer.Items.FindByText(woInfo.Customer);
            if (itemp != null)
            {
                ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(itemp);
            }
            else
            {
                itemp = new ListItem(woInfo.Customer);
                ddlCustomer.Items.Insert(0, itemp);
            }
            #endregion

            #region 產品碼
            itemp = ddlProduct.Items.FindByText(woInfo.ProductName);
            if (itemp != null)
            {
                ddlProduct.SelectedIndex = ddlProduct.Items.IndexOf(itemp);
            }
            else
            {
                itemp = new ListItem(woInfo.ProductName);
                ddlProduct.Items.Insert(0, itemp);
            }
            #endregion

            #region 型號
            if (woInfo.ProductName.IsNullOrTrimEmpty() == false)
            {
                ProductInfo prodData = ProductInfo.GetProductByName(woInfo.ProductName);
                if (prodData == null)
                    throw new RuleCimesException(TextMessage.Error.T00030(GetUIResource("Product"), woInfo.ProductName));

                List<DeviceVersionInfo> deviceVerDataSet = DeviceVersionInfo.GetActiveDeviceVersionsByProduct(prodData);
                if (deviceVerDataSet.Count == 0)
                    throw new RuleCimesException(TextMessage.Error.T00026(GetUIResource("Product"), GetUIResource("Device"), GetUIResource("Product")));//{0}：{1} 尚未指定任何{2}！

                ddlDevice.DataSource = deviceVerDataSet;
                ddlDevice.DataTextField = "DeviceName";
                ddlDevice.DataValueField = "DeviceVersionSID";
                ddlDevice.DataBind();
                ddlDevice.Items.Insert(0, string.Empty);
            }

            if (woInfo.DeviceName.IsNullOrEmpty() == false)
            {
                itemp = ddlDevice.Items.FindByText(woInfo.DeviceName);
                if (itemp != null)
                {
                    ddlDevice.SelectedIndex = ddlDevice.Items.IndexOf(itemp);

                    //依造型號載入型號的所有流程
                    List<RouteInfo> routes = RouteInfo.GetDeviceRoute(woInfo.DeviceName, 1);
                    ddlRoute.DataSource = routes;
                    ddlRoute.DataTextField = "RouteName";
                    ddlRoute.DataValueField = "RouteSID";
                    ddlRoute.DataBind();
                    ddlRoute.Items.Insert(0, string.Empty);
                }
                else
                {
                    itemp = new ListItem(woInfo.DeviceName);
                    ddlDevice.Items.Insert(0, itemp);
                    ddlDevice.SelectedIndex = 0;
                }
            }
            #endregion

            #region 流程
            if (woInfo.RouteName.IsNullOrEmpty() == false)
            {
                itemp = ddlRoute.Items.FindByText(woInfo.RouteName);
                if (itemp != null)
                {
                    ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(itemp);
                }
                else
                {
                    ddlRoute.Items.Insert(0, woInfo.RouteName);
                    ddlRoute.SelectedIndex = 0;
                }
            }
            #endregion

            #region 優先等級
            itemp = ddlPriority.Items.FindByText(woInfo.Priority.ToCimesString());
            if (itemp != null)
            {
                ddlPriority.SelectedIndex = ddlPriority.Items.IndexOf(itemp);
            }
            else
            {
                ddlPriority.Items.Insert(0, woInfo.Priority.ToCimesString());
                ddlPriority.SelectedIndex = 0;
            }
            #endregion

            #region 生產等級
            itemp = ddlCritical.Items.FindByText(woInfo.Critical);
            if (itemp != null)
            {
                ddlCritical.SelectedIndex = ddlCritical.Items.IndexOf(itemp);
            }
            else
            {
                ddlCritical.Items.Insert(0, woInfo.Critical.ToCimesString());
                ddlCritical.SelectedIndex = 0;
            }
            #endregion

            #region 負責人
            itemp = ddlOwner.Items.FindByText(woInfo.Owner);
            if (itemp != null)
            {
                ddlOwner.SelectedIndex = ddlOwner.Items.IndexOf(itemp);
            }
            else
            {
                ddlOwner.Items.Insert(0, woInfo.Owner);
                ddlOwner.SelectedIndex = 0;
            }
            #endregion

            #region 依造狀態決定是否可以修正
            bool enableEditFlag = true;
            switch (woInfo.Flag)
            {
                case "UnRelease":
                    enableEditFlag = true;
                    break;
                case "Release":
                case "Created":
                    enableEditFlag = false;
                    break;
            }

            ddlCustomer.Enabled = enableEditFlag;
            ddlProduct.Enabled = enableEditFlag;
            ddlDevice.Enabled = enableEditFlag;
            ddlRoute.Enabled = enableEditFlag;
            ddlUnit.Enabled = enableEditFlag;
            ddlLotType.Enabled = enableEditFlag;
            ddlSUnit.Enabled = enableEditFlag;
            ddlOwner.Enabled = enableEditFlag;
            ddlPriority.Enabled = enableEditFlag;
            ddlCritical.Enabled = enableEditFlag;
            ddlLotTemplate.Enabled = enableEditFlag;
            gvLotTemplate.Enabled = enableEditFlag;
            ibtScheduleDate.Visible = enableEditFlag;
            ibtCommitDate.Visible = enableEditFlag;
            ibtDueDate.Visible = enableEditFlag;
            ttbRemark.Enabled = enableEditFlag;

            ttbQuantity.ReadOnly = !enableEditFlag;
            ttbSO.ReadOnly = !enableEditFlag;
            ttbSQuantity.ReadOnly = !enableEditFlag;
            rtbManual.Enabled = enableEditFlag;
            rtbAuto.Enabled = enableEditFlag;

            if (enableEditFlag == true)
            {
                ttbScheduleDate.Width = Unit.Percentage(85);
                ttbCommitDate.Width = Unit.Percentage(85);
                ttbDueDate.Width = Unit.Percentage(85);
            }
            else
            {
                ttbScheduleDate.Width = Unit.Percentage(97);
                ttbCommitDate.Width = Unit.Percentage(97);
                ttbDueDate.Width = Unit.Percentage(97);

            }
            #endregion

            if (woInfo["PRODLINE"].ToString() == "Manual")
            {
                rtbManual.Checked = true;
            }
            else if (woInfo["PRODLINE"].ToString() == "Auto")
            {
                rtbAuto.Checked = true;
            }
            else
            {
                rtbAuto.Checked = false;
                rtbManual.Checked = false;
            }

            LoadControlButton();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                EnableTab();
                ClearWOField();
                LoadControlDefault();

                _CurrentWorkOrder = InfoCenter.Create<WorkOrderInfo>();
                _CurrentWorkOrder.WorkOrder = "Automatically";
                _CurrentWorkOrder.Flag = "UnRelease";

                gvQuery.SelectedIndex = -1;
                BindGvData();
                LoadControlByWO(_CurrentWorkOrder);
                LoadControlButton();
                AjaxFocus(ddlCustomer);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnRelease_Click(object sender, EventArgs e)
        {
            try
            {
                if (_CurrentWorkOrder == null)
                    return;

                if (_CurrentWorkOrder.Flag != "UnRelease") return;

                if (CheckTag()) return;

                var routeVersion = RouteVersionInfo.GetRouteActiveVersion(_CurrentWorkOrder.RouteName);

                TransactionStamp txnStamp = new TransactionStamp(_ProgramInformationBlock.UserID, _ProgramInformationBlock.ProgramRight, _ProgramInformationBlock.ProgramRight, ApplicationName);
                using (CimesTransactionScope cts = CimesTransactionScope.Create())
                {
                    _CurrentWorkOrder.RouteVersion = routeVersion.RouteVersion;
                    _CurrentWorkOrder.Flag = "Release";
                    _CurrentWorkOrder.MMSTime = txnStamp.RecordTime;
                    _CurrentWorkOrder.RMSTime = txnStamp.RecordTime;
                    _CurrentWorkOrder.EDCTIME = txnStamp.RecordTime;
                    _CurrentWorkOrder.TOOLTime = txnStamp.RecordTime;
                    _CurrentWorkOrder.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

                    //新增MES_WPC_WO_HIST 紀錄
                    WorkOrderHistoryInfo woHistData = InfoCenter.Create<WorkOrderHistoryInfo>();
                    woHistData.WPC_WO_HIST_SID = DBCenter.GetSystemID();
                    _CurrentWorkOrder.Fill<WorkOrderHistoryInfo>(woHistData);
                    woHistData.InsertToDB();

                    cts.Complete();
                }

                int index = _WODataList.FindIndex(p => p.WorkOrder.Equals(_CurrentWorkOrder.WorkOrder));
                if (index > 0)
                {
                    _WODataList.RemoveAt(index);
                    _WODataList.Insert(index, _CurrentWorkOrder);

                    BindGvData();
                }

                LoadControlByWO(_CurrentWorkOrder);
                LoadControlButton();
                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00667(ttbWO.Text));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnUnRelease_Click(object sender, EventArgs e)
        {
            try
            {
                if (_CurrentWorkOrder == null)
                    return;

                if (_CurrentWorkOrder.Flag != "Release")
                {
                    //WRN-00012 此工單已產生批號，無法留滯！
                    throw new RuleCimesException(TextMessage.Error.T00310());
                }

                if (CheckTag()) return;
                string sRecTime = DBCenter.GetSystemTime();

                TransactionStamp txnStamp = new TransactionStamp(_ProgramInformationBlock.UserID, _ProgramInformationBlock.ProgramRight, _ProgramInformationBlock.ProgramRight, ApplicationName);
                using (CimesTransactionScope cts = CimesTransactionScope.Create())
                {
                    _CurrentWorkOrder.Flag = "UnRelease";
                    _CurrentWorkOrder.UpdateToDB(this.User.Identity.Name, sRecTime);

                    WorkOrderHistoryInfo woHistData = InfoCenter.Create<WorkOrderHistoryInfo>();
                    _CurrentWorkOrder.Fill<WorkOrderHistoryInfo>(woHistData);
                    woHistData.InsertToDB();
                    cts.Complete();
                }

                int index = _WODataList.FindIndex(p => p.WorkOrder.Equals(_CurrentWorkOrder.WorkOrder));
                if (index > 0)
                {
                    _WODataList.RemoveAt(index);
                    _WODataList.Insert(index, _CurrentWorkOrder);

                    BindGvData();
                }

                LoadControlByWO(_CurrentWorkOrder);
                LoadControlButton();
                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00578(ttbWO.Text));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private bool CheckTag()
        {
            WorkOrderInfo woData = WorkOrderInfo.GetWorkOrderByWorkOrder(ttbWO.Text.ToCimesString());

            //判斷Tag是否有被變更, 若Tag相同才可以執行
            if (_CurrentWorkOrder.Tag != woData.Tag)
                throw new RuleCimesException(TextMessage.Error.T00747(""));
            return false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckFunction();

                if (_CurrentWorkOrder == null)
                    return;

                RouteVersionInfo activeRouteVersion = RouteVersionInfo.GetRouteActiveVersion(ddlRoute.SelectedItem.Text);
                if (activeRouteVersion == null)
                {
                    //WRN-00103 流程{0}無線上版本！
                    throw new RuleCimesException(TextMessage.Error.T00562(GetUIResource(ddlRoute.SelectedItem.Text)), ddlRoute);
                }

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, _ProgramInformationBlock.ProgramRight, _ProgramInformationBlock.ProgramRight, ApplicationName);
                using (CimesTransactionScope cts = CimesTransactionScope.Create())
                {
                    #region 更新工單資料
                    if (_CurrentWorkOrder.InfoState == InfoState.NewCreate)
                    {
                        _CurrentWorkOrder.WorkOrder = GenerateNewWO(_CurrentWorkOrder, "WO", txnStamp);
                        _CurrentWorkOrder.Flag = "UnRelease";
                        _CurrentWorkOrder.Tag = 1;
                        _CurrentWorkOrder.CreateTime = txnStamp.RecordTime;
                        _CurrentWorkOrder.MMSTime = txnStamp.RecordTime;
                        _CurrentWorkOrder.RMSTime = txnStamp.RecordTime;
                        _CurrentWorkOrder.EDCTIME = txnStamp.RecordTime;
                        _CurrentWorkOrder.TOOLTime = txnStamp.RecordTime;
                    }
                    else
                    {
                        CheckTag();
                    }

                    _CurrentWorkOrder.SO = ttbSO.Text;
                    _CurrentWorkOrder.LotType = ddlLotType.SelectedItem.Text;
                    _CurrentWorkOrder.Template = ddlLotTemplate.SelectedItem.Text;
                    _CurrentWorkOrder.Quantity = ttbQuantity.Text.ToCimesDecimal(0);
                    _CurrentWorkOrder.Unit = ddlUnit.SelectedItem.Text;
                    _CurrentWorkOrder.SecondQuantity = ttbSQuantity.Text.ToCimesDecimal(0);
                    _CurrentWorkOrder.SecondUnit = ddlSUnit.SelectedItem.Text;
                    _CurrentWorkOrder.Customer = ddlCustomer.SelectedItem.Text;
                    _CurrentWorkOrder.DeviceName = ddlDevice.SelectedItem.Text;
                    _CurrentWorkOrder.DeviceVersion = 1;
                    _CurrentWorkOrder.ProductName = ddlProduct.SelectedItem.Text;
                    _CurrentWorkOrder.ProductVersion = 1;
                    _CurrentWorkOrder.RouteName = ddlRoute.SelectedItem.Text;
                    _CurrentWorkOrder.RouteVersion = activeRouteVersion.RouteVersion;
                    _CurrentWorkOrder.Priority = ddlPriority.SelectedItem.Text.ToCimesDecimal();
                    _CurrentWorkOrder.Critical = ddlCritical.SelectedItem.Text;
                    _CurrentWorkOrder.Owner = ddlOwner.SelectedItem.Text;
                    _CurrentWorkOrder.CommitDate = CorrectDate(ttbCommitDate.Text);
                    _CurrentWorkOrder.ScheduleDate = CorrectDate(ttbScheduleDate.Text);
                    _CurrentWorkOrder.DueDate = CorrectDate(ttbDueDate.Text);
                    _CurrentWorkOrder.Description = ttbRemark.Text.Trim();

                    if (rtbManual.Checked)
                    {
                        _CurrentWorkOrder["PRODLINE"] = "Manual";
                    }
                    else
                    {
                        _CurrentWorkOrder["PRODLINE"] = "Auto";
                    }

                    if (_CurrentWorkOrder.InfoState == InfoState.NewCreate)
                    {
                        _CurrentWorkOrder.InsertImmediately(txnStamp.UserID, txnStamp.RecordTime);
                    }
                    else
                    {
                        _CurrentWorkOrder.UpdateImmediately(txnStamp.UserID, txnStamp.RecordTime);
                    }

                    
                    #endregion

                    #region 新增MES_WPC_WO_HIST 紀錄
                    WorkOrderHistoryInfo woHistData = InfoBase.Create<WorkOrderHistoryInfo>();
                    woHistData.WPC_WO_HIST_SID = DBCenter.GetSystemID();
                    _CurrentWorkOrder.Fill<WorkOrderHistoryInfo>(woHistData);
                    woHistData.InsertToDB();
                    #endregion

                    #region 更新批號範本屬性
                    if (_CurrentWorkOrder.Template.IsNullOrTrimEmpty() == false)
                    {
                        for (int i = 0; i < gvLotTemplate.Rows.Count; i++)
                        {
                            AttributeInstanceInfo instInfo = _LotTemplateAttrInstancies[i];
                            var attrInfo = _LotTemplateAttrs.Find(p => p.AttributeName.Equals(instInfo.AttributeName));
                            string value = "";

                            TextBox ttbValue = (TextBox)gvLotTemplate.Rows[i].FindControl("ttbValue");
                            DropDownList ddlValue = (DropDownList)gvLotTemplate.Rows[i].FindControl("ddlValue");

                            switch (instInfo.CheckType)
                            {
                                case "SQLString":
                                case "ExtendItem":
                                    value = ddlValue.SelectedValue;
                                    break;
                                default:
                                    value = ttbValue.Text.Trim();
                                    break;
                            }

                            if (attrInfo != null)
                            {
                                if (attrInfo.AttributeValue.Equals(value) == false)
                                    attrInfo.AttributeValue = value;
                            }
                            else
                            {
                                attrInfo = InfoCenter.Create<AttributeAttributeInfo>();
                                attrInfo.ObjectSID = _CurrentWorkOrder.WorkOrderSID;
                                attrInfo.AttributeName = instInfo.AttributeName;
                                attrInfo.AttributeValue = value;
                                attrInfo.DataClass = "WorkOrderAttribute";
                                _LotTemplateAttrs.Add(attrInfo);
                            }
                        }
                    }

                    _LotTemplateAttrs.ForEach(attr =>
                    {
                        switch (attr.InfoState)
                        {
                            case InfoState.NewCreate:
                                attr.InsertImmediately(txnStamp.UserID, txnStamp.RecordTime);
                                //LogCenter.LogToDB(attr, LogCenter.LogIndicator.Default);
                                break;
                            case InfoState.Modified:
                                attr.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                                //LogCenter.LogToDB(attr, LogCenter.LogIndicator.Create(ActionType.Set, txnStamp.UserID, txnStamp.RecordTime));
                                break;
                            case InfoState.Deleted:
                                attr.DeleteFromDB();
                                //LogCenter.LogToDB(attr, LogCenter.LogIndicator.Create(ActionType.Remove, txnStamp.UserID, txnStamp.RecordTime));
                                break;
                        }
                    });
                    #endregion

                    cts.Complete();
                }

                _CurrentWorkOrder = WorkOrderInfo.GetWorkOrderByWorkOrder(_CurrentWorkOrder.WorkOrder);

                int index = _WODataList.FindIndex(p => p.WorkOrder.Equals(_CurrentWorkOrder.WorkOrder));
                if (index < 0)
                {
                    _WODataList.Add(_CurrentWorkOrder);
                    gvQuery.PageIndex = _WODataList.Count / gvQuery.PageSize;
                    gvQuery.SelectedIndex = _WODataList.Count - (gvQuery.PageSize * (gvQuery.PageIndex)) - 1;
                    if (gvQuery.SelectedIndex < 0)
                        gvQuery.SelectedIndex = gvQuery.PageSize - 1;
                }
                else
                {
                    _WODataList.RemoveAt(index);
                    _WODataList.Insert(index, _CurrentWorkOrder);
                }

                BindGvData();

                LoadControlByWO(_CurrentWorkOrder);

                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00083(_CurrentWorkOrder.WorkOrder));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private string GenerateNewWO(WorkOrderInfo wo, string namingRule, TransactionStamp txnStamp)
        {
            var idGen = NamingIDGenerator.GetRule(namingRule);
            if (idGen == null)
            {
                //WRN-00200 無[{0}]設定資料，請洽IT人員！
                throw new Exception(TextMessage.Error.T00654("NamingSetup[" + namingRule + "]"));
            }

            var result = idGen.GenerateNextIDs(1, wo, new string[] { }, txnStamp.UserID);
            if (result.Second != null && result.Second.Count > 0)
            {
                DBCenter.ExecuteSQL(result.Second);
            }

            return result.First[0];
        }

        private string CorrectDate(string date)
        {
            if (date.Trim() == string.Empty) return string.Empty;
            DateTime dt = DateTime.Parse(date);

            return string.Format(@"{0:yyyy\/MM\/dd}", dt);
        }

        private void CheckFunction()
        {
            ddlCustomer.Must(lblCustomer);
            ddlProduct.Must(lblProduct);
            ddlDevice.Must(lblDevice);
            ddlRoute.Must(lblRoute);
            CheckTextBox(ttbQuantity, lblQuantity, CheckDataType.GreaterZeroDecimal);
            ddlUnit.Must(lblUnit);
            ddlLotType.Must(lblLotType);
            //若第二數量有輸入，需檢查次單位是否有輸入
            if (!ttbSQuantity.Text.IsNullOrTrimEmpty() && ttbSQuantity.Text != "0")
            {
                CheckTextBox(ttbSQuantity, lblSecondQty, CheckDataType.GreaterEqualZeroDecimal);
                ddlSUnit.Must(lblSecondUnit);
            }

            if (!rtbAuto.Checked && !rtbManual.Checked)
            {
                throw new RuleCimesException(TextMessage.Error.T00839(rtbManual.Text + "," + rtbAuto.Text));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearWOField();
                LoadControlButton();
                DisableTab();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void DisableTab()
        {
            foreach (CimesTabItem tab in rtsDetail.Tabs)
            {
                tab.Enabled = false;
            }
            foreach (CimesPageView view in rmpDetail.PageViews)
            {
                view.Enabled = false;
            }
        }

        private void ClearFiled()
        {
            _WODataList = null;
            gvLotTemplate.DataSource = null;
            gvLotTemplate.DataBind();

            gvWOLotTemplate.DataSource = null;
            gvWOLotTemplate.DataBind();

            ClearWOField();
            LoadControlButton();
            DisableTab();
            if (_dsReport != null)
                _dsReport.Tables.Clear();
        }

        private void ClearWOField()
        {
            _CurrentWorkOrder = null;
            _LotTemplateAttrInstancies = null;
            _LotTemplateAttrs = null;

            ttbWO.Text = string.Empty;
            ttbCommitDate.Text = string.Empty;
            ttbDueDate.Text = string.Empty;
            ttbScheduleDate.Text = string.Empty;
            ttbSO.Text = string.Empty;
            ttbCommitDate.Text = string.Empty;
            ttbScheduleDate.Text = string.Empty;
            ttbDueDate.Text = string.Empty;
            ttbQuantity.Text = "";
            ttbSQuantity.Text = "";
            ttbWO.Text = "Automatically";
            ttbRemark.Text = "";

            ddlCustomer.ClearSelection();
            ddlProduct.ClearSelection();
            ddlUnit.ClearSelection();
            ddlLotType.ClearSelection();
            ddlOwner.ClearSelection();
            ddlSUnit.ClearSelection();

            ddlDevice.Items.Clear();
            ddlRoute.Items.Clear();
            ddlPriority.ClearSelection();
            ddlCritical.ClearSelection();
        }

        protected void ddlLotTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _LotTemplateAttrInstancies = null;
                _LotTemplateAttrs = null;
                gvLotTemplate.DataSource = null;
                gvLotTemplate.DataBind();

                if (ddlLotTemplate.SelectedItem.Value.IsNullOrTrimEmpty() == true)
                {
                    return;
                }

                LoadLotTemplate(ddlLotTemplate.SelectedItem.Text);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void LoadLotTemplate(string lotTemplate)
        {
            string sql = @"SELECT IST.ATTR_INST_SID, IST.ATTRIBUTENAME, IST.DATACLASS, IST.DESCR, IST.DATATYPE, TA.VALUE AS DEFAULTVALUE, IST.CHECKTYPE, IST.CHECKCONDITION, IST.STATUS, IST.REQUESTFLAG, IST.ACTIVEFLAG, IST.TAG, IST.USERID, IST.UPDATETIME 
                      FROM MES_WIP_TEMP T
                     INNER JOIN  MES_WIP_TEMP_ATTR TA ON  T.WIP_TEMP_SID = TA.WIP_TEMP_SID 
                     INNER JOIN MES_ATTR_INST IST ON  TA.WIP_ATTR_INST_SID = IST.ATTR_INST_SID AND IST.DATACLASS = 'LotAttribute'  
                     WHERE T.TEMPLATE = #[STRING]";

            _LotTemplateAttrInstancies = InfoCenter.GetList<AttributeInstanceInfo>(sql, lotTemplate);
            _LotTemplateAttrs = AttributeAttributeInfo.GetByObjectSIDAndDataClass(_CurrentWorkOrder.WorkOrderSID, "WorkOrderAttribute");

            gvLotTemplate.DataSource = _LotTemplateAttrInstancies;
            gvLotTemplate.DataBind();
        }


        protected void ddlProduct_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                //清除連動資料
                ddlDevice.ClearSelection();
                ddlDevice.DataSource = null;
                ddlDevice.DataBind();
                ddlDevice_SelectedIndexChanged(ddlDevice, null);

                if (ddlProduct.SelectedItem == null)
                    return;

                ProductInfo prodData = ProductInfo.GetProductByName(ddlProduct.SelectedItem.Text);
                if (prodData == null)
                    //產品不存在
                    throw new RuleCimesException(TextMessage.Error.T00030(lblProduct.Text, ddlProduct.SelectedItem.Text));

                List<DeviceVersionInfo> deviceVerDataSet = DeviceVersionInfo.GetActiveDeviceVersionsByProduct(prodData);
                if (deviceVerDataSet.Count == 0)
                    //{0}：{1} 尚未指定任何{2}！
                    throw new RuleCimesException(TextMessage.Error.T00026(lblProduct.Text, lblDevice.Text, lblProduct.Text));

                ddlDevice.DataSource = deviceVerDataSet;
                ddlDevice.DataTextField = "DeviceName";
                ddlDevice.DataValueField = "DeviceVersionSID";
                ddlDevice.DataBind();
                ddlDevice.Items.Insert(0, string.Empty);

                AjaxFocus(ddlDevice);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ddlDevice_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                //清除連動資料
                ddlRoute.ClearSelection();
                ddlRoute.DataSource = null;
                ddlRoute.DataBind();

                if (ddlDevice.SelectedItem == null || ddlDevice.SelectedItem.Text.IsNullOrEmpty() == true)
                    return;

                List<RouteInfo> routes = RouteInfo.GetDeviceRoute(ddlDevice.SelectedItem.Text, 1);
                ddlRoute.DataSource = routes;
                ddlRoute.DataTextField = "RouteName";
                ddlRoute.DataValueField = "RouteSID";
                ddlRoute.DataBind();
                ddlRoute.Items.Insert(0, string.Empty);

                if (routes.Count == 0)
                {
                    //WRN-00188 {0}：[{1}]，無法找到對應的流程。
                    throw new Exception(TextMessage.Error.T00025(lblDevice.Text, ddlDevice.SelectedItem.Text));
                }

                AjaxFocus(ddlRoute);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFiled();

                ttbWOSelect.Text = ttbWOSelect.Text.Trim();

                #region 狀態
                string flagCheck = "";
                if (cbxStatusUnAll.Checked == true)
                    flagCheck = "";

                if (cbxStatusUnRelease.Checked == true)
                    flagCheck = "UnRelease";

                if (cbxStatusRelease.Checked == true)
                    flagCheck = "Release";

                if (cbxStatusCreated.Checked == true)
                    flagCheck = "Created";
                #endregion

                QueryData(ttbWOSelect.Text, flagCheck);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvLotTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                AttributeInstanceInfo instInfo = (AttributeInstanceInfo)e.Row.DataItem;
                var attrInfo = _LotTemplateAttrs.Find(p => p.AttributeName.Equals(instInfo.AttributeName));

                //如果值是空的用預設值來替代
                string value = attrInfo != null ? attrInfo.AttributeValue : instInfo.DefaultValue;

                TextBox ttbValue = (TextBox)e.Row.FindControl("ttbValue");
                DropDownList ddlValue = (DropDownList)e.Row.FindControl("ddlValue");

                switch (instInfo.CheckType)
                {
                    case "SQLString":
                        {
                            #region SQLString
                            ddlValue.Visible = true;
                            ttbValue.Visible = false;

                            ddlValue.Items.Clear();
                            DataTable dtCheck = DBCenter.GetDataTable(instInfo.CheckCondition);
                            if (dtCheck != null && dtCheck.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtCheck.Rows.Count; i++)
                                {
                                    ddlValue.Items.Add(new ListItem(dtCheck.Rows[i][0].ToCimesString()));
                                }
                            }
                            ddlValue.Items.Insert(0, new ListItem());

                            ListItem item = ddlValue.Items.FindByText(value);
                            if (item != null)
                                ddlValue.SelectedIndex = ddlValue.Items.IndexOf(item);
                            #endregion
                        }
                        break;
                    case "ExtendItem":
                        {
                            #region ExtendItem
                            ddlValue.Visible = true;
                            ttbValue.Visible = false;

                            ddlValue.DataSource = WpcExClassItemInfo.GetInfoByClass(instInfo.CheckCondition);
                            ddlValue.DataTextField = "Remark01";
                            ddlValue.DataValueField = "Remark01";
                            ddlValue.DataBind();
                            ddlValue.Items.Insert(0, new ListItem());

                            ListItem item = ddlValue.Items.FindByText(value);
                            if (item != null)
                                ddlValue.SelectedIndex = ddlValue.Items.IndexOf(item);
                            #endregion
                        }
                        break;
                    default:
                        ddlValue.Visible = false;
                        ttbValue.Visible = true;
                        ttbValue.Text = value;
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvQuery_DataSourceChanged(object sender, Ares.Cimes.IntelliService.Web.UI.CimesDataSourceEventArgs e)
        {
            try
            {
                _WODataList = (e.CurrentDataSource as IList).NewList(p => (WorkOrderInfo)p);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvQuery_CimesSorted(object sender, Ares.Cimes.IntelliService.Web.UI.CimesSortedArgs e)
        {
            try
            {
                btnCancel_Click(btnCancel, null);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvQuery_CimesFiltered(object sender, Ares.Cimes.IntelliService.Web.UI.CimesFilteredArgs e)
        {
            try
            {
                btnCancel_Click(btnCancel, null);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}
