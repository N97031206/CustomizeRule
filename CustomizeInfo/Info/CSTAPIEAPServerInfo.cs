using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTAPIEAPServerInfo
    {
        public static CSTAPIEAPServerInfo GetDataByEAPName(string EAPName)
        {
            string sSQL = @" SELECT * FROM CST_API_EAP_SERVER 
                                WHERE EAP_NAME = #[STRING]";

            return InfoCenter.GetBySQL<CSTAPIEAPServerInfo>(sSQL, EAPName);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

        public static CSTAPIEAPServerInfo GetDataByEAPNameAndLine(string EAPName, string line)
        {
            string sSQL = @" SELECT * FROM CST_API_EAP_SERVER 
                                WHERE EAP_NAME = #[STRING] AND LINE = #[STRING]";

            return InfoCenter.GetBySQL<CSTAPIEAPServerInfo>(sSQL, EAPName, line);
        }
    }
}
