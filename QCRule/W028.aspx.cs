/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：品管從製造人員手上接收首件。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/08/15      keith      初版
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
    public partial class W028 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 執行PPK/FAI參數
        /// </summary>
        private string _QCType
        {
            get { return this["_QCType"] as string; }
            set { this["_QCType"] = value; }
        }

        /// <summary>
        /// 檢驗清單資料
        /// </summary>
        private DataTable _QCTable
        {
            get { return this["_QCTable"] as DataTable; }
            set { this["_QCTable"] = value; }
        }

        /// <summary>
        /// 機台資訊
        /// </summary>
        private EquipmentInfo _EquipData
        {
            get { return this["_EquipData"] as EquipmentInfo; }
            set { this["_EquipData"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            ttbEquipOrCompLot.Text = "";

            btnOK.Enabled = false;
            btnReject.Enabled = false;

            _QCTable = new DataTable();

            //機台
            _QCTable.Columns.Add("Equipment");

            //序號
            _QCTable.Columns.Add("SN");

            //狀態
            _QCTable.Columns.Add("Status");

            //QC檢驗單SID
            _QCTable.Columns.Add("QCINSPSID");

            //料號
            _QCTable.Columns.Add("Device");

            gvQC.SetDataSource(_QCTable, true);
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
                    if (Request["QCType"] != null)
                    {
                        _QCType = Request["QCType"].ToUpper();
                    }
                    else
                    {
                        //網頁沒有設定QCType!!
                        HintAndRedirect(RuleMessage.Error.C10067());
                        return;
                    }

                    if (Request["QCType"] != null)
                    {
                        _QCType = Request["QCType"].ToUpper();

                        //PPK不用退回鍵
                        if (_QCType != CustomizeFunction.QCType.FAI.ToCimesString())
                        {
                            btnReject.Visible = false;
                        }
                    }
                    else
                    {
                        //網頁沒有設定QCType!!
                        HintAndRedirect(RuleMessage.Error.C10067());
                        return;
                    }

                    //第一次開啟頁面
                    ClearField();
                    AjaxFocus(ttbEquipOrCompLot);
                    gvQC.Initialize();
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
                //清除資料
                _QCTable.Rows.Clear();

                string equipmentName = "";

                //檢查是否有輸入機台/序號
                ttbEquipOrCompLot.Must(lblEquipOrCompLot);

                #region 檢查輸入的內容在MES_QC_INSP_OBJ.ITEM1是否有符合的資料，如果沒有，則檢查機台
                var QCDataObjList = QCInspectionObjectInfoEx.GetDataListByComponentLot(ttbEquipOrCompLot.Text);

                if (QCDataObjList.Count == 0)
                {
                    var QCDataList = QCInspectionInfoEx.GetDataListByEquip(ttbEquipOrCompLot.Text, _QCType);

                    if (QCDataList.Count == 0)
                    {
                        //找不到檢驗資料!!
                        throw new Exception(RuleMessage.Error.C00038());
                    }
                    else
                    {
                        //取得檢驗機台名稱
                        equipmentName = QCDataList[0].EquipmentName;
                    }
                }
                else
                {
                    //取得檢驗機台名稱
                    equipmentName = InfoCenter.GetBySID<QCInspectionInfo>(QCDataObjList[0].QC_INSP_SID).EquipmentName;
                }
                #endregion

                #region 檢查機台是否存在
                _EquipData = EquipmentInfo.GetEquipmentByName(equipmentName);

                if (_EquipData == null)
                {
                    //請輸入正確的機台/序號!
                    throw new Exception(RuleMessage.Error.C10066());
                }
                #endregion

                #region 檢查機台狀態是否為WaitReceiveFAI/WaitReceivePPK
                string currentState = "WaitReceive" + _QCType;

                //如果傳入參數是FAI，則必須檢查機台狀態
                if (_QCType == CustomizeFunction.QCType.FAI.ToCimesString())
                {
                    if (_EquipData.CurrentState != currentState)
                    {
                        //[00902]機台狀態不是{0}，不可使用{1}規則！
                        throw new Exception(TextMessage.Error.T00902(currentState, _ProgramInformationBlock.ProgramRight));
                    }
                }
                #endregion

                #region 顯示檢驗清單
                //取得檢驗主檔
                var qcInspectionDataList = QCInspectionInfoEx.GetDataListByEquip(equipmentName, _QCType);

                if (qcInspectionDataList.Count == 0)
                {
                    //機台:{0}，查無任何檢驗資料。
                    throw new Exception(RuleMessage.Error.C10053(equipmentName));
                }

                qcInspectionDataList.ForEach(qcData =>
                {
                    //只有狀態為WaitReceivePPK/WaitReceiveFAI/WaitReceivePQC才可以加入清單
                    if (qcData.Status == currentState)
                    {
                        //取得檢驗明細
                        var QCInspectionObjList = QCInspectionObjectInfo.GetInspectionObjects(qcData);

                        QCInspectionObjList.ForEach(qcObjData =>
                        {
                            var row = _QCTable.NewRow();
                            row["Equipment"] = qcData.EquipmentName;
                            row["Device"] = qcData.DeviceName;
                            row["SN"] = qcObjData.ItemName1;
                            row["Status"] = qcData.PassFlag == "Y" ? "PASS" : GetUIResource("WaitQC");
                            row["QCINSPSID"] = qcData.ID;

                            _QCTable.Rows.Add(row);
                        });
                    }
                });

                if (_QCTable.Rows.Count == 0)
                {
                    //機台:{0}，查無任何檢驗資料。
                    throw new Exception(RuleMessage.Error.C10053(equipmentName));
                }

                gvQC.SetDataSource(_QCTable, true);

                #endregion

                btnOK.Enabled = true;
                btnReject.Enabled = true;
            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbEquipOrCompLot);
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

                //檢查機台/序號是否有輸入
                ttbEquipOrCompLot.Must(lblEquipOrCompLot);

                using (var cts = CimesTransactionScope.Create())
                {
                    string equipmentState = "Wait" + _QCType;

                    #region 更新機台狀態為WaitFAI
                    //只有FAI需要變更機台狀態，如果QCType參數為PPK或PQC，則不需更新機台狀態
                    if (_QCType.Equals(CustomizeFunction.QCType.FAI.ToCimesString()))
                    {
                        //取得機台狀態資料
                        var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState(equipmentState);
                        if (newStateInfo == null)
                            //C00028 無機台狀態{0}的設定資料!
                            throw new CimesException(RuleMessage.Error.C00028(equipmentState));

                        //更新機台狀態
                        EMSTransaction.ChangeState(_EquipData, newStateInfo, txnStamp);
                    }
                    #endregion

                    #region 更新單號狀態為WaitFAI/WaitPPK
                    foreach (DataRow dr in _QCTable.Rows)
                    {
                        var qcInspectionData = InfoCenter.GetBySID<QCInspectionInfo>(dr["QCINSPSID"].ToString()).ChangeTo<QCInspectionInfoEx>();

                        qcInspectionData.Status = equipmentState;
                        qcInspectionData.ReceiveTime = txnStamp.RecordTime;
                        qcInspectionData.ReceiveUser = txnStamp.UserID;
                        qcInspectionData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }
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

        /// <summary>
        /// 退回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                //檢查機台/序號是否有輸入
                ttbEquipOrCompLot.Must(lblEquipOrCompLot);

                string status = "ToBeSent" + _QCType;

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 更新單號狀態為ToBeSentFAI及設置PASSFLAG = N
                    foreach (DataRow dr in _QCTable.Rows)
                    {
                        var qcInspectionData = InfoCenter.GetBySID<QCInspectionInfo>(dr["QCINSPSID"].ToString()).ChangeTo<QCInspectionInfoEx>();

                        qcInspectionData.Status = status;
                        qcInspectionData.PassFlag = "N";
                        qcInspectionData.QCRejectTime = txnStamp.RecordTime;
                        qcInspectionData.QCRejectUser = txnStamp.UserID;
                        qcInspectionData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }
                    #endregion

                    #region 更新機台狀態
                    //取得機台狀態資料
                    var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState(status);
                    if (newStateInfo == null)
                        //C00028 無機台狀態{0}的設定資料!
                        throw new CimesException(RuleMessage.Error.C00028(status));

                    //更新機台狀態
                    EMSTransaction.ChangeState(_EquipData, newStateInfo, txnStamp);
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
    }
}