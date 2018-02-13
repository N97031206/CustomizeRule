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
    ///  (Mapping CST_TOOL_DEVICE Table)
    /// </summary>
    [Serializable]
    public partial class CSTToolDeviceInfo : InfoBase 
    {
        private static readonly CSTToolDeviceInfo s_ProxyInfo;
        
        
        
        private CST_TOOL_DEVICE_Row m_StrongTypeRow { get { return (CST_TOOL_DEVICE_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_TOOL_DEVICE.TOOL_DEVICE_SID; } }
        
                /// <summary>
        ///  資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String ToolDeviceSID 
        {
            get { return m_StrongTypeRow.TOOL_DEVICE_SID; }
            set { m_StrongTypeRow.TOOL_DEVICE_SID = value; }
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
        public String EquipmentName 
        {
            get { return m_StrongTypeRow.EQP; }
            set { m_StrongTypeRow.EQP = value; }
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
        static CSTToolDeviceInfo()
        {
            CSTToolDeviceInfo info = new CSTToolDeviceInfo();
            info.SetSchemaRow(CST_TOOL_DEVICE.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTToolDeviceInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTToolDeviceInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTToolDeviceInfo object，由指定的 CST_TOOL_DEVICE_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTToolDeviceInfo(CST_TOOL_DEVICE_Row row) : base(row) { }
        #endregion
        
     
		
    }
}
