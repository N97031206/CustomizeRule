/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：品管判定自主檢結果。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2018/01/05      kmchen      初版
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

namespace CustomizeRule.QCRule
{
    public partial class W041 : CustomizeRuleBasePage
    {
        private class QCData
        {
            /// <summary>
            /// QC檢驗明細SID
            /// </summary>
            public string ID { get; set; }

            /// <summary>
            /// 機台名稱
            /// </summary>
            public string EquipmentName { get; set; }

            /// <summary>
            /// 是否需要檢驗
            /// </summary>
            public string PassFlag { get; set; }

            /// <summary>
            /// 同次送驗繫結的SID
            /// </summary>
            public string BatchID { get; set; }

            /// <summary>
            /// QC檢驗單SID
            /// </summary>
            public string QCInspectionSID { get; set; }

            /// <summary>
            /// 物件系統編號
            /// </summary>
            public string ObjectSID { get; set; }

            /// <summary>
            /// 序號
            /// </summary>
            public string ComponentLot { get; set; }

            /// <summary>
            /// 小工單號
            /// </summary>
            public string WorkOderLot { get; set; }

            /// <summary>
            /// 鍛造批號
            /// </summary>
            public string MaterialLot { get; set; }

            /// <summary>
            /// 料號
            /// </summary>
            public string DeviceName { get; set; }
        }

        /// <summary>
        /// 中心孔量測的上下限設定
        /// </summary>
        private WpcExClassItemInfo _SAICenterHole
        {
            get { return (WpcExClassItemInfo)this["SAICenterHole"]; }
            set { this["SAICenterHole"] = value; }
        }

        /// <summary>
        /// 檢驗清單所有資料
        /// </summary>
        private List<QCData> _QCDataList
        {
            get { return this["_QCData"] as List<QCData>; }
            set { this["_QCData"] = value; }
        }

