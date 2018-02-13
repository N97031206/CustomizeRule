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
    ///  (Mapping CST_WIP_RESERVE_CHECKIN_LOG Table)
    /// </summary>
    [Serializable]
    public partial class CSTWIPReserveCheckInLogInfo : InfoBase 
    {
        private static readonly CSTWIPReserveCheckInLogInfo s_ProxyInfo;
        
        
        
        private CST_WIP_RESERVE_CHECKIN_LOG_Row m_StrongTypeRow { get { return (CST_WIP_RESERVE_CHECKIN_LOG_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_WIP_RESERVE_CHECKIN_LOG.WIP_RESERVE_CHECKIN_LOG_SID; } }
        
                /// <summary>
        /// 批號預約進站歷史紀錄系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String WIP_RESERVE_CHECKIN_LOG_SID 
        {
            get { return m_StrongTypeRow.WIP_RESERVE_CHECKIN_LOG_SID; }
            set { m_StrongTypeRow.WIP_RESERVE_CHECKIN_LOG_SID = value; }
        }
        /// <summary>
        /// 批號預約進站系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String WIPReserveCheckInInfoSID 
        {
            get { return m_StrongTypeRow.WIP_RESERVE_CHECKIN_INFO_SID; }
            set { m_StrongTypeRow.WIP_RESERVE_CHECKIN_INFO_SID = value; }
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
        /// 工作站 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String OperationName 
        {
            get { return m_StrongTypeRow.OPERATION; }
            set { m_StrongTypeRow.OPERATION = value; }
        }
        /// <summary>
        /// 機台 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Equipment 
        {
            get { return m_StrongTypeRow.EQUIPMENT; }
            set { m_StrongTypeRow.EQUIPMENT = value; }
        }
        /// <summary>
        /// 規則名稱 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String RuleName 
        {
            get { return m_StrongTypeRow.RULENAME; }
            set { m_StrongTypeRow.RULENAME = value; }
        }
        /// <summary>
        /// 預約進站時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String InTime 
        {
            get { return m_StrongTypeRow.INTIME; }
            set { m_StrongTypeRow.INTIME = value; }
        }
        /// <summary>
        /// 實際出站時間 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String OutTime 
        {
            get { return m_StrongTypeRow.OUTTIME; }
            set { m_StrongTypeRow.OUTTIME = value; }
        }
        /// <summary>
        /// 當次作業的繫結ID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String LinkSID 
        {
            get { return m_StrongTypeRow.LINKSID; }
            set { m_StrongTypeRow.LINKSID = value; }
        }

                
        ///// <summary>
        ///// User ID
        ///// </summary>
        //public override string UserID { get { return m_StrongTypeRow.USERID; } protected set { m_StrongTypeRow.USERID = value; } }
        
        ///// <summary>
        ///// Update Time
        ///// </summary>
        //public override string UpdateTime { get { return m_StrongTypeRow.UPDATETIME; } protected set { m_StrongTypeRow.UPDATETIME = value; } }
        #endregion Properties

        #region Ctor(...)
        static CSTWIPReserveCheckInLogInfo()
        {
            CSTWIPReserveCheckInLogInfo info = new CSTWIPReserveCheckInLogInfo();
            info.SetSchemaRow(CST_WIP_RESERVE_CHECKIN_LOG.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTWIPReserveCheckInLogInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTWIPReserveCheckInLogInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTWIPReserveCheckInLogInfo object，由指定的 CST_WIP_RESERVE_CHECKIN_LOG_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTWIPReserveCheckInLogInfo(CST_WIP_RESERVE_CHECKIN_LOG_Row row) : base(row) { }
        #endregion
        
     
		
    }
}
