using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTEQPWOAssignInfo
    {
        /// <summary>
        /// 根據工單及機台查詢資料
        /// </summary>
        /// <param name="workOrder">工單</param>
        /// <param name="equipmentName">機台</param>
        /// <returns></returns>
        public static CSTEQPWOAssignInfo GetDataByWorkOrderAndEquipmentName(string workOrder, string equipmentName)
        {
            string sSQL = @" SELECT * FROM CST_EQP_WO_ASSIGN WHERE WO = #[STRING] AND EQP = #[STRING] ";

            return InfoCenter.GetBySQL<CSTEQPWOAssignInfo>(sSQL, workOrder, equipmentName);
        }

        /// <summary>
        /// 根據工單查詢資料
        /// </summary>
        /// <param name="workOrder"></param>
        /// <returns></returns>
        public static CSTEQPWOAssignInfo GetDataByWorkOrder(string workOrder)
        {
            string sSQL = @" SELECT * FROM CST_EQP_WO_ASSIGN WHERE WO = #[STRING]";

            return InfoCenter.GetBySQL<CSTEQPWOAssignInfo>(sSQL, workOrder);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

        /// <summary>
        /// 根據機台查詢相關資料
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <returns></returns>
        public static List<CSTEQPWOAssignInfo> GetDataByEquipmentName(string equipmentName)
        {
            string sSQL = @" SELECT * FROM CST_EQP_WO_ASSIGN WHERE EQP = #[STRING]";

            return InfoCenter.GetList<CSTEQPWOAssignInfo>(sSQL, equipmentName);
        }

    }
}
