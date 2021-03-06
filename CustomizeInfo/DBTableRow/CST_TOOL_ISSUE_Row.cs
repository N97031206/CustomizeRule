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
    /// CST_TOOL_ISSUE_Row, 刀具領用表
    /// </summary>
    [Serializable]
    public class CST_TOOL_ISSUE_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_TOOL_ISSUE Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_TOOL_ISSUE.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_TOOL_ISSUE_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_TOOL_ISSUE_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_TOOL_ISSUE_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_TOOL_ISSUE_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_TOOL_ISSUE_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 刀具領用表SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String TOOL_ISSUE_SID { 
      get { return (this["TOOL_ISSUE_SID"] is DBNull) ? string.Empty : (String) this["TOOL_ISSUE_SID"]; } 
      set { this["TOOL_ISSUE_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 領用總數量 資料型態:NUMBER(38) 是否允許Null:Y 
    /// </summary>
    public Decimal TOTALQTY { 
      get { return (this["TOTALQTY"] is DBNull) ? 0 : decimal.Parse(this["TOTALQTY"].ToString()).FormatDecimal(); } 
      set { this["TOTALQTY"] = value; } 
    } 

    /// <summary>
    /// 領用原因 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String REASON { 
      get { return (this["REASON"] is DBNull) ? string.Empty : (String) this["REASON"]; } 
      set { this["REASON"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 領用說明 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String DESCR { 
      get { return (this["DESCR"] is DBNull) ? string.Empty : (String) this["DESCR"]; } 
      set { this["DESCR"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 領用機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String EQP { 
      get { return (this["EQP"] is DBNull) ? string.Empty : (String) this["EQP"]; } 
      set { this["EQP"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
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
    /// 交易繫結SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LINKSID { 
      get { return (this["LINKSID"] is DBNull) ? string.Empty : (String) this["LINKSID"]; } 
      set { this["LINKSID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 交易動作 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String ACTION { 
      get { return (this["ACTION"] is DBNull) ? string.Empty : (String) this["ACTION"]; } 
      set { this["ACTION"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_TOOL_ISSUE_Row DeepCopy()
        {
            return (CST_TOOL_ISSUE_Row) base.DeepCopy();
        }
    }
}


