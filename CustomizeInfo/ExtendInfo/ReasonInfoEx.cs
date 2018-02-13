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
    public partial class ReasonInfoEx : ReasonInfo
    {
        static ReasonInfoEx()
        {
            MES_WIP_REASON.Singleton.SyncColumnsFromDB();
        }

        public string DefectActionEx
        {
            get { return (this["DEFECTACTION"] is DBNull) ? string.Empty : (String)this["DEFECTACTION"]; }
            set { this["DEFECTACTION"] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value; }
        }

        public string ReasonDescription
        {
            get { return (String)this["REASON"] + "(" + (String)this["DESCR"] + ")"; }
        }
    }
}

