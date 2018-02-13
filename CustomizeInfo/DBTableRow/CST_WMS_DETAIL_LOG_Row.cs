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
    /// CST_WMS_DETAIL_LOG_Row, 
    /// </summary>
    [Serializable]
    public class CST_WMS_DETAIL_LOG_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_WMS_DETAIL_LOG Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_WMS_DETAIL_LOG.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_WMS_DETAIL_LOG_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_WMS_DETAIL_LOG_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_WMS_DETAIL_LOG_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_WMS_DETAIL_LOG_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_WMS_DETAIL_LOG_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 入庫明細領用紀錄系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String WMS_DETAIL_LOG_SID { 
      get { return (this["WMS_DETAIL_LOG_SID"] is DBNull) ? string.Empty : (String) this["WMS_DETAIL_LOG_SID"]; } 
      set { this["WMS_DETAIL_LOG_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 入庫明細系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String WMS_DETAIL_SID { 
      get { return (this["WMS_DETAIL_SID"] is DBNull) ? string.Empty : (String) this["WMS_DETAIL_SID"]; } 
      set { this["WMS_DETAIL_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 入庫批號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LOT { 
      get { return (this["LOT"] is DBNull) ? string.Empty : (String) this["LOT"]; } 
      set { this["LOT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 入庫序號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String COMPONENTID { 
      get { return (this["COMPONENTID"] is DBNull) ? string.Empty : (String) this["COMPONENTID"]; } 
      set { this["COMPONENTID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 數量 資料型態:NUMBER(13) 是否允許Null:Y 
    /// </summary>
    public Decimal QUANTITY { 
      get { return (this["QUANTITY"] is DBNull) ? 0 : decimal.Parse(this["QUANTITY"].ToString()).FormatDecimal(); } 
      set { this["QUANTITY"] = value; } 
    } 

    /// <summary>
    /// 主檔與明細的關聯編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LINKSID { 
      get { return (this["LINKSID"] is DBNull) ? string.Empty : (String) this["LINKSID"]; } 
      set { this["LINKSID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_WMS_DETAIL_LOG_Row DeepCopy()
        {
            return (CST_WMS_DETAIL_LOG_Row) base.DeepCopy();
        }
    }
}


