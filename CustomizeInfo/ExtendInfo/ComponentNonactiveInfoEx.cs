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
    public partial class ComponentNonactiveInfoEx : ComponentNonactiveInfo
    {
        static ComponentNonactiveInfoEx()
        {
            MES_WIP_COMP_NONACTIVE.Singleton.SyncColumnsFromDB();
        }

        #region Property
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
        /// 型號
        /// </summary>
        public string DeviceName
        {
            get
            {
                return (this["DEVICE"] is DBNull) ? string.Empty : (String)this["DEVICE"];
            }
            set
            {
                this["DEVICE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
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
        /// 批號序號
        /// </summary>
        public string CPFSN
        {
            get
            {
                return (this["CPF_SN"] is DBNull) ? string.Empty : (String)this["CPF_SN"];
            }
            set
            {
                this["CPF_SN"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 進熱爐先後順序
        /// </summary>
        public string SN
        {
            get
            {
                return (this["SN"] is DBNull) ? string.Empty : (String)this["SN"];
            }
            set
            {
                this["SN"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 鍛造的位置
        /// </summary>
        public string Location
        {
            get
            {
                return (this["LOCATION"] is DBNull) ? string.Empty : (String)this["LOCATION"];
            }
            set
            {
                this["LOCATION"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 鍛造紀錄是否有領過序號
        /// </summary>
        public string SNFlag
        {
            get
            {
                return (this["SNFLAG"] is DBNull) ? string.Empty : (String)this["SNFLAG"];
            }
            set
            {
                this["SNFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// A孔檔案
        /// </summary>
        public string FileA
        {
            get
            {
                return (this["FILE_A"] is DBNull) ? string.Empty : (String)this["FILE_A"];
            }
            set
            {
                this["FILE_A"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// B孔檔案
        /// </summary>
        public string FileB
        {
            get
            {
                return (this["FILE_B"] is DBNull) ? string.Empty : (String)this["FILE_B"];
            }
            set
            {
                this["FILE_B"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// C孔檔案
        /// </summary>
        public string FileC
        {
            get
            {
                return (this["FILE_C"] is DBNull) ? string.Empty : (String)this["FILE_C"];
            }
            set
            {
                this["FILE_C"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// A位置華司
        /// </summary>
        public string WasherA
        {
            get
            {
                return (this["WASHER_A"] is DBNull) ? string.Empty : (String)this["WASHER_A"];
            }
            set
            {
                this["WASHER_A"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// B位置華司
        /// </summary>
        public string WasherB
        {
            get
            {
                return (this["WASHER_B"] is DBNull) ? string.Empty : (String)this["WASHER_B"];
            }
            set
            {
                this["WASHER_B"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// C位置華司
        /// </summary>
        public string WasherC
        {
            get
            {
                return (this["WASHER_C"] is DBNull) ? string.Empty : (String)this["WASHER_C"];
            }
            set
            {
                this["WASHER_C"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        ///紀錄是否有量測過中心孔
        /// </summary>
        public string CenterHoleFlag
        {
            get
            {
                return (this["CENTER_HOLE_FLAG"] is DBNull) ? string.Empty : (String)this["CENTER_HOLE_FLAG"];
            }
            set
            {
                this["CENTER_HOLE_FLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }
        #endregion

        /// <summary>
        /// 型號類型: LOT
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        public static List<ComponentNonactiveInfo> GetDataByMaterialLot(string materialLot)
        {
            string sql = @"SELECT * FROM MES_WIP_COMP_NONACTIVE
                            WHERE MATERIALLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, materialLot);

            return InfoCenter.GetList<ComponentNonactiveInfo>(sa);
        }
        /// <summary>
        /// 型號類型: RUNCARD
        /// </summary>
        /// <param name="workOrderLot"></param>
        /// <returns></returns>
        public static List<ComponentNonactiveInfo> GetDataByWorkOrderLot(string workOrderLot)
        {
            string sql = @"SELECT * FROM MES_WIP_COMP_NONACTIVE
                            WHERE WOLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, workOrderLot);

            return InfoCenter.GetList<ComponentNonactiveInfo>(sa);
        }

        public static List<ComponentNonactiveInfoEx> GetDataByInvBoxNo(string workOrderLot)
        {
            string sql = @"SELECT * FROM MES_WIP_COMP_NONACTIVE
                WHERE CURRENTLOT IN (SELECT COMPONENTID FROM MES_WIP_COMP_NONACTIVE WHERE CURRENTLOT = #[STRING])";
            SqlAgent sa = SQLCenter.Parse(sql, workOrderLot);

            return InfoCenter.GetList<ComponentNonactiveInfoEx>(sa);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}

