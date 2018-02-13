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


using Ares.Cimes.IntelliService.DbTableRow;
using Ares.Cimes.IntelliService.DbTableSchema;

namespace Ares.Cimes.IntelliService.Info
{
    /// <summary>
    /// IQC資料匯入LOG表 (Mapping CST_MMS_IQC_INFO_LOG Table)
    /// </summary>
    [Serializable]
    public partial class CSTMMSIqcInfoLog : InfoBase 
    {
        private static readonly CSTMMSIqcInfoLog s_ProxyInfo;
        
        
        
        private CST_MMS_IQC_INFO_LOG_Row m_StrongTypeRow { get { return (CST_MMS_IQC_INFO_LOG_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_MMS_IQC_INFO_LOG.MMS_IQC_INFO_LOG_SID; } }
        
                /// <summary>
        /// IQC資訊LOG紀錄系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String MMS_IQC_INFO_LOG_SID 
        {
            get { return m_StrongTypeRow.MMS_IQC_INFO_LOG_SID; }
            set { m_StrongTypeRow.MMS_IQC_INFO_LOG_SID = value; }
        }
        /// <summary>
        /// IQC資訊系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String MMS_IQC_INFO_SID 
        {
            get { return m_StrongTypeRow.MMS_IQC_INFO_SID; }
            set { m_StrongTypeRow.MMS_IQC_INFO_SID = value; }
        }
        /// <summary>
        /// 物料號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String MaterialNO 
        {
            get { return m_StrongTypeRow.MATERIALNO; }
            set { m_StrongTypeRow.MATERIALNO = value; }
        }
        /// <summary>
        /// 來料爐號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String RUNID 
        {
            get { return m_StrongTypeRow.RUNID; }
            set { m_StrongTypeRow.RUNID = value; }
        }
        /// <summary>
        /// 來料批號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Lot 
        {
            get { return m_StrongTypeRow.LOT; }
            set { m_StrongTypeRow.LOT = value; }
        }
        /// <summary>
        /// 入廠日期 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String IQCTIME 
        {
            get { return m_StrongTypeRow.IQCTIME; }
            set { m_StrongTypeRow.IQCTIME = value; }
        }
        /// <summary>
        /// 來料數量 資料型態:NUMBER(13) 是否允許Null:Y 
        /// </summary>
        public Decimal Quantity 
        {
            get { return m_StrongTypeRow.QUANTITY; }
            set { m_StrongTypeRow.QUANTITY = value; }
        }
        /// <summary>
        /// 交易範疇識別碼 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String TXNSCOPEUID 
        {
            get { return m_StrongTypeRow.TXNSCOPEUID; }
            set { m_StrongTypeRow.TXNSCOPEUID = value; }
        }
        /// <summary>
        /// 交易行為 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String ActionType 
        {
            get { return m_StrongTypeRow.ACTIONTYPE; }
            set { m_StrongTypeRow.ACTIONTYPE = value; }
        }
        /// <summary>
        /// 執行的應用程式名稱 (Rule路徑+ClientIP: SessionID) 資料型態:NVARCHAR2(2000) 是否允許Null:Y 
        /// </summary>
        public String ApplicationName 
        {
            get { return m_StrongTypeRow.APPLICATIONNAME; }
            set { m_StrongTypeRow.APPLICATIONNAME = value; }
        }

                
        /// <summary>
        /// User ID
        /// </summary>
        public override string UserID { get { return m_StrongTypeRow.USERID; } protected set { m_StrongTypeRow.USERID = value; } }
        
        /// <summary>
        /// Update Time
        /// </summary>
        public override string UpdateTime { get { return m_StrongTypeRow.UPDATETIME; } protected set { m_StrongTypeRow.UPDATETIME = value; } }
        #endregion Properties

        #region Ctor(...)
        static CSTMMSIqcInfoLog()
        {
            CSTMMSIqcInfoLog info = new CSTMMSIqcInfoLog();
            info.SetSchemaRow(CST_MMS_IQC_INFO_LOG.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTMMSIqcInfoLog 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTMMSIqcInfoLog() { }
        
        /// <summary>
        /// 建構一新的 CSTMMSIqcInfoLog object，由指定的 CST_MMS_IQC_INFO_LOG_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTMMSIqcInfoLog(CST_MMS_IQC_INFO_LOG_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

