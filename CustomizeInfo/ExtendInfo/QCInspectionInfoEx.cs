using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class QCInspectionInfoEx : QCInspectionInfo
    {
        #region Property
        /// <summary>
        /// 是否需要檢驗
        /// </summary>
        public string PassFlag
        {
            get
            {
                return (this["PASSFLAG"] is DBNull) ? string.Empty : (String)this["PASSFLAG"];
            }
            set
            {
                this["PASSFLAG"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 送件時間
        /// </summary>
        public string SendTime
        {
            get
            {
                return (this["SEND_TIME"] is DBNull) ? string.Empty : (String)this["SEND_TIME"];
            }
            set
            {
                this["SEND_TIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 送件用戶
        /// </summary>
        public string SendUser
        {
            get
            {
                return (this["SEND_USER"] is DBNull) ? string.Empty : (String)this["SEND_USER"];
            }
            set
            {
                this["SEND_USER"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 開始檢驗時間
        /// </summary>
        public string StartTime
        {
            get
            {
                return (this["START_TIME"] is DBNull) ? string.Empty : (String)this["START_TIME"];
            }
            set
            {
                this["START_TIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 開始檢驗用戶
        /// </summary>
        public string StartUser
        {
            get
            {
                return (this["START_USER"] is DBNull) ? string.Empty : (String)this["START_USER"];
            }
            set
            {
                this["START_USER"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 同次送驗繫結的SID
        /// </summary>
        public string BatchID
        {
            get
            {
                return (this["BATCHID"] is DBNull) ? string.Empty : (String)this["BATCHID"];
            }
            set
            {
                this["BATCHID"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }


        /// <summary>
        /// 送件時間
        /// </summary>
        public string ReceiveTime
        {
            get
            {
                return (this["RECEIVE_TIME"] is DBNull) ? string.Empty : (String)this["RECEIVE_TIME"];
            }
            set
            {
                this["RECEIVE_TIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 送件用戶
        /// </summary>
        public string ReceiveUser
        {
            get
            {
                return (this["RECEIVE_USER"] is DBNull) ? string.Empty : (String)this["RECEIVE_USER"];
            }
            set
            {
                this["RECEIVE_USER"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 首件退回時間
        /// </summary>
        public string QCRejectTime
        {
            get
            {
                return (this["QCREJECT_TIME"] is DBNull) ? string.Empty : (String)this["QCREJECT_TIME"];
            }
            set
            {
                this["QCREJECT_TIME"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// 首件退回用戶
        /// </summary>
        public string QCRejectUser
        {
            get
            {
                return (this["QCREJECT_USER"] is DBNull) ? string.Empty : (String)this["QCREJECT_USER"];
            }
            set
            {
                this["QCREJECT_USER"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// NG原因分類
        /// </summary>
        public string NG_Category
        {
            get
            {
                return (this["NG_CATEGORY"] is DBNull) ? string.Empty : (String)this["NG_CATEGORY"];
            }
            set
            {
                this["NG_CATEGORY"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// NG原因碼
        /// </summary>
        public string NG_Reason
        {
            get
            {
                return (this["NG_REASON"] is DBNull) ? string.Empty : (String)this["NG_REASON"];
            }
            set
            {
                this["NG_REASON"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        /// <summary>
        /// NG說明
        /// </summary>
        public string NG_Description
        {
            get
            {
                return (this["NG_DESCR"] is DBNull) ? string.Empty : (String)this["NG_DESCR"];
            }
            set
            {
                this["NG_DESCR"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            }
        }

        #endregion

        public static List<QCInspectionInfoEx> GetDataListByEquip(string equip, string QCType)
        {
            string sql = @"SELECT * FROM MES_QC_INSP
                            WHERE EQUIPMENT = #[STRING] AND QCTYPE = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, equip, QCType);

            return InfoCenter.GetList<QCInspectionInfoEx>(sa);
        }

        public static List<QCInspectionInfoEx> GetDataListByBatchID(string batchID)
        {
            string sql = @"SELECT * FROM MES_QC_INSP
                            WHERE BATCHID = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, batchID);

            return InfoCenter.GetList<QCInspectionInfoEx>(sa);
        }

        public static List<QCInspectionInfoEx> GetDataListByBatchIDAndDeviceName(string batchID, string deviceName)
        {
            string sql = @"SELECT * FROM MES_QC_INSP
                            WHERE BATCHID = #[STRING] AND DEVICE = #[STRING]";
            SqlAgent sa = SQLCenter.Parse(sql, batchID, deviceName);

            return InfoCenter.GetList<QCInspectionInfoEx>(sa);
        }

        public static List<QCInspectionInfoEx> GetInspDataListByEquipmentAndDevice(string equipment, string device)
        {
            string sql = @"SELECT * FROM MES_QC_INSP 
                            WHERE EQUIPMENT = #[STRING] AND DEVICE = #[STRING] AND QCTYPE IN ('FAI', 'PQC') 
                            ORDER BY CREATETIME DESC";

            return InfoCenter.GetList<QCInspectionInfoEx>(sql, equipment, device);
        }

        public static List<QCInspectionInfoEx> GetInspDataListByWOLot(string woLot)
        {
            string sql = @"SELECT * 
                             FROM MES_QC_INSP
                            INNER JOIN MES_QC_INSP_OBJ ON MES_QC_INSP.QC_INSP_SID = MES_QC_INSP_OBJ.QC_INSP_SID
                            WHERE MES_QC_INSP_OBJ.ITEM2 = #[STRING]
                              AND MES_QC_INSP.RESULT IS NULL";

            return InfoCenter.GetList<QCInspectionInfoEx>(sql, woLot);
        }

        public static QCInspectionInfoEx GetInspDataListByEquipmentAndLot(string equipment, string lot)
        {
            string sql = @"SELECT * 
                             FROM MES_QC_INSP
                            INNER JOIN MES_QC_INSP_OBJ ON MES_QC_INSP.QC_INSP_SID = MES_QC_INSP_OBJ.QC_INSP_SID
                            WHERE MES_QC_INSP.EQUIPMENT = #[STRING]
                              AND MES_QC_INSP_OBJ.OBJECTNAME = #[STRING]
                              AND MES_QC_INSP.QCTYPE = 'FAI'
                              AND MES_QC_INSP.RESULT IS NULL";

            return InfoCenter.GetBySQL<QCInspectionInfoEx>(sql, equipment, lot);
        }

        public static QCInspectionInfoEx GetCPFQCDataByObjectName(string objectName)
        {
            string sql = @"SELECT * FROM MES_QC_INSP 
                INNER JOIN MES_QC_INSP_OBJ ON (MES_QC_INSP.QC_INSP_SID = MES_QC_INSP_OBJ.QC_INSP_SID)
                WHERE OBJECTNAME=#[STRING] AND STATUS='WaitQC' AND CREATEUSER='CPFService' ORDER BY MES_QC_INSP.QC_INSP_SID DESC";
            SqlAgent sa = SQLCenter.Parse(sql, objectName);

            return InfoCenter.GetBySQL<QCInspectionInfoEx>(sa);
        }
        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
