using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Web.UI;
using ciMes.Web.Common.UserControl;
using CustomizeRule.RuleUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.ToolRule
{
    public partial class T009 : CustomizeRuleBasePage
    {
        string sql = "";
        int maxReturnQty = 100;

        /// <summary>
        /// 資料清單
        /// </summary>
        private List<CSTToolDeviceInfo> _ToolDevices
        {
            get { return this["_ToolDevices"] as List<CSTToolDeviceInfo>; }
            set { this["_ToolDevices"] = value; }
        }

        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
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
                    gvQuery.SetDataSource(_ToolDevices, true);
                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        private void ClearData()
        {
            ddlDevice.Items.Clear();

            _ToolDevices = new List<CSTToolDeviceInfo>();

            gvQuery.SetDataSource(_ToolDevices, true);
        }

        /// <summary>
        /// 載入控制項的初始值
        /// </summary>
        private void LoadControlDefault()
        {
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
        /// 取得配件工作站設定Tool資料
        /// </summary>
        private void QueryData()
        {
            string deviceName = "";
            string equipmentName = "";

            if (ddlDevice.Items.Count != 0)
            {
                deviceName = ddlDevice.SelectedItem.Value;
            }

            if (ddlEquipment.Items.Count != 0)
            {
                equipmentName = ddlEquipment.SelectedItem.Value;
            }

            if (deviceName.IsNullOrTrimEmpty() && equipmentName.IsNullOrTrimEmpty())
            {
                _ToolDevices = new List<CSTToolDeviceInfo>();
            }
            else if (deviceName.IsNullOrTrimEmpty() && equipmentName.IsNullOrTrimEmpty() == false)
            {
                _ToolDevices = CSTToolDeviceInfo.GetDataListByEquipmentName(equipmentName);
            }
            else if (deviceName.IsNullOrTrimEmpty() == false && equipmentName.IsNullOrTrimEmpty())
            {
                _ToolDevices = CSTToolDeviceInfo.GetDataListByDeviceName(deviceName);
            }
            else if (deviceName.IsNullOrTrimEmpty() == false && equipmentName.IsNullOrTrimEmpty() == false)
            {
                _ToolDevices = CSTToolDeviceInfo.GetDataListByDeviceAndEquipmantName(deviceName, equipmentName);
            }

            gvQuery.SetDataSource(_ToolDevices, true);

        }

        protected void gvQuery_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                int dataItemIndex = e.Row.DataItemIndex;
                CimesGridView gvTool = (CimesGridView)e.Row.FindControl("gvTool");

                //取得此筆主檔對應的明細資料
                var toolDeviceDetails = CSTToolDeviceDetailInfo.GetDataListByToolDeviceSID(_ToolDevices[dataItemIndex].ToolDeviceSID);

                gvTool.SetDataSource(toolDeviceDetails, true);
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void gvQuery_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                int dataIndex = gvQuery.Rows[e.NewSelectedIndex].DataItemIndex;

                var toolDevice = _ToolDevices[dataIndex];

                string src = string.Format("T009Set.aspx?SecurityRightSID={0}&Equipment={1}&Device={2}&ToolDeviceSID={3}",
                    ProgramRightSID, toolDevice.EquipmentName, toolDevice.DeviceName, toolDevice.ToolDeviceSID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenDialog", String.Format("OpenjQueryDialog('{0}',1060,600);", src), true);
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
                //btnCopy.Enabled = _ToolDevices.Count > 0;
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

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

        protected void gvQuery_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int dataIndex = gvQuery.Rows[e.RowIndex].DataItemIndex;

                //取得登入者資訊
                var recordTime = DBCenter.GetSystemTime();
                var userID = User.Identity.Name;

                using (CimesTransactionScope cts = CimesTransactionScope.Create())
                {
                    //刪除主檔資料
                    _ToolDevices[dataIndex].DeleteFromDB();

                    LogCenter.LogToDB(_ToolDevices[dataIndex], LogCenter.LogIndicator.Create(ActionType.Remove, userID, recordTime));

                    //取得明細資料
                    var toolDeviceDetails = CSTToolDeviceDetailInfo.GetDataListByToolDeviceSID(_ToolDevices[dataIndex].ToolDeviceSID);
                    toolDeviceDetails.ForEach(toolDeviceDetail =>
                    {
                        //刪除明細資料
                        toolDeviceDetail.DeleteFromDB();

                        LogCenter.LogToDB(toolDeviceDetail, LogCenter.LogIndicator.Create(ActionType.Remove, userID, recordTime));

                    });

                    cts.Complete();
                }

                QueryData();
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
                ClearData();

                ddlProduct.Must(lblProduct);
                ddlDevice.Items.Clear();

                LoadDeviceControl();
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
                //確認產品是否有選擇
                ddlProduct.Must(lblProduct);

                //確認料號是否有選擇
                ddlDevice.Must(lblDevice);

                //確認機台群組是否有選擇
                ddlEquipGroup.Must(lblEquipGroup);

                //確認機台編號是否有選擇
                ddlEquipment.Must(lblEquipment);

                //確認是否有資料存在
                if (_ToolDevices.Count > 0)
                {
                    //此機台+料號已有設定資料，如要重新設定，請先刪除再新增設定資料!!
                    throw new Exception(RuleMessage.Error.C10154());
                }

                //SecurityRightSID是為了讓Popup的視窗，ProgramInfoBlock的Caption能顯示主視窗的標題
                string src = string.Format("T009Set.aspx?SecurityRightSID={0}&Equipment={1}&Device={2}",
                    ProgramRightSID, ddlEquipment.SelectedItem.Value, ddlDevice.SelectedItem.Value);

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenDialog", String.Format("OpenjQueryDialog('{0}',1060,600);", src), true);
            }
            catch (Exception E)
            {
                HandleError(E, MessageShowOptions.Both);
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //取得配件工作站設定Tool資料
                QueryData();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                //確認產品是否有選擇
                ddlProduct.Must(lblProduct);

                //確認料號是否有選擇
                ddlDevice.Must(lblDevice);

                //確認機台群組是否有選擇
                ddlEquipGroup.Must(lblEquipGroup);

                //確認機台編號是否有選擇
                ddlEquipment.Must(lblEquipment);

                //確認是否有資料存在
                if (_ToolDevices.Count == 0)
                {
                    //料號({0})及機台({1}) 查無任何資訊，不可執行複製動作!
                    throw new Exception(RuleMessage.Error.C10116(ddlDevice.SelectedItem.Value, ddlEquipment.SelectedItem.Value));
                }

                //SecurityRightSID是為了讓Popup的視窗，ProgramInfoBlock的Caption能顯示主視窗的標題
                string src = string.Format("T009Copy.aspx?SecurityRightSID={0}&Equipment={1}&Device={2}&ToolDeviceSID={3}",
                    ProgramRightSID, ddlEquipment.SelectedItem.Value, ddlDevice.SelectedItem.Value, _ToolDevices[0].ToolDeviceSID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenDialog", String.Format("OpenjQueryDialog('{0}',1060,600);", src), true);
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        protected void ddlDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ddlDevice.Must(lblDevice);

                QueryData();
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
                QueryData();
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }
    }
}