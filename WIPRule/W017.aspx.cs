/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：提供不良品入庫作業，並將入庫資訊拋轉到ERP TABLE。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/08/09      keith      初版
*/

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.CustomizeInfo.ExtendInfo;

using CustomizeRule.RuleUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CustomizeRule.WIPRule
{
    public partial class W017 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 不良品資料
        /// </summary>
        private List<LotInfoEx> _LotDatas
        {
            get { return this["_LotDatas"] as List<LotInfoEx>; }
            set { this["_LotDatas"] = value; }
        }

        /// <summary>
        /// 己勾選不良品資料
        /// </summary>
        private List<LotInfoEx> _SelectLotDatas
        {
            get { return this["_SelectLotDatas"] as List<LotInfoEx>; }
            set { this["_SelectLotDatas"] = value; }
        }

        private List<SqlAgent> _ExecuteNamingSQLList
        {
            get { return this["ExecuteNamingSQLList"] as List<SqlAgent>; }
            set { this["_ExecuteNamingSQLList"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
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

                    gvDefect.CimesEmptyDataText = GetUIResource("NoDefectData");
                    gvDefect.Initialize();

                    //查詢不良品資料
                    Query();
                }
                else
                {
                    // gvDefect.SetDataSource(_LotDatas, true);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 取得不良品單號
        /// </summary>
        private void GetDefectNaming()
        {
            //取得不良子批批號名稱
            var defectNamingList = GetNaming("DefectInvNo", User.Identity.Name);

            ttbDefectNo.Text = defectNamingList[0];
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearField()
        {
            _LotDatas = new List<LotInfoEx>();
            _SelectLotDatas = new List<LotInfoEx>();
            _ExecuteNamingSQLList = new List<SqlAgent>();

            ttbDefectCount.Text = "";
            ttbDefectNo.Text = "";
            ttbDefectQty.Text = "";

            btnOK.Enabled = false;

            gvDefect.SetDataSource(_LotDatas, true);
        }

        /// <summary>
        /// 查詢不良品資料
        /// </summary>
        private void Query()
        {
            _LotDatas = LotInfoEx.GetLotListByStatus("DefectInv");

            if (_LotDatas.Count > 0)
            {
                //取得不良品單號
                GetDefectNaming();

                _SelectLotDatas.AddRange(_LotDatas);

                //計算批數及數量
                CalculationCountAndQty();

                gvDefect.SetDataSource(_LotDatas, true);

                btnOK.Enabled = true;
            }
            else
            {
                // gvDefect.CimesEmptyDataText = "No Data";
            }
        }

        /// <summary>
        /// 計算批數及數量
        /// </summary>
        private void CalculationCountAndQty()
        {
            decimal defectQty = 0;

            _SelectLotDatas.ForEach(p => { defectQty += p.Quantity; });

            //設置不良批數
            ttbDefectCount.Text = _SelectLotDatas.Count.ToCimesString();

            //設置不良總數量
            ttbDefectQty.Text = defectQty.ToCimesString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDefect_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;

                case DataControlRowType.DataRow:

                    int index = e.Row.DataItemIndex;

                    bool isFind = false;

                    foreach (var lot in _SelectLotDatas)
                    {
                        if (lot.Lot == _LotDatas[index].Lot)
                        {
                            isFind = true;
                            break;
                        }
                    };

                    CheckBox cbx = (CheckBox)e.Row.FindControl("ckbDefectSelect");
                    cbx.Checked = isFind;
                    break;
            }
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

                var thisCheckBox = (CheckBox)gvDefect.Rows[gridIndex].FindControl("ckbDefectSelect");

                if (thisCheckBox.Checked == true)
                {
                    _SelectLotDatas.Add(_LotDatas[gridIndex]);
                }
                else
                {
                    _SelectLotDatas.Remove(_LotDatas[gridIndex]);
                }

                //計算批數及數量
                CalculationCountAndQty();

                gvDefect.SetDataSource(_LotDatas, true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    if (_SelectLotDatas.Count > 0)
                    {
                        string sLotInString = "";
                        // 組出where in字串
                        _SelectLotDatas.ForEach(p =>
                        {
                            sLotInString += "'" + p.Lot + "',";
                        });
                        sLotInString = sLotInString.EndsWith(",") ? sLotInString.Substring(0, sLotInString.Length - 1) : sLotInString;

                        var dtInvData = DBCenter.GetDataTable(@"SELECT WO,INVLOT,SUM(QUANTITY) QUANTITY,DEVICE,FACTORY FROM MES_WIP_LOT  WHERE LOT IN(" + sLotInString + ") GROUP BY WO,INVLOT,DEVICE,FACTORY ORDER BY WO");
                        string headerSID = "";
                        string date = txnStamp.RecordTime.Substring(0, 10).Replace("/", "");
                        string time = txnStamp.RecordTime.Substring(11).Replace(":", "");
                        string preWo = string.Empty;
                        int seq = 1;
                        // SAI倉位對應表
                        var lstSAIWarehouse = WpcExClassItemInfo.GetInfoByClass("SAIWarehouse");

                        dtInvData.Rows.LoopDo<DataRow>((p, i) =>
                        {
                            string currentWO = p["WO"].ToString();
                            string device = p["DEVICE"].ToString();
                            string factory = p["FACTORY"].ToString();
                            string invLot = p["INVLOT"].ToString();
                            string qty = p["QUANTITY"].ToString();
                            if (preWo != currentWO)
                            {
                                seq = 1;
                                headerSID = DBCenter.GetSystemID();
                                DBCenter.ExecuteParse(@"INSERT INTO PPFHK (SID, WDATE, WTIME, BUDAT, AUFNR, FLAG) 
                                    VALUES (#[STRING], #[STRING], #[STRING], #[STRING], #[STRING],'N')", headerSID, date, time, date, currentWO);
                            }
                            //寫入PPFHP
                            //WERKS:Factory
                            //LGORT:入庫倉位
                            //CHARG:INVLOT
                            //MATNR:Device
                            var lstDetail = _SelectLotDatas.FindAll(select => select.InventoryLot == invLot);
                            var warehouseInfo = lstSAIWarehouse.Find(wpcItem => wpcItem.Remark01 == lstDetail[0].Process && wpcItem.Remark03 == "DEFECT");
                            if (warehouseInfo == null)
                            {
                                //T00555:查無資料，請至系統資料維護新增類別{0}、項目{1}!
                                throw new CimesException(TextMessage.Error.T00555("SAIWarehouse", lstDetail[0].Process + "Remark03:DEFECT"));
                            }
                            DBCenter.ExecuteParse(@"INSERT INTO PPFHP (SID, SEQNR, WDATE, WTIME, MATNR, WERKS, LGORT, CHARG, MENGE, MEINS, FLAG)
                                VALUES (#[STRING], #[DECIMAL], #[STRING], #[STRING], #[STRING], #[STRING], #[STRING], #[STRING], #[STRING], #[STRING], 'N')", headerSID, seq, date, time, device, factory, warehouseInfo.Remark02, invLot, qty, lstDetail[0].Unit);

                            int detailseq = 1;
                            lstDetail.ForEach(lot => {
                                var defectInfo = CSTWIPDefectJudgementInfo.GetDataByLot(lot.Lot);
                                var reasonGroupInfo = WIPReasonGroupInfoEx.GetReasonGroupByCategory(defectInfo["REASONCATEGORY"].ToString());
                                var reasonGroup = reasonGroupInfo == null ? "" : reasonGroupInfo.REASONGROUP;
                                //寫入PPBCH
                                DBCenter.ExecuteParse(@"INSERT INTO PPBCH (SID, MATNR, WERKS, CHARG, SEQNO, WDATE, WTIME, CLASS, ATINN, ATWRT, FLAG)
                                    Values (#[STRING], #[STRING], #[STRING], #[STRING], #[DECIMAL], #[STRING], #[STRING], #[STRING], #[STRING], #[STRING], 'N')", headerSID, device, factory, invLot, detailseq, date, time, reasonGroup, defectInfo["REASONCATEGORY"].ToString(), defectInfo.Reason);
                                detailseq += 1;
                            });
                            
                            seq += 1;
                            preWo = currentWO;
                        });

                        _SelectLotDatas.ForEach(lot =>
                        {
                            //寫入WMSMaster & Detail
                            var wmsMaster = CSTWMSMastInfo.GetMaterialLotDataByMaterialLot(lot.InventoryLot);
                            if (wmsMaster == null)
                            {
                                wmsMaster = InfoCenter.Create<CSTWMSMastInfo>();
                                wmsMaster.Lot = lot.InventoryLot;
                                wmsMaster.DeviceName = lot.DeviceName;
                                wmsMaster.RuleName = txnStamp.RuleName;
                                wmsMaster.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                            }
                            //理論上只會有一個Comp
                            lot.GetLotAllComponents().ForEach(comp => {
                                var wmsDetail = InfoCenter.Create<CSTWMSDetailInfo>();
                                wmsDetail.Lot = lot.InventoryLot;
                                wmsDetail.ComponentID = comp.ComponentID;
                                wmsDetail.Quantity = comp.ComponentQuantity;
                                wmsDetail.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                            });

                            //將入庫單號回寫到批號身上
                            WIPTransaction.ModifyLotSystemAttribute(lot, "INVNO", ttbDefectNo.Text, txnStamp);

                            //將勾選的批號結批
                            WIPTransaction.TerminateLot(lot, txnStamp);                                                    
                        });

                        #region 更新命名規則
                        if (_ExecuteNamingSQLList != null && _ExecuteNamingSQLList.Count > 0)
                        {
                            DBCenter.ExecuteSQL(_ExecuteNamingSQLList);
                        }
                        #endregion                        

                        cts.Complete();
                    }
                    else
                    {
                        //[00816]請至少選取一個{0}！
                        throw new Exception(TextMessage.Error.T00816(GetUIResource("WorkpieceLot")));
                    }
                }

                ClearField();

                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00614(""));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 取消
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
        /// <param name="iCount"></param>
        /// <returns></returns>
        private List<string> GetNaming(string ruleName, string user, InfoBase info = null, int iCount = 1)
        {
            var naming = NamingIDGenerator.GetRule(ruleName);
            if (naming == null)
            {
                //[00437]找不到可產生的序號，請至命名規則維護設定，規則名稱：{0}!!!
                throw new Exception(TextMessage.Error.T00437(ruleName));
            }

            string[] ags = new string[] { };

            var nextIDPair = naming.GenerateNextIDs(iCount, info, ags, user);

            _ExecuteNamingSQLList = nextIDPair.Second;

            return nextIDPair.First.ToList();
        }
    }
}