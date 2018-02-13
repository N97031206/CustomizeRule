using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Transaction;
using Ares.Cimes.IntelliService.Models;
using CustomizeRule.CustomizeInfo.ExtendInfo;
using ciMes.Web.Common;

namespace CustomizeRule.RuleUtility
{
    public class CustomizeFunction
    {
        #region 刀具身份
        public enum ToolIdentity
        {
            /// <summary>
            /// 新品
            /// </summary>
            新品,
            /// <summary>
            /// 堪用
            /// </summary>
            堪用,
            /// <summary>
            /// 維修
            /// </summary>
            維修
        }
        #endregion

        #region QC單類別
        public enum QCType
        {
            /// <summary>
            /// 首件
            /// </summary>
            FAI,
            /// <summary>
            /// 穩定度
            /// </summary>
            PPK,
            /// <summary>
            /// 三次元巡檢
            /// </summary>
            PQC,
            /// <summary>
            /// 自主檢
            /// </summary>
            MPQC
        }
        #endregion

        #region QC單狀態
        public enum QCStatus
        {
            /// <summary>
            /// 等待送首件
            /// </summary>
            ToBeSentFAI,
            /// <summary>
            /// 等待首件接收
            /// </summary>
            WaitReceiveFAI,
            /// <summary>
            /// 等待首件檢驗
            /// </summary>
            WaitFAI,
            /// <summary>
            /// 首件檢驗中
            /// </summary>
            FAI,
            /// <summary>
            /// 檢驗完成
            /// </summary>
            Finished,
            /// <summary>
            /// 等待送PPK
            /// </summary>
            ToBeSentPPK,
            /// <summary>
            /// 等待PPK接收
            /// </summary>
            WaitReceivePPK,
            /// <summary>
            /// 等待PPK檢驗
            /// </summary>
            WaitPPK,
            /// <summary>
            /// PPK檢驗中
            /// </summary>
            PPK,
            /// <summary>
            /// 等待送巡檢
            /// </summary>
            ToBeSentPQC,
            /// <summary>
            /// 等待巡檢接收
            /// </summary>
            WaitReceivePQC,
            /// <summary>
            /// 等待PQC檢驗
            /// </summary>
            WaitPQC,
            /// <summary>
            /// PQC檢驗中
            /// </summary>
            PQC,
            /// <summary>
            /// 等待送自主檢
            /// </summary>
            ToBeSentMPQC,
            /// <summary>
            /// 等待自主檢接收
            /// </summary>
            WaitReceiveMPQC,
            /// <summary>
            /// 等待自主檢驗
            /// </summary>
            WaitMPQC,
            /// <summary>
            /// 自主檢驗中
            /// </summary>
            MPQC
        }
        #endregion

        #region 首件測試次數
        public enum FAIType
        {
            /// <summary>
            /// 連N
            /// </summary>
            CN,
            /// <summary>
            /// 首N
            /// </summary>
            FN
        }
        #endregion

        #region 料號屬性設定ProdType
        public enum ProdType
        {
            /// <summary>
            /// 有刻字，有序號
            /// </summary>
            S,
            /// <summary>
            /// 有刻字，無序號
            /// </summary>
            G,
            /// <summary>
            /// 有刻字，無意義，BMW
            /// </summary>
            B,
            /// <summary>
            /// 無刻字，賓利
            /// </summary>
            W
        }
        #endregion

        /// <summary>
        /// 檢查首件是否檢驗完成，檢驗完成回傳 True，未完成回傳 False
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="lot"></param>
        /// <returns></returns>
        public static bool CheckFAI(string equipmentName, string lot)
        {
            /*
             * 在CST_EQP_FAI裡沒有資料代表沒有送驗，則視為FAI未完成
             * 在CST_EQP_FAI有資料，但是waitCount不等於0，代表FAI未完成
             * 在CST_EQP_FAI有資料，且waitCount = 0，代表FAI完成
             */

            //取得批號資料
            var lotData = LotInfo.GetLotByLot(lot);

            var faiData = CSTEquipmentFAIInfo.GetDataByEquipmentAndDevice(equipmentName, lotData.DeviceName);

            if (faiData == null)
                //找不到首件檢驗的資料，請確認是否已執行調機作業!!!
                throw new CimesException(RuleMessage.Error.C00026());

            if (faiData != null && faiData.WAITFAICOUNT <= 0)
                return true;

            return false;
        }

