using System;
using System.Collections.Generic;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CWRTraitManager : GRINGlobalDataManagerBase, IManager<CWRTrait, CWRTraitSearch>
    {
        protected virtual void BuildInsertUpdateParameters(CWRTrait entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_cwr_trait_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            //TEMP
            if (entity.IsPotentialOption)
            {          
                entity.IsPotential = "Y";
            }
            else
            {
                entity.IsPotential = "N";
            }

            AddParameter("taxonomy_cwr_map_id", (object)entity.CWRMapID, false);
            AddParameter("trait_class_code", String.IsNullOrEmpty(entity.TraitClassCode) ? DBNull.Value : (object)entity.TraitClassCode, true);
            AddParameter("is_potential", String.IsNullOrEmpty(entity.IsPotential) ? DBNull.Value : (object)entity.IsPotential, true);
            AddParameter("breeding_type_code", String.IsNullOrEmpty(entity.BreedingTypeCode) ? DBNull.Value : (object)entity.BreedingTypeCode, true);
            AddParameter("breeding_usage_note", String.IsNullOrEmpty(entity.BreedingUsageNote) ? DBNull.Value : (object)entity.BreedingUsageNote, true);
            AddParameter("ontology_trait_identifier", String.IsNullOrEmpty(entity.OntologyTraitIdentifier) ? DBNull.Value : (object)entity.OntologyTraitIdentifier, true);
            AddParameter("citation_id", entity.CitationID == 0 ? DBNull.Value : (object)entity.CitationID, true);
            AddParameter("note", String.IsNullOrEmpty(entity.Note) ? DBNull.Value : (object)entity.Note, true);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            }
            else
            {
                AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            }
        }
        public virtual List<CWRTrait> Search(CWRTraitSearch searchEntity)
        {
            List<CWRTrait> results = new List<CWRTrait>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_CWR_Trait ";
            SQL += " WHERE  (@ID                        IS NULL OR ID                       =     @ID)";
            SQL += " AND    (@CropForCWRID              IS NULL OR CropForCWRID             =     @CropForCWRID)";
            SQL += " AND    (@CWRMapID                  IS NULL OR CWRMapID             =     @CWRMapID)";
            SQL += " AND    (@CropForCWRName            IS NULL OR CropForCWRName           LIKE  '%' + @CropForCWRName + '%')";
            SQL += " AND    (@SpeciesID                 IS NULL OR SpeciesID                =     @SpeciesID)";
            SQL += " AND    (@SpeciesName               IS NULL OR SpeciesName              LIKE  '%' + @SpeciesName + '%')";
            SQL += " AND    (@TraitClassCode            IS NULL OR TraitClassCode           =     @TraitClassCode)";
            SQL += " AND    (@IsPotential               IS NULL OR IsPotential              =     @IsPotential) ";
            SQL += " AND    (@BreedingTypeCode          IS NULL OR BreedingTypeCode         =     @BreedingTypeCode) ";
            SQL += " AND    (@BreedingUsageNote         IS NULL OR BreedingUsageNote        LIKE  '%' + @BreedingUsageNote + '%') ";
            SQL += " AND    (@OntologyTraitIdentifier   IS NULL OR OntologyTraitIdentifier  LIKE  '%' + @OntologyTraitIdentifier + '%') ";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR CreatedByCooperatorID    =     @CreatedByCooperatorID)";
            SQL += " AND    (@Note                      IS NULL OR Note                     LIKE  '%' + @Note + '%') ";
            SQL += " AND    (@CitationText              IS NULL OR CitationText             LIKE  '%' + @CitationText + '%') ";

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
                CreateParameter("CropForCWRID", searchEntity.CropForCWRID > 0 ? (object)searchEntity.CropForCWRID : DBNull.Value, true),
                CreateParameter("CWRMapID", searchEntity.CWRMapID > 0 ? (object)searchEntity.CWRMapID : DBNull.Value, true),
                CreateParameter("CropForCWRName", (object)searchEntity.CropForCWRName ?? DBNull.Value, true),
                CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                CreateParameter("TraitClassCode", (object)searchEntity.TraitClassCode ?? DBNull.Value, true),
                CreateParameter("IsPotential", (object)searchEntity.IsPotential ?? DBNull.Value, true),
                CreateParameter("BreedingTypeCode", (object)searchEntity.BreedingTypeCode ?? DBNull.Value, true),
                CreateParameter("BreedingUsageNote", (object)searchEntity.BreedingUsageNote ?? DBNull.Value, true),
                CreateParameter("OntologyTraitIdentifier", (object)searchEntity.OntologyTraitIdentifier ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
                CreateParameter("CitationText", (object)searchEntity.CitationText ?? DBNull.Value, true),
            };

            results = GetRecords<CWRTrait>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        

       
        public int Insert(CWRTrait entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CWRTrait>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Cwr_Trait_Insert";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_cwr_trait_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            entity.ID = GetParameterValue<int>("@out_taxonomy_cwr_trait_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            return entity.ID;
        }

        public int Update(CWRTrait entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CWRTrait>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Cwr_Trait_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }

        public int Delete(CWRTrait entity)
        {
            throw new NotImplementedException();
        }

        public CWRTrait Get(int entityId)
        {
            throw new NotImplementedException();
        }
        
        public List<CWRTrait> GetFolderItems(CWRTraitSearch searchEntity)
        {
            List<CWRTrait> results = new List<CWRTrait>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_CWR_Trait_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<CWRTrait>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// REFACTOR
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public List<CodeValue> SearchNotes(string tableName, string note)
        {
            // Create SQL to search for rows
            SQL = "SELECT Value, Description FROM vw_GRINGlobal_Taxonomy_Note ";
            SQL += " WHERE (@Note      IS NULL      OR Description     LIKE     '%' + @Note + '%') ";
            SQL += " AND   (Value      =            @TableName) ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("TableName", (object)tableName ?? DBNull.Value, true),
                CreateParameter("Note", (object)note ?? DBNull.Value, true),
            };
            List<CodeValue> codeValues = GetRecords<CodeValue>(SQL, parameters.ToArray());
            RowsAffected = codeValues.Count;
            return codeValues;
        }
    }
}
