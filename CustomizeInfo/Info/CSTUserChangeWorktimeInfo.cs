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

    public partial class CSTUserChangeWorktimeInfo
    {
        public static List<CSTUserChangeWorktimeInfo> GetNonFullChangeWorkTimeListByLotAndOperation(string lot, string operation)
        {
            return InfoCenter.GetList<CSTUserChangeWorktimeInfo>("SELECT * FROM CST_USR_CHANGE_WORKTIME WHERE LOT= #[STRING] AND OPERATION = #[STRING] AND FULLFLAG = 'N'", lot, operation);
        }
    }
}