        /// <summary>
        /// 所選擇的檢驗資料
        /// </summary>
        private QCData _SelectedQCData
        {
            get { return this["_SelectedQCData"] as QCData; }
            set { this["_SelectedQCData"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            ddlEquip.Items.Clear();
            ddlPQCReasonCode.Items.Clear();
            ddlSN.Items.Clear();

            ttbLot.Text = "";
            ttbMaterialLot.Text = "";
            ttbDescr.Text = "";
            ttbWorkOrderLot.Text = "";

            btnOK.Enabled = false;

            rdbOK.Checked = true;
            rdbNG.Checked = false;

            rdbOK.Enabled = true;
            rdbNG.Enabled = false;

            _QCDataList = new List<QCData>();
            _SelectedQCData = null;
            _SAICenterHole = null;

            gvComponentEDC.DataSource = null;
            gvComponentEDC.DataBind();
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
                    AjaxFocus(ttbLot);

                    gvComponentEDC.Initialize();
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
        /// 輸入生產編號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //確認是否有輸入資料
                ttbLot.Must(lblLot);

                #region 取得檢驗資料(查詢機台、序號、機加批號及鍛造批號欄位是否有符合介面輸入的資料，資料狀態必須為MPQC且判定結果為NULL)
                string sql = @"SELECT A.*, B.EQUIPMENT, B.PASSFLAG, B.BATCHID, B.DEVICE
                                  FROM MES_QC_INSP_OBJ A
                                       LEFT JOIN MES_QC_INSP B ON A.QC_INSP_SID = B.QC_INSP_SID
                                 WHERE (ITEM1 = #[STRING] OR ITEM2 = #[STRING] OR ITEM3 = #[STRING] OR EQUIPMENT = #[STRING]) 
                                    AND STATUS = 'MPQC'
                                    AND QCTYPE = 'MPQC' 
                                    AND RESULT IS NULL";

                var lot = CustomizeFunction.ConvertDMCCode(ttbLot.Text.Trim());

                var table = DBCenter.GetDataTable(sql, lot, lot, lot, lot);

                if (table.Rows.Count == 0)
                {
                    //生產編號:{0} 查無檢驗資料!
                    throw new Exception(RuleMessage.Error.C10062(lot));
                }

                //清除檢驗清單資料
                _QCDataList.Clear();

                foreach (DataRow dr in table.Rows)
                {
                    _QCDataList.Add(new QCData()
                    {
                        ID = dr["QC_INSP_OBJ_SID"].ToString(),
                        EquipmentName = dr["EQUIPMENT"].ToString(),
                        BatchID = dr["BATCHID"].ToString(),
                        ComponentLot = dr["ITEM1"].ToString(),
                        WorkOderLot = dr["ITEM2"].ToString(),
                        MaterialLot = dr["ITEM3"].ToString(),
                        ObjectSID = dr["OBJECTSID"].ToString(),
                        PassFlag = dr["PASSFLAG"].ToString(),
                        QCInspectionSID = dr["QC_INSP_SID"].ToString(),
                        DeviceName = dr["DEVICE"].ToString()
                    });
                }
                #endregion

                #region 清除介面資料

                ddlPQCReasonCode.Items.Clear();
                ddlSN.Items.Clear();
                ddlEquip.Items.Clear();

                ttbMaterialLot.Text = "";
                ttbDescr.Text = "";
                ttbWorkOrderLot.Text = "";

                btnOK.Enabled = false;

                rdbOK.Checked = true;
                rdbNG.Checked = false;

                rdbOK.Enabled = true;
                rdbNG.Enabled = false;
                #endregion

                #region 設置機台資料

                //取得所有檢驗資料之不重複機台名稱
                foreach (var QCData in _QCDataList)
                {
                    ListItem newItem = new ListItem(QCData.EquipmentName, QCData.EquipmentName);

                    if (ddlEquip.Items.Contains(newItem) == false)
                    {
                        ddlEquip.Items.Add(newItem);
                    }
                }

                if (ddlEquip.Items.Count == 0)
                {
                    //生產編號:{0} 沒有機台可以選擇!
                    throw new Exception(RuleMessage.Error.C10063(lot));
                }

                if (ddlEquip.Items.Count != 1)
                    ddlEquip.Items.Insert(0, "");
                else
                {
                    ddlEquip.SelectedIndex = 0;
                    ddlEquip_SelectedIndexChanged(null, EventArgs.Empty);
                }
                #endregion

            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbLot);
                HandleError(ex);
            }
        }

        /// <summary>
        /// 切換機台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEquip_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlEquip.Must(lblEquip);

                if (ddlEquip.SelectedIndex != -1)
                {
                    //清除序號資料
                    ddlSN.Items.Clear();

                    for (int i = 0; i < _QCDataList.Count; i++)
                    {
                        string equipmentName = _QCDataList[i].EquipmentName;

                        //比對符合選擇機台編號的資料
                        if (equipmentName == ddlEquip.SelectedValue)
                        {
                            var componentLot = _QCDataList[i].ComponentLot;
                            var ID = _QCDataList[i].ID;

                            ListItem newItem = new ListItem(componentLot, ID);
                            ddlSN.Items.Add(newItem);
                        }
                    }

                    if (ddlSN.Items.Count == 0)
                    {
                        //生產編號:{0} 沒有序號可以選擇!
                        throw new Exception(RuleMessage.Error.C10064(ttbLot.Text));
                    }

                    if (ddlSN.Items.Count != 1)
                        ddlSN.Items.Insert(0, "");
                    else
                    {
                        ddlSN.SelectedIndex = 0;
                        ddlSN_SelectedIndexChanged(null, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);

                ddlPQCReasonCode.Items.Clear();
                ddlSN.Items.Clear();

                ttbMaterialLot.Text = "";
                ttbDescr.Text = "";
                ttbWorkOrderLot.Text = "";

                btnOK.Enabled = false;

                rdbOK.Checked = true;
                rdbNG.Checked = false;

                rdbOK.Enabled = true;
                rdbNG.Enabled = false;
            }
        }

        /// <summary>
        /// 切換序號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSN.Must(lblSN);

                if (ddlSN.SelectedIndex != -1)
                {
                    //清除原因碼資料
                    ddlPQCReasonCode.Items.Clear();

                    ttbMaterialLot.Text = "";
                    ttbWorkOrderLot.Text = "";
                    ttbDescr.Text = "";

                    rdbNG.Enabled = false;
                    rdbOK.Enabled = false;

                    int index = -1;

                    for (int i = 0; i < _QCDataList.Count; i++)
                    {
                        string ID = _QCDataList[i].ID;

                        //比對符合選擇機台編號的資料
                        if (ID == ddlSN.SelectedValue)
                        {
                            index = i;
                            break;
                        }
                    }

                    _SelectedQCData = _QCDataList[index];

                    ttbMaterialLot.Text = _SelectedQCData.MaterialLot;
                    ttbWorkOrderLot.Text = _SelectedQCData.WorkOderLot;

                    if (_SelectedQCData.PassFlag == "N")
                    {
                        // 取得中心孔量測設定值
                        var lstSAICenterHolde = WpcExClassItemInfo.GetExClassItemInfo("SAICenterHole", _SelectedQCData.DeviceName);
                        if (lstSAICenterHolde.Count == 0)
                        {
                            lstSAICenterHolde = WpcExClassItemInfo.GetExClassItemInfo("SAICenterHole", "ALL");
                        }
                        // 若找不到中心孔量測需拋錯
                        if (lstSAICenterHolde.Count == 0)
                        {
                            throw new Exception(TextMessage.Error.T00555("SAICenterHole", _SelectedQCData.DeviceName + "," + "ALL"));
                        }

                        _SAICenterHole = lstSAICenterHolde[0];
                        // 設定中心孔量測的DataTable資料
                        DataTable dtEmpty = new DataTable();
                        dtEmpty.Columns.Add("ITEM", typeof(int));
                        dtEmpty.Columns.Add("EDC", typeof(String));

                        for (int i = 0; i < _SAICenterHole.Remark04.ToDecimal(0); i++)
                        {
                            DataRow dr = dtEmpty.NewRow();
                            dr["ITEM"] = i + 1;
                            dtEmpty.Rows.Add(dr);
                        }
                        // 將產生的資料表顯示在畫面上
                        gvComponentEDC.DataSource = dtEmpty;
                        gvComponentEDC.DataBind();
                    }
                    
                    //取得Lot資料
                    var lotData = InfoCenter.GetBySID<LotInfo>(_SelectedQCData.ObjectSID);

                    rdbNG.Enabled = true;
                    rdbOK.Enabled = true;

                    #region 設置原因碼

                    List<BusinessReason> reason = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(ProgramRight, "ALL", "Default", ReasonMode.Category);
                    if (reason.Count > 0)
                    {
                        ddlPQCReasonCode.DataSource = reason;
                        ddlPQCReasonCode.DataTextField = "ReasonDescription";
                        ddlPQCReasonCode.DataValueField = "ReasonCategorySID";
                        ddlPQCReasonCode.DataBind();

                        if (ddlPQCReasonCode.Items.Count != 1)
                            ddlPQCReasonCode.Items.Insert(0, "");
                        else
                        {
                            ddlPQCReasonCode.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        //[00641]規則:{0} 工作站:{1} 使用的原因碼未設定，請洽IT人員!
                        throw new Exception(TextMessage.Error.T00641(ProgramRight, "ALL"));
                    }
                    #endregion

                    btnOK.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                HandleError(ex);

                ddlPQCReasonCode.Items.Clear();

                ttbMaterialLot.Text = "";
                ttbDescr.Text = "";
                ttbWorkOrderLot.Text = "";

                btnOK.Enabled = false;

                rdbOK.Checked = true;
                rdbNG.Checked = false;

                rdbOK.Enabled = true;
                rdbNG.Enabled = false;

                gvComponentEDC.DataSource = null;
                gvComponentEDC.DataBind();
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
                #region 確認選擇結果，如果是選擇NG，確認是否有選擇原因碼
                string result = "";
                if (rdbOK.Checked)
                {
                    result = "OK";
                }
                else if (rdbNG.Checked)
                {
                    result = "NG";

                    //確認是否有選擇原因碼
                    ddlPQCReasonCode.Must(lblPQCReasonCode);
                }
                #endregion  

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 更新檢驗主檔[MES_QC_INSP]
                    //取得檢驗主檔資料
                    var QCInsepctionData = InfoCenter.GetBySID<QCInspectionInfo>(_SelectedQCData.QCInspectionSID).ChangeTo<QCInspectionInfoEx>();

                    //有選擇原因碼才更新[NG_Category]及[NG_Reason]
                    if (string.IsNullOrEmpty(ddlPQCReasonCode.SelectedValue) == false)
                    {
                        var reason = InfoCenter.GetBySID<ReasonCategoryInfo>(ddlPQCReasonCode.SelectedValue);
                        QCInsepctionData.NG_Category = reason.Category;
                        QCInsepctionData.NG_Reason = reason.Reason;
                    }

                    QCInsepctionData.NG_Description = ttbDescr.Text;
                    QCInsepctionData.Result = result;
                    QCInsepctionData.Status = "Finished";

                    QCInsepctionData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

                    #endregion

                    #region 更新自主檢數據內容[CST_EDC_COMP]
                    bool inSpec = true;

                    gvComponentEDC.Rows.LoopDo<GridViewRow>((p, i) => {
                        // 取得TextBox Control
                        var ttbEDC = p.FindControl("ttbEDC") as TextBox;
                        if (ttbEDC.Text.IsNullOrEmpty())
                        {
                            AjaxFocus(ttbEDC);
                            throw new Exception(TextMessage.Error.T00043(GetUIResource("CenterHoleData")));
                        }
                        // 是否符合規格判斷
                        var measureVal = ttbEDC.Text.ToDecimal();
                        if (measureVal > _SAICenterHole.Remark02.ToDecimal())
                        {
                            inSpec = false;
                        }

                        if (measureVal < _SAICenterHole.Remark03.ToDecimal())
                        {
                            inSpec = false;
                        }

                        var lotData = ComponentInfo.GetComponentByComponentID(_SelectedQCData.ComponentLot);

                        // 將量測資料記錄到客製表
                        var edcCompInfo = InfoCenter.Create<CSTEDCComponentInfo>();
                        edcCompInfo.ComponentID = _SelectedQCData.ComponentLot;
                        edcCompInfo.Data = measureVal;
                        edcCompInfo.UpSpecification = _SAICenterHole.Remark02.ToDecimal();
                        edcCompInfo.LowSpecification = _SAICenterHole.Remark03.ToDecimal();
                        edcCompInfo.INSPEC = inSpec == true ? "OK" : "NG";
                        edcCompInfo.Lot = (lotData == null) ? "" : lotData.CurrentLot;
                        edcCompInfo["LINKSID"] = txnStamp.LinkSID;
                        edcCompInfo["PARAMETER"] = "SC" + (i + 1).ToString();
                        edcCompInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    });
                    #endregion

                    #region 更新機台狀態及更新檢驗單對應的批號資料

                    //如果選擇結果為NG時，則更新機台狀態及更新檢驗單對應的批號資料
                    if (rdbNG.Checked)
                    {
                        //取得機台狀態資料
                        var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("DOWN");

                        var equipData = EquipmentInfo.GetEquipmentByName(_SelectedQCData.EquipmentName);

                        //如果機台狀態為IDLE，則變更狀態為DOWN
                        if (equipData.CurrentState == "IDLE")
                        {
                            //更新機台狀態
                            EMSTransaction.ChangeState(equipData, newStateInfo, txnStamp);
                        }
                        else
                        {
                            //如果機台狀態不為IDLE，則註記機台[USERDEFINECOL01]為Y
                            EMSTransaction.ModifyEquipmentSystemAttribute(equipData, "USERDEFINECOL01", "Y", txnStamp);
                        }

                        //上一次的檢驗單號資料
                        QCInspectionInfoEx previousInspectionData = null;

                        //依據機台及料號查詢，然後用建立時間逆排序，找出所有FAI及MPQC的檢驗單號資料
                        var QCDataList = QCInspectionInfoEx.GetInspDataListByEquipmentAndDevice(QCInsepctionData.EquipmentName, QCInsepctionData.DeviceName);

                        //如果筆數大於1，則目前檢驗單號的索引值
                        if (QCDataList.Count > 1)
                        {
                            //找出目前檢驗單號的索引值
                            var NGIndex = QCDataList.FindIndex(p => p.InspectionNo == QCInsepctionData.InspectionNo);

                            //如果找到的索引值不是最後一筆的話，則找出上一次的檢驗單號資料
                            if (NGIndex != (QCDataList.Count - 1))
                            {
                                //找出上一次的檢驗單號資料
                                previousInspectionData = QCDataList[NGIndex + 1];
                            }
                        }

                        //取得目前檢驗單號的批號子單元資料
                        var componentList = ComponentInfoEx.GetDataByInspectionNo(QCInsepctionData.InspectionNo);
                        componentList.ForEach(component =>
                        {
                            //取得批號資料
                            var lotData = LotInfo.GetLotByLot(component.CurrentLot);

                            //更新欄位[PQCNGFLAG]
                            WIPTransaction.ModifyLotComponentSystemAttribute(lotData, component, "PQCNGFLAG", "Y", txnStamp);

                            //更新欄位[PQCNGNO]
                            WIPTransaction.ModifyLotComponentSystemAttribute(lotData, component, "PQCNGNO", QCInsepctionData.InspectionNo, txnStamp);
                        });

                        if (previousInspectionData != null)
                        {
                            //取得上一次檢驗單號的批號子單元資料
                            var previousComponentList = ComponentInfoEx.GetDataByInspectionNo(previousInspectionData.InspectionNo);

                            //取得不需要註記的ComponentID
                            var passInspectionDataList = QCInspectionObjectInfo.GetInspectionObjects(previousInspectionData);

                            previousComponentList.ForEach(component =>
                            {
                                //確認是否為不需要註記的ComponentID
                                if (passInspectionDataList.FindIndex(p => p.ItemName1 == component.ComponentID) == -1)
                                {
                                    //取得批號資料
                                    var lotData = LotInfo.GetLotByLot(component.CurrentLot);

                                    //更新欄位[PQCNGFLAG]
                                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, component, "PQCNGFLAG", "Y", txnStamp);

                                    //更新欄位[PQCNGNO]
                                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, component, "PQCNGNO", QCInsepctionData.InspectionNo, txnStamp);
                                }
                            });
                        }
                    }
                    #endregion

                    cts.Complete();
                }

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

        protected void gvComponentEDC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //顯示量測規格於表頭
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[1].Text = GetUIResource("UPSPEC") + ":" + _SAICenterHole.Remark02 + "<br>"
                        + GetUIResource("LOWSPEC") + ":" + _SAICenterHole.Remark03 + "<br>";
                }

                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                DataRowView dr = e.Row.DataItem as DataRowView;
                // 顯示項次
                var lblItem = e.Row.FindControl("lblItem") as Label;
                lblItem.Text = dr["ITEM"].ToString();

                var ttbEDC = e.Row.FindControl("ttbEDC") as TextBox;
                ttbEDC.Attributes.Add("EDCFlag", "Y");
                if (e.Row.RowIndex == 0)
                {
                    AjaxFocus(ttbEDC);
                }

                // 註冊前端Javascript
                string validateString = "javascript:ValidateInputNumber(this,{0},{1});";
                validateString = string.Format(validateString, _SAICenterHole.Remark03, _SAICenterHole.Remark02);
                ttbEDC.Attributes.Add("onblur", validateString);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}