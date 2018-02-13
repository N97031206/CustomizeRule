using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTAPIMoldCountInfo
    {
        /// <summary>
        /// 取得模具使用次數資料
        /// </summary>
        /// <param name="MoldSN"></param>
        /// <returns></returns>
        public static CSTAPIMoldCountInfo GetMoldCount(string MoldSN)
        {
            string SqlStr = "Select * From CST_API_MOLD_COUNT WHERE MOLD_NAME = #[STRING]";
            return InfoCenter.GetBySQL<CSTAPIMoldCountInfo>(SqlStr, MoldSN);
        }


    }
}
