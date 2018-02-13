using Ares.Cimes.IntelliService.DbTableSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class ToolInfoEx : ToolInfo
    {
        static ToolInfoEx()
        {
            MES_TOOL_MAST.Singleton.SyncColumnsFromDB();
        }
        #region Property
        /// <summary>
        /// 供應商
        /// </summary>
        public string Vendor
        {
            get
            {
                return (this["VENDOR"] is DBNull) ? string.Empty : (String)this["VENDOR"];
            }
            set
            {
                this["VENDOR"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 刀具群組ID
        /// </summary>
        public string GroupID
        {
            get
            {
                return (this["GROUPID"] is DBNull) ? string.Empty : (String)this["GROUPID"];
            }
            set
            {
                this["GROUPID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 身分：新品、堪用、維修
        /// </summary>
        public string Identity
        {
            get
            {
                return (this["IDENTITY"] is DBNull) ? string.Empty : (String)this["IDENTITY"];
            }
            set
            {
                this["IDENTITY"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
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

        /// <summary>
        /// 刀面
        /// </summary>
        public string Head
        {
            get
            {
                return (this["HEAD"] is DBNull) ? string.Empty : (String)this["HEAD"];
            }
            set
            {
                this["HEAD"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
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
        /// 維修次數
        /// </summary>
        public int MaintainCount
        {
            get
            {
                return (this["MAINTAINCOUNT"] is DBNull) ? 0 : int.Parse(this["MAINTAINCOUNT"].ToString());
            }
            set
            {
                this["MAINTAINCOUNT"] = value;
            }
        }

        /// <summary>
        /// 配件類別
        /// </summary>
        public string ToolClass
        {
            get
            {
                return (this["TOOLCLASS"] is DBNull) ? string.Empty : (String)this["TOOLCLASS"];
            }
            set
            {
                this["TOOLCLASS"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }
        #endregion

        public static List<ToolInfoEx> GetToolListByGroupID(string groupID)
        {
            string sql = @"SELECT * FROM MES_TOOL_MAST 
                            WHERE GROUPID = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, groupID);

            return InfoCenter.GetList<ToolInfoEx>(sa);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/
        public static List<ToolInfoEx> GetToolByToolClass(string toolCalss)
        {
            string sql = @"SELECT * FROM MES_TOOL_MAST WHERE TOOLCLASS = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, toolCalss);

            return InfoCenter.GetList<ToolInfoEx>(sa);
        }

        public static List<ToolInfoEx> GetToolByToolTypeAndVendor(string toolType, string vendor)
        {
            string sql = @"SELECT * FROM MES_TOOL_MAST 
                            WHERE TOOLTYPE = #[STRING] AND VENDOR = #[STRING]
                            ORDER BY TOOLNAME";
            SqlAgent sa = SQLCenter.Parse(sql, toolType, vendor);

            return InfoCenter.GetList<ToolInfoEx>(sa);
        }

        public static List<ToolInfoEx> GetToolsByEquipmentAndLocation(string equipment, string location)
        {
            string sql = @"SELECT * FROM MES_EQP_TOOL EQPTOOL,
                        (SELECT TOOLNAME, THREE_FORGING_TYPE FROM MES_TOOL_MAST TOOL, MES_TOOL_TYPE TYPE
                        WHERE TOOL.TOOLTYPE = TYPE.TYPE AND TOOL.TOOLCLASS = 'DIE') TOOL
                        WHERE EQPTOOL.TOOLNAME = TOOL.TOOLNAME AND EQPTOOL.EQUIPMENT = #[STRING] AND THREE_FORGING_TYPE = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, equipment, location);

            return InfoCenter.GetList<ToolInfoEx>(sa);
        }
    }
}
