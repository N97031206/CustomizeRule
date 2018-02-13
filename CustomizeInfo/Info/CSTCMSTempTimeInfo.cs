using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTCMSTempTimeInfo
    {
        /// <summary>
        /// 依據傳入的載具批號系統編號取得資料
        /// </summary>
        /// <param name="CMSLotSID">載具批號系統編號</param>
        /// <returns></returns>
        public static CSTCMSTempTimeInfo GetDataByCMSLotSID(string CMSLotSID)
        {
            string sSQL = @" SELECT * FROM CST_CMS_TEMP_TIME WHERE CMS_LOT_SID = #[STRING]";

            return InfoCenter.GetBySQL<CSTCMSTempTimeInfo>(sSQL, CMSLotSID);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
