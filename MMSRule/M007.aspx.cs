/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：MMS
作者：Evan

功能說明：提供修改鋁材物料數量
------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/09/19      Evan        初版
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

namespace CustomizeRule.MMSRule
{
    public partial class M007 : CustomizeRuleBasePage
    {
        /// <summary>
        /// MMSIQCInfo清單資料
        /// </summary>
        private List<CSTMMSIqcInfo> _CSTMMSIqcInfoDataList
        {
            get { return this["_CSTMMSIqcInfoDataList"] as List<CSTMMSIqcInfo>; }
            set { this["_CSTMMSIqcInfoDataList"] = value; }
        }

        /// <summary>
        /// MMSIQCInfo資料
        /// </summary>
        private CSTMMSIqcInfo _CSTMMSIqcInfoData
        {
            get { return this["_CSTMMSIqcInfoData"] as CSTMMSIqcInfo; }
            set { this["_CSTMMSIqcInfoData"] = value; }
        }

        /// <summary>
        /// MMSIQCPass資料清單
        /// </summary>
        private List<CSTMMSIqcPassInfo> _CSTMMSIqcPassDataList
        {
            get { return this["_CSTMMSIqcPassDataList"] as List<CSTMMSIqcPassInfo>; }
            set { this["_CSTMMSIqcPassDataList"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        private void ClearField()
        {
            ttbIQCRunID.Text = "";
            ttbIQCLot.Text = "";
            ttbOldQuantity.Text = "";
            ttbNewQuantity.Text = "";

            _CSTMMSIqcInfoData = null;
            _CSTMMSIqcInfoDataList = new List<CSTMMSIqcInfo>();
            _CSTMMSIqcPassDataList = new List<CSTMMSIqcPassInfo>();

            ddlIQCTime.Items.Clear();
            ddlIQCTime.Enabled = false;
            btnOK.Enabled = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //第一次開啟頁面
                    ClearField();
                    AjaxFocus(ttbIQCRunID);
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

        protected void ttbIQCLot_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //清除資料
                ddlIQCTime.Items.Clear();
                ddlIQCTime.Enabled = false;

                //檢查是否有輸入
                ttbIQCRunID.Must(lblIQCRunID);
                ttbIQCLot.Must(lblIQCLot);

                //取得IQCInfo資料
                _CSTMMSIqcInfoDataList = CSTMMSIqcInfo.GetIQCInfoDataByRunIDAndLot(ttbIQCRunID.Text, ttbIQCLot.Text);

                if (_CSTMMSIqcInfoDataList.Count > 0)
                {
                    ddlIQCTime.DataSource = _CSTMMSIqcInfoDataList;
                    ddlIQCTime.DataTextField = "IQCTIME";
                    ddlIQCTime.DataValueField = "IQCTIME";
                    ddlIQCTime.DataBind();
                    if (ddlIQCTime.Items.Count != 1)
                        ddlIQCTime.Items.Insert(0, "");
                    else
                    {
                        ddlIQCTime.SelectedIndex = 0;
                        GetData();
                    }
                    ddlIQCTime.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                ClearField();
                HandleError(ex);
            }
        }

        protected void ttbNewQuantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //檢查是否有輸入
                ttbNewQuantity.Must(lblNewQuantity);

                decimal qty = 0;
                _CSTMMSIqcPassDataList.ForEach(p =>
                {
                    qty += p.Quantity.ToDecimal();
                });

                if(ttbNewQuantity.Text.ToDecimal() < qty)
                {
                    // [00040]{0}不可小於{1}！
                    throw new Exception(TextMessage.Error.T00040(lblNewQuantity.Text, qty.ToString()));
                }
                btnOK.Enabled = true;

            }
            catch (Exception ex)
            {
                ttbNewQuantity.Text = "";
                btnOK.Enabled = false;
                HandleError(ex);
            }
        }

        protected void ddlIQCTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ttbIQCRunID.Must(lblIQCRunID);
                ttbIQCLot.Must(lblIQCLot);

                GetData();
            }
            catch (Exception ex)
            {
                btnOK.Enabled = false;
                HandleError(ex);
            }
        }

        private void GetData()
        {
            //取得IQCInfo資料
            _CSTMMSIqcInfoData = CSTMMSIqcInfo.GetIQCInfoData(ttbIQCRunID.Text, ttbIQCLot.Text, ddlIQCTime.SelectedValue);

            //取得IQCPass資料清單
            _CSTMMSIqcPassDataList = CSTMMSIqcPassInfo.GetIQCPassData(ttbIQCRunID.Text, ttbIQCLot.Text, ddlIQCTime.SelectedValue);

            ttbOldQuantity.Text = _CSTMMSIqcInfoData.Quantity.ToString();
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
                if(_CSTMMSIqcInfoData == null)
                {
                    // [00060]{0}沒有資料可顯示！
                    throw new Exception(TextMessage.Error.T00060(""));
                }
                if (ttbNewQuantity.Text.IsNullOrTrimEmpty())
                {
                    // [00060]{0}沒有資料可顯示！
                    throw new Exception(TextMessage.Error.T00060(lblNewQuantity.Text));
                }
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    var CSTMMSIqcInfoLogData = _CSTMMSIqcInfoData.Fill<CSTMMSIqcInfoLog>();
                    CSTMMSIqcInfoLogData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    _CSTMMSIqcInfoData.Quantity = ttbNewQuantity.Text.ToDecimal();
                    _CSTMMSIqcInfoData.UpdateToDB();
                    cts.Complete();
                }

                ClearField();
                AjaxFocus(ttbIQCRunID);

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