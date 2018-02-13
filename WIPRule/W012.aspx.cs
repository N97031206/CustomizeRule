/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：進行不良品的判定。
良品：送回送出站的下一站。
維修：送到下一站(原則上一定是維修站)。
不良品入庫：變更批號狀態為DefectInv。
報廢品入庫：變更批號狀態為ScrapInv。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/08/02      keith       初版
*/


using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.RuleUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.WIPRule
{
    public partial class W012 : CustomizeRuleBasePage
    {
        /// <summary>
        /// LOT資料
        /// </summary>
        private LotInfoEx _LotData
        {
            get { return this["_LotData"] as LotInfoEx; }
            set { this["_LotData"] = value; }
        }

        /// <summary>
        /// LOTDefect資料
        /// </summary>
        private LotDefectInfo _LotDefectData
        {
            get { return this["_LotDefectData"] as LotDefectInfo; }
            set { this["_LotDefectData"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            ttbItemSN.Text = "";
            ddlJudgeDefect.ClearSelection();

            ttbQty.Text = "";
            ttbJudgeDescr.Text = "";

            ddlJudgeReason.Items.Clear();
            ddlJudgeDefect.Items.Clear();
            _LotData = null;
            _LotDefectData = null;
            rdbGoods.Checked = false;
            rdbDefectInv.Checked = false;
            rdbRepair.Checked = false;
            rdbScrapInv.Checked = false;
            btnOK.Enabled = false;
            btnPrint.Enabled = false;
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
                    ttbLot.Text = "";

                    //第一次開啟頁面
                    ClearField();
                    AjaxFocus(ttbLot);
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
        /// 輸入機加批號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //取得 LOT / COMPONENTID / MATERIALLOT / INVLOT / WOLOT
                string lot = CustomizeFunction.ConvertDMCCode(ttbLot.Text.Trim());

                //清除資料
                ClearField();

                //是否取得批號資料旗標
                bool isFindLotData = false;

                //確認輸入資料是否為LOT
                isFindLotData = CheckLot(lot);

                if (isFindLotData == false)
                {
                    //確認輸入資料是否為COMPONENTID
                    isFindLotData = CheckComponentID(lot);
                }

                if (isFindLotData == false)
                {
                    //確認此批號是否為MATERIALLOT
                    isFindLotData = CheckMaterialLot(lot);
                }

                if (isFindLotData == false)
                {
                    //確認此批號是否為INVLOT
                    isFindLotData = CheckInvLot(lot);
                }

                if (isFindLotData == false)
                {
                    //確認此批號是否為WOLOT
                    isFindLotData = CheckWOLot(lot);
                }

                //如果五種搜尋方式都沒有找到批號資料的話，則顯示錯誤訊息
                if (isFindLotData == false)
                {
                    //[00030]{0}：{1}不存在!
                    throw new Exception(TextMessage.Error.T00030(lblLot.Text, ttbLot.Text));
                }
            }
            catch (Exception ex)
            {
                ttbLot.Text = "";

                ClearField();
                AjaxFocus(ttbLot);
                HandleError(ex);
            }
        }

        /// <summary>
        /// 確認輸入資料是否為LOT
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        private bool CheckLot(string lot)
        {
            //確認輸入資料是否為LOT
            var lotData = LotInfoEx.GetLotByLot(lot).ChangeTo<LotInfoEx>();
            if (lotData == null)
            {
                return false;
            }

            //驗證批號狀態，Wait的批號
            if (lotData.Status != LotDefaultStatus.Wait.ToString())
            {
                //狀態為{0}，不可執行
                throw new Exception(RuleMessage.Error.C10003(lotData.Status));
            }

            //驗證currentRule
            if (lotData.CurrentRuleName != _ProgramInformationBlock.ProgramRight)
            {
                //該批號作業為XXXX，不為此功能，請遵循作業規範
                throw new Exception(RuleMessage.Error.C10004(lotData.CurrentRuleName));
            }

            //取得工件序號
            var componentDataList = ComponentInfoEx.GetDataByCurrentLot(lotData.Lot);
            if (componentDataList.Count > 0)
            {
                ttbItemSN.Text = componentDataList[0].ComponentID;
            }

            //取得送待判原因
            var lotDefectData = LotDefectInfoEx.GetDataByLotAndComponentID(lotData.Lot, ttbItemSN.Text);
            if (lotDefectData != null)
            {
                var reason = ReasonInfo.GetReasonByName(lotDefectData.ReasonCode);
                if (reason == null)
                    throw new CimesException(RuleMessage.Error.C00051(lotDefectData.ReasonCode));

                //格式:(批號) 原因碼_說明_備註 
                string defectText = "(" + lotDefectData.Lot + ")";

                defectText += " " + lotDefectData.ReasonCode;

                if (!reason.Description.IsNullOrTrimEmpty())
                    defectText += "_" + reason.Description;

                if (!lotDefectData.Description.IsNullOrTrimEmpty())
                    defectText += "_" + lotDefectData.Description;

                ddlJudgeDefect.Items.Add(new ListItem(defectText, lotDefectData.ID));

                ddlJudgeDefect_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                //查無送待判原因！
                throw new Exception(RuleMessage.Error.C10187());
            }

            return true;
        }

        /// <summary>
        /// 確認此批號是否為COMPONENTID
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        private bool CheckComponentID(string lot)
        {
            //確認此批號是否為COMPONENTID
            var component = ComponentInfo.GetComponentByComponentID(lot);
            if (component == null)
            {
                return false;
            }

            //取得批號資料
            var lotData = LotInfoEx.GetLotByLot(component.CurrentLot).ChangeTo<LotInfoEx>();
            if (lotData == null)
            {
                return false;
            }

            //驗證批號狀態，Wait的批號
            if (lotData.Status != LotDefaultStatus.Wait.ToString())
            {
                //狀態為{0}，不可執行
                throw new Exception(RuleMessage.Error.C10003(lotData.Status));
            }

            //驗證currentRule
            if (lotData.CurrentRuleName != _ProgramInformationBlock.ProgramRight)
            {
                //該批號作業為XXXX，不為此功能，請遵循作業規範
                throw new Exception(RuleMessage.Error.C10004(lotData.CurrentRuleName));
            }

            //取得工件序號
            var componentDataList = ComponentInfoEx.GetDataByCurrentLot(lotData.Lot);
            if (componentDataList.Count > 0)
            {
                ttbItemSN.Text = componentDataList[0].ComponentID;
            }

            //取得送待判原因
            var lotDefectData = LotDefectInfoEx.GetDataByLotAndComponentID(lotData.Lot, ttbItemSN.Text);
            if (lotDefectData != null)
            {
                var reason = ReasonInfo.GetReasonByName(lotDefectData.ReasonCode);
                if (reason == null)
                    throw new CimesException(RuleMessage.Error.C00051(lotDefectData.ReasonCode));

                //格式:(批號) 原因碼_說明_備註 
                string defectText = "(" + lotDefectData.Lot + ")";

                defectText += " " + lotDefectData.ReasonCode;

                if (!reason.Description.IsNullOrTrimEmpty())
                    defectText += "_" + reason.Description;

                if (!lotDefectData.Description.IsNullOrTrimEmpty())
                    defectText += "_" + lotDefectData.Description;

                ddlJudgeDefect.Items.Add(new ListItem(defectText, lotDefectData.ID));

                ddlJudgeDefect_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                //查無送待判原因！
                throw new Exception(RuleMessage.Error.C10187());
            }

            return true;
        }

        /// <summary>
        /// 確認此批號是否為MATERIALLOT
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        private bool CheckMaterialLot(string lot)
        {
            //確認此批號是否為MATERIALLOT
            var materialLotList = LotDefectInfoEx.GetDataListByMaterialLot(lot);
            if (materialLotList.Count == 0)
            {
                return false;
            }

            foreach (var data in materialLotList)
            {
                //取得批號資訊
                var lotData = LotInfoEx.GetLotByLot(data.Lot).ChangeTo<LotInfoEx>();

                //驗證批號狀態，Wait的批號
                if (lotData.Status != LotDefaultStatus.Wait.ToString())
                {
                    continue;
                }

                //驗證currentRule
                if (lotData.CurrentRuleName != _ProgramInformationBlock.ProgramRight)
                {
                    continue;
                }

                var reason = ReasonInfo.GetReasonByName(data.ReasonCode);
                if (reason == null)
                    throw new CimesException(RuleMessage.Error.C00051(data.ReasonCode));

                //格式:(批號) 原因碼_說明_備註 
                string defectText = "(" + data.Lot + ")";

                defectText += " " + data.ReasonCode;

                if (!reason.Description.IsNullOrTrimEmpty())
                    defectText += "_" + reason.Description;

                if (!data.Description.IsNullOrTrimEmpty())
                    defectText += "_" + data.Description;

                ddlJudgeDefect.Items.Add(new ListItem(defectText, data.ID));
            }

            if (ddlJudgeDefect.Items.Count == 0)
            {
                //查無送待判原因！
                throw new Exception(RuleMessage.Error.C10187());
            }
            else if (ddlJudgeDefect.Items.Count == 1)
            {
                ddlJudgeDefect_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                ddlJudgeDefect.Items.Insert(0, new ListItem());
            }

            return true;
        }

        /// <summary>
        /// 確認此批號是否為INVLOT
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        private bool CheckInvLot(string lot)
        {
            //確認此批號是否為INVLOT
            var invLotList = LotDefectInfoEx.GetDataListByInvLot(lot);
            if (invLotList.Count == 0)
            {
                return false;
            }

            foreach (var data in invLotList)
            {
                //取得批號資訊
                var lotData = LotInfoEx.GetLotByLot(data.Lot).ChangeTo<LotInfoEx>();

                //驗證批號狀態，Wait的批號
                if (lotData.Status != LotDefaultStatus.Wait.ToString())
                {
                    continue;
                }

                //驗證currentRule
                if (lotData.CurrentRuleName != _ProgramInformationBlock.ProgramRight)
                {
                    continue;
                }

                var reason = ReasonInfo.GetReasonByName(data.ReasonCode);
                if (reason == null)
                    throw new CimesException(RuleMessage.Error.C00051(data.ReasonCode));

                //格式:(批號) 原因碼_說明_備註 
                string defectText = "(" + data.Lot + ")";

                defectText += " " + data.ReasonCode;

                if (!reason.Description.IsNullOrTrimEmpty())
                    defectText += "_" + reason.Description;

                if (!data.Description.IsNullOrTrimEmpty())
                    defectText += "_" + data.Description;

                ddlJudgeDefect.Items.Add(new ListItem(defectText, data.ID));
            }

            if (ddlJudgeDefect.Items.Count == 0)
            {
                //查無送待判原因！
                throw new Exception(RuleMessage.Error.C10187());
            }
            else if (ddlJudgeDefect.Items.Count == 1)
            {
                ddlJudgeDefect_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                ddlJudgeDefect.Items.Insert(0, new ListItem());
            }

            return true;
        }

        /// <summary>
        /// 確認此批號是否為WOLOT
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        private bool CheckWOLot(string lot)
        {
            //確認此批號是否為WOLOT
            var woLotList = LotDefectInfoEx.GetDataListByWOLot(lot);
            if (woLotList.Count == 0)
            {
                return false;
            }

            foreach (var data in woLotList)
            {
                //取得批號資訊
                var lotData = LotInfoEx.GetLotByLot(data.Lot).ChangeTo<LotInfoEx>();

                //驗證批號狀態，Wait的批號
                if (lotData.Status != LotDefaultStatus.Wait.ToString())
                {
                    continue;
                }

                //驗證currentRule
                if (lotData.CurrentRuleName != _ProgramInformationBlock.ProgramRight)
                {
                    continue;
                }

                var reason = ReasonInfo.GetReasonByName(data.ReasonCode);
                if (reason == null)
                    throw new CimesException(RuleMessage.Error.C00051(data.ReasonCode));

                //格式:(批號) 原因碼_說明_備註 
                string defectText = "(" + data.Lot + ")";

                defectText += " " + data.ReasonCode;

                if (!reason.Description.IsNullOrTrimEmpty())
                    defectText += "_" + reason.Description;

                if (!data.Description.IsNullOrTrimEmpty())
                    defectText += "_" + data.Description;

                ddlJudgeDefect.Items.Add(new ListItem(defectText, data.ID));

            }

            if (ddlJudgeDefect.Items.Count == 0)
            {
                //查無送待判原因！
                throw new Exception(RuleMessage.Error.C10187());
            }
            else if (ddlJudgeDefect.Items.Count == 1)
            {
                ddlJudgeDefect_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                ddlJudgeDefect.Items.Insert(0, new ListItem());
            }

            return true;
        }

        /// <summary>
        /// 切換不同的送待判原因
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlJudgeDefect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //確認是否有選擇送待判原因
                ddlJudgeDefect.Must(lblJudgeDefect);

                //取得批號資訊
                _LotDefectData = InfoCenter.GetBySID<LotDefectInfo>(ddlJudgeDefect.SelectedValue);
                _LotData = LotInfoEx.GetLotByLot(_LotDefectData.Lot).ChangeTo<LotInfoEx>();

                #region 判定原因碼(DropDownList)：依照原因碼工作站設定，帶出原因碼
                ddlJudgeReason.Items.Clear();

                List<BusinessReason> reasonList = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(_LotData.CurrentRuleName, _LotData.OperationName, "Default", ReasonMode.Category);

                if (reasonList.Count > 0)
                {
                    ddlJudgeReason.DataSource = reasonList;
                    ddlJudgeReason.DataTextField = "ReasonDescription";
                    ddlJudgeReason.DataValueField = "ReasonCategorySID";
                    ddlJudgeReason.DataBind();

                    if (ddlJudgeReason.Items.Count != 1)
                        ddlJudgeReason.Items.Insert(0, "");
                    else
                    {
                        ddlJudgeReason.SelectedIndex = 0;
                    }
                }
                else
                {
                    //[00641]規則:{0} 工作站:{1} 使用的原因碼未設定，請洽IT人員!
                    throw new Exception(TextMessage.Error.T00641(ProgramRight, _LotData.OperationName));
                }
                #endregion

                //數量
                ttbQty.Text = _LotData.Quantity.ToString();

                //取得工件序號
                var componentDataList = ComponentInfoEx.GetDataByCurrentLot(_LotData.Lot);
                if (componentDataList.Count > 0)
                {
                    ttbItemSN.Text = componentDataList[0].ComponentID;
                }

                ttbJudgeDescr.Text = "";
                btnOK.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                ttbItemSN.Text = "";
                ttbQty.Text = "";
                ttbJudgeDescr.Text = "";

                ddlJudgeReason.Items.Clear();

                btnOK.Enabled = false;
                btnPrint.Enabled = false;

                _LotData = null;
                _LotDefectData = null;

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
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                //確認是否有選擇送待判原因
                ddlJudgeDefect.Must(lblJudgeDefect);

                #region 檢查判定結果是否有選擇
                if (rdbGoods.Checked == false && rdbRepair.Checked == false && rdbDefectInv.Checked == false && rdbScrapInv.Checked == false)
                {
                    throw new Exception(TextMessage.Error.T00841(lblJudgeResult.Text));
                }
                #endregion

                //檢查判定原因是否有選擇
                ddlJudgeReason.Must(lblJudgeReason);

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 依照選擇的判定結果，對批號進行不同的處置

                    //選取的判定結果
                    string result = "";

                    //選取的原因碼
                    var reasonCategory = InfoCenter.GetBySID<ReasonCategoryInfo>(ddlJudgeReason.SelectedValue);

                    #region 良品：依照批號的UDC02(工作站序號)+UDC03(工作站名稱)找出預設流程的下一站點，將批號跳站至該站
                    if (rdbGoods.Checked)
                    {
                        result = "Good";

                        //取得流程線上版本 取得目前Lot的所有流程(所有工作站)
                        RouteVersionInfo RouteVersion = RouteVersionInfo.GetRouteActiveVersion(_LotData.RouteName);

                        //取得設定包裝工作站名稱
                        var packingOperation = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIPackingOperation");
                        if (packingOperation.Count == 0)
                        {
                            //T00555:查無資料，請至系統資料維護新增類別{0}、項目{1}!
                            throw new CimesException(TextMessage.Error.T00555("SAIPackingOperation", ""));
                        }

                        var reassignOperation = packingOperation[0];

                        //如果送待判站之前記錄的是包裝站，則直接跳至包裝站，反之，跳至下一站
                        if (reassignOperation.Remark01 == _LotData.UserDefineColumn03)
                        {
                            //取得包裝站名稱
                            string qcOperationName = reassignOperation.Remark01;

                            //取得包裝工作站資訊
                            var operation = OperationInfo.GetOperationByName(qcOperationName);
                            if (operation == null)
                            {
                                //T00171, 工作站:{0}不存在!!
                                throw new CimesException(TextMessage.Error.T00171(qcOperationName));
                            }
                            //根據指定的流程名稱、流程版本、工作站名稱, 找出第一個符合的流程工作站，新的站點包裝
                            var newOperation = RouteOperationInfo.GetRouteOperationByOperationName(_LotData.RouteName, _LotData.RouteVersion, qcOperationName);

                            //將批號的UDC02清空
                            WIPTransaction.ModifyLotSystemAttribute(_LotData, "USERDEFINECOL02", "", txnStamp);

                            //將批號的UDC03清空
                            WIPTransaction.ModifyLotSystemAttribute(_LotData, "USERDEFINECOL03", "", txnStamp);

                            //變更至指定工作站
                            WIPTransaction.ReassignOperation(_LotData, newOperation, reasonCategory, ttbJudgeDescr.Text, txnStamp);
                        }
                        else
                        {
                            //以此工作站名稱去查詢在流程中的序號
                            var routeOperation = RouteOperationInfo.GetRouteAllOperations(RouteVersion).Find(p => p.OperationName == _LotData.UserDefineColumn03);

                            var lastOperationSeq = string.Format("{0:000}", (Convert.ToDecimal(routeOperation.OperationSequence) + 1));

                            //下一個工作站 用LOT和流程中下一站的序號去查出下一個工作站資訊
                            var NextRouteOperation = RouteOperationInfo.GetRouteOperationByLotSequence(_LotData, lastOperationSeq);

                            if (NextRouteOperation == null)
                            {
                                //批號：{0}已無下個工作站點，請確認[流程設定]
                                throw new Exception(RuleMessage.Error.C10008(_LotData.Lot));
                            }

                            //將批號的UDC02清空
                            WIPTransaction.ModifyLotSystemAttribute(_LotData, "USERDEFINECOL02", "", txnStamp);

                            //將批號的UDC03清空
                            WIPTransaction.ModifyLotSystemAttribute(_LotData, "USERDEFINECOL03", "", txnStamp);

                            WIPTransaction.ReassignOperation(_LotData, NextRouteOperation, reasonCategory, ttbJudgeDescr.Text, txnStamp);
                        }
                    }
                    #endregion

                    #region 維修：紀錄維修及將批號派送至下一規則
                    if (rdbRepair.Checked)
                    {
                        result = "Repair";

                        List<WIPRepairInfo> repairDatas = new List<WIPRepairInfo>();

                        var componentData = ComponentInfoEx.GetDataByCurrentLot(_LotData.Lot)[0];
                        var repairData = WIPRepairInfo.CreateInfo(_LotData, componentData, componentData.ComponentQuantity, reasonCategory, _LotData.OperationName, _LotData.ResourceName, string.Empty, string.Empty);
                        repairDatas.Add(repairData);

                        var issueOperation = OperationInfo.GetOperationByName(_LotData.OperationName);

                        WIPTransaction.RepairAdd(_LotData, repairDatas, issueOperation, txnStamp);
                        WIPTransaction.DispatchLot(_LotData, txnStamp);
                    }
                    #endregion

                    #region 不良品入庫：將批號狀態變更為DefectInv
                    if (rdbDefectInv.Checked)
                    {
                        result = "DefectInv";
                        WIPTransaction.ModifyLotSystemAttribute(_LotData, "STATUS", "DefectInv", txnStamp);
                    }
                    #endregion

                    #region 報廢品入庫：將批號狀態變更為ScrapInv
                    if (rdbScrapInv.Checked)
                    {
                        result = "ScrapInv";
                        WIPTransaction.ModifyLotSystemAttribute(_LotData, "STATUS", "ScrapInv", txnStamp);
                    }
                    #endregion

                    #endregion

                    #region 將判定結果紀錄在CST_WIP_DEFECT_JUDGEMENT，Result：良品(Goods)、維修(Repair)、不良品入庫(DefectInv)、報廢入庫(ScrapInv)

                    var insertData = InfoCenter.Create<CSTWIPDefectJudgementInfo>();

                    insertData.WIPDefectSID = _LotDefectData.DefectSID;
                    insertData["REASONCATEGORY"] = reasonCategory.Category;
                    insertData["LOT"] = _LotData.Lot;
                    insertData.Reason = reasonCategory.Reason;
                    insertData.Result = result;
                    insertData.Description = ttbJudgeDescr.Text;
                    insertData.LinkSID = WIPHistoryInfo.GetLotFirstHistory(_LotData.Lot).LinkSID;

                    insertData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    #endregion

                    cts.Complete();
                }

                ttbLot.Text = "";

                ClearField();
                AjaxFocus(ttbLot);

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
                    //[00060]{0}沒有資料可顯示！
                    throw new Exception(TextMessage.Error.T00060(""));
                }
                DataSet dsReport = new DataSet();

                // 取得Report資料
                List<LotInfoEx> lotDataList = new List<LotInfoEx>();
                lotDataList.Add(_LotData);
                DataView dvRepot = GetRunCardDataSource(lotDataList);
                dsReport.Tables.Add(dvRepot.Table.Copy());

                dsReport.Tables.Add(GetLotTitle());
                dsReport.AcceptChanges();

                if (dsReport.Tables.Count > 0)
                {
                    string sPrintProgram = "/CustomizeRule/WIPRule/W006View.aspx";
                    string sHost = Request.Url.Host;
                    string sApplicationPath = Request.ApplicationPath;
                    string ReportPath = "http://" + sHost + sApplicationPath + sPrintProgram;
                    Session["W006View"] = dsReport;
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

        /// <summary>
        /// 取的runcard的資料來源
        /// </summary>
        /// <param name="LotDataList"></param>
        /// <returns></returns>
        private DataView GetRunCardDataSource(List<LotInfoEx> LotDataList)
        {
            string sql = "";

            DataTable dtReportData = new DataTable();
            dtReportData.Columns.Add("OPERSEQ");
            dtReportData.Columns.Add("OPERNO");
            dtReportData.Columns.Add("Operation");
            dtReportData.Columns.Add("OperationDescr");
            dtReportData.Columns.Add("RECIPEID");
            dtReportData.Columns.Add("EDC");
            dtReportData.Columns.Add("WorkCenter");//工作中心

            LotDataList.ForEach(p =>
            {
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

                #region RecipeOperationReleaseInfo
                sql = @"SELECT OPERRELS.*,PARA.PARAMETER,PARA.TARGET,PARA.UPSPEC,PARA.LOWSPEC
                      FROM MES_RMS_OPER_RELEASE OPERRELS, MES_RMS_RECIPE RECIPE, MES_RMS_RECIPE_VER VER, MES_RMS_RECIPE_PARA PARA
                     WHERE OPERRELS.RMS_RECIPE_SID = RECIPE.RMS_RECIPE_SID
                       AND RECIPE.RMS_RECIPE_SID = VER.RMS_RECIPE_SID
                       AND VER.REVSTATE = 'ACTIVE' 
                       AND VER.RMS_RECIPE_VER_SID = PARA.RMS_RECIPE_VER_SID
                       AND OPERRELS.DEVICE = #[STRING]
                       AND OPERRELS.ENABLETIME <= #[STRING]
                       AND OPERRELS.DISABLETIME >=#[STRING]
                     ORDER BY OPERRELS.RECIPEID";

                List<RecipeOperationReleaseInfo> recipeRelease = InfoCenter.GetList<RecipeOperationReleaseInfo>(sql, p.DeviceName, p.RMSTime, p.RMSTime);

                if (recipeRelease.Count == 0)
                {
                    recipeRelease = InfoCenter.GetList<RecipeOperationReleaseInfo>(sql, "ALL", p.RMSTime, p.RMSTime);
                }
                #endregion

                #region EDCOperationParameterReleaseInfo
                sql = @"SELECT R.OPERATION, P.* FROM MES_EDC_OPER_RLS R
                     INNER JOIN MES_EDC_OPER_RLS_PARA P ON R.EDC_OPER_SET_SID = P.EDC_OPER_SET_SID
                     WHERE DEVICE = #[STRING]
                       AND P.ENABLETIME <= #[STRING]
                       AND P.DISABLETIME >= #[STRING]";

                List<EDCOperationParameterReleaseInfo> edcPara = InfoCenter.GetList<EDCOperationParameterReleaseInfo>(sql, p.DeviceName, p.EDCTIME, p.EDCTIME);

                if (edcPara.Count == 0)
                {
                    edcPara = InfoCenter.GetList<EDCOperationParameterReleaseInfo>(sql, "ALL", p.EDCTIME, p.EDCTIME);
                }
                #endregion

                #region 工作站
                routeOpers.ForEach(oper =>
                {
                    DataRow dr = dtReportData.NewRow();
                    dr["OPERSEQ"] = oper.OperationSequence;
                    dr["Operation"] = oper.OperationName;
                    dr["OPERNO"] = oper["OPERATIONNO"].ToString();
                    dr["OperationDescr"] = oper["DESCR"].ToString();

                    var recipes = recipeRelease.FindAll(q => q.OperationName.Equals(oper.OperationName));

                    string recipeID = "";
                    string edcList = "";

                    recipes.ForEach(recipe =>
                    {
                        string target = recipe["TARGET"].ToString();
                        string upSpec = recipe["UPSPEC"].ToString();
                        string lowSpec = recipe["LOWSPEC"].ToString();
                        string parameter = recipe["PARAMETER"].ToString();

                        recipeID += parameter;

                        if (target.IsNullOrTrimEmpty() == false)
                            recipeID += " : " + target;

                        if (upSpec.IsNullOrTrimEmpty() == false || lowSpec.IsNullOrTrimEmpty() == false)
                        {
                            recipeID += " [ " + lowSpec + "~" + upSpec + " ]";
                        }


                        recipeID += "\r";
                    });

                    var edc = edcPara.FindAll(q => q["OPERATION"].ToString().Equals(oper.OperationName));

                    edc.ForEach(parameter =>
                    {
                        edcList += parameter.ParameterName;

                        if (parameter.Target.IsNullOrTrimEmpty() == false)
                            edcList += "，Target=" + parameter.Target;

                        if (parameter.DataType.ToUpper().Equals("NUMBER"))
                        {
                            edcList += "[" + parameter.LowControl + "~" + parameter.UpControl + "]";
                        }

                        edcList += "\r";
                    });

                    if (recipeID.IsNullOrTrimEmpty() == false)
                        recipeID = recipeID.Substring(0, recipeID.LastIndexOf("\r"));

                    if (edcList.IsNullOrTrimEmpty() == false)
                        edcList = edcList.Substring(0, edcList.LastIndexOf("\r"));

                    dr["RECIPEID"] = recipeID;
                    dr["EDC"] = edcList;

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

            dt.Rows[0]["DeviceCode"] = _LotData.DeviceName.Substring(1, 2);
            var lsLotType = WpcExClassItemInfo.GetExClassItemInfo("LotType", _LotData.LotType);
            dt.Rows[0]["LotTypeDescr"] = lsLotType.Count > 0 ? lsLotType[0].Remark02 : "";

            var WOData = WorkOrderInfo.GetWorkOrderByWorkOrder(_LotData.WorkOrder);
            dt.Rows[0]["WOQuantity"] = WOData.Quantity;
            dt.Rows[0]["ProductionDate"] = WOData["ProductionDate"].ToCimesString();

            var DeviceData = DeviceVersionInfoEx.GetActiveDeviceVersion(_LotData.DeviceName).ChangeTo<DeviceVersionInfoEx>();
            dt.Rows[0]["DeviceDescr"] = DeviceData.Description;
            dt.Rows[0]["DeviceBPNo"] = "0 無欄位";
            dt.Rows[0]["DeviceBPRev"] = "0 無欄位";

            return dt;
        }
    }
}