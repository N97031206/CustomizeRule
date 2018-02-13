using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTMMSIqcPassInfo
    {
        /// <summary>
        /// 取得IQCPass資料經由爐號+批號+進料日當關鍵值
        /// </summary>
        /// <param name="iqcRunID"></param>
        /// <param name="iqcLot"></param>
        /// <param name="iqcTime"></param>
        /// <returns></returns>
        public static List<CSTMMSIqcPassInfo> GetIQCPassData(string iqcRunID, string iqcLot, string iqcTime)
        {
            string sSQL = @" SELECT * FROM CST_MMS_IQC_PASS WHERE RUNID = #[STRING] AND LOT = #[STRING] AND IQCTIME = #[STRING] ";

            return InfoCenter.GetList<CSTMMSIqcPassInfo>(sSQL, iqcRunID, iqcLot, iqcTime);
        }

        /// <summary>
        /// 取得IQCPass資料經由序號
        /// </summary>
        /// <param name="iqcSN"></param>
        /// <returns></returns>
        public static CSTMMSIqcPassInfo GetIQCPassDataByiqcSN(string iqcSN)
        {
            string sSQL = @" SELECT * FROM CST_MMS_IQC_PASS WHERE SN = #[STRING] ";

            return InfoCenter.GetBySQL<CSTMMSIqcPassInfo>(sSQL, iqcSN);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/
        public static List<CSTMMSIqcPassInfo> GetIQCPassDataListByLot(string Lot)
        {
            string sSQL = @" SELECT * FROM CST_MMS_IQC_PASS WHERE LOT = #[STRING]";

            return InfoCenter.GetList<CSTMMSIqcPassInfo>(sSQL, Lot);
        }
    }
}
