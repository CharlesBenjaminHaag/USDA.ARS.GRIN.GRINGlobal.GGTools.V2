using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class EconomicUsageTypeManager : GRINGlobalDataManagerBase, IManager<EconomicUsageType, EconomicUsageTypeSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(EconomicUsageType entity)
        {
            throw new NotImplementedException();
        }

        public EconomicUsageType Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<EconomicUsageType> GetFolderItems(EconomicUsageTypeSearch searchEntity)
        {
            List<EconomicUsageType> results = new List<EconomicUsageType>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Economic_Usage_Type_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<EconomicUsageType>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Insert(EconomicUsageType entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<EconomicUsageType>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Economic_Usage_Type_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_economic_usage_type_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddStandardParameters();

            entity.ID = ExecuteNonQuery(false, "@out_taxonomy_economic_usage_type_id");
            entity.ID = GetParameterValue<int>("@out_taxonomy_economic_usage_type_id", -1);

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = entity.ID;
            return RowsAffected;
        }

        public List<EconomicUsageType> Search(EconomicUsageTypeSearch searchEntity)
        {
            List<EconomicUsageType> results = new List<EconomicUsageType>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Economic_Usage_Type ";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL     OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@ID                        IS NULL     OR ID                       =       @ID)";
            SQL += " AND    (@EconomicUsageCode         IS NULL     OR EconomicUsageCode        LIKE   '%' + @EconomicUsageCode + '%')";
            SQL += " AND    (@UsageType                 IS NULL     OR UsageType                LIKE   '%' + @UsageType + '%')";
     
            var parameters = new List<IDbDataParameter> {
            CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
            CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            CreateParameter("EconomicUsageCode", (object)searchEntity.EconomicUsageCode ?? DBNull.Value, true),
            CreateParameter("UsageType", (object)searchEntity.UsageType ?? DBNull.Value, true),
        };

            results = GetRecords<EconomicUsageType>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public int Update(EconomicUsageType entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<EconomicUsageType>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Economic_Usage_Type_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("SQL Error " + errorNumber.ToString());
            }
            return RowsAffected;
        }
        protected virtual void BuildInsertUpdateParameters(EconomicUsageType entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_economic_usage_type_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("economic_usage_code", (object)entity.EconomicUsageCode ?? DBNull.Value, false);
            AddParameter("usage_type", (object)entity.UsageType ?? DBNull.Value, false);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, false);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            }
            else
            {
                AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            }
        }
    }
}