        /// <summary>
        /// 更新FAI，檢驗完成回壓FAICount，第一次首件回壓FAIDevice
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="lot"></param>
        public static void UpdateFAI(string equipmentName, string lot, bool initFlag, TransactionStamp txs)
        {
            //取得批號資料
            var lotData = LotInfo.GetLotByLot(lot);

            var faiData = CSTEquipmentFAIInfo.GetDataByEquipmentAndDevice(equipmentName, lotData.DeviceName);

            if (faiData == null)
                return;

            if (faiData.INSPTYPE == FAIType.CN.ToCimesString())
            {
                #region 連N測試，只要一支NG，就要reset檢驗次數，重新送件

                //判定NG的時候回壓初始化FAICount，判定OK則WAITFAICOUNT減1
                if (initFlag) //NG
                {
                    //取得料號版本的資料
                    var deviceVerExData = DeviceVersionInfo.GetDeviceVersion(lotData.DeviceName, lotData.DeviceVersion).ChangeTo<DeviceVersionInfoEx>();

                    //檢查料號的FAICount有無設定
                    if (deviceVerExData.FAICount.ToString().IsNullOrTrimEmpty())
                    {
                        //T00541
                        //料號：xxx的系統屬性：xxx 未設定，請洽IT人員！) error code：T00031
                        throw new Exception(TextMessage.Error.T00541(deviceVerExData.DeviceName, "FAICount"));
                    }

                    faiData.WAITFAICOUNT = deviceVerExData.FAICount;
                }
                else //OK
                {
                    faiData.WAITFAICOUNT = faiData.WAITFAICOUNT - 1;
                }
                #endregion
            }
            else
            {
                //首N測試，判定OK將WAITFAICOUNT減1就好，判定NG不須動作
                if (!initFlag)
                    faiData.WAITFAICOUNT = faiData.WAITFAICOUNT - 1;
            }

            faiData.UpdateToDB(txs.UserID, txs.RecordTime);
            LogCenter.LogToDB(faiData, LogCenter.LogIndicator.Create(ActionType.Set, txs.UserID, txs.RecordTime));
        }


        public static void SaveQCData(string InspectionNo, QCType qcType, LotInfoEx lot, EquipmentInfo equipment, string batchID, QCStatus status, string objectType, TransactionStamp txnStamp)
        {
            QCInspectionInfoEx inspection = null;

            #region 產生檢驗單
            inspection = InfoCenter.Create<QCInspectionInfo>().ChangeTo<QCInspectionInfoEx>();
            inspection.InspectionNo = InspectionNo;
            inspection.QCTYPE = qcType.ToCimesString();
            inspection.OperationName = lot.OperationName;
            inspection.EquipmentName = equipment.EquipmentName;
            inspection.DeviceName = lot.DeviceName;
            inspection.BatchID = batchID;
            inspection.Status = status.ToCimesString();
            inspection.CreateUser = txnStamp.UserID;
            inspection.CreateTime = txnStamp.RecordTime;
            inspection.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
            #endregion

            if (inspection == null)
            {
                //檢驗單主檔沒有資料
                throw new Exception(TextMessage.Error.T00060("QCData"));
            }
            #region 產生檢驗單對象
            //新增一筆資料到MES_QC_INSP_OBJ
            var inspectionObject = InfoCenter.Create<QCInspectionObjectInfo>();
            if (inspectionObject.InfoState == InfoState.NewCreate)
            {
                inspectionObject.QC_INSP_SID = inspection.QC_INSP_SID;
                inspectionObject.ObjectType = objectType;
                inspectionObject.OBJECTSID = lot.ID;
                inspectionObject.ObjectName = lot.Lot;
                //COMPLOT
                inspectionObject.ItemName1 = lot.ComponentLot;
                //WOLOT
                inspectionObject.ItemName2 = lot.WorkOrderLot;
                //MATERIALLOT
                inspectionObject.ItemName3 = lot.MaterialLot;
                //OP1機台
                inspectionObject.ItemName5 = lot.ProcessEquipment;
                inspectionObject.Quantity = 1;
                inspectionObject.Unit = lot.Unit;
                inspectionObject.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
            }
            #endregion
        }

        public static void SaveQCData(string InspectionNo, QCType qcType, LotNonActiveInfo lot, EquipmentInfo equipment, string batchID, QCStatus status, string objectType, TransactionStamp txnStamp)
        {
            QCInspectionInfoEx inspection = null;

            var lotNonActiveInfoEx = lot.ChangeTo<LotNonActiveInfoEx>();

            #region 產生檢驗單
            inspection = InfoCenter.Create<QCInspectionInfo>().ChangeTo<QCInspectionInfoEx>();
            inspection.InspectionNo = InspectionNo;
            inspection.QCTYPE = qcType.ToCimesString();
            inspection.OperationName = lotNonActiveInfoEx.OperationName;
            inspection.EquipmentName = equipment.EquipmentName;
            inspection.DeviceName = lotNonActiveInfoEx.DeviceName;
            inspection.BatchID = batchID;
            inspection.Status = status.ToCimesString();
            inspection.CreateUser = txnStamp.UserID;
            inspection.CreateTime = txnStamp.RecordTime;
            inspection.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
            #endregion

            if (inspection == null)
            {
                //檢驗單主檔沒有資料
                throw new Exception(TextMessage.Error.T00060("QCData"));
            }
            #region 產生檢驗單對象
            //新增一筆資料到MES_QC_INSP_OBJ
            var inspectionObject = InfoCenter.Create<QCInspectionObjectInfo>();
            if (inspectionObject.InfoState == InfoState.NewCreate)
            {
                inspectionObject.QC_INSP_SID = inspection.QC_INSP_SID;
                inspectionObject.ObjectType = objectType;
                inspectionObject.OBJECTSID = lotNonActiveInfoEx.ID;
                inspectionObject.ObjectName = lotNonActiveInfoEx.Lot;
                //COMPLOT
                inspectionObject.ItemName1 = lotNonActiveInfoEx.ComponentLot;
                //WOLOT
                inspectionObject.ItemName2 = lotNonActiveInfoEx.WorkOrderLot;
                //MATERIALLOT
                inspectionObject.ItemName3 = lotNonActiveInfoEx.MaterialLot;
                //OP1機台
                inspectionObject.ItemName5 = lotNonActiveInfoEx.ProcessEquipment;
                inspectionObject.Quantity = 1;
                inspectionObject.Unit = lotNonActiveInfoEx.Unit;
                inspectionObject.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
            }
            #endregion
        }

