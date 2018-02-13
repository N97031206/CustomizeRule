using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ares.Cimes.IntelliService.Info
{
    public partial class CSTWIPCMMInfo
    {
        public static List<CSTWIPCMMInfo> GetDataByEquipmantAndFileID(string equipmant, string componentLot, string workOrderLot, string materialLot)
        {
            string sSQL = @" SELECT * FROM CST_WIP_CMM
                                WHERE EQUIPMENT = #[STRING] 
                                AND ( FILEID = #[STRING] OR FILEID = #[STRING] OR FILEID = #[STRING]) 
                                AND QC_INSP_SID IS NULL";

            return InfoCenter.GetList<CSTWIPCMMInfo>(sSQL, equipmant, componentLot, workOrderLot, materialLot);
        }

        public static List<CSTWIPCMMInfo> GetDataByEquipmant(string equipmant)
        {
            string sSQL = @" SELECT * FROM CST_WIP_CMM
                                WHERE EQUIPMENT = #[STRING] 
                                AND QC_INSP_SID IS NULL 
                                ORDER BY WIP_CMM_SID ";

            return InfoCenter.GetList<CSTWIPCMMInfo>(sSQL, equipmant);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/


        public static List<CSTWIPCMMInfo> GetDataByEquipmantAndDevice(string equipmant, string device, string face)
        {
            string sSQL = @" SELECT * FROM CST_WIP_CMM
                              WHERE EQUIPMENT = #[STRING] 
                                AND DEVICE = #[STRING]
                                AND QC_INSP_SID IS NULL 
                                AND FACE = #[STRING]
                                ORDER BY FILEID, SEQUENCE DESC";

            return InfoCenter.GetList<CSTWIPCMMInfo>(sSQL, equipmant, device, face);
        }

        public static List<CSTWIPCMMInfo> GetDataListByEquipmantAndDevice(string equipmant, string device)
        {
            string sSQL = @" SELECT * FROM CST_WIP_CMM
                              WHERE (EQUIPMENT = #[STRING] OR SEQUIPMENT = #[STRING])
                                AND DEVICE = #[STRING]
                                AND QC_INSP_SID IS NULL 
                                ORDER BY FILEID, SN, SEQUENCE";

            return InfoCenter.GetList<CSTWIPCMMInfo>(sSQL, equipmant, equipmant, device);
        }

        public static List<CSTWIPCMMInfo> GetDataByEquipmantAndFileIDAndDevice(string equipmant, string componentLot, string workOrderLot, string materialLot, string deviceName, string face = "")
        {
            SqlAgent sa = null;

            string sSQL = @" SELECT * FROM CST_WIP_CMM
                                WHERE EQUIPMENT = #[STRING] 
                                AND DEVICE = #[STRING]
                                AND ( FILEID = #[STRING] OR FILEID = #[STRING] OR FILEID = #[STRING]) 
                                AND QC_INSP_SID IS NULL";

            if (face.IsNullOrTrimEmpty() == false)
            {
                sSQL += " AND FACE = #[STRING] ORDER BY SEQUENCE DESC";
                sa = SQLCenter.Parse(sSQL, equipmant, deviceName, componentLot, workOrderLot, materialLot, face);
            }
            else
            {
                sSQL += " ORDER BY SEQUENCE DESC";
                sa = SQLCenter.Parse(sSQL, equipmant, deviceName, componentLot, workOrderLot, materialLot);
            }

            return InfoCenter.GetList<CSTWIPCMMInfo>(sa);
        }

        public static List<CSTWIPCMMInfo> GetDataListByEquipmantAndFileIDAndDevice(string equipmant, string componentLot, string workOrderLot, string materialLot, string deviceName)
        {
            SqlAgent sa = null;

            string sSQL = @" SELECT * FROM CST_WIP_CMM
                                WHERE (EQUIPMENT = #[STRING] OR SEQUIPMENT = #[STRING])
                                AND DEVICE = #[STRING]
                                AND ( FILEID = #[STRING] OR FILEID = #[STRING] OR FILEID = #[STRING]) 
                                AND QC_INSP_SID IS NULL 
                                ORDER BY SEQUENCE DESC";

            
                sa = SQLCenter.Parse(sSQL, equipmant, equipmant, deviceName, componentLot, workOrderLot, materialLot);
            
            return InfoCenter.GetList<CSTWIPCMMInfo>(sa);
        }

        public static List<CSTWIPCMMInfo> GetDataByEquipmantAndFileIDAndDevice(string equipmant, string fileID, string deviceName, string face = "")
        {
            SqlAgent sa = null;

            string sSQL = @" SELECT * FROM CST_WIP_CMM
                                WHERE EQUIPMENT = #[STRING] 
                                AND DEVICE = #[STRING]
                                AND FILEID = #[STRING]
                                AND QC_INSP_SID IS NULL";

            if (face.IsNullOrTrimEmpty() == false)
            {
                sSQL += " AND FACE = #[STRING] ORDER BY SEQUENCE DESC";
                sa = SQLCenter.Parse(sSQL, equipmant, deviceName, fileID, face);
            }
            else
            {
                sSQL += " ORDER BY SEQUENCE DESC";
                sa = SQLCenter.Parse(sSQL, equipmant, deviceName, fileID);
            }

            return InfoCenter.GetList<CSTWIPCMMInfo>(sa);
        }
    }
}
