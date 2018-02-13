using Ares.Cimes.IntelliService.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizeRule.CustomizeInfo.ExtendInfo
{
    public class OperationResourceInfoEx : OperationResourceInfo
    {
        public static List<OperationResourceInfoEx> GetDataByOperSID(string operSID)
        {
            string sSQL = @"SELECT * FROM MES_PRC_OPER_RESO 
                        WHERE PRC_OPER_SID = #[STRING]";

            return InfoCenter.GetList<OperationResourceInfoEx>(sSQL, operSID);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}
