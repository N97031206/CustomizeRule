using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using ciMes.Web.Common;
using ciMes.Web.Common.UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.ToolRule
{
    public partial class T009Copy : CimesBasePage
    {
        string sql = "";
        int maxReturnQty = 100;

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        /// <summary>
        /// 資料清單
        /// </summary>
        private List<CSTToolDeviceDetailInfo> _ToolDeviceDetails
        {
            get { return this["_ToolDeviceDetails"] as List<CSTToolDeviceDetailInfo>; }
            set { this["_ToolDeviceDetails"] = value; }
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

        ///// <summary>
        ///// 傳入的SID
        ///// </summary>
        //private string _CurrentToolDeviceSID
        //{
        //    get
        //    {
        //        if (Request["ToolDeviceSID"].IsNullOrTrimEmpty())
        //        {
        //            //throw new Exception(TextMessage.Error.T00045(GetUIResource("ToolDeviceSID")));
        //        }

        //        return Request["ToolDeviceSID"];
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadControlDefault();

                    QueryData();
                }
                else
                {
                    gvQuery.SetDataSource(_ToolDeviceDetails, true);
                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        /// <summary>
        /// 取得設定資料
        /// </summary>
        private void QueryData()
        {
            _ToolDeviceDetails = CSTToolDeviceDetailInfo.GetDataListByDeviceAndEquipmantName(_CurrentDevice, _CurrentEquipment);

            gvQuery.SetDataSource(_ToolDeviceDetails, true);
        }

        private void LoadControlDefault()
        {
            btnCopy.Enabled = false;

            ttbSourceEquipment.Text = _CurrentEquipment;
            ttbSourceDevice.Text = _CurrentDevice;

            ddlProduct.Items.Clear();
            ddlDevice.Items.Clear();
            ddlEquipGroup.Items.Clear();
            ddlEquipment.Items.Clear();

            LoadProductControl();

            //帶出已選產品
            if (Session["ToolProduct"].ToCimesString().IsNotEmpty())
            {
                ListItem liItem = ddlProduct.Items.FindByText(Session["ToolProduct"].ToCimesString());
                if (liItem != null)
                {
                    ddlProduct.SelectedIndex = ddlProduct.Items.IndexOf(liItem);
                    ddlProduct_SelectedIndexChanged(ddlProduct, EventArgs.Empty);

                    //帶出已選料號
                    liItem = ddlDevice.Items.FindByText(Session["ToolDevice"].ToCimesString());
                    if (liItem != null)
                    {
                        ddlDevice.SelectedIndex = ddlDevice.Items.IndexOf(liItem);
                        ddlDevice_SelectedIndexChanged(ddlDevice, EventArgs.Empty);
                    }
                }
            }

            LoadEquipGroupControl();
        }

        private void LoadProductControl()
        {
            ProductInfo product = null;
            ddlProduct.Items.Clear();
            ttbProductFilter.Text = ttbProductFilter.Text.Trim();

            if (ttbProductFilter.Text.IsNullOrTrimEmpty() == false)
            {
                product = ProductInfo.GetProductByName(ttbProductFilter.Text);
            }

            if (product != null)
            {
                ddlProduct.Items.Add(new ListItem(product.ProductName, product.ProductName));
                ddlProduct_SelectedIndexChanged(ddlProduct, EventArgs.Empty);
            }
            else
            {
                List<string> pars = new List<string>();

                sql = @" SELECT {0} FROM MES_PRC_PROD P";

                if (ttbProductFilter.Text.IsNullOrTrimEmpty() == false)
                {
                    sql += @" WHERE P.PRODUCT LIKE #[STRING]";
                    pars.Add(ttbProductFilter.Text + "%");
                }

                //sql += @" ORDER BY P.PRODUCT ";

                //取得產品
                if (DBCenter.GetSingleResult<int>(string.Format(sql, "COUNT(P.PRODUCT)"), pars.ToArray()) > maxReturnQty)
                {
                    ddlProduct.Items.Add(new ListItem(GetUIResource("InputRequired", lblProduct.Text), string.Empty));
                }
                else
                {
                    List<ProductInfo> ProductList = InfoCenter.GetList<ProductInfo>(string.Format(sql + " ORDER BY P.PRODUCT ", "P.PRODUCT"), pars.ToArray());
                    ddlProduct.DataSource = ProductList;
                    ddlProduct.DataTextField = "ProductName";
                    ddlProduct.DataValueField = "ProductName";
                    ddlProduct.DataBind();

                    if (ProductList.Count == 0)
                        ddlProduct.Items.Add(new ListItem(TextMessage.Error.T00550(""), string.Empty));

                    if (ProductList.Count == 1)
                        ddlProduct_SelectedIndexChanged(ddlProduct, EventArgs.Empty);

                    if (ProductList.Count > 1)
                        ddlProduct.Items.Insert(0, "");
                }
            }
        }

        private void LoadDeviceControl()
        {
            ttbDeviceFilter.Text = ttbDeviceFilter.Text.Trim();
            ddlDevice.Items.Clear();
            DeviceVersionInfo device = null;

            if (ttbDeviceFilter.Text.IsNullOrTrimEmpty() == false)
                device = DeviceVersionInfo.GetActiveDeviceVersion(ttbDeviceFilter.Text);

            if (device != null)
            {
                ddlProduct.Items.Clear();
                ddlProduct.Items.Insert(0, new ListItem(device.ProductName, device.ProductName));
                ddlDevice.Items.Add(new ListItem(device.DeviceName, device.DeviceName));
                ddlDevice_SelectedIndexChanged(ddlDevice, EventArgs.Empty);
            }
            else
            {
                if (ddlProduct.SelectedItem.Text.IsNullOrTrimEmpty() == true)
                    return;

                List<string> paras = new List<string>();
                sql = @"SELECT {0} FROM MES_PRC_DEVICE_VER V WHERE V.REVSTATE = 'ACTIVE' AND V.PRODUCT = #[STRING] ";
                paras.Add(ddlProduct.SelectedItem.Value);

                if (ttbDeviceFilter.Text.IsNullOrTrimEmpty() == false)
                {
                    sql += " AND V.DEVICE LIKE #[STRING] ";
                    paras.Add(ttbDeviceFilter.Text + "%");
                }

                //sql += " ORDER BY V.DEVICE";

                if (DBCenter.GetSingleResult<int>(string.Format(sql, "COUNT(V.DEVICE)"), paras.ToArray()) > maxReturnQty)
                {
                    ddlDevice.Items.Add(new ListItem(GetUIResource("InputRequired", lblDevice.Text), string.Empty));
                }
                else
                {
                    List<DeviceVersionInfo> DeviceList = InfoCenter.GetList<DeviceVersionInfo>(string.Format(sql + " ORDER BY V.DEVICE", "V.DEVICE"), paras.ToArray());

                    ddlDevice.DataSource = DeviceList;
                    ddlDevice.DataTextField = "DeviceName";
                    ddlDevice.DataValueField = "DeviceName";
                    ddlDevice.DataBind();

                    if (DeviceList.Count == 0)
                        ddlDevice.Items.Add(new ListItem(TextMessage.Error.T00550(""), string.Empty));

                    if (DeviceList.Count == 1)
                        ddlDevice_SelectedIndexChanged(ddlDevice, EventArgs.Empty);

                    if (DeviceList.Count > 1)
                        ddlDevice.Items.Insert(0, "");
                }
            }
        }

        private void LoadEquipGroupControl()
        {
            EquipmentGroupInfo equipGroup = null;
            ddlEquipGroup.Items.Clear();
            ttbEquipGroupFilter.Text = ttbEquipGroupFilter.Text.Trim();

            if (ttbEquipGroupFilter.Text.IsNullOrTrimEmpty() == false)
            {
                equipGroup = EquipmentGroupInfo.GetEquipmentGroupByName(ttbEquipGroupFilter.Text);
            }

            if (equipGroup != null)
            {
                ddlEquipGroup.Items.Add(new ListItem(equipGroup.GroupName, equipGroup.ID));
                ddlEquipGroup_SelectedIndexChanged(ddlEquipGroup, EventArgs.Empty);
            }
            else
            {
                List<string> pars = new List<string>();

                sql = @" SELECT {0} FROM MES_EQP_GROUP P";

                if (ttbEquipGroupFilter.Text.IsNullOrTrimEmpty() == false)
                {
                    sql += @" WHERE P.GROUPNAME LIKE #[STRING]";
                    pars.Add(ttbEquipGroupFilter.Text + "%");
                }

                //取得機台群組
                if (DBCenter.GetSingleResult<int>(string.Format(sql, "COUNT(P.GROUPNAME)"), pars.ToArray()) > maxReturnQty)
                {
                    ddlEquipGroup.Items.Add(new ListItem(GetUIResource("InputRequired", lblEquipment.Text), string.Empty));
                }
                else
                {

                    List<EquipmentGroupInfo> equipGroupList = InfoCenter.GetList<EquipmentGroupInfo>(string.Format(sql + " ORDER BY P.GROUPNAME ", "P.GROUPNAME, P.EQP_GROUP_SID"), pars.ToArray());
                    ddlEquipGroup.DataSource = equipGroupList;
                    ddlEquipGroup.DataTextField = "GroupName";
                    ddlEquipGroup.DataValueField = "EquipmentGroupSID";
                    ddlEquipGroup.DataBind();

                    if (equipGroupList.Count == 0)
                        ddlEquipGroup.Items.Add(new ListItem(TextMessage.Error.T00550(""), string.Empty));

                    if (equipGroupList.Count == 1)
                        ddlEquipGroup_SelectedIndexChanged(ddlEquipGroup, EventArgs.Empty);

                    if (equipGroupList.Count > 1)
                        ddlEquipGroup.Items.Insert(0, "");
                }
            }
        }

        private void LoadEquipmentControl()
        {
            ttbEquipmentFilter.Text = ttbEquipmentFilter.Text.Trim();
            ddlEquipment.Items.Clear();
            EquipmentInfo equipmant = null;

            if (ttbEquipmentFilter.Text.IsNullOrTrimEmpty() == false)
                equipmant = EquipmentInfo.GetEquipmentByName(ttbEquipmentFilter.Text);

            if (equipmant != null)
            {
                ddlEquipGroup.Items.Clear();
                //ddlEquipGroup.Items.Insert(0, new ListItem(ddlEquipment.ProductName, device.ProductName));
                ddlEquipment.Items.Add(new ListItem(equipmant.EquipmentName, equipmant.EquipmentName));
                ddlEquipment_SelectedIndexChanged(ddlEquipment, EventArgs.Empty);
            }
            else
            {
                if (ddlEquipGroup.SelectedItem.Text.IsNullOrTrimEmpty() == true)
                    return;

                List<string> paras = new List<string>();
                sql = @"SELECT {0} FROM MES_EQP_GROUP_EQP G WHERE G.EQP_GROUP_SID = #[STRING] ";
                paras.Add(ddlEquipGroup.SelectedItem.Value);

                if (ttbEquipmentFilter.Text.IsNullOrTrimEmpty() == false)
                {
                    sql += " AND G.EQUIPMENT LIKE #[STRING] ";
                    paras.Add(ttbEquipmentFilter.Text + "%");
                }

                if (DBCenter.GetSingleResult<int>(string.Format(sql, "COUNT(G.EQUIPMENT)"), paras.ToArray()) > maxReturnQty)
                {
                    ddlEquipment.Items.Add(new ListItem(GetUIResource("InputRequired", lblEquipment.Text), string.Empty));
                }
                else
                {
                    List<EquipGroupEquipInfo> equipmentList = InfoCenter.GetList<EquipGroupEquipInfo>(string.Format(sql + " ORDER BY G.EQUIPMENT", "G.EQUIPMENT"), paras.ToArray());

                    ddlEquipment.DataSource = equipmentList;
                    ddlEquipment.DataTextField = "EquipmentName";
                    ddlEquipment.DataValueField = "EquipmentName";
                    ddlEquipment.DataBind();

                    if (equipmentList.Count == 0)
                        ddlEquipment.Items.Add(new ListItem(TextMessage.Error.T00550(""), string.Empty));

                    if (equipmentList.Count == 1)
                        ddlEquipment_SelectedIndexChanged(ddlEquipment, EventArgs.Empty);

                    if (equipmentList.Count > 1)
                        ddlEquipment.Items.Insert(0, "");
                }
            }
        }

        protected void ddlDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDevice.Must(lblDevice);

                if (ddlEquipment.Items.Count > 0)
                {
                    if (ddlEquipment.SelectedItem.Value.IsNullOrTrimEmpty() == false)
                    {
                        btnCopy.Enabled = true;
                    }
                    else
                    {
                        btnCopy.Enabled = false;
                    }
                }
                else
                {
                    btnCopy.Enabled = false;
                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }


        protected void ddlEquipGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlEquipment.Items.Clear();

                LoadEquipmentControl();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void ddlEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlEquipment.Must(lblEquipment);

                if (ddlDevice.Items.Count > 0)
                {
                    if (ddlDevice.SelectedItem.Value.IsNullOrTrimEmpty() == false)
                    {
                        btnCopy.Enabled = true;
                    }
                    else
                    {
                        btnCopy.Enabled = false;
                    }
                }
                else
                {
                    btnCopy.Enabled = false;
                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void ttbProductFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadProductControl();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

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

        protected void ttbEquipGroupFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadEquipGroupControl();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void ttbDeviceFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDeviceControl();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void ttbEquipmentFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadEquipmentControl();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlProduct.Must(lblProduct);
                ddlDevice.Items.Clear();

                LoadDeviceControl();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
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

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                //取得登入者資訊
                var recordTime = DBCenter.GetSystemTime();
                var userID = User.Identity.Name;

                //確認目標料號及機台編號是否已經存在資料
                var toolDevices = CSTToolDeviceInfo.GetDataListByDeviceAndEquipmantName(ddlDevice.SelectedItem.Value, ddlEquipment.SelectedItem.Value);
                if (toolDevices.Count > 0)
                {
                    //料號({0})及機台({1}) 資料已存在，不可執行複製動作!
                    throw new Exception(RuleMessage.Error.C10117(ddlDevice.SelectedItem.Value, ddlEquipment.SelectedItem.Value));
                }

                using (var cts = CimesTransactionScope.Create())
                {
                    //新增一筆CST_TOOL_DEVICE資料
                    var toolDevice = InfoCenter.Create<CSTToolDeviceInfo>();
                    toolDevice.DeviceName = ddlDevice.SelectedItem.Value;
                    toolDevice.EquipmentName = ddlEquipment.SelectedItem.Value;
                    toolDevice.Tag = 1;

                    toolDevice.InsertToDB(userID, recordTime);
                    LogCenter.LogToDB(toolDevice, LogCenter.LogIndicator.Create(ActionType.Add, userID, recordTime));

                    //複製CST_TOOL_DEVICE_DETAIL資料
                    _ToolDeviceDetails.ForEach(toolDeviceDetail =>
                    {
                        var newToolDeviceDetail = InfoCenter.Create<CSTToolDeviceDetailInfo>();
                        newToolDeviceDetail.EquipmentName = toolDevice.EquipmentName;
                        newToolDeviceDetail.Quantity = toolDeviceDetail.Quantity;
                        newToolDeviceDetail.DeviceName = toolDevice.DeviceName;
                        newToolDeviceDetail.ToolType = toolDeviceDetail.ToolType;
                        newToolDeviceDetail.ToolDeviceSID = toolDevice.ToolDeviceSID;

                        newToolDeviceDetail.InsertToDB();

                        LogCenter.LogToDB(newToolDeviceDetail, LogCenter.LogIndicator.Create(ActionType.Add, userID, recordTime));
                    });

                    cts.Complete();
                }

                //INF-00002:{0}儲存成功！
                _ProgramInformationBlock.ShowMessage(TextMessage.Hint.T00083(""), MessageShowOptions.OnLabel);

                LoadControlDefault();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }
    }
}