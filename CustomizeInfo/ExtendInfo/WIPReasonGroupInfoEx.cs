using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;

namespace CustomizeRule.CustomizeInfo.ExtendInfo
{
    public class WIPReasonGroupInfoEx : WIPReasonGroupInfo
    {
        public static WIPReasonGroupInfoEx GetReasonGroupByCategory(string category)
        {
            return InfoCenter.GetBySQL<WIPReasonGroupInfoEx>(
                "SELECT * FROM MES_WIP_REASON_GRP WHERE WIP_REASON_GRP_SID = (SELECT WIP_REASON_GRP_SID FROM MES_WIP_REASON_GRP_CTGR  WHERE CATEGORY=#[STRING])", category);
        }
    }
}
