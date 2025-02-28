using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class GenusManager : GRINGlobalDataManagerBase, IManager<Genus, GenusSearch>
    {
        public int Delete(Genus entity)
        {
            throw new NotImplementedException();
        }

        public Genus Get(int entityId)
        {
            Genus genus = new Genus();
            SQL = "usp_GRINGlobal_Taxonomy_Genus_Select";
           
            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_genus_id", (object)entityId, false)
            };
            genus = GetRecord<Genus>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return genus;
        }
        public List<Genus> GetFolderItems(GenusSearch searchEntity)
        {
            List<Genus> results = new List<Genus>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Genus_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Genus>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public List<Genus> GetSynonyms(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Genus_Synonyms_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@taxonomy_genus_id", (object)entityId, false)
            };
            List<Genus> genera = GetRecords<Genus>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return genera;
        }
        public List<Genus> GetSubdivisions(string genusName)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Genus_Subdivisions_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@genus_name", (object)genusName, false)
            };
            List<Genus> genera = GetRecords<Genus>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return genera;
        }
        public int Insert(Genus entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Genus>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Genus_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_genus_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_genus_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public int InsertInfrageneric(Genus entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Genus>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Genus_Infrageneric_Insert";

            BuildInfrageneticInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_genus_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_genus_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        public Genus GetGenus(string genusName)
        {
            GenusSearch searchEntity = new GenusSearch { Name = genusName };

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Genus WHERE Name = @Name AND Rank='GENUS'";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("Name", (object)searchEntity.Name ?? DBNull.Value, true)
            };
            Genus result = GetRecord<Genus>(SQL, parameters.ToArray());
            return result;
        }

        public List<Genus> Search(GenusSearch searchEntity)
        {
            List<Genus> results = new List<Genus>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Genus";
            
            // Common extended fields
            SQL += " WHERE  (@ID   IS NULL OR ID       =         @ID)";
            SQL += " AND    (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@CreatedDate                   IS NULL OR CreatedDate              =       @CreatedDate)";
            SQL += " AND    (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND    (@ModifiedDate                  IS NULL OR ModifiedDate             =       @ModifiedDate)";
            SQL += " AND    (@Note                          IS NULL OR Note                     LIKE    '%' + @Note + '%')";

            SQL += " AND    (@FamilyMapID                   IS NULL OR FamilyMapID              =       @FamilyMapID)";
            SQL += " AND    (@FamilyName                    IS NULL OR FamilyName               LIKE    '%' + @FamilyName + '%')";
            SQL += " AND    (@Name                          IS NULL OR Name                     LIKE    @Name + '%')";
            SQL += " AND    (@IsAcceptedName                IS NULL OR IsAcceptedName           =       @IsAcceptedName)";
            SQL += " AND    (@AcceptedName                  IS NULL OR AcceptedName             LIKE    @AcceptedName + '%')";
            SQL += " AND    (@Rank                          IS NULL OR Rank                     LIKE    '%' + @Rank + '%')";
            SQL += " AND    (@Authority                     IS NULL OR Authority                LIKE    '%' + @Authority + '%')";
            SQL += " AND    (@QualifyingCode                IS NULL OR QualifyingCode           =       @QualifyingCode)";
            SQL += " AND    (@HybridCode                    IS NULL OR HybridCode               =       @HybridCode)";
            SQL += " ORDER BY Name ";

            var parameters = new List<IDbDataParameter> {
                // Common extended fields
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),

                CreateParameter("FamilyMapID", searchEntity.FamilyID > 0 ? (object)searchEntity.FamilyID : DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("Name", (object)searchEntity.Name ?? DBNull.Value, true),
                CreateParameter("IsAcceptedName", (object)searchEntity.IsAcceptedName ?? DBNull.Value, true),
                CreateParameter("AcceptedName", (object)searchEntity.AcceptedName ?? DBNull.Value, true),
                CreateParameter("Rank", (object)searchEntity.Rank ?? DBNull.Value, true),
                CreateParameter("Authority", (object)searchEntity.Authority ?? DBNull.Value, true),
                CreateParameter("QualifyingCode", (object)searchEntity.QualifyingCode ?? DBNull.Value, true),
                CreateParameter("HybridCode", (object)searchEntity.HybridCode ?? DBNull.Value, true),
            };

            results = GetRecords<Genus>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Genus> SearchFolderItems(GenusSearch searchEntity)
        {
            List<Genus> results = new List<Genus>();

            SQL = " SELECT vgtcn.* FROM vw_GRINGlobal_Taxonomy_Genus vgtcn JOIN vw_GRINGlobal_App_User_Item_List vgga " +
                   " ON vgtcn.ID = vgga.EntityID WHERE vgga.TableName = 'taxonomy_genus' ";
            SQL += "AND  (@FolderID                          IS NULL OR  FolderID       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Genus>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(Genus entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Genus>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Genus_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            return RowsAffected;
        }

        public int UpdateInfrageneric(Genus entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Genus>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Genus_Infrageneric_Update";

            BuildInfrageneticInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            return RowsAffected;
        }

        public void BuildInsertUpdateParameters(Genus entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_genus_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("current_taxonomy_genus_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("qualifying_code", (object)entity.QualifyingCode ?? DBNull.Value, true);
            AddParameter("hybrid_code", (object)entity.HybridCode ?? DBNull.Value, true);
            AddParameter("genus_name", String.IsNullOrEmpty(entity.Name) ? DBNull.Value : (object)entity.Name, true);
            AddParameter("subgenus_name", String.IsNullOrEmpty(entity.SubgenusName) ? DBNull.Value : (object)entity.SubgenusName, true);
            AddParameter("section_name", String.IsNullOrEmpty(entity.SectionName) ? DBNull.Value : (object)entity.SectionName, true);
            AddParameter("subsection_name", String.IsNullOrEmpty(entity.SubsectionName) ? DBNull.Value : (object)entity.SubsectionName, true);
            AddParameter("series_name", String.IsNullOrEmpty(entity.SeriesName) ? DBNull.Value : (object)entity.SeriesName, true);
            AddParameter("subseries_name", String.IsNullOrEmpty(entity.SubseriesName) ? DBNull.Value : (object)entity.SubseriesName, true);
            AddParameter("genus_authority", String.IsNullOrEmpty(entity.Authority) ? DBNull.Value : (object)entity.Authority, true);
            AddParameter("is_web_visible", entity.IsWebVisible == null ? "N" : (object)entity.IsWebVisible, false);
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

        public void BuildInfrageneticInsertUpdateParameters(Genus entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_genus_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            else
            {
                AddParameter("taxonomy_genus_id", entity.ParentID == 0 ? DBNull.Value : (object)entity.ParentID, true);
            }
            AddParameter("current_taxonomy_genus_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("qualifying_code", (object)entity.QualifyingCode ?? DBNull.Value, true);
            AddParameter("hybrid_code", (object)entity.HybridCode ?? DBNull.Value, true);
            AddParameter("subgenus_name", String.IsNullOrEmpty(entity.SubgenusName) ? DBNull.Value : (object)entity.SubgenusName, true);
            AddParameter("section_name", String.IsNullOrEmpty(entity.SectionName) ? DBNull.Value : (object)entity.SectionName, true);
            AddParameter("subsection_name", String.IsNullOrEmpty(entity.SubsectionName) ? DBNull.Value : (object)entity.SubsectionName, true);
            AddParameter("series_name", String.IsNullOrEmpty(entity.SeriesName) ? DBNull.Value : (object)entity.SeriesName, true);
            AddParameter("subseries_name", String.IsNullOrEmpty(entity.SubseriesName) ? DBNull.Value : (object)entity.SubseriesName, true);
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

        public List<Folder> GetFolders(string tableName)
        {
            List<Folder> results = new List<Folder>();

            SQL = "SELECT " +
                "taxonomy_folder_id AS ID, " +
                "title AS Title,	" +
                "category AS Category, " +
                "description AS Description," +
                "note AS Note," +
                "is_shared AS IsShared," +
                "is_favorite AS IsFavorite," +
                "data_source_name AS TableName," +
                "created_date AS CreatedDate," +
                "modified_date AS ModifiedDate " +
                "FROM " +
                "taxonomy_folder tf ";
            //SQL += "WHERE(@ID   IS NULL         OR   taxonomy_folder_id = @ID)";
            SQL += "WHERE(@TableName   IS NULL  OR   data_source_name = @TableName)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("TableName", (object)tableName ?? DBNull.Value, true),
            };

            results = GetRecords<Folder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        //REFACTOR CBH, 11/29/21
        #region Taxonomy Common
       
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

        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
