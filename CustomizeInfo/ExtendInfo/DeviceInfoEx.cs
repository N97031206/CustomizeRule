using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizeRule.CustomizeInfo.ExtendInfo
{
    public partial class DeviceInfoEx : DeviceInfo
    {
        //public static DeviceInfoEx GetDataByDeviceName(string deviceName)
        //{
        //    string sSQL = @"SELECT * FROM MES_PRC_DEVICE_VER WHERE DEVICE = #[STRING] AND REVSTATE = 'ACTIVE'";

        //    return InfoCenter.GetBySQL<DeviceInfoEx>(sSQL, deviceName);
        //}

        //public string ProdType
        //{
        //    get
        //    {
        //        return this["PRODTYPE"].ToCimesString(string.Empty);
        //    }
        //    set
        //    {
        //        this["PRODTYPE"] = value;
        //    }
        //}
    }
}
