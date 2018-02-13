using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWMSMastBAKInfo
    {
        //public partial class CSTWMSMastLogInfo
        //{
        //    public static CSTWorkOrderLotInfo GetWorkOrderLotDataByWorkOrderLot(string workOrderLot)
        //    {
        //        string sSQL = @" SELECT * FROM CST_WPC_WO_LOT WHERE WOLOT = #[STRING] ";

        //        return InfoCenter.GetBySQL<CSTWorkOrderLotInfo>(sSQL, workOrderLot);
        //    }


        //}
        public static CSTWMSMastBAKInfo GetDataByLot(string lot)
        {
            string sSQL = @"SELECT * FROM CST_WMS_MAST_BAK WHERE LOT = #[STRING] ";

            return InfoCenter.GetBySQL<CSTWMSMastBAKInfo>(sSQL, lot);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/


    }
}
