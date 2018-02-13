﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//
//     變更這個檔案可能會導致不正確的行為，而且如果已重新產生
//     程式碼，這個檔案將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

using Ares.Cimes.IntelliService.DbTableSchema;

namespace Ares.Cimes.IntelliService.DbTableRow
{
    /// <summary>
    /// CST_USR_WORKTIME_HIST_Row, 人員上下工HIST資料
    /// </summary>
    [Serializable]
    public class CST_USR_WORKTIME_HIST_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_USR_WORKTIME_HIST Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_USR_WORKTIME_HIST.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_USR_WORKTIME_HIST_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_USR_WORKTIME_HIST_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_USR_WORKTIME_HIST_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_USR_WORKTIME_HIST_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_USR_WORKTIME_HIST_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 人員上下工歷史紀錄系統編號 資料型態:NVARCHAR2(100) 是否允許Null:N 
    /// </summary>
    public String USR_WORKTIME_HIST_SID { 
      get { return (this["USR_WORKTIME_HIST_SID"] is DBNull) ? string.Empty : (String) this["USR_WORKTIME_HIST_SID"]; } 
      set { this["USR_WORKTIME_HIST_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 人員上下工系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String USR_WORKTIME_SID { 
      get { return (this["USR_WORKTIME_SID"] is DBNull) ? string.Empty : (String) this["USR_WORKTIME_SID"]; } 
      set { this["USR_WORKTIME_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 工單 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String WO { 
      get { return (this["WO"] is DBNull) ? string.Empty : (String) this["WO"]; } 
      set { this["WO"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 線別/機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LINE { 
      get { return (this["LINE"] is DBNull) ? string.Empty : (String) this["LINE"]; } 
      set { this["LINE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 上工人員 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String WORKUSERID { 
      get { return (this["WORKUSERID"] is DBNull) ? string.Empty : (String) this["WORKUSERID"]; } 
      set { this["WORKUSERID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 工位 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String USR_WORKPALCE { 
      get { return (this["USR_WORKPALCE"] is DBNull) ? string.Empty : (String) this["USR_WORKPALCE"]; } 
      set { this["USR_WORKPALCE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 上工時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String STARTTIME { 
      get { return (this["STARTTIME"] is DBNull) ? string.Empty : (String) this["STARTTIME"]; } 
      set { this["STARTTIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 下工時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String ENDTIME { 
      get { return (this["ENDTIME"] is DBNull) ? string.Empty : (String) this["ENDTIME"]; } 
      set { this["ENDTIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 更新人員 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String USERID { 
      get { return (this["USERID"] is DBNull) ? string.Empty : (String) this["USERID"]; } 
      set { this["USERID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 更新時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String UPDATETIME { 
      get { return (this["UPDATETIME"] is DBNull) ? string.Empty : (String) this["UPDATETIME"]; } 
      set { this["UPDATETIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// Comm系统编号 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String COMM_SID { 
      get { return (this["COMM_SID"] is DBNull) ? string.Empty : (String) this["COMM_SID"]; } 
      set { this["COMM_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 執行標記 資料型態:CHAR(1) 是否允許Null:Y 
    /// </summary>
    public String EXECUTEFLAG { 
      get { return (this["EXECUTEFLAG"] is DBNull) ? string.Empty : (String) this["EXECUTEFLAG"]; } 
      set { this["EXECUTEFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 班别日期 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String SHIFTDATE { 
      get { return (this["SHIFTDATE"] is DBNull) ? string.Empty : (String) this["SHIFTDATE"]; } 
      set { this["SHIFTDATE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 班別 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String SHIFT { 
      get { return (this["SHIFT"] is DBNull) ? string.Empty : (String) this["SHIFT"]; } 
      set { this["SHIFT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 人員除外系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String USR_EXCEPT_WORKTIME_SID { 
      get { return (this["USR_EXCEPT_WORKTIME_SID"] is DBNull) ? string.Empty : (String) this["USR_EXCEPT_WORKTIME_SID"]; } 
      set { this["USR_EXCEPT_WORKTIME_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LOT { 
      get { return (this["LOT"] is DBNull) ? string.Empty : (String) this["LOT"]; } 
      set { this["LOT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_USR_WORKTIME_HIST_Row DeepCopy()
        {
            return (CST_USR_WORKTIME_HIST_Row) base.DeepCopy();
        }
    }
}


