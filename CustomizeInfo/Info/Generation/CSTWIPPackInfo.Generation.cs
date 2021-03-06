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
    /// 包裝主檔 (Mapping CST_WIP_PACK Table)
    /// </summary>
    [Serializable]
    public partial class CSTWIPPackInfo : InfoBase 
    {
        private static readonly CSTWIPPackInfo s_ProxyInfo;
        
        
        
        private CST_WIP_PACK_Row m_StrongTypeRow { get { return (CST_WIP_PACK_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// WIP_PACK_SID
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_WIP_PACK.WIP_PACK_SID; } }
        
                /// <summary>
        /// 包裝主檔表SID 資料型態:NVARCHAR2(100) 是否允許Null:N 
        /// </summary>
        public String WIP_PACK_SID 
        {
            get { return m_StrongTypeRow.WIP_PACK_SID; }
            set { m_StrongTypeRow.WIP_PACK_SID = value; }
        }
        /// <summary>
        /// 箱號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String BOXNO 
        {
            get { return m_StrongTypeRow.BOXNO; }
            set { m_StrongTypeRow.BOXNO = value; }
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
        /// 檢驗人員 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String INSPUSER 
        {
            get { return m_StrongTypeRow.INSPUSER; }
            set { m_StrongTypeRow.INSPUSER = value; }
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
        static CSTWIPPackInfo()
        {
            CSTWIPPackInfo info = new CSTWIPPackInfo();
            info.SetSchemaRow(CST_WIP_PACK.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTWIPPackInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTWIPPackInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTWIPPackInfo object，由指定的 CST_WIP_PACK_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTWIPPackInfo(CST_WIP_PACK_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

