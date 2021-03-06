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
    /// CST_WIP_CMM_Row, 
    /// </summary>
    [Serializable]
    public class CST_WIP_CMM_Row : DBTableRowBase
    {            
        /// <summary>
        /// 取得該 Row 是屬於 CST_WIP_CMM Table, 傳回代表該 Table Schema 的唯一實體
        /// </summary>
        public override DBTableSchemaBase TableSchema
        {
            get { return CST_WIP_CMM.Singleton; }
        }
    
        #region Ctor()
        /// <summary>
        /// 建構一個空的 CST_WIP_CMM_Row 物件.
        /// 此 Ctor 僅供 Cimes 內部 Generator 使用, 若要取得一 Row 實體, 請使用 Schema.NewRow()
        /// </summary>
        public CST_WIP_CMM_Row() : base() {  }

        /// <summary>
        /// 以原生 DataRow 建構一個 CST_WIP_CMM_Row 物件
        /// </summary>
        /// <param name="row">原生 DataRow</param>
        public CST_WIP_CMM_Row(DataRow row) : base(row) {  }
        
        /// <summary>
        /// For Serializable Operation
        /// </summary>
        /// <param name="si">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected CST_WIP_CMM_Row(SerializationInfo si, StreamingContext context) : base(si, context) {  }
        #endregion

                    /// <summary>
    /// 三次元主檔系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String WIP_CMM_SID { 
      get { return (this["WIP_CMM_SID"] is DBNull) ? string.Empty : (String) this["WIP_CMM_SID"]; } 
      set { this["WIP_CMM_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 加工機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String EQUIPMENT { 
      get { return (this["EQUIPMENT"] is DBNull) ? string.Empty : (String) this["EQUIPMENT"]; } 
      set { this["EQUIPMENT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 產品名稱 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String DEVICE { 
      get { return (this["DEVICE"] is DBNull) ? string.Empty : (String) this["DEVICE"]; } 
      set { this["DEVICE"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 班別 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String SHIFT { 
      get { return (this["SHIFT"] is DBNull) ? string.Empty : (String) this["SHIFT"]; } 
      set { this["SHIFT"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 量測機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String MEASUREEQP { 
      get { return (this["MEASUREEQP"] is DBNull) ? string.Empty : (String) this["MEASUREEQP"]; } 
      set { this["MEASUREEQP"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 檢驗員 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String INSPUSER { 
      get { return (this["INSPUSER"] is DBNull) ? string.Empty : (String) this["INSPUSER"]; } 
      set { this["INSPUSER"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 工件號或是小工單號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String FILEID { 
      get { return (this["FILEID"] is DBNull) ? string.Empty : (String) this["FILEID"]; } 
      set { this["FILEID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 解析時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String PARSETIME { 
      get { return (this["PARSETIME"] is DBNull) ? string.Empty : (String) this["PARSETIME"]; } 
      set { this["PARSETIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
    } 

    /// <summary>
    /// 配對到的檢驗單號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public String QC_INSP_SID { 
      get { return (this["QC_INSP_SID"] is DBNull) ? string.Empty : (String) this["QC_INSP_SID"]; } 
      set { this["QC_INSP_SID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object) value; } 
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
        public new CST_WIP_CMM_Row DeepCopy()
        {
            return (CST_WIP_CMM_Row) base.DeepCopy();
        }
    }
}


