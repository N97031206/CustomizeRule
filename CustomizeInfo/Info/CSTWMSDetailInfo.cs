using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWMSDetailInfo
    {
        public static List<CSTWMSDetailInfo> GetMaterialLotDetailDataByMaterialLot(string materialLot)
        {
            string sSQL = @" SELECT * FROM CST_WMS_DETAIL WHERE LOT = #[STRING] ";

            return InfoCenter.GetList<CSTWMSDetailInfo>(sSQL, materialLot);
        }

        public static CSTWMSDetailInfo GetMaterialLotDetailDataBySN(string componentID)
        {
            string sSQL = @" SELECT * FROM CST_WMS_DETAIL WHERE COMPONENTID = #[STRING] ";

            return InfoCenter.GetBySQL<CSTWMSDetailInfo>(sSQL, componentID);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
