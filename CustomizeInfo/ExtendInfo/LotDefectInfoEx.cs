using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    [Serializable]
    public partial class LotDefectInfoEx : LotDefectInfo
    {
        public static LotDefectInfoEx GetDataByLotAndComponentID(string lot, string componentID)
        {
            string sql = @"SELECT * FROM MES_WIP_DEFECT 
                            WHERE LOT = #[STRING] AND COMPONENTID =  #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, lot, componentID);

            return InfoCenter.GetBySQL<LotDefectInfoEx>(sa);
        }

        public static List<LotDefectInfoEx> GetDataByLotAndOperation(string lot, string operation)
        {
            return InfoCenter.GetList<LotDefectInfoEx>("SELECT * FROM MES_WIP_DEFECT WHERE LOT = #[STRING] AND OPERATION = #[STRING]", lot, operation);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

        public static List<LotDefectInfoEx> GetDataListByMaterialLot(string materialLot)
        {
            string sql = @"SELECT *
                             FROM MES_WIP_DEFECT MWD
                            INNER JOIN MES_WIP_LOT MWL ON MWL.MATERIALLOT = #[STRING] 
                                                      AND MWD.LOT = MWL.LOT";
            SqlAgent sa = SQLCenter.Parse(sql, materialLot);

            return InfoCenter.GetList<LotDefectInfoEx>(sa);
        }

        public static List<LotDefectInfoEx> GetDataListByInvLot(string invLot)
        {
            string sql = @"SELECT *
                             FROM MES_WIP_DEFECT MWD
                            INNER JOIN MES_WIP_LOT MWL ON MWL.INVLOT = #[STRING] 
                                                      AND MWD.LOT = MWL.LOT";
            SqlAgent sa = SQLCenter.Parse(sql, invLot);

            return InfoCenter.GetList<LotDefectInfoEx>(sa);
        }

        public static List<LotDefectInfoEx> GetDataListByWOLot(string woLot)
        {
            string sql = @"SELECT *
                             FROM MES_WIP_DEFECT MWD
                            INNER JOIN MES_WIP_LOT MWL ON MWL.WOLOT = #[STRING] 
                                                      AND MWD.LOT = MWL.LOT";
            SqlAgent sa = SQLCenter.Parse(sql, woLot);

            return InfoCenter.GetList<LotDefectInfoEx>(sa);
        }
    }
}
