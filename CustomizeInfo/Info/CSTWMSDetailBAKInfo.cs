using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWMSDetailBAKInfo
    {
        public static List<CSTWMSDetailBAKInfo> GetListDataByLot(string lot)
        {
            string sSQL = @"SELECT * FROM CST_WMS_DETAIL_BAK WHERE LOT =#[STRING] ";

            return InfoCenter.GetList<CSTWMSDetailBAKInfo>(sSQL, lot);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
