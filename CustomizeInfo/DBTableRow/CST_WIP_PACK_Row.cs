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
    /// CST_WIP_PACK_Row, 包裝主檔
    /// </summary>
    [Serializable]
    public class CST_WIP_PACK_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_WIP_PACK Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_WIP_PACK.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_WIP_PACK_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_WIP_PACK_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_WIP_PACK_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_WIP_PACK_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_WIP_PACK_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 包裝主檔表SID 資料型態:NVARCHAR2(100) 是否允許Null:N 
    /// </summary>
    public String WIP_PACK_SID { 
      get { return (this["WIP_PACK_SID"] is DBNull) ? string.Empty : (String) this["WIP_PACK_SID"]; } 
      set { this["WIP_PACK_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 箱號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String BOXNO { 
      get { return (this["BOXNO"] is DBNull) ? string.Empty : (String) this["BOXNO"]; } 
      set { this["BOXNO"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 數量 資料型態:NUMBER(13) 是否允許Null:Y 
    /// </summary>
    public Decimal QUANTITY { 
      get { return (this["QUANTITY"] is DBNull) ? 0 : decimal.Parse(this["QUANTITY"].ToString()).FormatDecimal(); } 
      set { this["QUANTITY"] = value; } 
    } 

    /// <summary>
    /// 檢驗人員 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String INSPUSER { 
      get { return (this["INSPUSER"] is DBNull) ? string.Empty : (String) this["INSPUSER"]; } 
      set { this["INSPUSER"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 包裝人員 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String USERID { 
      get { return (this["USERID"] is DBNull) ? string.Empty : (String) this["USERID"]; } 
      set { this["USERID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 包裝時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String UPDATETIME { 
      get { return (this["UPDATETIME"] is DBNull) ? string.Empty : (String) this["UPDATETIME"]; } 
      set { this["UPDATETIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_WIP_PACK_Row DeepCopy()
        {
            return (CST_WIP_PACK_Row) base.DeepCopy();
        }
    }
}

