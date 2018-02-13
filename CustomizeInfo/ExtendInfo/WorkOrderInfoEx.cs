using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizeRule.CustomizeInfo.ExtendInfo
{
    public partial class WorkOrderInfoEx : WorkOrderInfo
    {
        /// <summary>
        /// 工單線別
        /// </summary>
        public string Division
        {
            get
            {
                return this["DIVISION"].ToCimesString(string.Empty);
            }
            set
            {
                this["DIVISION"] = value;
            }
        }
    }
}
