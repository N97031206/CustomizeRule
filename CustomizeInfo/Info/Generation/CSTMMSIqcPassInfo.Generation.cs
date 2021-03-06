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
    ///  (Mapping CST_MMS_IQC_PASS Table)
    /// </summary>
    [Serializable]
    public partial class CSTMMSIqcPassInfo : InfoBase 
    {
        private static readonly CSTMMSIqcPassInfo s_ProxyInfo;
        
        
        
        private CST_MMS_IQC_PASS_Row m_StrongTypeRow { get { return (CST_MMS_IQC_PASS_Row)GetSchemaRow(); } }

        /// <summary>
        /// Non-Static Proxy Info Get Property
        /// </summary>        
        protected override InfoBase ProxyInfo { get { return s_ProxyInfo; } }

        #region Properties
        /// <summary>
        /// #SIDColumnName
        /// </summary>
        protected override DataColumn IDColumn { get { return CST_MMS_IQC_PASS.MMS_IQC_PASS_SID; } }
        
                /// <summary>
        /// IQC登錄系統編號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String MMS_IQC_PASS_SID 
        {
            get { return m_StrongTypeRow.MMS_IQC_PASS_SID; }
            set { m_StrongTypeRow.MMS_IQC_PASS_SID = value; }
        }
        /// <summary>
        /// 來料序號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String SN 
        {
            get { return m_StrongTypeRow.SN; }
            set { m_StrongTypeRow.SN = value; }
        }
        /// <summary>
        /// 來料爐號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String RUNID 
        {
            get { return m_StrongTypeRow.RUNID; }
            set { m_StrongTypeRow.RUNID = value; }
        }
        /// <summary>
        /// 來料批號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Lot 
        {
            get { return m_StrongTypeRow.LOT; }
            set { m_StrongTypeRow.LOT = value; }
        }
        /// <summary>
        /// 物料號 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String MATERIALNO
        {
            get { return m_StrongTypeRow.MATERIALNO; }
            set { m_StrongTypeRow.MATERIALNO = value; }
        }
        /// <summary>
        /// 入廠日期 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String IQCTIME 
        {
            get { return m_StrongTypeRow.IQCTIME; }
            set { m_StrongTypeRow.IQCTIME = value; }
        }
        /// <summary>
        /// 數量 資料型態:NVARCHAR2(100) 是否允許Null:Y 
        /// </summary>
        public String Quantity 
        {
            get { return m_StrongTypeRow.QUANTITY; }
            set { m_StrongTypeRow.QUANTITY = value; }
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
        static CSTMMSIqcPassInfo()
        {
            CSTMMSIqcPassInfo info = new CSTMMSIqcPassInfo();
            info.SetSchemaRow(CST_MMS_IQC_PASS.NewRow());
            s_ProxyInfo = info;
        }
        
        /// <summary>
        /// 建構一空的 CSTMMSIqcPassInfo 物件, 此 Ctor 是供 Generator 使用, 請勿直接呼叫.
        /// </summary>
        public CSTMMSIqcPassInfo() { }
        
        /// <summary>
        /// 建構一新的 CSTMMSIqcPassInfo object，由指定的 CST_MMS_IQC_PASS_Row 為 Raw Row。
        /// 此 Ctor 只供內部程式呼叫使用, 不為 public ctor.
        /// </summary>
        [Obsolete("已被 InfoBase.Create method 取代")]
        protected internal CSTMMSIqcPassInfo(CST_MMS_IQC_PASS_Row row) : base(row) { }
        #endregion
        
     
		
    }
}

