using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizeRule
{
    public partial class RuleMessage
    {
        //客製訊息請從C10001編號開始
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
        /// 序號：{0}所屬批號為{0}，請確認資料正確性!
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
    }
}
