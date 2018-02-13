using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ares.Cimes.IntelliService.DbTableSchema;

namespace Ares.Cimes.IntelliService.Info
{
    [Serializable]
    public partial class LotNonActiveInfoEx : LotNonActiveInfo
    {
        static LotNonActiveInfoEx()
        {
            MES_WIP_LOT_NONACTIVE.Singleton.SyncColumnsFromDB();
        }

        #region Property   
        /// <summary>
        /// 機加批號
        /// </summary>
        public string WorkOrderLot
        {
            get
            {
                return (this["WOLOT"] is DBNull) ? string.Empty : (String)this["WOLOT"];
            }
            set
            {
                this["WOLOT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 紀錄ComponentID用的
        /// </summary>
        public string ComponentLot
        {
            get
            {
                return (this["COMPLOT"] is DBNull) ? string.Empty : (String)this["COMPLOT"];
            }
            set
            {
                this["COMPLOT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        public string ProcessEquipment
        {
            get
            {
                return (this["PROCESS_EQUIP"] is DBNull) ? string.Empty : (String)this["PROCESS_EQUIP"];
            }
            set
            {
                this["PROCESS_EQUIP"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 鍛造批號
        /// </summary>
        public string MaterialLot
        {
            get
            {
                return (this["MATERIALLOT"] is DBNull) ? string.Empty : (String)this["MATERIALLOT"];
            }
            set
            {
                this["MATERIALLOT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        #endregion

        /// <summary>
        /// 根據UDC09(出貨清單)查詢批號(箱號)清單
        /// </summary>
        /// <param name="udc09"></param>
        /// <returns></returns>
        public static List<LotNonActiveInfoEx> GetLotListByUDC09(string udc09)
        {
            string sql = @"SELECT * FROM MES_WIP_LOT_NONACTIVE WHERE USERDEFINECOL09 = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, udc09);

            return InfoCenter.GetList<LotNonActiveInfoEx>(sa);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}

