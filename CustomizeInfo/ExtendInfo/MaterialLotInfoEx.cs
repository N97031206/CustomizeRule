using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizeRule.CustomizeInfo.ExtendInfo
{
    public partial class MaterialLotInfoEx : MaterialLotInfo
    {
        public static MaterialLotInfoEx GetDataByMaterialNo(string materialNo)
        {
            string sql = @"SELECT * FROM MES_MMS_MLOT
                            WHERE MATERIALNO = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, materialNo);

            return InfoCenter.GetBySQL<MaterialLotInfoEx>(sa);
        }

        public static List<MaterialLotInfoEx> GetMaterialLotByUserDefineCol02(string udc02)
        {
            string sql = @"SELECT * FROM MES_MMS_MLOT
                            WHERE USERDEFINECOL02 = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, udc02);

            return InfoCenter.GetList<MaterialLotInfoEx>(sa);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
