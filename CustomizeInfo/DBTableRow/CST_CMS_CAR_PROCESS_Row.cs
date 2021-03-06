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
    /// CST_CMS_CAR_PROCESS_Row, 
    /// </summary>
    [Serializable]
    public class CST_CMS_CAR_PROCESS_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_CMS_CAR_PROCESS Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_CMS_CAR_PROCESS.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_CMS_CAR_PROCESS_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_CMS_CAR_PROCESS_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_CMS_CAR_PROCESS_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_CMS_CAR_PROCESS_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_CMS_CAR_PROCESS_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String CMS_CAR_PROCESS_SID { 
      get { return (this["CMS_CAR_PROCESS_SID"] is DBNull) ? string.Empty : (String) this["CMS_CAR_PROCESS_SID"]; } 
      set { this["CMS_CAR_PROCESS_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(5) 是否允許Null:Y 
    /// </summary>
    public String CAR_SEQ { 
      get { return (this["CAR_SEQ"] is DBNull) ? string.Empty : (String) this["CAR_SEQ"]; } 
      set { this["CAR_SEQ"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String CAR_NO { 
      get { return (this["CAR_NO"] is DBNull) ? string.Empty : (String) this["CAR_NO"]; } 
      set { this["CAR_NO"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    }
    /// <summary>
    ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String LINE
    {
        get { return (this["LINE"] is DBNull) ? string.Empty : (String)this["LINE"]; }
        set { this["LINE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
    }

        /// <summary>
        ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String GROUP_SID
        {
            get { return (this["GROUP_SID"] is DBNull) ? string.Empty : (String)this["GROUP_SID"]; }
            set { this["GROUP_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        /// <summary>
        ///  資料型態:NVARCHAR2(5) 是否允許Null:Y 
        /// </summary>
        public String LOADLOT_FLAG { 
      get { return (this["LOADLOT_FLAG"] is DBNull) ? string.Empty : (String) this["LOADLOT_FLAG"]; } 
      set { this["LOADLOT_FLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
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
        public new CST_CMS_CAR_PROCESS_Row DeepCopy()
        {
            return (CST_CMS_CAR_PROCESS_Row) base.DeepCopy();
        }
    }
}