        /// <summary>
        /// 檢查PPK是否完成，true: 表示完成、false: 表示未完成
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceVersion"></param>
        /// <returns></returns>
        public static void UpdatePPK(string equipmentName, string deviceName, decimal deviceVersion, string resultFlag = null, string updateStatus = null)
        {
            //取得PPK資料
            var ppkData = CSTWipPPKInfo.GetPPKDataByEqpAndDevice(equipmentName, deviceName);
            var deviceVerExData = DeviceVersionInfo.GetDeviceVersion(deviceName, deviceVersion).ChangeTo<DeviceVersionInfoEx>();
            if (ppkData == null)
            {
                #region 新增PPK資料
                var ppkItem = InfoCenter.Create<CSTWipPPKInfo>();
                ppkItem.EquipmentName = equipmentName;
                ppkItem.DeviceName = deviceName;
                ppkItem.Status = "NonActive";
                ppkItem.PPKCOUNT = deviceVerExData.PPKCount - 1;
                ppkItem.InsertToDB();
                #endregion
            }
            else
            {
                //PPKCount不等於0，Count - 1
                if (ppkData.PPKCOUNT != 0)
                {
                    ppkData.PPKCOUNT = ppkData.PPKCOUNT - 1;
                }
                else
                {
                    if (resultFlag != null)
                    {
                        //檢驗結果
                        if (resultFlag.ToBool())
                        {
                            ppkData.Status = "Active";
                        }
                        else
                        {
                            //回壓PPKConut為初始化
                            ppkData.PPKCOUNT = deviceVerExData.PPKCount;
                        }
                    }
                }

                ppkData.UpdateToDB();
            }
        }

