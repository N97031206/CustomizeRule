using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ares.Cimes.IntelliService.DbTableSchema;

namespace Ares.Cimes.IntelliService.Info
{
    [Serializable]
    public partial class EquipToolInfoEx : EquipToolInfo
    {
        static EquipToolInfoEx()
        {
            MES_EQP_TOOL.Singleton.SyncColumnsFromDB();
        }
        
        public string ToolType
        {
            get
            {
                return (this["TOOLTYPE"] is DBNull) ? string.Empty : (String)this["TOOLTYPE"];
            }
            set
            {
                this["TOOLTYPE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        public static List<EquipToolInfoEx> GetEquipmetToolListByEquipmentName(string equipment)
        {
            return InfoCenter.GetList<EquipToolInfoEx>(@"SELECT MES_TOOL_MAST.TOOLTYPE, MES_EQP_TOOL.* FROM MES_EQP_TOOL 
                INNER JOIN MES_TOOL_MAST ON(MES_EQP_TOOL.TOOLNAME = MES_TOOL_MAST.TOOLNAME)
                WHERE MES_EQP_TOOL.EQUIPMENT = #[STRING]", equipment);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}