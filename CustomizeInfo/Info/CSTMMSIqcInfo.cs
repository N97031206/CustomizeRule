using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTMMSIqcInfo
    {
        /// <summary>
        /// 取得IQCInfo資料經由爐號+批號+進料日當關鍵值
        /// </summary>
        /// <param name="iqcRunID"></param>
        /// <param name="iqcLot"></param>
        /// <param name="iqcTime"></param>
        /// <returns></returns>
        public static CSTMMSIqcInfo GetIQCInfoData(string iqcRunID, string iqcLot, string iqcTime)
        {
            string sSQL = @" SELECT * FROM CST_MMS_IQC_INFO WHERE RUNID = #[STRING] AND LOT = #[STRING] AND IQCTIME = #[STRING] ";

            return InfoCenter.GetBySQL<CSTMMSIqcInfo>(sSQL, iqcRunID, iqcLot, iqcTime);
        }

        public static List<CSTMMSIqcInfo> GetIQCInfoDataByRunIDAndLot(string iqcRunID, string iqcLot)
        {
            string sSQL = @" SELECT * FROM CST_MMS_IQC_INFO WHERE RUNID = #[STRING] AND LOT = #[STRING] ";

            return InfoCenter.GetList<CSTMMSIqcInfo>(sSQL, iqcRunID, iqcLot);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
