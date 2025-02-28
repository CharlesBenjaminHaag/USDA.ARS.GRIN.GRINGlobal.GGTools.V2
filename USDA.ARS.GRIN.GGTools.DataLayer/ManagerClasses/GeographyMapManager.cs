using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class GeographyMapManager : GRINGlobalDataManagerBase, IManager<GeographyMap, GeographyMapSearch>
    {
        public virtual int Insert(GeographyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<GeographyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Geography_Map_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_geography_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddStandardParameters();

            RowsAffected = ExecuteNonQuery(false, "@out_taxonomy_geography_map_id");
            entity.ID = GetParameterValue<int>("@out_taxonomy_geography_map_id", -1);

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("SQL Error " + errorNumber.ToString());
            }

            RowsAffected = entity.ID;
            return entity.ID;
        }

        public virtual int Update(GeographyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<GeographyMap>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Geography_Map_Update";

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

        public int Delete(GeographyMap entity)
        {
            throw new NotImplementedException();
        }

        public GeographyMap Get(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Geography_Map_Select";
            GeographyMap geographyMap = new GeographyMap();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_geography_map_id", (object)entityId, false)
            };

            geographyMap = GetRecord<GeographyMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return geographyMap;
        }
        
        public List<GeographyMap> GetFolderItems(GeographyMapSearch searchEntity)
        {
            List<GeographyMap> results = new List<GeographyMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Geography_Map_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<GeographyMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public void GetList(int[] idList)
        { 
        
        }

        public List<GeographyMap> Search(GeographyMapSearch searchEntity)
        {
            List<GeographyMap> results = new List<GeographyMap>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Geography_Map ";
            SQL += "WHERE (@GeographyDescription        IS NULL OR      GeographyDescription        LIKE  '%' + @GeographyDescription + '%')";
            SQL += " AND  ( @ID   IS NULL OR ID                                                      =       @ID)";
            SQL += " AND    (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID        =       @CreatedByCooperatorID)";
            SQL += " AND    (@CreatedDate                   IS NULL OR CreatedDate                  =       @CreatedDate)";
            SQL += " AND    (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID       =       @ModifiedByCooperatorID)";
            SQL += " AND    (@ModifiedDate                  IS NULL OR ModifiedDate                 =       @ModifiedDate)";
            SQL += " AND    (@Note                          IS NULL OR Note                         LIKE    '%' + @Note + '%')";
            SQL += " AND    (@GeographyID                   IS NULL OR      GeographyID             =           @GeographyID)";
            SQL += " AND    (@SpeciesID                     IS NULL OR      SpeciesID               =           @SpeciesID)";
            SQL += " AND    (@SpeciesName                   IS NULL OR      SpeciesName             LIKE  '%' + @SpeciesName + '%')";
            SQL += " AND    (@GeographyDescription          IS NULL OR      GeographyDescription    =           @GeographyDescription)";
            SQL += " AND    (@GeographyStatusCode       IS NULL OR      GeographyStatusCode         =           @GeographyStatusCode)";
            SQL += " AND    (@CountryCode               IS NULL OR      CountryCode                 =           @CountryCode)";
            SQL += " AND    (@CountryName               IS NULL OR      CountryName                 LIKE  '%' + @CountryName + '%')";
            SQL += " AND    (@IsCited                   IS NULL OR      IsCited                      =           @IsCited)";
            SQL += " AND    (@IsValid                   IS NULL OR      IsValid                      =           @IsValid)";

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

            if (!String.IsNullOrEmpty(searchEntity.GeographyMapIDList))
            {
                SQL += " AND    ID IN (" + searchEntity.GeographyMapIDList + ")";
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),

                CreateParameter("GeographyDescription", (object)searchEntity.GeographyDescription ?? DBNull.Value, true),
                CreateParameter("GeographyID", searchEntity.GeographyID > 0 ? (object)searchEntity.GeographyID : DBNull.Value, true),
                CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                CreateParameter("GeographyStatusCode", (object)searchEntity.GeographyStatusCode ?? DBNull.Value, true),
                CreateParameter("CountryCode", (object)searchEntity.CountryCode ?? DBNull.Value, true),
                CreateParameter("CountryName", (object)searchEntity.CountryName  ?? DBNull.Value, true),
                CreateParameter("IsCited", (object)searchEntity.IsCited ?? DBNull.Value, true),
                CreateParameter("IsValid", (object)searchEntity.IsValid ?? DBNull.Value, true),
            };

            results = GetRecords<GeographyMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<GeographyMap> SearchFolderItems(GeographyMapSearch searchEntity)
        {
            List<GeographyMap> results = new List<GeographyMap>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgtgm.* " +
                " FROM vw_GRINGlobal_Taxonomy_Geography_Map vgtgm " +
                " JOIN app_user_item_list auil " +
                " ON vgtgm.ID = auil.id_number " +
                " WHERE auil.id_type = 'taxonomy_geography_map' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<GeographyMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public int AddCitation(int citationId, int id)
        {
            int newCitationId = 0;
            int errorNumber = 0;
            Reset(CommandType.StoredProcedure);
            SQL = "usp_TaxonomyCitationGeographyMapClone_Insert";

            AddParameter("@citation_id", (object)citationId, false);
            AddParameter(("@taxonomy_geography_map_id"), (object)id, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_citation_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            // Get the primary key generated from the SQL Server IDENTITY
            newCitationId = GetParameterValue<int>("out_citation_id", -1);
            errorNumber = GetParameterValue<int>("@out_error_number", -1);

            return RowsAffected;
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

        #endregion

        protected virtual void BuildInsertUpdateParameters(GeographyMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_geography_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("geography_id", entity.GeographyID == 0 ? DBNull.Value : (object)entity.GeographyID, true);
            AddParameter("geography_status_code", (object)entity.GeographyStatusCode ?? DBNull.Value, false);
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
