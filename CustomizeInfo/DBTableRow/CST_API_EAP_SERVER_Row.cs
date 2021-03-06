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
    /// CST_API_EAP_SERVER_Row, 
    /// </summary>
    [Serializable]
    public class CST_API_EAP_SERVER_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_API_EAP_SERVER Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_API_EAP_SERVER.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_API_EAP_SERVER_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_API_EAP_SERVER_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_API_EAP_SERVER_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_API_EAP_SERVER_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_API_EAP_SERVER_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// EAP設定系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String API_EAP_SERVER { 
      get { return (this["API_EAP_SERVER"] is DBNull) ? string.Empty : (String) this["API_EAP_SERVER"]; } 
      set { this["API_EAP_SERVER"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// EAP SERVER名稱 資料型態:NVARCHAR2(100) 是否允許Null:N 
    /// </summary>
    public String EAP_NAME { 
      get { return (this["EAP_NAME"] is DBNull) ? string.Empty : (String) this["EAP_NAME"]; } 
      set { this["EAP_NAME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// EAP SERVER的IP 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String IP { 
      get { return (this["IP"] is DBNull) ? string.Empty : (String) this["IP"]; } 
      set { this["IP"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// EAP SERVER的Port 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String PORT { 
      get { return (this["PORT"] is DBNull) ? string.Empty : (String) this["PORT"]; } 
      set { this["PORT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    }

    /// <summary>
    /// EAP SERVER的Line 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LINE
    {
        get { return (this["LINE"] is DBNull) ? string.Empty : (String)this["LINE"]; }
        set { this["LINE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
    }




        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_API_EAP_SERVER_Row DeepCopy()
        {
            return (CST_API_EAP_SERVER_Row) base.DeepCopy();
        }
    }
}


