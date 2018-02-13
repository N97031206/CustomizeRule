using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using ciMes.Web.Common;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common.UserControl;
using CustomizeRule.RuleUtility;

namespace CustomizeRule.ToolRule
{

    public partial class T015 : CustomizeRuleBasePage
    {
        private EquipmentInfo _EquipmentInfo
        {
            get
            {
                return (EquipmentInfo)this["_EquipmentInfo"];
            }
            set
            {
                this["_EquipmentInfo"] = value;
            }
        }

        private DeviceInfo _DeviceInfo1
        {
            get
            {
                return (DeviceInfo)this["_DeviceInfo1"];
            }
            set
            {
                this["_DeviceInfo1"] = value;
            }
        }

        private DeviceInfo _DeviceInfo2
        {
            get
            {
                return (DeviceInfo)this["_DeviceInfo2"];
            }
            set
            {
                this["_DeviceInfo2"] = value;
            }
        }

        private DataTable _dtVerifiy1
        {
            get
            {
                return (DataTable)this["_dtVerifiy1"];
            }
            set
            {
                this["_dtVerifiy1"] = value;
            }
        }

        private DataTable _dtVerifiy2
        {
            get
            {
                return (DataTable)this["_dtVerifiy2"];
            }
            set
            {
                this["_dtVerifiy2"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ClearField();

                    LoadDefaultControl();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void LoadDefaultControl()
        {
            lblDevice1.Text += "1";
            lblDevice2.Text += "2";
        }

        private void ClearField()
        {
            ttbEquipmentName.Text = "";
            ttbDevice1.Text = "";
            ttbDevice2.Text = "";
            gvVerifiy1.SetDataSource(null, true);
            gvVerifiy2.SetDataSource(null, true);
            ClearData();
        }

        private void ClearData()
        {
            _EquipmentInfo = null;
            _DeviceInfo1 = null;
            _DeviceInfo2 = null;
            _dtVerifiy1 = null;
            _dtVerifiy2 = null;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                // 返回在製品查詢頁面
                ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ttbEquipmentName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ttbEquipmentName.Must(lblEquipment);
                string sEquipment = ttbEquipmentName.Text.Trim();

                _EquipmentInfo = EquipmentInfo.GetEquipmentByName(sEquipment);

                if (_EquipmentInfo == null)
                {
                    ttbEquipmentName.Text = "";
                    AjaxFocus(ttbEquipmentName);
                    throw new RuleCimesException(TextMessage.Error.T00030(lblEquipment.Text, sEquipment));
                }

                AjaxFocus(ttbDevice1);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ttbDevice1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //資料檢查
                ttbEquipmentName.Must(lblEquipment);
                ttbDevice1.Must(lblDevice1);

                string sDeviceName = ttbDevice1.Text.Trim();
                _DeviceInfo1 = DeviceInfo.GetDeviceByName(sDeviceName);

                if (_DeviceInfo1 == null)
                {
                    ttbDevice1.Text = "";
                    throw new RuleCimesException(TextMessage.Error.T00030(lblDevice1.Text, sDeviceName), ttbDevice1);
                }

                if (_DeviceInfo2 != null && _DeviceInfo1.DeviceName == _DeviceInfo2.DeviceName)
                {
                    ttbDevice1.Text = "";
                    throw new RuleCimesException(RuleMessage.Error.T00078(lblDevice1.Text, lblDevice2.Text), ttbDevice1);
                }

                _dtVerifiy1 = GetToolUseStatus(_EquipmentInfo.EquipmentName, _DeviceInfo1.DeviceName);
                if (_dtVerifiy1.Rows.Count == 0)
                {
                    ttbDevice1.Text = "";
                    _dtVerifiy1 = null;
                    throw new RuleCimesException(RuleMessage.Error.C10140(_EquipmentInfo.EquipmentName, _DeviceInfo1.DeviceName), ttbDevice1);
                }

                gvVerifiy1.DataSource = _dtVerifiy1;
                gvVerifiy1.DataBind();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvVerifiy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                var data = e.Row.DataItem as DataRowView;
                if (data["NEEDQTY"].ToDecimal() != data["EQPTOOLCOUNT"].ToDecimal())
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private DataTable GetToolUseStatus(string equipment, string device)
        {        
            return DBCenter.GetDataTable(@"SELECT TOOLTYPE,
         SUM (NEEDQTY) AS NEEDQTY,
         SUM (EQPTOOLCOUNT) AS EQPTOOLCOUNT,
         OPERATION
    FROM (  SELECT MES_TOOL_MAST.TOOLTYPE,
                   0 AS NEEDQTY,
                   COUNT (1) AS EQPTOOLCOUNT,
                   USERDEFINECOL08 OPERATION
              FROM MES_EQP_TOOL
                   INNER JOIN MES_TOOL_MAST
                      ON (MES_EQP_TOOL.TOOLNAME = MES_TOOL_MAST.TOOLNAME)
             WHERE MES_EQP_TOOL.EQUIPMENT = #[STRING]
          GROUP BY MES_TOOL_MAST.TOOLTYPE, MES_TOOL_MAST.USERDEFINECOL08
          UNION ALL
            SELECT TOOLTYPE,
                   SUM (QUANTITY) AS NEEDQTY,
                   0 AS EQPTOOLCOUNT,
                   OPERATION
              FROM CST_TOOL_DEVICE_DETAIL
             WHERE EQP = #[STRING] AND DEVICE = #[STRING]
          GROUP BY TOOLTYPE, OPERATION)
GROUP BY TOOLTYPE, OPERATION", equipment, equipment, device);
        }

        protected void ttbDevice2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //資料檢查
                ttbEquipmentName.Must(lblEquipment);
                ttbDevice2.Must(lblDevice2);

                string sDeviceName = ttbDevice2.Text.Trim();
                _DeviceInfo2 = DeviceInfo.GetDeviceByName(sDeviceName);

                if (_DeviceInfo2 == null)
                {
                    ttbDevice2.Text = "";
                    AjaxFocus(ttbDevice2);
                    throw new RuleCimesException(TextMessage.Error.T00030(lblDevice2.Text, sDeviceName), ttbDevice2);
                }

                if (_DeviceInfo1 != null && _DeviceInfo1.DeviceName == _DeviceInfo2.DeviceName)
                {
                    ttbDevice2.Text = "";
                    throw new RuleCimesException(RuleMessage.Error.T00078(lblDevice1.Text, lblDevice2.Text), ttbDevice2);
                }

                _dtVerifiy2 = GetToolUseStatus(_EquipmentInfo.EquipmentName, _DeviceInfo2.DeviceName);
                if (_dtVerifiy2.Rows.Count == 0)
                {
                    ttbDevice2.Text = "";
                    _dtVerifiy2 = null;
                    throw new RuleCimesException(RuleMessage.Error.C10140(_EquipmentInfo.EquipmentName, _DeviceInfo1.DeviceName), ttbDevice2);
                }

                gvVerifiy2.DataSource = _dtVerifiy2;
                gvVerifiy2.DataBind();
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
                ttbEquipmentName.Must(lblEquipment);
                ttbDevice1.Must(lblDevice1);
                
                //料號1一定要有資料
                if (_DeviceInfo1 == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00826(lblDevice1.Text));
                }

                //一定要輸入一種料號
                if (_dtVerifiy1 == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00826(lblDevice1.Text));
                }

                //若有輸入料號1則需驗證應領數量與領用數量是否相同
                if (_dtVerifiy1 != null)
                {
                    _dtVerifiy1.Rows.LoopDo<DataRow>((p, i) =>
                    {
                        if (p["NEEDQTY"].ToDecimal() != p["EQPTOOLCOUNT"].ToDecimal())
                        {
                            throw new RuleCimesException(RuleMessage.Error.C10141(_EquipmentInfo.EquipmentName, _DeviceInfo1.DeviceName, p["TOOLTYPE"].ToString(), p["NEEDQTY"].ToString()));
                        }
                    });
                }

                //若有輸入料號2則需驗證應領數量與領用數量是否相同
                if (_dtVerifiy2 != null)
                {
                    _dtVerifiy2.Rows.LoopDo<DataRow>((p, i) =>
                    {
                        if (p["NEEDQTY"].ToDecimal() != p["EQPTOOLCOUNT"].ToDecimal())
                        {
                            throw new RuleCimesException(RuleMessage.Error.C10141(_EquipmentInfo.EquipmentName, _DeviceInfo1.DeviceName, p["TOOLTYPE"].ToString(), p["NEEDQTY"].ToString()));
                        }
                    });
                }

                string sReleaseID = DBCenter.GetSystemID();
                TransactionStamp txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    //先將舊的資料刪除
                    var lstEqpToolRelease = CSTEquipmentToolReleaseInfo.GetDataByEquipment(_EquipmentInfo.EquipmentName);
                    lstEqpToolRelease.ForEach(p => {
                        p.DeleteFromDB();
                        LogCenter.LogToDB(p, LogCenter.LogIndicator.Create(ActionType.Remove, txnStamp.UserID, txnStamp.RecordTime));
                    });

                    var lstEqpTool = EquipToolInfo.GetByEquipmentName(_EquipmentInfo.EquipmentName);
                    lstEqpTool.ForEach(p => {
                        //取得配件資訊
                        var toolInfo = ToolInfo.GetToolByName(p.ToolName);
                        //新增至CST_EQP_TOOL_RELEASE的Info物件
                        var eqpToolReleaseInfo = InfoCenter.Create<CSTEquipmentToolReleaseInfo>();
                        eqpToolReleaseInfo.EquipmentName = _EquipmentInfo.EquipmentName;
                        eqpToolReleaseInfo.ToolName = toolInfo.ToolName;
                        eqpToolReleaseInfo.HEAD = toolInfo["HEAD"].ToString();
                        eqpToolReleaseInfo.RELEASEID = sReleaseID;
                        eqpToolReleaseInfo["OPERATION"] = toolInfo.UserDefineColumn08;
                        eqpToolReleaseInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                        LogCenter.LogToDB(eqpToolReleaseInfo, LogCenter.LogIndicator.Create(ActionType.Add, txnStamp.UserID, txnStamp.RecordTime));
                        //異動Tool的RELEASEID欄位
                        TMSTransaction.ModifyToolSystemAttribute(toolInfo, "RELEASEID", sReleaseID, txnStamp);
                    });

                    //先將舊的資料刪除
                    var lstEqpDeviceRelease = CSTEquipmentDeviceReleaseInfo.GetDataByEquipment(_EquipmentInfo.EquipmentName);
                    lstEqpDeviceRelease.ForEach(p => {
                        p.DeleteFromDB();
                        LogCenter.LogToDB(p, LogCenter.LogIndicator.Create(ActionType.Remove, txnStamp.UserID, txnStamp.RecordTime));
                    });

                    if (_dtVerifiy1 != null)
                    {
                        //新增至CST_EQP_DEVICE_RELEASE的Info物件
                        var eqpDeviceReleaseInfo = InfoCenter.Create<CSTEquipmentDeviceReleaseInfo>();
                        eqpDeviceReleaseInfo.EquipmentName = _EquipmentInfo.EquipmentName;
                        eqpDeviceReleaseInfo.DeviceName = _DeviceInfo1.DeviceName;
                        eqpDeviceReleaseInfo.RELEASEID = sReleaseID;
                        eqpDeviceReleaseInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                        LogCenter.LogToDB(eqpDeviceReleaseInfo, LogCenter.LogIndicator.Create(ActionType.Add, txnStamp.UserID, txnStamp.RecordTime));
                    }

                    if (_dtVerifiy2 != null)
                    {
                        //新增至CST_EQP_DEVICE_RELEASE的Info物件
                        var eqpDeviceReleaseInfo = InfoCenter.Create<CSTEquipmentDeviceReleaseInfo>();
                        eqpDeviceReleaseInfo.EquipmentName = _EquipmentInfo.EquipmentName;
                        eqpDeviceReleaseInfo.DeviceName = _DeviceInfo2.DeviceName;
                        eqpDeviceReleaseInfo.RELEASEID = sReleaseID;
                        eqpDeviceReleaseInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                        LogCenter.LogToDB(eqpDeviceReleaseInfo, LogCenter.LogIndicator.Create(ActionType.Add, txnStamp.UserID, txnStamp.RecordTime));
                    }
                    cts.Complete();
                }

                ((ProgramInformationBlock)ProgramInformationBlock1).ShowMessage(TextMessage.Hint.T00057(GetUIResource("ToolVerifiy")));
                ClearField();
                AjaxFocus(ttbEquipmentName);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}