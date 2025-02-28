using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesSynonymMapManager : GRINGlobalDataManagerBase, IManager<SpeciesSynonymMap, SpeciesSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(SpeciesSynonymMap entity)
        {
            throw new NotImplementedException();
        }

        public SpeciesSynonymMap Get(int entityId)
        {
            SpeciesSynonymMap result = new SpeciesSynonymMap();
            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Species_Synonym_Map ";
            SQL += " WHERE      (@ID                        IS NULL OR  ID = @ID) ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", entityId, true),
            };

            result = GetRecord<SpeciesSynonymMap>(SQL, parameters.ToArray());
            return result;
        }
        public List<SpeciesSynonymMap> GetFolderItems(SpeciesSynonymMapSearch searchEntity)
        {
            List<SpeciesSynonymMap> results = new List<SpeciesSynonymMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Species_Synonym_Map_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<SpeciesSynonymMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public int Insert(SpeciesSynonymMap entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<SpeciesSynonymMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Species_Synonym_Map_Insert";

            AddParameter("taxonomy_species_id_subject", (object)entity.SpeciesAID, false);
            AddParameter("taxonomy_species_id_predicate", (object)entity.SpeciesBID, false);
            AddParameter("synonym_code", (object)entity.SynonymCode ?? DBNull.Value, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_species_synonym_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            entity.ID = GetParameterValue<int>("@out_taxonomy_species_synonym_map_id", -1);
            return entity.ID;
        }

        public List<SpeciesSynonymMap> Search(SpeciesSynonymMapSearch searchEntity)
        {
            List<SpeciesSynonymMap> results = new List<SpeciesSynonymMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Species_Synonym_Map ";
            SQL += " WHERE      (@ID                        IS NULL OR  ID = @ID) ";
            SQL += " AND        (@SpeciesAID                IS NULL OR  SpeciesAID = @SpeciesAID) ";
            SQL += " AND        (@SpeciesBID                IS NULL OR  SpeciesBID = @SpeciesBID) ";
            SQL += " AND        (@CreatedByCooperatorID     IS NULL OR  CreatedByCooperatorID               = @CreatedByCooperatorID)";
            SQL += " AND        ((@SpeciesAName             IS NULL OR  REPLACE(SpeciesAName, ' x ', '')    LIKE    'X ' + @SpeciesAName + '%')";
            SQL += " OR         (@SpeciesAName              IS NULL OR  REPLACE(SpeciesAName, ' x ', '')    LIKE    '+' + @SpeciesAName + '%')";
            SQL += " OR         (@SpeciesAName              IS NULL OR  REPLACE(SpeciesAName, ' x ', '')    LIKE    @SpeciesAName + '%'))";
            SQL += " AND        (@SynonymCode               IS NULL OR  SynonymCode                         = @SynonymCode)";
            SQL += " AND        ((@SpeciesBName             IS NULL OR  REPLACE(SpeciesBName, ' x ', '')    LIKE    'X ' + @SpeciesBName + '%')";
            SQL += " OR         (@SpeciesBName              IS NULL OR  REPLACE(SpeciesBName, ' x ', '')    LIKE    '+' + @SpeciesBName + '%')";
            SQL += " OR         (@SpeciesBName              IS NULL OR  REPLACE(SpeciesBName, ' x ', '')    LIKE    @SpeciesBName + '%'))";

            if (!String.IsNullOrEmpty(searchEntity.IDList))
            {
                SQL += " AND (SpeciesAID IN (" + searchEntity.IDList + "))";
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("SpeciesAID", searchEntity.SpeciesAID > 0 ? (object)searchEntity.SpeciesAID : DBNull.Value, true),
                CreateParameter("SpeciesBID", searchEntity.SpeciesBID > 0 ? (object)searchEntity.SpeciesBID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("SpeciesAName", (object)searchEntity.SpeciesAName ?? DBNull.Value, true),
                CreateParameter("SynonymCode", (object)searchEntity.SynonymCode ?? DBNull.Value, true),
                CreateParameter("SpeciesBName", (object)searchEntity.SpeciesBName ?? DBNull.Value, true),
            };

            results = GetRecords<SpeciesSynonymMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<SpeciesSynonymMap> Search(SpeciesSearch searchEntity)
        {
            throw new NotImplementedException();
        }

        public int Update(SpeciesSynonymMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<SpeciesSynonymMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Species_Synonym_Map_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }
        protected virtual void BuildInsertUpdateParameters(SpeciesSynonymMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_species_synonym_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("taxona_taxonomy_species_id", entity.SpeciesAID == 0 ? entity.SpeciesAID : (object)entity.SpeciesAID, true);
            AddParameter("synonym_code", (object)entity.SynonymCode ?? DBNull.Value, true);
            AddParameter("taxonb_taxonomy_species_id", entity.SpeciesBID == 0 ? entity.SpeciesBID : (object)entity.SpeciesBID, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);

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