        public static bool CheckPPK(string equipmentName, string deviceName, decimal deviceVersion)
        {
            //取得PPK資料
            var ppkData = CSTWipPPKInfo.GetPPKDataByEqpAndDevice(equipmentName, deviceName);
            if (ppkData != null && ppkData.PPKCOUNT == 0 && ppkData.Status == "Active")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 檢查是否為最後一筆ppk資料
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="deviceName"></param>
        /// <param name="deviceVersion"></param>
        /// <returns></returns>
        public static bool CheckFinalPPK(string equipmentName, string deviceName, decimal deviceVersion)
        {
            //取得PPK資料
            var ppkData = CSTWipPPKInfo.GetPPKDataByEqpAndDevice(equipmentName, deviceName);
            if (ppkData != null && ppkData.PPKCOUNT == 0 && ppkData.Status == "NonActive")
            {
                return true;
            }
            return false;
        }

        public static void AutoMerge(string invLot, TransactionStamp txs, ReasonCategoryInfo reasonCategory)
        {
            //找到相同的INVLot的序號
            var lots = LotInfoEx.GetLotListByInvertoryLot(invLot);
            //排除送過NG的序號
            var autoMergeLots = lots.FindAll(p => p.UserDefineColumn01.IsNullOrTrimEmpty());

            //利用小工單號確認是否還有QC未檢驗的資料(包含FAI、PQC、PPK)
            //只要有一張未檢，則代表檢驗未完成，回傳false，反之
            var qcFlag = CheckQCInspectionDone(lots[0].WorkOrderLot);

            //檢查數量是否可以做AUTOMERGE
            var waitMergeLots = autoMergeLots.FindAll(p => p.Status == "WaitMerge");
            if (waitMergeLots.Count == autoMergeLots.Count && qcFlag)
            {
                var customizeFunction = new CustomizeFunction();
                //取得命名規則
                //var naming = customizeFunction.GetNamingRule("SplitLot", txs.UserID, autoMergeLots[0], null);
                //拆批
                var split = SplitLotInfo.CreateSplitLotByLotAndQuantity(autoMergeLots[0].Lot, autoMergeLots[0].WorkOrderLot, 0, 0, txs.CategoryReasonCode, txs.Description);
                //母批不做結批
                var splitIndicator = WIPTxn.SplitIndicator.Create(null, null, null, TerminateBehavior.NoTerminate);
                WIPTxn.Default.SplitLot(autoMergeLots[0], split, splitIndicator, txs);
                //更新命名規則
                //if (naming.Second.Count > 0)
                //{
                //    DBCenter.ExecuteSQL(naming.Second);
                //}
                //再取一次批號資訊
                var newLot = LotInfo.GetLotByLot(split.Lot);
                //併批與子單元
                List<MergeLotInfo> mergeLotList = new List<MergeLotInfo>();
                waitMergeLots.ForEach(mergelot =>
                {
                    var compData = ComponentInfo.GetComponentByComponentID(mergelot.ComponentLot);
                    var mergeLot = MergeLotInfo.GetMergeLotByLotAndQuantity(mergelot.Lot, new List<ComponentInfo>() { compData }, reasonCategory, txs.Description);
                    mergeLotList.Add(mergeLot);
                });
                WIPTransaction.MergeLot(newLot, mergeLotList, txs);
                //再取一次批號資訊
                var newMergeLot = LotInfo.GetLotByLot(split.Lot);

                //將批號狀態變更為wait
                WIPTransaction.ModifyLotSystemAttribute(newMergeLot, "STATUS", LotDefaultStatus.Wait.ToCimesString(), txs);
                //將COMPLOT、PROCESS_EQUIP欄位清空，因為這個時間點這個欄位已經沒意義了
                WIPTransaction.ModifyLotSystemAttribute(newMergeLot, "COMPLOT", string.Empty, txs);
                WIPTransaction.ModifyLotSystemAttribute(newMergeLot, "PROCESS_EQUIP", string.Empty, txs);

                //Dispatch到下一站
                WIPTransaction.DispatchLot(newMergeLot, txs);
            }
        }

        /// <summary>
        /// 取得預約工作站的序號
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        public static string GetAppointmentOperationSequence(string lot, string ruleName)
        {
            var lotData = LotInfo.GetLotByLot(lot).ChangeTo<LotInfoEx>();
            string nextOperationSequence = "";

            //依據傳入的批號去取得預約表資訊
            var WIPReserveCheckInDataList = CSTWIPReserveCheckInInfo.GetDataByLot(lotData.Lot);

            //用批號去客製表找出所有預約紀錄，先比較currentRule是否存在於客製表，若有，則拋錯
            if (WIPReserveCheckInDataList.Count > 0)
            {
                //從預約表中找出相同的RuleName
                List<CSTWIPReserveCheckInInfo> matchReserveList = WIPReserveCheckInDataList.FindAll(x => x.RuleName == ruleName);

                if (matchReserveList.Count > 0)
                {
                    //批號：{0}已進行站點：{1}預約進站作業，請遵循作業規範	
                    throw new Exception(RuleMessage.Error.C10001(lotData.Lot, lotData.OperationName));
                }

                //取得最後一次預約的工作站點名稱
                var lastAppointmentOperation = WIPReserveCheckInDataList[0].OperationName;

                //取得目前批號的流程線上版本
                RouteVersionInfo RouteVersion = RouteVersionInfo.GetRouteActiveVersion(lotData.RouteName);

                //以目前工作站名稱去查詢在所有流程中的序號
                var routeOperation = RouteOperationInfo.GetRouteAllOperations(RouteVersion).Find(p => p.OperationName == lastAppointmentOperation);

                //取得下一個工作站序號
                nextOperationSequence = string.Format("{0:000}", (Convert.ToDecimal(routeOperation.OperationSequence) + 1));
            }

            return nextOperationSequence;
        }

        /// <summary>
        /// 取得預約工作站名稱
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="nextOperationSequence"></param>
        /// <returns></returns>
        public static string GetAppointmentOperationName(LotInfo lot, string nextOperationSequence = "")
        {
            var nextOperation = new RouteOperationInfo();

            //找出下一個工作站(預約站點)
            if (nextOperationSequence.IsNullOrTrimEmpty())
            {
                nextOperation = RouteOperationInfo.GetLotNextDefaultOperation(lot);
            }
            else
            {
                nextOperation = RouteOperationInfo.GetRouteOperationByLotSequence(lot, nextOperationSequence);
            }

            if (nextOperation == null)
            {
                //批號：{0}已無下個工作站點，請確認[流程設定]
                throw new Exception(RuleMessage.Error.C10008(lot.Lot));
            }

            return nextOperation.OperationName;
        }

        /// <summary>
        /// 取得預約工作站的第一個RuleName
        /// </summary>
        /// <param name="operationName"></param>
        /// <returns></returns>
        public static string GetFirstOperationTypeRuleByOperationName(LotInfo lot, string operationName, string ruleName)
        {
            //取得預約站點的第一個Rule
            var frstRuleType = OperationTypeRuleInfo.GetFirstOperationTypeRuleByOperationName(operationName);
            if (frstRuleType == null)
            {
                //站點：{0}，找不到設定規則，請確認[工作站設定]
                throw new Exception(RuleMessage.Error.C10002(operationName));
            }

            //檢查目前使用程式的RuleName是否與預約站點的第一個Rule相同，如果不同，則拋出錯誤訊息
            if (frstRuleType.RuleName != ruleName)
            {
                //該批號作業為{0}，不為此功能，請遵循作業規範
                throw new Exception(RuleMessage.Error.C10004(lot.CurrentRuleName));
            }

            //確認預約工作站是否有開啟預約功能
            var operationData = OperationInfo.GetOperationByName(operationName).ChangeTo<OperationInfoEx>();
            if (operationData.ReserveFlag == "N")
            {
                //工作站:{0} 系統屬性設定沒有開啟預約功能!
                throw new Exception(RuleMessage.Error.C10068(operationData.OperationName));
            }

            return frstRuleType.RuleName;
        }

        /// <summary>
        /// 檢查傳入機台是否符合工單上線別內可使用機台名稱
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="lotData"></param>
        /// <returns></returns>
        public static bool CheckEqpGruopAndDivision(string equipmentName, LotInfo lot)
        {
            bool checkResult = false;

            //找出工單上的線別(division)，再用線別名稱去工作站的機台設定，找出同名稱的機台群組，再比照機台的正確性

            //取得批號目前的工作站資料
            var operationData = OperationInfo.GetOperationByName(lot.OperationName);
            if (operationData == null)
            {
                //機台：{0}，不在可用機台範圍，請至 [工作站設定] 確認機台設定
                throw new Exception(RuleMessage.Error.C10005(equipmentName));
            }

            //依據批號工作站找出歸屬的機台群組清單
            List<OperationResourceInfoEx> resourceDataList = OperationResourceInfoEx.GetDataByOperSID(operationData.OperationSID);

            if (resourceDataList.Count == 0)
            {
                //機台：{0}，不在可用機台範圍，請至 [工作站設定] 確認機台設定
                throw new Exception(RuleMessage.Error.C10005(equipmentName));
            }

            foreach (var resourceData in resourceDataList)
            {
                //找出同名稱的機台群組
                var equipGroupData = EquipmentGroupInfo.GetEquipmentGroupByID(resourceData.ResourceID);

                //找出工單上的線別(Division)
                var workOrder = WorkOrderInfo.GetWorkOrderByWorkOrder(lot.WorkOrder).ChangeTo<WorkOrderInfoEx>();

                if (equipGroupData.GroupName == workOrder.Division)
                {
                    //用群組SID找出在該群組下的工作站
                    List<EquipGroupEquipInfoEx> eqGroupDataList = EquipGroupEquipInfoEx.GetEquipGroupByGroupSID(equipGroupData.ID);
                    if (eqGroupDataList.Find(p => p.EquipmentName == equipmentName) != null)
                    {
                        checkResult = true;
                    }
                }
            }

            return checkResult;
        }


        /// <summary>
        /// 取得Naming
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="user"></param>
        /// <param name="info"></param>
        /// <param name="lstArgs"></param>
        /// <returns></returns>
        public Pair<string[], List<SqlAgent>> GetNamingRule(string ruleName, string user, InfoBase info = null, List<string> lstArgs = null)
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
                var result = naming.GenerateNextIDs(1, info, lstArgs.ToArray(), user);
                return new Pair<string[], List<SqlAgent>>(result.First, result.Second);
            }
            else
            {
                var result = naming.GenerateNextIDs(1, lstArgs.ToArray(), user);
                return new Pair<string[], List<SqlAgent>>(result.First, result.Second);
            }
        }

