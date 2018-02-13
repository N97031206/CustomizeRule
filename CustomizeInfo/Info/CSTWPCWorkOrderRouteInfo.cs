using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWPCWorkOrderRouteInfo
    {
        /// <summary>
        /// 依據工單，取得ERP工單流程
        /// </summary>
        /// <param name="workOrder">工單</param>
        /// <returns></returns>
        public static List<CSTWPCWorkOrderRouteInfo> GetDataByWorkOrder(string workOrder)
        {
            string sSQL = @" SELECT * FROM CST_WPC_WO_ROUTE WHERE AUFNR = #[STRING] ";

            return InfoCenter.GetList<CSTWPCWorkOrderRouteInfo>(sSQL, workOrder);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
