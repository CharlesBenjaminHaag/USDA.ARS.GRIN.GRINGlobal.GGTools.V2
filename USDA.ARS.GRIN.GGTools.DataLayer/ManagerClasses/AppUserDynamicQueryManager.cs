using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AppUserDynamicQueryManager : AppDataManagerBase, IManager<AppUserDynamicQuery, AppUserDynamicQuerySearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        protected virtual void BuildInsertUpdateParameters(AppUserDynamicQuery entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("app_user_dynamic_query_id", (object)entity.ID, false);
            }
            AddParameter("title", (object)entity.Title ?? DBNull.Value, true);
            AddParameter("description", (object)entity.Description ?? DBNull.Value, true);
            AddParameter("data_source", (object)entity.DataSource ?? DBNull.Value, true);
            AddParameter("query_syntax", (object)entity.QuerySyntax ?? DBNull.Value, true);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", (object)entity.ModifiedByCooperatorID ?? DBNull.Value, true);
            }
            else
            {
                AddParameter("created_by", (object)entity.CreatedByCooperatorID ?? DBNull.Value, true);
            }
        }

        public int Delete(AppUserDynamicQuery entity)
        {
            throw new NotImplementedException();
        }

        public AppUserDynamicQuery Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(AppUserDynamicQuery entity)
        {
            int errorNumber = 0;
            Reset(CommandType.StoredProcedure);
            Validate<AppUserDynamicQuery>(entity);
            SQL = "usp_GGTools_GRINGlobal_AppUserDynamicQuery_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_dynamic_query_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            entity.ID = GetParameterValue<int>("@out_app_user_dynamic_query_id", -1);
            return entity.ID;
        }

        public List<AppUserDynamicQuery> Search(AppUserDynamicQuerySearch searchEntity)
        {
            List<AppUserDynamicQuery> results = new List<AppUserDynamicQuery>();

            SQL = " SELECT * FROM vw_GRINGlobal_App_User_Dynamic_Query";
            SQL += " WHERE  (@CreatedByCooperatorID       IS NULL OR   CreatedByCooperatorID    =   @CreatedByCooperatorID)";
            SQL += " AND    (@ID                          IS NULL OR   ID                       =   @ID)";
            SQL += " AND    (@TableName                   IS NULL OR   DataSource               =   @TableName)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("TableName", (object)searchEntity.TableName ?? DBNull.Value, true)
            };

            results = GetRecords<AppUserDynamicQuery>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(AppUserDynamicQuery entity)
        {
            throw new NotImplementedException();
        }

    }
}
