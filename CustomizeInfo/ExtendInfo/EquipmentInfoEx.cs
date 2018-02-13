using Ares.Cimes.IntelliService;
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
    public partial class EquipmentInfoEx : EquipmentInfo
    {
        static EquipmentInfoEx()
        {
            MES_EQP_EQP.Singleton.SyncColumnsFromDB();
        }

        #region Property
        
        /// <summary>
        /// 設定孔位
        /// </summary>
        public string PushLocation
        {
            get
            {
                return (this["PUSH_LOCATION"] is DBNull) ? string.Empty : (String)this["PUSH_LOCATION"];
            }
            set
            {
                this["PUSH_LOCATION"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 報工機台權重
        /// </summary>
        public string WorktimeRate
        {
            get
            {
                return (this["WORKTIME_RATE"] is DBNull) ? string.Empty : (String)this["WORKTIME_RATE"];
            }
            set
            {
                this["WORKTIME_RATE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 是否為自動線機台
        /// </summary>
        public string AutoFlag
        {
            get
            {
                return (this["AUTOFLAG"] is DBNull) ? string.Empty : (String)this["AUTOFLAG"];
            }
            set
            {
                this["AUTOFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 是否控管刀具
        /// </summary>
        public string ToolFlag
        {
            get
            {
                return (this["TOOLFLAG"] is DBNull) ? string.Empty : (String)this["TOOLFLAG"];
            }
            set
            {
                this["TOOLFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }
        /// <summary>
        /// 機台簡碼
        /// </summary>
        public string Code
        {
            get
            {
                return (this["CODE"] is DBNull) ? string.Empty : (String)this["CODE"];
            }
            set
            {
                this["CODE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }


        #endregion

        /// <summary>
        /// 依據機台簡碼來取得機台資訊
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        public static EquipmentInfoEx GetDataByEquipmentCode(string equipmentCode)
        {
            string sSQL = @"SELECT * FROM MES_EQP_EQP 
                        WHERE CODE = #[STRING]";

            return InfoCenter.GetBySQL<EquipmentInfoEx>(sSQL, equipmentCode);
        }
    }
}
