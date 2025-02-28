using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class SysGroupManager : GRINGlobalDataManagerBase, IManager<SysGroup, SysGroupSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(SysGroup entity)
        {
            throw new NotImplementedException();
        }

        public SysGroup Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<SysGroupUserMap> GetSysGroupUserMaps(int sysGroupId)
        {
            List<SysGroupUserMap> sysGroupUserMaps = new List<SysGroupUserMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Group_User_Map";
            SQL += " WHERE  (@SysGroupID            IS NULL OR  SysGroupID      = @SysGroupID)";
            SQL += " ORDER BY FullName ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysGroupID", sysGroupId > 0 ? (object)sysGroupId : DBNull.Value, true),
            };

            sysGroupUserMaps = GetRecords<SysGroupUserMap>(SQL, parameters.ToArray());
            parameters.Clear();
            return sysGroupUserMaps;
        }

        public List<SysPermission> GetSysPermissions(int sysGroupId)
        {
            List <SysPermission> sysPermissions = new List<SysPermission>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Permission";
            SQL += " WHERE  (@SysGroupID            IS NULL OR  SysGroupID      = @SysGroupID)";
            SQL += " ORDER BY PermissionTag ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysGroupID", sysGroupId > 0 ? (object)sysGroupId : DBNull.Value, true),
            };

            sysPermissions = GetRecords<SysPermission>(SQL, parameters.ToArray());
            parameters.Clear();
            return sysPermissions;
        }

        public List<SysGroup> Search(SysGroupSearch searchEntity)
        {
            List<SysGroup> sysGroups = new List<SysGroup>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Group";
            SQL += " WHERE  (@ID            IS NULL OR  ID              = @ID)";
            SQL += " AND    (@GroupTag      IS NULL OR  GroupTag        = @GroupTag)";
            SQL += " AND    (@GroupTitle    IS NULL OR  GroupTitle      LIKE '' + @GroupTag = '%')";

            SQL += " ORDER BY GroupTitle ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("GroupTag", (object)searchEntity.GroupTag ?? DBNull.Value, true),
                CreateParameter("GroupTitle", (object)searchEntity.GroupTitle ?? DBNull.Value, true),

            };

            sysGroups = GetRecords<SysGroup>(SQL, parameters.ToArray());
            parameters.Clear();
            return sysGroups;
        }

        public int Insert(SysGroup entity)
        {
            throw new NotImplementedException();
        }

        public int Update(SysGroup entity)
        {
            throw new NotImplementedException();
        }
    }
}
