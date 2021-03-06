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
    /// 工單BOM資料 (Mapping CST_WPC_WO_BOM Table)
    /// </summary>
    [Serializable]
    public partial class CSTWPCWorkOrderBOMInfo : InfoBase 
    {
        private static readonly CSTWPCWorkOrderBOMInfo s_ProxyInfo;
        
        
        
        private CST_WPC_WO_BOM_Row m_StrongTypeRow { get { return (CST_WPC_WO_BOM_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_WPC_WO_BOM.WPC_WO_BOM_SID; } }
        
                /// <summary>
        /// 表SID 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String WPCWorkOrderBOMSID 
        {
            get { return m_StrongTypeRow.WPC_WO_BOM_SID; }
            set { m_StrongTypeRow.WPC_WO_BOM_SID = value; }
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
        /// 項目 資料型態:NVARCHAR2(4) 是否允許Null:Y 
        /// </summary>
        public String POSNR 
        {
            get { return m_StrongTypeRow.POSNR; }
            set { m_StrongTypeRow.POSNR = value; }
        }
        /// <summary>
        /// 元件 資料型態:NVARCHAR2(18) 是否允許Null:Y 
        /// </summary>
        public String MATNR 
        {
            get { return m_StrongTypeRow.MATNR; }
            set { m_StrongTypeRow.MATNR = value; }
        }
        /// <summary>
        /// 說明 資料型態:NVARCHAR2(40) 是否允許Null:Y 
        /// </summary>
        public String MATXT 
        {
            get { return m_StrongTypeRow.MATXT; }
            set { m_StrongTypeRow.MATXT = value; }
        }
        /// <summary>
        /// 需求數量 資料型態:NUMBER(13) 是否允許Null:Y 
        /// </summary>
        public Decimal MENGE 
        {
            get { return m_StrongTypeRow.MENGE; }
            set { m_StrongTypeRow.MENGE = value; }
        }
        /// <summary>
        /// 計量單位 資料型態:NVARCHAR2(3) 是否允許Null:Y 
        /// </summary>
        public String EINHEIT 
        {
            get { return m_StrongTypeRow.EINHEIT; }
            set { m_StrongTypeRow.EINHEIT = value; }
        }
        /// <summary>
        /// 作業 資料型態:NVARCHAR2(4) 是否允許Null:Y 
        /// </summary>
        public String VORNR 
        {
            get { return m_StrongTypeRow.VORNR; }
            set { m_StrongTypeRow.VORNR = value; }
        }
        /// <summary>
        /// 工廠 資料型態:NVARCHAR2(4) 是否允許Null:Y 
        /// </summary>
        public String WERKS 
        {
            get { return m_StrongTypeRow.WERKS; }
            set { m_StrongTypeRow.WERKS = value; }
        }
        /// <summary>
        /// 儲位 資料型態:NVARCHAR2(4) 是否允許Null:Y 
        /// </summary>
        public String LGORT 
        {
            get { return m_StrongTypeRow.LGORT; }
            set { m_StrongTypeRow.LGORT = value; }
        }
        /// <summary>
        /// 批次 資料型態:NVARCHAR2(13) 是否允許Null:Y 
        /// </summary>
        public String CHARG 
        {
            get { return m_StrongTypeRow.CHARG; }
            set { m_StrongTypeRow.CHARG = value; }
        }
        /// <summary>
        /// 已刪除 資料型態:NVARCHAR2(1) 是否允許Null:Y 
        /// </summary>
        public String XLOEK 
        {
            get { return m_StrongTypeRow.XLOEK; }
            set { m_StrongTypeRow.XLOEK = value; }
        }
        /// <summary>
        /// 需求日期 資料型態:NVARCHAR2(8) 是否允許Null:Y 
        /// </summary>
        public String BDTER 
        {
            get { return m_StrongTypeRow.BDTER; }
            set { m_StrongTypeRow.BDTER = value; }
        }

        /// <summary>
        /// 區分料別，1：主料，2：華司料 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String SORTF
        {
            get { return m_StrongTypeRow.SORTF; }
            set { m_StrongTypeRow.SORTF = value; }
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
        static CSTWPCWorkOrderBOMInfo()
        {
            CSTWPCWorkOrderBOMInfo info = new CSTWPCWorkOrderBOMInfo();
            info.SetSchemaRow(CST_WPC_WO_BOM.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTWPCWorkOrderBOMInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTWPCWorkOrderBOMInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTWPCWorkOrderBOMInfo object，由指定的 CST_WPC_WO_BOM_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTWPCWorkOrderBOMInfo(CST_WPC_WO_BOM_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