        /// <summary>
        /// 檢查BOM表
        /// </summary>
        public static void CheckBOM()
        {
            //TODO: 檢查BOM表，目前SAP尚未提供資料

        }

        /// <summary>
        /// 依據傳入的工作站取得可使用機台清單
        /// </summary>
        /// <param name="operationName">工作站名稱</param>
        public static List<EquipmentInfo> GetEquipmentListByOperationName(string operationName)
        {
            //清除機台清單
            List<EquipmentInfo> equipmentList = new List<EquipmentInfo>();

            //取得批號目前的工作站資料
            var operationData = OperationInfo.GetOperationByName(operationName);
            if (operationData == null)
            {
                //[00171]工作站:{0}不存在!!
                throw new Exception(TextMessage.Error.T00171(operationName));
            }

            //確認工作站是否有啟用使用機台的功能
            if (operationData.EQUIPFLAG == "Y")
            {
                //依據工作站找出歸屬的機台群組清單
                List<OperationResourceInfoEx> resourceDataList = OperationResourceInfoEx.GetDataByOperSID(operationData.OperationSID);

                if (resourceDataList.Count > 0)
                {
                    #region 取得該工作站可使用的所有機台資料
                    foreach (var resourceData in resourceDataList)
                    {
                        if (resourceData.ResourceType == "Group")
                        {
                            //如果資源類別為Group，則找出此機台群組內的所有機台資訊
                            var equipGroupData = EquipmentGroupInfo.GetEquipmentGroupByID(resourceData.ResourceID);

                            //用群組SID找出在該群組下的工作站
                            List<EquipGroupEquipInfoEx> eqGroupDataList = EquipGroupEquipInfoEx.GetEquipGroupByGroupSID(equipGroupData.ID);

                            if (eqGroupDataList.Count > 0)
                            {
                                foreach (var eqGroupData in eqGroupDataList)
                                {
                                    //取得機台資訊
                                    var equipmentData = EquipmentInfo.GetEquipmentByName(eqGroupData.EquipmentName);

                                    //如果機台資料不為NULL才要確認是否己經加入清單內
                                    if (equipmentData != null)
                                    {
                                        //確認是否已經加入機台清單
                                        if (equipmentList.Contains(equipmentData) == false)
                                        {
                                            equipmentList.Add(equipmentData);
                                        }
                                    }
                                }
                            }
                        }
                        else if (resourceData.ResourceType == "Equip")
                        {
                            //如果資源類別為Equip，則加入此機台資訊
                            var equipmentData = EquipmentInfo.GetEquipmentByID(resourceData.ResourceID);

                            //如果機台資料不為NULL才要確認是否己經加入清單內
                            if (equipmentData != null)
                            {
                                //確認是否已經加入機台清單
                                if (equipmentList.Contains(equipmentData) == false)
                                {
                                    equipmentList.Add(equipmentData);
                                }
                            }
                        }
                        else if (resourceData.ResourceType == "Type")
                        {
                            //如果資源類別為Type，則加入所有此Type的機台資訊
                            var equipmentTypeData = EquipmentTypeInfo.GetEquipmentTypeByID(resourceData.ResourceID);
                            var equipmentDatalList = EquipmentInfo.GetEquipmentsByType(equipmentTypeData);

                            foreach (var equipmentData in equipmentDatalList)
                            {
                                //確認是否已經加入機台清單
                                if (equipmentList.Contains(equipmentData) == false)
                                {
                                    equipmentList.Add(equipmentData);
                                }
                            }
                        }
                    }
                    #endregion  
                }
            }

            //以機台名稱排序
            equipmentList.Sort(p => p.EquipmentName);

            return equipmentList;
        }

