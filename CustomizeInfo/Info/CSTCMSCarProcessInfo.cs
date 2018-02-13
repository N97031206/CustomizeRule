using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTCMSCarProcessInfo
    {
        public static CSTCMSCarProcessInfo GetDataByLoadLotFlagIsN(string line)
        {
            string sSQL = @"SELECT * FROM CST_CMS_CAR_PROCESS 
                            WHERE LOADLOT_FLAG = 'N' AND LINE = #[STRING]
                            ORDER BY UPDATETIME DESC";

            return InfoCenter.GetBySQL<CSTCMSCarProcessInfo>(sSQL, line);
        }

        public static CSTCMSCarProcessInfo GetDataByLoadLotFlagIsN(string line, string carSequence)
        {
            string sSQL = @"SELECT * FROM CST_CMS_CAR_PROCESS 
                            WHERE LOADLOT_FLAG = 'N' AND LINE = #[STRING] AND CAR_SEQ = #[STRING]
                            ORDER BY UPDATETIME DESC";

            return InfoCenter.GetBySQL<CSTCMSCarProcessInfo>(sSQL, line, carSequence);
        }

        public static CSTCMSCarProcessInfo GetDataByLoadLotFlagIsY(string line, string carSequence)
        {
            string sSQL = @"SELECT * FROM CST_CMS_CAR_PROCESS 
                            WHERE LOADLOT_FLAG = 'Y' AND LINE = #[STRING] AND CAR_SEQ = #[STRING]
                            ORDER BY UPDATETIME DESC";

            return InfoCenter.GetBySQL<CSTCMSCarProcessInfo>(sSQL, line, carSequence);
        }

        public static CSTCMSCarProcessInfo GetDataByLine(string line)
        {
            string sSQL = @"SELECT * FROM CST_CMS_CAR_PROCESS 
                            WHERE LINE = #[STRING]
                            ORDER BY UPDATETIME DESC";

            return InfoCenter.GetBySQL<CSTCMSCarProcessInfo>(sSQL, line);
        }
    }
}
