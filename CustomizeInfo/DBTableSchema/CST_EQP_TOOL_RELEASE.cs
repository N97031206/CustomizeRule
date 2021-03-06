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
  /// CST_EQP_TOOL_RELEASE table schema 機台刀具驗證成功資訊
  /// </summary>
  public class CST_EQP_TOOL_RELEASE : DBTableSchemaBase
  {
    /// <summary>
    /// 該類別的唯一實體, 供 Runtime 才能決定類別情況下使用
    /// </summary>
    public static CST_EQP_TOOL_RELEASE Singleton = new CST_EQP_TOOL_RELEASE();

    private static int s_NewRowCount = 0;

    #region Properties
    /// <summary>
    /// 此類別對應的資料表名稱。
    /// </summary>
    public static new string TableName { get { return Singleton.m_DataTable.TableName; } }

    /// <summary>
    /// 機台刀具驗證成功資訊表SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn EQP_TOOL_RELEASE_SID { get { return Singleton.Columns["EQP_TOOL_RELEASE_SID"]; } } 
    /// <summary>
    /// 機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn EQUIP { get { return Singleton.Columns["EQUIP"]; } } 
    /// <summary>
    /// 刀具 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn TOOL { get { return Singleton.Columns["TOOL"]; } } 
    /// <summary>
    /// 刀面 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn HEAD { get { return Singleton.Columns["HEAD"]; } } 
    /// <summary>
    /// 驗證成功序號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn RELEASEID { get { return Singleton.Columns["RELEASEID"]; } } 
    /// <summary>
    /// 使用者 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn USERID { get { return Singleton.Columns["USERID"]; } } 
    /// <summary>
    /// 更新時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
    /// </summary>
    public static DataColumn UPDATETIME { get { return Singleton.Columns["UPDATETIME"]; } } 

    #endregion Properties

    #region Ctor()
    
    private CST_EQP_TOOL_RELEASE()
    {
        // 不須鎖定 m_DataTable, 因為在這個時點, 不可能 有其他地方 同時存取 m_DataTable       
        
        base.m_DataTable.TableName = "CST_EQP_TOOL_RELEASE";
        
        base.SyncColumnsFromDB();
        // #TableColAddStatements
    }
    
    #endregion Ctor()
    
        /// <summary>
        /// 產生一個新的 Row 類別, 以便後續操作如填值等
        /// </summary>
        /// <returns>新的 Row 類別</returns>
        public static CST_EQP_TOOL_RELEASE_Row NewRow() { return (CST_EQP_TOOL_RELEASE_Row)Singleton.CreateRow(); }

        /// <summary>
        /// 產生一個新的 Row Array, 以便後續操作如填值等
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public override DBTableRowBase[] CreateRowArray(int length)
        {
            CST_EQP_TOOL_RELEASE_Row[] rowArray = new CST_EQP_TOOL_RELEASE_Row[length];
            for (int i = 0; i < rowArray.Length; ++i)
            {
                rowArray[i] = (CST_EQP_TOOL_RELEASE_Row)CreateRow();
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

            return new CST_EQP_TOOL_RELEASE_Row(row);
        }
                
        /// <summary>
        /// 執行傳入的 Sql, 取得符合的第一筆 Record.
        /// </summary>
        /// <param name="sqlAgent">Sql</param>
        /// <returns>Match Row</returns>
        public override DBTableRowBase GetOneRow(SqlAgent sqlAgent)
        {
            return this.GetOneRow<CST_EQP_TOOL_RELEASE_Row>(sqlAgent);
        }
      
        #region SelectRows()
       
        /// <summary>
        /// 取得所有欄位，沒有 Where 條件限制，無排序的資料；如果沒有資料則會回傳 Length 為 0 的 Array
        /// </summary>
        /// <returns>Row Array</returns>
        public static CST_EQP_TOOL_RELEASE_Row[] SelectRows()
        {
            return Singleton.GetRows<CST_EQP_TOOL_RELEASE_Row>(SelectStar.Singleton, null, null);
        }
  
        /// <summary>
        /// 根據 Where 條件, OrderBy 設定取得特定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="cols">指定欄位s</param>
        /// <returns>Row Array</returns>
        public static CST_EQP_TOOL_RELEASE_Row[] SelectRows(DataColumn[] cols)
        {
            return Singleton.GetRows<CST_EQP_TOOL_RELEASE_Row>(cols, null, null);
        }
      
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>Row Array</returns>
        public static CST_EQP_TOOL_RELEASE_Row[] SelectRows(SqlAgent sql) { return Singleton.GetRows<CST_EQP_TOOL_RELEASE_Row>(sql, "", null); }
              
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="rowFilter">RowFilter</param>
        /// <returns>Row Array</returns>
        public static CST_EQP_TOOL_RELEASE_Row[] SelectRows(SqlAgent sql, string rowFilter) { return Singleton.GetRows<CST_EQP_TOOL_RELEASE_Row>(sql, rowFilter, null); }
       
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="dataAgent">協助查詢的物件</param>
        /// <returns>Row Array</returns>
        public static CST_EQP_TOOL_RELEASE_Row[] SelectRows(SqlAgent sql, CustomDataAgent dataAgent) { return Singleton.GetRows<CST_EQP_TOOL_RELEASE_Row>(sql, "", dataAgent); }
            
        /// <summary>
        /// 根據 SQL 取得指定欄位的資料，回傳 XXX_Row[]，
        /// 如果沒有資料則會回傳 Length 為 0 的 Array。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="rowFilter">RowFilter</param>
        /// <param name="dataAgent">協助查詢的物件</param>
        /// <returns>Row Array</returns>
        public static CST_EQP_TOOL_RELEASE_Row[] SelectRows(SqlAgent sql, string rowFilter, CustomDataAgent dataAgent) { return Singleton.GetRows<CST_EQP_TOOL_RELEASE_Row>(sql, rowFilter, dataAgent); }

        #endregion SelectRows()
           

    }  // end class
}  // end namespace
