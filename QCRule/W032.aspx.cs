/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：品管人員開始作PPK檢驗。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/08/28      keith      初版
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
    public partial class W032 : CustomizeRuleBasePage
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

                string currentState = "Wait" + _QCType;

                string equipmentName = "";

                //檢查是否有輸入機台/序號
                ttbEquipOrCompLot.Must(lblEquipOrCompLot);

                #region 檢查輸入的內容在MES_QC_INSP_OBJ.ITEM1是否有符合的資料，如果沒有，則檢查機台
                var QCDataObjList = QCInspectionObjectInfoEx.GetDataListByComponentLot(ttbEquipOrCompLot.Text);

                if (QCDataObjList.Count == 0)
                {
                    var QCInspectionDataList = QCInspectionInfoEx.GetDataListByEquip(ttbEquipOrCompLot.Text, _QCType);

                    if (QCInspectionDataList.Count == 0)
                    {
                        //找不到檢驗資料!!
                        throw new Exception(RuleMessage.Error.C00038());
                    }
                    else
                    {
                        //取得檢驗機台名稱
                        equipmentName = QCInspectionDataList[0].EquipmentName;
                    }

                    #region 取得檢驗清單
                    //string currentState = "WaitPPK";

                    QCInspectionDataList.ForEach(qcData =>
                    {
                        //只有狀態為WaitPPK才可以加入清單
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
                    #endregion
                }
                else
                {
                    //取得檢驗主檔
                    var QCInspectionData = InfoCenter.GetBySID<QCInspectionInfo>(QCDataObjList[0].QC_INSP_SID).ChangeTo<QCInspectionInfoEx>();

                    //確認檢驗主檔狀態是否為WaitPPK
                    if (QCInspectionData.Status != currentState)
                    {
                        //序號:{0} 檢驗主檔狀態為{1}，不可以執行
                        throw new Exception(RuleMessage.Error.C10100(ttbEquipOrCompLot.Text, QCInspectionData.Status));
                    }

                    //取得檢驗機台名稱
                    equipmentName = QCInspectionData.EquipmentName;

                    //取得檢驗資料
                    var row = _QCTable.NewRow();
                    row["Equipment"] = QCInspectionData.EquipmentName;
                    row["Device"] = QCInspectionData.DeviceName;
                    row["SN"] = QCDataObjList[0].ItemName1;
                    row["Status"] = QCInspectionData.PassFlag == "Y" ? "PASS" : GetUIResource("WaitQC");
                    row["QCINSPSID"] = QCInspectionData.ID;

                    _QCTable.Rows.Add(row);
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

                if (_QCTable.Rows.Count == 0)
                {
                    //機台:{0}，查無任何檢驗資料。
                    throw new Exception(RuleMessage.Error.C10053(equipmentName));
                }

                //顯示檢驗清單
                gvQC.SetDataSource(_QCTable, true);
                
                btnOK.Enabled = true;

                //設置初始勾選狀態
                CheckBox ckbSelectAll = gvQC.HeaderRow.FindControl("ckbSelectAll") as CheckBox;
                ckbSelectAll.Checked = true;
                ckbSelectAll_CheckedChanged(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ClearField();
                AjaxFocus(ttbEquipOrCompLot);
                HandleError(ex);
            }
        }

        /// <summary>
        /// 全勾選/全不勾選觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ckbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox ckbSelectAll = gvQC.HeaderRow.FindControl("ckbSelectAll") as CheckBox;
                foreach (GridViewRow gvRow in gvQC.Rows)
                {
                    CheckBox ckbSelect = gvRow.FindControl("ckbSelect") as CheckBox;
                    ckbSelect.Checked = ckbSelectAll.Checked;
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
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                //檢查機台是否有輸入
                ttbEquipOrCompLot.Must(lblEquipOrCompLot);

                //確認是否有勾選資料
                if (SelectedData() == false)
                {
                    // [00816]請至少選取一個{0}！
                    throw new Exception(TextMessage.Error.T00816(GetUIResource("SN")));
                }

                using (var cts = CimesTransactionScope.Create())
                {
                    #region 更新單號狀態為PPK

                    for (int i = 0; i < gvQC.Rows.Count; i++)
                    {
                        var thisCheckBox = (CheckBox)gvQC.Rows[i].FindControl("ckbSelect");

                        //只有勾選的資料才要更新單號狀態
                        if (thisCheckBox.Checked)
                        {
                            DataRow dr = _QCTable.Rows[i];
                            var qcInspectionData = InfoCenter.GetBySID<QCInspectionInfo>(dr["QCINSPSID"].ToString()).ChangeTo<QCInspectionInfoEx>();

                            qcInspectionData.Status = _QCType;
                            qcInspectionData.StartTime = DBCenter.GetSystemTime();
                            qcInspectionData.StartUser = txnStamp.UserID;
                            qcInspectionData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                        }
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

        private bool SelectedData()
        {
            bool result = false;

            for (int i = 0; i < gvQC.Rows.Count; i++)
            {
                var thisCheckBox = (CheckBox)gvQC.Rows[i].FindControl("ckbSelect");

                if (thisCheckBox.Checked)
                {
                    result = true;
                    break;
                }
            }

            return result;
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