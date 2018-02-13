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
    ///  (Mapping CST_API_COOLING_DOWN_TEMP Table)
    /// </summary>
    [Serializable]
    public partial class CSTAPICoolingDownTempInfo : InfoBase 
    {
        private static readonly CSTAPICoolingDownTempInfo s_ProxyInfo;
        
        
        
        private CST_API_COOLING_DOWN_TEMP_Row m_StrongTypeRow { get { return (CST_API_COOLING_DOWN_TEMP_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_API_COOLING_DOWN_TEMP.API_COOLING_DOWN_TEMP_SID; } }
        
                /// <summary>
        /// 降溫溫度記錄表LOG SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String APICoolingDownSID 
        {
            get { return m_StrongTypeRow.API_COOLING_DOWN_TEMP_SID; }
            set { m_StrongTypeRow.API_COOLING_DOWN_TEMP_SID = value; }
        }
        /// <summary>
        /// 工件編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String ComponsetLot 
        {
            get { return m_StrongTypeRow.COMPLOT; }
            set { m_StrongTypeRow.COMPLOT = value; }
        }
        /// <summary>
        /// 溫度 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Temp 
        {
            get { return m_StrongTypeRow.TEPM; }
            set { m_StrongTypeRow.TEPM = value; }
        }
        /// <summary>
        /// WIP歷史記錄LINKSID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String LinkSID 
        {
            get { return m_StrongTypeRow.LINKSID; }
            set { m_StrongTypeRow.LINKSID = value; }
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
        static CSTAPICoolingDownTempInfo()
        {
            CSTAPICoolingDownTempInfo info = new CSTAPICoolingDownTempInfo();
            info.SetSchemaRow(CST_API_COOLING_DOWN_TEMP.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTAPICoolingDownTempInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTAPICoolingDownTempInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTAPICoolingDownTempInfo object，由指定的 CST_API_COOLING_DOWN_TEMP_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTAPICoolingDownTempInfo(CST_API_COOLING_DOWN_TEMP_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

