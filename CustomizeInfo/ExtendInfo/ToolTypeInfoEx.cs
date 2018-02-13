using Ares.Cimes.IntelliService.DbTableSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class ToolTypeInfoEx : ToolTypeInfo
    {
        static ToolTypeInfoEx()
        {
            MES_TOOL_TYPE.Singleton.SyncColumnsFromDB();
        }
        #region Property
        /// <summary>
        /// 是否上傳檢驗資料
        /// </summary>
        public string InspectionFlag
        {
            get
            {
                return (this["INSPFLAG"] is DBNull) ? string.Empty : (String)this["INSPFLAG"];
            }
            set
            {
                this["INSPFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 安全存量
        /// </summary>
        public int SafeQuantity
        {
            get
            {
                return (this["SAFEQTY"] is DBNull) ? 0 : int.Parse(this["SAFEQTY"].ToString());
            }
            set
            {
                this["SAFEQTY"] = value;
            }
        }

        /// <summary>
        /// 刀面數
        /// </summary>
        public string SideCount
        {
            get
            {
                return (this["SIDECOUNT"] is DBNull) ? string.Empty : (String)this["SIDECOUNT"];
            }
            set
            {
                this["SIDECOUNT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 品名規格
        /// </summary>
        public string Specification
        {
            get
            {
                return (this["SPEC"] is DBNull) ? string.Empty : (String)this["SPEC"];
            }
            set
            {
                this["SPEC"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 單位別
        /// </summary>
        public string Department
        {
            get
            {
                return (this["DEPARTMENT"] is DBNull) ? string.Empty : (String)this["DEPARTMENT"];
            }
            set
            {
                this["DEPARTMENT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }
        #endregion

        public static List<ToolTypeInfoEx> GetToolTypeByToolClass(string toolCalss)
        {
            string sql = @"SELECT * FROM MES_TOOL_TYPE WHERE TOOLCLASS = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, toolCalss);

            return InfoCenter.GetList<ToolTypeInfoEx>(sa);
        }
    }
}
