using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Common;
using Ares.Cimes.IntelliService.Info;
using Ares.Cimes.IntelliService.Lite.Core;
using Ares.Cimes.IntelliService.Models;
using Ares.Cimes.IntelliService.Transaction;
using CustomizeRule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Service
{
    /// <summary>
    ///CPCService 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class CPCService : System.Web.Services.WebService
    {
        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        string _ApplicationName = "AutomationUser";
        string _AutomationUserName = "AutomationUser";

        string _WepAPIUrl = System.Configuration.ConfigurationManager.AppSettings["WebAPIUrl"].ToString();

        private enum EDCRows : int
        {
            None = -1,

            /// <summary>
            /// SC1
            /// </summary>
            SC1 = 6,

            /// <summary>
            /// SC2
            /// </summary>
            SC2 = 9,

            /// <summary>
            /// SC1_MAX
            /// </summary>
            SC1_MAX = 12,

            /// <summary>
            /// SC2_MAX
            /// </summary>
            SC2_MAX = 15,

            /// <summary>
            /// SC1_MIN
            /// </summary>
            SC1_MIN = 18,

            /// <summary>
            /// SC2_MIN
            /// </summary>
            SC2_MIN = 21,

            /// <summary>
            /// SC1_ROUNDNESS
            /// </summary>
            SC1_ROUNDNESS = 24,

            /// <summary>
            /// SC2_ROUNDNESS
            /// </summary>
            SC2_ROUNDNESS = 27,

            /// <summary>
            /// 量測結果
            /// </summary>
            OutResult = 35
        }

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
        /// 上料區驗證
        /// </summary>
        /// <param name="ItemSN">工件序號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckItem(string ItemSN, string Line)
        {
            string ProgramRight = "CheckItem";
            string operationName = "上料區驗證";

            try
            {
                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //驗證都已經上線到自動化中，準備生產
                var lot = GetLotByComponentLot(ItemSN);

                //驗證2支工件是否都在第一站(OP1)
                if (lot.OperationSequence != "001")
                {
                    //工件:{0}必須為第一站OP1(目前所在站點為:{1})
                    throw new Exception(RuleMessage.Error.C10076(ItemSN, lot.OperationName));
                }

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 上料區驗證
        /// </summary>
        /// <param name="ItemSN1">第一支工件序號</param>
        /// <param name="ItemSN2">第二支工件序號</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckItems(string ItemSN1, string ItemSN2)
        {
            try
            {
                string ProgramRight = "CheckItems";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 驗證工件
                string[] itemSN = new string[] { ItemSN1, ItemSN2 };

                for (int i = 0; i < itemSN.Length; i++)
                {
                    #region 驗證2支工件是否都已經上線到自動化中，準備生產
                    var lot = GetLotByComponentLot(itemSN[i]);
                    #endregion

                    #region 驗證2支工件是否都在第一站(OP1)
                    if (lot.OperationSequence != "001")
                    {
                        //工件:{0}必須為第一站OP1(目前所在站點為:{1})
                        throw new Exception(RuleMessage.Error.C10076(itemSN[i], lot.OperationName));
                    }
                    #endregion
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
        /// OP1, OP2進站
        /// </summary>
        /// <param name="ItemSN1">第一支工件序號</param>
        /// <param name="ItemSN2">第二支工件序號</param>
        /// <param name="PLC_EQP">PLC的機台代碼</param>
        /// <param name="AlarmSignal">清洗機異常訊號(0:正常, 1:異常)</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckInOP(string ItemSN1, string ItemSN2, string PLC_EQP, string AlarmSignal, string Line)
        {
            string ProgramRight = "CheckInOP";
            string operationName = "";
            string equipmentName = "";
            string OutPut = "";

            try
            {
                //檢驗旗標(0:初始狀態, 1:巡檢, 2:首件檢查)
                string QC_Status = "0";

                string identifyName = "OP1";

                //確認為OP1還是OP2
                if (PLC_EQP == "OP2A4" || PLC_EQP == "OP2B4")
                {
                    identifyName = "OP2";
                }

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                #region 取得機台資訊

                //取得機台資訊
                var equipmentData = GetEquipmentByEquipmentCode(PLC_EQP);

                equipmentName = equipmentData.EquipmentName;

                //確認機台是否有掛載任何批號，如果有的話，自動執行批號下機台及執行出站
                ConfirmLotFromEquipment(ProgramRight, equipmentName);

                //重新取得機台資料
                equipmentData = EquipmentInfo.GetEquipmentByName(equipmentName).ChangeTo<EquipmentInfoEx>();

                //驗證機台狀態
                if (equipmentData.CurrentState != "IDLE")
                {
                    //[00872]機台({0})目前的狀態({1})無法使用.
                    throw new Exception(TextMessage.Error.T00872(equipmentData.EquipmentName, equipmentData.CurrentState));
                }
                #endregion

                #region 驗證2支工件是否存在及是否為自動線使用的工件序號
                string[] itemSN = new string[] { ItemSN1, ItemSN2 };
                List<LotModel> lotDatas = new List<LotModel>();

                for (int i = 0; i < itemSN.Length; i++)
                {
                    var lot = GetLotByComponentLot(itemSN[i]);

                    //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                    if (lot.OperationName != operationName)
                    {
                        //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                        throw new Exception(RuleMessage.Error.C10084(itemSN[i], lot.OperationName, identifyName));
                    }

                    //確認批號是否有首件資料
                    if (lot.FAIFlag == "Y")
                    {
                        //註記首件
                        QC_Status = "2";
                    }
                    //確認批號是否執行巡檢
                    else if (lot.PQCFlag == "Y")
                    {
                        //註記巡檢
                        QC_Status = "1";
                    }

                    //如果目前站點為OP2，則檢查PROCESS_EQUIP欄位是否有寫入資料
                    if (identifyName == "OP2")
                    {
                        if (lot.ProcessEquipment.IsNullOrTrimEmpty())
                        {
                            //工件編號({0}) [PROCESS_EQUIP]沒有資料 !
                            throw new Exception(RuleMessage.Error.C10146(itemSN[i]));
                        }
                    }

                    lotDatas.Add(lot.ToData<LotModel>());
                }

                #endregion

                #region 取得使用者資訊
                var userData = GetUserByID(_AutomationUserName);
                userData.CurrentProgramName = ProgramRight;
                #endregion

                #region 執行交易(呼叫ServerAPI-ExecuteCheckInOP)
                ExecuteCheckInOP requestData = new ExecuteCheckInOP()
                {
                    equipmentData = equipmentData.ToData<EquipmentModel>(),
                    lotDatas = lotDatas,
                    userData = userData
                };

                var result = CallServerAPI<ServiceResult, ExecuteCheckInOP>("WIP", "ExecuteCheckInOP", requestData);

                if (result.Success == false)
                {
                    throw new Exception(result.Message);
                }

                #endregion

                //工件排出旗標
                OutPut = (QC_Status != "0" ? "1" : "0");
                string message = "";
                #region 確認異常訊號是否為ON (只有在OP2進站時才檢查)
                if (AlarmSignal == "1" && identifyName == "OP2")
                {
                    message = "清洗機異常訊號ON，工件必須被排出";
                    message += AddErrorMessageToDB("", message, ItemSN1, Line, operationName, ProgramRight, equipmentName);
                    message += AddErrorMessageToDB("", message, ItemSN2, Line, operationName, ProgramRight, equipmentName);
                    OutPut = "1";
                }
                #endregion

                return JsonConvert.SerializeObject(new
                {
                    Result = true,
                    Message = message,
                    EQP_Available = (QC_Status == "2") ? "2" : "1",
                    QC_Status = QC_Status,
                    OutPut = OutPut,
                });
            }
            catch (Exception ex)
            {
                string returnMessage = "";
                returnMessage += AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN1, Line, operationName, ProgramRight, equipmentName);
                returnMessage += AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN2, Line, operationName, ProgramRight, equipmentName);

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage, EQP_Available = "1", QC_Status = "0", OutPut = "1", });
            }
        }

        /// <summary>
        /// OP1, OP2出站
        /// </summary>
        /// <param name="ItemSN1">第一支工件序號</param>
        /// <param name="ItemSN2">第二支工件序號</param>
        /// <param name="InspQC">巡檢 (Y: 巡檢，N:不巡檢)</param>
        /// <param name="PLC_EQP">PLC的機台代碼</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckOutOP(string ItemSN1, string ItemSN2, string InspQC, string PLC_EQP, string Line)
        {
            string ProgramRight = "CheckOutOP";
            string operationName = "";
            string equipmentName = "";

            EquipmentInfo equipmentData = null;

            try
            {
                string identifyName = "OP1";

                //確認為OP1還是OP2
                if (PLC_EQP == "OP2A4" || PLC_EQP == "OP2B4")
                {
                    identifyName = "OP2";
                }

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                #region 驗證2支工件是否存在及是否為自動線使用的工件序號
                string[] itemSN = new string[] { ItemSN1, ItemSN2 };
                List<LotModel> lotDatas = new List<LotModel>();

                for (int i = 0; i < itemSN.Length; i++)
                {
                    var lot = GetLotByComponentLot(itemSN[i]);

                    //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                    if (lot.OperationName != operationName)
                    {
                        //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                        throw new Exception(RuleMessage.Error.C10084(itemSN[i], lot.OperationName, identifyName));
                    }

                    lotDatas.Add(lot.ToData<LotModel>());
                }

                #endregion

                #region 取得機台資訊

                equipmentName = lotDatas[0].ResourceName;

                //取得機台資訊
                equipmentData = EquipmentInfo.GetEquipmentByName(lotDatas[0].ResourceName);

                if (equipmentData == null)
                {
                    //[00885]機台{0}不存在！
                    throw new Exception(TextMessage.Error.T00885(lotDatas[0].ResourceName));
                }

                //驗證機台啟用狀態
                if (equipmentData.UsingStatus == UsingStatus.Disable)
                {
                    //機台：{0}已停用，如需使用，請至"機台資料維護"啟用!!
                    throw new Exception(RuleMessage.Error.C10025(equipmentData.EquipmentName));
                }

                //驗證機台狀態(暫時先不執行此判斷式..)
                //if (equipmentData.CurrentState != "RUN")
                //{
                //    //[00872]機台({0})目前的狀態({1})無法使用.
                //    throw new Exception(TextMessage.Error.T00872(equipmentData.EquipmentName, equipmentData.CurrentState));
                //}
                #endregion

                #region 取得使用者資訊
                var userData = GetUserByID(_AutomationUserName);
                userData.CurrentProgramName = ProgramRight;
                #endregion

                #region 執行交易(呼叫ServerAPI-ExecuteCheckOutOP)
                ExecuteCheckOutOP requestData = new ExecuteCheckOutOP()
                {
                    equipmentData = equipmentData.ToData<EquipmentModel>(),
                    lotDatas = lotDatas,
                    userData = userData,
                    PQCFlag = (InspQC == "Y") ? true : false
                };

                var result = CallServerAPI<ServiceResult, ExecuteCheckOutOP>("WIP", "ExecuteCheckOutOP", requestData);

                if (result.Success == false)
                {
                    throw new Exception(result.Message);
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                string returnMessage = "";
                returnMessage += AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN1, Line, operationName, ProgramRight, equipmentName);
                returnMessage += AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN2, Line, operationName, ProgramRight, equipmentName);

                #region 更改機台狀態為MESException
                try
                {
                    //如果機台資訊不為NULL，則程式異常時，才執行變更機台狀態交易
                    if (equipmentData != null)
                    {
                        TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                        using (var cts = CimesTransactionScope.Create())
                        {
                            #region 執行變更機台狀態
                            var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("MESException");
                            if (newStateInfo == null)
                                //C00028 無機台狀態{0}的設定資料!
                                throw new CimesException(RuleMessage.Error.C00028("MESException"));

                            if (equipmentData.CurrentState != "MESException")
                            {
                                //更新機台狀態
                                EMSTransaction.ChangeState(equipmentData, newStateInfo, txnStamp);
                            }
                            #endregion

                            cts.Complete();
                        }
                    }
                }
                catch (Exception excp)
                {
                    return JsonConvert.SerializeObject(new { Result = false, Message = excp.Message });
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// O2異常或檢驗排出
        /// </summary>
        /// <param name="ItemSN1">第一支工件序號</param>
        /// <param name="ItemSN2">第二支工件序號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OutPutO2(string ItemSN1, string ItemSN2, string Line)
        {
            string ProgramRight = "OutPutO2";
            string message = "";

            try
            {
                #region 驗證2支工件是否存在及是否為自動線使用的工件序號
                string[] itemSN = new string[] { ItemSN1, ItemSN2 };
                List<LotModel> lotDatas = new List<LotModel>();

                for (int i = 0; i < itemSN.Length; i++)
                {
                    var lot = GetLotByComponentLot(itemSN[i]);

                    //若MES_WIP_LOT的FAIFLAG為N時，表示異常排出。直接送到待判站
                    if (lot.FAIFlag == "N")
                    {
                        var lotData = lot.ToData<LotModel>();
                        lotData.ReasonData = new ReasonModel() { ReasonCode = "AutomationO2Defect", ReasonCategory = "CustomizeReason" };

                        lotDatas.Add(lotData);
                    }

                    var componentData = ComponentInfo.GetComponentByComponentID(lot.ComponentLot).ChangeTo<ComponentInfoEx>();
                    if (componentData == null)
                    {
                        //工件編號:{0} 於MES_WIP_COMP查無對應資料!
                        throw new Exception(RuleMessage.Error.C10137(lot.ComponentLot));
                    }

                    if (lot.FAIFlag == "Y") message = "首件排出";
                    else if (lot.PQCFlag == "Y") message = "巡檢排出";
                    else if (componentData.PQCNGFLAG == "Y") message = "巡檢異常排出";
                    else
                    {
                        string sql = @"SELECT * FROM CST_API_ERROR_LOG WHERE COMPLOT = #[STRING] ORDER BY UPDATETIME DESC";
                        SqlAgent sa = SQLCenter.Parse(sql, lot.ComponentLot);
                        var errorLogData = InfoCenter.GetBySQL<CSTAPIErrorLogInfo>(sa);
                        if (errorLogData != null)
                        {
                            message = errorLogData.Message;
                        }
                    }
                }

                #endregion

                #region 取得使用者資訊
                var userData = GetUserByID(_AutomationUserName);
                userData.CurrentProgramName = ProgramRight;
                #endregion

                #region 執行交易(呼叫ServerAPI-ExecuteCheckOutOP)
                ExecuteOutPutO2 requestData = new ExecuteOutPutO2()
                {
                    lotDatas = lotDatas,
                    userData = userData
                };

                if (lotDatas.Count > 0)
                {
                    var result = CallServerAPI<ServiceResult, ExecuteOutPutO2>("WIP", "ExecuteOutPutO2", requestData);

                    if (result.Success == false)
                    {
                        throw new Exception(result.Message);
                    }
                }
                #endregion

                string returnMessage = "";
                returnMessage += AddErrorMessageToDB("", message, ItemSN1, Line, "O2", ProgramRight);
                returnMessage += AddErrorMessageToDB("", message, ItemSN2, Line, "O2", ProgramRight);

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                string returnMessage = "";
                returnMessage += AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN1, Line, "O2", ProgramRight);
                returnMessage += AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN2, Line, "O2", ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 清洗機入料
        /// </summary>
        /// <param name="ItemSN">工件序號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CleaningIn(string ItemSN, string Line)
        {
            string ProgramRight = "CleaningIn";
            string operationName = "";
            string equipmentName = "";

            try
            {
                string identifyName = "Cleaning";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                //取得機台資訊
                var equipmentData = GetEquipmentByEquipmentCode("CleaningEQP");

                equipmentName = equipmentData.EquipmentName;

                //取得工件序號資訊
                var lotData = GetLotByComponentLot(ItemSN);

                //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                if (lotData.OperationName != operationName)
                {
                    //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                    throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                }

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
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

                    //批號清洗進站
                    WIPTransaction.CheckIn(lotData, equipmentData.EquipmentName, "", "", LotDefaultStatus.Run.ToString(), txnStamp);

                    //批號分派
                    WIPTransaction.DispatchLot(lotData, txnStamp);

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = true, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight, equipmentName);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 清洗機出料
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="AlarmSignal">中心孔裝配異常訊號(0:正常, 1:異常)</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CleaningOut(string ItemSN, string AlarmSignal, string Line)
        {
            string ProgramRight = "CleaningOut";
            string operationName = "";
            string equipmentName = "";

            try
            {
                string identifyName = "Cleaning";
                bool result = false;
                string message = "";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                //取得工件序號資訊
                var lotData = GetLotByComponentLot(ItemSN);

                //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                if (lotData.OperationName != operationName)
                {
                    //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                    throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                }

                #region 取得機台資訊

                equipmentName = lotData.ResourceName; ;

                //取得機台資訊
                var equipmentData = EquipmentInfo.GetEquipmentByName(equipmentName);

                if (equipmentData == null)
                {
                    //[00885]機台{0}不存在！
                    throw new Exception(TextMessage.Error.T00885(equipmentName));
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

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //批號下機台
                    EMSTransaction.RemoveLotFromEquipment(lotData, equipmentData, txnStamp);

                    //批號出站
                    WIPTransaction.CheckOut(lotData, txnStamp);

                    //批號分派
                    WIPTransaction.DispatchLot(lotData, txnStamp);

                    #region 執行變更機台狀態
                    var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("IDLE");

                    if (equipmentData.CurrentState != "IDLE" && equipmentData.Capacity == 0)
                    {
                        //更新機台狀態 Run -> IDLE
                        EMSTransaction.ChangeState(equipmentData, newStateInfo, txnStamp);
                    }
                    #endregion

                    //巡檢異常時，將該工件排出。(不良原因碼記錄: INSP_NG)，否則執行降溫進站。
                    if (CheckLotQCInspectionError(lotData, txnStamp) == false)
                    {
                        result = true;
                    }
                    else
                    {
                        //工件序號:{0} 巡檢異常，必須將該工件排出！
                        message = RuleMessage.Error.C10080(ItemSN);
                    }

                    cts.Complete();
                }
                #endregion

                #region 確認異常訊號是否為ON
                if (AlarmSignal == "1")
                {
                    message = "中心孔裝配異常訊號ON，工件必須被排出";
                    result = false;
                }
                #endregion

                var returnMessage = AddErrorMessageToDB("", message, ItemSN, Line, operationName, ProgramRight, equipmentName);

                return JsonConvert.SerializeObject(new { Result = result, Message = message + returnMessage });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight, equipmentName);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 降溫進站
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CoolingDownIn(string ItemSN, string Line)
        {
            string ProgramRight = "CoolingDownIn";
            string operationName = "";

            try
            {
                string identifyName = "TempDown";
                bool result = false;

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                //取得工件序號資訊
                var lotData = GetLotByComponentLot(ItemSN);

                //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                if (lotData.OperationName != operationName)
                {
                    //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                    throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                }

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //批號降溫進站
                    WIPTransaction.CheckIn(lotData, "", "", txnStamp);

                    //批號分派
                    WIPTransaction.DispatchLot(lotData, txnStamp);

                    result = true;

                    cts.Complete();
                }
                #endregion

                return JsonConvert.SerializeObject(new { Result = result, Message = "" });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 降溫出站
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="Temp">溫度</param>
        /// <param name="AlarmSignal">中心孔裝配異常訊號(0:正常, 1:異常)</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CoolingDownOut(string ItemSN, string Temp, string AlarmSignal, string Line)
        {
            string ProgramRight = "CoolingDownOut";
            string operationName = "";

            try
            {
                string identifyName = "TempDown";
                bool result = false;
                string message = "";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                //取得工件序號資訊
                var lotData = GetLotByComponentLot(ItemSN);

                //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                if (lotData.OperationName != operationName)
                {
                    //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                    throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                }

                //如果傳入溫度為NULL，則回拋錯誤
                if (Temp.IsNullOrTrimEmpty())
                {
                    //溫度必須為數字型態!
                    throw new Exception(RuleMessage.Error.C10090());
                }

                //如果傳入溫度不為數字型態，則回拋錯誤
                decimal dTemp = 0;
                if (decimal.TryParse(Temp, out dTemp) == false)
                {
                    //溫度必須為數字型態!
                    throw new Exception(RuleMessage.Error.C10090());
                }

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    #region 記錄溫度
                    var insertData = InfoCenter.Create<CSTAPICoolingDownTempInfo>();

                    insertData.Temp = Temp;
                    insertData.ComponsetLot = ItemSN;
                    insertData.LinkSID = txnStamp.LinkSID;
                    insertData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);

                    #endregion

                    //批號出站
                    WIPTransaction.CheckOut(lotData, txnStamp);

                    //批號分派
                    WIPTransaction.DispatchLot(lotData, txnStamp);

                    //巡檢異常時，將該工件排出。(不良原因碼記錄: INSP_NG)
                    result = !CheckLotQCInspectionError(lotData, txnStamp);
                    if (result == false)
                    {
                        //工件序號:{0} 巡檢異常，必須將該工件排出！
                        message = RuleMessage.Error.C10080(ItemSN);
                    }

                    cts.Complete();
                }
                #endregion

                #region 確認異常訊號是否為ON
                if (AlarmSignal == "1")
                {
                    message = "中心孔裝配異常訊號ON，工件必須被排出";
                    result = false;
                }
                #endregion

                var returnMessage = AddErrorMessageToDB("", message, ItemSN, Line, operationName, ProgramRight);

                return JsonConvert.SerializeObject(new { Result = result, Message = message + returnMessage });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 中心孔進(寫入Barcode.txt)
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="GoldenSample">0:N 1:Y</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CenterHoleIn(string ItemSN, string GoldenSample, string Line)
        {
            string ProgramRight = "CenterHoleIn";
            string operationName = "";

            try
            {
                string identifyName = "CenterHole";
                string userName = "";
                string password = "";
                string dirPath = "";
                bool result = false;
                string message = "";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                //取得工件序號資訊
                var lotData = GetLotByComponentLot(ItemSN);

                //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                if (lotData.OperationName != operationName)
                {
                    //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                    throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                }

                #region 取得寫檔路徑及覆寫Barcode檔案
                //取得路徑對應清單
                var pathList = GetExtendItemListByClass("AutoCPC_CenterHoleFilePath");

                //寫入檔案路徑
                dirPath = pathList[0].Remark01;

                //使用者帳號
                userName = pathList[0].Remark04;

                //使用者密碼
                password = pathList[0].Remark05;

                //將工件序號寫入Barcode.txt
                ReWriteBarcodeFile(ItemSN, dirPath, userName, password);

                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //如果GoldenSample是N的話，執行中心孔進站
                    if (GoldenSample == "0")
                    {
                        //判斷巡檢是否異常，如果異常，就將該工件排出，不用進站。(不良原因碼記錄: INSP_NG)
                        if (CheckLotQCInspectionError(lotData, txnStamp) == false)
                        {
                            //中心孔進站
                            WIPTransaction.CheckIn(lotData, "", "", txnStamp);

                            //批號分派
                            WIPTransaction.DispatchLot(lotData, txnStamp);

                            result = true;
                        }
                        else
                        {
                            //工件序號:{0} 巡檢異常，必須將該工件排出！
                            message = RuleMessage.Error.C10080(ItemSN);
                        }
                    }
                    else
                    {
                        result = true;
                    }
                    cts.Complete();
                }
                #endregion

                var returnMessage = AddErrorMessageToDB("", message, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = result, Message = message + returnMessage });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 中心孔出
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="GoldenSample">0:N 1:Y</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CenterHoleOut(string ItemSN, string GoldenSample, string Line)
        {
            string ProgramRight = "CenterHoleOut";
            string operationName = "";

            try
            {
                string identifyName = "CenterHole";
                string userName = "";
                string password = "";
                string dirPath = "";
                string backupDirPath = "";
                string message = "";
                bool centerHoleOutResult = true;

                List<EDCBaseData> measureDataList = new List<EDCBaseData>();

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                #region 取得讀檔路徑及收集的資料清單
                //取得路徑對應清單
                var pathList = GetExtendItemListByClass("AutoCPC_CenterHoleFilePath");

                //讀取檔案路徑
                dirPath = pathList[0].Remark02;

                //出站備份中心孔檔案路徑
                backupDirPath = pathList[0].Remark03;

                //使用者帳號
                userName = pathList[0].Remark04;

                //使用者密碼
                password = pathList[0].Remark05;

                //讀取中心孔量測結果及收集的數據清單
                var outResult = ReadCenterHoleOutCSV(ItemSN, dirPath, ref measureDataList, userName, password);
                #endregion

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //取得MES讀檔結果
                    var MESMeasureResult = (outResult > 0) ? "NG" : "OK";

                    //確認MES讀檔結果，如果結果為NG就回傳Result = FALSE
                    if (MESMeasureResult == "NG")
                    {
                        //註記回傳結果為FALSE
                        centerHoleOutResult = false;

                        //工件序號:{0} 中心孔量測的結果為NG，必須將該工件排出！
                        message = RuleMessage.Error.C10081(ItemSN);
                    }

                    var lotNo = ItemSN;

                    if (GoldenSample == "0")
                    {
                        //取得工件序號資訊
                        var lotData = GetLotByComponentLot(ItemSN);

                        //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                        if (lotData.OperationName != operationName)
                        {
                            //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                            throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                        }

                        //註記批號名稱
                        lotNo = lotData.Lot;

                        #region 將判斷結果更新到批號的子單元系統屬性
                        string result = "Y";

                        if (MESMeasureResult == "NG")
                        {
                            result = "N";
                        }

                        //取得批號子單元資訊
                        var componentData = ComponentInfo.GetComponentByComponentID(ItemSN);

                        // 修改子單元系統屬性
                        WIPTransaction.ModifyLotComponentSystemAttribute(lotData, componentData, "CENTER_HOLE_FLAG", result, txnStamp);
                        #endregion

                        //中心孔出站
                        WIPTransaction.CheckOut(lotData, txnStamp);

                        //批號分派
                        WIPTransaction.DispatchLot(lotData, txnStamp);

                        //判斷巡檢是否異常，如果異常，就將該工件排出。(不良原因碼記錄: INSP_NG)
                        var QCInspectionError = CheckLotQCInspectionError(lotData, txnStamp);
                        if (QCInspectionError)
                        {
                            //註記回傳結果為FALSE
                            centerHoleOutResult = false;

                            //工件序號:{0} 巡檢異常，必須將該工件排出！
                            message = RuleMessage.Error.C10080(ItemSN);
                        }

                        #region TO DO 機台量測的結果(MeasureResult)與MES量測的結果，其中一個為NG時，送待判站。
                        if (MESMeasureResult == "NG" && QCInspectionError == false)
                        {
                            string reasonCode = "";

                            reasonCode = "MeasureNG_MES";

                            //送至待判站點
                            DefectNG(lotData, "CustomizeReason", reasonCode, txnStamp);

                            //註記回傳結果為FALSE
                            centerHoleOutResult = false;

                            //工件序號:{0} 中心孔量測的結果為NG，必須將該工件排出！
                            message = RuleMessage.Error.C10081(ItemSN);
                        }
                        #endregion
                    }

                    #region 記錄量測資料
                    foreach (var measureData in measureDataList)
                    {
                        var insertData = InfoCenter.Create<CSTEDCComponentInfo>();

                        insertData.Data = measureData.Data.ToCimesDecimal();
                        insertData.UpSpecification = measureData.UpSpec.ToCimesDecimal();
                        insertData.LowSpecification = measureData.LowSpec.ToCimesDecimal();
                        insertData.ComponentID = ItemSN;
                        insertData.Lot = lotNo;
                        insertData.LinkSID = txnStamp.LinkSID;
                        insertData.INSPEC = measureData.Result;
                        insertData["PARAMETER"] = "";
                        insertData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
                    }
                    #endregion

                    //自動依據系統時間新建資料夾名稱
                    backupDirPath += "\\" + DBCenter.GetSystemDateTime().ToString("yyyyMMdd");

                    //搬移檔案
                    MoveAllFileToAssemblyOutFinishDir(dirPath, backupDirPath, userName, password);

                    cts.Complete();
                }
                #endregion

                var returnMessage = AddErrorMessageToDB("", message, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = centerHoleOutResult, Message = message + returnMessage });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 組裝機進料(但不進站)
        /// </summary>
        /// <param name="ItemSN">工件序號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AssemblyInAndNotCheckIn(string ItemSN, string Line)
        {
            string ProgramRight = "AssemblyInAndNotCheckIn";
            string operationName = "";
            string equipmentName = "";

            try
            {
                string identifyName = "Assembly";
                bool result = false;
                string message = "";
                string engraving = "";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                //取得工件序號資訊
                var lotData = GetLotByComponentLot(ItemSN);

                //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                if (lotData.OperationName != operationName)
                {
                    //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                    throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                }

                //取得機台資訊
                var equipmentData = GetEquipmentByEquipmentCode("AssemblyEQP");
                equipmentName = equipmentData.EquipmentName;

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //判斷巡檢是否異常，如果異常，就將該工件排出，不用進站。(不良原因碼記錄: INSP_NG)
                    if (CheckLotQCInspectionError(lotData, txnStamp) == false)
                    {
                        result = true;

                        //取得機台對應清單
                        var equipmentCode = GetEquipmentCodeByEquipmentName(lotData.ProcessEquipment);

                        //取得刻字內容 = 機台簡碼(2碼) + INVLOT(7碼) + DEVICE(7碼)
                        engraving = equipmentCode + lotData.InventoryLot + DeviceVersionInfo.GetDeviceCurrentVersion(DeviceInfo.GetDeviceByName(lotData.DeviceName))["DEVICE_ITEM"].ToCimesString();

                        //如果刻字內容長度不等於16碼，則回拋錯誤訊息
                        if (engraving.Length != 16)
                        {
                            //刻字內容:{0} 必須等於16碼!
                            throw new Exception(RuleMessage.Error.C10091(engraving));
                        }
                    }
                    else
                    {
                        //工件序號:{0} 巡檢異常，必須將該工件排出！
                        message = RuleMessage.Error.C10080(ItemSN);
                    }

                    cts.Complete();
                }
                #endregion

                var returnMessage = AddErrorMessageToDB("", message, ItemSN, Line, operationName, ProgramRight, equipmentName);
                return JsonConvert.SerializeObject(new { Result = result, Message = message + returnMessage, Engraving = engraving });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight, equipmentName);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 組裝機進料
        /// </summary>
        /// <param name="ItemSN">工件序號</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AssemblyIn(string ItemSN, string Line)
        {
            string ProgramRight = "AssemblyIn";
            string operationName = "";
            string equipmentName = "";

            try
            {
                string identifyName = "Assembly";
                bool result = false;
                string message = "";
                string engraving = "";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                //取得工件序號資訊
                var lotData = GetLotByComponentLot(ItemSN);

                //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                if (lotData.OperationName != operationName)
                {
                    //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                    throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                }

                //取得機台資訊
                var equipmentData = GetEquipmentByEquipmentCode("AssemblyEQP");
                equipmentName = equipmentData.EquipmentName;

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    //判斷巡檢是否異常，如果異常，就將該工件排出，不用進站。(不良原因碼記錄: INSP_NG)
                    if (CheckLotQCInspectionError(lotData, txnStamp) == false)
                    {
                        #region 確認機台是否有上料及上料的資訊是否符合料號需要執行孔位

                        //取得料號版本的資料
                        var deviceVerExData = DeviceVersionInfo.GetDeviceVersion(lotData.DeviceName, lotData.DeviceVersion).ChangeTo<DeviceVersionInfoEx>();

                        //取得此料號需執行的孔位資料
                        var splitArray = deviceVerExData.PushLocation.Trim().Split(',');
                        List<string> pushLocationList = new List<string>();
                        pushLocationList.AddRange(splitArray);

                        //取得已上機的物料清單
                        var eqpMLotList = EquipmentMaterialLotInfo.GetEquipmentMaterialLotByEquipment(equipmentData.EquipmentName);
                        if (eqpMLotList.Count == 0)
                        {
                            //[01749]機台:{0}尚未上料，無法執行!
                            throw new Exception(TextMessage.Error.T01749(equipmentData.EquipmentName));
                        }

                        //比對已上機的物料清單是否都符合機台需要執行孔位資料
                        foreach (var pushLocation in pushLocationList)
                        {
                            //是否有找到符合資料的旗標
                            bool isFind = false;

                            foreach (var equMLot in eqpMLotList)
                            {
                                //取得物料資訊
                                var materialLotData = MaterialLotInfo.GetMaterialLotByMaterialLot(equMLot.MaterialLot);

                                //比對孔位是否符合
                                if (pushLocation == materialLotData.Location)
                                {
                                    isFind = true;
                                    break;
                                }
                            }

                            if (isFind == false)
                            {
                                //機台:{0} {1}孔沒有上料！
                                throw new Exception(RuleMessage.Error.C10071(equipmentData.EquipmentName, pushLocation));
                            }
                        }

                        #endregion

                        #region 執行變更機台狀態
                        var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("RUN");

                        if (equipmentData.CurrentState != "RUN")
                        {
                            //更新機台狀態 IDLE -> Run
                            EMSTransaction.ChangeState(equipmentData, newStateInfo, txnStamp);
                        }
                        #endregion

                        //批號上機台
                        EMSTxn.Default.AddLotToEquipment(lotData, equipmentData, txnStamp);

                        //組裝機進料進站
                        WIPTransaction.CheckIn(lotData, equipmentData.EquipmentName, "", "", LotDefaultStatus.Run.ToString(), txnStamp);

                        //批號分派
                        WIPTransaction.DispatchLot(lotData, txnStamp);

                        result = true;
                    }
                    else
                    {
                        //工件序號:{0} 巡檢異常，必須將該工件排出！
                        message = RuleMessage.Error.C10080(ItemSN);
                    }

                    cts.Complete();
                }
                #endregion

                var returnMessage = AddErrorMessageToDB("", message, ItemSN, Line, operationName, ProgramRight, equipmentName);
                return JsonConvert.SerializeObject(new { Result = result, Message = message + returnMessage, Engraving = engraving });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight, equipmentName);

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 組裝機出料
        /// </summary>
        /// <param name="ItemSN">工件的序號</param>
        /// <param name="AssemblyResult">1:OK 2:NG</param>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AssemblyOut(string ItemSN, string AssemblyResult, string Line)
        {
            string ProgramRight = "AssemblyOut";
            string operationName = "";
            string equipmentName = "";

            try
            {
                //確認EQP組裝結果回傳內容是否正確
                if ((AssemblyResult == "1" || AssemblyResult == "2") == false)
                {
                    //EQP組裝結果回傳: {0}，內容必須為1或2!
                    throw new Exception(RuleMessage.Error.C10099(AssemblyResult));
                }


                string identifyName = "Assembly";
                string assemblyFileCreatePath = "";
                string assemblyFileStoragePath = "";
                string assemblyFileName = "Meas_C1_Kanal_1_Prg0";
                string userName = "";
                string password = "";
                bool result = false;
                string message = "";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                //取得程式對應的工作站名稱
                operationName = OperationMapping(identifyName, Line);

                //取得工件序號資訊
                var lotData = GetLotByComponentLot(ItemSN);

                //確認工作站名稱是否符合，如果不符合的話，則拋出錯誤訊息
                if (lotData.OperationName != operationName)
                {
                    //工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
                    throw new Exception(RuleMessage.Error.C10084(ItemSN, lotData.OperationName, identifyName));
                }

                #region 取得讀檔路徑及取得檔案名稱
                //取得路徑對應清單
                var pathList = GetExtendItemListByClass("AutoCPC_AssemblyFilePath");

                //檔案產生路徑
                assemblyFileCreatePath = pathList[0].Remark01;

                //檔案保存路徑
                assemblyFileStoragePath = pathList[0].Remark02;

                //使用者帳號
                userName = pathList[0].Remark03;

                //使用者密碼
                password = pathList[0].Remark04;

                //取得CSV檔案清單
                var fileNameList = ReadAssemblyOutFileName(lotData, assemblyFileCreatePath, userName, password);

                #endregion

                #region 取得機台資訊

                equipmentName = lotData.ResourceName;

                //取得機台資訊
                var equipmentData = EquipmentInfo.GetEquipmentByName(equipmentName);

                if (equipmentData == null)
                {
                    //[00885]機台{0}不存在！
                    throw new Exception(TextMessage.Error.T00885(equipmentName));
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

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    var componentData = ComponentInfo.GetComponentByComponentID(ItemSN).ChangeTo<ComponentInfoEx>();
                    if (componentData == null)
                    {
                        //序號：{0}不存在
                        throw new Exception(RuleMessage.Error.C10009(ItemSN));
                    }

                    //確認工件是否需要檢查中心孔量測
                    //取得料號版本的資料
                    var deviceVerExData = DeviceVersionInfo.GetDeviceVersion(lotData.DeviceName, lotData.DeviceVersion).ChangeTo<DeviceVersionInfoEx>();

                    //如果旗標為Y的話，則必須檢查MES_WIP_COMP.CENTER_HOLE_FLAG是否為Y
                    if (deviceVerExData.CenterHoleFlag == "Y" && componentData.CenterHoleFlag != "Y")
                    {
                        //工件序號:{0} 必須執行中心孔量測，但尚未執行中心孔量測動作！
                        throw new Exception(RuleMessage.Error.C10077(ItemSN));
                    }

                    //取得已上機的物料清單
                    var eqpMaterialLotList = EquipmentMaterialLotInfo.GetEquipmentMaterialLotByEquipment(equipmentData.EquipmentName);
                    if (eqpMaterialLotList.Count == 0)
                    {
                        //[01749]機台:{0}尚未上料，無法執行!
                        throw new Exception(TextMessage.Error.T01749(equipmentData.EquipmentName));
                    }

                    #region 更新[MES_WIP_COMP]及扣料

                    //取得所有檔案名稱逆排序
                    var fileNameListOrderByDescending = fileNameList.OrderByDescending(data => data);
                    //註記孔位是否己經處理過(理論上，同一個孔位只會更新一次
                    bool[] isProcessedList = new bool[6];

                    foreach (var fileName in fileNameListOrderByDescending)
                    {
                        int index = 0;
                        var codeString = fileName.Substring(assemblyFileName.Length, 1);
                        //機器左手:1, 3, 5 (A, B, C)
                        //機器右手:2, 4, 6 (A, B, C)
                        switch (codeString)
                        {
                            case "1":
                            case "2":
                                index = Convert.ToInt32(codeString) - 1;
                                if (isProcessedList[index] == false)
                                {
                                    ExecuteUpdateDBAndConsumeMaterialLot("A", fileName, equipmentData.EquipmentName, eqpMaterialLotList, componentData, txnStamp);
                                    isProcessedList[index] = true;
                                }
                                break;
                            case "3":
                            case "4":
                                index = Convert.ToInt32(codeString) - 1;
                                if (isProcessedList[index] == false)
                                {
                                    ExecuteUpdateDBAndConsumeMaterialLot("B", fileName, equipmentData.EquipmentName, eqpMaterialLotList, componentData, txnStamp);
                                    isProcessedList[index] = true;
                                }
                                break;
                            case "5":
                            case "6":
                                index = Convert.ToInt32(codeString) - 1;
                                if (isProcessedList[index] == false)
                                {
                                    ExecuteUpdateDBAndConsumeMaterialLot("C", fileName, equipmentData.EquipmentName, eqpMaterialLotList, componentData, txnStamp);
                                    isProcessedList[index] = true;
                                }
                                break;
                        }
                    }
                    #endregion

                    //批號下機台
                    EMSTransaction.RemoveLotFromEquipment(lotData, equipmentData, txnStamp);

                    //批號出站
                    WIPTransaction.CheckOut(lotData, txnStamp);

                    //批號分派
                    WIPTransaction.DispatchLot(lotData, txnStamp);

                    #region 執行變更機台狀態
                    var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("IDLE");

                    if (equipmentData.CurrentState != "IDLE" && equipmentData.Capacity == 0)
                    {
                        //更新機台狀態 Run -> IDLE
                        EMSTransaction.ChangeState(equipmentData, newStateInfo, txnStamp);
                    }
                    #endregion

                    #region 巡檢異常時，將該工件排出。(不良原因碼記錄: INSP_NG)，否則執行降溫進站。
                    if (componentData.PQCNGFLAG == "Y" || AssemblyResult == "2")
                    {
                        string reasonCode = "";

                        if (componentData.PQCNGFLAG == "Y")
                        {
                            reasonCode = "INSP_NG";
                        }
                        else
                        {
                            reasonCode = "AssemblyNG";
                        }
                        //送至待判站點
                        DefectNG(lotData, "CustomizeReason", reasonCode, txnStamp);

                        if (componentData.PQCNGFLAG == "Y")
                        {
                            //工件序號:{0} 巡檢異常，必須將該工件排出！
                            message = RuleMessage.Error.C10080(ItemSN);
                        }
                        else
                        {
                            //工件序號:{0} 組裝結果為NG，必須將該工件排出！
                            message = RuleMessage.Error.C10082(ItemSN);
                        }
                    }
                    else
                    {
                        result = true;
                    }
                    #endregion

                    //自動依據系統時間新建資料夾名稱
                    assemblyFileStoragePath += "\\" + DBCenter.GetSystemDateTime().ToString("yyyyMMdd");

                    //搬移檔案
                    MoveAllFileToAssemblyOutFinishDir(assemblyFileCreatePath, assemblyFileStoragePath, userName, password, fileNameList);

                    cts.Complete();
                }
                #endregion

                var returnMessage = AddErrorMessageToDB("", message, ItemSN, Line, operationName, ProgramRight, equipmentName);
                return JsonConvert.SerializeObject(new { Result = result, Message = message + returnMessage });
            }
            catch (Exception ex)
            {
                var returnMessage = AddErrorMessageToDB(ex.StackTrace, ex.Message, ItemSN, Line, operationName, ProgramRight, equipmentName);
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message + returnMessage });
            }
        }

        /// <summary>
        /// 上傳清洗機量測數值
        /// </summary>
        /// <param name="CleaningDataList">參數清單</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UploadCleaningData(List<CleaningData> CleaningDataList)
        {
            try
            {
                string ProgramRight = "CleaningData";

                TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, ProgramRight, ProgramRight, _ApplicationName);

                #region 執行交易
                using (var cts = CimesTransactionScope.Create())
                {
                    foreach (var data in CleaningDataList)
                    {
                        //新增量測資料
                        var newCleaningData = InfoCenter.Create<CSTAPICleaningDataInfo>();

                        newCleaningData.Location = data.Location;
                        newCleaningData.DegreasingTemp = data.DegreasingTemp;
                        newCleaningData.DryingTemp = data.DryingTemp;
                        newCleaningData.ExitTemp = data.ExitTemp;
                        newCleaningData.WaterTemp = data.WaterTemp;
                        newCleaningData.DegreasingConcentration = data.DegreasingConcentration;

                        newCleaningData.InsertToDB(txnStamp.UserID, txnStamp.RecordTime);
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
        /// 取得目前該線別的機台狀態
        /// </summary>
        /// <param name="Line">線別</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetEquipmentStatus(string Line)
        {
            List<EquipmentStatus> equipStatusList = new List<EquipmentStatus>();

            try
            {
                //取得機台對應清單
                var equipMappingList = GetExtendItemListByClass("AutoCPC_EQP_Mapping");

                equipMappingList.ForEach(data =>
                {
                    var equipmentCode = data.Remark01;
                    var equipmentName = data.Remark02;
                    var line = data.Remark03;

                    //比對是否為傳入線別機台
                    if (line == Line)
                    {
                        //取得機台資訊
                        var equipmentData = EquipmentInfo.GetEquipmentByName(equipmentName).ChangeTo<EquipmentInfoEx>();
                        if (equipmentData == null)
                        {
                            //[00885]機台{0}不存在！
                            throw new Exception(TextMessage.Error.T00885(equipmentName + "(" + equipmentCode + ")"));
                        }

                        string status = "1";

                        //如果機台狀態不為RUN、IDLE、INITIAL的話，表示要停機不可生產
                        if (equipmentData.CurrentState != "RUN" && equipmentData.CurrentState != "IDLE" && equipmentData.CurrentState != "INITIAL")
                        {
                            status = "2";
                        }

                        equipStatusList.Add(new EquipmentStatus() { EquipmentName = equipmentCode, Status = status });
                    }
                });

                //如果機台資料為零筆的話，則拋錯誤訊息
                if (equipStatusList.Count == 0)
                {
                    //找不到任何機台，請確認傳入的線別({0})及系統資料設定({1})是否正確
                    throw new Exception(RuleMessage.Error.C10103(Line, "AutoCPC_EQP_Mapping"));
                }

                return JsonConvert.SerializeObject(new { Result = true, EquipmentList = equipStatusList, Message = "" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, EquipmentList = equipStatusList, Message = ex.Message });
            }
        }

        /// <summary>
        /// 呼叫ServerAPI函式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="controller">對應的控制器名稱</param>
        /// <param name="url">對應的函式名稱</param>
        /// <param name="upJson">傳入對應的參數</param>
        /// <returns></returns>
        private T CallServerAPI<T, V>(string controller, string url, V upJson)
            where T : new()
            where V : new()
        {
            string responseJson = string.Empty;

            string webAPIUrl = string.Format(_WepAPIUrl + "{0}/", controller);

            string requestJson = JsonConvert.SerializeObject(upJson);

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                client.Headers["Language"] = "zh-TW";
                byte[] response = client.UploadData(webAPIUrl + url, Encoding.UTF8.GetBytes(requestJson));
                responseJson = Encoding.UTF8.GetString(response);
            }

            return !string.IsNullOrEmpty(responseJson) ? JsonConvert.DeserializeObject<T>(responseJson) : new T();

        }

        /// <summary>
        /// 執行拆批、Defect及送至待判站點
        /// </summary>
        /// <param name="lotData"></param>
        /// <param name="txnStamp"></param>
        private void DefectNG(LotInfoEx lotData, string reasonCategory, string reasonCode, TransactionStamp txnStamp)
        {
            //批號拆批
            //取得naming
            var naming = GetNamingRule("SplitLot", txnStamp.UserID, 1, lotData);
            //更新命名規則
            if (naming.Second.Count > 0)
            {
                DBCenter.ExecuteSQL(naming.Second);
            }
            //取得原因碼
            var reason = ReasonCategoryInfo.GetReasonCategoryByCategoryNameAndReason(reasonCategory, reasonCode);

            //將母批的子單元數量給子批
            var compData = ComponentInfo.GetComponentByComponentID(lotData.ComponentLot);
            var split = SplitLotInfo.CreateSplitLotByLotAndQuantity(lotData.Lot, naming.First[0], new List<ComponentInfo>() { compData }, reason, reason.Description);
            //母批做結批
            var splitIndicator = WIPTxn.SplitIndicator.Create(null, null, null, TerminateBehavior.TerminateWithHistory);
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
            var comp = ComponentInfo.GetComponentByComponentID(lotData.ComponentLot);
            var compDefect = ComponentDefectObject.Create(comp, splitInfo.Quantity, 0, reason, reason.Description);
            WIPTransaction.DefectComponent(splitInfo, new List<ComponentDefectObject>() { compDefect }, defectIndicator, txnStamp);
            //子批做Reassign Operation
            //取得代判站點
            var judgeOperation = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks("SAIJudgeOperation");
            //取得代判站點為CPC
            var reassignOperation = judgeOperation.Find(p => p.Remark01 == "CPC");
            if (reassignOperation == null)
            {
                //T00555:查無資料，請至系統資料維護新增類別{0}、項目{1}!
                throw new CimesException(TextMessage.Error.T00555("SAIJudgeOperation", "CPC"));
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
            //變更至指定工作站
            WIPTransaction.ReassignOperation(splitInfo, newOperation, reason, "", txnStamp);
            //判定不良會拆批，子批的UDC02會記錄工作的SEQ，UDC03會記錄工作站名稱
            WIPTransaction.ModifyLotSystemAttribute(splitInfo, "USERDEFINECOL02", lotData.OperationSequence, txnStamp);
            WIPTransaction.ModifyLotSystemAttribute(splitInfo, "USERDEFINECOL03", lotData.OperationName, txnStamp);
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
        /// 傳入EAP機台取得機台簡碼
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        private string GetEquipmentCodeByEquipmentName(string equipmentName)
        {
            //取得機台對應清單
            var equipMappingList = GetExtendItemListByClass("AutoCPC_EQP_Mapping");

            string equipmentCode = "";

            foreach (var data in equipMappingList)
            {
                //比對機台名稱是否相同
                if (data.Remark02 == equipmentName)
                {
                    //取得機台簡碼
                    equipmentCode = data.Remark04;
                    break;
                }
            }

            if (string.IsNullOrEmpty(equipmentCode))
            {
                // [00466]系統控制設定，類別：[{0}]，項目：[{1}]未設定！
                throw new Exception(TextMessage.Error.T00466("AutoCPC_EQP_Mapping", equipmentName));
            }

            return equipmentCode;
        }

        /// <summary>
        /// 傳入EAP機台代碼取得機台資訊
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        private EquipmentInfoEx GetEquipmentByEquipmentCode(string equipmentCode)
        {
            //取得機台對應清單
            var equipMappingList = GetExtendItemListByClass("AutoCPC_EQP_Mapping");

            string equipmentName = "";

            equipMappingList.ForEach(data =>
            {
                if (data.Remark01 == equipmentCode)
                {
                    equipmentName = data.Remark02;
                }
            });

            if (string.IsNullOrEmpty(equipmentName))
            {
                // [00466]系統控制設定，類別：[{0}]，項目：[{1}]未設定！
                throw new Exception(TextMessage.Error.T00466("AutoCPC_EQP_Mapping", equipmentName));
            }

            //取得機台資訊
            var equipmentData = EquipmentInfo.GetEquipmentByName(equipmentName).ChangeTo<EquipmentInfoEx>();

            if (equipmentData == null)
            {
                //[00885]機台{0}不存在！
                throw new Exception(TextMessage.Error.T00885(equipmentName));
            }

            //驗證機台啟用狀態
            if (equipmentData.UsingStatus == UsingStatus.Disable)
            {
                //機台：{0}已停用，如需使用，請至"機台資料維護"啟用!!
                throw new Exception(RuleMessage.Error.C10025(equipmentData.EquipmentName));
            }

            return equipmentData;
        }

        /// <summary>
        /// 傳入工件序號取得批號資訊
        /// </summary>
        /// <param name="componentLot"></param>
        /// <returns></returns>
        private LotInfoEx GetLotByComponentLot(string componentLot)
        {
            if (componentLot.IsNullOrTrimEmpty())
            {
                //PLC工件點位為空值！
                throw new Exception(RuleMessage.Error.C10183());
            }

            var lotList = LotInfoEx.GetLotListByComponentLot(componentLot);
            if (lotList.Count == 0)
            {
                //工件序號：{0} 不存在!!
                throw new Exception(RuleMessage.Error.C10047(componentLot));
            }

            var automationLots = lotList.FindAll(lot => lot.UserDefineColumn08 == "Y");
            if (automationLots.Count == 0)
            {
                //工件序號:{0} 不屬於自動線!
                throw new Exception(RuleMessage.Error.C10058(componentLot));
            }

            return automationLots[0];
        }

        /// <summary>
        /// 確認傳入的批號是否有過巡檢異常的註記，如果有的話，則直接送至待判站，並回傳結果為TRUE
        /// </summary>
        /// <param name="lotData"></param>
        /// <param name="">txnStamp</param>
        /// <param name="reasonCode">原因碼(預設為INSP_NG)</param>
        /// <returns></returns>
        private bool CheckLotQCInspectionError(LotInfoEx lotData, TransactionStamp txnStamp, string reasonCode = "INSP_NG")
        {
            bool result = false;

            var componentData = ComponentInfo.GetComponentByComponentID(lotData.ComponentLot).ChangeTo<ComponentInfoEx>();

            if (componentData == null)
            {
                //工件編號:{0} 於MES_WIP_COMP查無對應資料!
                throw new Exception(RuleMessage.Error.C10137(lotData.ComponentLot));
            }

            //確認是否註記巡檢異常
            if (componentData.PQCNGFLAG == "Y")
            {
                //送至待判站點
                DefectNG(lotData, "CustomizeReason", reasonCode, txnStamp);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 切換使用者權限
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private IntPtr Impersonate(string userName, string password, string domain)
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
            IntPtr tokenHandle = new IntPtr(0);
            tokenHandle = IntPtr.Zero;

            bool returnValue = LogonUser(userName, domain, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref tokenHandle);

            if (returnValue == false)
            {
                //切換使用者權限失敗(使用者:{0})！
                throw new Exception(RuleMessage.Error.C10072(userName));
            }

            return tokenHandle;
        }

        /// <summary>
        /// 取得組裝出料後的檔案名稱(CSV)
        /// </summary>
        /// <param name="lotData">工件序號資訊</param>
        /// <param name="DirPath">資料夾路徑</param>
        /// <param name="userName">使用者帳號</param>
        /// <param name="password">使用密碼</param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private List<string> ReadAssemblyOutFileName(LotInfoEx lotData, string DirPath, string userName, string password, string domain = "domain")
        {
            WindowsImpersonationContext impersonationContext = null;

            //如果使用者帳號不為空白才執行切換帳號功能
            if (userName.IsNullOrTrimEmpty() == false)
            {
                //切換使用者
                var tokenHandle = Impersonate(userName, password, domain);
                WindowsIdentity windowsIdentity = new WindowsIdentity(tokenHandle);
                impersonationContext = windowsIdentity.Impersonate();
            }

            List<string> FileNameList = new List<string>();

            DirectoryInfo di = new DirectoryInfo(@DirPath);

            //取得機台對應清單
            var equipmentCode = GetEquipmentCodeByEquipmentName(lotData.ProcessEquipment);

            //取得刻字內容 = 機台簡碼(2碼) + INVLOT(7碼) + DEVICE(7碼)
            var engraving = equipmentCode + lotData.InventoryLot + DeviceVersionInfo.GetDeviceCurrentVersion(DeviceInfo.GetDeviceByName(lotData.DeviceName))["DEVICE_ITEM"].ToCimesString();

            foreach (var fi in di.GetFiles())
            {
                //只加入副檔名為CSV的檔案名稱
                if (fi.Extension.ToLower() == ".csv")
                {
                    //檔案名稱符合刻字碼的才加入檔案清單
                    if (fi.Name.Contains(engraving))
                    {
                        FileNameList.Add(fi.Name);
                    }
                }
            }

            if (impersonationContext != null)
            {
                //切換回原身分
                impersonationContext.Undo();
            }

            return FileNameList;
        }

        /// <summary>
        /// 搬移檔案
        /// </summary>
        /// <param name="sourceDirPath">來源路徑位置</param>
        /// <param name="finishDirPath">目標路徑位置</param>
        /// <param name="userName">使用者帳號</param>
        /// <param name="password">使用密碼</param>
        /// <param name="fileNameList">搬移檔案清單</param>
        /// <param name="domain"></param>
        private void MoveAllFileToAssemblyOutFinishDir(string sourceDirPath, string finishDirPath, string userName,
            string password, List<string> fileNameList = null, string domain = "domain")
        {
            WindowsImpersonationContext impersonationContext = null;

            //如果使用者帳號不為空白才執行切換帳號功能
            if (userName.IsNullOrTrimEmpty() == false)
            {
                //切換使用者
                var tokenHandle = Impersonate(userName, password, domain);
                WindowsIdentity windowsIdentity = new WindowsIdentity(tokenHandle);
                impersonationContext = windowsIdentity.Impersonate();
            }

            //判斷來源路徑是否存在
            if (Directory.Exists(sourceDirPath) == false)
            {
                //來源路徑位置:{0} 不存在！
                throw new Exception(RuleMessage.Error.C10073(sourceDirPath));
            }

            //判斷目標路徑是否存在
            if (Directory.Exists(finishDirPath) == false)
            {
                //如果不存在，則新增一個資料夾路徑
                Directory.CreateDirectory(finishDirPath);
            }

            if (fileNameList == null)
            {
                //取得來源路徑底下的所有檔案資料
                string[] files = Directory.GetFiles(sourceDirPath);

                foreach (string s in files)
                {
                    //取得檔案名稱
                    var fileName = Path.GetFileName(s);

                    //取得檔案搬移的完整路徑
                    var destFile = Path.Combine(finishDirPath, fileName);

                    //確認路徑是否存在
                    if (File.Exists(destFile))
                    {
                        //如果存在，則刪除舊有的檔案
                        File.Delete(destFile);
                    }

                    //搬移檔案
                    File.Move(s, destFile);
                }
            }
            else
            {
                //搬移檔案清單內的資料
                foreach (string s in fileNameList)
                {
                    //取得原始檔案搬移的完整路徑
                    var sourceFile = Path.Combine(sourceDirPath, s);

                    //取得檔案搬移的完整路徑
                    var destFile = Path.Combine(finishDirPath, s);

                    //確認路徑是否存在
                    if (File.Exists(destFile))
                    {
                        //如果存在，則刪除舊有的檔案
                        File.Delete(destFile);
                    }

                    //搬移檔案
                    File.Move(sourceFile, destFile);
                }
            }

            if (impersonationContext != null)
            {
                //切換回原身分
                impersonationContext.Undo();
            }
        }

        /// <summary>
        /// 執行更新[MES_WIP_COMP]及扣物料動作
        /// </summary>
        /// <param name="pushLocation">孔位</param>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="equipmentName">機台名稱</param>
        /// <param name="eqpMaterialLotList">物料清單</param>
        /// <param name="componentData">批號子單元</param>
        /// <param name="txnStamp"></param>
        private void ExecuteUpdateDBAndConsumeMaterialLot(string pushLocation, string fileName, string equipmentName,
            List<EquipmentMaterialLotInfo> eqpMaterialLotList, ComponentInfoEx componentData, TransactionStamp txnStamp)
        {
            //扣料的物料批資訊
            MaterialLotInfo consumeMaterialLotData = null;
            DateTime consumeMLotUpdateTime = new DateTime();

            foreach (var equMLot in eqpMaterialLotList)
            {
                //取得相對應孔位的物料資料
                var materialLotData = MaterialLotInfo.GetMaterialLotByMaterialLot(equMLot.MaterialLot);

                //比對是否符合相對應的孔位
                if (pushLocation == materialLotData.Location)
                {
                    //如果consumeMaterialLotData不是null，表示此孔位的物料有多筆的情況
                    if (consumeMaterialLotData != null)
                    {
                        //機台上料可能有一筆以上相同的孔位物料，在記錄及扣料時，選擇上料時間比較早的物料資訊
                        var updateTime = DateTime.Parse(equMLot.UpdateTime);

                        //取上料時間比較早的物料資料
                        if (consumeMLotUpdateTime > updateTime)
                        {
                            consumeMaterialLotData = materialLotData;
                        }
                    }
                    else
                    {
                        consumeMaterialLotData = materialLotData;
                        consumeMLotUpdateTime = DateTime.Parse(equMLot.UpdateTime);
                    }
                }
            }

            //如果機台沒有上料的話，則執錯誤訊息
            if (consumeMaterialLotData == null)
            {
                //機台:{0} {1}孔沒有上料！
                throw new Exception(RuleMessage.Error.C10071(equipmentName, pushLocation));
            }

            //執行物料耗用記錄
            MMSTransaction.ConsumeMaterialLot(consumeMaterialLotData, InfoCenter.Create<EquipmentInfo>(),
                ConsumeInfo.CreateConsume(1, 0), MMSTransaction.MMSConsumeIndicator.Create(), txnStamp);

            //記錄孔位使用的檔案名稱及使用的物料批號
            switch (pushLocation)
            {
                case "A":
                    componentData.FileA = fileName;
                    componentData.WasherA = consumeMaterialLotData.MaterialLot;
                    break;
                case "B":
                    componentData.FileB = fileName;
                    componentData.WasherB = consumeMaterialLotData.MaterialLot;
                    break;
                case "C":
                    componentData.FileC = fileName;
                    componentData.WasherC = consumeMaterialLotData.MaterialLot;
                    break;
            }

            //更新DB
            componentData.UpdateToDB(txnStamp.UserID, txnStamp.RecordTime);
        }

        /// <summary>
        /// 取得使用者資訊(轉換成Model Data)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private UserData GetUserByID(string userID)
        {
            var info = UserProfileInfo.GetUserByID(userID);
            if (info == null)
            {
                //  [00474]使用者{0}不存在!
                throw new Exception(TextMessage.Error.T00474(userID));
            }
            return info.ToData<UserData>();
        }

        /// <summary>
        /// 將傳入的工件序號覆寫到Barcode.txt檔案
        /// </summary>
        /// <param name="itemSN">工件的序號</param>
        /// <param name="dirPath">寫檔的路徑</param>
        /// <param name="userName">使用者帳號</param>
        /// <param name="password">使用者密碼</param>
        /// <param name="domain"></param>
        private void ReWriteBarcodeFile(string itemSN, string dirPath, string userName, string password, string domain = "domain")
        {
            WindowsImpersonationContext impersonationContext = null;

            //如果使用者帳號不為空白才執行切換帳號功能
            if (userName.IsNullOrTrimEmpty() == false)
            {
                //切換使用者
                var tokenHandle = Impersonate(userName, password, domain);
                WindowsIdentity windowsIdentity = new WindowsIdentity(tokenHandle);
                impersonationContext = windowsIdentity.Impersonate();
            }

            var filePath = dirPath + @"\Barcode.txt";

            //確認路徑是否存在
            if (File.Exists(filePath))
            {
                //如果存在，則刪除舊有的檔案
                File.Delete(filePath);
            }

            File.WriteAllText(filePath, itemSN);

            if (impersonationContext != null)
            {
                //切換回原身分
                impersonationContext.Undo();
            }
        }

        /// <summary>
        /// 讀取中心孔量測結果及收集的數據清單
        /// </summary>
        /// <param name="itemSN">工件的序號</param>
        /// <param name="dirPath">讀檔的路徑</param>
        /// <param name="EDCDataList">收集數據清單</param>
        /// <param name="userName">使用者帳號</param>
        /// <param name="password">使用者密碼</param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private int ReadCenterHoleOutCSV(string itemSN, string dirPath, ref List<EDCBaseData> EDCDataList, string userName, string password, string domain = "domain")
        {
            int outResult = 0;

            WindowsImpersonationContext impersonationContext = null;

            //如果使用者帳號不為空白才執行切換帳號功能
            if (userName.IsNullOrTrimEmpty() == false)
            {
                //切換使用者
                var tokenHandle = Impersonate(userName, password, domain);
                WindowsIdentity windowsIdentity = new WindowsIdentity(tokenHandle);
                impersonationContext = windowsIdentity.Impersonate();
            }

            //讀取文件路徑
            string readFilePath = dirPath + @"\measure_" + itemSN + @".csv";

            //確認檔案是否存在
            if (File.Exists(readFilePath) == false)
            {
                //中心孔量測檔案:{0} 不存在！
                throw new Exception(RuleMessage.Error.C10078("measure_" + itemSN + @".csv"));
            }

            using (var reader = new StreamReader(readFilePath))
            {
                //定義文件讀取最大行數(實際為37行)
                int maxLine = 36;

                for (int i = 1; i <= maxLine; i++)
                {
                    //確認文件是否為最後一行，如果是的話，則表示行數小於設定值
                    if (reader.EndOfStream)
                    {
                        //中心孔量測檔案:{0} 文件行數小於{1}，格式不符，請檢查文件來源！
                        throw new Exception(RuleMessage.Error.C10079(itemSN + @".csv", maxLine.ToString()));
                    }

                    string data = "";
                    string[] arrData;

                    //將行數轉換成判斷的列舉型態
                    var line = ChangeEDCRows(i);

                    switch (line)
                    {
                        case EDCRows.SC1:
                        case EDCRows.SC2:
                        case EDCRows.SC1_MAX:
                        case EDCRows.SC2_MAX:
                        case EDCRows.SC1_MIN:
                        case EDCRows.SC2_MIN:
                        case EDCRows.SC1_ROUNDNESS:
                        case EDCRows.SC2_ROUNDNESS:
                            data = reader.ReadLine();
                            arrData = data.Split(new char[] { ',' }, StringSplitOptions.None);
                            EDCDataList.Add(CalculateEDCData(arrData, line.ToString()));
                            break;

                        case EDCRows.OutResult:
                            data = reader.ReadLine();
                            arrData = data.Split(new char[] { ',' }, StringSplitOptions.None);
                            outResult = Convert.ToInt32(arrData[1]);
                            break;

                        default:
                            reader.ReadLine();
                            break;
                    }
                }

                if (impersonationContext != null)
                {
                    //切換回原身分
                    impersonationContext.Undo();
                }
            }

            return outResult;
        }

        /// <summary>
        /// 將行數轉換成判斷的列舉型態
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private EDCRows ChangeEDCRows(int line)
        {
            EDCRows eunRows = EDCRows.None;

            foreach (EDCRows enumEDCRows in System.Enum.GetValues(typeof(EDCRows)))
            {
                if (line == (int)enumEDCRows)
                {
                    eunRows = enumEDCRows;
                    break;
                }
            }

            return eunRows;
        }

        /// <summary>
        /// 計算數值及上下限值
        /// </summary>
        /// <param name="arrEDCData"></param>
        /// <returns></returns>
        private EDCBaseData CalculateEDCData(string[] arrEDCData, string parameter)
        {
            var EDCData = new EDCBaseData();

            var actualData = "";
            decimal nominalData = 0;
            decimal lowTol = 0;
            decimal hiTol = 0;
            decimal lowLimitData = 0;
            decimal hiLimitData = 0;
            string result = "OK";

            actualData = arrEDCData[1];
            nominalData = arrEDCData[2] == "" ? 0 : Convert.ToDecimal(arrEDCData[2]);
            lowTol = arrEDCData[3] == "" ? 0 : Convert.ToDecimal(arrEDCData[3]);
            hiTol = arrEDCData[4] == "" ? 0 : Convert.ToDecimal(arrEDCData[4]);
            lowLimitData = nominalData + lowTol;
            hiLimitData = nominalData + hiTol;

            //如果大於上限值或小於下限值，則量測結果的NG
            if (Convert.ToDecimal(actualData) < lowLimitData || Convert.ToDecimal(actualData) > hiLimitData)
            {
                result = "NG";
            }

            //例外處理
            if (arrEDCData[3] == "" && arrEDCData[4] == "")
            {
                result = "OK";
            }

            EDCData.Parameter = parameter;
            EDCData.Data = actualData;
            EDCData.LowSpec = lowLimitData.ToString();
            EDCData.UpSpec = hiLimitData.ToString();
            EDCData.Result = result;

            return EDCData;
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
        /// 將傳入的資料進行比對，查詢是否符合自動化站點對應表中的資料
        /// </summary>
        /// <param name="identifyName">識別名稱</param>
        /// <param name="line">線別</param>
        private string OperationMapping(string identifyName, string line)
        {
            string operationName = "";
            string className = "AutoCPC_Operation_Mapping";

            //取得系統資料設定的自動化站點對應表
            var dataList = GetExtendItemListByClass(className);

            //比對符合的識別名稱
            foreach (var data in dataList)
            {
                if (data.Remark01 == identifyName && data.Remark03 == line)
                {
                    //取得工作站名稱
                    operationName = data.Remark02;
                    break;
                }
            }

            //如果工作站名稱找不到，則拋出錯誤訊息
            if (operationName.IsNullOrTrimEmpty())
            {
                // [00466]系統控制設定，類別：[{0}]，項目：[{1}]未設定！
                throw new Exception(TextMessage.Error.T00466(className, identifyName));
            }

            return operationName;
        }

        /// <summary>
        /// 測試函式
        /// </summary>
        /// <param name="sourceDirPath"></param>
        /// <param name="finishDirPath"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string _MoveAllFileToAssemblyOutFinishDir(string sourceDirPath, string finishDirPath, string userName,
            string password, string domain = "domain")
        {
            try
            {
                WindowsImpersonationContext impersonationContext = null;

                //如果使用者帳號不為空白才執行切換帳號功能
                if (userName.IsNullOrTrimEmpty() == false)
                {
                    //切換使用者
                    var tokenHandle = Impersonate(userName, password, domain);
                    WindowsIdentity windowsIdentity = new WindowsIdentity(tokenHandle);
                    impersonationContext = windowsIdentity.Impersonate();
                }

                //判斷來源路徑是否存在
                if (Directory.Exists(sourceDirPath) == false)
                {
                    //來源路徑位置:{0} 不存在！
                    throw new Exception(RuleMessage.Error.C10073(sourceDirPath));
                }

                //判斷目標路徑是否存在
                if (Directory.Exists(finishDirPath) == false)
                {
                    //如果不存在，則新增一個資料夾路徑
                    Directory.CreateDirectory(finishDirPath);
                }

                //取得來源路徑底下的所有檔案資料
                string[] files = Directory.GetFiles(sourceDirPath);

                foreach (string s in files)
                {
                    //取得檔案名稱
                    var fileName = Path.GetFileName(s);

                    //取得檔案搬移的完整路徑
                    var destFile = Path.Combine(finishDirPath, fileName);

                    //確認路徑是否存在
                    if (File.Exists(destFile))
                    {
                        //如果存在，則刪除舊有的檔案
                        File.Delete(destFile);
                    }

                    //搬移檔案
                    File.Move(s, destFile);
                }

                if (impersonationContext != null)
                {
                    //切換回原身分
                    impersonationContext.Undo();
                }

                return JsonConvert.SerializeObject(new { Result = true });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    Result = false,
                    Message = ex.Message
                });
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
        /// <param name="equipmentName">機台編號</param>
        /// <returns></returns>
        private string AddErrorMessageToDB(string stackTrace, string errorMessage, string compLot, string line, string opeationName, string programRight, string equipmentName = "")
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
        /// 確認機台是否有掛載任何批號，如果有的話，自動執行批號下機台及執行出站
        /// </summary>
        /// <param name="programRight"></param>
        /// <param name="equipmentName"></param>
        private void ConfirmLotFromEquipment(string programRight, string equipmentName)
        {
            TransactionStamp txnStamp = new TransactionStamp(_ApplicationName, programRight, programRight, _ApplicationName);

            //依據機台編號取得目前該機台有上機的批號資料
            var equipLots = EquipmentLotInfo.GetEquipmentLotByEquipment(equipmentName);

            //取得機台資料
            var equipment = EquipmentInfo.GetEquipmentByName(equipmentName).ChangeTo<EquipmentInfoEx>();

            using (var cts = CimesTransactionScope.Create())
            {
                equipLots.ForEach(equipLot =>
                {
                    //取得批號資料
                    var lotData = LotInfo.GetLotByLot(equipLot.Lot);

                    //批號下機台
                    EMSTransaction.RemoveLotFromEquipment(lotData, equipment, txnStamp);

                    //註記是系統執行出站
                    txnStamp.Remark10 = "System Check Out";

                    //批號出站
                    WIPTransaction.CheckOut(lotData, txnStamp);

                    //批號分派
                    WIPTransaction.DispatchLot(lotData, txnStamp);

                    //確認機台的最大容量是否為零，如果不是，則執行變更機台狀態
                    if (equipment.Capacity == 0)
                    {
                        var newStateInfo = EquipmentStateInfo.GetEquipmentStateByState("IDLE");

                        if (equipment.CurrentState != "IDLE")
                        {
                            //變更機台狀態 Run=>IDLE
                            EMSTransaction.ChangeState(equipment, newStateInfo, txnStamp);
                        }
                    }
                });

                cts.Complete();
            }
        }
        #region Models

        /// <summary>
        /// 機台目前的執行狀態
        /// </summary>
        public class EquipmentStatus
        {
            /// <summary>
            /// 機台名稱
            /// </summary>
            public string EquipmentName { get; set; }

            /// <summary>
            /// 機台狀態
            /// </summary>
            public string Status { get; set; }
        }

        /// <summary>
        /// 清洗機量測數值類別
        /// </summary>
        public class CleaningData
        {
            /// <summary>
            /// 脫脂濃度
            /// </summary>
            public string DegreasingConcentration { get; set; }

            /// <summary>
            /// 脫脂水溫
            /// </summary>
            public string DegreasingTemp { get; set; }

            /// <summary>
            /// 清水水溫
            /// </summary>
            public string WaterTemp { get; set; }

            /// <summary>
            /// 烘乾溫度
            /// </summary>
            public string DryingTemp { get; set; }

            /// <summary>
            /// 出口溫度
            /// </summary>
            public string ExitTemp { get; set; }

            /// <summary>
            /// 位置
            /// </summary>
            public string Location { get; set; }
        }

        /// <summary>
        /// 中心孔量測數值類別
        /// </summary>
        public class EDCBaseData
        {
            /// <summary>
            /// 數值
            /// </summary>
            public string Data { get; set; }

            /// <summary>
            /// 上限值
            /// </summary>
            public string UpSpec { get; set; }

            /// <summary>
            /// 下限值
            /// </summary>
            public string LowSpec { get; set; }

            /// <summary>
            /// 量測結果
            /// </summary>
            public string Result { get; set; }

            /// <summary>
            /// 參數名稱
            /// </summary>
            public string Parameter { get; set; }
        }

        /// <summary>
        ///  OP1, OP2進站-傳入參數類別
        /// </summary>
        private class ExecuteCheckInOP
        {
            /// <summary>
            /// 機台資訊
            /// </summary>
            public EquipmentModel equipmentData { get; set; }

            /// <summary>
            /// 批號清單
            /// </summary>
            public List<LotModel> lotDatas { get; set; }

            /// <summary>
            /// 使用者資訊
            /// </summary>
            public UserData userData { get; set; }
        }

        /// <summary>
        /// OP1, OP2出站-傳入參數類別
        /// </summary>
        private class ExecuteCheckOutOP
        {
            /// <summary>
            /// 機台資訊
            /// </summary>
            public EquipmentModel equipmentData { get; set; }

            /// <summary>
            /// 批號清單
            /// </summary>
            public List<LotModel> lotDatas { get; set; }

            /// <summary>
            /// 使用者資訊
            /// </summary>
            public UserData userData { get; set; }

            /// <summary>
            /// 執行巡檢旗標
            /// </summary>
            public bool PQCFlag { get; set; }
        }

        /// <summary>
        /// O2異常或檢驗排出-傳入參數類別
        /// </summary>
        private class ExecuteOutPutO2
        {
            /// <summary>
            /// 批號清單
            /// </summary>
            public List<LotModel> lotDatas { get; set; }

            /// <summary>
            /// 使用者資訊
            /// </summary>
            public UserData userData { get; set; }
        }

        /// <summary>
        /// CallWepAPI-回傳參數類別
        /// </summary>
        private class ServiceResult
        {
            /// <summary>
            /// 回傳執行結果
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// 錯誤訊息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 回傳資料結構
            /// </summary>
            public object Data { get; set; }
        }

        #endregion
    }
}
