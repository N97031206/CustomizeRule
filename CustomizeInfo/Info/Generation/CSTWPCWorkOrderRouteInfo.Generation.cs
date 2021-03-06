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
    ///  (Mapping CST_WPC_WO_ROUTE Table)
    /// </summary>
    [Serializable]
    public partial class CSTWPCWorkOrderRouteInfo : InfoBase 
    {
        private static readonly CSTWPCWorkOrderRouteInfo s_ProxyInfo;
        
        
        
        private CST_WPC_WO_ROUTE_Row m_StrongTypeRow { get { return (CST_WPC_WO_ROUTE_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_WPC_WO_ROUTE.WPC_WO_ROUTE_SID; } }
        
                /// <summary>
        /// 表SID 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String WPCWorkOrderRouteSID 
        {
            get { return m_StrongTypeRow.WPC_WO_ROUTE_SID; }
            set { m_StrongTypeRow.WPC_WO_ROUTE_SID = value; }
        }
        /// <summary>
        /// 工單號碼 資料型態:NVARCHAR2(10) 是否允許Null:Y 
        /// </summary>
        public String AUFNR 
        {
            get { return m_StrongTypeRow.AUFNR; }
            set { m_StrongTypeRow.AUFNR = value; }
        }
        /// <summary>
        /// 途程代碼 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String PLNNR 
        {
            get { return m_StrongTypeRow.PLNNR; }
            set { m_StrongTypeRow.PLNNR = value; }
        }
        /// <summary>
        /// 作業號碼 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String VORNR 
        {
            get { return m_StrongTypeRow.VORNR; }
            set { m_StrongTypeRow.VORNR = value; }
        }
        /// <summary>
        /// 工作中心 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String ARBPL 
        {
            get { return m_StrongTypeRow.ARBPL; }
            set { m_StrongTypeRow.ARBPL = value; }
        }
        /// <summary>
        /// 作業短文 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String LTXA1 
        {
            get { return m_StrongTypeRow.LTXA1; }
            set { m_StrongTypeRow.LTXA1 = value; }
        }
        /// <summary>
        /// 基礎數量 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String BMSCH 
        {
            get { return m_StrongTypeRow.BMSCH; }
            set { m_StrongTypeRow.BMSCH = value; }
        }
        /// <summary>
        /// 單位 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String MEINH 
        {
            get { return m_StrongTypeRow.MEINH; }
            set { m_StrongTypeRow.MEINH = value; }
        }
        /// <summary>
        /// 長文 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String LTEXT 
        {
            get { return m_StrongTypeRow.LTEXT; }
            set { m_StrongTypeRow.LTEXT = value; }
        }

                
        ///// <summary>
        ///// User ID
        ///// </summary>
        //public override string UserID { get { return m_StrongTypeRow.USERID; } protected set { m_StrongTypeRow.USERID = value; } }
        
        /// <summary>
        /// Update Time
        /// </summary>
        public override string UpdateTime { get { return m_StrongTypeRow.UPDATETIME; } protected set { m_StrongTypeRow.UPDATETIME = value; } }
        #endregion Properties

        #region Ctor(...)
        static CSTWPCWorkOrderRouteInfo()
        {
            CSTWPCWorkOrderRouteInfo info = new CSTWPCWorkOrderRouteInfo();
            info.SetSchemaRow(CST_WPC_WO_ROUTE.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTWPCWorkOrderRouteInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTWPCWorkOrderRouteInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTWPCWorkOrderRouteInfo object，由指定的 CST_WPC_WO_ROUTE_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTWPCWorkOrderRouteInfo(CST_WPC_WO_ROUTE_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

