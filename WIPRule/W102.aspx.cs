/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：Keith

功能說明：提供批號從自動線轉手動線。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/09/18      Keith       初版
*/

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.RuleUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.WIPRule
{
    public partial class W102 : CustomizeRuleBasePage
    {
        private class RouteOperationData
        {
            public string OperationName { get; set; }
            public string RouteOperationSID { get; set; }
        }

        /// <summary>
        /// 可合併的工件序號清單資料
        /// </summary>
        private List<LotInfoEx> _MergeLotDataList
        {
            get { return this["_MergeLotDataList"] as List<LotInfoEx>; }
            set { this["_MergeLotDataList"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            ttbCompLot.Text = "";
            ttbHaveMergeQty.Text = "";
            ttbMergeQty.Text = "";
            ttbNotMergeQty.Text = "";
            ttbOperation.Text = "";
            ttbWOLot.Text = "";
            ttbWOQty.Text = "";

            
            ddlRoute.Items.Clear();
            ddlRoute.Enabled = false;

            ddlRouteOperation.Items.Clear();
            ddlRouteOperation.Enabled = false;
            btnOK.Enabled = false;

            gvMergeCompLot.Initialize();
            gvNotMergeCompLot.Initialize();
        }

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
                    AjaxFocus(ttbCompLot);

                    gvMergeCompLot.Initialize();
                    gvNotMergeCompLot.Initialize();
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
        /// 輸入工件序號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbCompLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal haveMergeQty = 0;
                decimal mergeQty = 0;
                decimal notMergeQty = 0;

                //確認工件序號是否有輸入
                ttbCompLot.Must(lblCompLot);

                //確認工件序號是否存在
                var lotList = LotInfoEx.GetLotListByComponentLot(ttbCompLot.Text);
                if (lotList.Count == 0)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ttbCompLot.Text));
                }

                var compLotData = lotList[0];

                //確認狀態是否為WAIT
                if (compLotData.Status != LotDefaultStatus.Wait.ToString())
                {
                    //狀態為{0}，不可執行
                    throw new Exception(RuleMessage.Error.C10003(compLotData.Status));
                }

                //確認批號執行規則是否與程式相同
                //if (compLotData.CurrentRuleName != _ProgramInformationBlock.ProgramRight)
                //{
                //    //該批號作業為XXXX，不為此功能，請遵循作業規範
                //    throw new Exception(RuleMessage.Error.C10004(compLotData.CurrentRuleName));
                //}

                //取得小工單資料
                var workOrderLot = CSTWorkOrderLotInfo.GetWorkOrderLotDataByWorkOrderLot(compLotData.WorkOrderLot);
                if (workOrderLot == null)
                {
                    // [00030]{0}：{1}不存在!
                    throw new Exception(TextMessage.Error.T00030(lblWOLot.Text, compLotData.WorkOrderLot));
                }

                //取得相同小工單的工件序號清單資料
                var sameWOLotList = LotInfoEx.GetLotListByWorkOrderLot(compLotData.WorkOrderLot);

                #region 取得相同工作站、UDC08等於Y及狀態為Wait的工件序號清單
                var autoTypeLotList = sameWOLotList.FindAll(lot => lot.UserDefineColumn08 == "Y");

                _MergeLotDataList = autoTypeLotList.FindAll(lot => lot.OperationName == compLotData.OperationName && lot.Status == LotDefaultStatus.Wait.ToString());

                //計算可合併數量
                _MergeLotDataList.ForEach(lot => { mergeQty += lot.Quantity; });
                #endregion

                #region 取得不可合併的工件序號清單
                var notMergeLotDataList = autoTypeLotList.FindAll(lot => (lot.OperationName == compLotData.OperationName && lot.Status == LotDefaultStatus.Wait.ToString()) == false);

                //計算不可合併數量
                notMergeLotDataList.ForEach(lot => { notMergeQty += lot.Quantity; });
                #endregion

                #region 取得已合併的工件序號清單
                var haveMergeLotDataList = sameWOLotList.FindAll(lot => lot.UserDefineColumn08 == "N");

                //計算已合併數量
                haveMergeLotDataList.ForEach(lot => { haveMergeQty += lot.Quantity; });
                #endregion

                //取得流程清單
                GetRoute(compLotData);

                //顯示介面資料
                ttbOperation.Text = compLotData.OperationName;
                ttbWOLot.Text = compLotData.WorkOrderLot;
                ttbWOQty.Text = workOrderLot.Quantity.ToString();
                ttbMergeQty.Text = mergeQty.ToString();
                ttbNotMergeQty.Text = notMergeQty.ToString();
                ttbHaveMergeQty.Text = haveMergeQty.ToString();

                gvMergeCompLot.SetDataSource(_MergeLotDataList, true);
                gvNotMergeCompLot.SetDataSource(notMergeLotDataList, true);

                btnOK.Enabled = true;

            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbCompLot);
                HandleError(ex);
            }
        }

        /// <summary>
        /// 取得流程清單
        /// </summary>
        private void GetRoute(LotInfoEx lotData)
        {
            //清除資料
            ddlRoute.Items.Clear();
            ddlRoute.Enabled = false;

            //依據目前的料號及料號版本取得所有可選的流程
            var routeList = RouteInfo.GetDeviceRoute(lotData.DeviceName, lotData.DeviceVersion);
            if (routeList.Count > 0)
            {
                ddlRoute.DataSource = routeList;
                ddlRoute.DataTextField = "RouteName";
                ddlRoute.DataValueField = "RouteSID";
                ddlRoute.DataBind();

                if (ddlRoute.Items.Count != 1)
                    ddlRoute.Items.Insert(0, "");
                else
                {
                    ddlRoute.SelectedIndex = 0;
                }

                ddlRoute.Enabled = true;
            }
        }

        /// <summary>
        /// 流程切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //清除資料
                ddlRouteOperation.Items.Clear();
                ddlRouteOperation.Enabled = false;

                //確認是否有選擇流程
                ddlRoute.Must(lblRoute);

                //取得所選擇的流程資料
                var routeData = InfoCenter.GetBySID<RouteInfo>(ddlRoute.SelectedValue);

                //取得所選擇流程目前線上版本資料
                var routeVersionData = RouteVersionInfo.GetActiveRouteVersion(routeData);

                //取得所選擇流程目前線上版本的所有工作站資料
                var routeOperationList = RouteOperationInfo.GetRouteAllOperations(routeVersionData);
                if (routeOperationList.Count > 0)
                {
                    List<RouteOperationData> routeOperationDataList = new List<RouteOperationData>();

                    //轉換型態
                    routeOperationList.ForEach(data =>
                    {
                        var operData = OperationInfo.GetOperationByName(data.OperationName);

                        //CNC的站點不可併批(需排除)，因為CNC後才能併批
                        if (operData["AUTOMERGE"].ToBool() == false && operData["FAIFLAG"].ToBool() == false && operData["MAIN_EQUIP"].ToBool() == false)
                        {
                            RouteOperationData routeOperationData = new RouteOperationData();
                            routeOperationData.OperationName = string.Format("[{0}]{1}", data.OperationSequence, data.OperationName);
                            routeOperationData.RouteOperationSID = data.RouteOperationSID;

                            routeOperationDataList.Add(routeOperationData);
                        }
                    });

                    ddlRouteOperation.DataSource = routeOperationDataList;
                    ddlRouteOperation.DataTextField = "OperationName";
                    ddlRouteOperation.DataValueField = "RouteOperationSID";
                    ddlRouteOperation.DataBind();

                    if (ddlRouteOperation.Items.Count != 1)
                        ddlRouteOperation.Items.Insert(0, "");
                    else
                    {
                        ddlRouteOperation.SelectedIndex = 0;
                    }

                    ddlRouteOperation.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //確認是否輸入工件序號
                ttbCompLot.Must(lblCompLot);

                //確認是否有選擇流程
                ddlRoute.Must(lblRoute);

                //確認是否有選擇流程工作站
                ddlRouteOperation.Must(lblRouteOperation);
                
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    //以第一個工件序號為拆批的母批
                    var txnLotData = _MergeLotDataList[0];

                    //取得原因碼資料
                    var reasonData = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");

                    #region 執行拆批
                    //批號子單元清單
                    var lsComponetData = ComponentInfoEx.GetDataByCurrentLot(txnLotData.Lot);

                    //取得併批批號名稱
                    var splitLotNaming = GetNamingRule("SplitLot", txnStamp.UserID, txnLotData);

                    //批號拆批
                    var splitLot = SplitLotInfo.CreateSplitLotByLotAndQuantity(txnLotData.Lot, splitLotNaming.First[0], lsComponetData, reasonData, reasonData.Description);
                    WIPTxn.Default.SplitLot(txnLotData, splitLot, WIPTxn.SplitIndicator.Create(null, null, null, TerminateBehavior.TerminateWithHistory), txnStamp);

                    //將批號的UDC08註記N
                    WIPTransaction.ModifyLotSystemAttribute(splitLot, "USERDEFINECOL08", "N", txnStamp);

                    if (splitLotNaming.Second != null && splitLotNaming.Second.Count != 0)
                    {
                        DBCenter.ExecuteSQL(splitLotNaming.Second);
                    }
                    #endregion

                    #region 執行併批
                    //移除第一個工件序號(因為拆批已處理過了)
                    _MergeLotDataList.RemoveAt(0);

                    List<MergeLotInfo> lsMergeLots = new List<MergeLotInfo>();

                    foreach (var mergeLot in _MergeLotDataList)
                    {
                        //批號子單元清單
                        var componetDataList = ComponentInfoEx.GetDataByCurrentLot(mergeLot.Lot);

                        //加入併批清單
                        lsMergeLots.Add(MergeLotInfo.GetMergeLotByLotAndQuantity(mergeLot.Lot, componetDataList, reasonData, reasonData.Description));
                    }

                    //將數量合併到在狀態為Run的批號上
                    if (lsMergeLots.Count > 0)
                    {
                        WIPTransaction.MergeLot(splitLot, lsMergeLots, txnStamp);
                    }
                    #endregion

                    #region 變更流程

                    //取得料號版本資料
                    var deviceVersionData = DeviceVersionInfo.GetDeviceVersion(splitLot.DeviceName, splitLot.DeviceVersion);

                    //取得所選擇的流程資料
                    var routeData = InfoCenter.GetBySID<RouteInfo>(ddlRoute.SelectedValue);

                    //取得所選擇流程目前線上版本資料
                    var routeVersionData = RouteVersionInfo.GetActiveRouteVersion(routeData);

                    //取得所選擇的流程工作站資料
                    var routeOpeartionData = InfoCenter.GetBySID<RouteOperationInfo>(ddlRouteOperation.SelectedValue);

                    WIPTransaction.ReassignRoute(splitLot, deviceVersionData, routeVersionData, routeOpeartionData, reasonData, reasonData.Description, txnStamp);

                    #endregion

                    cts.Complete();
                }

                ClearField();
                AjaxFocus(ttbCompLot);

                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00614(""));

            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 離開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //返回在製品查詢頁面
                ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 取得Naming
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="user"></param>
        /// <param name="info"></param>
        /// <param name="lstArgs"></param>
        /// <returns></returns>
        public Pair<string[], List<SqlAgent>> GetNamingRule(string ruleName, string user, InfoBase info = null, List<string> lstArgs = null)
        {
            var naming = NamingIDGenerator.GetRule(ruleName);
            if (naming == null)
            {
                throw new Exception(TextMessage.Error.T00437(ruleName));   //[00437]找不到可產生的序號，請至命名規則維護設定，規則名稱：{0}!!!
            }

            if (lstArgs == null)
            {
                lstArgs = new List<string>();
            }

            if (info != null)
            {
                var result = naming.GenerateNextIDs(1, info, lstArgs.ToArray(), user);
                return new Pair<string[], List<SqlAgent>>(result.First, result.Second);
            }
            else
            {
                var result = naming.GenerateNextIDs(1, lstArgs.ToArray(), user);
                return new Pair<string[], List<SqlAgent>>(result.First, result.Second);
            }
        }
    }
}