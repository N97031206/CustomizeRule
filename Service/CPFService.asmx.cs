using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Transaction;
using CustomizeRule;
using CustomizeRule.CustomizeInfo.ExtendInfo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using CustomizeRule.RuleUtility;

namespace Service
{
    /// <summary>
    ///CPFService 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class CPFService : System.Web.Services.WebService
    {
        string _ApplicationName = "CPFService";

        /// <summary>
        /// 連線檢查
        /// </summary>
        /// <param name="EAP">EAP主機名稱</param>
        /// <param name="IP">IP位址</param>
        /// <param name="Port">Port</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckAlive(string EAP, string IP, string Port, string Line)
        {
            try
            {
                string ProgramRight = "CheckAlive";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    var EAPData = CSTAPIEAPServerInfo.GetDataByEAPNameAndLine(EAP, Line);

                    if (EAPData == null)
                    {
                        //新增資料
                        var insertEAPData = InfoCenter.Create<CSTAPIEAPServerInfo>();
                        insertEAPData.APIEAPServer = DBCenter.GetSystemID();
                        insertEAPData.EAPName = EAP;
                        insertEAPData.IP = IP;
                        insertEAPData.Port = Port;
                        insertEAPData.Line = Line;

                        insertEAPData.InsertToDB();
                    }
                    else
                    {
                        //更新資料
                        EAPData.IP = IP;
                        EAPData.Port = Port;

                        EAPData.UpdateToDB();
                    }

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 模具資料
        /// </summary>
        /// <param name="MoldSerial"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ScanMoldSN(string MoldSN, string Count)
        {
            try
            {
                //MoldSN不為空值
                if (!MoldSN.IsNullOrTrimEmpty())
                {
                    string ProgramRight = "ScanMoldSerial";

                    TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                    #region 執行交易
                    using (var cts = CimesTransactionScope.Create())
                    {
                        //取得CST_API_Count內模具資料
                        var MoldData = CSTAPIMoldCountInfo.GetMoldCount(MoldSN);                     

                        #region 模具名稱                
                        //CST_API_COUNT無該模具資料
                        if (MoldData == null)
                        {
                            ////新增模具次數資料
                            var insertMoldCount = InfoCenter.Create<CSTAPIMoldCountInfo>();
                            insertMoldCount.MoldName = MoldSN;//模具名稱
                            insertMoldCount.MoldCount = 0;//模具次數
                            insertMoldCount.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);//寫入DB
                        }
                        else//CST_API_COUNT有模具資料
                        {
                            ///PLC Count次數轉型
                            int chkCount = Convert.ToInt32(Count.Trim());

                            #region 模具使用次數
                            //PLC次數與CST_API_COUNT次數不一致才執行
                            if (MoldData.MoldCount != chkCount)
                            {
                                //次數是否等於0
                                if (chkCount.Equals(0))
                                {
                                    //新增模具次數歸零資料
                                    var insertMoldResetlog = InfoCenter.Create<CSTAPIMoldResetLogInfo>();
                                    //將CST_API_COUNT的次數寫入CST_API_RESET_LOG的.FinalCount
                                    insertMoldResetlog.FinalCount = MoldData.MoldCount;
                                    //寫入CST_API_RESET_LOG的重置時間
                                    insertMoldResetlog.ResetTime = DBCenter.GetSystemTime();
                                    //寫入模具名稱
                                    insertMoldResetlog.MoldName = MoldSN;
                                    //使用者名稱
                                    insertMoldResetlog["USERID"] = txnStamp.UserID;
                                    //UPDATETIME用CST_API_COUNT的UpdateTime
                                    insertMoldResetlog["UPDATETIME"] = MoldData.UpdateTime;
                                    //更新CST_API_RESET_LOG
                                    insertMoldResetlog.InsertToDB();

                                    //更新CST_API_Count的MoidCount為0
                                    MoldData.MoldCount = 0;
                                    MoldData.UpdateToDB();//更新
                                }
                                else if (chkCount > 0)//次數不等於0，且次數大於0
                                {
                                    //將PLC所讀取的模具次數寫入CSTAPIMoldCountInfo的MoldCount
                                    MoldData.MoldCount = Count.ToInt();
                                    //更新CSTAPIMoldCountInfo
                                    MoldData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                                }
                                else//為負值
                                {
                                    ///檢查PLC傳回次數資料格式
                                }
                            }
                            #endregion
                        }
                        #endregion

                        //執行交易
                        cts.Complete();
                    }
                    #endregion

                }
                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 裁切的重量
        /// </summary>
        /// <param name="Weight">重量</param>
        /// <param name="Result">秤重結果(OK/NG)</param>
        /// <param name="ShortMaterialNO">來料序號的第幾支短料</param>
        /// <param name="RawMaterialNO">來料序號</param>
        /// <param name="IsFinalNO">是否為來料序號的最後一支(Y/N)</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CutWeight(string Weight, string Result, string ShortMaterialNO, string RawMaterialNO, string IsFinalNO)
        {
            try
            {
                string ProgramRight = "CutWeight";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 取得物料資訊
                var materialLotData = MaterialLotInfoEx.GetMaterialLotByMaterialLot(RawMaterialNO);

                if (materialLotData == null)
                {
                    //[00512]物料批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00512(RawMaterialNO));
                }
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    #region 物料批更新第一數量

                    //更新第一數量
                    decimal quantity = materialLotData.Quantity + 1;
                    MMSTransaction.ModifyMaterialLotSystemAttribute(materialLotData, "QUANTITY", quantity.ToString(), txnStamp);

                    #endregion

                    #region 新增一筆物料批子單元

                    //取得子單元名稱
                    var componentID = materialLotData.MaterialLot + "." + ShortMaterialNO.PadLeft(2, '0');

                    #region 針對第二數量例外處理

                    //目前此物料批的第二總數量
                    decimal totalSecondQuantity = 0;

                    //取得目前所有的Components
                    var components = MaterialLotComponentInfo.GetMaterialLotAllComponents(materialLotData);
                    components.ForEach(component =>
                    {
                        totalSecondQuantity += component.SecondQuantity;
                    });
                    #endregion

                    var newMLotComponentData = MaterialLotComponentInfo.CreateMaterialLotComponent(componentID, 1);
                    newMLotComponentData.SecondQuantity = materialLotData.SecondQuantity - totalSecondQuantity;

                    //新增物料批子單元
                    MMSTransaction.SetComponentMaterialLot(materialLotData, newMLotComponentData, txnStamp);

                    List<ModifyAttributeInfo> lsModifyAttrData = new List<ModifyAttributeInfo>();

                    //更新欄位[SQUANTITY]內容為重量
                    lsModifyAttrData.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("SQUANTITY", Weight.ToCimesDecimal()));

                    //更新欄位[LASTFLAG]內容為是否為來料序號的最後一支
                    lsModifyAttrData.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("LASTFLAG", (IsFinalNO == "1") ? "Y" : "N"));

                    //更新欄位[WEIGHT_RESULT]內容為秤重結果
                    lsModifyAttrData.Add(ModifyAttributeInfo.CreateSystemAttributeInfo("WEIGHT_RESULT", (Result == "1") ? "OK" : "NG"));

                    var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");

                    //更新物料批子單元資料
                    MMSTransaction.ModifyMaterialLotComponentMultipleAttribute(materialLotData, newMLotComponentData, lsModifyAttrData, txnStamp);

                    #endregion

                    #region 如果是來料序號的最後一支，則執行物料批耗用記錄
                    if (IsFinalNO == "Y")
                    {
                        MMSTransaction.ConsumeMaterialLot(materialLotData, InfoCenter.Create<EquipmentInfo>(),
                            ConsumeInfo.CreateConsume(materialLotData.Quantity, materialLotData.SecondQuantity), MMSTransaction.MMSConsumeIndicator.Create(), txnStamp);
                    }
                    #endregion

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 料材分線下電梯
        /// </summary>
        /// <param name="WO">大工單</param>
        /// <param name="WO_SEQ">工單的序號</param>
        /// <param name="Qty">數量</param>
        /// <param name="RawMaterialNO">來料序號</param>
        /// <param name="AccQty">小工單的累積數量</param>
        /// <param name="FinalStove">尾爐(1或2)</param>
        /// <param name="FinalWO">尾工單(1或2)</param>
        /// <param name="FinalSubWO">斷小工單(1或2)，最後一批的註記</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateLot(string WO, string WO_SEQ, string Qty, string RawMaterialNO, string AccQty, string FinalStove, string FinalWO, string FinalSubWO, string Line)
        {
            string ProgramRight = "CreateLot";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得客製表資料
                var woLotData = CSTWorkOrderLotInfo.GetWorkOrderLotDataByWOAndWOSequence(WO, WO_SEQ.ToCimesDecimal());

                //設定此次CPF_SN的前二碼
                string newSNFirstPart = "";

                //批號名稱
                string namingLot = "";

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    #region 確認批號名稱
                    var lotData = LotInfo.GetLotByLot(woLotData.INVLOT);

                    var lotNonActiveData = LotNonActiveInfo.GetLotNonActiveByLot(woLotData.INVLOT);

                    if (lotData == null && lotNonActiveData == null)
                    {
                        //第一次新增批號
                        namingLot = woLotData.INVLOT;
                        lot = namingLot;

                        //重新取得CPF_SN前二碼
                        newSNFirstPart = GetNewSNFirstPart();
                    }
                    else
                    {
                        //接續新增批號，以Naming取批號名稱
                        if (lotData != null)
                        {
                            var splitLotNaming = GetNamingRule("SplitLotForging", txnStamp.UserID, 1, lotData);
                            namingLot = splitLotNaming.First[0];
                            lot = namingLot;

                            if (splitLotNaming.Second != null && splitLotNaming.Second.Count != 0)
                            {
                                DBCenter.ExecuteSQL(splitLotNaming.Second);
                            }
                        }
                        else
                        {
                            var splitLotNaming = GetNamingRule("SplitLotForging", txnStamp.UserID, 1, lotNonActiveData);
                            namingLot = splitLotNaming.First[0];
                            lot = namingLot;

                            if (splitLotNaming.Second != null && splitLotNaming.Second.Count != 0)
                            {
                                DBCenter.ExecuteSQL(splitLotNaming.Second);
                            }
                        }

                        //取得此批號所有的Component資料
                        var tmpComponents = ComponentInfo.GetLotAllComponents(lotData).ChangeTo<ComponentInfoEx>();

                        //取得CPF_SN前二碼
                        newSNFirstPart = GetSNFirstPart(tmpComponents[0]);
                    }
                    #endregion

                    #region 執行新增批號
                    List<LotCreateInfo> lsLotData = new List<LotCreateInfo>();

                    //取得工單資料
                    var woData = WorkOrderInfo.GetWorkOrderByWorkOrder(WO);

                    //建置批號資料
                    LotCreateInfo createLotData = LotCreateInfo.CreateLotInfo(woData, namingLot, Convert.ToDecimal(Qty), 0);
                    lsLotData.Add(createLotData);

                    //取得工作站
                    operationName = createLotData.OperationName;


                    createLotData["USERDEFINECOL04"] = FinalStove;//UDC4註記尾爐
                    createLotData["USERDEFINECOL05"] = FinalWO;//UDC5註記尾工單
                    createLotData["USERDEFINECOL06"] = FinalSubWO; //UDC6註記斷小工單
                    createLotData["WOLOT"] = woLotData.WOLOT; //WOLOT註記子工單
                    createLotData["INVLOT"] = woLotData.INVLOT; //INVLOT註記入庫批號
                    createLotData["PROCESS"] = "CPF"; //PROCESS註記CPF
                    //註記爐號、來料批號
                    if (!string.IsNullOrEmpty(RawMaterialNO))
                    {
                        var iqcPass = CSTMMSIqcPassInfo.GetIQCPassDataByiqcSN(RawMaterialNO);
                        if (iqcPass == null)
                        {
                            throw new Exception(TextMessage.Error.T00708("CST_MMS_IQC_PASS RUNID"));
                        }
                        createLotData["RUNID"] = iqcPass.RUNID; //RUNID註記爐號
                        createLotData["INCOMING_LOT"] = iqcPass.Lot; //INCOMING_LOT註記來料批號
                    }

                    //判斷同批的爐號必需相同
                    if (lotData != null)
                    {
                        if (lotData["RUNID"].ToCimesString() != createLotData["RUNID"].ToCimesString())
                        {
                            //同入庫批({0})的爐號必須相同!(LotID:{1};爐號:{2}；Create的爐號為:{3})
                            throw new Exception(RuleMessage.Error.C10174(lotData["INVLOT"].ToCimesString(), lotData.Lot, lotData["RUNID"].ToCimesString(), createLotData["RUNID"].ToCimesString()));
                        }
                    }

                    WIPTransaction.CreateLots(lsLotData, txnStamp);

                    #endregion

                    #region 執行新增批號子單元
                    List<ComponentInfo> lsComponentData = new List<ComponentInfo>();

                    var componentNaming = GetNamingRule("CreateComponentID", txnStamp.UserID, Convert.ToInt32(Qty), null, new List<string>() { woLotData.INVLOT });

                    //號碼從01開始，所以要加上1
                    int SNSecondPart = (Convert.ToInt32(AccQty) - Convert.ToInt32(Qty)) + 1;

                    for (int i = 0; i < Convert.ToInt32(Qty); i++)
                    {
                        var componentData = ComponentInfo.CreateLotComponent(componentNaming.First[i], 1, ComponentStatus.Normal);

                        //取得設定的CPF_SN
                        var CPF_SN = newSNFirstPart + string.Format("{0:00}", SNSecondPart);
                        componentData["CPF_SN"] = CPF_SN;
                        SNSecondPart++;

                        //更新欄位[LINE]
                        componentData["LINE"] = Line;

                        //記錄來料序號
                        componentData["INCOMING_SN"] = RawMaterialNO;

                        //component加入List
                        lsComponentData.Add(componentData);
                    }

                    if (componentNaming.Second != null && componentNaming.Second.Count != 0)
                    {
                        DBCenter.ExecuteSQL(componentNaming.Second);
                    }

                    WIPTransaction.SetLotComponent(createLotData, lsComponentData, txnStamp);

                    #endregion

                    //執行批號下線
                    WIPTransaction.StartLot(createLotData, LotDefaultStatus.Wait.ToString(), txnStamp);

                    #region 更新工單內容

                    //已經發放的數量
                    woData.ReleaseQuantity = Convert.ToDecimal(woData.ReleaseQuantity) + Convert.ToDecimal(Qty);
                    //改為Created
                    if (woData.Flag == "Release")
                        woData.Flag = "Created";

                    if (woData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime) == 0)
                    {
                        //WRN-00001:資料已被其他使用者更新，請重新查詢！
                        throw new Exception(TextMessage.Error.T00747(""));
                    }

                    #endregion

                    //更新CST_WO_LOT的CreateFlag
                    woLotData.CreateFlag = "Y";
                    woLotData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 鍛造電熱爐進站
        /// </summary>
        /// <param name="WO">工單</param>
        /// <param name="WO_SEQ">工單序號</param>
        /// <param name="AccQty">小工單的累積數量</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckInForging(string WO, string WO_SEQ, string AccQty, string Line)
        {
            string ProgramRight = "CheckInForging";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得客製表資料
                var woLotData = CSTWorkOrderLotInfo.GetWorkOrderLotDataByWOAndWOSequence(WO, WO_SEQ.ToCimesDecimal());

                lot = woLotData.INVLOT;

                #region 確認批號是否存在
                var lotData = LotInfo.GetLotByLot(woLotData.INVLOT);

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(woLotData.INVLOT));
                }

                operationName = lotData.OperationName;
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    #region 批號執行進站
                    if (lotData.Status != LotDefaultStatus.Run.ToString())
                    {
                        #region 更新批號子單元資料[LOCATION]及[SN]

                        //批號子單元清單
                        var lsComponetData = ComponentInfoEx.GetDataByCurrentLot(lotData.Lot);
                        lsComponetData.Sort(p => p.ComponentID);

                        for (int i = 0; i < lsComponetData.Count; i++)
                        {
                            string location = "A1";
                            string newSN = "1";

                            #region 取得最後一筆工件的SN
                            var firstComponentDatas = ComponentInfoEx.GetDataByLocation(location);

                            if (firstComponentDatas.Count == 0)
                            {
                                //鍛造位置：{0} 無任何工件序號
                                //throw new Exception(RuleMessage.Error.C10048(location));
                            }
                            else
                            {
                                decimal dSN = 0;
                                if (decimal.TryParse(firstComponentDatas[firstComponentDatas.Count - 1].SN, out dSN))
                                {
                                    //取得新的SN編號
                                    newSN = (dSN + 1).ToString();
                                }
                            }
                            #endregion

                            //更新欄位[LOCATION]
                            WIPTransaction.ModifyLotComponentSystemAttribute(lotData, lsComponetData[i], "LOCATION", location, txnStamp);

                            //更新欄位[SN]
                            WIPTransaction.ModifyLotComponentSystemAttribute(lotData, lsComponetData[i], "SN", newSN, txnStamp);

                            //更新欄位[FORGING_FLAG]
                            WIPTransaction.ModifyLotComponentSystemAttribute(lotData, lsComponetData[i], "FORGING_FLAG", "Y", txnStamp);
                        }
                        #endregion

                        #region 取得機台資訊
                        //取得機台名稱
                        var equipmentName = EquipmantMapping(Line);

                        //取得機台資訊
                        var equipmentData = EquipmentInfo.GetEquipmentByName(equipmentName).ChangeTo<EquipmentInfoEx>();

                        if (equipmentData == null)
                        {
                            //[00885]機台{0}不存在！
                            throw new Exception(TextMessage.Error.T00885(equipmentName));
                        }
                        #endregion

                        #region 執行變更機台狀態
                        var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("RUN");

                        if (equipmentData.CurrentState != "RUN")
                        {
                            //更新機台狀態
                            EMSTransaction.ChangeState(equipmentData, newStateInfo, txnStamp);
                        }
                        #endregion

                        //批號上機台
                        EMSTxn.Default.AddLotToEquipment(lotData, equipmentData, txnStamp);

                        //執行進站動作
                        WIPTransaction.CheckIn(lotData, equipmentData.EquipmentName, "", "", LotDefaultStatus.Run.ToString(), txnStamp);

                        //跳至下一個規則
                        WIPTransaction.DispatchLot(lotData, txnStamp);
                    }
                    #endregion

                    #region 執行併批動作

                    var lsLotData = LotInfoEx.GetLotListByInvertoryLot(lotData.Lot);

                    //取得併批的批號
                    var waitMergeLots = lsLotData.FindAll(p => p.Status == LotDefaultStatus.Wait.ToString() && p.Lot != lotData.Lot);
                    waitMergeLots.Sort(p => p.Lot);

                    List<MergeLotInfo> lsMergeLots = new List<MergeLotInfo>();

                    List<MergeComponentInfo> lsMergeComponents = new List<MergeComponentInfo>();

                    foreach (var waitMergeLot in waitMergeLots)
                    {
                        var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");

                        //批號子單元清單
                        var lsComponetData = ComponentInfoEx.GetDataByCurrentLot(waitMergeLot.Lot);
                        lsComponetData.Sort(p => p.ComponentID);

                        #region 更新批號子單元資料[LOCATION]及[SN]
                        for (int i = 0; i < lsComponetData.Count; i++)
                        {
                            string location = "A1";
                            string newSN = "1";

                            #region 取得最後一筆工件的SN
                            var firstComponentDatas = ComponentInfoEx.GetDataByLocation(location);

                            if (firstComponentDatas.Count == 0)
                            {
                                //鍛造位置：{0} 無任何工件序號
                                //throw new Exception(RuleMessage.Error.C10048(location));
                            }
                            else
                            {
                                decimal dSN = 0;
                                if (decimal.TryParse(firstComponentDatas[firstComponentDatas.Count - 1].SN, out dSN))
                                {
                                    //取得新的SN編號
                                    newSN = (dSN + 1).ToString();
                                }
                            }
                            #endregion

                            //更新欄位[LOCATION]
                            WIPTransaction.ModifyLotComponentSystemAttribute(waitMergeLot, lsComponetData[i], "LOCATION", location, txnStamp);

                            //更新欄位[SN]
                            WIPTransaction.ModifyLotComponentSystemAttribute(waitMergeLot, lsComponetData[i], "SN", newSN, txnStamp);

                            //更新欄位[FORGING_FLAG]
                            WIPTransaction.ModifyLotComponentSystemAttribute(waitMergeLot, lsComponetData[i], "FORGING_FLAG", "Y", txnStamp);
                        }
                        #endregion

                        //執行進站動作
                        WIPTransaction.CheckIn(waitMergeLot, "", "", LotDefaultStatus.Run.ToString(), txnStamp);

                        //跳至下一個規則
                        WIPTransaction.DispatchLot(waitMergeLot, txnStamp);

                        //加入併批清單
                        lsMergeLots.Add(MergeLotInfo.GetMergeLotByLotAndQuantity(waitMergeLot.Lot, lsComponetData, reasonCategory, reasonCategory.Description));
                    }

                    //將數量合併到在狀態為Run的批號上
                    if (lsMergeLots.Count > 0)
                    {
                        WIPTransaction.MergeLot(lotData, lsMergeLots, txnStamp);
                    }

                    #endregion

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 台車位置
        /// </summary>
        /// <param name="CarrierNO">台車編號</param>
        /// <param name="Location">位置(1/2/3/5/6/9/10/11/12)</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SetCarrierNO(string CarrierNO, string Location, string Line)
        {
            string ProgramRight = "SetCarrierNO";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認台車是否存在
                var carrierData = CarrierInfo.GetCarrierByCarrierNo(CarrierNO);

                if (carrierData == null)
                {
                    ////台車編號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10045(CarrierNO));
                }
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //變更位置內容
                    CMSTransaction.ModifyAttributeSystemCarrier(carrierData, "LOCATION", Location, txnStamp);

                    cts.Complete();

                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 編輯工單
        /// </summary>
        /// <param name="WO">工單編號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SuspendWorkOrders(string WO, string Line)
        {
            string ProgramRight = "SuspendWorkOrders";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    var eqpWorkOrderData = CSTEQPWOAssignInfo.GetDataByWorkOrder(WO);
                    if (eqpWorkOrderData == null)
                    {
                        //[00187]工單:{0}不存在!
                        throw new Exception(TextMessage.Error.T00187(WO));
                    }

                    var eqpWorkOrderDataLog = eqpWorkOrderData.Fill<CSTEQPWOAssignLogInfo>();
                    //刪除資料
                    eqpWorkOrderData.DeleteFromDB();
                    //新增一筆LOG記錄
                    eqpWorkOrderDataLog.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 空車資訊
        /// </summary>
        /// <param name="CarrierNO">台車編號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string NullCarrierNO(string CarrierNO, string Line)
        {
            string ProgramRight = "NullCarrierNO";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                //To do...
                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 確認高熟爐線別[回傳批號數量、批號及確認結果(1:OK 2:NG)]
        /// </summary>
        /// <param name="CarrierNO">掃描器讀取的台車編號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckStoveLine(string CarrierNO, string Line)
        {
            string ProgramRight = "CheckStoveLine";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            //預設值為NG
            string resultCheckLine = "2";

            try
            {
                #region 確認台車是否存在
                var carrierData = CarrierInfo.GetCarrierByCarrierNo(CarrierNO);

                if (carrierData == null)
                {
                    //台車編號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10045(CarrierNO));
                }
                #endregion

                #region 確認台車是否有掛載批號
                var carrierLotDatas = CarrierLotInfo.GetCarrierLotListByCarrier(CarrierNO);

                if (carrierLotDatas.Count == 0)
                {
                    //台車編號：{0} 沒有掛載任何批號!!
                    throw new Exception(RuleMessage.Error.C10046(CarrierNO));
                }
                #endregion

                #region 確認掛載批號是否存在，如果存在，則取得該批號對應的工單線別
                lot = carrierLotDatas[0].LOT;

                var lotData = LotInfo.GetLotByLot(carrierLotDatas[0].LOT);

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(carrierLotDatas[0].LOT));
                }


                operationName = lotData.OperationName;

                var workOrderData = WorkOrderInfo.GetWorkOrderByWorkOrder(lotData.WorkOrder).ChangeTo<WorkOrderInfoEx>();

                if (workOrderData == null)
                {
                    //[00187]工單:{0}不存在!
                    throw new Exception(TextMessage.Error.T00187(lotData.WorkOrder));
                }

                if (Line == workOrderData.Division)
                {
                    resultCheckLine = "1";
                }

                #endregion

                return JsonConvert.SerializeObject(new
                {
                    Result = true,
                    Qty = lotData.Quantity.ToString(),
                    Lot = lotData.Lot,
                    CheckLine = resultCheckLine,
                    Message = ""
                });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new
                {
                    Result = false,
                    Qty = "",
                    Lot = "",
                    CheckLine = resultCheckLine,
                    Message = ex.Message + returnMessage
                });
            }
        }

        /// <summary>
        /// 高熱爐-溫度
        /// </summary>
        /// <param name="CarrierNO">台車編號</param>
        /// <param name="Temp1">倒數第1隻的溫度</param>
        /// <param name="Temp2">倒數第2隻的溫度</param>
        /// <param name="TempWater">水溫</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OutputT4Temp(string CarrierNO, string Temp1, string Temp2, string TempWater, string Line)
        {
            string ProgramRight = "OutputT4Temp";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認台車是否存在
                var carrierData = CarrierInfo.GetCarrierByCarrierNo(CarrierNO);

                if (carrierData == null)
                {
                    //台車編號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10045(CarrierNO));
                }
                #endregion

                #region 確認台車是否有掛載批號
                var carrierLotDatas = CarrierLotInfo.GetCarrierLotListByCarrier(CarrierNO);

                if (carrierLotDatas.Count == 0)
                {
                    //台車編號：{0} 沒有掛載任何批號!!
                    throw new Exception(RuleMessage.Error.C10046(CarrierNO));
                }
                #endregion

                #region 確認掛載批號是否存在
                lot = carrierLotDatas[0].LOT;

                var lotData = LotInfo.GetLotByLot(carrierLotDatas[0].LOT);

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(carrierLotDatas[0].LOT));
                }

                operationName = lotData.OperationName;
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //將溫度記錄在[CST_CMS_TEMP_TIME]及[CST_CMS_TEMP_TIME_LOG]

                    CSTCMSTempTimeInfo CMSTempTimeData = null;

                    CMSTempTimeData = CSTCMSTempTimeInfo.GetDataByCMSLotSID(carrierLotDatas[0].CMS_LOT_SID);

                    if (CMSTempTimeData == null)
                    {
                        CMSTempTimeData = InfoCenter.Create<CSTCMSTempTimeInfo>();
                    }

                    //記錄倒數第1隻的溫度
                    CMSTempTimeData.TEMP1 = Temp1;

                    //記錄倒數第2隻的溫度
                    CMSTempTimeData.TEMP2 = Temp2;

                    //水溫
                    CMSTempTimeData.TempWater = TempWater;

                    //記錄載具批號系統編號
                    CMSTempTimeData.CMSLotSID = carrierLotDatas[0].CMS_LOT_SID;

                    if (CMSTempTimeData.InfoState == InfoState.NewCreate)
                    {
                        //新增一筆[CST_CMS_TEMP_TIME]
                        CMSTempTimeData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }
                    else
                    {
                        //更新資料[CST_CMS_TEMP_TIME]
                        CMSTempTimeData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }

                    var CMSTempTimeLog = CMSTempTimeData.Fill<CSTCMSTempTimeLogInfo>();

                    //新增一筆[CST_CMS_TEMP_TIME_LOG]
                    CMSTempTimeLog.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    cts.Complete();

                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 高熱爐-時間
        /// </summary>
        /// <param name="CarrierNO">台車編號</param>
        /// <param name="TransportTime">高熱爐開門到下水時間</param>
        /// <param name="InWaterTime">浸水時間</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OutputT4Time(string CarrierNO, string TransportTime, string InWaterTime, string Line)
        {
            string ProgramRight = "OutputT4Time";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認台車是否存在
                var carrierData = CarrierInfo.GetCarrierByCarrierNo(CarrierNO);

                if (carrierData == null)
                {
                    //台車編號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10045(CarrierNO));
                }
                #endregion

                #region 確認台車是否有掛載批號
                var carrierLotDatas = CarrierLotInfo.GetCarrierLotListByCarrier(CarrierNO);

                if (carrierLotDatas.Count == 0)
                {
                    //台車編號：{0} 沒有掛載任何批號!!
                    throw new Exception(RuleMessage.Error.C10046(CarrierNO));
                }
                #endregion

                #region 確認掛載批號是否存在
                lot = carrierLotDatas[0].LOT;

                var lotData = LotInfo.GetLotByLot(carrierLotDatas[0].LOT);

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(carrierLotDatas[0].LOT));
                }


                operationName = lotData.OperationName;

                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //將溫度記錄在[CST_CMS_TEMP_TIME]及[CST_CMS_TEMP_TIME_LOG]

                    CSTCMSTempTimeInfo CMSTempTimeData = null;

                    CMSTempTimeData = CSTCMSTempTimeInfo.GetDataByCMSLotSID(carrierLotDatas[0].CMS_LOT_SID);

                    if (CMSTempTimeData == null)
                    {
                        CMSTempTimeData = InfoCenter.Create<CSTCMSTempTimeInfo>();
                    }

                    //高熱爐開門到下水時間
                    CMSTempTimeData.TransportTime = TransportTime;

                    //浸水時間
                    CMSTempTimeData.InWaterTime = InWaterTime;

                    //記錄載具批號系統編號
                    CMSTempTimeData.CMSLotSID = carrierLotDatas[0].CMS_LOT_SID;

                    if (CMSTempTimeData.InfoState == InfoState.NewCreate)
                    {
                        //新增一筆[CST_CMS_TEMP_TIME]
                        CMSTempTimeData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }
                    else
                    {
                        //更新資料[CST_CMS_TEMP_TIME]
                        CMSTempTimeData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }

                    var CMSTempTimeLog = CMSTempTimeData.Fill<CSTCMSTempTimeLogInfo>();

                    //新增一筆[CST_CMS_TEMP_TIME_LOG]
                    CMSTempTimeLog.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    cts.Complete();

                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 取得工件序號
        /// </summary>
        /// <param name="Location">擠型:A1, 粗鍛:B1, 細鍛:C1 </param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetItemSN(string Location, string Line)
        {
            string ProgramRight = "GetItemSN";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 取得未領用的第一筆工件序號
                var componentData = ComponentInfoEx.GetDataByLocationAndSNFlag(Location.Replace("_1", ""), "N");

                if (componentData == null)
                {
                    //鍛造位置：{0} 無任何工件序號
                    throw new Exception(RuleMessage.Error.C10048(Location.Replace("_1", "")));
                }
                itemSN = componentData.CPFSN;
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot);

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }

