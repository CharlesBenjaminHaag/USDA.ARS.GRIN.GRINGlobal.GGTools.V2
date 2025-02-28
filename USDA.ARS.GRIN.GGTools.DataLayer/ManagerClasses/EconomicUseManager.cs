using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class EconomicUseManager : GRINGlobalDataManagerBase, IManager<EconomicUse, EconomicUseSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(EconomicUse entity)
        {
            throw new NotImplementedException();
        }

        public EconomicUse Get(int entityId)
        {
            throw new NotImplementedException();
        }
        public List<EconomicUse> GetFolderItems(EconomicUseSearch searchEntity)
        {
            List<EconomicUse> results = new List<EconomicUse>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Economic_Use_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<EconomicUse>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual int Insert(EconomicUse entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<EconomicUse>(entity);
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

        public virtual int Update(EconomicUse entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<EconomicUse>(entity);

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

        public List<EconomicUse> Search(EconomicUseSearch searchEntity)
        {
            List<EconomicUse> results = new List<EconomicUse>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Economic_Use ";
            SQL += " WHERE  (@SpeciesID                 IS NULL     OR SpeciesID                =       @SpeciesID)";
            SQL += " AND    (@SpeciesName               IS NULL     OR SpeciesName              LIKE    '%' +  @SpeciesName + '%')";
            SQL += " AND    (@AssembledName             IS NULL     OR AssembledName            LIKE    '%' +  @AssembledName + '%')";
            SQL += " AND    (@PlantPartCode             IS NULL     OR PlantPartCode            =       @PlantPartCode)";

            // Common extended fields
            SQL += " AND    (@ID   IS NULL OR ID       =         @ID)";
            SQL += " AND    (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@CreatedDate                   IS NULL OR CreatedDate              =       @CreatedDate)";
            SQL += " AND    (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND    (@ModifiedDate                  IS NULL OR ModifiedDate             =       @ModifiedDate)";
            SQL += " AND    (@Note                          IS NULL OR Note                     LIKE    '%' + @Note + '%')";
            SQL += " AND    (@CitationText                  IS NULL OR CitationText             LIKE    '%' + @CitationText + '%')";

            //if (!String.IsNullOrEmpty(searchEntity.IDList))
            //{
            //    SQL += " AND    SpeciesID IN (" + searchEntity.IDList + ")";
            //}

            if (!String.IsNullOrEmpty(searchEntity.IDList))
            {
                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }
                SQL += " ID IN (" + searchEntity.IDList + ")";
            }

            var parameters = new List<IDbDataParameter> {
            
            CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
            CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
            CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
            CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
            CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
            CreateParameter("CitationText", (object)searchEntity.CitationText ?? DBNull.Value, true),
            CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
            CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
            CreateParameter("AssembledName", (object)searchEntity.AssembledName ?? DBNull.Value, true),
            CreateParameter("PlantPartCode", (object)searchEntity.PlantPartCode ?? DBNull.Value, true),
        };

        results = GetRecords<EconomicUse>(SQL, parameters.ToArray());
        RowsAffected = results.Count;

        return results;
    }

        public List<EconomicUse> SearchFolderItems(EconomicUseSearch searchEntity)
        {
            List<EconomicUse> results = new List<EconomicUse>();

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
            results = GetRecords<EconomicUse>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public virtual List<EconomicUsageType> GetEconomicUsageTypes(string economicUsageCode = "")
        {
            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Economic_Usage_Type ";

            if (!String.IsNullOrWhiteSpace(economicUsageCode))
            {
                SQL += " WHERE EconomicUsageCode = '" + economicUsageCode + "'";
            }

            SQL += " ORDER BY UsageType ASC "; 
            List<EconomicUsageType> usageTypes = GetRecords<EconomicUsageType>(SQL);
            return usageTypes;
        }

        protected virtual void BuildInsertUpdateParameters(EconomicUse entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_use_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("taxonomy_economic_usage_type_id", entity.EconomicUsageTypeID == 0 ? DBNull.Value : (object)entity.EconomicUsageTypeID, true); ;
            AddParameter("plant_part_code", (object)entity.PlantPartCode ?? DBNull.Value, false);
            AddParameter("citation_id", entity.CitationID == 0 ? DBNull.Value : (object)entity.CitationID, true); ;
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
