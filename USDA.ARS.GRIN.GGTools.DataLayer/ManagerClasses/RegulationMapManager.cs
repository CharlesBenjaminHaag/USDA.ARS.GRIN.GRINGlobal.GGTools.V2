using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class RegulationMapManager : GRINGlobalDataManagerBase, IManager<RegulationMap, RegulationMapSearch>
    {
        public virtual int Insert(RegulationMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<RegulationMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Regulation_Map_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_regulation_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddStandardParameters();

            RowsAffected = ExecuteNonQuery(false, "@out_taxonomy_regulation_map_id");
            entity.ID = GetParameterValue<int>("@out_taxonomy_regulation_map_id", -1);

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("SQL Error " + errorNumber.ToString());
            }

            RowsAffected = entity.ID;
            return RowsAffected;
        }

        public virtual int Update(RegulationMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<RegulationMap>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Regulation_Map_Update";

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

        public int Delete(RegulationMap entity)
        {
            throw new NotImplementedException();
        }

        public RegulationMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<RegulationMap> Search(RegulationMapSearch searchEntity)
        {
            List<RegulationMap> results = new List<RegulationMap>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Regulation_Map";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL OR  CreatedByCooperatorID       =       @CreatedByCooperatorID)";
            SQL += " AND    (@ID                        IS NULL OR  ID                          =       @ID) ";
            SQL += " AND    (@FamilyID                  IS NULL OR  FamilyID                    =       @GenusID) ";
            SQL += " AND    (@GenusID                   IS NULL OR  GenusID                     =       @GenusID) ";
            SQL += " AND    (@SpeciesID                 IS NULL OR  SpeciesID                   =       @SpeciesID) ";
            SQL += " AND    (@GeographyID               IS NULL OR  GeographyID                 =       @GeographyID) ";
            SQL += " AND    (@RegulationID              IS NULL OR  RegulationID                =       @RegulationID) ";
            SQL += " AND    (@FamilyName                IS NULL OR  FamilyName                  LIKE   '%' + @FamilyName + '%')";
            SQL += " AND    (@GenusName                 IS NULL OR  GenusName                   LIKE   '%' + @GenusName + '%')";
            SQL += " AND    (@SpeciesName               IS NULL OR  SpeciesName                 LIKE   '%' + @SpeciesName + '%')";
            SQL += " AND    (@Description               IS NULL OR  Description                 LIKE   '%' + @Description + '%')";
            SQL += " AND    (@AssembledName             IS NULL OR  AssembledName               LIKE   '%' + @AssembledName + '%')";
            SQL += " AND    (@IsExempt                  IS NULL OR  IsExempt                    =      @IsExempt)";
                
            if (!String.IsNullOrEmpty(searchEntity.SpeciesIDList))
            {
                SQL += " AND    SpeciesID IN (" + searchEntity.SpeciesIDList + ")";
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("FamilyID", searchEntity.FamilyID > 0 ? (object)searchEntity.FamilyID : DBNull.Value, true),
                CreateParameter("GenusID", searchEntity.GenusID > 0 ? (object)searchEntity.GenusID : DBNull.Value, true),
                CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
                CreateParameter("GeographyID", searchEntity.GeographyID > 0 ? (object)searchEntity.GeographyID : DBNull.Value, true),
                CreateParameter("RegulationID", searchEntity.RegulationID > 0 ? (object)searchEntity.RegulationID : DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("GenusName", (object)searchEntity.GenusName ?? DBNull.Value, true),
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                CreateParameter("Description", (object)searchEntity.Description ?? DBNull.Value, true),
                CreateParameter("AssembledName", (object)searchEntity.AssembledName ?? DBNull.Value, true),
                CreateParameter("IsExempt", (object)searchEntity.IsExempt ?? DBNull.Value, true),
            };

            results = GetRecords<RegulationMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }
        
        public List<RegulationMap> SearchFolderItems(RegulationMapSearch searchEntity)
        {
            List<RegulationMap> results = new List<RegulationMap>();

            SQL = " SELECT vgtcn.* FROM vw_GRINGlobal_Taxonomy_Regulation_Map vgtcn JOIN vw_GRINGlobal_App_User_Item_List vgga " +
                   " ON vgtcn.ID = vgga.EntityID WHERE vgga.TableName = 'taxonomy_regulation_map' ";
            SQL += "AND  (@FolderID                          IS NULL OR  FolderID       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<RegulationMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        #region Taxonomy Common

        public Dictionary<string, string> GetTableNames()
        {
            return new Dictionary<string, string>
            {
                { "taxonomy_family", "Family" },
                { "taxonomy_genus", "Genus" },
                { "taxonomy_species", "Species" }
            };
        }
        
        public List<Citation> GetAvailableCitations(int speciesId)
        {
            SQL = "usp_TaxonomyCitationsSpeciesLookup_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_species_id", (object)speciesId, false)
            };
            List<Citation> citations = GetRecords<Citation>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return citations;
        }
        
        #endregion
        
        protected virtual void BuildInsertUpdateParameters(RegulationMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_regulation_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("taxonomy_genus_id", entity.GenusID == 0 ? DBNull.Value : (object)entity.GenusID, true);
            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("taxonomy_regulation_id", entity.RegulationID == 0 ? DBNull.Value : (object)entity.RegulationID, true);
            AddParameter("is_exempt", entity.IsExemptOption == true ? "Y" : "N", false);
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
        
        public List<RegulationMap> GetFolderItems(RegulationMapSearch searchEntity)
        {
            List<RegulationMap> results = new List<RegulationMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Regulation_Map_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<RegulationMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
    }
}