                operationName = lotData.OperationName;

                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //設置工件所在位置
                    SetLocation(componentData, Location, txnStamp, "Y");

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "", ItemSN = componentData.CPFSN });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 設定工件位置
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Location">例:B5/C4/C7/….</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SetLocation(string ItemSN, string Location, string Line)
        {
            string ProgramRight = "SetLocation";
            string operationName = "";
            string lot = "";

            //轉換為四碼
            ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);

                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot);

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }

                operationName = lotData.OperationName;
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //設置工件所在位置
                    SetLocation(componentData, Location, txnStamp);

                    //Location為Q1,Q2,Q3需開品檢預設的檢驗單
                    if (Location.StartsWith("Q"))
                    {
                        var lstMolds = ToolInfoEx.GetToolsByEquipmentAndLocation(lotData.ResourceName, Location);

                        //先抓檢驗類型鍛造的預設品檢單
                        var QCTypeData = QCTypeInfo.GetInfoByName("QC_Default");
                        if (QCTypeData == null)
                        {
                            //查無鍛造品檢預設的檢驗類型 !                            
                            throw new Exception(RuleMessage.Error.C10188());
                        }
                        
                        //依照檢驗計畫取得該物件的datatable
                        DataTable dtQCTarget = CustomizeFunction.GetInspectionData(QCTypeData, componentData.ComponentID);

                        List<string> controlItems = QCTypeInfo.GenerateControlItems(QCTypeData, dtQCTarget);
                        var qcPlanSetups = QCPlanSetupInfo.GetActiveVersionQCPlanInfos(QCTypeData.QCTYPE, controlItems.ToArray());
                        if (qcPlanSetups.Count == 0)
                        {
                            //查無{0}資訊!
                            throw new Exception(TextMessage.Error.T00550("QCPlan"));
                        }
                        var planVersionSetupData = QCPlanSetupVersionInfo.GetActiveVersionInfo(qcPlanSetups[0]);
                        
                        #region 產生檢驗單資料
                        var inspection = InfoCenter.Create<QCInspectionInfo>().ChangeTo<QCInspectionInfoEx>();
                        inspection.InspectionNo = DBCenter.GetSystemID();
                        inspection.QCTYPE = QCTypeData.QCTYPE;
                        inspection.ShiftDate = DBCenter.GetSystemTime().Substring(0, 10);
                        inspection.QC_PLAN_SETUP_VER_SID = planVersionSetupData.ID;
                        inspection.OperationName = lotData.OperationName;
                        inspection.EquipmentName = lotData.ResourceName;
                        inspection.DeviceName = lotData.DeviceName;
                        inspection.Status = "WaitQC";
                        inspection.CreateUser = txnStamp.UserID;
                        inspection.CreateTime = txnStamp.RecordTime;
                        inspection["MOLD1"] = lstMolds.Count > 0 ? lstMolds[0].ToolName : "";
                        inspection["MOLD2"] = lstMolds.Count > 1 ? lstMolds[1].ToolName : "";
                        inspection.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                        QCInspectionObjectInfo inspectionObject = QCInspectionObjectInfo.CreateInspectionObject(planVersionSetupData, dtQCTarget);
                        inspectionObject.QC_INSP_SID = inspection.QC_INSP_SID;
                        inspectionObject.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                        #endregion
                    }

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 重工放入
        /// </summary>
        /// <param name="Location">例:I0/I1/I2</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string InputItem(string Location, string Line)
        {
            string ProgramRight = "InputItem";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 取得未領用的第一筆工件序號
                var componentData = ComponentInfoEx.GetDataByLocationAndSNFlag(Location, "N");

                if (componentData == null)
                {
                    //鍛造位置：{0} 無任何工件序號
                    throw new Exception(RuleMessage.Error.C10048(Location));
                }
                itemSN = componentData.CPFSN;
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot);

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    #region 取得機台資訊
                    //取得機台名稱
                    var equipmentName = EquipmantMapping(Line);

                    //取得機台資訊
                    var equipmentData = EquipmentInfo.GetEquipmentByName(equipmentName).ChangeTo<EquipmentInfoEx>();

                    if (equipmentData == null)
                    {
                        //[00885]機台{0}不存在！
                        throw new Exception(TextMessage.Error.T00885(equipmentName));
                    }
                    #endregion

                    #region 執行變更機台狀態
                    var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("RUN");

                    if (equipmentData.CurrentState != "RUN")
                    {
                        //更新機台狀態
                        EMSTransaction.ChangeState(equipmentData, newStateInfo, txnStamp);
                    }
                    #endregion

                    //批號上機台
                    EMSTxn.Default.AddLotToEquipment(lotData, equipmentData, txnStamp);

                    //執行進站動作
                    WIPTransaction.CheckIn(lotData, equipmentData.EquipmentName, "", "", LotDefaultStatus.Run.ToString(), txnStamp);

                    //跳至下一個規則
                    WIPTransaction.DispatchLot(lotData, txnStamp);

                    string newSN = "1";

                    #region 取得最後一筆工件的SN
                    var firstComponentDatas = ComponentInfoEx.GetDataByLocation(Location);

                    if (firstComponentDatas.Count == 0)
                    {
                        //鍛造位置：{0} 無任何工件序號
                        //throw new Exception(RuleMessage.Error.C10048(location));
                    }
                    else
                    {
                        decimal dSN = 0;
                        if (decimal.TryParse(firstComponentDatas[firstComponentDatas.Count - 1].SN, out dSN))
                        {
                            //取得新的SN編號
                            newSN = (dSN + 1).ToString();
                        }
                    }
                    #endregion

                    //更新欄位[LOCATION]
                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "LOCATION", Location, txnStamp);

                    //更新欄位[SN]
                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "SN", newSN, txnStamp);

                    //更新欄位[FORGING_FLAG]
                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "FORGING_FLAG", "Y", txnStamp);

                    //更新欄位[SNFLAG]
                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "SNFLAG", "Y", txnStamp);

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "", ItemSN = componentData.CPFSN });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 異常排出
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Location">例:O1/O2/O3</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OutputErrorItem(string ItemSN, string Location, string Line)
        {
            string ProgramRight = "OutputErrorItem";
            string operationName = "";
            string lot = "";

            //轉換為四碼
            ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);
                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot).ChangeTo<LotInfoEx>();
                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;
                #endregion

                //有找到範圍在01-15之間的工件編號
                bool findInterval1 = false;

                //有找到範圍在16-30之間的工件編號
                bool findInterval2 = false;

                //取工件編號前二碼
                string SN = ItemSN.Substring(0, 2);

                //確認C10邏輯
                CheckC10Rule(SN, ref findInterval1, ref findInterval2);

                //取得原因碼
                var reason = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "Forging_NG");

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //拆批
                    var split = DefectNG(lotData, componentData, reason, txnStamp);

                    //重新取得子批資料
                    var splitData = LotInfo.GetLotByLot(split.Lot);

                    if (splitData.Status == LotDefaultStatus.Run.ToString())
                    {
                        //執行出站
                        WIPTransaction.CheckOut(splitData, txnStamp);

                        //批號分派
                        WIPTransaction.DispatchLot(splitData, txnStamp);

                        //更新欄位[FORGING_FLAG]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "FORGING_FLAG", "N", txnStamp);

                        #region 計算持溫時間
                        string accTime = "";
                        string endTime = DBCenter.GetSystemTime();
                        string startTime = componentData["CPF_STARTTIME"].ToString();

                        var dtStartTime = Convert.ToDateTime(startTime);
                        var dtEndTime = Convert.ToDateTime(endTime);
                        TimeSpan sp = new TimeSpan(dtEndTime.Ticks - dtStartTime.Ticks);

                        accTime = sp.TotalMinutes.ToString();

                        //更新欄位[CPF_ENDTIME]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ENDTIME", endTime, txnStamp);

                        //更新欄位[CPF_ACCTIME]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ACCTIME", accTime, txnStamp);
                        #endregion
                    }

                    //變更工作站至待判
                    ReassignJudgeOperation(lotData, splitData, reason, txnStamp);

                    SetLocation(componentData, Location, txnStamp);

                    cts.Complete();
                }
                #endregion  

                return JsonConvert.SerializeObject(new { Result = true, Message = "", ChangeCarFlag = !findInterval1, ChangeLotFlag = !findInterval2 });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 細鍛淬水排出口O4
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Location">例:O1/O2/O3</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OutputO4Item(string ItemSN, string Location, string Line)
        {
            string ProgramRight = "OutputO4Item";
            string operationName = "";
            string lot = "";

            //轉換為四碼
            ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);
                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot).ChangeTo<LotInfoEx>();
                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;
                #endregion

                //取得原因碼
                var reason = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "Forging_NG");

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //拆批
                    var split = DefectNG(lotData, componentData, reason, txnStamp);

                    //重新取得子批資料
                    var splitData = LotInfo.GetLotByLot(split.Lot);

                    if (splitData.Status == LotDefaultStatus.Run.ToString())
                    {
                        //執行出站
                        WIPTransaction.CheckOut(splitData, txnStamp);

                        //批號分派
                        WIPTransaction.DispatchLot(splitData, txnStamp);

                        //更新欄位[FORGING_FLAG]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "FORGING_FLAG", "N", txnStamp);

                        #region 計算持溫時間
                        string accTime = "";
                        string endTime = DBCenter.GetSystemTime();
                        string startTime = componentData["CPF_STARTTIME"].ToString();

                        var dtStartTime = Convert.ToDateTime(startTime);
                        var dtEndTime = Convert.ToDateTime(endTime);
                        TimeSpan sp = new TimeSpan(dtEndTime.Ticks - dtStartTime.Ticks);

                        accTime = sp.TotalMinutes.ToString();

                        //更新欄位[CPF_ENDTIME]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ENDTIME", endTime, txnStamp);

                        //更新欄位[CPF_ACCTIME]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ACCTIME", accTime, txnStamp);
                        #endregion
                    }

                    SetLocation(componentData, Location, txnStamp);

                    cts.Complete();
                }
                #endregion  

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 檢驗排出
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Location">例:Q1/Q2/Q3</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OutputQCItem(string ItemSN, string Location, string Line)
        {
            string ProgramRight = "OutputQCItem";
            string operationName = "";
            string lot = "";

            //轉換為四碼
            ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));
            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);

                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot).ChangeTo<LotInfoEx>();

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;
                #endregion

                //有找到範圍在01-15之間的工件編號
                bool findInterval1 = false;

                //有找到範圍在16-30之間的工件編號
                bool findInterval2 = false;

                //取工件編號前二碼
                string SN = ItemSN.Substring(0, 2);

                //確認C10邏輯
                CheckC10Rule(SN, ref findInterval1, ref findInterval2);

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //設置工件所在位置
                    SetLocation(componentData, Location, txnStamp);

                    #region 送待判站
                    //取得原因碼
                    var reason = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "Forging_NG");

                    //拆批
                    var split = DefectNG(lotData, componentData, reason, txnStamp);

                    //重新取得子批資料
                    var splitData = LotInfo.GetLotByLot(split.Lot);

                    if (splitData.Status == LotDefaultStatus.Run.ToString())
                    {
                        //執行出站
                        WIPTransaction.CheckOut(splitData, txnStamp);

                        //批號分派
                        WIPTransaction.DispatchLot(splitData, txnStamp);

                        //更新欄位[FORGING_FLAG]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "FORGING_FLAG", "N", txnStamp);

                        #region 計算持溫時間
                        string accTime = "";
                        string endTime = DBCenter.GetSystemTime();
                        string startTime = componentData["CPF_STARTTIME"].ToString();

                        var dtStartTime = Convert.ToDateTime(startTime);
                        var dtEndTime = Convert.ToDateTime(endTime);
                        TimeSpan sp = new TimeSpan(dtEndTime.Ticks - dtStartTime.Ticks);

                        accTime = sp.TotalMinutes.ToString();

                        //更新欄位[CPF_ENDTIME]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ENDTIME", endTime, txnStamp);

                        //更新欄位[CPF_ACCTIME]
                        WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ACCTIME", accTime, txnStamp);
                        #endregion
                    }

                    //變更工作站至待判
                    ReassignJudgeOperation(lotData, splitData, reason, txnStamp);
                    #endregion

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "", ChangeCarFlag = !findInterval1, ChangeLotFlag = !findInterval2 });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 檢驗結果通知
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Result">OK/NG</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string QCResult(string ItemSN, string Result, string Line)
        {
            string ProgramRight = "QCResult";
            string operationName = "";
            string lot = "";

            //轉換為四碼
            ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);
                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot).ChangeTo<LotInfoEx>();
                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;
                #endregion

                //有找到範圍在01-15之間的工件編號
                bool findInterval1 = false;

                //有找到範圍在16-30之間的工件編號
                bool findInterval2 = false;

                //取工件編號前二碼
                string SN = ItemSN.Substring(0, 2);

                //取得原因碼
                var reason = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "Forging_NG");

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    if (Result == "NG")
                    {
                        //確認C10邏輯
                        CheckC10Rule(SN, ref findInterval1, ref findInterval2);

                        //拆批
                        var split = DefectNG(lotData, componentData, reason, txnStamp);

                        //重新取得子批資料
                        var splitData = LotInfo.GetLotByLot(split.Lot);

                        if (splitData.Status == LotDefaultStatus.Run.ToString())
                        {
                            //執行出站
                            WIPTransaction.CheckOut(splitData, txnStamp);

                            //批號分派
                            WIPTransaction.DispatchLot(splitData, txnStamp);

                            //更新欄位[FORGING_FLAG]
                            WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "FORGING_FLAG", "N", txnStamp);

                            #region 計算持溫時間
                            string accTime = "";
                            string endTime = DBCenter.GetSystemTime();
                            string startTime = componentData["CPF_STARTTIME"].ToString();

                            var dtStartTime = Convert.ToDateTime(startTime);
                            var dtEndTime = Convert.ToDateTime(endTime);
                            TimeSpan sp = new TimeSpan(dtEndTime.Ticks - dtStartTime.Ticks);

                            accTime = sp.TotalMinutes.ToString();

                            //更新欄位[CPF_ENDTIME]
                            WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ENDTIME", endTime, txnStamp);

                            //更新欄位[CPF_ACCTIME]
                            WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ACCTIME", accTime, txnStamp);
                            #endregion
                        }

                        //變更工作站至待判
                        ReassignJudgeOperation(lotData, splitData, reason, txnStamp);

                    }
                    else
                    {
                        //預設為TRUE
                        findInterval1 = true;
                        findInterval2 = true;
                    }

                    #region QC
                    var qcInsp = QCInspectionInfoEx.GetCPFQCDataByObjectName(componentData.ComponentID);
                    if (qcInsp != null)
                    {
                        qcInsp.Result = Result;
                        qcInsp.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }
                    #endregion

                    cts.Complete();
                }
                #endregion  

                return JsonConvert.SerializeObject(new { Result = true, Message = "", ChangeCarFlag = !findInterval1, ChangeLotFlag = !findInterval2 });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 細鍛檢驗排出口(Q3) 排出模式(手動上料)
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Location">例:Q1/Q2/Q3</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OutputNormalItem(string ItemSN, string Location, string Line)
        {
            string ProgramRight = "OutputNormalItem";
            string operationName = "";
            string lot = "";

            //轉換為四碼
            ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));
            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);

                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot).ChangeTo<LotInfoEx>();

                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //設置工件所在位置
                    SetLocation(componentData, Location, txnStamp);

                    #region 計算持溫時間
                    string accTime = "";
                    string endTime = DBCenter.GetSystemTime();
                    string startTime = componentData["CPF_STARTTIME"].ToString();

                    var dtStartTime = Convert.ToDateTime(startTime);
                    var dtEndTime = Convert.ToDateTime(endTime);
                    TimeSpan sp = new TimeSpan(dtEndTime.Ticks - dtStartTime.Ticks);

                    accTime = sp.TotalMinutes.ToString();

                    //更新欄位[CPF_ENDTIME]
                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "CPF_ENDTIME", endTime, txnStamp);

                    //更新欄位[CPF_ACCTIME]
                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "CPF_ACCTIME", accTime, txnStamp);
                    #endregion

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// MES執行出站與送待判站
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckOutForging(string ItemSN, string Line)
        {
            string ProgramRight = "CheckOutForging";
            string operationName = "";
            string lot = "";

            //轉換為四碼
            ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);
                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }
                #endregion

                #region 確認工件序號所屬批號是否存在
                lot = componentData.CurrentLot;

                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot).ChangeTo<LotInfoEx>();
                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;
                #endregion

                //取得原因碼
                var reason = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("CustomizeReason", "Forging_NG");

                //有找到範圍在01-15之間的工件編號
                bool findInterval1 = false;

                //有找到範圍在16-30之間的工件編號
                bool findInterval2 = false;

                //取工件編號前二碼
                string SN = ItemSN.Substring(0, 2);

                //確認C10邏輯
                CheckC10Rule(SN, ref findInterval1, ref findInterval2);

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //拆批
                    var split = DefectNG(lotData, componentData, reason, txnStamp);

                    //重新取得子批資料
                    var splitData = LotInfo.GetLotByLot(split.Lot);

                    //執行出站
                    WIPTransaction.CheckOut(splitData, txnStamp);

                    //批號分派
                    WIPTransaction.DispatchLot(splitData, txnStamp);

                    //更新欄位[FORGING_FLAG]
                    WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "FORGING_FLAG", "N", txnStamp);

                    //更新欄位[LOCATION]
                    WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "LOCATION", "", txnStamp);

                    //更新欄位[SN]
                    WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "SN", "", txnStamp);

                    //更新欄位[SNFLAG]
                    WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "SNFLAG", "N", txnStamp);

                    #region 計算持溫時間
                    string accTime = "";
                    string endTime = DBCenter.GetSystemTime();
                    string startTime = componentData["CPF_STARTTIME"].ToString();

                    var dtStartTime = Convert.ToDateTime(startTime);
                    var dtEndTime = Convert.ToDateTime(endTime);
                    TimeSpan sp = new TimeSpan(dtEndTime.Ticks - dtStartTime.Ticks);

                    accTime = sp.TotalMinutes.ToString();

                    //更新欄位[CPF_ENDTIME]
                    WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ENDTIME", endTime, txnStamp);

                    //更新欄位[CPF_ACCTIME]
                    WIPTransaction.ModifyLotComponentSystemAttribute(splitData, componentData, "CPF_ACCTIME", accTime, txnStamp);
                    #endregion

                    //變更工作站至待判
                    ReassignJudgeOperation(lotData, splitData, reason, txnStamp);

                    cts.Complete();
                }
                #endregion  

                return JsonConvert.SerializeObject(new { Result = true, Message = "", ChangeCarFlag = !findInterval1, ChangeLotFlag = !findInterval2 });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 台車上批號
        /// </summary>
        /// <param name="ItemSN">工件編號</param>
        /// <param name="Location">位置</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CarAddComponent(string ItemSN, string Location, string Line)
        {
            string ProgramRight = "CarAddComponent";
            string operationName = "";
            string lot = "";

            //轉換為四碼
            ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);
                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }

                lot = componentData.CurrentLot;

                //確認工件序號所屬批號是否存在
                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot).ChangeTo<LotInfoEx>();
                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //設置工件所在位置
                    SetLocation(componentData, Location, txnStamp);

                    #region 計算持溫時間
                    string accTime = "";
                    string endTime = DBCenter.GetSystemTime();
                    string startTime = componentData["CPF_STARTTIME"].ToString();

                    var dtStartTime = Convert.ToDateTime(startTime);
                    var dtEndTime = Convert.ToDateTime(endTime);
                    TimeSpan sp = new TimeSpan(dtEndTime.Ticks - dtStartTime.Ticks);

                    accTime = sp.TotalMinutes.ToString();

                    //更新欄位[CPF_ENDTIME]
                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "CPF_ENDTIME", endTime, txnStamp);

                    //更新欄位[CPF_ACCTIME]
                    WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "CPF_ACCTIME", accTime, txnStamp);
                    #endregion

                    string carSeq = "0";
                    if(1<= ItemSN.Substring(2, 2).ToCimesDecimal() && ItemSN.Substring(2, 2).ToCimesDecimal() <= 15)
                    {
                        carSeq = "1";
                    }
                    else
                    {
                        carSeq = "2";
                    }

                    //取得可使用的台車編號
                    var carProcessData = CSTCMSCarProcessInfo.GetDataByLoadLotFlagIsN(Line, carSeq);
                    if (carProcessData == null)
                    {
                        //[CST_CMS_CAR_PROCESS]查無台車可使用!
                        throw new Exception(RuleMessage.Error.C10165());
                    }

                    //取得台車資料
                    var carrierData = CarrierInfo.GetCarrierByCarrierNo(carProcessData.CarNo);

                    //取得台車上批號資料
                    var carrierLots = CarrierLotInfo.GetCarrierLotListByCarrier(carrierData.CarrierNO);

                    //確認批號是否己經上台車
                    var carLot = carrierLots.Find(p => p.LOT == lot);
                    if (carLot == null)
                    {
                        CMSTransaction.Load(carrierData, lotData, 0, true, txnStamp);

                        //若是台車狀態不是 USED，變更台車狀態
                        if (carrierData.State != "USED")
                        {
                            CarrierStateInfo carrierState = CarrierStateInfo.GetCarrierStateByState("USED");
                            CMSTransaction.ChangeCarrierState(carrierData, carrierState, txnStamp);
                        }

                        //註記台車編號(因為一批會有可能上2台車，所以不記錄)
                        //WIPTransaction.ModifyLotSystemAttribute(lotData, "CARRIER", carProcessData.CarNo, txnStamp);
                    }

                    //更新欄位[CAR_NO]
                    //WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "CAR_NO", carrierData.CarrierNO, txnStamp);
                    componentData["CAR_NO"] = carrierData.CarrierNO;
                    componentData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    //if (ChangeFlag.ToBool())
                    //{
                    //    //更新[CST_CMS_CAR_PROCESS]資料
                    //    carProcessData.LoadLotFlag = "Y";
                    //    carProcessData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
                    //}

                    cts.Complete();
                }
                #endregion  

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 更新台車資料旗標
        /// </summary>
        /// <param name="CarSequence">車次(1:第一台, 2:第二台)</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateLoadLotFlag(string CarSequence, string Line)
        {
            string ProgramRight = "UpdateLoadLotFlag";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //取得可使用的台車編號
                    var carProcessData = CSTCMSCarProcessInfo.GetDataByLoadLotFlagIsN(Line, CarSequence);
                    if (carProcessData == null)
                    {
                        //[CST_CMS_CAR_PROCESS]查無台車可使用!
                        throw new Exception(RuleMessage.Error.C10165());
                    }

                    //更新[CST_CMS_CAR_PROCESS]資料
                    carProcessData.LoadLotFlag = "Y";
                    carProcessData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

                    //如果傳入車次為第二台，則執行出站
                    if (CarSequence == "2")
                    {
                        //取得前一次使用的台車編號
                        var carFirstProcessData = CSTCMSCarProcessInfo.GetDataByLoadLotFlagIsY(Line, "1");

                        //取得台車上批號資料
                        var carrierLots = CarrierLotInfo.GetCarrierLotListByCarrier(carFirstProcessData.CarNo);
                        if (carrierLots.Count == 0)
                        {
                            //台車編號：{0} 沒有掛載任何批號!!
                            throw new Exception(RuleMessage.Error.C10046(carFirstProcessData.CarNo));
                        }

                        #region 確認工件序號所屬批號是否存在
                        lot = carrierLots.OrderByDescending(p=>p.CMS_LOT_SID).ToList()[0].LOT;

                        var lotData = LotInfo.GetLotByLot(lot).ChangeTo<LotInfoEx>();
                        if (lotData == null)
                        {
                            //[00378]批號{0}不存在！
                            throw new Exception(TextMessage.Error.T00378(lot));
                        }
                        #endregion

                        //因大爭尚未整合而執行的程式
                        carFirstProcessData.LoadLotFlag = "N";
                        carFirstProcessData.UpdateToDB();
                        carFirstProcessData = CSTCMSCarProcessInfo.GetDataByLoadLotFlagIsY(Line, "2");
                        carFirstProcessData.LoadLotFlag = "N";
                        carFirstProcessData.UpdateToDB();

                        #region 取得機台資訊
                        
                        var equipmentLot = EquipmentLotInfo.GetEquipmentLotByLot(lotData.Lot);

                        //如果批號有上機台資料的話，則執行批號下機台
                        if (equipmentLot != null)
                        {
                            //取得機台資訊
                            var equipmentData = EquipmentInfo.GetEquipmentByName(equipmentLot.EquipmentName);

                            if (equipmentData == null)
                            {
                                //[00885]機台{0}不存在！
                                throw new Exception(TextMessage.Error.T00885(equipmentLot.EquipmentName));
                            }

                            //驗證機台啟用狀態
                            if (equipmentData.UsingStatus == UsingStatus.Disable)
                            {
                                //機台：{0}已停用，如需使用，請至"機台資料維護"啟用!!
                                throw new Exception(RuleMessage.Error.C10025(equipmentData.EquipmentName));
                            }

                            //驗證機台狀態
                            if (equipmentData.CurrentState != "RUN")
                            {
                                //[00872]機台({0})目前的狀態({1})無法使用.
                                throw new Exception(TextMessage.Error.T00872(equipmentData.EquipmentName, equipmentData.CurrentState));
                            }

                            #endregion

                            //批號下機台
                            EMSTransaction.RemoveLotFromEquipment(lotData, equipmentData, txnStamp);

                            #region 執行變更機台狀態
                            var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("IDLE");

                            if (equipmentData.CurrentState != "IDLE" && equipmentData.Capacity == 0)
                            {
                                //更新機台狀態 Run -> IDLE
                                EMSTransaction.ChangeState(equipmentData, newStateInfo, txnStamp);
                            }
                            #endregion
                        }

                        //執行出站
                        WIPTransaction.CheckOut(lotData, txnStamp);

                        //批號分派
                        WIPTransaction.DispatchLot(lotData, txnStamp);

                        //批號子單元清單
                        var lsComponetData = ComponentInfoEx.GetDataByCurrentLot(lotData.Lot);

                        //更新批號子單元資料[FORGING_FLAG]
                        for (int i = 0; i < lsComponetData.Count; i++)
                        {
                            //更新欄位[FORGING_FLAG]
                            WIPTransaction.ModifyLotComponentSystemAttribute(lotData, lsComponetData[i], "FORGING_FLAG", "N", txnStamp);
                        }

                        //批號分派
                        WIPTransaction.DispatchLot(lotData, txnStamp);
                    }

                    cts.Complete();
                }
                #endregion  

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 台車資訊
        /// </summary>
        /// <param name="CarSequence">台車序號</param>
        /// <param name="CarNo">台車編號</param>
        /// <param name="Location">台車位置</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CarInformation(string CarSequence, string CarNo, string Location, string Line)
        {
            string ProgramRight = "CarInformation";
            string operationName = "";
            string lot = "";
            string itemSN = "";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);
                string linkSID = txnStamp.LinkSID;

                //轉換為三碼
                CarNo = string.Format("{0:000}", Convert.ToInt32(CarNo));

                #region 確認台車是否存在
                var carrierData = CarrierInfo.GetCarrierByCarrierNo(CarNo);

                if (carrierData == null)
                {
                    //台車編號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10045(CarNo));
                }
                #endregion

                #region 取得GroupSID
                if (CarSequence == "2")
                {
                    //取得前一個台車資訊
                    var CMSCarData = CSTCMSCarProcessInfo.GetDataByLine(Line);

                    linkSID = (CMSCarData == null) ? txnStamp.LinkSID : CMSCarData.GroupSID;
                }
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //新增一筆台車資訊
                    var insertData = InfoCenter.Create<CSTCMSCarProcessInfo>();
                    insertData.CarNo = CarNo;
                    insertData.CarSequence = CarSequence;
                    insertData.Line = Line;
                    insertData.GroupSID = linkSID;

                    insertData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    //變更位置內容
                    CMSTransaction.ModifyAttributeSystemCarrier(carrierData, "LOCATION", Location, txnStamp);

                    #region 將台車資料寫入EAP02
                    //從連線EAP
                    var EAP_Server = CSTAPIEAPServerInfo.GetDataByEAPNameAndLine("EAP2", Line);
                    Client eapClient = new Client(EAP_Server.IP, Convert.ToInt32(EAP_Server.Port));
                    //將結果轉換Json回傳至MES
                    string requestJson = JsonConvert.SerializeObject(new
                    {
                        Function = "CarInformation",
                        CarSequence = CarSequence,
                        CarNo = CarNo
                    });

                    var ResponseString = eapClient.ConnectToServer(requestJson);

                    var resultData = JsonConvert.DeserializeObject<ResultData>(ResponseString);
                    if (resultData.Result == false)
                    {
                        throw new CimesException(resultData.Message);
                    }
                    #endregion

                    cts.Complete();
                }
                #endregion  

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, itemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 確認換車換批資訊
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ChangeCarAndChangeLot(string ItemSN, string Line)
        {
            string ProgramRight = "ChangeCarAndChangeLot";
            string operationName = "";
            string lot = "";

            try
            {
                //轉換為四碼
                ItemSN = string.Format("{0:0000}", Convert.ToInt32(ItemSN));

                #region 確認工件序號是否存在
                var componentData = ComponentInfoEx.GetDataByCPFSN(ItemSN);
                if (componentData == null)
                {
                    //工件序號：{0} 不存在!!
                    throw new Exception(RuleMessage.Error.C10047(ItemSN));
                }
                #endregion

                #region 確認工件序號所屬批號是否存在
                var lotData = LotInfo.GetLotByLot(componentData.CurrentLot).ChangeTo<LotInfoEx>();
                if (lotData == null)
                {
                    //[00378]批號{0}不存在！
                    throw new Exception(TextMessage.Error.T00378(componentData.CurrentLot));
                }
                operationName = lotData.OperationName;
                #endregion

                //有找到範圍在01-15之間的工件編號
                bool findInterval1 = false;

                //有找到範圍在16-30之間的工件編號
                bool findInterval2 = false;

                //取工件編號前二碼
                string SN = ItemSN.Substring(0, 2);

                //確認C10邏輯
                CheckC10Rule(SN, ref findInterval1, ref findInterval2);

                return JsonConvert.SerializeObject(new { Result = true, Message = "", ChangeCarFlag = !findInterval1, ChangeLotFlag = !findInterval2 });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, lot, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 變更工作站至待判站
        /// </summary>
        /// <param name="lotData">批號資料</param>
        /// <param name="splitInfo">子批資料</param>
        /// <param name="reason">原因碼資料</param>
        /// <param name="txnStamp"></param>
        private void ReassignJudgeOperation(LotInfoEx lotData, LotInfo splitInfo, ReasonCategoryInfo reason, TransactionStamp txnStamp)
        {
            //取得代判站點
            var judgeOperation = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIJudgeOperation");
            //取得代判站點為CPF
            var reassignOperation = judgeOperation.Find(p => p.Remark01 == "CPF");
            if (reassignOperation == null)
            {
                //T00555:查無資料，請至系統資料維護新增類別{0}、項目{1}!
                throw new CimesException(TextMessage.Error.T00555("SAIJudgeOperation", "CPF"));
            }
            //取得工作站資訊
            var operation = OperationInfo.GetOperationByName(reassignOperation.Remark02);
            if (operation == null)
            {
                //T00171, 工作站:{0}不存在!!
                throw new CimesException(TextMessage.Error.T00171(reassignOperation.Remark02));
            }
            //根據指定的流程名稱、流程版本、工作站名稱, 找出第一個符合的流程工作站，新的站點-代判站
            var newOperation = RouteOperationInfo.GetRouteOperationByOperationName(splitInfo.RouteName, splitInfo.RouteVersion, reassignOperation.Remark02);

            //重新取得子批資料
            var splitData = LotInfo.GetLotByLot(splitInfo.Lot);

            //變更至指定工作站
            WIPTransaction.ReassignOperation(splitData, newOperation, reason, "", txnStamp);
            //判定不良會拆批，子批的UDC02會記錄工作的SEQ，UDC03會記錄工作站名稱
            WIPTransaction.ModifyLotSystemAttribute(splitData, "USERDEFINECOL02", lotData.OperationSequence, txnStamp);
            WIPTransaction.ModifyLotSystemAttribute(splitData, "USERDEFINECOL03", lotData.OperationName, txnStamp);
        }

        /// <summary>
        /// 執行拆批、Defect及送至待判站點
        /// </summary>
        /// <param name="lotData">批號資料</param>
        /// <param name="txnStamp"></param>
        private SplitLotInfo DefectNG(LotInfoEx lotData, ComponentInfoEx componentData, ReasonCategoryInfo reason, TransactionStamp txnStamp)
        {
            //批號拆批SplitLotForging
            //取得naming
            var naming = GetNamingRule("SplitLotForging", txnStamp.UserID, 1, lotData);

            //更新命名規則
            if (naming.Second.Count > 0)
            {
                DBCenter.ExecuteSQL(naming.Second);
            }

            //將母批的子單元數量給子批
            var split = SplitLotInfo.CreateSplitLotByLotAndQuantity(lotData.Lot, naming.First[0], new List<ComponentInfo>() { componentData }, reason, reason.Description);
            //母批做結批
            var splitIndicator = WIPTxn.SplitIndicator.Create(null, null, null, TerminateBehavior.NoTerminate);
            //批號拆批
            WIPTxn.Default.SplitLot(lotData, split, splitIndicator, txnStamp);
            //在取一次子批批號
            var splitInfo = LotInfo.GetLotByLot(split.Lot);
            //回壓USERDEFINECOL01=Y，表示有NG不良過
            WIPTransaction.ModifyLotSystemAttribute(splitInfo, "USERDEFINECOL01", "Y", txnStamp);
            //子批做DefectLot

            var defectQty = splitInfo.Quantity;
            var defectIndicator = WIPTransaction.DefectIndicator.Create();
            //var lotDefectData = LotDefectInfo.CreateInfo(txnLotData, defectQty, reason);

            var compDefect = ComponentDefectObject.Create(componentData, componentData.ComponentQuantity, 0, reason, reason.Description);
            WIPTransaction.DefectComponent(splitInfo, new List<ComponentDefectObject>() { compDefect }, defectIndicator, txnStamp);

            return split;
        }

        /// <summary>
        /// 設定工件所在位置
        /// </summary>
        /// <param name="componentData">子單元資料</param>
        /// <param name="location">位置</param>
        /// <param name="txnStamp"></param>
        private void SetLocation(ComponentInfoEx componentData, string location, TransactionStamp txnStamp, string SNFlag = "N")
        {
            string newSN = "1";

            #region 取得最後一筆工件的SN
            var selectComponentDatas = ComponentInfoEx.GetDataByLocation(location);

            if (selectComponentDatas.Count > 0)
            {
                decimal dSN = 0;
                if (decimal.TryParse(selectComponentDatas[selectComponentDatas.Count - 1].SN, out dSN))
                {
                    //取得新的SN編號
                    newSN = (dSN + 1).ToString();
                }
            }
            #endregion

            //註記原本的位置
            var fromLocation = componentData.Location;

            //更新欄位[LOCATION]
            componentData.Location = location;

            //更新欄位[SNFLAG]
            if (SNFlag.IsNullOrTrimEmpty() == false) componentData.SNFlag = SNFlag;

            //更新欄位[SN]
            componentData.SN = newSN;

            componentData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);

            //新增LOG                    
            var insertLogData = InfoCenter.Create<CSTAPICPFSetLocationInfo>();

            insertLogData.FromLocation = fromLocation;
            insertLogData.ToLocation = location;
            insertLogData.ComponsetLot = componentData.ComponentID;
            insertLogData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
        }

        /// <summary>
        /// 重新取得CPF_SN前二碼
        /// </summary>
        /// <returns></returns>
        private string GetNewSNFirstPart()
        {
            string resultSNFirstPart = "";

            //以SID逆排序取得最新的一筆資料
            var latestComponent = ComponentInfoEx.GetDataByLatestSID();

            //確認是否為NULL
            if (latestComponent.CPFSN.IsNullOrTrimEmpty())
            {
                //最新一筆Component資料({0})，CPFSN為空白，無法正常取得CPF_SN!
                throw new Exception(RuleMessage.Error.C10162(latestComponent.ComponentID));
            }

            //確認長度是否為四碼
            if (latestComponent.CPFSN.Length != 4)
            {
                //最新一筆Component資料({0})，CPFN({1})碼長不為四碼，無法正常取得CPF_SN!
                throw new Exception(RuleMessage.Error.C10163(latestComponent.ComponentID, latestComponent.CPFSN));
            }

            //取得前二碼
            var latestSN = latestComponent.CPFSN.Substring(0, 2);
            int number = 0;

            //確認前二碼是否為數字
            if (int.TryParse(latestSN, out number))
            {
                //如果數字為99，表示己經是最後碼數
                if (number == 99)
                {
                    resultSNFirstPart = "01";
                }
                else
                {
                    resultSNFirstPart = string.Format("{0:00}", (number + 1));
                }
            }
            else
            {
                //最新一筆Component資料({0})，CPFN({1})前二碼不為數字，無法正常取得CPF_SN!
                throw new Exception(RuleMessage.Error.C10164(latestComponent.ComponentID, latestComponent.CPFSN));
            }

            return resultSNFirstPart;
        }

        /// <summary>
        /// 取得CPF_SN前二碼
        /// </summary>
        /// <returns></returns>
        private string GetSNFirstPart(ComponentInfoEx latestComponent)
        {
            string resultSNFirstPart = "";

            //確認是否為NULL
            if (latestComponent.CPFSN.IsNullOrTrimEmpty())
            {
                //最新一筆Component資料({0})，CPFSN為空白，無法正常取得CPF_SN!
                throw new Exception(RuleMessage.Error.C10162(latestComponent.ComponentID));
            }

            //確認長度是否為四碼
            if (latestComponent.CPFSN.Length != 4)
            {
                //最新一筆Component資料({0})，CPFN({1})碼長不為四碼，無法正常取得CPF_SN!
                throw new Exception(RuleMessage.Error.C10163(latestComponent.ComponentID, latestComponent.CPFSN));
            }

            //取得前二碼
            var latestSN = latestComponent.CPFSN.Substring(0, 2);
            int number = 0;

            //確認前二碼是否為數字
            if (int.TryParse(latestSN, out number))
            {
                resultSNFirstPart = latestSN;
            }
            else
            {
                //最新一筆Component資料({0})，CPFN({1})前二碼不為數字，無法正常取得CPF_SN!
                throw new Exception(RuleMessage.Error.C10164(latestComponent.ComponentID, latestComponent.CPFSN));
            }

            return resultSNFirstPart;
        }


        /// <summary>
        /// 取得Naming
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="user"></param>
        /// <param name="info"></param>
        /// <param name="lstArgs"></param>
        /// <returns></returns>
        private Pair<string[], List<SqlAgent>> GetNamingRule(string ruleName, string user, int count, InfoBase info = null, List<string> lstArgs = null)
        {
            var naming = NamingIDGenerator.GetRule(ruleName);
            if (naming == null)
            {
                throw new Exception(TextMessage.Error.T00437(ruleName));   //[00437]找不到可產生的序號，請至命名規則維護設定，規則名稱：{0}!!!
            }

            if (lstArgs == null)
            {
                lstArgs = new List<string>();
            }

            if (info != null)
            {
                var result = naming.GenerateNextIDs(count, info, lstArgs.ToArray(), user);
                return new Pair<string[], List<SqlAgent>>(result.First, result.Second);
            }
            else
            {
                var result = naming.GenerateNextIDs(count, lstArgs.ToArray(), user);
                return new Pair<string[], List<SqlAgent>>(result.First, result.Second);
            }
        }

        /// <summary>
        /// 記錄錯誤訊息
        /// </summary>
        /// <param name="stackTrace">程式錯誤訊息</param>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <param name="compLot">工件序號</param>
        /// <param name="line">線別</param>
        /// <param name="opeationName">工作站</param>
        /// <param name="programRight">程式名稱</param>
        /// <param name="lot">批號</param>
        /// <param name="equipmentName">機台編號</param>
        /// <returns></returns>
        private string AddErrorMessageToDB(string stackTrace, string errorMessage, string lot, string compLot, string line, string opeationName, string programRight, string equipmentName = "")
        {
            string returnString = "";

            //如果錯誤訊息為NULL，則直接回傳空白
            if (errorMessage.IsNullOrTrimEmpty())
                return returnString;

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, programRight, programRight, _ApplicationName);

                using (var cts = CimesTransactionScope.Create())
                {
                    var insertData = InfoCenter.Create<CSTAPIErrorLogInfo>();

                    //工件序號
                    insertData.CompLot = compLot;

                    //程式名稱
                    insertData.FunctionName = programRight;

                    //線別
                    insertData.Line = line;

                    //錯誤訊息
                    insertData.Message = errorMessage;

                    //工作站
                    insertData.Operation = opeationName;

                    //機台編號
                    insertData.EquipmentName = equipmentName;

                    //批號
                    insertData.Lot = lot;

                    //程式錯誤訊息
                    insertData.Description = stackTrace;

                    insertData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    cts.Complete();
                }
            }
            catch (Exception excp)
            {
                returnString = " [ " + excp.Message + " ]";
            }

            return returnString;
        }

        /// <summary>
        /// 確認C10邏輯
        /// </summary>
        /// <param name="SN">取工件編號前二碼</param>
        /// <param name="findInterval1">有找到範圍在01-15之間的工件編號</param>
        /// <param name="findInterval2">有找到範圍在16-30之間的工件編號</param>
        private void CheckC10Rule(string SN, ref bool findInterval1, ref bool findInterval2)
        {
            string filterLocation = "C10";

            List<string> Locations = new List<string>();

            //取得鍛造工作站位置設定資料
            var tmpLocations = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIForgingLocation");

            #region 以Remark01作排序並且取出filterLocation之前的Lcation資料
            var forgingLocations = tmpLocations.OrderBy(p => p.Remark01).ToList();
            foreach (var location in forgingLocations)
            {
                if (location.Remark02 == filterLocation)
                {
                    break;
                }
                else
                {
                    Locations.Add(location.Remark02);
                }
            }
            #endregion

            //取得工件編號前二碼相符的資料清單
            var components = ComponentInfoEx.GetDatasByLikeCPF_SN(SN);
            if (components.Count == 0)
            {
                //報錯??
            }

            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];

                //確認工件編號的Location是否還在C10前面的位置
                var findIndex = Locations.FindIndex(location => location == component.Location);
                if (findIndex != -1)
                {
                    //取得CPFSN後二碼
                    var findCPFSN = Convert.ToInt32(component.CPFSN.Substring(2, 2));

                    //確認找到的工件編號CPSN範圍是否在01-15或16-30之間
                    if (findCPFSN >= 1 && findCPFSN <= 15)
                    {
                        findInterval1 = true;
                    }
                    else if (findCPFSN >= 16 && findCPFSN <= 30)
                    {
                        findInterval2 = true;
                    }
                }
            }
        }

        /// <summary>
        /// 取得系統控制設定的類別資料
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private List<WpcExClassItemInfo> GetExtendItemListByClass(string className)
        {
            var dataList = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks(className);

            if (dataList.Count == 0)
            {
                // [00466]系統控制設定，類別：[{0}]，項目：[{1}]未設定！
                throw new Exception(TextMessage.Error.T00466(className, ""));
            }

            return dataList;
        }

        /// <summary>
        /// 將傳入的資料進行比對，查詢是否符合機台對應表中的資料
        /// </summary>
        /// <param name="line">線別</param>
        private string EquipmantMapping(string line)
        {
            string equipmentName = "";
            string className = "AutoCPF_EQP_Mapping";

            //取得系統資料設定的自動化站點對應表
            var dataList = GetExtendItemListByClass(className);

            //比對符合的識別名稱
            foreach (var data in dataList)
            {
                if (data.Remark03 == line)
                {
                    //取得機台名稱
                    equipmentName = data.Remark02;
                    break;
                }
            }

            //如果機台名稱找不到，則拋出錯誤訊息
            if (equipmentName.IsNullOrTrimEmpty())
            {
                // [00466]系統控制設定，類別：[{0}]，項目：[{1}]未設定！
                throw new Exception(TextMessage.Error.T00466(className, ""));
            }

            return equipmentName;
        }

        #region Model
        public class ResultData
        {
            /// <summary>
            /// 回傳處理結果
            /// </summary>
            public bool Result { get; set; }

            /// <summary>
            /// 錯誤訊息
            /// </summary>
            public string Message { get; set; }

            public ResultData(bool result = true, string message = "")
            {
                Result = result;
                Message = message;
            }
        }
        #endregion
    }
}