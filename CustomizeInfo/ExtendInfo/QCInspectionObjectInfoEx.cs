using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class QCInspectionObjectInfoEx : QCInspectionObjectInfo
    {
        public static List<QCInspectionObjectInfoEx> GetDataListByComponentLot(string componentLot)
        {
            string sql = @"SELECT * FROM MES_QC_INSP_OBJ 
                            WHERE ITEM1 = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, componentLot);

            return InfoCenter.GetList<QCInspectionObjectInfoEx>(sa);
        }

        public static List<QCInspectionObjectInfoEx> GetDataListByQCInspectionSID(string QCInspectionSID)
        {
            string sql = @"SELECT * FROM MES_QC_INSP_OBJ 
                            WHERE QC_INSP_SID = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, QCInspectionSID);

            return InfoCenter.GetList<QCInspectionObjectInfoEx>(sa);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
