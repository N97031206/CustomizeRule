using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    [Serializable]
    public partial class EquipmentMaterialLotInfoEx : EquipmentMaterialLotInfo
    {
        public static List<EquipmentMaterialLotInfoEx> GetEquipmentMaterialLotByMaterialLot(string materialLot)
        {
            string sql = @"SELECT * FROM MES_EQP_MLOT 
                                WHERE MLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, materialLot);

            return InfoCenter.GetList<EquipmentMaterialLotInfoEx>(sa);
        }

        public static List<EquipmentMaterialLotInfoEx> GetEquipmentMaterialLotByEquipmentAndLocation(string equipment, string location)
        {
            return InfoCenter.GetList<EquipmentMaterialLotInfoEx>(@"SELECT * FROM MES_EQP_MLOT 
                        INNER JOIN MES_MMS_MLOT ON(MES_EQP_MLOT.MLOT = MES_MMS_MLOT.MLOT)
                        WHERE MES_EQP_MLOT.EQUIPMENT = #[STRING] AND MES_MMS_MLOT.LOCATION = #[STRING] ORDER BY EQP_MLOT_SID", equipment, location);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
