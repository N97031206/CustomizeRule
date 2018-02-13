/*
  資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  

  程式命名：ToolOperSet.aspx
	 
  說明：配件工作站設定
	    設定工程資料參數，可新增、刪除、編輯
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;
using System.Data.OleDb;
using System.Text;
using System.Text.RegularExpressions;

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Function;
using Ares.Cimes.IntelliService.Transaction;
using ciMes.Web.Common;
using Ares.Cimes.IntelliService.DbTableSchema;
using Ares.Cimes.IntelliService.DbTableRow;
using Ares.Cimes.IntelliService.Web.UI;
using ciMes.Web.Common.UserControl;

namespace CustomizeRule.ToolRule
{
    public partial class T009Set : CimesBasePage
    {
        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        /// <summary>
        /// 原始的資料清單
        /// </summary>
        private List<CSTToolDeviceDetailInfo> _SourceToolDeviceDetails
        {
            get { return this["_SourceToolDeviceDetails"] as List<CSTToolDeviceDetailInfo>; }
            set { this["_SourceToolDeviceDetails"] = value; }
        }

        /// <summary>
        /// 修改過的資料清單
        /// </summary>
        private List<CSTToolDeviceDetailInfo> _ModifyToolDeviceDetails
        {
            get { return this["_ModifyToolDeviceDetails"] as List<CSTToolDeviceDetailInfo>; }
            set { this["_ModifyToolDeviceDetails"] = value; }
        }

        /// <summary>
        /// 傳入的機台名稱
        /// </summary>
        private string _CurrentEquipment
        {
            get
            {
                if (Request["Equipment"].IsNullOrTrimEmpty())
                {
                    throw new Exception(TextMessage.Error.T00045(GetUIResource("Equipment")));
                }

                return Request["Equipment"];
            }
        }

        /// <summary>
        /// 傳入的料號
        /// </summary>
        private string _CurrentDevice
        {
            get
            {
                if (Request["Device"].IsNullOrTrimEmpty())
                {
                    throw new Exception(TextMessage.Error.T00045(GetUIResource("Device")));
                }

                return Request["Device"];
            }
        }

        /// <summary>
        /// 傳入的SID
        /// </summary>
        private string _CurrentToolDeviceSID
        {
            get
            {
                if (Request["ToolDeviceSID"].IsNullOrTrimEmpty())
                {
                    //throw new Exception(TextMessage.Error.T00045(GetUIResource("ToolDeviceSID")));
                }

                return Request["ToolDeviceSID"];
            }
        }

        protected override void OnInit(EventArgs e)
        {
            gvQuery.DataBound += gvQuery_DataBound;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ClearData();

                    LoadControlDefault();

                    QueryData();
                }
                else
                {
                    gvQuery.SetDataSource(_ModifyToolDeviceDetails, true);
                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        private void LoadControlDefault()
        {
            ttbQuantity.Text = "";
            ttbEquipment.Text = _CurrentEquipment;
            ttbDevice.Text = _CurrentDevice;

            //取得所有啟用之刀具類別
            var toolTypes = ToolTypeInfoEx.GetToolTypeByToolClass("CUTTER");
            ddlToolType.Items.Clear();
            ddlToolType.DataSource = toolTypes;
            ddlToolType.DataTextField = "Type";
            ddlToolType.DataValueField = "Type";
            ddlToolType.DataBind();
            ddlToolType.Items.Insert(0, "");

            var lstToolOperation = WpcExClassItemInfo.GetInfoByClass("SAIToolOperation");
            lstToolOperation.ForEach(oper => {
                ddlOperation.Items.Add(oper.Remark01);
            });
            ddlOperation.Items.Insert(0, "");
        }

        /// <summary>
        /// 清除所有項目
        /// </summary>
        private void ClearData()
        {
            _SourceToolDeviceDetails = new List<CSTToolDeviceDetailInfo>();
            _ModifyToolDeviceDetails = new List<CSTToolDeviceDetailInfo>();

            gvQuery.EditIndex = -1;
            gvQuery.SelectedIndex = -1;
            gvQuery.DataSource = null;
            gvQuery.DataBind();
        }

        /// <summary>
        /// 取得設定資料
        /// </summary>
        private void QueryData()
        {
            _SourceToolDeviceDetails = CSTToolDeviceDetailInfo.GetDataListByDeviceAndEquipmantName(_CurrentDevice, _CurrentEquipment);

            if (_SourceToolDeviceDetails.Count > 0)
            {
                _ModifyToolDeviceDetails.AddRange(new List<CSTToolDeviceDetailInfo>(_SourceToolDeviceDetails));
            }

            gvQuery.SetDataSource(_ModifyToolDeviceDetails, true);
        }

        /// <summary>
        /// 離開此畫面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenDialog", "javascript:window.parent.closeMasterWindow();", true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        void gvQuery_DataBound(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = _ModifyToolDeviceDetails.Count > 0;
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        //protected void gvQuery_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    try
        //    {
        //        gvQuery.EditIndex = e.NewEditIndex;
        //        gvQuery.DataBind();
        //    }
        //    catch (Exception E)
        //    {
        //        HandleError(E);
        //    }
        //}

        //protected void gvQuery_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception E)
        //    {
        //        HandleError(E);
        //    }
        //}

        protected void gvQuery_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = gvQuery.Rows[e.RowIndex].DataItemIndex;

                _ModifyToolDeviceDetails.RemoveAt(index);

                gvQuery.SetDataSource(_ModifyToolDeviceDetails, true);
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        //protected void gvQuery_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception E)
        //    {
        //        HandleError(E);
        //    }
        //}

        /// <summary>
        /// 畫面換頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvQuery_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvQuery.PageIndex = e.NewPageIndex;
                gvQuery.DataBind();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //確認是否有選擇刀具類別
                ddlToolType.Must(lblToolType);
                ddlOperation.Must(lblOperation);

                //確認選擇刀具類別是否已新增
                _ModifyToolDeviceDetails.ForEach(toolDeviceDetail =>
                {
                    if (toolDeviceDetail.ToolType == ddlToolType.SelectedValue && toolDeviceDetail.OperationName == ddlOperation.SelectedValue)
                    {
                        //刀具類型:{0} 資料已存在!
                        throw new Exception(RuleMessage.Error.C10121(ddlToolType.SelectedValue));
                    }
                });

                //確認數量是否為整數
                ttbQuantity.MustInt(lblQuantity);

                //確數量是否大於零 
                int value = Convert.ToInt32(ttbQuantity.Text);
                if (value <= 0)
                {
                    //[01397]數量必須大於零！
                    throw new Exception(TextMessage.Error.T01397());
                }

                //新增一筆資料
                var newToolDeviceDetail = InfoCenter.Create<CSTToolDeviceDetailInfo>();
                newToolDeviceDetail.EquipmentName = ttbEquipment.Text;
                newToolDeviceDetail.OperationName = ddlOperation.SelectedValue;
                newToolDeviceDetail.Quantity = Convert.ToDecimal(ttbQuantity.Text);
                newToolDeviceDetail.DeviceName = ttbDevice.Text;
                newToolDeviceDetail.ToolType = ddlToolType.SelectedValue;

                //加入清單
                _ModifyToolDeviceDetails.Insert(0, newToolDeviceDetail);

                gvQuery.SetDataSource(_ModifyToolDeviceDetails, true);

                //清除資料
                ddlToolType.ClearSelection();
                ttbQuantity.Text = "";
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //取得登入者資訊
                var recordTime = DBCenter.GetSystemTime();
                var userID = User.Identity.Name;

                //確認是否有資料
                if (_ModifyToolDeviceDetails.Count == 0)
                {
                    //請新增一筆刀具類型資料!
                    throw new Exception(RuleMessage.Error.C10122());
                }

                using (var cts = CimesTransactionScope.Create())
                {
                    CSTToolDeviceInfo toolDevice = null;

                    //如果傳入ToolDeviceSID為NULL，表示要新增一筆CST_TOOL_DEVICE資料
                    if (_CurrentToolDeviceSID.IsNullOrTrimEmpty())
                    {
                        toolDevice = InfoCenter.Create<CSTToolDeviceInfo>();
                        toolDevice.DeviceName = ttbDevice.Text;
                        toolDevice.EquipmentName = ttbEquipment.Text;
                        toolDevice.Tag = 1;

                        toolDevice.InsertToDB(userID, recordTime);
                        LogCenter.LogToDB(toolDevice, LogCenter.LogIndicator.Create(ActionType.Add, userID, recordTime));
                    }
                    else
                    {
                        toolDevice = InfoCenter.GetBySID<CSTToolDeviceInfo>(_CurrentToolDeviceSID);
                    }

                    #region 更新資料清單
                    _ModifyToolDeviceDetails.ForEach(deviceDetail =>
                    {
                        //註記資料LOG的狀態
                        ActionType deviceDetailActionType = new ActionType();

                        if (deviceDetail.InfoState == InfoState.NewCreate)
                        {
                            deviceDetail.ToolDeviceSID = toolDevice.ToolDeviceSID;

                            //新增資料
                            deviceDetail.InsertToDB(userID, recordTime);
                            deviceDetailActionType = ActionType.Add;
                        }
                        else if (deviceDetail.InfoState == InfoState.Modified)
                        {
                            //更改資料
                            deviceDetail.UpdateToDB(userID, recordTime);
                            deviceDetailActionType = ActionType.Set;
                            _SourceToolDeviceDetails.Remove(deviceDetail);
                        }
                        else if (deviceDetail.InfoState == InfoState.Unchanged)
                        {
                            _SourceToolDeviceDetails.Remove(deviceDetail);
                        }

                        //紀錄歷史紀錄[CST_TOOL_DEVICE_DETAIL_LOG]，有異動資料才更新
                        if (deviceDetailActionType != ActionType.None)
                        {
                            LogCenter.LogToDB(deviceDetail, LogCenter.LogIndicator.Create(deviceDetailActionType, userID, recordTime));
                        }
                    });
                    #endregion

                    #region 處理資料刪除的部份
                    _SourceToolDeviceDetails.ForEach(deviceDetail =>
                    {
                        deviceDetail.DeleteFromDB();

                        LogCenter.LogToDB(deviceDetail, LogCenter.LogIndicator.Create(ActionType.Remove, userID, recordTime));
                    });
                    #endregion

                    cts.Complete();
                }

                //INF-00002:{0}儲存成功！
                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00083(""), MessageShowOptions.OnLabel);

                ClearData();

                LoadControlDefault();

                QueryData();

                btnExit_Click(null, EventArgs.Empty);

            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }
    }
}