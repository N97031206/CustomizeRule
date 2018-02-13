using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTToolTypePictureInfo
    {
        public static List<CSTToolTypePictureInfo> GetDataListByToolType(string toolType)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_TYPE_PICT
                                WHERE TOOLTYPE = #[STRING] 
                                ORDER BY FILENAME";

            return InfoCenter.GetList<CSTToolTypePictureInfo>(sSQL, toolType);
        }

        public static CSTToolTypePictureInfo GetDataByFileName(string fileName)
        {
            string sSQL = @" SELECT * FROM CST_TOOL_TYPE_PICT
                                WHERE FILENAME = #[STRING] ";

            return InfoCenter.GetBySQL<CSTToolTypePictureInfo>(sSQL, fileName);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
