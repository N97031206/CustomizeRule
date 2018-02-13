using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizeRule.CustomizeInfo.ExtendInfo
{
    public partial class EquipGroupEquipInfoEx : EquipGroupEquipInfo
    {
        public static List<EquipGroupEquipInfoEx> GetEquipGroupByGroupSID(string groupSID)
        {
            string sSQL = @"SELECT * FROM MES_EQP_GROUP_EQP 
                        WHERE EQP_GROUP_SID = #[STRING]";

            return InfoCenter.GetList<EquipGroupEquipInfoEx>(sSQL, groupSID);
        }

        public static EquipGroupEquipInfoEx GetEquipGroupByEquipmentName(string equipmentName)
        {
            string sSQL = @"SELECT * FROM MES_EQP_GROUP_EQP 
                        WHERE EQUIPMENT = #[STRING]";

            return InfoCenter.GetBySQL<EquipGroupEquipInfoEx>(sSQL, equipmentName);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
