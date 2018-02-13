using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ares.Cimes.IntelliService.DbTableSchema;
using CustomizeRule.RuleUtility;

namespace Ares.Cimes.IntelliService.Info
{
    [Serializable]
    public partial class LotInfoEx : LotInfo
    {
        static LotInfoEx()
        {
            MES_WIP_LOT.Singleton.SyncColumnsFromDB();
        }

        #region Property
        /// <summary>
        /// 入庫編號
        /// </summary>
        public string InventoryNo
        {
            get
            {
                return (this["INVNO"] is DBNull) ? string.Empty : (String)this["INVNO"];
            }
            set
            {
                this["INVNO"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 入庫批號
        /// </summary>
        public string InventoryLot
        {
            get
            {
                return (this["INVLOT"] is DBNull) ? string.Empty : (String)this["INVLOT"];
            }
            set
            {
                this["INVLOT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
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
        /// <summary>
        /// 製程別
        /// </summary>
        public string Process
        {
            get
            {
                return (this["PROCESS"] is DBNull) ? string.Empty : (String)this["PROCESS"];
            }
            set
            {
                this["PROCESS"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

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
        /// 自動線判斷巡檢使用
        /// </summary>
        public string PQCFlag
        {
            get
            {
                return (this["PQCFLAG"] is DBNull) ? string.Empty : (String)this["PQCFLAG"];
            }
            set
            {
                this["PQCFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        #endregion


        public static List<LotInfoEx> GetLotListByWorkOrderLot(string lot)
        {
            lot = CustomizeFunction.ConvertDMCCode(lot);

            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE WOLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, lot);

            return InfoCenter.GetList<LotInfoEx>(sa);
        }

        public static LotInfoEx GetLotByWorkOrderLot(string lot)
        {
            lot = CustomizeFunction.ConvertDMCCode(lot);

            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE WOLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, lot);

            return InfoCenter.GetBySQL<LotInfoEx>(sa);
        }

        public static List<LotInfoEx> GetLotByWorkOrderLotAndOperation(string lot, string operation)
        {
            lot = CustomizeFunction.ConvertDMCCode(lot);

            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE WOLOT = #[STRING] AND OPERATION = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, lot, operation);

            return InfoCenter.GetList<LotInfoEx>(sa);
        }

        public static LotInfoEx GetLotInfoByCompLot(string compLot)
        {
            compLot = CustomizeFunction.ConvertDMCCode(compLot);

            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE COMPLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, compLot);
            return InfoCenter.GetBySQL<LotInfoEx>(sa);
        }

        public static List<LotInfoEx> GetLotListByMaterialLot(string lot)
        {
            lot = CustomizeFunction.ConvertDMCCode(lot);

            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE MATERIALLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, lot);

            return InfoCenter.GetList<LotInfoEx>(sa);
        }

        public static List<LotInfoEx> GetLotListByInvertoryLot(string lot)
        {
            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE INVLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, lot);

            return InfoCenter.GetList<LotInfoEx>(sa);
        }

        public static List<LotInfoEx> GetLotListByStatus(string status)
        {
            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE STATUS = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, status);

            return InfoCenter.GetList<LotInfoEx>(sa);
        }

        public static List<LotInfoEx> GetLotListByComponentLot(string componentLot)
        {
            componentLot = CustomizeFunction.ConvertDMCCode(componentLot);

            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE COMPLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, componentLot);

            return InfoCenter.GetList<LotInfoEx>(sa);
        }

        public static LotInfoEx GetLotByLot(string lot)
        {
            lot = CustomizeFunction.ConvertDMCCode(lot);

            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE LOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, lot);
            return InfoCenter.GetBySQL<LotInfoEx>(sa);
        }

        public static List<LotInfoEx> GetLotByMaterialLotAndWOLot(string materialLot, string woLot)
        {
            materialLot = CustomizeFunction.ConvertDMCCode(materialLot);
            woLot = CustomizeFunction.ConvertDMCCode(woLot);

            string sql = @"SELECT * FROM MES_WIP_LOT 
                            WHERE MATERIALLOT = #[STRING] AND WOLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, materialLot, woLot);

            return InfoCenter.GetList<LotInfoEx>(sa);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}

