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
using System.Text;
using System.Data;

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.DbTableRow;

namespace Ares.Cimes.IntelliService.DbTableSchema
{
  /// <summary>
  /// CST_WIP_CMM table schema 
  /// </summary>
  public class CST_WIP_CMM : DBTableSchemaBase
  {
    /// <summary>
    /// 該類別的唯一實體, 供 Runtime 才能決定類別情況下使用
    /// </summary>
    public static CST_WIP_CMM Singleton = new CST_WIP_CMM();

    private static int s_NewRowCount = 0;

    #region Properties
    /// <summary>
    /// 此類別對應的資料表名稱。
    /// </summary>
    public static new string TableName { get { return Singleton.m_DataTable.TableName; } }

    /// <summary>
    /// 三次元主檔系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn WIP_CMM_SID { get { return Singleton.Columns["WIP_CMM_SID"]; } } 
    /// <summary>
    /// 加工機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn EQUIPMENT { get { return Singleton.Columns["EQUIPMENT"]; } } 
    /// <summary>
    /// 產品名稱 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn DEVICE { get { return Singleton.Columns["DEVICE"]; } } 
    /// <summary>
    /// 班別 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn SHIFT { get { return Singleton.Columns["SHIFT"]; } } 
    /// <summary>
    /// 量測機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn MEASUREEQP { get { return Singleton.Columns["MEASUREEQP"]; } } 
    /// <summary>
    /// 檢驗員 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn INSPUSER { get { return Singleton.Columns["INSPUSER"]; } } 
    /// <summary>
    /// 工件號或是小工單號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn FILEID { get { return Singleton.Columns["FILEID"]; } } 
    /// <summary>
    /// 解析時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn PARSETIME { get { return Singleton.Columns["PARSETIME"]; } } 
    /// <summary>
    /// 配對到的檢驗單號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn QC_INSP_SID { get { return Singleton.Columns["QC_INSP_SID"]; } } 
    /// <summary>
    /// 更新時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn UPDATETIME { get { return Singleton.Columns["UPDATETIME"]; } } 

    #endregion Properties

    #region Ctor()
    
    private CST_WIP_CMM()
    {
        // 不須鎖定 m_DataTable, 因為在這個時點, 不可能 有其他地方 同時存取 m_DataTable       
        
        base.m_DataTable.TableName = "CST_WIP_CMM";
        
        base.SyncColumnsFromDB();
        // #TableColAddStatements
    }
    
    #endregion Ctor()
    
        /// <summary>
        /// 產生一個新的 Row 類別, 以便後續操作如填值等
        /// </summary>
        /// <returns>新的 Row 類別</returns>
        public static CST_WIP_CMM_Row NewRow() { return (CST_WIP_CMM_Row)Singleton.CreateRow(); }

        /// <summary>
        /// 產生一個新的 Row Array, 以便後續操作如填值等
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public override DBTableRowBase[] CreateRowArray(int length)
        {
            CST_WIP_CMM_Row[] rowArray = new CST_WIP_CMM_Row[length];
            for (int i = 0; i < rowArray.Length; ++i)
            {
                rowArray[i] = (CST_WIP_CMM_Row)CreateRow();
            }
            return rowArray;
        }
        
        /// <summary>
        /// 無條件取得指定欄位的資料並轉成一個指定型別的陣列
        /// </summary>
        /// <param name="col">指定欄位</param>
        /// <returns>Array</returns>
        public static T[] SelectArray<T>(DataColumn col) { return Singleton.GetArray<T>(col, null, null); }
             
       
        /// <summary>
        /// 產生一個新的 Row 類別, 以便後續操作如填值等
        /// </summary>
        /// <returns></returns>
        public override DBTableRowBase CreateRow() 
        {
            DataRow row = NewRow(ref s_NewRowCount);

            return new CST_WIP_CMM_Row(row);
        }
                
        /// <summary>
        /// 執行傳入的 Sql, 取得符合的第一筆 Record.
        /// </summary>
        /// <param name="sqlAgent">Sql</param>
        /// <returns>Match Row</returns>
        public override DBTableRowBase GetOneRow(SqlAgent sqlAgent)
        {
            return this.GetOneRow<CST_WIP_CMM_Row>(sqlAgent);
        }
      
        #region SelectRows()
       
        /// <summary>
        /// 取得所有欄位，沒有 Where 條件限制，無排序的資料；如果沒有資料則會回傳 Length 為 0 的 Array
        /// </summary>
        /// <returns>Row Array</returns>
        public static CST_WIP_CMM_Row[] SelectRows()
        {
            return Singleton.GetRows<CST_WIP_CMM_Row>(SelectStar.Singleton, null, null);
        }
  
        /// <summary>
        /// 根據 Where 條件, OrderBy 設定取得特定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="cols">指定欄位s</param>
        /// <returns>Row Array</returns>
        public static CST_WIP_CMM_Row[] SelectRows(DataColumn[] cols)
        {
            return Singleton.GetRows<CST_WIP_CMM_Row>(cols, null, null);
        }
      
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>Row Array</returns>
        public static CST_WIP_CMM_Row[] SelectRows(SqlAgent sql) { return Singleton.GetRows<CST_WIP_CMM_Row>(sql, "", null); }
              
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="rowFilter">RowFilter</param>
        /// <returns>Row Array</returns>
        public static CST_WIP_CMM_Row[] SelectRows(SqlAgent sql, string rowFilter) { return Singleton.GetRows<CST_WIP_CMM_Row>(sql, rowFilter, null); }
       
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="dataAgent">協助查詢的物件</param>
        /// <returns>Row Array</returns>
        public static CST_WIP_CMM_Row[] SelectRows(SqlAgent sql, CustomDataAgent dataAgent) { return Singleton.GetRows<CST_WIP_CMM_Row>(sql, "", dataAgent); }
            
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="rowFilter">RowFilter</param>
        /// <param name="dataAgent">協助查詢的物件</param>
        /// <returns>Row Array</returns>
        public static CST_WIP_CMM_Row[] SelectRows(SqlAgent sql, string rowFilter, CustomDataAgent dataAgent) { return Singleton.GetRows<CST_WIP_CMM_Row>(sql, rowFilter, dataAgent); }

        #endregion SelectRows()
           

    }  // end class
}  // end namespace
