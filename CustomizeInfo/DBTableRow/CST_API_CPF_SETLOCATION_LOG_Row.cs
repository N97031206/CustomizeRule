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
    /// CST_API_CPF_SETLOCATION_LOG_Row, 
    /// </summary>
    [Serializable]
    public class CST_API_CPF_SETLOCATION_LOG_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_API_CPF_SETLOCATION_LOG Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_API_CPF_SETLOCATION_LOG.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_API_CPF_SETLOCATION_LOG_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_API_CPF_SETLOCATION_LOG_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_API_CPF_SETLOCATION_LOG_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_API_CPF_SETLOCATION_LOG_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_API_CPF_SETLOCATION_LOG_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String API_CPF_SETLOCATION_LOG_SID { 
      get { return (this["API_CPF_SETLOCATION_LOG_SID"] is DBNull) ? string.Empty : (String) this["API_CPF_SETLOCATION_LOG_SID"]; } 
      set { this["API_CPF_SETLOCATION_LOG_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 工件序號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String COMPLOT { 
      get { return (this["COMPLOT"] is DBNull) ? string.Empty : (String) this["COMPLOT"]; } 
      set { this["COMPLOT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 來源位置 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String FROM_LOCATION { 
      get { return (this["FROM_LOCATION"] is DBNull) ? string.Empty : (String) this["FROM_LOCATION"]; } 
      set { this["FROM_LOCATION"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 目的位置 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TO_LOCATION { 
      get { return (this["TO_LOCATION"] is DBNull) ? string.Empty : (String) this["TO_LOCATION"]; } 
      set { this["TO_LOCATION"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 使用者 資料型態:NVARCHAR2(100) 是否允許Null:Y 
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
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_API_CPF_SETLOCATION_LOG_Row DeepCopy()
        {
            return (CST_API_CPF_SETLOCATION_LOG_Row) base.DeepCopy();
        }
    }
}


