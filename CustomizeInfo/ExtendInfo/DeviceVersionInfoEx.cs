using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ares.Cimes.IntelliService.DbTableSchema;

namespace Ares.Cimes.IntelliService.Info
{
    [Serializable]
    public partial class DeviceVersionInfoEx : DeviceVersionInfo
    {
        static DeviceVersionInfoEx()
        {
            MES_PRC_DEVICE_VER.Singleton.SyncColumnsFromDB();
        }

        #region Property
        /// <summary>
        /// 首件次數
        /// </summary>
        public int FAICount
        {
            get
            {
                return (this["FAICOUNT"] is DBNull) ? 0 : int.Parse(this["FAICOUNT"].ToString());
            }
            set
            {
                this["FAICOUNT"] = value;
            }
        }
        /// <summary>
        /// PPK次數
        /// </summary>
        public int PPKCount
        {
            get
            {
                return (this["PPKCOUNT"] is DBNull) ? 0 : int.Parse(this["PPKCOUNT"].ToString());
            }
            set
            {
                this["PPKCOUNT"] = value;
            }
        }

        /// <summary>
        /// 華司物料紀錄的位置
        /// </summary>
        public string PushLocation
        {
            get
            {
                return this["PUSH_LOCATION"].ToCimesString(string.Empty);
            }
            set
            {
                this["PUSH_LOCATION"] = value;
            }
        }

        /// <summary>
        /// 裝配機台檔案產生的孔位
        /// </summary>
        public string AssemblyFileLocation
        {
            get
            {
                return this["ASSEMBLY_FILE_LOCATION"].ToCimesString(string.Empty);
            }
            set
            {
                this["ASSEMBLY_FILE_LOCATION"] = value;
            }
        }

        /// <summary>
        /// 料號是否需要執行中心孔量測
        /// </summary>
        public string CenterHoleFlag
        {
            get
            {
                return this["CENTER_HOLE_FLAG"].ToCimesString(string.Empty);
            }
            set
            {
                this["CENTER_HOLE_FLAG"] = value;
            }
        }

        /// <summary>
        /// PLM編號(流程卡使用)
        /// </summary>
        public string PLMNO
        {
            get
            {
                return this["PLMNO"].ToCimesString(string.Empty);
            }
            set
            {
                this["PLMNO"] = value;
            }
        }

        /// <summary>
        /// PLM版本
        /// </summary>
        public string PLMVR
        {
            get
            {
                return this["PLMVR"].ToCimesString(string.Empty);
            }
            set
            {
                this["PLMVR"] = value;
            }
        }

        /// <summary>
        /// 藍圖號碼(流程卡使用)
        /// </summary>
        public string BPNO
        {
            get
            {
                return this["BPNO"].ToCimesString(string.Empty);
            }
            set
            {
                this["BPNO"] = value;
            }
        }

        /// <summary>
        /// 藍圖版期(流程卡使用)
        /// </summary>
        public string BPREV
        {
            get
            {
                return this["BPREV"].ToCimesString(string.Empty);
            }
            set
            {
                this["BPREV"] = value;
            }
        }

        /// <summary>
        /// 生產型態：內容包含SN、LOT、RUNCARD
        /// </summary>
        public string ProdType
        {
            get
            {
                return this["PRODTYPE"].ToCimesString(string.Empty);
            }
            set
            {
                this["PRODTYPE"] = value;
            }
        }

        #endregion

        public static DeviceVersionInfoEx GetDataByDeviceName(string deviceName)
        {
            string sSQL = @"SELECT * FROM MES_PRC_DEVICE_VER WHERE DEVICE = #[STRING] AND REVSTATE = 'ACTIVE'";

            return InfoCenter.GetBySQL<DeviceVersionInfoEx>(sSQL, deviceName);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
