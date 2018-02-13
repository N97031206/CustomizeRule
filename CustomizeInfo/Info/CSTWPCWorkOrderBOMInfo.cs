using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWPCWorkOrderBOMInfo
    {
        /// <summary>
        /// 依據工單，取得ERP工單BOM
        /// </summary>
        /// <param name="workOrder">工單</param>
        /// <returns></returns>
        public static List<CSTWPCWorkOrderBOMInfo> GetDataByWorkOrder(string workOrder)
        {
            string sSQL = @" SELECT * FROM CST_WPC_WO_BOM WHERE AUFNR = #[STRING] ";

            return InfoCenter.GetList<CSTWPCWorkOrderBOMInfo>(sSQL, workOrder);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

        /// <summary>
        /// 依據工單及料別種類，取得ERP工單BOM
        /// </summary>
        /// <param name="workOrder">工單</param>
        /// <returns></returns>
        public static List<CSTWPCWorkOrderBOMInfo> GetDataByWorkOrderAndSORTF(string workOrder, string SORTF)
        {
            string sSQL = @" SELECT * FROM CST_WPC_WO_BOM WHERE AUFNR = #[STRING] AND SORTF = #[STRING]";

            return InfoCenter.GetList<CSTWPCWorkOrderBOMInfo>(sSQL, workOrder, SORTF);
        }
    }
}