        public static string ConvertDMCCode(string dmcCode)
        {
            if (!dmcCode.Contains("."))
            {
                return dmcCode;
            }

            var lstDMC = dmcCode.Split('.');
            return lstDMC[lstDMC.Length - 1];
        }

        /// <summary>
        /// First:SHIFTDATE Second:SHIFT
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="systime"></param>
        /// <returns></returns>                    
        public static Pair<string, string> GetUserShifeData(string systime, bool workon)
        {
            var nowTime = DateTime.Parse(systime);
            var today = nowTime.ToString("yyyy/MM/dd");
            var lstShift = WpcExClassItemInfo.GetInfoByClass("Shift");
            var firstShif = lstShift.Find(p => p.Remark05 == "1");
            var todayStart = DateTime.Parse(today + " " + "00:00:00");
            var firstSecond = DateTime.Parse(today + " " + firstShif.Remark02).Subtract(todayStart);
           
            string nowShift = "", nowShiftDate = "";
            nowShiftDate = nowTime.Subtract(firstSecond).ToString("yyyy/MM/dd");

            // 上工才找最早允許上工時間
            if (workon)
            {
                lstShift.ForEach(p =>
                {
                    // 優先找最早允許上工時間到開始時間
                    var earliestStartTime = DateTime.Parse(today + " " + p["REMARK04"].ToString());
                    var shiftStartTime = DateTime.Parse(today + " " + p["REMARK02"].ToString());
                    var shiftEndTime = DateTime.Parse(today + " " + p["REMARK03"].ToString());

                    if (shiftEndTime < shiftStartTime)
                    {
                        if (nowTime.Subtract(firstSecond).Day != nowTime.Day)
                        {
                            earliestStartTime = DateTime.Parse(nowTime.AddDays(-1).ToString("yyyy/MM/dd") + " " + p["REMARK04"].ToString());
                        }
                        else
                        {
                            shiftEndTime = DateTime.Parse(nowTime.AddDays(1).ToString("yyyy/MM/dd") + " " + p["REMARK03"].ToString());
                        }
                    }
                    var earSecond = shiftStartTime.Subtract(earliestStartTime);
                    if (nowTime >= earliestStartTime && nowTime.Add(earSecond) < shiftEndTime)
                    {
                        nowShift = p["REMARK01"].ToString();
                    }
                });
            }

            if (nowShift.IsNullOrEmpty())
            {
                lstShift.ForEach(p =>
                {
                    var shiftStartTime = DateTime.Parse(today + " " + p["REMARK02"].ToString());
                    var shiftEndTime = DateTime.Parse(today + " " + p["REMARK03"].ToString());

                    if (shiftEndTime < shiftStartTime)
                    {
                        if (nowTime.Subtract(firstSecond).Day != nowTime.Day)
                        {
                            shiftStartTime = DateTime.Parse(nowTime.AddDays(-1).ToString("yyyy/MM/dd") + " " + p["REMARK02"].ToString());
                        }
                        else
                        {
                            shiftEndTime = DateTime.Parse(nowTime.AddDays(1).ToString("yyyy/MM/dd") + " " + p["REMARK03"].ToString());
                        }
                    }

                    if (nowTime >= shiftStartTime && nowTime <= shiftEndTime)
                    {
                        nowShift = p["REMARK01"].ToString();
                    }
                });
            }

            if (nowShift == "")
            {
                throw new Exception(RuleMessage.Error.C10155());
            }

            //var dt = DBCenter.GetDataTable("SELECT * FROM BPM_MES_EMPSHIFT WHERE USERID =#[STRING] AND SHIFTDATE = #[STRING]", userID, nowShiftDate);
            //if (dt.Rows.Count  == 0)
            //{
            //    throw new Exception(RuleMessage.Error.C10139());
            //}

            //string EMPShiftDate = dt.Rows[0]["SHIFTDATE"].ToString();
            //string EMPShift = dt.Rows[0]["SHIFT"].ToString();
            //if (EMPShiftDate.IsNullOrEmpty())
            //{
            //    throw new Exception(RuleMessage.Error.T00043("SHIFTDATE"));
            //}

            //if (EMPShift.IsNullOrEmpty())
            //{
            //    throw new Exception(RuleMessage.Error.T00043("SHIFT"));
            //}

            //if (nowShift == "早" && EMPShiftDate == "晚")
            //{
            //    throw new Exception(RuleMessage.Error.C10156(nowShift, EMPShiftDate));
            //}

            //if (nowShift == "中" && EMPShiftDate == "早")
            //{
            //    throw new Exception(RuleMessage.Error.C10156(nowShift, EMPShiftDate));
            //}

            //if (nowShift == "晚" && EMPShiftDate == "中")
            //{
            //    throw new Exception(RuleMessage.Error.C10156(nowShift, EMPShiftDate));
            //}

            return new Pair<string, string>(nowShiftDate, nowShift);
        }

