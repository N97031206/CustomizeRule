using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTAPIMoldResetLogInfo
    {
        /// <summary>
        /// 取得模具重置資料
        /// </summary>
        /// <param name="MoldSN"></param>
        /// <returns></returns>
        public static CSTAPIMoldResetLogInfo GetMoldLog(string MoldSN)
        {
            string SqlStr = "Select * From CST_API_MOLD_RESET_LOG WHERE MOLD_NAME = #[STRING]";
            return InfoCenter.GetBySQL<CSTAPIMoldResetLogInfo>(SqlStr,MoldSN);
        }

    }
}
