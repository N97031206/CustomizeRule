/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：提供報廢品入庫作業，並將入庫資訊拋轉到ERP TABLE。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/08/09      keith      初版
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
    public partial class W016 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 報廢品資料
        /// </summary>
        private List<LotInfoEx> _LotDatas
        {
            get { return this["_LotDatas"] as List<LotInfoEx>; }
            set { this["_LotDatas"] = value; }
        }

        /// <summary>
        /// 己勾選報廢品資料
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

                    gvScrap.CimesEmptyDataText = GetUIResource("NoScrapData");
                    gvScrap.Initialize();

                    //查詢報廢品資料
                    Query();
                }
                else
                {
                   // gvScrap.SetDataSource(_LotDatas, true);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 取得報廢品單號
        /// </summary>
        private void GetScrapNaming()
        {
            //取得不良子批批號名稱
            var scrapNamingList = GetNaming("ScrapInvNo", User.Identity.Name);

            ttbScrapNo.Text = scrapNamingList[0];
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearField()
        {
            _LotDatas = new List<LotInfoEx>();
            _SelectLotDatas = new List<LotInfoEx>();
            _ExecuteNamingSQLList = new List<SqlAgent>();

            ttbScrapCount.Text = "";
            ttbScrapNo.Text = "";
            ttbScrapQty.Text = "";

            btnOK.Enabled = false;

            gvScrap.SetDataSource(_LotDatas, true); 
        }

        /// <summary>
        /// 查詢報廢品資料
        /// </summary>
        private void Query()
        {
            _LotDatas = LotInfoEx.GetLotListByStatus("ScrapInv");

            if (_LotDatas.Count > 0)
            {
                //取得報廢品單號
                GetScrapNaming();

                _SelectLotDatas.AddRange(_LotDatas);

                //計算批數及數量
                CalculationCountAndQty();

                gvScrap.SetDataSource(_LotDatas, true);

                btnOK.Enabled = true;
            }
            else
            {
               // gvScrap.CimesEmptyDataText = "No Data";
            }
        }

        /// <summary>
        /// 計算批數及數量
        /// </summary>
        private void CalculationCountAndQty()
        {
            decimal scrapQty = 0;

            _SelectLotDatas.ForEach(p => { scrapQty += p.Quantity; });

            //設置報廢批數
            ttbScrapCount.Text = _SelectLotDatas.Count.ToCimesString();

            //設置報廢總數量
            ttbScrapQty.Text = scrapQty.ToCimesString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvScrap_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    
                    CheckBox cbx = (CheckBox)e.Row.FindControl("ckbScrapSelect");
                    cbx.Checked = isFind;
                    break;
            }
        }

        /// <summary>
        /// 勾選觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ckbScrapSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var gridIndex = ((GridViewRow)(((CheckBox)sender).NamingContainer)).DataItemIndex;

                var thisCheckBox = (CheckBox)gvScrap.Rows[gridIndex].FindControl("ckbScrapSelect");

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

                gvScrap.SetDataSource(_LotDatas, true);
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
                        _SelectLotDatas.ForEach(lot =>
                        {
                            //將勾選的入庫資料塞到ERP TEMP表 To do...

                            //將入庫單號回寫到INVNO
                            WIPTransaction.ModifyLotSystemAttribute(lot, "INVNO", ttbScrapNo.Text, txnStamp);

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