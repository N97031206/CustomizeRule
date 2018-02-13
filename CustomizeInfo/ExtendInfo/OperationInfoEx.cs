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
    public partial class OperationInfoEx : OperationInfo
    {
        static OperationInfoEx()
        {
            MES_PRC_OPER.Singleton.SyncColumnsFromDB();
        }

        #region Property   
        /// <summary>
        /// 判定該站點是否執行併批作業
        /// </summary>
        public string AutoMerge
        {
            get { return (this["AUTOMERGE"] is DBNull) ? string.Empty : (String)this["AUTOMERGE"]; }
            set { this["AUTOMERGE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        /// <summary>
        /// 判定是否要送首件
        /// </summary>
        public string FAIFlag
        {
            get { return (this["FAIFLAG"] is DBNull) ? string.Empty : (String)this["FAIFLAG"]; }
            set { this["FAIFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        /// <summary>
        /// 是否可預約進站
        /// </summary>
        public string ReserveFlag
        {
            get { return (this["RESERVE_FLAG"] is DBNull) ? string.Empty : (String)this["RESERVE_FLAG"]; }
            set { this["RESERVE_FLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        /// <summary>
        /// 是否需要檢查BOM表
        /// </summary>
        public string CheckBOM
        {
            get { return (this["CHECKBOM"] is DBNull) ? string.Empty : (String)this["CHECKBOM"]; }
            set { this["CHECKBOM"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        /// <summary>
        /// 紀錄CNC主要機台，此設定與首件有關聯
        /// </summary>
        public string MainEquip
        {
            get { return (this["MAIN_EQUIP"] is DBNull) ? string.Empty : (String)this["MAIN_EQUIP"]; }
            set { this["MAIN_EQUIP"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        /// <summary>
        /// 判斷該站點是否可做中心孔量測
        /// </summary>
        public string CenterHoleFlag
        {
            get { return (this["CENTER_HOLE_FLAG"] is DBNull) ? string.Empty : (String)this["CENTER_HOLE_FLAG"]; }
            set { this["CENTER_HOLE_FLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        /// <summary>
        /// 建立批號的基礎，COMP：一支一批，LOT：N支一批
        /// </summary>
        public string RunBase
        {
            get { return (this["RUNBASE"] is DBNull) ? string.Empty : (String)this["RUNBASE"]; }
            set { this["RUNBASE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        /// <summary>
        /// 判定該站點是否需收集DMC
        /// </summary>
        public string GetDMC
        {
            get { return (this["GETDMC"] is DBNull) ? string.Empty : (String)this["GETDMC"]; }
            set { this["GETDMC"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }
        #endregion

    }
}

