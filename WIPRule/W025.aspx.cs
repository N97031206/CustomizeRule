/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：Nick

功能說明：提供批號進行拆包作業，還原批號與批號對應的子單元。

------------------------------------------------------------------
*   日期            作者        變更內容
*   2017/07/23      Nick       初版
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ciMes.Web.Common;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.CustomizeInfo.ExtendInfo;
using CustomizeRule.RuleUtility;

namespace CustomizeRule.WIPRule
{
    public partial class W025 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 批號全域變數
        /// </summary>
        private LotInfo _ProcessLot
        {
            get
            {
                return (LotInfo)this["_ProcessLot"];
            }
            set
            {
                this["_ProcessLot"] = value;
            }
        }

        /// <summary>
        /// 子單元清單全域變數 
        /// </summary>
        private List<ComponentInfo> _ComponentList
        {
            get
            {
                return (List<ComponentInfo>)this["_ComponentList"];
            }
            set
            {
                this["_ComponentList"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // 檢查權限
                    if (!UserProfileInfo.CheckUserRight(User.Identity.Name, ProgramRight))
                    {
                        HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// 清除資料與使用者介面
        /// </summary>
        private void ClearField()
        {
            ClearData();
            ClearUI();
        }

        /// <summary>
        /// 清除使用者介面
        /// </summary>
        private void ClearUI()
        {
            gvComponent.SetDataSource(null, true);
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearData()
        {
            _ProcessLot = null;
            _ComponentList = null;
        }

        protected void ttbBoxNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // 清除資料與使用者介面
                ClearField();

                var convertInfo = WIPConvertInfoEx.GetWIPConvertInfoByTargetLot(ttbBoxNo.Text.Trim());
                if (convertInfo == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00030(lblBoxNo.Text, ttbBoxNo.Text.Trim()), ttbBoxNo);
                }

                // 取得批號
                _ProcessLot = LotInfo.GetLotByLot(ttbBoxNo.Text.Trim());
                // 若批號不存在拋錯
                if (_ProcessLot == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Lot")));
                }
                // 取得批號的所有資單元
                _ComponentList = ComponentInfo.GetLotAllComponents(_ProcessLot);
                // 資料繫結
                gvComponent.SetDataSource(_ComponentList, true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ProcessLot == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00826(lblBoxNo.Text));
                }

                // 定義交易戳記
                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    // 將轉為子單元的批號還原，例如可使用於拆包
                    WIPTxn.Default.ConvertToLot(_ProcessLot, _ComponentList, txnStamp);
                    cts.Complete();
                }
                // 返回定義的預設網頁
                ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                // 返回定義的預設網頁
                ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}