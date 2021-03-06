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
    ///  (Mapping CST_WIP_PACK_TEMP Table)
    /// </summary>
    [Serializable]
    public partial class CSTWIPPackTempInfo : InfoBase 
    {
        private static readonly CSTWIPPackTempInfo s_ProxyInfo;
        
        
        
        private CST_WIP_PACK_TEMP_Row m_StrongTypeRow { get { return (CST_WIP_PACK_TEMP_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_WIP_PACK_TEMP.WIP_PACK_TEMP_SID; } }
        
                /// <summary>
        ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String WIP_PACK_TEMP_SID 
        {
            get { return m_StrongTypeRow.WIP_PACK_TEMP_SID; }
            set { m_StrongTypeRow.WIP_PACK_TEMP_SID = value; }
        }
        /// <summary>
        ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String DeviceName 
        {
            get { return m_StrongTypeRow.DEVICE; }
            set { m_StrongTypeRow.DEVICE = value; }
        }
        /// <summary>
        ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String ComponentID 
        {
            get { return m_StrongTypeRow.COMPONENTID; }
            set { m_StrongTypeRow.COMPONENTID = value; }
        }
        /// <summary>
        ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String SIDE 
        {
            get { return m_StrongTypeRow.SIDE; }
            set { m_StrongTypeRow.SIDE = value; }
        }
        /// <summary>
        ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String BatchID 
        {
            get { return m_StrongTypeRow.BATCHID; }
            set { m_StrongTypeRow.BATCHID = value; }
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
        static CSTWIPPackTempInfo()
        {
            CSTWIPPackTempInfo info = new CSTWIPPackTempInfo();
            info.SetSchemaRow(CST_WIP_PACK_TEMP.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTWIPPackTempInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTWIPPackTempInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTWIPPackTempInfo object，由指定的 CST_WIP_PACK_TEMP_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTWIPPackTempInfo(CST_WIP_PACK_TEMP_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

