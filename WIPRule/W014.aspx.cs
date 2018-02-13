/*
------------------------------------------------------------------
資通電腦股份有限公司 ： ciMes – 工廠製造執行軟體  Ver 5.0.0
版權所有、禁止複製

模組：WIP
作者：Nick

功能說明：包裝作業。包裝方式分為：標準、混和及不卡控。
依照方式不同，執行不一樣的邏輯。

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
using System.Data;

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
    public partial class W014 : CustomizeRuleBasePage
    {
        /// <summary>
        /// 實作IEqualityComparer，判斷PackingInfo是否為相同Lot
        /// </summary>
        class PackCompare : IEqualityComparer<PackingInfo>
        {
            #region IEqualityComparer<Person> Members

            public bool Equals(PackingInfo x, PackingInfo y)
            {
                return x.LotInfo.Lot.Equals(y.LotInfo.Lot);
            }

            public int GetHashCode(PackingInfo obj)
            {
                return obj.LotInfo.Lot.GetHashCode();
            }

            #endregion
        }

        /// <summary>
        /// 定義包裝所需的類別
        /// </summary>
        class PackingInfo
        {
            public string Item { get; set; }
            public string ComponentID { get; set; }
            public string Device { get; set; }
            public string DMC { get; set; }
            public ComponentInfo ComponentInfo { get; set; }
            public LotInfo LotInfo { get; set; }
        }

        /// <summary>
        /// 包裝清單全域變數
        /// </summary>
        private List<PackingInfo> _PackingList
        {
            get
            {
                return (List<PackingInfo>)this["_PackingList"];
            }
            set
            {
                this["_PackingList"] = value;
            }
        }

        /// <summary>
        /// 對應料號包裝清單全域變數
        /// </summary>
        private List<PackingInfo> _RelativePackingList
        {
            get
            {
                return (List<PackingInfo>)this["_RelativePackingList"];
            }
            set
            {
                this["_RelativePackingList"] = value;
            }
        }

        /// <summary>
        /// 生產型態
        /// </summary>
        private String _ProdType
        {
            get
            {
                return (String)this["_ProdType"];
            }
            set
            {
                this["_ProdType"] = value;
            }
        }

        /// <summary>
        /// 包裝方式：Standard(單手)、Mix(左右手)
        /// </summary>
        private String _PackType
        {
            get
            {
                return (String)this["_PackType"];
            }
            set
            {
                this["_PackType"] = value;
            }
        }

        /// <summary>
        /// 型號名稱
        /// </summary>
        private String _DeviceName
        {
            get
            {
                return (String)this["_DeviceName"];
            }
            set
            {
                this["_DeviceName"] = value;
            }
        }

        /// <summary>
        /// 對應型號名稱
        /// </summary>
        private String _RelativeDeviceName
        {
            get
            {
                return (String)this["_RelativeDeviceName"];
            }
            set
            {
                this["_RelativeDeviceName"] = value;
            }
        }

        private String _BatchID
        {
            get
            {
                return (String)this["_BatchID"];
            }
            set
            {
                this["_BatchID"] = value;
            }
        }

        private DataSet _dsReport
        {
            get
            {
                return (DataSet)this["_dsReport"];
            }
            set
            {
                this["_dsReport"] = value;
            }
        }

        private string _Shift
        {
            get
            {
                return (String)this["_Shift"];
            }
            set
            {
                this["_Shift"] = value;
            }
        }

        private string _ShiftDate
        {
            get
            {
                return (String)this["_ShiftDate"];
            }
            set
            {
                this["_ShiftDate"] = value;
            }
        }

        private string _StartTime
        {
            get
            {
                return (String)this["_StartTime"];
            }
            set
            {
                this["_StartTime"] = value;
            }
        }

        private List<CSTUserWorkTimeInfo> _UserWorkTimeList
        {
            get
            {
                return (List<CSTUserWorkTimeInfo>)this["_UserWorkTimeList"];
            }
            set
            {
                this["_UserWorkTimeList"] = value;
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
            ttbPackType.Text = "";
            ttbMaxPackSize.Text = "";
            ttbRelativeDevice.Text = "";
            ttbWorkpiece.Text = "";
            ttbTempWorkpiece.Text = "";
            ttbTempWorkpiece.ReadOnly = false;
            ddlInspector.ClearSelection();
            gvWorkpiece.SetDataSource(null, true);
            gvRelativeWorkpiece.SetDataSource(null, true);
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        private void ClearData()
        {
            _ProdType = "";
            _DeviceName = "";
            _PackType = "";
            _RelativeDeviceName = "";
            _BatchID = "";
            _dsReport = null;
            _Shift = "";
            _ShiftDate = "";
            _StartTime = "";
            _PackingList = new List<PackingInfo>();
            _RelativePackingList = new List<PackingInfo>();
            _UserWorkTimeList = new List<CSTUserWorkTimeInfo>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // 檢查權限
                if (!IsPostBack)
                {
                    ClearField();
                    if (!UserProfileInfo.CheckUserRight(User.Identity.Name, ProgramRight))
                    {
                        HintAndRedirect(TextMessage.Error.T00264(User.Identity.Name, ProgramRight));
                        return;
                    }
                    // 載入預設畫面
                    LoadDefaultControl();
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

        /// <summary>
        /// 載入預設畫面
        /// </summary>
        private void LoadDefaultControl()
        {
            // 判斷是否有不卡控權限，若無則不可選取不卡控核選方塊
            if (!UserProfileInfo.CheckUserRight(User.Identity.Name, "PackNoControl"))
            {
                ckbNoControl.Visible = false;
            }

            // 取得所有檢驗人員
            var lstInspector = InfoCenter.GetList<UserProfileInfo>("SELECT * FROM MES_SEC_USER_PRFL WHERE SECTION='FQC'");
            lstInspector.ForEach(p =>
            {
                ddlInspector.Items.Add(new ListItem(p.UserName, p.UserID));
            });
            ddlInspector.Items.Insert(0, "");
            AjaxFocus(ttbDeviceName);
        }

        protected void ttbDeviceName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // 清除資料與使用者介面
                ClearField();
                // 取得輸入的料號
                string sDeviceName = ttbDeviceName.Text.Trim();

                #region 輸入料號客製屬性檢查
                var deviceVersionInfo = DeviceVersionInfo.GetActiveDeviceVersion(sDeviceName);
                if (deviceVersionInfo == null)
                {
                    throw new CimesException(TextMessage.Error.T00030(lblDeviceName.Text, sDeviceName));
                }
                // 檢查生產型態系統屬性是否有設定
                var prodTypeAttr = deviceVersionInfo["PRODTYPE"].ToString();
                if (prodTypeAttr.IsNullOrEmpty())
                {
                    throw new CimesException(TextMessage.Error.T00541(sDeviceName, "PRODTYPE"));
                }
                _ProdType = prodTypeAttr;
                // 檢查包裝方式系統屬性是否有設定
                var packTypeAttr = deviceVersionInfo["PACKTYPE"].ToString();
                if (packTypeAttr.IsNullOrEmpty())
                {
                    throw new CimesException(TextMessage.Error.T00541(sDeviceName, "PACKTYPE"));
                }
                _PackType = packTypeAttr;
                // 檢查包裝滿箱數量是否有設定
                var maxPackSizeAttr = deviceVersionInfo["MAX_PACK_SIZE"].ToString();
                if (maxPackSizeAttr.IsNullOrEmpty())
                {
                    throw new CimesException(TextMessage.Error.T00541(sDeviceName, "MAX_PACK_SIZE"));
                }
                #endregion

                string relativeDeviceAttr = "";
                // 包裝方式為Mix(左右手)須設定與檢查對應料號
                if (packTypeAttr == "Mix")
                {
                    // 若設定為混合，則滿箱數必須為雙數
                    //if (maxPackSizeAttr.ToDecimal() % 2 != 0)
                    //{
                    //    throw new CimesException(RuleMessage.Error.C10036(sDeviceName, maxPackSizeAttr.ToString()));
                    //}

                    // 檢查對應料號系統屬性是否有設定
                    relativeDeviceAttr = deviceVersionInfo["RELATIVE_DEVICE"].ToCimesString();
                    if (relativeDeviceAttr.IsNullOrEmpty())
                    {
                        throw new CimesException(TextMessage.Error.T00541(relativeDeviceAttr, "RELATIVE_DEVICE"));
                    }

                    #region 對應檢查

                    string sRelativeDeviceName = relativeDeviceAttr;
                    _RelativeDeviceName = sRelativeDeviceName;
                    // 檢查對應的料號是否存在
                    var relativeDeviceIfno = DeviceVersionInfo.GetActiveDeviceVersion(sRelativeDeviceName);
                    if (relativeDeviceIfno == null)
                    {
                        throw new CimesException(RuleMessage.Error.C10037(sDeviceName, sRelativeDeviceName));
                    }
                    // 檢查對應的料號生產型態系統屬性是否有設定
                    var relativeProdTypeAttr = relativeDeviceIfno["PRODTYPE"].ToString();
                    if (relativeProdTypeAttr.IsNullOrEmpty())
                    {
                        throw new CimesException(TextMessage.Error.T00541(sRelativeDeviceName, "PRODTYPE"));
                    }
                    // 檢查對應的料號包裝方式系統屬性是否有設定
                    var relativePackTypeAttr = relativeDeviceIfno["PACKTYPE"].ToString();
                    if (relativePackTypeAttr.IsNullOrEmpty())
                    {
                        throw new CimesException(TextMessage.Error.T00541(sRelativeDeviceName, "PACKTYPE"));
                    }
                    // 檢查對應的料號包裝滿箱數量是否有設定
                    var relativeMaxPackSizeAttr = relativeDeviceIfno["MAX_PACK_SIZE"].ToString();
                    if (relativeMaxPackSizeAttr.IsNullOrEmpty())
                    {
                        throw new CimesException(TextMessage.Error.T00541(sRelativeDeviceName, "MAX_PACK_SIZE"));
                    }
                    // 檢查對應的料號對應料號系統屬性
                    var relativeRelativeDeviceAttr = relativeDeviceIfno["RELATIVE_DEVICE"].ToString();
                    if (relativeRelativeDeviceAttr.IsNullOrEmpty())
                    {
                        throw new CimesException(TextMessage.Error.T00541(sRelativeDeviceName, "RELATIVE_DEVICE"));
                    }
                    // 檢查輸入料號與對應料號生產型態是否相同
                    if (prodTypeAttr != relativeProdTypeAttr)
                    {
                        throw new CimesException(RuleMessage.Error.C10038(sDeviceName, "PRODTYPE", prodTypeAttr, sRelativeDeviceName, "PRODTYPE", relativeProdTypeAttr));
                    }
                    // 檢查輸入料號與對應料號包裝方式是否相同
                    if (packTypeAttr != relativePackTypeAttr)
                    {
                        throw new CimesException(RuleMessage.Error.C10038(sDeviceName, "PACKTYPE", packTypeAttr, sRelativeDeviceName, "PACKTYPE", relativePackTypeAttr));
                    }
                    // 檢查輸入料號與對應料號滿箱數量是否相同
                    if (maxPackSizeAttr != relativeMaxPackSizeAttr)
                    {
                        throw new CimesException(RuleMessage.Error.C10038(sDeviceName, "MAX_PACK_SIZE", maxPackSizeAttr, sRelativeDeviceName, "MAX_PACK_SIZE", relativeMaxPackSizeAttr));
                    }
                    // 檢查輸入料號與對應料號[對應料號]是否相同
                    if (relativeRelativeDeviceAttr != sDeviceName)
                    {
                        throw new CimesException(RuleMessage.Error.C10039(sRelativeDeviceName, "RELATIVE_DEVICE", relativeRelativeDeviceAttr, sDeviceName));
                    }

                    #endregion

                    // 只有在包裝方式為Mix(左右手)才顯示右邊表格
                    gvRelativeWorkpiece.Visible = true;
                }
                else
                {
                    // 包裝方式非Mix(左右手)不顯示右邊表格
                    gvRelativeWorkpiece.Visible = false;
                }

                // 顯示包裝方式
                ttbPackType.Text = GetUIResource(packTypeAttr);
                // 顯示滿箱數量
                ttbMaxPackSize.Text = maxPackSizeAttr.ToDecimal().ToString();
                // 顯示對應料號
                ttbRelativeDevice.Text = relativeDeviceAttr == null ? "" : relativeDeviceAttr;

                _DeviceName = sDeviceName;
                // 取得班別資料
                _StartTime = DBCenter.GetSystemTime();
                var shiftData = CustomizeFunction.GetUserShifeData(_StartTime, false);
                _ShiftDate = shiftData.First;
                _Shift = shiftData.Second;                

                AjaxFocus(ttbWorkpiece);
            }
            catch (Exception ex)
            {
                HandleError(ex, ttbDeviceName.ClientID);
            }
        }

        protected void ttbWorkpiece_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // 若卡控需輸入料號
                if (ttbDeviceName.Text.Trim().IsNullOrEmpty() && !ckbNoControl.Checked)
                {
                    throw new RuleCimesException(TextMessage.Error.T00043(lblDeviceName.Text));
                }
                // 取得輸入工件
                string inputObject = ttbWorkpiece.Text.Trim();
                if (inputObject.IsNullOrEmpty())
                {
                    return;
                }

                ComponentInfo componentInfo = null;
                LotInfo lotInfo = null;

                var lstDMCComponent = ComponentInfoEx.GetComponentByDMCCode(inputObject).OrderBy(p => p.ComponentID).ToList();
                if (lstDMCComponent.Count == 0)
                {
                    var compID = CustomizeFunction.ConvertDMCCode(inputObject);
                    lstDMCComponent = ComponentInfoEx.GetComponentListByComponentID(compID).OrderBy(p => p.ComponentID).ToList();
                }

                lstDMCComponent.ForEach(comp =>
                {
                    // 若DMC是唯一值可以檢查是否已經重複
                    if (_ProdType == "S" || _ProdType == "B")
                    {
                        // 取得批號資料
                        lotInfo = LotInfo.GetLotByLot(comp.CurrentLot);

                        // 自動線沒有DMCCode
                        if (lotInfo.UserDefineColumn08 == "Y" && comp["DMC"].ToString().IsNullOrEmpty())
                        {
                            var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                            using (var cts = CimesTransactionScope.Create())
                            {
                                WIPTransaction.ModifyLotComponentSystemAttribute(lotInfo, comp, "DMC", inputObject, txnStamp);
                                cts.Complete();
                            }
                        }

                        #region 檢查子單元資料是否重複
                        if (lotInfo.DeviceName == _DeviceName)
                        {
                            _PackingList.ForEach(p =>
                            {
                                if (p.ComponentID == comp.ComponentID)
                                {
                                    throw new RuleCimesException(TextMessage.Error.T00033(lblWorkpiece.Text, comp.ComponentID));
                                }
                            });
                        }
                        else if (lotInfo.DeviceName == _RelativeDeviceName)
                        {
                            _RelativePackingList.ForEach(p =>
                            {
                                if (p.ComponentID == comp.ComponentID)
                                {
                                    throw new RuleCimesException(TextMessage.Error.T00033(lblWorkpiece.Text, comp.ComponentID));
                                }
                            });
                        }
                        #endregion
                        componentInfo = comp;
                    }
                    else
                    {
                        if (_PackingList.Find(p => p.ComponentID == comp.ComponentID) == null && _RelativePackingList.Find(p => p.ComponentID == comp.ComponentID) == null && componentInfo == null)
                        {
                            componentInfo = comp;
                            // 取得批號資料
                            lotInfo = LotInfo.GetLotByLot(componentInfo.CurrentLot);
                        }
                    }
                });

                // 找不到子單元需拋錯
                if (componentInfo == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Workpiece")));
                }
                // 找不到批號需拋錯
                if (lotInfo == null)
                {
                    throw new RuleCimesException(TextMessage.Error.T00045(GetUIResource("Lot")));
                }
                // 檢查CurrentRule是否正確
                if (lotInfo.CurrentRuleName != ProgramInformationBlock1.ProgramRight)
                {
                    throw new RuleCimesException(TextMessage.Error.T00384(lotInfo.CurrentRuleName, ProgramInformationBlock1.ProgramRight));
                }
                // 若需卡控
                if (!ckbNoControl.Checked)
                {
                    // 檢查批號型號與輸入的型號是否相同
                    if (_PackType == "Standard" && lotInfo.DeviceName != _DeviceName)
                    {
                        throw new RuleCimesException(RuleMessage.Error.C10041(_DeviceName));
                    }
                    // 如果為包裝方式為Mix(左右手)，則對應料號與對應料號須符合
                    if (_PackType == "Mix" && lotInfo.DeviceName != _DeviceName && lotInfo.DeviceName != _RelativeDeviceName)
                    {
                        throw new RuleCimesException(RuleMessage.Error.C10042(_DeviceName, _RelativeDeviceName));
                    }
                }

                var packTempInfo = CSTWIPPackTempInfo.GetPackTempByComponentID(componentInfo.ComponentID);
                if (packTempInfo != null)
                {
                    ttbWorkpiece.Text = "";
                    throw new RuleCimesException(RuleMessage.Error.C10093());
                }

                // 新增PackingInfo物件
                var newPackItem = new PackingInfo();
                newPackItem.ComponentID = componentInfo.ComponentID;
                newPackItem.ComponentInfo = componentInfo;
                newPackItem.LotInfo = lotInfo;
                newPackItem.DMC = componentInfo["DMC"].ToString();

                // 將PackingInfo物件加入包裝清單全域變數
                if (_PackType == "Standard" || (_PackType == "Mix" && lotInfo.DeviceName == _DeviceName) || ckbNoControl.Checked)
                {
                    // 工件數量({0})達到滿箱數量({1}) !
                    if (!ckbNoControl.Checked && _PackingList.Count >= ttbMaxPackSize.Text.ToDecimal())
                    {
                        throw new RuleCimesException(RuleMessage.Error.C10092(_PackingList.Count.ToString(), ttbMaxPackSize.Text));
                    }

                    newPackItem.Device = lotInfo.DeviceName;
                    newPackItem.Item = (_PackingList.Count + 1).ToString();
                    _PackingList.Add(newPackItem);
                }
                // 將PackingInfo物件加入對應料號包裝清單全域變數
                else
                {

                    // 工件數量({0})達到滿箱數量({1}) !
                    if (!ckbNoControl.Checked && _RelativePackingList.Count >= ttbMaxPackSize.Text.ToDecimal())
                    {
                        throw new RuleCimesException(RuleMessage.Error.C10092(_RelativePackingList.Count.ToString(), ttbMaxPackSize.Text));
                    }

                    newPackItem.Device = lotInfo.DeviceName;
                    newPackItem.Item = (_RelativePackingList.Count + 1).ToString();
                    _RelativePackingList.Add(newPackItem);
                }
                // 將目前已經輸入的物件顯示至畫面上
                _PackingList = _PackingList.OrderByDescending(p => p.Item.ToDecimal()).ToList();
                gvWorkpiece.DataSource = _PackingList;
                gvWorkpiece.DataBind();
                // 將目前已經輸入的物件顯示至畫面上
                _RelativePackingList = _RelativePackingList.OrderByDescending(p => p.Item.ToDecimal()).ToList();
                gvRelativeWorkpiece.DataSource = _RelativePackingList;
                gvRelativeWorkpiece.DataBind();
                // 清除工件欄位
                ttbWorkpiece.Text = "";
                // 將指標焦點放工件欄位輸入框
                AjaxFocus(ttbWorkpiece);

                if (_PackingList.Count != 0 || _RelativePackingList.Count != 0)
                {
                    ttbTempWorkpiece.ReadOnly = true;
                }
                else
                {
                    ttbTempWorkpiece.ReadOnly = false;
                }
                // 取得BatchID
                if (_BatchID == "")
                {
                    var cstWIPPackTempInfo = InfoCenter.GetBySQL<CSTWIPPackTempInfo>("SELECT * FROM CST_WIP_PACK_TEMP WHERE COMPONENTID = #[STRING]", inputObject);
                    _BatchID = (cstWIPPackTempInfo == null) ? "" : cstWIPPackTempInfo.BatchID;
                }

                #region ProcessWorkTime
                string sysTime = DBCenter.GetSystemTime();
                var usrWorkTime = InfoCenter.Create<CSTUserWorkTimeInfo>();
                usrWorkTime.WorkOrder = lotInfo.WorkOrder;
                usrWorkTime.WorkUserID = User.Identity.Name;
                usrWorkTime.StartTime = _StartTime;
                usrWorkTime.EndTime = DateTime.Parse(sysTime).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");
                usrWorkTime.EXECUTEFLAG = "N";
                usrWorkTime.SHIFTDATE = _ShiftDate;
                usrWorkTime.SHIFT = _Shift;
                usrWorkTime.Lot = lotInfo.Lot;
                usrWorkTime["OPERATION"] = lotInfo.OperationName;
                _StartTime = sysTime;
                _UserWorkTimeList.Add(usrWorkTime);
                #endregion
            }
            catch (Exception ex)
            {
                HandleError(ex, ttbWorkpiece.ClientID);
            }
        }

        protected void gvWorkpiece_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // 刪除PackingInfo物件於包裝清單全域變數
                int index = gvWorkpiece.Rows[e.RowIndex].DataItemIndex;
                _PackingList.RemoveAt(index);

                int idx = _PackingList.Count;
                _PackingList.ForEach(p =>
                {
                    p.Item = idx.ToString();
                    idx--;
                });
                // 畫面重新顯示
                gvWorkpiece.DataSource = _PackingList;
                gvWorkpiece.DataBind();
                // 若已經有輸入工件則不允許再輸入暫存工件
                if (_PackingList.Count != 0 || _RelativePackingList.Count != 0)
                {
                    ttbTempWorkpiece.ReadOnly = true;
                }
                else
                {
                    ttbTempWorkpiece.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void gvRelativeWorkpiece_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // 刪除PackingInfo物件於對應料號包裝清單全域變數
                int index = gvRelativeWorkpiece.Rows[e.RowIndex].DataItemIndex;
                _RelativePackingList.RemoveAt(index);

                int idx = _RelativePackingList.Count;
                _RelativePackingList.ForEach(p =>
                {
                    p.Item = idx.ToString();
                    idx--;
                });
                // 畫面重新顯示
                gvRelativeWorkpiece.DataSource = _RelativePackingList;
                gvRelativeWorkpiece.DataBind();
                // 若已經有輸入工件則不允許再輸入暫存工件
                if (_PackingList.Count != 0 || _RelativePackingList.Count != 0)
                {
                    ttbTempWorkpiece.ReadOnly = true;
                }
                else
                {
                    ttbTempWorkpiece.ReadOnly = false;
                }
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
                // 包裝數量必須符合滿箱數量，否則須拋錯
                if (!ckbNoControl.Checked && _PackType == "Standard" && (_PackingList.Count + _RelativePackingList.Count) != ttbMaxPackSize.Text.ToDecimal())
                {
                    throw new RuleCimesException(RuleMessage.Error.C10043((_PackingList.Count + _RelativePackingList.Count).ToString(), ttbMaxPackSize.Text));
                }
                // 若包裝方式為左右手，則左右手的輸入數量需相同
                if (!ckbNoControl.Checked && _PackType == "Mix" && _PackingList.Count != _RelativePackingList.Count)
                {
                    throw new RuleCimesException(RuleMessage.Error.C10044(_DeviceName, _PackingList.Count.ToString(), _RelativeDeviceName, _RelativePackingList.Count.ToString()));
                }
                // 若包裝方式為左右手，則左右手的輸入數量需相同且需符合滿箱數量乘以二
                if (!ckbNoControl.Checked && _PackType == "Mix" && (_PackingList.Count + _RelativePackingList.Count) != (ttbMaxPackSize.Text.ToDecimal() * 2))
                {
                    throw new RuleCimesException(RuleMessage.Error.C10043((_PackingList.Count + _RelativePackingList.Count).ToString(), (ttbMaxPackSize.Text.ToDecimal() * 2).ToString()));
                }

                if (ddlInspector.SelectedValue.IsNullOrEmpty())
                {
                    //[00826] 請輸入{0}！
                    throw new RuleCimesException(TextMessage.Error.T00826(lblInspector.Text), ddlInspector);
                }


                var lstSourceLot = new List<LotInfo>();
                // 定義交易戳記         
                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    // 若有不同的批號需拆批
                    _PackingList.Distinct<PackingInfo>(new PackCompare()).ToList().ForEach(p =>
                    {
                        lstSourceLot.Add(SplitLot(p.LotInfo, false, txnStamp));
                    });
                    // 若有不同的批號需拆批
                    _RelativePackingList.Distinct<PackingInfo>(new PackCompare()).ToList().ForEach(p =>
                    {
                        lstSourceLot.Add(SplitLot(p.LotInfo, true, txnStamp));
                    });

                    #region SplitBoxLot
                    List<SqlAgent> splitLotArchiSQLList = new List<SqlAgent>();
                    var generator = NamingIDGenerator.GetRule("BoxNo");
                    if (generator == null)
                    {
                        //WRN-00411,找不到可產生的序號，請至命名規則維護設定，規則名稱：{0}!!!
                        throw new Exception(TextMessage.Error.T00437("BoxNo"));
                    }
                    var serialData = generator.GenerateNextIDs(1, lstSourceLot[0], new string[] { }, User.Identity.Name);
                    splitLotArchiSQLList = serialData.Second;
                    var boxNoLotID = serialData.First[0];

                    var reasonCategoryInfo = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");
                    var splitLotData = SplitLotInfo.CreateSplitLotByLotAndQuantity(lstSourceLot[0].Lot, boxNoLotID, 0, 0, reasonCategoryInfo, "SplitBoxLot");

                    WIPTxn.SplitIndicator splitInd = WIPTxn.SplitIndicator.Create();
                    WIPTxn.Default.SplitLot(lstSourceLot[0], splitLotData, splitInd, txnStamp);

                    //若子單元為自動產生，更新序號至DB
                    if (splitLotArchiSQLList != null && splitLotArchiSQLList.Count > 0)
                    {
                        DBCenter.ExecuteSQL(splitLotArchiSQLList);
                    }
                    #endregion

                    var targetLot = LotInfo.GetLotByLot(boxNoLotID);
                    // 將批號轉換為另一個批號的子單元，轉換後批號結批，例如可使用於包裝
                    WIPTxn.Default.ConvertToComponent(targetLot, lstSourceLot, "Box", WIPTxn.ConvertToComponentIndicator.Default, txnStamp);
                    // 紀錄檢驗人員
                    //WIPTransaction.ModifyLotSystemAttribute(targetLot, "USERDEFINECOL12", ddlInspector.SelectedValue, txnStamp);
                    // 進行Dispatch
                    WIPTransaction.DispatchLot(targetLot, txnStamp);
                    // 刪除暫存檔案
                    if (_BatchID != "")
                    {
                        CSTWIPPackTempInfo.DeletePackTempByBatchID(_BatchID);
                    }                    

                    var packInfo = InfoCenter.Create<CSTWIPPackInfo>();
                    packInfo.BOXNO = boxNoLotID;
                    packInfo.Quantity = _PackingList.Count + _RelativePackingList.Count;
                    packInfo.INSPUSER = ddlInspector.SelectedValue;
                    packInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    _PackingList.ForEach(comp => {
                        var packDataInfo = InfoCenter.Create<CSTWIPPackDataInfo>();
                        packDataInfo.WIP_PACK_SID = packInfo.ID;
                        packDataInfo.ComponentID = comp.ComponentID;
                        packDataInfo.DMC = comp.DMC;
                        packDataInfo.DeviceName = comp.Device;
                        packDataInfo.Quantity = 1;
                        packDataInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    });

                    _RelativePackingList.ForEach(comp => {
                        var packDataInfo = InfoCenter.Create<CSTWIPPackDataInfo>();
                        packDataInfo.WIP_PACK_SID = packInfo.ID;
                        packDataInfo.ComponentID = comp.ComponentID;
                        packDataInfo.DMC = comp.DMC;
                        packDataInfo.DeviceName = comp.Device;
                        packDataInfo.Quantity = 1;
                        packDataInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    });

                    //工時
                    _UserWorkTimeList.ForEach(p => {
                        p.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    });
                    _UserWorkTimeList.Clear();

                    #region Print
                    _dsReport = new DataSet();
                    // 取得Report資料
                    _dsReport = GetRunCardDataSource(targetLot);
                    _dsReport.AcceptChanges();

                    if (_dsReport.Tables.Count > 0)
                    {
                        string sPrintProgram = "/CustomizeRule/WIPRule/W035View.aspx";
                        string sHost = Request.Url.Host;
                        string sApplicationPath = Request.ApplicationPath;
                        string ReportPath = "http://" + sHost + sApplicationPath + sPrintProgram;
                        Session["W035View"] = _dsReport;
                        //開啟查詢工單頁面
                        string openPrintWindow = "window.open('" + ReportPath + "','pop','resizable: yes; status: no; scrollbars:no; menubar:no;toolbar:no;location:no;dialogLeft:10px;dialogTop:10px;dialogHeight:10px;dialogWidth:10px',false);";
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), Guid.NewGuid().ToString(), openPrintWindow, true);
                    }
                    #endregion
                    cts.Complete();
                }
                // ClearUI不會清除Device資料
                ttbDeviceName.Text = "";
                ClearField();
                //ReturnToPortal();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private LotInfo SplitLot(LotInfo distinctLot, bool isRelative, TransactionStamp txnStamp)
        {
            LotInfo lotData = LotInfo.GetLotByLot(distinctLot.Lot);

            List<SqlAgent> splitLotArchiSQLList = new List<SqlAgent>();
            List<ComponentInfo> lsComponentDatas = new List<ComponentInfo>();
            if (isRelative)
            {
                // 找出相同批號的所有Component
                _RelativePackingList.FindAll(lot => lot.LotInfo.Lot == lotData.Lot).ForEach(pack =>
                {
                    lsComponentDatas.Add(ComponentInfo.GetComponentByComponentID(pack.ComponentID));
                });
            }
            else
            {
                // 找出相同批號的所有Component
                _PackingList.FindAll(lot => lot.LotInfo.Lot == lotData.Lot).ForEach(pack =>
                {
                    lsComponentDatas.Add(ComponentInfo.GetComponentByComponentID(pack.ComponentID));
                });
            }

            var generator = NamingIDGenerator.GetRule("SplitLot");
            if (generator == null)
            {
                //WRN-00411,找不到可產生的序號，請至命名規則維護設定，規則名稱：{0}!!!
                throw new Exception(TextMessage.Error.T00437("SplitLot"));
            }
            var serialData = generator.GenerateNextIDs(1, lotData, new string[] { }, User.Identity.Name);
            splitLotArchiSQLList = serialData.Second;
            var splitLotID = serialData.First[0];

            var reasonCategoryInfo = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");
            SplitLotInfo splitLotData = SplitLotInfo.CreateSplitLotByLotAndQuantity(lotData.Lot, splitLotID, lsComponentDatas, reasonCategoryInfo, "Pack");

            WIPTxn.SplitIndicator splitInd = WIPTxn.SplitIndicator.Create();
            // 拆批交易
            WIPTxn.Default.SplitLot(lotData, splitLotData, splitInd, txnStamp);

            //若子單元為自動產生，更新序號至DB
            if (splitLotArchiSQLList != null && splitLotArchiSQLList.Count > 0)
            {
                DBCenter.ExecuteSQL(splitLotArchiSQLList);
            }

            return LotInfo.GetLotByLot(splitLotID);
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

        protected void ckbNoControl_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // 清除清除資料與使用者介面
                ClearField();
                ttbDeviceName.Text = "";
                // 若不卡控不需輸入料號
                if (ckbNoControl.Checked)
                {
                    ttbDeviceName.ReadOnly = true;
                    gvRelativeWorkpiece.Visible = false;
                }
                else
                {
                    ttbDeviceName.ReadOnly = false;
                    gvRelativeWorkpiece.Visible = true;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void btnTempSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_DeviceName.IsNullOrEmpty())
                {
                    throw new RuleCimesException(TextMessage.Error.T00826(lblDeviceName.Text));
                }

                if (_PackingList.Count == 0 && _RelativePackingList.Count == 0)
                {
                    throw new RuleCimesException(TextMessage.Error.T00826(lblWorkpiece.Text));
                }

                var txnStamp = new TransactionStamp(User.Identity.Name, ProgramRight, ProgramRight, ApplicationName);
                using (var cts = CimesTransactionScope.Create())
                {
                    //刪除舊的資料
                    if (!_BatchID.IsNullOrEmpty())
                    {
                        DBCenter.ExecuteParse("DELETE CST_WIP_PACK_TEMP WHERE BATCHID = #[STRING]", _BatchID);
                    }
                    //產生新的批次ID
                    _BatchID = DBCenter.GetSystemID();
                    _PackingList.ForEach(p =>
                    {
                        var tempInfo = InfoCenter.Create<CSTWIPPackTempInfo>();
                        tempInfo.DeviceName = _DeviceName;
                        tempInfo.ComponentID = p.ComponentID;
                        tempInfo.SIDE = "L";
                        tempInfo.BatchID = _BatchID;
                        tempInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    });
                    _RelativePackingList.ForEach(p =>
                    {
                        var tempInfo = InfoCenter.Create<CSTWIPPackTempInfo>();
                        tempInfo.DeviceName = _RelativeDeviceName;
                        tempInfo.ComponentID = p.ComponentID;
                        tempInfo.SIDE = "R";
                        tempInfo.BatchID = _BatchID;
                        tempInfo.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    });
                    //工時
                    _UserWorkTimeList.ForEach(p => {
                        p.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    });
                    _UserWorkTimeList.Clear();
                    cts.Complete();
                }

                ProgramInformationBlock1.ShowMessage(TextMessage.Hint.T00057(GetUIResource("TempSave")));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        protected void ttbTempWorkpiece_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //必須輸入Device
                if (_DeviceName.IsNullOrEmpty())
                {
                    ttbTempWorkpiece.Text = "";
                    AjaxFocus(ttbDeviceName);
                    throw new RuleCimesException(TextMessage.Error.T00826(lblDeviceName.Text));
                }

                if (ttbTempWorkpiece.Text.Trim().IsNullOrEmpty())
                {
                    return;
                }

                string sTempWorkpiece = CustomizeFunction.ConvertDMCCode(ttbTempWorkpiece.Text.Trim());

                //取得BatchID
                var cstWIPPackTempInfo = InfoCenter.GetBySQL<CSTWIPPackTempInfo>("SELECT * FROM CST_WIP_PACK_TEMP WHERE COMPONENTID = #[STRING]", sTempWorkpiece);

                if(cstWIPPackTempInfo == null)
                {
                    throw new CimesException(RuleMessage.Error.C00054(ttbTempWorkpiece.Text.Trim()));
                }

                if (cstWIPPackTempInfo.SIDE == "R" && _DeviceName == cstWIPPackTempInfo.DeviceName)
                {
                    throw new RuleCimesException(RuleMessage.Error.C10168());
                }

                _BatchID = (cstWIPPackTempInfo == null) ? "" : cstWIPPackTempInfo.BatchID;

                if (_BatchID == "")
                {
                    AjaxFocus(ttbTempWorkpiece);
                    throw new RuleCimesException(TextMessage.Error.T00045(lblTempWorkpiece.Text));
                }

                var lstCSTWIPPackTempInfo = InfoCenter.GetList<CSTWIPPackTempInfo>("SELECT * FROM CST_WIP_PACK_TEMP WHERE BATCHID = #[STRING]", _BatchID);

                int idx = 1;
                lstCSTWIPPackTempInfo.FindAll(p => p.SIDE == "L").ForEach(p =>
                {
                    var packingInfo = new PackingInfo();
                    packingInfo.Item = idx.ToString();
                    packingInfo.Device = _DeviceName;
                    packingInfo.ComponentID = p.ComponentID;
                    ComponentInfo componentInfo = ComponentInfo.GetComponentByComponentID(p.ComponentID);
                    packingInfo.ComponentInfo = componentInfo;
                    packingInfo.DMC = componentInfo["DMC"].ToString();
                    LotInfo lotInfo = LotInfo.GetLotByLot(componentInfo.CurrentLot);
                    packingInfo.LotInfo = lotInfo;
                    idx++;
                    _PackingList.Add(packingInfo);
                });

                idx = 1;
                lstCSTWIPPackTempInfo.FindAll(p => p.SIDE == "R").ForEach(p =>
                {
                    var packingInfo = new PackingInfo();
                    packingInfo.Item = idx.ToString();
                    packingInfo.Device = _RelativeDeviceName;
                    packingInfo.ComponentID = p.ComponentID;
                    ComponentInfo componentInfo = ComponentInfo.GetComponentByComponentID(p.ComponentID);
                    packingInfo.ComponentInfo = componentInfo;
                    packingInfo.DMC = componentInfo["DMC"].ToString();
                    LotInfo lotInfo = LotInfo.GetLotByLot(componentInfo.CurrentLot);
                    packingInfo.LotInfo = lotInfo;
                    idx++;
                    _RelativePackingList.Add(packingInfo);
                });

                gvWorkpiece.DataSource = _PackingList;
                gvWorkpiece.DataBind();

                gvRelativeWorkpiece.DataSource = _RelativePackingList;
                gvRelativeWorkpiece.DataBind();

                ttbTempWorkpiece.Text = "";
                if (_PackingList.Count != 0 || _RelativePackingList.Count != 0)
                {
                    ttbTempWorkpiece.ReadOnly = true;
                }
                else
                {
                    ttbTempWorkpiece.ReadOnly = false;
                }
                AjaxFocus(ttbWorkpiece);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private DataSet GetRunCardDataSource(LotInfo LotData)
        {
            string sql = "";

            #region 定義 LOTDATA 資料表
            DataTable dtLotData = LotData.CopyDataToTable("LOTDATA");
            dtLotData.Columns.Add("CartonNo");
            dtLotData.Columns.Add("DeviceDescr1");
            dtLotData.Columns.Add("DeviceDescr2");
            dtLotData.Columns.Add("CustomerNo1");
            dtLotData.Columns.Add("CustomerNo2");
            dtLotData.Columns.Add("Device1");
            dtLotData.Columns.Add("Device2");
            dtLotData.Columns.Add("Quantity1");
            dtLotData.Columns.Add("Quantity2");
            dtLotData.Columns.Add("Remark");
            dtLotData.Columns.Add("Inspectors");
            dtLotData.Columns.Add("Packers");
            dtLotData.Columns.Add("InspectionDate");
            #endregion

            #region 定義 COMPDATA 資料表
            DataTable dtCompData = new DataTable("COMPDATA");
            dtCompData.Columns.Add("ComponentID1");
            dtCompData.Columns.Add("ComponentID2");
            dtCompData.Columns.Add("Quantity1");
            dtCompData.Columns.Add("Quantity2");
            #endregion

            dtLotData.Rows[0]["CartonNo"] = LotData.Lot;
            dtLotData.Rows[0]["Remark"] = "";

            var packInfo = CSTWIPPackInfo.GetPackInfoByBoxNo(LotData.Lot);
            dtLotData.Rows[0]["Inspectors"] = packInfo.INSPUSER;
            dtLotData.Rows[0]["Packers"] = packInfo.UserID;
            dtLotData.Rows[0]["InspectionDate"] = packInfo.UpdateTime.Replace("/","-");

            sql = @" SELECT DEVICE,COUNT(*) QTY 
                       FROM CST_WIP_PACK 
                      INNER JOIN CST_WIP_PACK_DATA ON CST_WIP_PACK.WIP_PACK_SID = CST_WIP_PACK_DATA.WIP_PACK_SID
                      WHERE BOXNO = #[STRING] 
                      GROUP BY DEVICE";

            DataTable dtData = DBCenter.GetDataTable(sql, LotData.Lot);
            int iIndex = 1;
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                if (i >= 2)
                    break;

                iIndex = i + 1;
                dtLotData.Rows[0]["Quantity" + iIndex.ToCimesString()] = dtData.Rows[i]["QTY"].ToCimesString();

                var DeviceData = DeviceVersionInfoEx.GetActiveDeviceVersion(dtData.Rows[i]["DEVICE"].ToCimesString()).ChangeTo<DeviceVersionInfoEx>();
                if (DeviceData != null)
                {
                    dtLotData.Rows[0]["DeviceDescr" + iIndex.ToCimesString()] = DeviceData.Description;
                    dtLotData.Rows[0]["CustomerNo" + iIndex.ToCimesString()] = DeviceData["CustomerNo"].ToCimesString();
                    dtLotData.Rows[0]["Device" + iIndex.ToCimesString()] = DeviceData.DeviceName;
                }

                #region 入庫批號

                sql = @"SELECT DMC,COUNT(*) COMPONENTQTY 
                          FROM CST_WIP_PACK 
                         INNER JOIN CST_WIP_PACK_DATA ON CST_WIP_PACK.WIP_PACK_SID = CST_WIP_PACK_DATA.WIP_PACK_SID
                         WHERE BOXNO = #[STRING] 
                           AND DEVICE = #[STRING] 
                         GROUP BY DMC 
                         ORDER BY DMC ";

                var dt = DBCenter.GetDataTable(sql, LotData.Lot, DeviceData.DeviceName);
                if (dt.Rows.Count > dtCompData.Rows.Count)
                {
                    int iRowCount = dt.Rows.Count - dtCompData.Rows.Count;
                    for (int j = 0; j < iRowCount; j++)
                    {
                        DataRow dr = dtCompData.NewRow();
                        dtCompData.Rows.Add(dr);
                    }
                }
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    dtCompData.Rows[k]["ComponentID" + iIndex.ToCimesString()] = dt.Rows[k]["DMC"].ToString();
                    dtCompData.Rows[k]["Quantity" + iIndex.ToCimesString()] = dt.Rows[k]["COMPONENTQTY"].ToString();
                }
                #endregion
            }

            dtCompData.AcceptChanges();
            DataSet dsReportData = new DataSet();
            dsReportData.Tables.Add(dtLotData);
            dsReportData.Tables.Add(dtCompData);

            return dsReportData;
        }
    }
}