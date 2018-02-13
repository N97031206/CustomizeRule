/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：品管判定首件結果。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/08/21      keith      初版
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
    public partial class W030 : CustomizeRuleBasePage
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
            /// OP1機台名稱
            /// </summary>
            public string SecondEquipmentName { get; set; }

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
        /// 是否要顯示[結單]CLOSE選項旗標
        /// </summary>
        private const string _QCCloseFlag = "QCClose";

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

        /// <summary>
        /// 機台檢驗結果資料清單
        /// </summary>
        private List<CSTWIPCMMDataInfo> _CSTWIPCMMDataList
        {
            get { return this["_CSTWIPCMMDataList"] as List<CSTWIPCMMDataInfo>; }
            set { this["_CSTWIPCMMDataList"] = value; }
        }

        /// <summary>
        /// 機台檢驗結果資料主檔
        /// </summary>
        private List<CSTWIPCMMInfo> _CSTWIPCMMList
        {
            get { return this["_CSTWIPCMMList"] as List<CSTWIPCMMInfo>; }
            set { this["_CSTWIPCMMList"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            ddlEquip.Items.Clear();
            ddlFAIReasonCode.Items.Clear();
            ddlSN.Items.Clear();
            ddlFileName.Items.Clear();

            ttbLot.Text = "";
            ttbMaterialLot.Text = "";
            ttbDescr.Text = "";
            ttbWorkOrderLot.Text = "";

            btnOK.Enabled = false;

            rdbOK.Checked = true;
            rdbNG.Checked = false;
            rdbPASS.Checked = false;
            rdbCLOSE.Checked = false;

            rdbOK.Enabled = true;
            rdbNG.Enabled = false;
            rdbPASS.Enabled = false;

            _QCDataList = new List<QCData>();
            _SelectedQCData = null;

            _CSTWIPCMMList = new List<CSTWIPCMMInfo>();
            _CSTWIPCMMDataList = new List<CSTWIPCMMDataInfo>();
            gvInspectionData.SetDataSource(_CSTWIPCMMDataList, true);
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
                    //確認是否要顯示結單選項
                    rdbCLOSE.Visible = UserProfileInfo.CheckUserRight(User.Identity.Name, _QCCloseFlag);

                    //第一次開啟頁面
                    ClearField();
                    AjaxFocus(ttbLot);
                    gvInspectionData.Initialize();
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

                #region 取得檢驗資料(查詢機台、序號、機加批號及鍛造批號欄位是否有符合介面輸入的資料，資料狀態必須為FAI且判定結果為NULL)
                string sql = @"SELECT A.*, B.EQUIPMENT, B.PASSFLAG, B.BATCHID, B.DEVICE
                                  FROM MES_QC_INSP_OBJ A
                                       LEFT JOIN MES_QC_INSP B ON A.QC_INSP_SID = B.QC_INSP_SID
                                 WHERE (ITEM1 = #[STRING] OR ITEM2 = #[STRING] OR ITEM3 = #[STRING] OR EQUIPMENT = #[STRING]) 
                                    AND STATUS = 'FAI' 
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
                        SecondEquipmentName = dr["ITEM5"].ToString(),
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

                ddlFAIReasonCode.Items.Clear();
                ddlSN.Items.Clear();
                ddlEquip.Items.Clear();

                ttbMaterialLot.Text = "";
                ttbDescr.Text = "";
                ttbWorkOrderLot.Text = "";

                btnOK.Enabled = false;

                rdbOK.Checked = true;
                rdbNG.Checked = false;
                rdbPASS.Checked = false;

                rdbOK.Enabled = true;
                rdbNG.Enabled = false;
                rdbPASS.Enabled = false;
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

                ddlFAIReasonCode.Items.Clear();
                ddlSN.Items.Clear();

                ttbMaterialLot.Text = "";
                ttbDescr.Text = "";
                ttbWorkOrderLot.Text = "";

                btnOK.Enabled = false;

                rdbOK.Checked = true;
                rdbNG.Checked = false;
                rdbPASS.Checked = false;

                rdbOK.Enabled = true;
                rdbNG.Enabled = false;
                rdbPASS.Enabled = false;
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
                //清除機台檢驗資料
                _CSTWIPCMMList = new List<CSTWIPCMMInfo>();
                _CSTWIPCMMDataList = new List<CSTWIPCMMDataInfo>();
                gvInspectionData.SetDataSource(_CSTWIPCMMDataList, true);
                ddlFileName.Items.Clear();

                ddlSN.Must(lblSN);

                if (ddlSN.SelectedIndex != -1)
                {
                    //清除原因碼資料
                    ddlFAIReasonCode.Items.Clear();

                    ttbMaterialLot.Text = "";
                    ttbWorkOrderLot.Text = "";
                    ttbDescr.Text = "";

                    rdbPASS.Enabled = false;
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
                        //以機台名稱檢查是否有檢驗結果資料
                        _CSTWIPCMMList = CSTWIPCMMInfo.GetDataListByEquipmantAndFileIDAndDevice(_SelectedQCData.EquipmentName, _SelectedQCData.ComponentLot,
                             _SelectedQCData.WorkOderLot, _SelectedQCData.MaterialLot, _SelectedQCData.DeviceName);

                        //以OP1機台名稱檢查是否有檢驗結果資料
                        var CSTWIPCMMListBySEquipment = CSTWIPCMMInfo.GetDataListByEquipmantAndFileIDAndDevice(_SelectedQCData.SecondEquipmentName, _SelectedQCData.ComponentLot,
                             _SelectedQCData.WorkOderLot, _SelectedQCData.MaterialLot, _SelectedQCData.DeviceName);

                        //將二組資料合併，去除重覆資料
                        CSTWIPCMMListBySEquipment.ForEach(data =>
                        {
                            if (_CSTWIPCMMList.Contains(data) == false)
                            {
                                _CSTWIPCMMList.Add(data);
                            }
                        });

                        if (_CSTWIPCMMList.Count == 0)
                        {
                            //序號:{0} 沒有機台檢驗資料可以顯示!
                            throw new Exception(RuleMessage.Error.C10065(ddlSN.SelectedItem.Text));
                        }

                        //加入機台檢驗檔名稱
                        _CSTWIPCMMList.ForEach(data =>
                        {
                            ddlFileName.Items.Add(new ListItem(data["FILENAME"].ToString(), data.ID));
                        });

                        if (ddlFileName.Items.Count > 0) ddlFileName.Items.Insert(0, "");
                    }
                    else
                    {
                        rdbPASS.Enabled = true;
                    }

                    //取得Lot資料
                    var lotData = InfoCenter.GetBySID<LotInfo>(_SelectedQCData.ObjectSID);

                    rdbNG.Enabled = true;
                    rdbOK.Enabled = true;

                    #region 設置原因碼

                    List<BusinessReason> reason = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(ProgramRight, "ALL", "Default", ReasonMode.Category);
                    if (reason.Count > 0)
                    {
                        ddlFAIReasonCode.DataSource = reason;
                        ddlFAIReasonCode.DataTextField = "ReasonDescription";
                        ddlFAIReasonCode.DataValueField = "ReasonCategorySID";
                        ddlFAIReasonCode.DataBind();

                        if (ddlFAIReasonCode.Items.Count != 1)
                            ddlFAIReasonCode.Items.Insert(0, "");
                        else
                        {
                            ddlFAIReasonCode.SelectedIndex = 0;
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

                ddlFAIReasonCode.Items.Clear();

                ttbMaterialLot.Text = "";
                ttbDescr.Text = "";
                ttbWorkOrderLot.Text = "";

                btnOK.Enabled = false;

                rdbOK.Checked = true;
                rdbNG.Checked = false;
                rdbPASS.Checked = false;
                rdbCLOSE.Checked = false;

                rdbOK.Enabled = true;
                rdbNG.Enabled = false;
                rdbPASS.Enabled = false;

                _CSTWIPCMMList = new List<CSTWIPCMMInfo>();
                _CSTWIPCMMDataList = new List<CSTWIPCMMDataInfo>();
                gvInspectionData.SetDataSource(_CSTWIPCMMDataList, true);
            }
        }

        /// <summary>
        /// 切換檔案名稱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFileName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //確認是否有選擇檔案名稱
                ddlFileName.Must(lblFileName);

                if (ddlFileName.SelectedValue.IsNullOrTrimEmpty() == false)
                {
                    _CSTWIPCMMDataList = CSTWIPCMMDataInfo.GetDataByWIPCMMSID(ddlFileName.SelectedValue);
                    if (_CSTWIPCMMDataList.Count == 0)
                    {
                        //序號:{0} 沒有機台檢驗資料可以顯示!
                        throw new Exception(RuleMessage.Error.C10065(ddlSN.SelectedItem.Text));
                    }

                    gvInspectionData.SetDataSource(_CSTWIPCMMDataList, true);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);

                _CSTWIPCMMDataList = new List<CSTWIPCMMDataInfo>();
                gvInspectionData.SetDataSource(_CSTWIPCMMDataList, true);
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
                    ddlFAIReasonCode.Must(lblFAIReasonCode);
                }
                else if (rdbPASS.Checked)
                {
                    result = "PASS";
                }
                else if (rdbCLOSE.Checked)
                {
                    result = "CLOSE";

                    //確認是否有選擇原因碼
                    ddlFAIReasonCode.Must(lblFAIReasonCode);
                }
                #endregion  

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 更新檢驗主檔[MES_QC_INSP]
                    //取得檢驗主檔資料
                    var QCInsepctionData = InfoCenter.GetBySID<QCInspectionInfo>(_SelectedQCData.QCInspectionSID).ChangeTo<QCInspectionInfoEx>();

                    //原因碼
                    ReasonCategoryInfo reason = null;

                    //有選擇原因碼才更新[NG_Category]及[NG_Reason]
                    if (string.IsNullOrEmpty(ddlFAIReasonCode.SelectedValue) == false)
                    {
                        reason = InfoCenter.GetBySID<ReasonCategoryInfo>(ddlFAIReasonCode.SelectedValue);
                        QCInsepctionData.NG_Category = reason.Category;
                        QCInsepctionData.NG_Reason = reason.Reason;
                    }

                    //如果判定結果為結單的話，則必須把結單時間及人員資料寫回資料庫
                    if (result == "CLOSE")
                    {
                        QCInsepctionData.FINISHTIME = txnStamp.RecordTime;
                        QCInsepctionData.FINISHUSER = txnStamp.UserID;
                    }

                    QCInsepctionData.NG_Description = ttbDescr.Text;
                    QCInsepctionData.Result = result;
                    QCInsepctionData.Status = "Finished";

                    QCInsepctionData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

                    #endregion

                    #region 更新機台檢驗主檔[CST_WIP_CMM]

                    if (_CSTWIPCMMList != null)
                    {
                        //更新檢驗主檔的QCInspectionSID欄位
                        _CSTWIPCMMList.ForEach(data =>
                        {
                            data.QCInspectionSID = QCInsepctionData.ID;
                            data.UpdateToDB();
                        });
                    }

                    #endregion

                    #region 更新機台資訊

                    //取得相同BatchID的檢驗資料
                    var QCDataList = QCInspectionInfoEx.GetDataListByBatchID(_SelectedQCData.BatchID);

                    //相同BatchID都已完成檢驗旗標
                    bool isAllFinish = true;

                    QCDataList.ForEach(p =>
                    {
                        if (!(p.Status == "Finished"))
                        {
                            isAllFinish = false;
                        }
                    });

                    //取得lot資料
                    var lot = InfoCenter.GetBySID<LotInfo>(_SelectedQCData.ObjectSID);

                    //更新機台屬性[FAICOUNT]
                    CustomizeFunction.UpdateFAI(_SelectedQCData.EquipmentName, lot.Lot, (rdbNG.Checked) ? true : false, txnStamp);

                    //如果相同的BatchID都檢驗完成或選擇結果為NG時，則更新狀態為IDLE及更新FAICOUNT
                    if (isAllFinish)
                    {
                        //取得機台狀態資料
                        var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("IDLE");

                        var equipData = EquipmentInfo.GetEquipmentByName(_SelectedQCData.EquipmentName);

                        //更新機台狀態
                        EMSTransaction.ChangeState(equipData, newStateInfo, txnStamp);

                    }
                    #endregion

                    //如果判定結果選擇為NG，則必須拆批送待判
                    if (rdbNG.Checked)
                    {
                        //判定NG直接拆批及送待判工作站
                        CustomizeFunction.SplitDefectLot(_SelectedQCData.ComponentLot, ttbDescr.Text, reason, txnStamp);
                    }

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
    }
}