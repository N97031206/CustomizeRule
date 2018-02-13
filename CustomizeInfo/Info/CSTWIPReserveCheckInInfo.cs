using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWIPReserveCheckInInfo
    {
        #region Property
        /// <summary>
        /// 機台編號
        /// </summary>
        public string Equipment
        {
            get
            {
                return (this["EQUIPMENT"] is DBNull) ? string.Empty : (String)this["EQUIPMENT"];
            }
            set
            {
                this["EQUIPMENT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }
        #endregion

        public static List<CSTWIPReserveCheckInInfo> GetDataByLot(string lot)
        {
            string sSQL = @" SELECT * FROM CST_WIP_RESERVE_CHECKIN_INFO 
                                WHERE LOT = #[STRING] AND OUTTIME IS NULL 
                            ORDER BY INTIME DESC";

            return InfoCenter.GetList<CSTWIPReserveCheckInInfo>(sSQL, lot);
        }

        public static List<CSTWIPReserveCheckInInfo> GetDataByLotAndOper(string lot, string operation)
        {
            string sSQL = @" SELECT * FROM CST_WIP_RESERVE_CHECKIN_INFO 
                                WHERE LOT = #[STRING] AND OPERATION = #[STRING] AND OUTTIME IS NULL 
                            ORDER BY INTIME DESC";

            return InfoCenter.GetList<CSTWIPReserveCheckInInfo>(sSQL, lot, operation);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

        //public static CSTWIPReserveCheckInInfo GetDataByLost(string lot)
        //{
        //    string sSQL = @" SELECT * FROM CST_WIP_RESERVE_CHECKIN_INFO 
        //                        WHERE LOT = #[STRING] AND OUTTIME IS NULL 
        //                    ORDER BY INTIME DESC";

        //    return InfoCenter.GetBySQL<CSTWIPReserveCheckInInfo>(sSQL, lot);
        //}
    }
}
