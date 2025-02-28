using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class SysPermissionManager : GRINGlobalDataManagerBase, IManager<SysPermission, SysPermissionSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(SysPermission entity)
        {
            throw new NotImplementedException();
        }

        public SysPermission Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(SysPermission entity)
        {
            throw new NotImplementedException();
        }

        public List<SysPermission> GetSysPermissionsByTable(int sysUserId, string tableName)
        {
            List<SysPermission> sysPermissions = new List<SysPermission>();
            SQL = "usp_GRINGlobal_Sys_Permission_ByTable_Select";
           
            var parameters = new List<IDbDataParameter> {
                CreateParameter("sys_user_id", (object)sysUserId, false),
                CreateParameter("table_name", (object)tableName, false)
            };

            sysPermissions = GetRecords<SysPermission>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return sysPermissions;
        }

        public List<SysPermission> Search(SysPermissionSearch searchEntity)
        {
            List<SysPermission> sysPermissions = new List<SysPermission>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Permission";
            SQL += " WHERE  (@TableName     IS NULL OR  TableName       = @TableName)";
            SQL += " AND    (@SysGroupID    IS NULL OR  SysGroupID      = @SysGroupID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("TableName", !String.IsNullOrEmpty(searchEntity.TableName) ? (object)searchEntity.TableName : DBNull.Value, true),
                CreateParameter("SysGroupID", searchEntity.SysGroupID > 0 ? (object)searchEntity.SysGroupID : DBNull.Value, true),
            };

            sysPermissions = GetRecords<SysPermission>(SQL, parameters.ToArray());
            parameters.Clear();
            return sysPermissions;
        }

        public int Update(SysPermission entity)
        {
            throw new NotImplementedException();
        }
    }
}
