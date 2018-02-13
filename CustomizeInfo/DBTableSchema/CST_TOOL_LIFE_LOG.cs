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
  /// CST_TOOL_LIFE_LOG table schema 刀具每面壽命使用狀態紀錄
  /// </summary>
  public class CST_TOOL_LIFE_LOG : DBTableSchemaBase
  {
    /// <summary>
    /// 該類別的唯一實體, 供 Runtime 才能決定類別情況下使用
    /// </summary>
    public static CST_TOOL_LIFE_LOG Singleton = new CST_TOOL_LIFE_LOG();

    private static int s_NewRowCount = 0;

    #region Properties
    /// <summary>
    /// 此類別對應的資料表名稱。
    /// </summary>
    public static new string TableName { get { return Singleton.m_DataTable.TableName; } }

    /// <summary>
    /// 刀具每面壽命使用狀態紀錄表SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn TOOL_LIFE_LOG_SID { get { return Singleton.Columns["TOOL_LIFE_LOG_SID"]; } } 
    /// <summary>
    /// 刀具每面壽命使用狀態表SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn TOOL_LIFE_SID { get { return Singleton.Columns["TOOL_LIFE_SID"]; } } 
    /// <summary>
    /// 刀具名稱 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn TOOLNAME { get { return Singleton.Columns["TOOLNAME"]; } } 
    /// <summary>
    /// 刀面 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn HEAD { get { return Singleton.Columns["HEAD"]; } } 
    /// <summary>
    /// 刀具壽命 資料型態:NUMBER() 是否允許Null:Y 
    /// </summary>
    public static DataColumn LIFE { get { return Singleton.Columns["LIFE"]; } } 
    /// <summary>
    /// 使用次數 資料型態:NUMBER() 是否允許Null:Y 
    /// </summary>
    public static DataColumn USECOUNT { get { return Singleton.Columns["USECOUNT"]; } } 
    /// <summary>
    /// 使用者 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn USERID { get { return Singleton.Columns["USERID"]; } } 
    /// <summary>
    /// 更新時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn UPDATETIME { get { return Singleton.Columns["UPDATETIME"]; } } 
    /// <summary>
    /// 交易範疇識別碼 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn TXNSCOPEUID { get { return Singleton.Columns["TXNSCOPEUID"]; } } 
    /// <summary>
    /// 交易行為 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn ACTIONTYPE { get { return Singleton.Columns["ACTIONTYPE"]; } } 
    /// <summary>
    /// 執行的應用程式名稱 (Rule路徑+ClientIP: SessionID) 資料型態:NVARCHAR2(2000) 是否允許Null:Y 
    /// </summary>
    public static DataColumn APPLICATIONNAME { get { return Singleton.Columns["APPLICATIONNAME"]; } } 

    #endregion Properties

    #region Ctor()
    
    private CST_TOOL_LIFE_LOG()
    {
        // 不須鎖定 m_DataTable, 因為在這個時點, 不可能 有其他地方 同時存取 m_DataTable       
        
        base.m_DataTable.TableName = "CST_TOOL_LIFE_LOG";
        
        base.SyncColumnsFromDB();
        // #TableColAddStatements
    }
    
    #endregion Ctor()
    
        /// <summary>
        /// 產生一個新的 Row 類別, 以便後續操作如填值等
        /// </summary>
        /// <returns>新的 Row 類別</returns>
        public static CST_TOOL_LIFE_LOG_Row NewRow() { return (CST_TOOL_LIFE_LOG_Row)Singleton.CreateRow(); }

        /// <summary>
        /// 產生一個新的 Row Array, 以便後續操作如填值等
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public override DBTableRowBase[] CreateRowArray(int length)
        {
            CST_TOOL_LIFE_LOG_Row[] rowArray = new CST_TOOL_LIFE_LOG_Row[length];
            for (int i = 0; i < rowArray.Length; ++i)
            {
                rowArray[i] = (CST_TOOL_LIFE_LOG_Row)CreateRow();
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

            return new CST_TOOL_LIFE_LOG_Row(row);
        }
                
        /// <summary>
        /// 執行傳入的 Sql, 取得符合的第一筆 Record.
        /// </summary>
        /// <param name="sqlAgent">Sql</param>
        /// <returns>Match Row</returns>
        public override DBTableRowBase GetOneRow(SqlAgent sqlAgent)
        {
            return this.GetOneRow<CST_TOOL_LIFE_LOG_Row>(sqlAgent);
        }
      
        #region SelectRows()
       
        /// <summary>
        /// 取得所有欄位，沒有 Where 條件限制，無排序的資料；如果沒有資料則會回傳 Length 為 0 的 Array
        /// </summary>
        /// <returns>Row Array</returns>
        public static CST_TOOL_LIFE_LOG_Row[] SelectRows()
        {
            return Singleton.GetRows<CST_TOOL_LIFE_LOG_Row>(SelectStar.Singleton, null, null);
        }
  
        /// <summary>
        /// 根據 Where 條件, OrderBy 設定取得特定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="cols">指定欄位s</param>
        /// <returns>Row Array</returns>
        public static CST_TOOL_LIFE_LOG_Row[] SelectRows(DataColumn[] cols)
        {
            return Singleton.GetRows<CST_TOOL_LIFE_LOG_Row>(cols, null, null);
        }
      
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>Row Array</returns>
        public static CST_TOOL_LIFE_LOG_Row[] SelectRows(SqlAgent sql) { return Singleton.GetRows<CST_TOOL_LIFE_LOG_Row>(sql, "", null); }
              
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="rowFilter">RowFilter</param>
        /// <returns>Row Array</returns>
        public static CST_TOOL_LIFE_LOG_Row[] SelectRows(SqlAgent sql, string rowFilter) { return Singleton.GetRows<CST_TOOL_LIFE_LOG_Row>(sql, rowFilter, null); }
       
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="dataAgent">協助查詢的物件</param>
        /// <returns>Row Array</returns>
        public static CST_TOOL_LIFE_LOG_Row[] SelectRows(SqlAgent sql, CustomDataAgent dataAgent) { return Singleton.GetRows<CST_TOOL_LIFE_LOG_Row>(sql, "", dataAgent); }
            
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="rowFilter">RowFilter</param>
        /// <param name="dataAgent">協助查詢的物件</param>
        /// <returns>Row Array</returns>
        public static CST_TOOL_LIFE_LOG_Row[] SelectRows(SqlAgent sql, string rowFilter, CustomDataAgent dataAgent) { return Singleton.GetRows<CST_TOOL_LIFE_LOG_Row>(sql, rowFilter, dataAgent); }

        #endregion SelectRows()
           

    }  // end class
}  // end namespace
