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
    /// 調機工單紀錄 (Mapping CST_EQP_SETUP_WO Table)
    /// </summary>
    [Serializable]
    public partial class CSTEquipmentSetupWorkOrderInfo : InfoBase 
    {
        private static readonly CSTEquipmentSetupWorkOrderInfo s_ProxyInfo;
        
        
        
        private CST_EQP_SETUP_WO_Row m_StrongTypeRow { get { return (CST_EQP_SETUP_WO_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_EQP_SETUP_WO.EQP_SETUP_WO_SID; } }
        
                /// <summary>
        /// 調機工單紀錄表SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String EQP_SETUP_WO_SID 
        {
            get { return m_StrongTypeRow.EQP_SETUP_WO_SID; }
            set { m_StrongTypeRow.EQP_SETUP_WO_SID = value; }
        }
        /// <summary>
        /// 機台調機SID 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String EQP_SETUP_SID 
        {
            get { return m_StrongTypeRow.EQP_SETUP_SID; }
            set { m_StrongTypeRow.EQP_SETUP_SID = value; }
        }
        /// <summary>
        /// 工單 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String WorkOrder 
        {
            get { return m_StrongTypeRow.WO; }
            set { m_StrongTypeRow.WO = value; }
        }
        /// <summary>
        /// 料號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String DeviceName 
        {
            get { return m_StrongTypeRow.DEVICE; }
            set { m_StrongTypeRow.DEVICE = value; }
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
        static CSTEquipmentSetupWorkOrderInfo()
        {
            CSTEquipmentSetupWorkOrderInfo info = new CSTEquipmentSetupWorkOrderInfo();
            info.SetSchemaRow(CST_EQP_SETUP_WO.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTEquipmentSetupWorkOrderInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTEquipmentSetupWorkOrderInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTEquipmentSetupWorkOrderInfo object，由指定的 CST_EQP_SETUP_WO_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTEquipmentSetupWorkOrderInfo(CST_EQP_SETUP_WO_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

