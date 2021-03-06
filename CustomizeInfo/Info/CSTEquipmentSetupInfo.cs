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
    /// 紀錄機台調機資訊 (Mapping CST_EQP_SETUP Table)
    /// </summary>
    public partial class CSTEquipmentSetupInfo
    {
        /// <summary>
        /// 取得機台開始setup，但未完成紀錄
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        public static CSTEquipmentSetupInfo GetUnEquipSetupByEquipmentName(string equipment)
        {
            return InfoCenter.GetBySQL<CSTEquipmentSetupInfo>("SELECT * FROM CST_EQP_SETUP WHERE EQUIPMENT = #[STRING] AND ENDTIME IS NULL", equipment);
        }

        /// <summary>
        /// 依照機台+料號，取得機台調機完成紀錄
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        public static CSTEquipmentSetupInfo GetEquipSetupDownByEquipmentName(string equipment)
        {
            return InfoCenter.GetBySQL<CSTEquipmentSetupInfo>("SELECT * FROM CST_EQP_SETUP WHERE EQUIPMENT = #[STRING] AND ENDTIME IS NOT NULL", equipment);
        }

        /************************以上FUNCTION, INDEX 已確認，新增FUNCTION請往下新增**************************/

    }
}

