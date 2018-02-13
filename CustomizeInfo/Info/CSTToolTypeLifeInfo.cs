using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTToolTypeLifeInfo
    {
        public static List<CSTToolTypeLifeInfo> GetDataListByToolType(string toolType)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_TYPE_LIFE
                                WHERE TOOLTYPE = #[STRING] 
                                ORDER BY SUPPLIER";

            return InfoCenter.GetList<CSTToolTypeLifeInfo>(sSQL, toolType);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
