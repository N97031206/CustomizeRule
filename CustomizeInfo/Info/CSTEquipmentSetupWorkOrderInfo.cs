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
    public partial class CSTEquipmentSetupWorkOrderInfo 
    {
        public static List<CSTEquipmentSetupWorkOrderInfo> GetEquipmentSetupWOList(string eqpSetupSID)
        {
            return InfoCenter.GetList<CSTEquipmentSetupWorkOrderInfo>("SELECT * FROM CST_EQP_SETUP_WO WHERE EQP_SETUP_SID = #[STRING]", eqpSetupSID);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}

