using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWIPPackInfo
    {
        public static CSTWIPPackInfo GetPackInfoByBoxNo(string boxNo)
        {
            string sSQL = @"SELECT * FROM CST_WIP_PACK WHERE BOXNO = #[STRING] ";

            return InfoCenter.GetBySQL<CSTWIPPackInfo>(sSQL, boxNo);
        }        

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/
    }
}
