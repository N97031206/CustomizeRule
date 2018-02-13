using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWipAssemblyFileInfo
    {
        /// <summary>
        /// 是否已比對到Component
        /// </summary>
        public bool Flag { get; set; }

        public static List<CSTWipAssemblyFileInfo> GetAssemblyFile(string equipment, string location)
        {
            string sSQL = @" SELECT * FROM CST_WIP_ASSEMBLY_FILE 
                                WHERE EQUIPMENT = #[STRING] 
                                    AND LOCATION = #[STRING] ";

            return InfoCenter.GetList<CSTWipAssemblyFileInfo>(sSQL, equipment, location);
        }

        public static List<CSTWipAssemblyFileInfo> GetAssemblyFileByEquipmentName(string equipment)
        {
            string sSQL = @" SELECT * FROM CST_WIP_ASSEMBLY_FILE WHERE EQUIPMENT = #[STRING] ORDER BY FILETIME";
            return InfoCenter.GetList<CSTWipAssemblyFileInfo>(sSQL, equipment);
        }

        public static List<CSTWipAssemblyFileInfo> GetAssemblyFileByEquipmentNameAndInvLot(string equipment, string invLot)
        {
            string sSQL = @" SELECT * FROM CST_WIP_ASSEMBLY_FILE WHERE EQUIPMENT = #[STRING] AND INVLOT = #[STRING] ORDER BY FILETIME";
            return InfoCenter.GetList<CSTWipAssemblyFileInfo>(sSQL, equipment, invLot);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
