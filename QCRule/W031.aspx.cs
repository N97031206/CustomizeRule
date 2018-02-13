/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：品管判定PPK結果。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/08/23      keith      初版
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
    public partial class W031 : CustomizeRuleBasePage
    {
        private class NoSNData
        {
            /// <summary>
            /// 工件編號(小工單號)
            /// </summary>
            public string FileID { get; set; }

            /// <summary>
            /// 順序
            /// </summary>
            public string SN { get; set; }
        }

        /// <summary>
        /// 檢驗主檔資料清單
        /// </summary>
        private List<QCInspectionInfoEx> _QCInspectionDataList
        {
            get { return this["_QCInspectionDataList"] as List<QCInspectionInfoEx>; }
            set { this["_QCInspectionDataList"] = value; }
        }

        /// <summary>
        /// 檢驗明細資料清單
        /// </summary>
        private List<QCInspectionObjectInfoEx> _QCInspectionObjDataList
        {
            get { return this["_QCInspectionObjDataList"] as List<QCInspectionObjectInfoEx>; }
            set { this["_QCInspectionObjDataList"] = value; }
        }

        /// <summary>
        /// 是否要顯示[結單]CLOSE選項旗標
        /// </summary>
        private const string _QCCloseFlag = "QCClose";

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

        /// <summary>
        /// 料號資料
        /// </summary>
        private DeviceVersionInfoEx _DeviceVersionData
        {
            get { return this["_DeviceVersionData"] as DeviceVersionInfoEx; }
            set { this["_DeviceVersionData"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            _DeviceVersionData = null;

            ddlPPKReasonCode.Items.Clear();
            ddlFileName.Items.Clear();

            ddlDevice.Items.Clear();

            ttbEquipOrCompLot.Text = "";
            ttbDescr.Text = "";
            ttbEquip.Text = "";

            btnOK.Enabled = false;

            rdbOK.Checked = true;
            rdbNG.Checked = false;
            rdbCLOSE.Checked = false;

            rdbOK.Enabled = true;
            rdbNG.Enabled = false;

            _QCInspectionDataList = new List<QCInspectionInfoEx>();
            _QCInspectionObjDataList = new List<QCInspectionObjectInfoEx>();

            _CSTWIPCMMList = new List<CSTWIPCMMInfo>();
            _CSTWIPCMMDataList = new List<CSTWIPCMMDataInfo>();

            gvQC.SetDataSource(_QCInspectionObjDataList, true);
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
                    AjaxFocus(ttbEquipOrCompLot);
                    gvQC.Initialize();
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
        /// 輸入機台/序號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbEquipOrCompLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                QCInspectionInfoEx QCInspectionData = new QCInspectionInfoEx();

                //檢查是否有輸入機台/序號
                ttbEquipOrCompLot.Must(lblEquipOrCompLot);

                #region 檢查輸入的內容在MES_QC_INSP_OBJ.ITEM1是否有符合的資料，如果沒有，則檢查機台
                var QCDataObjList = QCInspectionObjectInfoEx.GetDataListByComponentLot(ttbEquipOrCompLot.Text);
                if (QCDataObjList.Count == 0)
                {
                    var QCDataList = QCInspectionInfoEx.GetDataListByEquip(ttbEquipOrCompLot.Text, CustomizeFunction.QCType.PPK.ToCimesString());

                    if (QCDataList.Count == 0)
                    {
                        //請輸入正確的機台/序號!
                        throw new Exception(RuleMessage.Error.C10066());
                    }

                    //取得檢驗主檔資訊
                    QCInspectionData = QCDataList[0];
                }
                else
                {
                    //取得檢驗主檔資訊
                    QCInspectionData = InfoCenter.GetBySID<QCInspectionInfo>(QCDataObjList[0].QC_INSP_SID).ChangeTo<QCInspectionInfoEx>();
                }

                //檢查機台資料及狀態
                CheckEquipCurrentState(QCInspectionData.EquipmentName);

                #endregion

                #region 查詢相同的BatchID的檢驗主檔清單
                _QCInspectionDataList = QCInspectionInfoEx.GetDataListByBatchID(QCInspectionData.BatchID);
                if (_QCInspectionDataList.Count == 0)
                {
                    //檢驗單號:{0} 查無相同的BatchID
                    throw new Exception(RuleMessage.Error.C10110(QCInspectionData.InspectionNo));
                }
                #endregion

                //設置機台名稱
                ttbEquip.Text = QCInspectionData.EquipmentName;

                //取得不重複的料號清單
                var deviceList = _QCInspectionDataList.FindAll(p => p.Status == CustomizeFunction.QCStatus.PPK.ToCimesString()).Select(p => p.DeviceName).Distinct().ToList();
                //var deviceList = _QCInspectionDataList.Select(p => p.DeviceName).Distinct().ToList();

                #region 設置料號資料(DropDownList)

                //清除資料
                ddlDevice.Items.Clear();

                //加入清單
                foreach (var deviceName in deviceList)
                {
                    ddlDevice.Items.Add(new ListItem(deviceName, deviceName));
                }

                if (ddlDevice.Items.Count != 1)
                {
                    ddlDevice.Items.Insert(0, "");
                }
                else
                {
                    ddlDevice.SelectedIndex = 0;
                    ddlDevice_SelectedIndexChanged(null, EventArgs.Empty);
                }
                #endregion  
            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbEquipOrCompLot);
                HandleError(ex);
            }
        }

        /// <summary>
        /// 切換料號
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region 清除資料
                ddlFileName.Items.Clear();
                ddlPPKReasonCode.Items.Clear();
                _CSTWIPCMMList = new List<CSTWIPCMMInfo>();

                _QCInspectionObjDataList.Clear();
                gvQC.SetDataSource(_QCInspectionObjDataList, true);

                _CSTWIPCMMDataList = new List<CSTWIPCMMDataInfo>();
                gvInspectionData.SetDataSource(_CSTWIPCMMDataList, true);

                btnOK.Enabled = false;
                #endregion

                //確認是否有選擇料號
                if (ddlDevice.SelectedItem.Text.IsNullOrTrimEmpty() == false)
                {
                    //取得料號名稱
                    string deviceName = ddlDevice.SelectedItem.Text;

                    //清除檢驗清單
                    _QCInspectionObjDataList.Clear();

                    List<QCInspectionObjectInfoEx> QCInspectionObjDataListTemp = new List<QCInspectionObjectInfoEx>();

                    _QCInspectionDataList.ForEach(QCData =>
                    {
                        //符合所選的料號才可以加入序號清單
                        if (QCData.DeviceName == deviceName)
                        {
                            var selectDataList = QCInspectionObjectInfoEx.GetDataListByQCInspectionSID(QCData.QC_INSP_SID);

                            QCInspectionObjDataListTemp.AddRange(selectDataList);
                        }
                    });

                    //以SID排序
                    _QCInspectionObjDataList = QCInspectionObjDataListTemp.OrderBy(p => p.ID).ToList();

                    #region 檢驗資料

                    //確認料號資料是否在在
                    _DeviceVersionData = DeviceVersionInfo.GetActiveDeviceVersion(deviceName).ChangeTo<DeviceVersionInfoEx>();
                    if (_DeviceVersionData == null)
                    {
                        //[00030]{0}：{1}不存在!
                        throw new Exception(TextMessage.Error.T00030(lblDevice.Text, deviceName));
                    }

                    //清除資料
                    _CSTWIPCMMList.Clear();

                    if (_DeviceVersionData.ProdType == CustomizeFunction.ProdType.S.ToCimesString())
                    {
                        _QCInspectionObjDataList.ForEach(data =>
                        {
                            //以機台名稱檢查是否有檢驗結果資料
                            var CSTWIPCMMListByEquipment = CSTWIPCMMInfo.GetDataListByEquipmantAndFileIDAndDevice(ttbEquip.Text, data.ItemName1, data.ItemName2,
                                data.ItemName3, deviceName);

                            //以OP1機台名稱檢查是否有檢驗結果資料
                            var CSTWIPCMMListBySEquipment = CSTWIPCMMInfo.GetDataListByEquipmantAndFileIDAndDevice(data.ItemName5, data.ItemName1, data.ItemName2,
                                data.ItemName3, deviceName);

                            //將二組資料合併，去除重覆資料
                            CSTWIPCMMListBySEquipment.ForEach(WIPCMMdata =>
                            {
                                if (CSTWIPCMMListByEquipment.Contains(WIPCMMdata) == false)
                                {
                                    CSTWIPCMMListByEquipment.Add(WIPCMMdata);
                                }
                            });

                            if (CSTWIPCMMListByEquipment.Count == 0)
                            {
                                //PPK 檢驗工件未到齊，無法執行 PPK 判定!
                                throw new Exception(RuleMessage.Error.C10118());
                            }

                            //再將找到的資料合併，去除重覆資料
                            CSTWIPCMMListByEquipment.ForEach(WIPCMMdata =>
                            {
                                //將[QC_INSP_SID]寫到每一筆檢驗機台資料
                                WIPCMMdata.QCInspectionSID = data.QC_INSP_SID;

                                if (_CSTWIPCMMList.Contains(WIPCMMdata) == false)
                                {
                                    _CSTWIPCMMList.Add(WIPCMMdata);
                                }
                            });
                        });
                    }
                    else
                    {
                        //以機台名稱檢查是否有檢驗結果資料
                        var CSTWIPCMMListByEquipment = CSTWIPCMMInfo.GetDataListByEquipmantAndDevice(ttbEquip.Text, deviceName);
                        if (CSTWIPCMMListByEquipment.Count == 0)
                        {
                            //PPK 檢驗工件未到齊，無法執行 PPK 判定!
                            throw new Exception(RuleMessage.Error.C10118());
                        }

                        _QCInspectionObjDataList.ForEach(data =>
                        {
                            //以OP1機台名稱檢查是否有檢驗結果資料
                            var CSTWIPCMMListBySEquipment = CSTWIPCMMInfo.GetDataListByEquipmantAndDevice(data.ItemName5, deviceName);

                            //將二組資料合併，去除重覆資料
                            CSTWIPCMMListBySEquipment.ForEach(WIPCMMdata =>
                            {
                                if (CSTWIPCMMListByEquipment.Contains(WIPCMMdata) == false)
                                {
                                    CSTWIPCMMListByEquipment.Add(WIPCMMdata);
                                }
                            });
                        });

                        //重新依據[FILEID], [SN], [SEQUENCE]執行排序 
                        _CSTWIPCMMList = CSTWIPCMMListByEquipment.OrderBy(data => data.FileID).ThenBy(data => data["SN"]).ToList();

                        #region 確認檢驗結果主檔筆數是否與檢驗單明細筆數相同
                        List<NoSNData> NoSNDataList = new List<NoSNData>();

                        _CSTWIPCMMList.ForEach(data =>
                        {
                            NoSNData tempNoSNData = new NoSNData();
                            tempNoSNData.SN = data["SN"].ToCimesString();
                            tempNoSNData.FileID = data.FileID;

                            if (NoSNDataList.Contains(tempNoSNData) == false)
                            {
                                NoSNDataList.Add(tempNoSNData);
                            }
                        });

                        //比對檢驗結果主檔筆數是否與檢驗單明細筆數相同
                        if (NoSNDataList.Count != _QCInspectionObjDataList.Count)
                        {
                            //檢驗結果主檔筆數({0})與檢驗單明細筆數({1})不相同
                            throw new Exception(RuleMessage.Error.C10112(NoSNDataList.Count().ToString(), _QCInspectionObjDataList.Count.ToString()));
                        }
                        #endregion

                        for (int i = 0; i < NoSNDataList.Count; i++)
                        {
                            string QCInspectionSID = _QCInspectionObjDataList[i].QC_INSP_OBJ_SID;

                            _CSTWIPCMMList.ForEach(data =>
                            {
                                if (data.FileID == NoSNDataList[i].FileID && data["SN"].ToCimesString() == NoSNDataList[i].SN)
                                {
                                    //將[QC_INSP_SID]寫到每一筆檢驗機台資料
                                    data.QCInspectionSID = QCInspectionSID;
                                }
                            });
                        }
                    }

                    //加入機台檢驗檔名稱
                    _CSTWIPCMMList.ForEach(WIPCMMdata =>
                    {
                        ddlFileName.Items.Add(new ListItem(WIPCMMdata["FILENAME"].ToString(), WIPCMMdata.ID));
                    });

                    if (ddlFileName.Items.Count > 0) ddlFileName.Items.Insert(0, "");

                    gvQC.SetDataSource(_QCInspectionObjDataList, true);
                    #endregion

                    #region 設置原因碼

                    //清除原因碼資料
                    ddlPPKReasonCode.Items.Clear();

                    rdbNG.Enabled = false;
                    rdbOK.Enabled = false;

                    rdbNG.Enabled = true;
                    rdbOK.Enabled = true;

                    //取得原因碼清單
                    List<BusinessReason> reason = ReasonCategoryInfo.GetOperationRuleCategoryReasonsWithReasonDescr(ProgramRight, "ALL", "Default", ReasonMode.Category);
                    if (reason.Count > 0)
                    {
                        ddlPPKReasonCode.DataSource = reason;
                        ddlPPKReasonCode.DataTextField = "ReasonDescription";
                        ddlPPKReasonCode.DataValueField = "ReasonCategorySID";
                        ddlPPKReasonCode.DataBind();

                        if (ddlPPKReasonCode.Items.Count != 1)
                            ddlPPKReasonCode.Items.Insert(0, "");
                        else
                        {
                            ddlPPKReasonCode.SelectedIndex = 0;
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
                //清除資料
                ddlFileName.Items.Clear();
                ddlPPKReasonCode.Items.Clear();
                _CSTWIPCMMList = new List<CSTWIPCMMInfo>();

                _QCInspectionObjDataList.Clear();
                gvQC.SetDataSource(_QCInspectionObjDataList, true);

                _CSTWIPCMMDataList = new List<CSTWIPCMMDataInfo>();
                gvInspectionData.SetDataSource(_CSTWIPCMMDataList, true);

                btnOK.Enabled = false;

                AjaxFocus(ddlDevice);
                HandleError(ex);
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
                        //throw new Exception(RuleMessage.Error.C10065(ddlSN.SelectedItem.Text));
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
        /// 檢查機台資料及狀態
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <returns></returns>
        private bool CheckEquipCurrentState(string equipmentName)
        {
            //檢查機台是否存在
            var equipData = EquipmentInfo.GetEquipmentByName(equipmentName);
            if (equipData == null)
            {
                //請輸入正確的機台/序號!
                throw new Exception(RuleMessage.Error.C10066());
            }

            //檢查機台狀態是否為PPK
            string currentState = CustomizeFunction.QCType.PPK.ToCimesString();
            if (equipData.CurrentState != currentState)
            {
                //[00902]機台狀態不是{0}，不可使用{1}規則！
                throw new Exception(TextMessage.Error.T00902(currentState, _ProgramInformationBlock.ProgramRight));
            }

            return true;
        }
        /// <summary>
        /// 勾選觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ckbDefectSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var gridIndex = ((GridViewRow)(((CheckBox)sender).NamingContainer)).DataItemIndex;

                var thisCheckBox = (CheckBox)gvQC.Rows[gridIndex].FindControl("ckbDefectSelect");

                if (thisCheckBox.Checked == true)
                {
                    _QCInspectionObjDataList[gridIndex].ItemName4 = "NG";
                }
                else
                {
                    _QCInspectionObjDataList[gridIndex].ItemName4 = "OK";
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private bool SelectedNGData()
        {
            bool result = false;

            for (int i = 0; i < gvQC.Rows.Count; i++)
            {
                var thisCheckBox = (CheckBox)gvQC.Rows[i].FindControl("ckbDefectSelect");

                if (thisCheckBox.Checked)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            /*******************************************************************
             * PPK判定後須執行事項：
             * 1. 依照判定的BATCHID+料號，將所屬單號資料更新。
             * 2. 依照判定狀態更新CST_WIP_PPK結果。
             * 3. 如同一個BATCHID都判定完成，變更機台狀態，從PPK變IDLE。
             *    如一個BATCHID有兩個料號，需兩個料號都判定完成才可以變更機台狀態。
             ******************************************************************/
            try
            {
                //取得所選擇的料號名稱
                var deviceName = ddlDevice.SelectedValue;

                #region 確認選擇結果，如果是選擇NG，確認是否有選擇原因碼及選擇結果及檢驗清單的勾選是否符合
                string result = "";
                if (rdbOK.Checked)
                {
                    result = "OK";
                }
                else if (rdbNG.Checked)
                {
                    result = "NG";

                    //確認是否有選擇原因碼
                    ddlPPKReasonCode.Must(lblPPKReasonCode);

                    if (SelectedNGData() == false)
                    {
                        //PPK結果選擇NG，檢驗清單至少勾選一筆資料！
                        throw new Exception(RuleMessage.Error.C10070());
                    }
                }
                else if (rdbCLOSE.Checked)
                {
                    result = "CLOSE";

                    //確認是否有選擇原因碼
                    ddlPPKReasonCode.Must(lblPPKReasonCode);
                }
                #endregion

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 更新檢驗主檔清單[MES_QC_INSP]
                    _QCInspectionDataList.ForEach(QCInspectionData =>
                        {
                            //符合選擇的料號才進行資料更新
                            if (QCInspectionData.DeviceName == deviceName)
                            {
                                //有選擇原因碼才更新[NG_Category]及[NG_Reason]
                                if (string.IsNullOrEmpty(ddlPPKReasonCode.SelectedValue) == false)
                                {
                                    var reason = InfoCenter.GetBySID<ReasonCategoryInfo>(ddlPPKReasonCode.SelectedValue);
                                    QCInspectionData.NG_Category = reason.Category;
                                    QCInspectionData.NG_Reason = reason.Reason;
                                }

                                //如果判定結果為結單的話，則必須把結單時間及人員資料寫回資料庫
                                if (result == "CLOSE")
                                {
                                    QCInspectionData.FINISHTIME = txnStamp.RecordTime;
                                    QCInspectionData.FINISHUSER = txnStamp.UserID;
                                }

                                QCInspectionData.NG_Description = ttbDescr.Text;
                                QCInspectionData.Result = result;
                                QCInspectionData.Status = "Finished";

                                QCInspectionData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                            }
                        });
                    #endregion

                    #region 更新機台資訊及檢驗工件結果

                    //取某一支工件序號的資料來傳入UpdatePPK
                    var updateLot = InfoCenter.GetBySID<LotInfo>(_QCInspectionObjDataList[0].OBJECTSID).ChangeTo<LotInfoEx>();

                    //更新[CST_WIP_PPK.PPKCOUNT]
                    CustomizeFunction.UpdatePPK(_QCInspectionDataList[0].EquipmentName, updateLot.DeviceName, updateLot.DeviceVersion, (rdbNG.Checked) ? "false" : "true");

                    //將介面上所勾選/不勾選的資料對應到MES_QC_INSP_OBJ.ITEM4的欄位
                    for (int i = 0; i < gvQC.Rows.Count; i++)
                    {
                        var thisCheckBox = (CheckBox)gvQC.Rows[i].FindControl("ckbDefectSelect");

                        if (thisCheckBox.Checked)
                        {
                            _QCInspectionObjDataList[i].ItemName4 = "NG";
                        }
                        else
                        {
                            _QCInspectionObjDataList[i].ItemName4 = "OK";
                        }

                        _QCInspectionObjDataList[i].UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }

                    //更新機台檢驗主檔
                    _CSTWIPCMMList.ForEach(data =>
                    {
                        data.UpdateToDB();
                    });

                    //取得相同BatchID的檢驗資料 (理論上應該都完成檢驗...)
                    //如果是雙料號，每次判定要分料號判定
                    var QCDataList = QCInspectionInfoEx.GetDataListByBatchIDAndDeviceName(_QCInspectionDataList[0].BatchID, deviceName);
                    //如果是雙料號，每次判定要分料號判定，但是機台的狀態要該batchID都判定通過，才算PPK完成
                    var QCDAllataList = QCInspectionInfoEx.GetDataListByBatchID(_QCInspectionDataList[0].BatchID);

                    #region 如果相同的BatchID都檢驗完成時，則更新機台狀態為IDLE
                    //相同BatchID都已完成檢驗旗標
                    //如果是雙料號，每次判定要分料號判定，但是機台的狀態要該batchID都判定通過，才算PPK完成
                    bool isAllFinish = true;

                    QCDAllataList.ForEach(p =>
                    {
                        if (p.Status != "Finished")
                        {
                            isAllFinish = false;
                        }
                    });

                    if (isAllFinish)
                    {
                        //取得機台狀態資料
                        var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("IDLE");

                        //檢查機台是否存在
                        var equipData = EquipmentInfo.GetEquipmentByName(_QCInspectionDataList[0].EquipmentName);
                        if (equipData == null)
                        {
                            //[00885]機台{0}不存在！
                            throw new Exception(TextMessage.Error.T00885(_QCInspectionDataList[0].EquipmentName));
                        }

                        //更新機台狀態
                        EMSTransaction.ChangeState(equipData, newStateInfo, txnStamp);
                    }
                    #endregion

                    #region 如果相同的BatchID都檢驗完成且結果都為OK時，則更新機台檢驗資料及更新COUNT
                    //相同BatchID都已完成檢驗及結果都為OK旗標
                    bool isAllFinishAndOK = true;
                    QCDataList.ForEach(p =>
                    {
                        if (!(p.Status == "Finished" && p.Result == "OK"))
                        {
                            isAllFinishAndOK = false;
                        }
                    });

                    if (isAllFinishAndOK)
                    {
                        //取得AutoMerge原因碼
                        var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "AutoMerge");
                        if (reasonCategory == null)
                        {
                            //[00030]{0}：{1}不存在!
                            throw new Exception(TextMessage.Error.T00030("", "CustomizeReason- AutoMerge"));
                        }

                        _QCInspectionObjDataList.ForEach(Data =>
                        {
                            //更新機台檢驗資料
                            Data.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                        });

                        var mergeList = _QCInspectionObjDataList.Select(p => p.ItemName2).Distinct().ToList();

                        foreach (var lot in mergeList)
                        {
                            var lotData = LotInfoEx.GetLotByWorkOrderLot(lot);

                            //確認FAI是否已經檢驗完成
                            if (CustomizeFunction.CheckFAI(_QCInspectionDataList[0].EquipmentName, lotData.Lot))
                            {

                                //執行AutoMerge
                                CustomizeFunction.AutoMerge(lotData.InventoryLot, txnStamp, reasonCategory);
                            }
                        }
                    }
                    #endregion

                    #endregion

                    cts.Complete();
                }

                ClearField();
                AjaxFocus(ttbEquipOrCompLot);

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