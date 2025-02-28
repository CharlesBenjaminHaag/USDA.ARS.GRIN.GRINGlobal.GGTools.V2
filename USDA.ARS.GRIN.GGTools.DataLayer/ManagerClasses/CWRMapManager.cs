using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public partial class CWRMapManager : GRINGlobalDataManagerBase, IManager<CWRMap, CWRMapSearch>
    {
        public CWRMap Get(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_CWR_Map_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_cwr_map_id", (object)entityId, false)
            };
            CWRMap cWRMap = GetRecord<CWRMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return cWRMap;
        }
        public virtual List<CWRMap> Search(CWRMapSearch searchEntity)
        {

            SQL = "SELECT * FROM  vw_GRINGlobal_Taxonomy_CWR_Map ";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@ID                        IS NULL OR ID                       =       @ID)";
            SQL += " AND    (@SpeciesID                 IS NULL OR SpeciesID                =       @SpeciesID)";
            SQL += " AND    (@SpeciesName               IS NULL OR SpeciesName              LIKE    '%' + @SpeciesName + '%')";
            SQL += " AND    (@CropForCWRID              IS NULL OR CropForCWRID             =       @CropForCWRID)";
            SQL += " AND    (@CropForCWRName            IS NULL OR CropForCWRName           LIKE  '%' + @CropForCWRName + '%')";
            SQL += " AND    (@CropCommonName            IS NULL OR CropCommonName           LIKE  '%' + @CropCommonName + '%')";
            SQL += " AND    (@GenepoolCode              IS NULL OR GenepoolCode             =      @GenepoolCode) ";
            SQL += " AND    (@IsCrop                    IS NULL OR IsCrop                   =       @IsCrop) ";
            SQL += " AND    (@IsGraftstock              IS NULL OR IsGraftstock             =      @IsGraftstock) ";
            SQL += " AND    (@IsPotential               IS NULL OR IsPotential              =      @IsPotential) ";
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

            SQL += " ORDER BY AssembledName";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                CreateParameter("CropForCWRID", searchEntity.CropForCWRID > 0 ? (object)searchEntity.CropForCWRID : DBNull.Value, true),
                CreateParameter("CropForCWRName", (object)searchEntity.CropForCWRName ?? DBNull.Value, true),
                CreateParameter("CropCommonName", (object)searchEntity.CropCommonName ?? DBNull.Value, true),
                CreateParameter("GenepoolCode", (object)searchEntity.GenepoolCode ?? DBNull.Value, true),
                CreateParameter("IsCrop", (object)searchEntity.IsCrop ?? DBNull.Value, true),
                CreateParameter("IsGraftstock", (object)searchEntity.IsGraftStock ?? DBNull.Value, true),
                CreateParameter("IsPotential", (object)searchEntity.IsPotential ?? DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
                CreateParameter("CitationText", (object)searchEntity.CitationText ?? DBNull.Value, true),
            };
            List<CWRMap> cropForCWRs = GetRecords<CWRMap>(SQL, parameters.ToArray());
            RowsAffected = cropForCWRs.Count;
            return cropForCWRs;
        }
        public List<CWRMap> SearchFolderItems(CWRMapSearch searchEntity)
        {
            List<CWRMap> results = new List<CWRMap>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " + 
                 " auil.list_name AS ListName, " + 
                 " auil.app_user_item_folder_id AS FolderID, " + 
                 " vgtc.* " +
                 " FROM vw_GRINGlobal_Taxonomy_CWR_Map vgtc " +
                 " JOIN app_user_item_list auil ON vgtc.ID = auil.id_number " +
                 " WHERE auil.id_type = 'taxonomy_cwr_map'  ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<CWRMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual int Insert(CWRMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CWRMap>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_CWR_Map_Insert";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_cwr_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            entity.ID = GetParameterValue<int>("@out_taxonomy_cwr_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return entity.ID;
        }
       
        public int Update(CWRMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CWRMap>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_CWR_Map_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
             return RowsAffected;
        }
        public int Delete(CWRMap entity)
        {
            throw new NotImplementedException();
        }
        public List<CWRMap> GetFolderItems(CWRMapSearch searchEntity)
        {
            List<CWRMap> results = new List<CWRMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_CWR_Map_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<CWRMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        protected void BuildInsertUpdateParameters(CWRMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_cwr_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            //TEMP
            if (entity.IsCropOption)
            {
                entity.IsCrop = "Y";
            }
            else
            {
                entity.IsCrop = "N";
            }

            if (entity.IsGraftStockOption)
            {
                entity.IsGraftstock = "Y";
            }
            else
            {
                entity.IsGraftstock = "N";
            }

            if (entity.IsPotentialOption)
            {
                entity.IsPotential = "Y";
            }
            else
            {
                entity.IsPotential = "N";
            }

            AddParameter("taxonomy_species_id", (object)entity.SpeciesID, false);
            AddParameter("taxonomy_cwr_crop_id", (object)entity.CropForCWRID, false);
            AddParameter("crop_common_name", String.IsNullOrEmpty(entity.CropCommonName) ? DBNull.Value : (object)entity.CropCommonName, true);
            AddParameter("is_crop", String.IsNullOrEmpty(entity.IsCrop) ? DBNull.Value : (object)entity.IsCrop, true);
            AddParameter("genepool_code", String.IsNullOrEmpty(entity.GenepoolCode) ? DBNull.Value : (object)entity.GenepoolCode, true);
            AddParameter("is_graftstock", String.IsNullOrEmpty(entity.IsGraftstock) ? DBNull.Value : (object)entity.IsGraftstock, true);
            AddParameter("is_potential", String.IsNullOrEmpty(entity.IsPotential) ? DBNull.Value : (object)entity.IsPotential, true);
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