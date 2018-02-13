using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Resources;
using System.Globalization;
using System.Diagnostics;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Transaction;
using System.Collections.Generic;
using ciMes.Web.Common;

namespace CustomizeRule.EQPRule
{
    public partial class E001 : CimesRuleBasePage
    {
        private List<WpcExClassItemInfo> _SAIEQPChangeStateList
        {
            get { return this["_SAIEQPChangeStateList"] as List<WpcExClassItemInfo>; }
            set { this["_SAIEQPChangeStateList"] = value; }
        }

        protected override void VCFailure(object sender, EventArgs e)
        {
        }

        protected override void VCSuccess(object sender, EventArgs e)
        {
        }

        private EquipmentInfo mEQP
        {
            get
            {
                return (EquipmentInfo)this["mEQP"];
            }
            set
            {
                this["mEQP"] = value;
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    _SAIEQPChangeStateList = WpcExClassItemInfo.GetExClassItemInfo("SAIEQPChangeState", ProgramRight);
                    ciEquipment.Text = this.GetCookie("Equipment");
                    if (!ciEquipment.Text.IsNullOrTrimEmpty())
                        ciEquipment.Verify(true);
                    LoadControlDefault();
                }
                else
                {
                    SetControlByData();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }

        }

        private void LoadControlDefault()
        {            
            ttbDesc.Enabled = false;
            csReason.Setup(ProgramRight, "ALL", "Default", ReasonMode.Category);
            if (csReason.IsNull)
                throw new RuleCimesException(TextMessage.Error.T00642(ProgramRight, "ALL", "Default", ReasonMode.Category.ToString()));

            if (ttbOldState.Text.Trim() != "")
            {
                #region P_GET_CHAGNESTATE_EQPSTATE
                List<EquipmentStateInfo> eqpState = RuleExtendManager.GetChangeStateEqpState(ttbOldState.Text.Trim());
                eqpState.Sort(p => p.State);
                #endregion
                ddlNewState.DataSource = eqpState;
            }

            ddlNewState.DataTextField = "State";
            ddlNewState.DataValueField = "State";
            ddlNewState.DataBind();
            ddlNewState.Items.Insert(0, string.Empty);
            ddlNewState.Enabled = false;            

            if (Request["NewStatus"].IsNullOrEmpty())
            {
                //[01597]傳入的參數{0}為空值，無法提供所需數據!
                throw new RuleCimesException(TextMessage.Error.T01597("NewStatus"));
            }
            var item = ddlNewState.Items.FindByValue(Request["NewStatus"].ToString());
            if (item != null)
            {
                ddlNewState.ClearSelection();
                item.Selected = true;
            }            
        }

        private void SetControlByData()
        {
            ttbDesc.Enabled = csReason.Validated;
        }

        private void ClearField()
        {
            csReason.ClearSelection();
            ddlNewState.SelectedIndex = -1;
            ttbOldState.Text = "";
            ttbDesc.Text = "";
            ttbDesc.Enabled = false;
            btnOK.Enabled = false;
            mEQP = null;
        }

        private void LoadControlByEquip(EquipmentInfo equipmentData)
        {
            try
            {
                if (_SAIEQPChangeStateList.Find(p => p.Remark02 == equipmentData.CurrentState) == null)
                {
                    //[00555]查無資料，請至系統資料維護新增類別{0}、項目{1}!
                    throw new RuleCimesException(TextMessage.Error.T00555("SAIEQPChangeState", "Remark01" + ":" +ProgramRight + ",Remark02" + ":" + equipmentData.CurrentState));
                }
                mEQP = equipmentData;
                ttbOldState.Text = equipmentData.CurrentState;
                btnOK.Enabled = true;
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            ciEquipment.BeginVerifyIng += new Ares.Cimes.IntelliService.Web.UI.CimesInput.BeginVerifyIngEventHandler(ciEquipment_BeginVerifyIng);
            ciEquipment.CimesTextChanged += new Ares.Cimes.IntelliService.Web.UI.CimesInput.TextChangedEventHandler(ciEquipment_CimesTextChanged);
            base.OnInit(e);
        }

        void ciEquipment_BeginVerifyIng(object sender)
        {
            try
            {
                ClearField();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        void ciEquipment_CimesTextChanged(object sender, Ares.Cimes.IntelliService.Web.UI.CimesTextChangedEventArgs e)
        {
            try
            {
                ciEquipment.LabelText = lblEquipment.Text;
                e.Must(ciEquipment, lblEquipment);

                LoadControlByEquip(e.InfoData.ChangeTo<EquipmentInfo>());
                SetEquipmentCookie(e.NewValue);
                LoadControlDefault();
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
                EquipmentInfo equipmentData = EquipmentInfo.GetEquipmentByName(mEQP.EquipmentName);

                if (mEQP.Tag != equipmentData.Tag)
                {
                    throw new RuleCimesException(TextMessage.Error.T00747(""));
                }

                string EqpID = ciEquipment.Must(lblEquipment);
                string Reason = csReason.Must(lblReasonCode).Value;
                EquipmentStateInfo equipmentState = EquipmentStateInfo.GetEquipmentStateByState(ddlNewState.Must(lblNewState).Text);

                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, this.ApplicationName);
                var rcData = csReason.GetBusinessReason().CategoryReason;
                txnStamp.CategoryReasonCode = rcData;
                txnStamp.Description = ttbDesc.Text.Trim();

                #region 檢查是否有設定機台變更狀態警報，若有，則發送警報
                //使用Function取得ALM設定，可彈性使用
                //取得機台狀態警報設定，預設是抓取系統資料設定[EquipStateAlarm]
                AlarmTypeInfo AlarmType = RuleExtendManager.GetEquipmentChangeStateAlarm(equipmentData, ddlNewState.Must(lblNewState).Text);
                string ALMsg = @"[Subject:{0}][Content:{1}:{2};   {3}:{4};   {5}:{6};   {7}:{8};   {9}:{10}]";
                if (AlarmType != null)
                {
                    // "[Content:TEST][Subject:TEST]"
                    ALMsg = string.Format(ALMsg, ProgramInformationBlock1.Caption, lblEquipment.Text, mEQP.EquipmentName, lblOldState.Text, mEQP.CurrentState, lblNewState.Text, equipmentState.State, lblReasonCode.Text, rcData.Reason, lblDescription.Text, txnStamp.Description);
                }
                #endregion

                using (var cts = CimesTransactionScope.Create())
                {
                    EMSTransaction.ChangeState(equipmentData, equipmentState, txnStamp);
                    RuleExtendManager.EquipmentTxnEnd(equipmentData);
                    cts.Complete();
                }
                
                #region 發送警報
                if (AlarmType != null)
                {                                        
                    AlarmService WebServiceALM = new AlarmService();
                    
                    string url = "http://" + Request.Url.Host + Request.ApplicationPath + "/ALM/Services/AlarmService.asmx";

                    WebServiceALM.Url = url;
                    string ReturnMsg = WebServiceALM.LaunchAlarm(AlarmType.GroupRights, AlarmType.TYPE, ALMsg, txnStamp.RecordTime, "UI", User.Identity.Name);
                }
                #endregion

                Response.Redirect(ciMes.Security.UserSetting.GetPreviousListPage(this), false);
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
                Response.Redirect(ciMes.Security.UserSetting.GetPreviousListPage(this), false);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}
