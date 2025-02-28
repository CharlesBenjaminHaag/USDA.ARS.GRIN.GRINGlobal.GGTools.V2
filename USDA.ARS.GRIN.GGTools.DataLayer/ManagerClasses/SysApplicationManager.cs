using System;
using System.Collections.Generic;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysApplicationManager : GRINGlobalDataManagerBase
    {
        public List<SysApplication> Search(SysApplicationSearch searchEntity)
        {
            List<SysApplication> results = new List<SysApplication>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Application";
            SQL += " WHERE (@ApplicationCode        IS NULL     OR      ApplicationCode                     =           @ApplicationCode)";
            SQL += " AND (@SysUserID IS NULL OR SysUserID = @SysUserID)";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("ApplicationCode", (object)searchEntity.ApplicationCode ?? DBNull.Value, true),
                CreateParameter("SysUserID", (object)searchEntity.SysUserID ?? DBNull.Value, true)
        };

            results = GetRecords<SysApplication>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
    }
}
