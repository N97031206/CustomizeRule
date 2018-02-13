using Ares.Cimes.IntelliService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomizeRule
{
    public partial class RuleMessage : TextMessage
    {
        public new static RuleMessage Error { get; set; }
        public new static RuleMessage Hint { get; set; }
        public new static RuleMessage Warning { get; set; }

        static RuleMessage()
        {
            RuleMessage.Error = RuleMessage.CreateError<RuleMessage>();
            RuleMessage.Hint = RuleMessage.CreateHint<RuleMessage>();
            RuleMessage.Warning = RuleMessage.CreateWarning<RuleMessage>();
        }

        public RuleMessage()
        {
            AddResourceManager(RuleMessageDefault.ResourceManager);
        }
        /// <summary>
        /// 型號:{0} 生產型態: {1} ，請輸入工件序號 !
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C00001(string args0, string args1)
        {
            return GetMessageString("C00001", args0, args1);
        }

        /// <summary>
        /// 批號：{0}已進行站點：{1}預約進站作業，請遵循作業規範
        /// </summary>
        /// <returns></returns>
        public string C10001(string args0, string args1)
        {
            return GetMessageString("C10001", args0, args1);
        }


        /// <summary>
        /// 站點：{0}，找不到設定規則，請確認[工作站設定]
        /// </summary>
        /// <returns></returns>
        public string C10002(string args0)
        {
            return GetMessageString("C10002", args0);
        }

        /// <summary>
        /// 狀態為{0}，不可執行
        /// </summary>
        /// <returns></returns>
        public string C10003(string args0)
        {
            return GetMessageString("C10003", args0);
        }

        /// <summary>
        /// 該批號作業為{0}，不為此功能，請遵循作業規範
        /// </summary>
        /// <returns></returns>
        public string C10004(string args0)
        {
            return GetMessageString("C10004", args0);
        }

        /// <summary>
        /// 機台：{0}，不在可用機台範圍，請至 [工作站設定] 確認機台設定
        /// </summary>
        /// <returns></returns>
        public string C10005(string args0)
        {
            return GetMessageString("C10005", args0);
        }

        /// <summary>
        /// 機台：{0}不存在
        /// </summary>
        /// <returns></returns>
        public string C10006(string args0)
        {
            return GetMessageString("C10006", args0);
        }

        /// <summary>
        /// 批號：{0}不存在
        /// </summary>
        /// <returns></returns>
        public string C10007(string args0)
        {
            return GetMessageString("C10007", args0);
        }

        /// <summary>
        /// 批號：{0}已無下個工作站點，請確認[流程設定]
        /// </summary>
        /// <returns></returns>
        public string C10008(string args0)
        {
            return GetMessageString("C10008", args0);
        }

        /// <summary>
        /// 序號：{0}不存在
        /// </summary>
        /// <returns></returns>
        public string C10009(string args0)
        {
            return GetMessageString("C10009", args0);
        }

        /// <summary>
        /// 序號：{0}所屬批號為{1}，請確認資料正確性!
        /// </summary>
        /// <returns></returns>
        public string C10010(string args0, string args1)
        {
            return GetMessageString("C10010", args0, args1);
        }

        /// <summary>
        /// 序號：{0}已存在於不良清單裡，請確認資料正確性!
        /// </summary>
        /// <returns></returns>
        public string C10011(string args0)
        {
            return GetMessageString("C10011", args0);
        }

        /// <summary>
        /// 請輸入生產編號!
        /// </summary>
        /// <returns></returns>
        public string C10012()
        {
            return GetMessageString("C10012");
        }

        /// <summary>
        /// 不良數量不可為0!
        /// </summary>
        /// <returns></returns>
        public string C10013()
        {
            return GetMessageString("C10013");
        }

        /// <summary>
        /// 找不到待判站資訊，請至系統資料維護增加資訊，屬性：{0}
        /// </summary>
        /// <returns></returns>
        public string C10014(string args0)
        {
            return GetMessageString("C10014", args0);
        }

        /// <summary>
        /// 不良數量不可大於批號數量!
        /// </summary>
        /// <returns></returns>
        public string C10015()
        {
            return GetMessageString("C10015");
        }

        /// <summary>
        /// 模具：{0}不存在，請確認資料正確性!!
        /// </summary>
        /// <returns></returns>
        public string C10016(string args0)
        {
            return GetMessageString("C10016", args0);
        }

        /// <summary>
        /// 模具：{0}已停用，如需使用，請至"配件資料維護"啟用!!
        /// </summary>
        /// <returns></returns>
        public string C10017(string args0)
        {
            return GetMessageString("C10017", args0);
        }

        /// <summary>
        /// 模具儲位為{0}，不在庫房，不須領用!!
        /// </summary>
        /// <returns></returns>
        public string C10018(string args0)
        {
            return GetMessageString("C10018", args0);
        }

        /// <summary>
        /// 模具狀態為{0}，請進行降模或鉗修保養!!
        /// </summary>
        /// <returns></returns>
        public string C10019(string args0)
        {
            return GetMessageString("C10019", args0);
        }

        /// <summary>
        /// 模具狀態為{0}，不可執行此功能!!
        /// </summary>
        /// <returns></returns>
        public string C10020(string args0)
        {
            return GetMessageString("C10020", args0);
        }

        /// <summary>
        /// 模治具儲位為{0}，不在線邊，不須歸還!!
        /// </summary>
        /// <returns></returns>
        public string C10021(string args0)
        {
            return GetMessageString("C10021", args0);
        }

        /// <summary>
        /// 模治具狀態為{0}，不可執行此功能!!
        /// </summary>
        /// <returns></returns>
        public string C10022(string args0)
        {
            return GetMessageString("C10022", args0);
        }

        /// <summary>
        /// 模治具：{0}已在機台：{1}上，不可再上機!!
        /// </summary>
        /// <returns></returns>
        public string C10023(string args0, string args1)
        {
            return GetMessageString("C10023", args0, args1);
        }

        /// <summary>
        /// 模治具儲位為{0}，尚未領用，不可執行上機!!
        /// </summary>
        /// <returns></returns>
        public string C10024(string args0)
        {
            return GetMessageString("C10024", args0);
        }

        /// <summary>
        /// 機台：{0}已停用，如需使用，請至"機台資料維護"啟用!!
        /// </summary>
        /// <returns></returns>
        public string C10025(string args0)
        {
            return GetMessageString("C10025", args0);
        }

        /// <summary>
        /// 機台狀態為{0}，不可執行此功能!!
        /// </summary>
        /// <returns></returns>
        public string C10026(string args0)
        {
            return GetMessageString("C10026", args0);
        }

        /// <summary>
        /// 模具狀態：{0}不存在!!
        /// </summary>
        /// <returns></returns>
        public string C10027(string args0)
        {
            return GetMessageString("C10027", args0);
        }

        /// <summary>
        /// 網頁沒有設定更變狀態!!
        /// </summary>
        /// <returns></returns>
        public string C10028()
        {
            return GetMessageString("C10028");
        }

        /// <summary>
        /// 模治具：{0}不存在，請確認資料正確性!!
        /// </summary>
        /// <returns></returns>
        public string C10029(string args0)
        {
            return GetMessageString("C10029", args0);
        }

        /// <summary>
        /// 模治具：{0}已停用，如需使用，請至"配件資料維護"啟用!!
        /// </summary>
        /// <returns></returns>
        public string C10030(string args0)
        {
            return GetMessageString("C10030", args0);
        }

        /// <summary>
        /// 模治具：{0}不在機台上，不須下機!!
        /// </summary>
        /// <returns></returns>
        public string C10031(string args0)
        {
            return GetMessageString("C10031", args0);
        }

        /// <summary>
        /// 模治具狀態：{0}不存在!!
        /// </summary>
        /// <returns></returns>
        public string C10032(string args0)
        {
            return GetMessageString("C10032", args0);
        }

        /// <summary>
        /// 模具狀態為{0}，不須進行鉗修作業!!
        /// </summary>
        /// <returns></returns>
        public string C10033(string args0)
        {
            return GetMessageString("C10033", args0);
        }

        /// <summary>
        /// 模具狀態為{0}，不可進行降模作業!!
        /// </summary>
        /// <returns></returns>
        public string C10034(string args0)
        {
            return GetMessageString("C10034", args0);
        }

        /// <summary>
        /// 模具儲位為{0}，不在庫房，不可進行降模作業!!
        /// </summary>
        /// <returns></returns>
        public string C10035(string args0)
        {
            return GetMessageString("C10035", args0);
        }

        /// <summary>
        /// 鍛造批號的數量不可小於發料數量
        /// </summary>
        /// <returns></returns>
        public string C00002()
        {
            return GetMessageString("C00002");
        }

        /// <summary>
        /// IQC 完成的總數量不得超過IQC 數量!
        /// </summary>
        /// <returns></returns>
        public string C00003()
        {
            return GetMessageString("C00003");
        }

        /// <summary>
        /// 料號({0})設定為混合，則滿箱數({1})必須為雙數
        /// </summary>
        /// <returns></returns>
        public string C10036(string args0, string args1)
        {
            return GetMessageString("C10036", args0, args1);
        }

        /// <summary>
        /// 料號({0})設定為混合，找不到對應的料號({1})資訊
        /// </summary>
        /// <returns></returns>
        public string C10037(string args0, string args1)
        {
            return GetMessageString("C10037", args0, args1);
        }

        /// <summary>
        /// 料號({0})系統屬性{1}:{2}與料號({3})系統屬性{4}:{5}需相同
        /// </summary>
        /// <returns></returns>
        public string C10038(string args0, string args1, string args2, string args3, string args4, string args5)
        {
            return GetMessageString("C10038", args0, args1, args2, args3, args4, args5);
        }

        /// <summary>
        /// 料號({0})系統屬性{1}:{2}應為{3}
        /// </summary>
        /// <returns></returns>
        public string C10039(string args0, string args1, string args2, string args3)
        {
            return GetMessageString("C10039", args0, args1, args2, args3);
        }

        /// <summary>
        /// 锻造批号({0}):分在两个机加批上，请改刷工单号!
        /// </summary>
        /// <returns></returns>
        public string C10040(string args0)
        {
            return GetMessageString("C10040", args0);
        }

        /// <summary>
        /// 包裝方式為Standard(單手)只允許輸入料號({0})的批號!
        /// </summary>
        /// <returns></returns>
        public string C10041(string args0)
        {
            return GetMessageString("C10041", args0);
        }

        /// <summary>
        /// 包裝方式為Mix(左右手)只允許輸入料號({0},{1})的批號!
        /// </summary>
        /// <returns></returns>
        public string C10042(string args0, string args1)
        {
            return GetMessageString("C10042", args0, args1);
        }

        /// <summary>
        /// 包裝數量({0})必須符合滿箱數量({1})!
        /// </summary>
        /// <returns></returns>
        public string C10043(string args0, string args1)
        {
            return GetMessageString("C10043", args0, args1);
        }

        /// <summary>
        /// 左右手包裝數量需相同:{0}數量({1}),{2}數量({3})!
        /// </summary>
        /// <returns></returns>
        public string C10044(string args0, string args1, string args2, string args3)
        {
            return GetMessageString("C10044", args0, args1, args2, args3);
        }

        /// <summary>
        /// 台車編號：{0} 不存在!!
        /// </summary>
        /// <returns></returns>
        public string C10045(string args0)
        {
            return GetMessageString("C10045", args0);
        }

        /// <summary>
        /// 台車編號：{0} 沒有掛載任何批號!!
        /// </summary>
        /// <returns></returns>
        public string C10046(string args0)
        {
            return GetMessageString("C10046", args0);
        }

        /// <summary>
        /// 工件序號：{0} 不存在!!
        /// </summary>
        /// <returns></returns>
        public string C10047(string args0)
        {
            return GetMessageString("C10047", args0);
        }

        /// <summary>
        /// 鍛造位置：{0} 無任何工件序號
        /// </summary>
        /// <returns></returns>
        public string C10048(string args0)
        {
            return GetMessageString("C10048", args0);
        }

        /// <summary>
        /// 工件已執行過中心孔量測
        /// </summary>
        /// <returns></returns>
        public string C10049()
        {
            return GetMessageString("C10049");
        }

        /// <summary>
        /// 流程找不到待判站
        /// </summary>
        /// <returns></returns>
        public string C10050()
        {
            return GetMessageString("C10050");
        }

        /// <summary>
        /// 台車編號: {0} 不存在!
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C00004(string args0)
        {
            return GetMessageString("C00004", args0);
        }

        /// <summary>
        /// 台車: {0} 尚未啟用!
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C00005(string args0)
        {
            return GetMessageString("C00005", args0);
        }
        /// <summary>
        /// {0}: {1} 狀態不為: {2}，不可使用!
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <param name="args3"></param>
        /// <returns></returns>
        public string C00006(string args0, string args1, string args2)
        {
            return GetMessageString("C00006", args0, args1, args2);
        }

        /// <summary>
        /// 工單：{0}，機台：{1} 已有資料存在，無法進行新增。
        /// </summary>
        /// <returns></returns>
        public string C10051(string args0, string args1)
        {
            return GetMessageString("C10051", args0, args1);
        }

        /// <summary>
        /// 工單：{0}，機台：{1} 未有資料存在，無法進行:{2}。
        /// </summary>
        /// <returns></returns>
        public string C10052(string args0, string args1, string args2)
        {
            return GetMessageString("C10052", args0, args1, args2);
        }

        /// <summary>
        /// 機台:{0}，查無任何檢驗資料。
        /// </summary>
        /// <returns></returns>
        public string C10053(string args0)
        {
            return GetMessageString("C10053", args0);
        }

        /// <summary>
        /// 批號不在第一站
        /// </summary>
        /// <returns></returns>
        public string C10054()
        {
            return GetMessageString("C10054");
        }

        /// <summary>
        /// 批號已上自動線
        /// </summary>
        /// <returns></returns>
        public string C10055()
        {
            return GetMessageString("C10055");
        }

        /// <summary>
        /// 庫存表中不存在此箱號:{0}。
        /// </summary>
        /// <returns></returns>
        public string C10056(string args0)
        {
            return GetMessageString("C10056", args0);
        }

        /// <summary>
        /// 箱號:{0} 重複。
        /// </summary>
        /// <returns></returns>
        public string C10057(string args0)
        {
            return GetMessageString("C10057", args0);
        }

        /// <summary>
        /// 工件序號:{0} 不屬於自動線!
        /// </summary>
        /// <returns></returns>
        public string C10058(string args0)
        {
            return GetMessageString("C10058", args0);
        }

        public string C00007()
        {
            return GetMessageString("C00007");
        }

        public string C00008()
        {
            return GetMessageString("C00008");
        }

        public string C00009()
        {
            return GetMessageString("C00009");
        }

        /// <summary>
        /// 機台{0}找不到檢驗單!
        /// </summary>
        /// <returns></returns>
        public string C10059(string args0)
        {
            return GetMessageString("C10059", args0);
        }

        /// <summary>
        /// 批號{0}找不到子單元!
        /// </summary>
        /// <returns></returns>
        public string C10060(string args0)
        {
            return GetMessageString("C10060", args0);
        }

        /// <summary>
        /// 批號{0}子單元數量大於1!
        /// </summary>
        /// <returns></returns>
        public string C10061(string args0)
        {
            return GetMessageString("C10061", args0);
        }

        /// <summary>
        /// 生產編號:{0} 查無檢驗資料!
        /// </summary>
        /// <returns></returns>
        public string C10062(string args0)
        {
            return GetMessageString("C10062", args0);
        }

        /// <summary>
        /// 生產編號:{0} 沒有機台可以選擇!
        /// </summary>
        /// <returns></returns>
        public string C10063(string args0)
        {
            return GetMessageString("C10063", args0);
        }

        /// <summary>
        /// 生產編號:{0} 沒有序號可以選擇!
        /// </summary>
        /// <returns></returns>
        public string C10064(string args0)
        {
            return GetMessageString("C10064", args0);
        }

        /// <summary>
        /// 序號:{0} 沒有機台檢驗資料可以顯示!
        /// </summary>
        /// <returns></returns>
        public string C10065(string args0)
        {
            return GetMessageString("C10065", args0);
        }

        /// <summary>
        /// 請輸入正確的機台/序號!
        /// </summary>
        /// <returns></returns>
        public string C10066()
        {
            return GetMessageString("C10066");
        }

        /// <summary>
        /// 網頁沒有設定QCType!
        /// </summary>
        /// <returns></returns>
        public string C10067()
        {
            return GetMessageString("C10067");
        }

        /// <summary>
        /// 工作站:{0} 系統屬性設定沒有開啟預約功能!
        /// </summary>
        /// <returns></returns>
        public string C10068(string args0)
        {
            return GetMessageString("C10068", args0);
        }

        /// <summary>
        /// PPK結果選擇OK，檢驗清單不得勾選任何資料！
        /// </summary>
        /// <returns></returns>
        public string C10069()
        {
            return GetMessageString("C10069");
        }

        /// <summary>
        /// PPK結果選擇NG，檢驗清單至少勾選一筆資料！
        /// </summary>
        /// <returns></returns>
        public string C10070()
        {
            return GetMessageString("C10070");
        }
        /// <summary>
        /// 機台上華司物料批號總數不可大於{0}筆資料
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C00010(string args0)
        {
            return GetMessageString("C00010", args0);
        }
        /// <summary>
        /// {0} 已經使用過!
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C00011(string args0)
        {
            return GetMessageString("C00011", args0);
        }

        /// <summary>
        /// 機台:{0} {1}孔沒有上料！
        /// </summary>
        /// <returns></returns>
        public string C10071(string args0, string args1)
        {
            return GetMessageString("C10071", args0, args1);
        }

        /// <summary>
        /// 一個機台、一個位置只能對應一個物料編號
        /// </summary>
        /// <returns></returns>
        public string C00012()
        {
            return GetMessageString("C00012");
        }
        /// <summary>
        /// 機台上不存在華司物料批號!
        /// </summary>
        /// <returns></returns>
        public string C00013()
        {
            return GetMessageString("C00013");
        }

        /// <summary>
        /// 切換使用者權限失敗(使用者:{0})！
        /// </summary>
        /// <returns></returns>
        public string C10072(string args0)
        {
            return GetMessageString("C10072", args0);
        }

        /// <summary>
        /// 來源路徑位置:{0} 不存在！
        /// </summary>
        /// <returns></returns>
        public string C10073(string args0)
        {
            return GetMessageString("C10073", args0);
        }

        /// <summary>
        /// 目標路徑位置:{0} 不存在！
        /// </summary>
        /// <returns></returns>
        public string C10074(string args0)
        {
            return GetMessageString("C10074", args0);
        }

        /// <summary>
        /// 找不到機台:{0}，孔位{1}的華司上機資料！
        /// </summary>
        public string C10075(string args0, string args1)
        {
            return GetMessageString("C10075", args0, args1);
        }
        /// <summary>
        /// 機加批號已經領料完成!
        /// </summary>
        /// <returns></returns>
        public string C00014()
        {
            return GetMessageString("C00014");
        }
        /// <summary>
        /// 華司批號的物料編號與輸入的物料編號不相同 !
        /// </summary>
        /// <returns></returns>
        public string C00015()
        {
            return GetMessageString("C00015");
        }

        /// <summary>
        /// 工件:{0}必須為第一站OP1(目前所在站點為:{1})
        /// </summary>
        public string C10076(string args0, string args1)
        {
            return GetMessageString("C10076", args0, args1);
        }

        /// <summary>
        /// 工件序號:{0} 必須執行中心孔量測，但尚未執行中心孔量測動作！
        /// </summary>
        public string C10077(string args0)
        {
            return GetMessageString("C10077", args0);
        }
        /// <summary>
        /// 來料序號已結批!
        /// </summary>
        /// <returns></returns>
        public string C00016()
        {
            return GetMessageString("C00016");
        }

        /// <summary>
        /// 不良ID的箱號與輸入的箱號不相同!
        /// </summary>
        /// <returns></returns>
        public string C00017()
        {
            return GetMessageString("C00017");
        }

        /// <summary>
        /// 選擇允退時需新增不良清單!
        /// </summary>
        /// <returns></returns>
        public string C00018()
        {
            return GetMessageString("C00018");
        }

        /// <summary>
        /// 中心孔量測檔案:{0} 不存在！
        /// </summary>
        public string C10078(string args0)
        {
            return GetMessageString("C10078", args0);
        }

        /// <summary>
        /// 中心孔量測檔案:{0} 文件行數小於{1}，格式不符，請檢查文件來源！
        /// </summary>
        public string C10079(string args0, string args1)
        {
            return GetMessageString("C10079", args0, args1);
        }

        /// <summary>
        /// 工件序號:{0} 巡檢異常，必須將該工件排出！
        /// </summary>
        public string C10080(string args0)
        {
            return GetMessageString("C10080", args0);
        }

        /// <summary>
        /// 工件序號:{0} 中心孔量測的結果為NG，必須將該工件排出！
        /// </summary>
        public string C10081(string args0)
        {
            return GetMessageString("C10081", args0);
        }

        /// <summary>
        /// 工件序號:{0} 組裝結果為NG，必須將該工件排出！
        /// </summary>
        public string C10082(string args0)
        {
            return GetMessageString("C10082", args0);
        }
        /// <summary>
        /// 依照型號類型:{0}, {1}沒有資料可以顯示!
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C00019(string args0, string args1)
        {
            return GetMessageString("C00019", args0, args1);
        }

        /// <summary>
        /// 人員{0}已上到機台{1}!
        /// </summary>
        public string C10083(string args0, string args1)
        {
            return GetMessageString("C10083", args0, args1);
        }

        /// <summary>
        /// 輸入的箱號倉位與選擇的倉位不同!
        /// </summary>
        /// <returns></returns>
        public string C00020()
        {
            return GetMessageString("C00020");
        }

        /// <summary>
        /// 工件序號({0})所在工作站({1}) 與 ({2}) 不同 !
        /// </summary>
        public string C10084(string args0, string args1, string args2)
        {
            return GetMessageString("C10084", args0, args1, args2);
        }

        /// <summary>
        /// 子單元{0}未執行中心孔量測!
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C10088(string args0)
        {
            return GetMessageString("C10088", args0);
        }

        /// <summary>
        /// 找不到人員({0})於機台({1})的上工資料!
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C10087(string args0)
        {
            return GetMessageString("C10087", args0);
        }

        /// <summary>
        /// 找不到人員({0})於機台({1})的上工資料!
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C10088(string args0, string args1, string args2)
        {
            return GetMessageString("C10088", args0, args1, args2);
        }

        /// <summary>
        /// 找不到機台({0})與批號({1})的機時資料 !
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C10089(string args0, string args1)
        {
            return GetMessageString("C10089", args0, args1);
        }

        /// <summary>
        /// 溫度必須為數字型態!
        /// </summary>
        /// <returns></returns>
        public string C10090()
        {
            return GetMessageString("C10090");
        }

        /// <summary>
        /// 刻字內容:{0} 必須等於16碼!
        /// </summary>
        /// <returns></returns>
        public string C10091(string args0)
        {
            return GetMessageString("C10091", args0);
        }

        /// <summary>
        /// 工件數量({0})超過滿箱數量({1}) !
        /// </summary>
        /// <returns></returns>
        public string C10092(string args0, string args1)
        {
            return GetMessageString("C10092", args0, args1);
        }

        /// <summary>
        /// 工件已暫存請使用暫存工件輸入 !
        /// </summary>
        /// <returns></returns>
        public string C10093()
        {
            return GetMessageString("C10093");
        }

        /// <summary>
        /// 華司批號不可以上同一個機台 !
        /// </summary>
        /// <returns></returns>
        public string C00021()
        {
            return GetMessageString("C00021");
        }


        /// <summary>
        /// 批號({0})數量:{1}大於機台({2})孔位:{3}華司數量:{4} !
        /// </summary>
        /// <returns></returns>
        public string C10094(string args0, string args1, string args2, string args3, string args4)
        {
            return GetMessageString("C10094", args0, args1, args2, args3, args4);
        }

        /// <summary>
        /// 鍛造批號:{0} 已存在，不可再執行入庫!
        /// </summary>
        /// <returns></returns>
        public string C10095(string args0)
        {
            return GetMessageString("C10095", args0);
        }

        /// <summary>
        /// 找不到人員於線別({0})與工位({1})的上工資料 !
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C10096(string args0, string args1)
        {
            return GetMessageString("C10096", args0, args1);
        }

        /// <summary>
        /// 線別({0})與工位({1})已有使用者({2})工單({3})的上工資料 !
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C10097(string args0, string args1, string args2, string args3)
        {
            return GetMessageString("C10097", args0, args1, args2, args3);
        }

        /// <summary>
        /// 找不到人員於線別({0})與工位({1})的開工資料 !
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C10098(string args0, string args1)
        {
            return GetMessageString("C10098", args0, args1);
        }

        /// <summary>
        /// EQP組裝結果回傳: {0}，內容必須為1或2!
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C10099(string args0)
        {
            return GetMessageString("C10099", args0);
        }

        /// <summary>
        /// 序號:{0} 檢驗主檔狀態為{1}，不可以執行!
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C10100(string args0, string args1)
        {
            return GetMessageString("C10100", args0, args1);
        }

        /// <summary>
        /// 找不到機加批號({0})與工單的對應檔案 !
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C10101(string args0)
        {
            return GetMessageString("C10100", args0);
        }

        /// <summary>
        /// 非自動線工單({0})不可使用上自動線 !
        /// </summary>
        /// <param name="args0"></param>
        /// <returns></returns>
        public string C10102(string args0)
        {
            return GetMessageString("C10102", args0);
        }

        /// <summary>
        /// 找不到任何機台，請確認傳入的線別({0})及系統資料設定({1})是否正確 !
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C10103(string args0, string args1)
        {
            return GetMessageString("C10103", args0, args1);
        }

        /// <summary>
        /// 機台({0})已有使用者({1})工單({2})的上工資料 !
        /// </summary>
        /// <param name="args0"></param>
        /// <param name="args1"></param>
        /// <returns></returns>
        public string C10104(string args0, string args1, string args2)
        {
            return GetMessageString("C10103", args0, args1, args2);
        }

        /// <summary>
        /// 調機開始只允許輸入兩筆工單 !
        /// </summary>
        /// <returns></returns>
        public string C10105()
        {
            return GetMessageString("C10105");
        }

        /// <summary>
        /// 機台({0})已有調機資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10106(string arg0)
        {
            return GetMessageString("C10106", arg0);
        }

        /// <summary>
        /// 機台 {0} 尚未調機完成，請先執行調機作業!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00025(string arg0)
        {
            return GetMessageString("C00025", arg0);
        }

        /// <summary>
        /// 找不到首件檢驗的資料，請確認是否已執行調機作業!!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00026()
        {
            return GetMessageString("C00026");
        }

        /// <summary>
        /// 機台{0}，機台狀態{1}，正在進行首件程序，請先確認首件狀態!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00027(string arg0, string arg1)
        {
            return GetMessageString("C00027", arg0, arg1);
        }

        /// <summary>
        /// 無機台狀態{0}的設定資料!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00028(string arg0)
        {
            return GetMessageString("C00028", arg0);
        }

        /// <summary>
        /// 找不到調機開始的工單資料 !
        /// </summary>
        /// <returns></returns>
        public string C10108()
        {
            return GetMessageString("C10108");
        }

        /// <summary>
        /// 程式新增設定傳入的參數{0} : {1} 錯誤，請聯繫IT修正!!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00024(string arg0, string arg1)
        {
            return GetMessageString("C00024", arg0, arg1);
        }

        /// <summary>
        /// 沒有人員上工資料 !
        /// </summary>
        /// <returns></returns>
        public string C10109()
        {
            return GetMessageString("C10109");
        }


        /// <summary>
        /// 檢驗單號:{0} 查無相同的BatchID!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10110(string arg0)
        {
            return GetMessageString("C10110", arg0);
        }

        /// <summary>
        /// 機台:{0} 沒有檢驗資料可以顯示!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10111(string arg0)
        {
            return GetMessageString("C10111", arg0);
        }

        /// <summary>
        /// 檢驗結果主檔筆數({0})與檢驗單明細筆數({1})不相同!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10112(string arg0, string arg1)
        {
            return GetMessageString("C10112", arg0, arg1);
        }

        /// <summary>
        /// 機台({0})料號:({1})未進行({2})檢驗 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public string C10113(string arg0, string arg1, string arg2)
        {
            return GetMessageString("C10113", arg0, arg1, arg2);
        }

        /// <summary>
        /// 批號({0})已進行檢驗中，不在以在做巡檢 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10114(string arg0)
        {
            return GetMessageString("C10114", arg0);
        }

        /// <summary>
        /// 資料已經存在，請直接點選資料進行修改 !
        /// </summary>
        /// <returns></returns>
        public string C10115()
        {
            return GetMessageString("C10115");
        }


        /// <summary>
        /// 料號({0})及機台({1}) 查無任何資訊，不可執行複製動作!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10116(string arg0, string arg1)
        {
            return GetMessageString("C10116", arg0, arg1);
        }

        /// <summary>
        /// 料號({0})及機台({1}) 資料已存在，不可執行複製動作!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10117(string arg0, string arg1)
        {
            return GetMessageString("C10117", arg0, arg1);
        }

        /// <summary>
        /// PPK 檢驗工件未到齊，無法執行 PPK 判定!
        /// </summary>
        /// <returns></returns>
        public string C10118()
        {
            return GetMessageString("C10118");
        }

        /// <summary>
        /// 檔案名稱:{0} 已存在!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10119(string arg0)
        {
            return GetMessageString("C10119", arg0);
        }

        /// <summary>
        /// 供應商:{0} 資料已存在!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10120(string arg0)
        {
            return GetMessageString("C10120", arg0);
        }

        /// <summary>
        /// 刀具類型:{0} 資料已存在!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10121(string arg0)
        {
            return GetMessageString("C10121", arg0);
        }

        /// <summary>
        /// 請新增一筆刀具類型資料!
        /// </summary>
        /// <returns></returns>
        public string C10122()
        {
            return GetMessageString("C10122");
        }

        /// <summary>
        /// 刀具:{0}找不到刀頭資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10123(string arg0)
        {
            return GetMessageString("C10123", arg0);
        }

        /// <summary>
        /// 刀具:{0} 位置:{1} 不可領用 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10124(string arg0, string arg1)
        {
            return GetMessageString("C10124", arg0, arg1);
        }

        /// <summary>
        /// 檔案名稱:{0}已上傳完成 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10125(string arg0)
        {
            return GetMessageString("C10125", arg0);
        }

        /// <summary>
        /// 刀具類型:{0} 必須上傳檢驗報告資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10126(string arg0)
        {
            return GetMessageString("C10126", arg0);
        }

        /// <summary>
        /// 刀具零組件:{0} 不在庫房，不得報廢!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10127(string arg0)
        {
            return GetMessageString("C10127", arg0);
        }

        /// <summary>
        /// 刀具零組件：{0}已停用，如需使用，請至"刀具零組件進料作業"啟用!!
        /// </summary>
        /// <returns></returns>
        public string C10128(string args0)
        {
            return GetMessageString("C10128", args0);
        }

        /// <summary>
        /// 刀具零組件狀態為{0}，不可執行此功能!!
        /// </summary>
        /// <returns></returns>
        public string C10129(string args0)
        {
            return GetMessageString("C10129", args0);
        }

        /// <summary>
        /// 刀具零組件:{0} 無刀面設定資料!!
        /// </summary>
        /// <returns></returns>
        public string C10130(string args0)
        {
            return GetMessageString("C10130", args0);
        }

        /// <summary>
        /// 刀具零組件：{0}未領用、不能上機!
        /// </summary>
        /// <returns></returns>
        public string C10131(string args0)
        {
            return GetMessageString("C10131", args0);
        }

        /// <summary>
        /// 刀具零組件：{0}已在機台：{1}上，不可再上機!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10132(string arg0, string arg1)
        {
            return GetMessageString("C10132", arg0, arg1);
        }

        /// <summary>
        /// 刀具零組件：{0}未有機台安裝，無須執行下機!
        /// </summary>
        /// <returns></returns>
        public string C10133(string args0)
        {
            return GetMessageString("C10133", args0);
        }

        /// <summary>
        /// 刀具零組件：{0}不在機台：{1}上，無法於此機台執行下機!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10134(string arg0, string arg1)
        {
            return GetMessageString("C10134", arg0, arg1);
        }

        /// <summary>
        /// 刀具零組件：{0}已在機台：{1}上，不可拆群組!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10135(string arg0, string arg1)
        {
            return GetMessageString("C10135", arg0, arg1);
        }

        /// <summary>
        /// 刀具零組件已被全部勾選，無法拆群組!
        /// </summary>
        /// <returns></returns>
        public string C10136()
        {
            return GetMessageString("C10136");
        }

        /// <summary>
        /// 工件編號:{0} 於MES_WIP_COMP查無對應資料!
        /// </summary>
        /// <returns></returns>
        public string C10137(string args0)
        {
            return GetMessageString("C10137", args0);
        }

        /// <summary>
        /// 刀具零組件：{0}已在機台：{1}上，不可執行刀具零組件歸還!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10138(string arg0, string arg1)
        {
            return GetMessageString("C10138", arg0, arg1);
        }

        /// <summary>
        /// 於(BPM_MES_EMPSHIFT)找不到人員班別資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10139()
        {
            return GetMessageString("C10139");
        }

        /// <summary>
        /// 工作站{0}不包含此機台{1}，請確認工作站機台設定!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00029(string arg0, string arg1)
        {
            return GetMessageString("C00029", arg0, arg1);
        }

        /// <summary>
        /// 找不到機台({0})與料號({1})的刀具零組件設定資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10140(string arg0, string arg1)
        {
            return GetMessageString("C10140", arg0, arg1);
        }

        /// <summary>
        /// 機台({0})與料號({1})的刀具類別({2})領用數量必須為:{3} !
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        public string C10141(string arg0, string arg1, string arg2, string arg3)
        {
            return GetMessageString("C10141", arg0, arg1, arg2, arg3);
        }

        /// <summary>
        /// 找不到刀具驗證資訊 !
        /// </summary>
        /// <returns></returns>
        public string C10142()
        {
            return GetMessageString("C10142");
        }

        /// <summary>
        /// 工件{0}不屬於Runcard {1}，請確認!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        public string C00030(string arg0, string arg1)
        {
            return GetMessageString("C00030", arg0, arg1);
        }

        /// <summary>
        /// 除外人員({0})已經有除外資料請先進行除外結束 !
        /// </summary>
        /// <returns></returns>
        public string C10143(string arg1)
        {
            return GetMessageString("C10143", arg1);
        }

        /// <summary>
        /// 除外人員({0})沒有除外開始資料 !
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10144(string arg1)
        {
            return GetMessageString("C10144", arg1);
        }

        /// <summary>
        /// 機台:{0} 沒有華司物料編號{1}的資料！
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10145(string arg0, string arg1)
        {
            return GetMessageString("C10145", arg0, arg1);
        }

        /// <summary>
        /// 工件編號({0}) [PROCESS_EQUIP]沒有資料 !
        /// </summary>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10146(string arg1)
        {
            return GetMessageString("C10146", arg1);
        }

        /// <summary>
        /// 機台{0}目前狀態為停機{1}，請先確認是否調機完成!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C00031(string arg0, string arg1)
        {
            return GetMessageString("C00031", arg0, arg1);
        }

        /// <summary>
        /// 機台{0}目前狀態為待送首件{1}，請先完成首件程序!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C00032(string arg0, string arg1)
        {
            return GetMessageString("C00032", arg0, arg1);
        }

        /// <summary>
        /// 機台{0}目前狀態為首件待接收{1}，請先完成首件程序!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C00033(string arg0, string arg1)
        {
            return GetMessageString("C00033", arg0, arg1);
        }

        /// <summary>
        /// 機台{0}目前狀態為待執行首件{1}，請先完成首件程序!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C00034(string arg0, string arg1)
        {
            return GetMessageString("C00034", arg0, arg1);
        }

        /// <summary>
        /// 機台{0}目前狀態為首件中{1}，請先完成首件程序!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C00035(string arg0, string arg1)
        {
            return GetMessageString("C00035", arg0, arg1);
        }

        /// <summary>
        /// 機台{0}目前狀態為{1}，請先完成穩定度測試!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C00036(string arg0, string arg1)
        {
            return GetMessageString("C00036", arg0, arg1);
        }

        /// <summary>
        /// 機台編號:{0} 沒有刀具上機資料!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10147(string arg0)
        {
            return GetMessageString("C10147", arg0);
        }

        /// <summary>
        /// 刀具零組件:{0} 不在上機清單內!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10148(string arg0)
        {
            return GetMessageString("C10148", arg0);
        }

        /// <summary>
        /// 刀具零組件狀態: {0}不存在，請至配件狀態維護新增此狀態!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10149(string arg0)
        {
            return GetMessageString("C10149", arg0);
        }

        /// <summary>
        /// 格式必須為正整數，且使用逗號隔開(例如:1,2,3)!!
        /// </summary>
        /// <returns></returns>
        public string C10150()
        {
            return GetMessageString("C10150");
        }

        /// <summary>
        /// 刀具類型:{0}，刀面數量({1})設定有誤，請至刀具類型維護修改此數量!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10151(string arg0, string arg1)
        {
            return GetMessageString("C10151", arg0, arg1);
        }

        /// <summary>
        /// 刀具零組件：{0} 已在機台({1})上，請先執行刀具下機!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10152(string arg0, string arg1)
        {
            return GetMessageString("C10152", arg0, arg1);
        }

        /// <summary>
        /// 刀具零組件：{0} 只有一個刀面，不能更換刀面!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10153(string arg0)
        {
            return GetMessageString("C10153", arg0);
        }

        /// <summary>
        /// 此機台+料號已有設定資料，如要重新設定，請先刪除再新增設定資料!!
        /// </summary>
        /// <returns></returns>
        public string C10154()
        {
            return GetMessageString("C10154");
        }

        /// <summary>
        /// 找不到目前班別日期請確認系統資料設定類別(Shift)是否正確 !
        /// </summary>
        /// <returns></returns>
        public string C10155()
        {
            return GetMessageString("C10155");
        }

        /// <summary>
        /// {0}班人員不可上{1}班 !
        /// </summary>
        /// <returns></returns>
        public string C10156(string arg0, string arg1)
        {
            return GetMessageString("C10156");
        }

        /// <summary>
        /// 找不到人員({0})與批號({1})的開工資料 !
        /// </summary>
        /// <returns></returns>
        public string C10157(string arg0, string arg1)
        {
            return GetMessageString("C10157");
        }

        /// <summary>
        /// 刀壽設定分頁中，請新增一筆資料!
        /// </summary>
        /// <returns></returns>
        public string C10158()
        {
            return GetMessageString("C10158");
        }

        /// <summary>
        /// 預定回廠日必須大於等於{0} !
        /// </summary>
        /// <returns></returns>
        public string C10159(string arg0)
        {
            return GetMessageString("C10159", arg0);
        }

        /// <summary>
        /// 批號({0})仍未換班 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10160(string arg0)
        {
            return GetMessageString("C10160", arg0);
        }

        public string C10161(string arg0, string arg1)
        {
            return GetMessageString("C10161", arg0, arg1);
        }

        /// <summary>
        /// 最新一筆Component資料({0})，CPFSN為空白，無法正常取得CPF_SN!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10162(string arg0)
        {
            return GetMessageString("C10162", arg0);
        }

        /// <summary>
        /// 最新一筆Component資料({0})，CPFN({1})碼長不為四碼，無法正常取得CPF_SN!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10163(string arg0, string arg1)
        {
            return GetMessageString("C10163", arg0, arg1);
        }

        /// <summary>
        /// 最新一筆Component資料({0})，CPFN({1})前二碼不為數字，無法正常取得CPF_SN!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10164(string arg0, string arg1)
        {
            return GetMessageString("C10164", arg0, arg1);
        }

        /// <summary>
        /// [CST_CMS_CAR_PROCESS]查無台車可使用!
        /// </summary>
        /// <returns></returns>
        public string C10165()
        {
            return GetMessageString("C10165");
        }

        /// <summary>
        /// 請使用工單領料與下線規則 !
        /// </summary>
        /// <returns></returns>
        public string C10166()
        {
            return GetMessageString("C10166");
        }

        /// <summary>
        /// 請使用重工工單領料與下線規則 !
        /// </summary>
        /// <returns></returns>
        public string C10167()
        {
            return GetMessageString("C10167");
        }

        /// <summary>
        /// 找不到小工單號({0})與工單的對應檔案 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00037(string arg0)
        {
            return GetMessageString("C00037", arg0);
        }

        /// <summary>
        /// 找不到檢驗資料!!
        /// </summary>
        /// <returns></returns>
        public string C00038()
        {
            return GetMessageString("C00038");
        }

        /// <summary>
        /// 工單批號 {0} 所屬工件，中心孔已全部量測完畢!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00039(string arg0)
        {
            return GetMessageString("C00039", arg0);
        }


        /// <summary>
        /// 型號名稱與暫包時候不同 !
        /// </summary>
        /// <returns></returns>
        public string C10168()
        {
            return GetMessageString("C10168");
        }

        /// <summary>
        /// 刀具數量必須等於1，才可執行上傳檢驗報 !
        /// </summary>
        /// <returns></returns>
        public string C10169()
        {
            return GetMessageString("C10169");
        }

        /// <summary>
        /// 工件批號 {0} 有DMC資訊未收集，請執行相關功能，收集DMC資訊!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00040(string arg0)
        {
            return GetMessageString("C00040", arg0);
        }

        /// <summary>
        /// 變更回廠日必須大於等於{0} !
        /// </summary>
        /// <returns></returns>
        public string C10170(string arg0)
        {
            return GetMessageString("C10170", arg0);
        }

        /// <summary>
        /// 機台({0})與料號({1})未進行刀具驗證 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10171(string arg0, string arg1)
        {
            return GetMessageString("C10171", arg0, arg1);
        }

        /// <summary>
        /// 工號{4} : 上工日期({0})班別({1})與目前日期({2})班別({3})，請重新上工 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <returns></returns>
        public string C10172(string arg0, string arg1, string arg2, string arg3, string arg4)
        {
            return GetMessageString("C10172", arg0, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// 機台{0}，{1}孔沒有華司物料批資訊，請確認!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C00041(string arg0, string arg1)
        {
            return GetMessageString("C00041", arg0, arg1);
        }

        /// <summary>
        /// 工作站{0}不需收集中心孔或DMC資訊，請確認!!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00042(string arg0)
        {
            return GetMessageString("C00042", arg0);
        }

        /// <summary>
        /// 料號{0}該產品不需收集中心孔資訊，請確認!!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00043(string arg0)
        {
            return GetMessageString("C00043", arg0);
        }

        /// <summary>
        /// 找不到機台({0})與工單({1})的機時資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10173(string arg0, string arg1)
        {
            return GetMessageString("C10173", arg0, arg1);
        }

        /// <summary>
        /// 刀具{0}已報廢，不可領用!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00044(string arg0)
        {
            return GetMessageString("C00044", arg0);
        }

        /// <summary>
        /// 刀具{0}送修未回廠，不可領用!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00045(string arg0)
        {
            return GetMessageString("C00045", arg0);
        }

        /// <summary>
        /// 同入庫批({0})的爐號必須相同!(LotID:{1};爐號:{2}；Create的爐號為:{3})
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        public string C10174(string arg0, string arg1, string arg2, string arg3)
        {
            return GetMessageString("C10174", arg0, arg1, arg2, arg3);
        }

        /// <summary>
        /// 找不到刀具{0}送修紀錄，請確認!!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00046(string arg0)
        {
            return GetMessageString("C00046", arg0);
        }

        /// <summary>
        /// 請選擇一種查詢條件!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00047()
        {
            return GetMessageString("C00047");
        }

        /// <summary>
        /// 工件序號{0}已在待判站，不需再送待判 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10175(string arg0)
        {
            return GetMessageString("C10175", arg0);
        }

        /// <summary>
        /// 找不到批號({0})的開工資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10176(string arg0)
        {
            return GetMessageString("C10176", arg0);
        }

        /// <summary>
        /// 小工單({0})及鍛造/入庫批號({1}) 查無資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10177(string arg0, string arg1)
        {
            return GetMessageString("C10177", arg0, arg1);
        }

        /// <summary>
        /// 來料批號({0}) 查無資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10178(string arg0)
        {
            return GetMessageString("C10178", arg0);
        }

        /// <summary>
        /// 來料批號({0})對應爐號({1}) 查無資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public string C10179(string arg0, string arg1)
        {
            return GetMessageString("C10179", arg0, arg1);
        }

        /// <summary>
        /// 鍛造/入庫批號:{0} 查無對應的工件資料 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10180(string arg0)
        {
            return GetMessageString("C10180", arg0);
        }

        /// <summary>
        /// 人員({0})已開始除外不可以上工 !
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C10181(string arg0)
        {
            return GetMessageString("C10181", arg0);
        }

        /// <summary>
        /// 僅接受格式為.jpg /.jpeg/.png/.bmp的檔案！
        /// </summary>
        /// <returns></returns>
        public string C10182()
        {
            return GetMessageString("C10182");
        }

        /// <summary>
        /// PLC工件點位為空值！
        /// </summary>
        /// <returns></returns>
        public string C10183()
        {
            return GetMessageString("C10183");
        }

        /// <summary>
        /// 查無IQC來料資訊(CST_MMS_IQC_INFO無資料)！
        /// </summary>
        /// <returns></returns>
        public string C10184()
        {
            return GetMessageString("C10184");
        }


        /// <summary>
        /// 批號{0}不在裝配站，請確認工作流程，或至系統資料設定增加分類 SAIAssemblyOperation 的設定!!!
        /// </summary>
        /// <param name="arg0"></param>
        /// <returns></returns>
        public string C00048(string arg0)
        {
            return GetMessageString("C00048", arg0);
        }

        /// <summary>
        /// 機台:{0} 己經上了{1}個物料序號，不可再執行上料動作！
        /// </summary>
        /// <returns></returns>
        public string C10185(string arg0, string arg1)
        {
            return GetMessageString("C10185", arg0, arg1);
        }

        /// <summary>
        /// 機台:{0} 已超過設定工單數量{1}！
        /// </summary>
        /// <returns></returns>
        public string C10186(string arg0, string arg1)
        {
            return GetMessageString("C10186", arg0, arg1);
        }

        /// <summary>
        /// 查無送待判原因！
        /// </summary>
        /// <returns></returns>
        public string C10187()
        {
            return GetMessageString("C10187");
        }

        /// <summary>
        /// DMC {0} 未落入系統，請改選擇工作站!!
        /// </summary>
        /// <returns></returns>
        public string C00049(string arg0)
        {
            return GetMessageString("C00049", arg0);
        }

        /// <summary>
        /// 工單號{0}在站點 {1} 沒有批號可送待判!!
        /// </summary>
        /// <returns></returns>
        public string C00050(string arg0, string arg1)
        {
            return GetMessageString("C00050", arg0, arg1);
        }

        /// <summary>
        /// 查無鍛造品檢預設的檢驗類型 !
        /// </summary>
        /// <returns></returns>
        public string C10188()
        {
            return GetMessageString("C10188");
        }

        /// <summary>
        /// 找不到原因碼設定資料，原因碼{0}!!
        /// </summary>
        /// <returns></returns>
        public string C00051(string arg0)
        {
            return GetMessageString("C00051", arg0);
        }

        /// <summary>
        /// 批號{0} 狀態必須為WaitMerge才可執行此功能！
        /// </summary>
        /// <returns></returns>
        public string C10189(string arg0)
        {
            return GetMessageString("C10189", arg0);
        }

        /// <summary>
        /// DMC{0}已收集過，請勿重複輸入!!
        /// </summary>
        /// <returns></returns>
        public string C00052(string arg0)
        {
            return GetMessageString("C00052", arg0);
        }

        /// <summary>
        /// 原因碼分類 {0}，未設定原因碼 {1}，請至原因碼管理添加!!
        /// </summary>
        /// <returns></returns>
        public string C00053(string arg0, string arg1)
        {
            return GetMessageString("C00053", arg0, arg1);
        }

        /// <summary>
        /// 工件{0} 找不到暫存的資料，請確認!!
        /// </summary>
        /// <returns></returns>
        public string C00054(string arg0)
        {
            return GetMessageString("C00054", arg0);
        }

        /// <summary>
        /// 機台:{0} 查無出站記錄！
        /// </summary>
        /// <returns></returns>
        public string C10190(string arg0)
        {
            return GetMessageString("C10190", arg0);
        }
    }
}
