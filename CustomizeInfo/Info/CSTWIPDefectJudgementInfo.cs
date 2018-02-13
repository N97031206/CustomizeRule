using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWIPDefectJudgementInfo
    {
        public static CSTWIPDefectJudgementInfo GetDataByWIPDefectSID(string WIPDefectSID)
        {
            string sSQL = @" SELECT * FROM CST_WIP_DEFECT_JUDGEMENT 
                                WHERE WIP_DEFECT_SID = #[STRING]";

            return InfoCenter.GetBySQL<CSTWIPDefectJudgementInfo>(sSQL, WIPDefectSID);
        }

        public static CSTWIPDefectJudgementInfo GetDataByLot(string lot)
        {
            string sSQL = @" SELECT * FROM CST_WIP_DEFECT_JUDGEMENT 
                                WHERE LOT = #[STRING] ORDER BY WIP_DEFECT_JUDGEMENT_SID";

            return InfoCenter.GetBySQL<CSTWIPDefectJudgementInfo>(sSQL, lot);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
