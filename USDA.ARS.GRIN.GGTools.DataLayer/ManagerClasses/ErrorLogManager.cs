using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class ErrorLogManager : GRINGlobalDataManagerBase
    {
        public List<ErrorLog> Search(ErrorLogSearch searchEntity)
        {
            List<ErrorLog> results = new List<ErrorLog>();

            SQL = "SELECT Source,Application,ID,Code,ErrorMessage,ErrorDetail,ErrorDetail2,ErrorDetail3,ErrorDetail4,CreateDate FROM gringlobal.dbo.vw_GRINGlobal_Error_Log";

            results = GetRecords<ErrorLog>(SQL);
            RowsAffected = results.Count;

            return results;
        }
    }
}
