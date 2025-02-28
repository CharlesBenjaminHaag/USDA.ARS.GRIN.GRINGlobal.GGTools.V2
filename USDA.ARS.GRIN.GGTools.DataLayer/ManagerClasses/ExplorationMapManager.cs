using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class ExplorationMapManager : GRINGlobalDataManagerBase, IManager<ExplorationMap, ExplorationMapSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(ExplorationMap entity)
        {
            throw new NotImplementedException();
        }

        public ExplorationMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<ExplorationMap> GetFolderItems(ExplorationMapSearch searchEntity)
        {
            List<ExplorationMap> results = new List<ExplorationMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Economic_Use_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<ExplorationMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public virtual int Insert(ExplorationMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<ExplorationMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Economic_Use_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_use_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddStandardParameters();

            RowsAffected = ExecuteNonQuery(false, "@out_taxonomy_use_id");
            entity.ID = GetParameterValue<int>("@out_taxonomy_use_id", -1);

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("SQL Error " + errorNumber.ToString());
            }

            RowsAffected = entity.ID;
            return RowsAffected;
        }

        public virtual int Update(ExplorationMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<ExplorationMap>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Economic_Use_Update";

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

        public List<ExplorationMap> Search(ExplorationMapSearch searchEntity)
        {
            List<ExplorationMap> results = new List<ExplorationMap>();

            SQL = "SELECT * FROM vw_GRINGlobal_Exploration_Map ";
            SQL += " WHERE  (@ExplorationID      IS NULL     OR ExplorationID   =       @ExplorationID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ExplorationID", searchEntity.ExplorationID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            };

            results = GetRecords<ExplorationMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
    }

        public List<ExplorationMap> SearchFolderItems(ExplorationMapSearch searchEntity)
        {
            List<ExplorationMap> results = new List<ExplorationMap>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgtf.* " +
                " FROM vw_GRINGlobal_Taxonomy_Family_Map vgtf " +
                " JOIN app_user_item_list auil " +
                " ON vgtf.ID = auil.id_number " +
                " WHERE auil.id_type = 'taxonomy_family' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<ExplorationMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        protected virtual void BuildInsertUpdateParameters(ExplorationMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_use_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            //AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            //AddParameter("taxonomy_economic_usage_type_id", entity.EconomicUsageTypeID == 0 ? DBNull.Value : (object)entity.EconomicUsageTypeID, true); ;
            //AddParameter("plant_part_code", (object)entity.PlantPartCode ?? DBNull.Value, false);
            //AddParameter("citation_id", entity.CitationID == 0 ? DBNull.Value : (object)entity.CitationID, true); ;
            //AddParameter("note", (object)entity.Note ?? DBNull.Value, false);

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