        /// <summary>
        /// 要merge的小工單，先確認該工單所有的檢驗項目是否都完成了
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <param name="lot"></param>
        /// <returns></returns>
        public static bool CheckQCInspectionDone(string woLot)
        {
            //利用小工單號確認是否還有QC未檢驗的資料(包含FAI、PQC、PPK)
            //只要有一張未檢，則代表檢驗未完成，回傳false，反之
            var qcLitst = QCInspectionInfoEx.GetInspDataListByWOLot(woLot);

            if (qcLitst.Count != 0)
                return false;

            return true;
        }

        /// <summary>
        /// 依照檢驗計畫取得該物件的datatable
        /// </summary>
        /// <param name="QCTypeData"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static DataTable GetInspectionData(QCTypeInfo QCTypeData, string objectName)
        {
            string sql = "";
            DataTable dtQCTarget = null;

            #region 依照檢驗計畫取得該物件的datatable
            switch (QCTypeData.QCTarget)
            {
                case "MES_WIP_LOT":
                    {
                        LotInfo lot = LotInfo.GetLotByLot(objectName);
                        if (lot == null)
                        {
                            throw new Exception(TextMessage.Error.T00378(objectName));
                        }

                        dtQCTarget = lot.CopyDataToTable(lot.ID);

                    }
                    break;
                case "MES_WIP_COMP":
                    {
                        //取得component資訊以及所在的工作站
                        sql = @"SELECT L.OPERATION,C.* FROM MES_WIP_COMP C
                                 INNER JOIN MES_WIP_LOT L ON C.CURRENTLOT = L.LOT
                                 WHERE COMPONENTID = #[STRING]";

                        ComponentInfo comp = InfoCenter.GetBySQL<ComponentInfo>(sql, objectName);
                        if (comp == null)
                        {
                            throw new Exception(TextMessage.Error.T00154(objectName));
                        }

                        dtQCTarget = comp.CopyDataToTable(comp.ID);
                    }
                    break;
                case "MES_CMS_CAR":
                    {
                        CarrierInfo carrier = CarrierInfo.GetCarrierByCarrierNo(objectName);
                        if (carrier == null)
                        {
                            throw new RuleCimesException(TextMessage.Error.T00725(objectName));
                        }

                        dtQCTarget = carrier.CopyDataToTable(carrier.ID);

                    }
                    break;
                case "MES_TOOL_MAST":
                    {
                        ToolInfo tool = ToolInfo.GetToolByName(objectName);
                        if (tool == null)
                        {
                            throw new Exception(TextMessage.Error.T00592(objectName));
                        }
                        dtQCTarget = tool.CopyDataToTable(tool.ID);

                    }
                    break;
                case "MES_MMS_MLOT":
                    {
                        MaterialLotInfo mlot = MaterialLotInfo.GetMaterialLotByMaterialLot(objectName);
                        if (mlot == null)
                        {
                            throw new Exception(TextMessage.Error.T00512(objectName));
                        }

                        dtQCTarget = mlot.CopyDataToTable(mlot.ID);

                    }
                    break;
                case "MES_EQP_EQP":
                    {
                        EquipmentInfo equipment = EquipmentInfo.GetEquipmentByName(objectName);
                        if (equipment == null)
                        {
                            throw new Exception(TextMessage.Error.T00885(objectName));
                        }

                        dtQCTarget = equipment.CopyDataToTable(equipment.ID);

                    }
                    break;
                default:
                    {
                        sql = string.Format("SELECT * FROM {0} WHERE {1} = #[STRING]", QCTypeData.QCTarget, QCTypeData.IdentityColumn);
                        dtQCTarget = DBCenter.GetDataTable(sql, objectName);
                        if (dtQCTarget == null || dtQCTarget.Rows.Count == 0)
                        {
                            throw new Exception(TextMessage.Error.T00030("InspectionTarget", objectName));
                        }
                    }
                    break;
            }
            #endregion

            return dtQCTarget;
        }

