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
    /// CST_EDC_COMP_Row, 
    /// </summary>
    [Serializable]
    public class CST_EDC_COMP_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_EDC_COMP Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_EDC_COMP.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_EDC_COMP_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_EDC_COMP_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_EDC_COMP_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_EDC_COMP_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_EDC_COMP_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:N 
    /// </summary>
    public String EDC_COMP_SID { 
      get { return (this["EDC_COMP_SID"] is DBNull) ? string.Empty : (String) this["EDC_COMP_SID"]; } 
      set { this["EDC_COMP_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LOT { 
      get { return (this["LOT"] is DBNull) ? string.Empty : (String) this["LOT"]; } 
      set { this["LOT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String COMPONENTID { 
      get { return (this["COMPONENTID"] is DBNull) ? string.Empty : (String) this["COMPONENTID"]; } 
      set { this["COMPONENTID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NUMBER() 是否允許Null:Y 
    /// </summary>
    public Decimal DATA { 
      get { return (this["DATA"] is DBNull) ? 0 : decimal.Parse(this["DATA"].ToString()).FormatDecimal(); } 
      set { this["DATA"] = value; } 
    } 

    /// <summary>
    ///  資料型態:NUMBER() 是否允許Null:Y 
    /// </summary>
    public Decimal UPSPEC { 
      get { return (this["UPSPEC"] is DBNull) ? 0 : decimal.Parse(this["UPSPEC"].ToString()).FormatDecimal(); } 
      set { this["UPSPEC"] = value; } 
    } 

    /// <summary>
    ///  資料型態:NUMBER() 是否允許Null:Y 
    /// </summary>
    public Decimal LOWSPEC { 
      get { return (this["LOWSPEC"] is DBNull) ? 0 : decimal.Parse(this["LOWSPEC"].ToString()).FormatDecimal(); } 
      set { this["LOWSPEC"] = value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String INSPEC { 
      get { return (this["INSPEC"] is DBNull) ? string.Empty : (String) this["INSPEC"]; } 
      set { this["INSPEC"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    }

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LINKSID
    {
        get { return (this["LINKSID"] is DBNull) ? string.Empty : (String)this["LINKSID"]; }
        set { this["LINKSID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
    }

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String UPDATETIME { 
      get { return (this["UPDATETIME"] is DBNull) ? string.Empty : (String) this["UPDATETIME"]; } 
      set { this["UPDATETIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String USERID { 
      get { return (this["USERID"] is DBNull) ? string.Empty : (String) this["USERID"]; } 
      set { this["USERID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_EDC_COMP_Row DeepCopy()
        {
            return (CST_EDC_COMP_Row) base.DeepCopy();
        }
    }
}


