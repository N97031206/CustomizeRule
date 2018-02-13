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
    ///  (Mapping CST_WMS_DETAIL_BAK Table)
    /// </summary>
    [Serializable]
    public partial class CSTWMSDetailBAKInfo : InfoBase 
    {
        private static readonly CSTWMSDetailBAKInfo s_ProxyInfo;
        
        
        
        private CST_WMS_DETAIL_BAK_Row m_StrongTypeRow { get { return (CST_WMS_DETAIL_BAK_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_WMS_DETAIL_BAK.WMS_DETAIL_BAK_SID; } }
        
                /// <summary>
        /// 入庫明細領用紀錄系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String WMS_DETAIL_BAK_SID 
        {
            get { return m_StrongTypeRow.WMS_DETAIL_BAK_SID; }
            set { m_StrongTypeRow.WMS_DETAIL_BAK_SID = value; }
        }
        /// <summary>
        /// 入庫明細系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String WMS_DETAIL_SID 
        {
            get { return m_StrongTypeRow.WMS_DETAIL_SID; }
            set { m_StrongTypeRow.WMS_DETAIL_SID = value; }
        }
        /// <summary>
        /// 入庫批號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Lot 
        {
            get { return m_StrongTypeRow.LOT; }
            set { m_StrongTypeRow.LOT = value; }
        }
        /// <summary>
        /// 入庫序號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String ComponentID 
        {
            get { return m_StrongTypeRow.COMPONENTID; }
            set { m_StrongTypeRow.COMPONENTID = value; }
        }
        /// <summary>
        /// 數量 資料型態:NUMBER(13) 是否允許Null:Y 
        /// </summary>
        public Decimal Quantity 
        {
            get { return m_StrongTypeRow.QUANTITY; }
            set { m_StrongTypeRow.QUANTITY = value; }
        }
        /// <summary>
        /// 主檔與明細的關聯編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String LinkSID 
        {
            get { return m_StrongTypeRow.LINKSID; }
            set { m_StrongTypeRow.LINKSID = value; }
        }

                
        /// <summary>
        /// User ID
        /// </summary>
        //public override string UserID { get { return m_StrongTypeRow.USERID; } protected set { m_StrongTypeRow.USERID = value; } }
        
        /// <summary>
        /// Update Time
        /// </summary>
        //public override string UpdateTime { get { return m_StrongTypeRow.UPDATETIME; } protected set { m_StrongTypeRow.UPDATETIME = value; } }
        #endregion Properties

        #region Ctor(...)
        static CSTWMSDetailBAKInfo()
        {
            CSTWMSDetailBAKInfo info = new CSTWMSDetailBAKInfo();
            info.SetSchemaRow(CST_WMS_DETAIL_BAK.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTWMSDetailBAKInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTWMSDetailBAKInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTWMSDetailBAKInfo object，由指定的 CST_WMS_DETAIL_BAK_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTWMSDetailBAKInfo(CST_WMS_DETAIL_BAK_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

