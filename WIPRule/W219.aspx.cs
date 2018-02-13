/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：keith

功能說明：因上線初期鍛造為上線，讓人員補鍛造來料用。
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/09/20      keith       初版
*/

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
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
    public partial class W219 : CustomizeRuleBasePage
    {
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

        private void ClearField()
        {
            ttbLot.Text = "";
            ttbDevice.Text = "";
            ttbQuantity.Text = "";
            ttbLossItemSN.Text = "";
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
                //確認介面是否有輸入資料
                ttbLot.Must(lblLot);
                ttbDevice.Must(lblDevice);
                ttbQuantity.MustInt(lblQuantity);

                //號碼排除清單
                List<int> lossItemSNList = new List<int>();

                //取得輸入的號碼排除
                string lossItemSNString = ttbLossItemSN.Text.Trim();

                if (lossItemSNString.IsNullOrTrimEmpty() == false)
                {
                    //依據逗號分割字串
                    var arrLossItemSN =  ttbLossItemSN.Text.Split(',');

                    //確認輸入的內容格式是否正確
                    for (int i = 0; i < arrLossItemSN.Length; i++)
                    {
                        int value = 0;

                        //如果轉換INT失敗，則顯示錯誤
                        if (int.TryParse(arrLossItemSN[i], out value) == false)
                        {
                            AjaxFocus(ttbLossItemSN);
                            
                            //格式必須為正整數，且使用逗號隔開(例如:1,2,3)!!
                            throw new Exception(RuleMessage.Error.C10150());
                        }
                        else
                        {
                            //確認數值是否大於零，如果小於等於零，則顯示錯誤
                            if (value <= 0)
                            {
                                AjaxFocus(ttbLossItemSN);

                                //格式必須為正整數，且使用逗號隔開(例如:1,2,3)!!
                                throw new Exception(RuleMessage.Error.C10150());
                            }

                            //確認清單內是否已經加入
                            if (lossItemSNList.Contains(value) == false)
                            {
                                lossItemSNList.Add(value);
                            }
                        }
                    }

                    //排序
                    lossItemSNList.Sort();
                }

                var lotNo = ttbLot.Text;
                var quantity = ttbQuantity.Text.ToCimesDecimal();

                #region 確認輸入的鍛造批號是否已建立資料
                var WMSMastBackData = CSTWMSMastBAKInfo.GetDataByLot(lotNo);
                if (WMSMastBackData != null)
                {
                    //鍛造批號:{0} 已存在，不可再執行入庫!
                    throw new Exception(RuleMessage.Error.C10095(lotNo));
                }

                var WMSMastData = CSTWMSMastInfo.GetMaterialLotDataByMaterialLot(lotNo);
                if (WMSMastData != null)
                {
                    //鍛造批號:{0} 已存在，不可再執行入庫!
                    throw new Exception(RuleMessage.Error.C10095(lotNo));
                }
                #endregion

                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    var newMastData = InfoCenter.Create<CSTWMSMastInfo>();

                    //新增主檔
                    newMastData.Lot = lotNo;
                    newMastData.DeviceName = ttbDevice.Text;
                    newMastData.RuleName = _ProgramInformationBlock.ProgramRight;

                    newMastData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    int itemSN = 1;

                    //新增明細檔
                    for (int i = 0; i < quantity; i++)
                    {
                        var newMastDetailData = InfoCenter.Create<CSTWMSDetailInfo>();

                        //確認必須排除的號碼
                        for (int j = 0; j < lossItemSNList.Count; j++)
                        {
                            if (lossItemSNList[j] == itemSN)
                            {
                                itemSN++;
                            }
                        }

                        newMastDetailData.Lot = lotNo;
                        newMastDetailData.ComponentID = string.Format("{0}-{1:000}", lotNo, itemSN);
                        newMastDetailData.Quantity = 1;

                        newMastDetailData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                        itemSN++;
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