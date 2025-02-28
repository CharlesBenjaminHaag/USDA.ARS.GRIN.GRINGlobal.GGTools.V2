using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class SysGroupUserMapManager : GRINGlobalDataManagerBase, IManager<SysGroupUserMap, SysGroupUserMapSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(SysGroupUserMap entity)
        {
            throw new NotImplementedException();
        }

        public SysGroupUserMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public virtual List<SysGroupUserMap> GetAvailable(int sysUserId)
        {
            List<SysGroupUserMap> sysGroupUserMaps = new List<SysGroupUserMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Group " +
                " WHERE ID NOT IN " +
                " (SELECT SysGroupID FROM vw_GRINGlobal_Sys_Group_User_Map WHERE SysUserID = @SysUserID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysUserID", sysUserId, true)
            };
            sysGroupUserMaps = GetRecords<SysGroupUserMap>(SQL, CommandType.Text, parameters.ToArray());
            return sysGroupUserMaps;
        }
        public virtual List<SysGroupUserMap> GetUnavailable(int sysUserId)
        {
            List<SysGroupUserMap> sysGroupUserMaps = new List<SysGroupUserMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Group " +
                " WHERE ID IN " +
                " (SELECT SysGroupID FROM vw_GRINGlobal_Sys_Group_User_Map WHERE SysUserID = @SysUserID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysUserID", sysUserId, true)
            };
            sysGroupUserMaps = GetRecords<SysGroupUserMap>(SQL, CommandType.Text, parameters.ToArray());
            return sysGroupUserMaps;
        }

        //public List<SysGroupUserMap> GetSysGroupUserMapsByTable(string tableName)
        //{
        //    SQL = "usp_GRINGlobal_Sys_Group_User_Maps_ByTable_Select";
        //    List<SysGroupUserMap> sysGroupUserMaps = new List<SysGroupUserMap>();

        //    var parameters = new List<IDbDataParameter> {
        //        CreateParameter("sys_table_name", (object)tableName, false)
        //    };

        //    sysGroupUserMaps = GetRecords<SysGroupUserMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
        //    return sysGroupUserMaps;
        //}

        public int Insert(SysGroupUserMap entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_Sys_Group_User_Map_Insert";

            AddParameter("sys_user_id", (object)entity.SysUserID, false);
            AddParameter("sys_group_id", (object)entity.SysGroupID, false);
            AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_group_user_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }

        public List<SysGroupUserMap> Search(SysGroupUserMapSearch searchEntity)
        {
            List<SysGroupUserMap> sysGroupUserMaps = new List<SysGroupUserMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Group_User_Map";
            SQL += " WHERE  (@GroupTag          IS NULL OR  GroupTag        LIKE    '%' + @GroupTag + '%')";
            SQL += " AND    (@SysUserID        IS NULL OR  SysUserID      =       @SysUserID)";
            SQL += " AND    (@SysGroupID        IS NULL OR  SysGroupID      =       @SysGroupID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("GroupTag", (object)searchEntity.GroupTag ?? DBNull.Value, true),
                CreateParameter("SysUserID", searchEntity.SysUserID > 0 ? (object)searchEntity.SysUserID : DBNull.Value, true),
                CreateParameter("SysGroupID", searchEntity.SysGroupID > 0 ? (object)searchEntity.SysGroupID : DBNull.Value, true)
            };

            sysGroupUserMaps = GetRecords<SysGroupUserMap>(SQL, parameters.ToArray());
            parameters.Clear();
            RowsAffected = sysGroupUserMaps.Count;
            return sysGroupUserMaps;
        }

        public int Update(SysGroupUserMap entity)
        {
            throw new NotImplementedException();
        }
    }
}
