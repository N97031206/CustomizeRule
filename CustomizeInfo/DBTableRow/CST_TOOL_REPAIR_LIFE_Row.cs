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
    /// CST_TOOL_REPAIR_LIFE_Row, 刀具維修時壽命表
    /// </summary>
    [Serializable]
    public class CST_TOOL_REPAIR_LIFE_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_TOOL_REPAIR_LIFE Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_TOOL_REPAIR_LIFE.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_TOOL_REPAIR_LIFE_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_TOOL_REPAIR_LIFE_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_TOOL_REPAIR_LIFE_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_TOOL_REPAIR_LIFE_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_TOOL_REPAIR_LIFE_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 刀具維修時壽命表SID 資料型態:NVARCHAR2(100) 是否允許Null:N 
    /// </summary>
    public String TOOL_REPAIR_LIFE_SID { 
      get { return (this["TOOL_REPAIR_LIFE_SID"] is DBNull) ? string.Empty : (String) this["TOOL_REPAIR_LIFE_SID"]; } 
      set { this["TOOL_REPAIR_LIFE_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 紀錄維修回廠資訊表ID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TOOL_REPAIR_SID { 
      get { return (this["TOOL_REPAIR_SID"] is DBNull) ? string.Empty : (String) this["TOOL_REPAIR_SID"]; } 
      set { this["TOOL_REPAIR_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 刀面 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String HEAD { 
      get { return (this["HEAD"] is DBNull) ? string.Empty : (String) this["HEAD"]; } 
      set { this["HEAD"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 使用壽命 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LIFE { 
      get { return (this["LIFE"] is DBNull) ? string.Empty : (String) this["LIFE"]; } 
      set { this["LIFE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_TOOL_REPAIR_LIFE_Row DeepCopy()
        {
            return (CST_TOOL_REPAIR_LIFE_Row) base.DeepCopy();
        }
    }
}


