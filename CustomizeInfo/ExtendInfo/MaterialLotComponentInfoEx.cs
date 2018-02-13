using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.DbTableSchema;
using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class MaterialLotComponentInfoEx : MaterialLotComponentInfo
    {
        static MaterialLotComponentInfoEx()
        {
            MES_MMS_COMP.Singleton.SyncColumnsFromDB();
        }

        #region Property
        /// <summary>
        /// 是否為來料序號的最後一支(Y/N)
        /// </summary>
        public string LastFlag
        {
            get
            {
                return (this["LASTFLAG"] is DBNull) ? string.Empty : (String)this["LASTFLAG"];
            }
            set
            {
                this["LASTFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }
        /// <summary>
        /// 秤重結果
        /// </summary>
        public string WeightResult
        {
            get
            {
                return (this["WEIGHT_RESULT"] is DBNull) ? string.Empty : (String)this["WEIGHT_RESULT"];
            }
            set
            {
                this["WEIGHT_RESULT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        #endregion

        public static MaterialLotComponentInfoEx GetMLotDataByComponentID(string componentID)
        {
            string sql = @"SELECT * FROM MES_MMS_COMP
                            WHERE COMPONENTID = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, componentID);

            return InfoCenter.GetBySQL<MaterialLotComponentInfoEx>(sa);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
