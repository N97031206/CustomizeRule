using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
   public partial class CSTToolDeviceDetailInfo
    {
        public static List<CSTToolDeviceDetailInfo> GetDataListByDeviceAndEquipmantName(string deviceName, string equipmentName)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_DEVICE_DETAIL 
                                WHERE DEVICE = #[STRING] AND EQP = #[STRING]";

            return InfoCenter.GetList<CSTToolDeviceDetailInfo>(sSQL, deviceName, equipmentName);
        }

        public static List<CSTToolDeviceDetailInfo> GetDataListByToolDeviceSID(string toolDeviceSID)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_DEVICE_DETAIL 
                                WHERE TOOL_DEVICE_SID = #[STRING]";

            return InfoCenter.GetList<CSTToolDeviceDetailInfo>(sSQL, toolDeviceSID);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/


        public static CSTToolDeviceDetailInfo GetDataListByDeviceAndEquipmantNameAndToolType(string deviceName, string equipmentName, string toolType)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_DEVICE_DETAIL 
                                WHERE DEVICE = #[STRING] AND EQP = #[STRING] AND TOOLTYPE = #[STRING]";

            return InfoCenter.GetBySQL<CSTToolDeviceDetailInfo>(sSQL, deviceName, equipmentName, toolType);
        }

    }
}
