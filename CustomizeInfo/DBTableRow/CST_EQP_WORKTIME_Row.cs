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
    /// CST_EQP_WORKTIME_Row, 設備上下工資料
    /// </summary>
    [Serializable]
    public class CST_EQP_WORKTIME_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_EQP_WORKTIME Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_EQP_WORKTIME.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_EQP_WORKTIME_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_EQP_WORKTIME_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_EQP_WORKTIME_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_EQP_WORKTIME_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_EQP_WORKTIME_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 設備上下工系統編號 資料型態:NVARCHAR2(100) 是否允許Null:N 
    /// </summary>
    public String EQP_WORKTIME_SID { 
      get { return (this["EQP_WORKTIME_SID"] is DBNull) ? string.Empty : (String) this["EQP_WORKTIME_SID"]; } 
      set { this["EQP_WORKTIME_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
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
    /// 設備 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String EQUIPMENT { 
      get { return (this["EQUIPMENT"] is DBNull) ? string.Empty : (String) this["EQUIPMENT"]; } 
      set { this["EQUIPMENT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 工位 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String EQP_WORKPALCE { 
      get { return (this["EQP_WORKPALCE"] is DBNull) ? string.Empty : (String) this["EQP_WORKPALCE"]; } 
      set { this["EQP_WORKPALCE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
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
    /// 操作者 資料型態:NVARCHAR2(100) 是否允許Null:Y 
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
    /// 执行标记（Y：已执行 N：未执行） 資料型態:CHAR(1) 是否允許Null:Y 
    /// </summary>
    public String EXECUTEFLAG { 
      get { return (this["EXECUTEFLAG"] is DBNull) ? string.Empty : (String) this["EXECUTEFLAG"]; } 
      set { this["EXECUTEFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
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
        public new CST_EQP_WORKTIME_Row DeepCopy()
        {
            return (CST_EQP_WORKTIME_Row) base.DeepCopy();
        }
    }
}


