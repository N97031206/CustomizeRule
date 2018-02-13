using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;

namespace CustomizeRule.CustomizeInfo.ExtendInfo
{
    public class WIPConvertInfoEx : WIPConvertInfo
    {
        public static WIPConvertInfoEx GetWIPConvertInfoByTargetLot(string targetLot)
        {
            return InfoCenter.GetBySQL<WIPConvertInfoEx>("SELECT * FROM MES_WIP_CONVERT WHERE TARGETLOT = #[STRING] AND CANCELFLAG ='N'", targetLot);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
