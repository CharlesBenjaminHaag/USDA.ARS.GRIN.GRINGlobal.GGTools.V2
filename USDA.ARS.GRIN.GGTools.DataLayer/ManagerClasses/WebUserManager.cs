using System;
using System.Collections.Generic;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class WebUserManager : GRINGlobalDataManagerBase
    {
        public WebUser Get(int entityId, string environment = "")
        {
            WebUser webUser = new WebUser();

            SQL = "usp_GRINGlobal_Web_User_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_user_id", (object)entityId, false)
            };
            webUser = GetRecord<WebUser>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            parameters.Clear();
            return webUser;
        }

        public virtual List<WebUser> Search(WebUserSearch searchEntity)
        {
            WebUser webUser = new WebUser();
            List<WebUser> webUsers = new List<WebUser>();

            SQL = " SELECT * FROM vw_GRINGLobal_Web_Cooperator";
            SQL += " WHERE  (@WebUserName       IS NULL OR  WebUserName     = @WebUserName)";
            SQL += " AND    (@WebUserID         IS NULL OR  WebUserID       = @WebUserID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("WebUserName", (object)searchEntity.WebUserName ?? DBNull.Value, true),
                CreateParameter("WebUserID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true)
            };

            webUser = GetRecord<WebUser>(SQL, parameters.ToArray());
            if (webUser.WebUserID == 0)
            {
                return webUsers;
            }
            parameters.Clear();

            webUsers.Add(webUser);
            RowsAffected = webUsers.Count;
            return webUsers;
        }
       
        public int Insert(WebUser entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebUser>(entity);
            SQL = "usp_GRINGlobal_Web_User_Insert";

            AddParameter("user_name", String.IsNullOrEmpty(entity.WebUserName) ? DBNull.Value : (object)entity.WebUserName, true);
            AddParameter("password", String.IsNullOrEmpty(entity.WebUserPassword) ? DBNull.Value : (object)entity.WebUserPassword, true);
            AddParameter("web_cooperator_id", entity.WebCooperatorID == 0 ? DBNull.Value : (object)entity.WebCooperatorID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_web_user_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_web_user_id", -1);
            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;

        }
        
        public int Update(WebUser entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebUser>(entity);
            SQL = "usp_GRINGlobal_Web_User_Update";

            AddParameter("web_user_id", entity.WebUserID == 0 ? DBNull.Value : (object)entity.WebUserID, true);
            AddParameter("user_name", String.IsNullOrEmpty(entity.WebUserName) ? DBNull.Value : (object)entity.WebUserName, true);
            AddParameter("password", String.IsNullOrEmpty(entity.WebUserPassword) ? DBNull.Value : (object)entity.WebUserPassword, true);
            AddParameter("is_enabled", String.IsNullOrEmpty(entity.IsEnabled) ? DBNull.Value : (object)entity.IsEnabled, true);
            AddParameter("web_cooperator_id", entity.WebCooperatorID == 0 ? DBNull.Value : (object)entity.WebCooperatorID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        
        public int Copy(int sysUserId, int webCooperatorId)
        {
            int entityId = 0;
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_Web_User_Copy";

            AddParameter("sys_user_id", sysUserId == 0 ? DBNull.Value : (object)sysUserId, true);
            AddParameter("web_cooperator_id", webCooperatorId == 0 ? DBNull.Value : (object)webCooperatorId, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_web_user_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entityId = GetParameterValue<int>("@out_web_user_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return entityId;
        }
       
    }
}
