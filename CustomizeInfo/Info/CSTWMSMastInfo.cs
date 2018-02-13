using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWMSMastInfo
    {
        public static CSTWMSMastInfo GetMaterialLotDataByMaterialLot(string materialLot)
        {
            string sSQL = @" SELECT * FROM CST_WMS_MAST WHERE LOT = #[STRING] ";

            return InfoCenter.GetBySQL<CSTWMSMastInfo>(sSQL, materialLot);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
