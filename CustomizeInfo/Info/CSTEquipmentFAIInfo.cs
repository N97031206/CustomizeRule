using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTEquipmentFAIInfo
    {
        /// <summary>
        /// 根據工單及機台查詢資料
        /// </summary>
        /// <param name="workOrder">工單</param>
        /// <param name="equipmentName">機台</param>
        /// <returns></returns>
        public static CSTEquipmentFAIInfo GetDataByEquipmentAndDevice(string equipment, string device)
        {
            string sSQL = @" SELECT * FROM CST_EQP_FAI WHERE EQUIPMENT = #[STRING] AND DEVICE = #[STRING] ";

            return InfoCenter.GetBySQL<CSTEquipmentFAIInfo>(sSQL, equipment, device);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
