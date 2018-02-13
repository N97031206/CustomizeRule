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
    public partial class ComponentInfoEx : ComponentInfo
    {
        static ComponentInfoEx()
        {
            MES_WIP_COMP.Singleton.SyncColumnsFromDB();
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

        /// <summary>
        ///檢驗單號
        /// </summary>
        public string InspectionNO
        {
            get
            {
                return (this["INSPNO"] is DBNull) ? string.Empty : (String)this["INSPNO"];
            }
            set
            {
                this["INSPNO"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        ///巡檢NG旗標
        /// </summary>
        public string PQCNGFLAG
        {
            get
            {
                return (this["PQCNGFLAG"] is DBNull) ? string.Empty : (String)this["PQCNGFLAG"];
            }
            set
            {
                this["PQCNGFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        ///巡檢NG單號
        /// </summary>
        public string PQCNGNO
        {
            get
            {
                return (this["PQCNGNO"] is DBNull) ? string.Empty : (String)this["PQCNGNO"];
            }
            set
            {
                this["PQCNGNO"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        ///DMC CODE
        /// </summary>
        public string DMC
        {
            get
            {
                return (this["DMC"] is DBNull) ? string.Empty : (String)this["DMC"];
            }
            set
            {
                this["DMC"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        #endregion

        public static List<ComponentInfo> GetDataByCurrentLot(string lot)
        {
            string sql = @"SELECT * FROM MES_WIP_COMP
                            WHERE CURRENTLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, lot);

            return InfoCenter.GetList<ComponentInfo>(sa);
        }

        /// <summary>
        /// 依據傳入Location參數取得資料
        /// </summary>
        /// <param name="location">鍛造的位置</param>
        /// <returns></returns>
        public static List<ComponentInfoEx> GetDataByLocation(string location)
        {
            string sql = @"SELECT *
                              FROM (  SELECT *
                                        FROM MES_WIP_COMP
                                       WHERE LOCATION = #[STRING]
                                    ORDER BY CAST(SN AS INT) ASC) MWC";

            SqlAgent sa = SQLCenter.Parse(sql, location);

            return InfoCenter.GetList<ComponentInfoEx>(sa);
        }

        /// <summary>
        /// 依據傳入Location及SNFlag參數取得第一筆SN的資料
        /// </summary>
        /// <param name="location">鍛造的位置</param>
        /// <param name="SNFlag">鍛造紀錄是否有領過序號</param>
        /// <returns></returns>
        public static ComponentInfoEx GetDataByLocationAndSNFlag(string location, string SNFlag)
        {
            string sql = @"SELECT *
                              FROM (  SELECT *
                                        FROM MES_WIP_COMP
                                       WHERE LOCATION = #[STRING] AND SNFLAG = #[STRING] AND FORGING_FLAG = 'Y'
                                    ORDER BY CAST(SN AS INT) ASC) MWC
                             WHERE ROWNUM < 2";
            SqlAgent sa = SQLCenter.Parse(sql, location, SNFlag);

            return InfoCenter.GetBySQL<ComponentInfoEx>(sa);
        }

        /// <summary>
        /// 依據傳入location及CPF_SN參數取得資料
        /// </summary>
        /// <param name="location"></param>
        /// <param name="CPF_SN"></param>
        /// <returns></returns>
        public static ComponentInfoEx GetDataByLocationAndCPFSN(string location, string CPF_SN)
        {
            string sql = @"SELECT *
                              FROM MES_WIP_COMP
                             WHERE LOCATION = #[STRING] AND CPF_SN = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, location, CPF_SN);

            return InfoCenter.GetBySQL<ComponentInfoEx>(sa);
        }

        /// <summary>
        /// 依據傳入CPF_SN參數及FORGING_FLAG=Y取得資料
        /// </summary>
        /// <param name="location"></param>
        /// <param name="CPF_SN"></param>
        /// <returns></returns>
        public static ComponentInfoEx GetDataByCPFSN(string CPF_SN)
        {
            string sql = @"SELECT *
                              FROM MES_WIP_COMP
                             WHERE FORGING_FLAG = 'Y' AND CPF_SN = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, CPF_SN);

            return InfoCenter.GetBySQL<ComponentInfoEx>(sa);
        }

        /// <summary>
        /// 依據傳入檢驗單號取得資料
        /// </summary>
        /// <param name="location"></param>
        /// <param name="CPF_SN"></param>
        /// <returns></returns>
        public static List<ComponentInfoEx> GetDataByInspectionNo(string inspectionNo)
        {
            string sql = @"SELECT *
                              FROM MES_WIP_COMP
                             WHERE INSPNO = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, inspectionNo);

            return InfoCenter.GetList<ComponentInfoEx>(sa);
        }

        public static ComponentInfoEx GetComponentByComponentID(string componentID)
        {
            componentID = CustomizeFunction.ConvertDMCCode(componentID);

            string sql = @"SELECT *
                              FROM MES_WIP_COMP
                             WHERE COMPONENTID = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, componentID);

            return InfoCenter.GetBySQL<ComponentInfoEx>(sa);
        }

        public static List<ComponentInfoEx> GetComponentListByComponentID(string componentID)
        {            

            string sql = @"SELECT *
                              FROM MES_WIP_COMP
                             WHERE COMPONENTID = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, componentID);

            return InfoCenter.GetList<ComponentInfoEx>(sa);
        }

        public static List<ComponentInfoEx> GetComponentByDMCCode(string dmcCode)
        {
            string sql = @"SELECT *
                              FROM MES_WIP_COMP
                             WHERE DMC = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, dmcCode);

            return InfoCenter.GetList<ComponentInfoEx>(sa);
        }

        public static ComponentInfoEx GetOneComponentByDMCCode(string dmcCode)
        {
            string sql = @"SELECT *
                              FROM MES_WIP_COMP
                             WHERE DMC = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, dmcCode);

            return InfoCenter.GetBySQL<ComponentInfoEx>(sa);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

        public static List<ComponentInfoEx> GetDatasByLikeCPF_SN(string CPF_SN)
        {
            string sql = @"SELECT * FROM MES_WIP_COMP
                            WHERE CPF_SN LIKE #[STRING]
                            AND FORGING_FLAG = 'Y'
                            ORDER BY CPF_SN";
            SqlAgent sa = SQLCenter.Parse(sql, CPF_SN + "%");

            return InfoCenter.GetList<ComponentInfoEx>(sa);
        }

        /// <summary>
        /// 取得最新建立的SID資料 
        /// </summary>
        /// <returns></returns>
        public static ComponentInfoEx GetDataByLatestSID()
        {
            string sql = @"SELECT * FROM MES_WIP_COMP WHERE CPF_SN IS NOT NULL
                            ORDER BY WIP_COMP_SID DESC";
            SqlAgent sa = SQLCenter.Parse(sql);

            return InfoCenter.GetBySQL<ComponentInfoEx>(sa);
        }

        /// <summary>
        /// 取得同小工單的COMPONENT
        /// </summary>
        /// <param name="WOLOT"></param>
        /// <returns></returns>
        public static List<ComponentInfoEx> GetAllComponentByWOLot(string woLot)
        {
            string sql = @"SELECT * FROM MES_WIP_COMP WHERE WOLOT = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, woLot);

            return InfoCenter.GetList<ComponentInfoEx>(sa);
        }
    }
}

