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
    /// CST_CMS_TEMP_TIME_Row, 收集載具通過T4T6溫度時間資料檔
    /// </summary>
    [Serializable]
    public class CST_CMS_TEMP_TIME_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_CMS_TEMP_TIME Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_CMS_TEMP_TIME.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_CMS_TEMP_TIME_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_CMS_TEMP_TIME_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_CMS_TEMP_TIME_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_CMS_TEMP_TIME_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_CMS_TEMP_TIME_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 收集載具通過T4T6溫度時間的邊號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String CMS_TEMP_TIME_SID { 
      get { return (this["CMS_TEMP_TIME_SID"] is DBNull) ? string.Empty : (String) this["CMS_TEMP_TIME_SID"]; } 
      set { this["CMS_TEMP_TIME_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 載具批號系統邊號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String CMS_LOT_SID { 
      get { return (this["CMS_LOT_SID"] is DBNull) ? string.Empty : (String) this["CMS_LOT_SID"]; } 
      set { this["CMS_LOT_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 倒數第1隻的溫度 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TEMP1 { 
      get { return (this["TEMP1"] is DBNull) ? string.Empty : (String) this["TEMP1"]; } 
      set { this["TEMP1"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 倒數第2隻的溫度 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TEMP2 { 
      get { return (this["TEMP2"] is DBNull) ? string.Empty : (String) this["TEMP2"]; } 
      set { this["TEMP2"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 水溫 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TEMP_WATER { 
      get { return (this["TEMP_WATER"] is DBNull) ? string.Empty : (String) this["TEMP_WATER"]; } 
      set { this["TEMP_WATER"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 高熱爐開門到下水時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TRANSPORT_TIME { 
      get { return (this["TRANSPORT_TIME"] is DBNull) ? string.Empty : (String) this["TRANSPORT_TIME"]; } 
      set { this["TRANSPORT_TIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 浸水時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String IN_WATER_TIME { 
      get { return (this["IN_WATER_TIME"] is DBNull) ? string.Empty : (String) this["IN_WATER_TIME"]; } 
      set { this["IN_WATER_TIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 最後更新時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String UPDATETIME { 
      get { return (this["UPDATETIME"] is DBNull) ? string.Empty : (String) this["UPDATETIME"]; } 
      set { this["UPDATETIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 使用者 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String USERID { 
      get { return (this["USERID"] is DBNull) ? string.Empty : (String) this["USERID"]; } 
      set { this["USERID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_CMS_TEMP_TIME_Row DeepCopy()
        {
            return (CST_CMS_TEMP_TIME_Row) base.DeepCopy();
        }
    }
}