        public static Pair<string[], List<SqlAgent>> GetNamingRule(string ruleName, string user, int count, InfoBase info = null, List<string> lstArgs = null)
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
        /// 判定NG直接拆批及送待判工作站
        /// </summary>
        /// <param name="componentID">工件序號</param>
        /// <param name="defectDescription">不良說明</param>
        /// <param name="reasonData">待判原因碼</param>
        /// <param name="txnStamp"></param>
        public static void SplitDefectLot(string componentID, string defectDescription, ReasonCategoryInfo reasonData, TransactionStamp txnStamp)
        {
            //待判工作站點名稱
            string judgeOperationName = "";

            //取得批號子單元資訊
            var component = ComponentInfo.GetComponentByComponentID(componentID);

            //取得批號資料
            var txnLotData = LotInfo.GetLotByLot(component.CurrentLot).ChangeTo<LotInfoEx>();

            //在系統資料維護裡，取得此批號對應製程(CPC/CPF)的待判工作站名稱
            List<WpcExClassItemInfo> operationList = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIJudgeOperation");
            WpcExClassItemInfo judgeOperationData = operationList.Find(p => p.Remark01 == txnLotData.Process);
            if (judgeOperationData == null)
            {
                //找不到待判站資訊，請至系統資料維護增加資訊，屬性：{0}
                throw new Exception(RuleMessage.Error.C10014(txnLotData.Process));
            }

            //取得待判工作站名稱
            judgeOperationName = judgeOperationData.Remark02;

            //取得不良子批批號名稱
            var splitLotNaming = CustomizeFunction.GetNamingRule("SplitLot", txnStamp.UserID, 1, txnLotData);

            //批號拆子批
            var splitLot = SplitLotInfo.CreateSplitLotByLotAndQuantity(txnLotData.Lot, splitLotNaming.First[0], new List<ComponentInfo>() { component }, reasonData, reasonData.Description);
            WIPTxn.Default.SplitLot(txnLotData, splitLot, WIPTxn.SplitIndicator.Create(null, null, null, TerminateBehavior.NoTerminate), txnStamp);

            if (splitLotNaming.Second != null && splitLotNaming.Second.Count != 0)
            {
                DBCenter.ExecuteSQL(splitLotNaming.Second);
            }

            //註記不良
            var compDefect = ComponentDefectObject.Create(component, component.ComponentQuantity, 0, reasonData, defectDescription);
            WIPTransaction.DefectComponent(splitLot, new List<ComponentDefectObject>() { compDefect }, WIPTransaction.DefectIndicator.Create(), txnStamp);

            #region 送至待判工作站

            //取得目前批號的流程線上版本
            RouteVersionInfo RouteVersion = RouteVersionInfo.GetRouteActiveVersion(txnLotData.RouteName);

            //以目前工作站名稱去查詢在所有流程中的序號
            var routeOperation = RouteOperationInfo.GetRouteAllOperations(RouteVersion).Find(p => p.OperationName == judgeOperationName);

            //以目前工作站名稱去查詢在所有流程中的序號
            var reasonCategory = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason("Common", "OTHER");

            var modifyAttrList = new List<ModifyLotAttributeInfo>();

            //將批號的UDC01註記不良批
            modifyAttrList.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL01", "Y"));

            //將批號的UDC02註記工作站序號
            modifyAttrList.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL02", splitLot.OperationSequence));

            //將批號的UDC03註記工作站名稱
            modifyAttrList.Add(ModifyLotAttributeInfo.CreateLotSystemAttributeInfo("USERDEFINECOL03", splitLot.OperationName));

            WIPTransaction.ModifyLotMultipleAttribute(splitLot, modifyAttrList, txnStamp);

            WIPTransaction.ReassignOperation(splitLot, routeOperation, reasonCategory, reasonCategory.Description, txnStamp);

            #endregion

        }
    }
}