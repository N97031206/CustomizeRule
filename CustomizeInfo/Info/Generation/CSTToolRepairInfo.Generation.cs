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
    /// 維修回廠資訊 (Mapping CST_TOOL_REPAIR Table)
    /// </summary>
    [Serializable]
    public partial class CSTToolRepairInfo : InfoBase 
    {
        private static readonly CSTToolRepairInfo s_ProxyInfo;
        
        
        
        private CST_TOOL_REPAIR_Row m_StrongTypeRow { get { return (CST_TOOL_REPAIR_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// TOOL_REPAIR_SID
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_TOOL_REPAIR.TOOL_REPAIR_SID; } }
        
                /// <summary>
        /// 紀錄維修回廠資訊表ID 資料型態:NVARCHAR2(100) 是否允許Null:N 
        /// </summary>
        public String ToolRepairSID 
        {
            get { return m_StrongTypeRow.TOOL_REPAIR_SID; }
            set { m_StrongTypeRow.TOOL_REPAIR_SID = value; }
        }
        /// <summary>
        /// 刀具編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String ToolName 
        {
            get { return m_StrongTypeRow.TOOLNAME; }
            set { m_StrongTypeRow.TOOLNAME = value; }
        }
        /// <summary>
        /// 維修原因分類 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String ReasonCategory 
        {
            get { return m_StrongTypeRow.REASON_CATEGORY; }
            set { m_StrongTypeRow.REASON_CATEGORY = value; }
        }
        /// <summary>
        /// 維修原因 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Reason 
        {
            get { return m_StrongTypeRow.REASON; }
            set { m_StrongTypeRow.REASON = value; }
        }
        /// <summary>
        /// 維修部位 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Parts 
        {
            get { return m_StrongTypeRow.PARTS; }
            set { m_StrongTypeRow.PARTS = value; }
        }
        /// <summary>
        /// 預計回廠日 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String EstimateDateOfReturn 
        {
            get { return m_StrongTypeRow.ESTIMATE_DATE_OF_RETURN; }
            set { m_StrongTypeRow.ESTIMATE_DATE_OF_RETURN = value; }
        }
        /// <summary>
        /// 送修人員 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String RepairUser 
        {
            get { return m_StrongTypeRow.REPAIR_USER; }
            set { m_StrongTypeRow.REPAIR_USER = value; }
        }
        /// <summary>
        /// 送修日 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String RepairTime 
        {
            get { return m_StrongTypeRow.REPAIR_TIME; }
            set { m_StrongTypeRow.REPAIR_TIME = value; }
        }
        /// <summary>
        /// 實際回廠日 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String ActualDateOfReturn 
        {
            get { return m_StrongTypeRow.ACTUAL_DATE_OF_RETURN; }
            set { m_StrongTypeRow.ACTUAL_DATE_OF_RETURN = value; }
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
        static CSTToolRepairInfo()
        {
            CSTToolRepairInfo info = new CSTToolRepairInfo();
            info.SetSchemaRow(CST_TOOL_REPAIR.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTToolRepairInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTToolRepairInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTToolRepairInfo object，由指定的 CST_TOOL_REPAIR_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTToolRepairInfo(CST_TOOL_REPAIR_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

