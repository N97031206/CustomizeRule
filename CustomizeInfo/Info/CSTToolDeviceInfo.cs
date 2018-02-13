using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTToolDeviceInfo
    {
        public static List<CSTToolDeviceInfo> GetDataListByDeviceAndEquipmantName(string deviceName, string equipmentName)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_DEVICE 
                                WHERE DEVICE = #[STRING] AND EQP = #[STRING]";

            return InfoCenter.GetList<CSTToolDeviceInfo>(sSQL, deviceName, equipmentName);
        }

        public static List<CSTToolDeviceInfo> GetDataListByDeviceName(string deviceName)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_DEVICE 
                                WHERE DEVICE = #[STRING]";

            return InfoCenter.GetList<CSTToolDeviceInfo>(sSQL, deviceName);
        }

        public static List<CSTToolDeviceInfo> GetDataListByEquipmentName(string equipmentName)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_DEVICE 
                                WHERE EQP = #[STRING]";

            return InfoCenter.GetList<CSTToolDeviceInfo>(sSQL, equipmentName);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
