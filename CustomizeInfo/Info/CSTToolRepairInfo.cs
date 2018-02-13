using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTToolRepairInfo
    {
        public static CSTToolRepairInfo GetDataByToolName(string toolName)
        {
            string sSQL = @"SELECT * FROM CST_TOOL_REPAIR 
                            WHERE TOOLNAME =#[STRING] 
                            AND ACTUAL_DATE_OF_RETURN IS NULL";

            return InfoCenter.GetBySQL<CSTToolRepairInfo>(sSQL, toolName);
        }
    }
}
