using System;
using System.Collections.Generic;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysUserManager : GRINGlobalDataManagerBase
    {
        public SysUser Get(int entityId)
        {
            SysUser sysUser = new SysUser();
            SQL = "usp_GRINGlobal_Sys_User_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("sys_user_id", (object)entityId, false)
            };
            sysUser = GetRecord<SysUser>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            parameters.Clear();
            return sysUser;
        }
        public SysUser GetByCooperatorID(int cooperatorId)
        {
            SysUser sysUser = new SysUser();
            SQL = "usp_GRINGlobal_Sys_User_By_Cooperator_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false)
            };
            sysUser = GetRecord<SysUser>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            parameters.Clear();
            return sysUser;
        }
        public List<SysUser> GetTaxonomySysUsers()
        {
            List<SysUser> sysUsers = new List<SysUser>();

            SQL = "SELECT * FROM vw_GRINGlobal_Sys_User " +
                " WHERE SysUserID IN " +
                " (SELECT SysUserID FROM vw_GRINGlobal_Sys_Group_User_Map " +
                " WHERE GroupTag LIKE '%TAXON%' " +
                " OR GroupTag = 'MANAGE_CITATION' " +
                " OR GroupTag = 'LITERATURE') " +
                " ORDER BY FullName ";
            sysUsers = GetRecords<SysUser>(SQL);
            RowsAffected = sysUsers.Count;
            return sysUsers;
        }
        public virtual List<SysUser> Search(SysUserSearch searchEntity)
        {
            SysUser sysUser = new SysUser();
            List<SysUser> sysUsers = new List<SysUser>();

            SQL = " SELECT * FROM vw_GRINGLobal_Sys_User";
            SQL += " WHERE  (@UserName  IS NULL OR  UserName = @UserName)";
            SQL += " AND    (@ID        IS NULL OR  SysUserID = @ID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("UserName", (object)searchEntity.UserName ?? DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true)
            };

            sysUser = GetRecord<SysUser>(SQL, parameters.ToArray());
            if (sysUser.SysUserID == 0)
            {
                return sysUsers;
            }
            parameters.Clear();

            // Get groups of which user is a member.
            SQL = "usp_GRINGlobal_Sys_Group_User_Map_Select";
            parameters = new List<IDbDataParameter> {
                CreateParameter("sys_user_id", (object)sysUser.SysUserID, false)
            };
            sysUser.Groups = GetRecords<SysGroupUserMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            parameters.Clear();

            // Get GGTools applications to which user has access.
            SQL = "usp_GRINGlobal_Sys_Group_User_Maps_Select";
            parameters = new List<IDbDataParameter> {
                CreateParameter("sys_user_id", (object)sysUser.SysUserID, false)
            };
            sysUser.Applications = GetRecords<SysApplication>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            sysUsers.Add(sysUser);
            RowsAffected = sysUsers.Count;
            return sysUsers;
        }
        public virtual List<SysGroupUserMap> SelectGroups(int sysUserId)
        {
            List<SysGroupUserMap> sysGroupUserMaps = new List<SysGroupUserMap>();
            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Group_User_Map";
            SQL += " WHERE  (@SysUserID         IS NULL OR  SysUserID       = @SysUserID)";
            SQL += " ORDER BY GroupTag ";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysUserID", sysUserId, true),
            };

            sysGroupUserMaps = GetRecords<SysGroupUserMap>(SQL, parameters.ToArray());
            parameters.Clear();

            RowsAffected = sysGroupUserMaps.Count;
            return sysGroupUserMaps;
        }
        
        public int InsertSysGroupUserMap(SysGroupUserMap sysGroupUserMap)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_Sys_Group_User_Map_Insert";

            AddParameter("sys_user_id", (object)sysGroupUserMap.SysUserID, false);
            AddParameter("sys_group_id", (object)sysGroupUserMap.SysGroupID, false);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_group_user_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("created_by", sysGroupUserMap.CreatedByCooperatorID == 0 ? DBNull.Value : (object)sysGroupUserMap.CreatedByCooperatorID, true);
            RowsAffected = ExecuteNonQuery();
            
            var errorCode = GetParameterValue<string>("@out_error_number", "");
            return RowsAffected;
        }
        public int DeleteSysGroupUserMap(SysGroupUserMap sysGroupUserMap)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_Sys_Group_User_Map_Delete";

            AddParameter("sys_user_id", (object)sysGroupUserMap.SysUserID, false);
            AddParameter("sys_group_id", (object)sysGroupUserMap.SysGroupID, false);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_group_user_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            var errorCode = GetParameterValue<string>("@out_error_number", "");
            return RowsAffected;
        }

        public int InsertSysUserPasswordResetToken(int sysUserId, string passwordResetToken)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_Sys_User_Password_Reset_Token_Insert";

            AddParameter("sys_user_id", (object)sysUserId, false);
            AddParameter("password_reset_token", (object)passwordResetToken, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_user_password_reset_token_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            var errorCode = GetParameterValue<string>("@out_error_number", "");
            return RowsAffected;
        }

        public SysUser ValidateSysUserPasswordResetToken(string passwordResetToken)
        {
            SysUser sysUser = new SysUser();
            SQL = "usp_GRINGlobal_Sys_User_Password_Reset_Token_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("password_reset_token", (object)passwordResetToken, false)
            };
            sysUser = GetRecord<SysUser>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return sysUser;
        }

        public int Insert(SysUser entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<SysUser>(entity);
            SQL = "usp_GRINGLobal_Sys_User_Insert";

            AddParameter("user_name", String.IsNullOrEmpty(entity.SysUserName) ? DBNull.Value : (object)entity.SysUserName, true);
            AddParameter("password", String.IsNullOrEmpty(entity.SysUserPassword) ? DBNull.Value : (object)entity.SysUserPassword, true);
            AddParameter("cooperator_id", entity.CooperatorID == 0 ? DBNull.Value : (object)entity.CooperatorID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_user_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_sys_user_id", -1);
            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;

        }
        public int Update(SysUser entity)
        {
            Reset(CommandType.StoredProcedure);
            
            SQL = "usp_GRINGlobal_Sys_User_Update";

            AddParameter("sys_user_id", entity.SysUserID == 0 ? entity.ID : (object)entity.SysUserID, true);
            AddParameter("is_enabled", String.IsNullOrEmpty(entity.IsEnabled) ? DBNull.Value : (object)entity.IsEnabled, true);
            AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            
            RowsAffected = ExecuteNonQuery();
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }

        public int UpdatePassword(SysUser entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_Sys_User_Password_Update";

            AddParameter("sys_user_id", (object)entity.SysUserID, false);
            AddParameter("password", (object)entity.SysUserPassword, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("DB Error");
            }

            return RowsAffected;
        }
    }
}
