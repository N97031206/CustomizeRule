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
    ///  (Mapping CST_API_ERROR_LOG Table)
    /// </summary>
    [Serializable]
    public partial class CSTAPIErrorLogInfo : InfoBase 
    {
        private static readonly CSTAPIErrorLogInfo s_ProxyInfo;
        
        
        
        private CST_API_ERROR_LOG_Row m_StrongTypeRow { get { return (CST_API_ERROR_LOG_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_API_ERROR_LOG.API_ERROR_LOG_SID; } }
        
                /// <summary>
        /// SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String APIErrorLogSID 
        {
            get { return m_StrongTypeRow.API_ERROR_LOG_SID; }
            set { m_StrongTypeRow.API_ERROR_LOG_SID = value; }
        }
        /// <summary>
        /// 線別 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Line 
        {
            get { return m_StrongTypeRow.LINE; }
            set { m_StrongTypeRow.LINE = value; }
        }
        /// <summary>
        /// 批號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Lot
        {
            get { return m_StrongTypeRow.LOT; }
            set { m_StrongTypeRow.LOT = value; }
        }
        /// <summary>
        /// 鍛造批號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String CompLot 
        {
            get { return m_StrongTypeRow.COMPLOT; }
            set { m_StrongTypeRow.COMPLOT = value; }
        }
        /// <summary>
        /// 機台編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String EquipmentName
        {
            get { return m_StrongTypeRow.EQUIPMENT; }
            set { m_StrongTypeRow.EQUIPMENT = value; }
        }
        /// <summary>
        /// 工作站 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Operation 
        {
            get { return m_StrongTypeRow.OPERATION; }
            set { m_StrongTypeRow.OPERATION = value; }
        }
        /// <summary>
        /// Function名稱 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String FunctionName 
        {
            get { return m_StrongTypeRow.FUNCTION_NAME; }
            set { m_StrongTypeRow.FUNCTION_NAME = value; }
        }
        /// <summary>
        /// 訊息 資料型態:NVARCHAR2(2000) 是否允許Null:Y 
        /// </summary>
        public String Message 
        {
            get { return m_StrongTypeRow.MESSAGE; }
            set { m_StrongTypeRow.MESSAGE = value; }
        }

        /// <summary>
        /// 程式錯誤訊息 資料型態: 是否允許Null:Y 
        /// </summary>
        public String Description
        {
            get { return m_StrongTypeRow.DESCR; }
            set { m_StrongTypeRow.DESCR = value; }
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
        static CSTAPIErrorLogInfo()
        {
            CSTAPIErrorLogInfo info = new CSTAPIErrorLogInfo();
            info.SetSchemaRow(CST_API_ERROR_LOG.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTAPIErrorLogInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTAPIErrorLogInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTAPIErrorLogInfo object，由指定的 CST_API_ERROR_LOG_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTAPIErrorLogInfo(CST_API_ERROR_LOG_Row row) : base(row) { }
        #endregion
        
     
		
    }
}
