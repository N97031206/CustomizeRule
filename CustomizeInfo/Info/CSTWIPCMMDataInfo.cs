using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWIPCMMDataInfo
    {
        public static List<CSTWIPCMMDataInfo> GetDataByWIPCMMSID(string WIPCMMSID)
        {
            string sSQL = @" SELECT * FROM CST_WIP_CMM_DATA
                                WHERE WIP_CMM_SID = #[STRING]";

            return InfoCenter.GetList<CSTWIPCMMDataInfo>(sSQL, WIPCMMSID);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
