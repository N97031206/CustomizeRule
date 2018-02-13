using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWorkOrderLotInfo
    {
        public static CSTWorkOrderLotInfo GetWorkOrderLotDataByWorkOrderLot(string workOrderLot)
        {
            string sSQL = @" SELECT * FROM CST_WPC_WO_LOT WHERE WOLOT = #[STRING] ";

            return InfoCenter.GetBySQL<CSTWorkOrderLotInfo>(sSQL, workOrderLot);
        }

        public static CSTWorkOrderLotInfo GetWorkOrderLotDataByWOAndWOSequence(string workOrder, decimal workOrderSequence)
        {
            string sSQL = @" SELECT * FROM CST_WPC_WO_LOT WHERE WO = #[STRING] AND WOSEQUENCE = #[DECIMAL]";

            return InfoCenter.GetBySQL<CSTWorkOrderLotInfo>(sSQL, workOrder, workOrderSequence);
        }

        public static List<CSTWorkOrderLotInfo> GetWorkOrderLotDataByWorkOrder(string workOrder)
        {
            string sSQL = @" SELECT * FROM CST_WPC_WO_LOT WHERE WO = #[STRING] ";

            return InfoCenter.GetList<CSTWorkOrderLotInfo>(sSQL, workOrder);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

        public static CSTWorkOrderLotInfo GetWorkOrderLotDataByWOLotAndMLotOrInvLot(string workOrderLot, string MLotOrInvLot)
        {
            string sSQL = @" SELECT *
                              FROM CST_WPC_WO_LOT
                             WHERE     WOLOT = #[STRING]
                                   AND (INVLOT = #[STRING] OR MATERIALLOT = #[STRING])";

            return InfoCenter.GetBySQL<CSTWorkOrderLotInfo>(sSQL, workOrderLot, MLotOrInvLot, MLotOrInvLot);
        }
    }
}
