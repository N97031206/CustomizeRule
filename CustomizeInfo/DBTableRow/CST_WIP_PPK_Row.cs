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
    /// CST_WIP_PPK_Row, 
    /// </summary>
    [Serializable]
    public class CST_WIP_PPK_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_WIP_PPK Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_WIP_PPK.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_WIP_PPK_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_WIP_PPK_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_WIP_PPK_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_WIP_PPK_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_WIP_PPK_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// PPK資訊系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String WIP_PPK_SID { 
      get { return (this["WIP_PPK_SID"] is DBNull) ? string.Empty : (String) this["WIP_PPK_SID"]; } 
      set { this["WIP_PPK_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String EQP { 
      get { return (this["EQP"] is DBNull) ? string.Empty : (String) this["EQP"]; } 
      set { this["EQP"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 料號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String DEVICE { 
      get { return (this["DEVICE"] is DBNull) ? string.Empty : (String) this["DEVICE"]; } 
      set { this["DEVICE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 狀態 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String STATUS { 
      get { return (this["STATUS"] is DBNull) ? string.Empty : (String) this["STATUS"]; } 
      set { this["STATUS"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// PPK送驗支數 資料型態:NUMBER() 是否允許Null:Y 
    /// </summary>
    public Decimal PPKCOUNT { 
      get { return (this["PPKCOUNT"] is DBNull) ? 0 : decimal.Parse(this["PPKCOUNT"].ToString()).FormatDecimal(); } 
      set { this["PPKCOUNT"] = value; } 
    } 


        
        /// <summary>
        /// 取得該 Row 的深層複製實體
        /// </summary>              
        /// <returns>深層複製實體</returns>
        public new CST_WIP_PPK_Row DeepCopy()
        {
            return (CST_WIP_PPK_Row) base.DeepCopy();
        }
    }
}


