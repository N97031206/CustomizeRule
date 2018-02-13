using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWipPPKInfo
    {
        public static CSTWipPPKInfo GetPPKDataByEqpAndDevice(string equipmentName, string deviceName)
        {
            string sSQL = @" SELECT * FROM CST_WIP_PPK WHERE EQP = #[STRING] AND DEVICE = #[STRING] ";

            return InfoCenter.GetBySQL<CSTWipPPKInfo>(sSQL, equipmentName, deviceName);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
