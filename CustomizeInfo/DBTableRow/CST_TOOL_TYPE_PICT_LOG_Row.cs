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
    /// CST_TOOL_TYPE_PICT_LOG_Row, 
    /// </summary>
    [Serializable]
    public class CST_TOOL_TYPE_PICT_LOG_Row : DBTableRowBase
    {
        /// <summary>
        /// 取得該 Row 是屬於 CST_TOOL_TYPE_PICT_LOG Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_TOOL_TYPE_PICT_LOG.Singleton; }
        }

        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_TOOL_TYPE_PICT_LOG_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_TOOL_TYPE_PICT_LOG_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_TOOL_TYPE_PICT_LOG_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_TOOL_TYPE_PICT_LOG_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_TOOL_TYPE_PICT_LOG_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TOOL_TYPE_PICT_HIST_SID { 
      get { return (this["TOOL_TYPE_PICT_HIST_SID"] is DBNull) ? string.Empty : (String) this["TOOL_TYPE_PICT_HIST_SID"]; } 
      set { this["TOOL_TYPE_PICT_HIST_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TOOL_TYPE_PICT_SID { 
      get { return (this["TOOL_TYPE_PICT_SID"] is DBNull) ? string.Empty : (String) this["TOOL_TYPE_PICT_SID"]; } 
      set { this["TOOL_TYPE_PICT_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TOOLTYPE { 
      get { return (this["TOOLTYPE"] is DBNull) ? string.Empty : (String) this["TOOLTYPE"]; } 
      set { this["TOOLTYPE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String FILENAME { 
      get { return (this["FILENAME"] is DBNull) ? string.Empty : (String) this["FILENAME"]; } 
      set { this["FILENAME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String USERID { 
      get { return (this["USERID"] is DBNull) ? string.Empty : (String) this["USERID"]; } 
      set { this["USERID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String UPDATETIME { 
      get { return (this["UPDATETIME"] is DBNull) ? string.Empty : (String) this["UPDATETIME"]; } 
      set { this["UPDATETIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_TOOL_TYPE_PICT_LOG_Row DeepCopy()
        {
            return (CST_TOOL_TYPE_PICT_LOG_Row) base.DeepCopy();
        }
    }
}

