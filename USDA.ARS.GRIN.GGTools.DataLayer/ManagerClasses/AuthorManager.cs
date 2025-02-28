using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class AuthorManager : GRINGlobalDataManagerBase, IManager<Author, AuthorSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        public int Delete(Author entity)
        {
            throw new NotImplementedException();
        }
        public Author Get(int entityId)
        {
            throw new NotImplementedException();
        }
        public List<Author> GetFolderItems(int sysFolderId)
        {
            List<Author> results = new List<Author>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Author_Sys_Folder_Item_Map WHERE SysFolderID = @SysFolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysFolderID", sysFolderId > 0 ? (object)sysFolderId : DBNull.Value, true)
            };
            results = GetRecords<Author>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public List<AuthorReference> GetReferences(string shortName)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Author_References_Select";
            List<AuthorReference> results = new List<AuthorReference>();
            
            var parameters = new List<IDbDataParameter> {
                CreateParameter("short_name", (object)shortName, false)
            };

            results = GetRecords<AuthorReference>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return results;
        }

        public int UpdateReferences(string originalValue, string newValue)
        {
            List<AuthorReference> authorReferences = new List<AuthorReference>();
            authorReferences = GetReferences(originalValue);

            foreach (var reference in authorReferences)
            {
                UpdateReference(reference);
            }
            return 0;
        }

        public int UpdateReference(AuthorReference entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<AuthorReference>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Author_Reference_Update";

            //AddParameter("table_name", (object)tableName, true);
            //AddParameter("field_name", (object)fieldName, true);
            //AddParameter("original_value", (object)originalValue, true);
            //AddParameter("new_value", (object)newValue, true);
            //TODO
            return 0;
        }

        public int Insert(Author entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Author>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Author_Insert";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_author_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            entity.ID = GetParameterValue<int>("@out_taxonomy_author_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }
        
        public List<Author> Search(AuthorSearch searchEntity)
        {
            List<Author> results = new List<Author>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Author";
            SQL += " WHERE  (@ID                IS NULL     OR  ID = @ID) ";
            SQL += " AND    (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@CreatedDate                   IS NULL OR CreatedDate              =       @CreatedDate)";
            SQL += " AND    (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND    (@ModifiedDate                  IS NULL OR ModifiedDate             =       @ModifiedDate)";
            SQL += " AND    (@Note                          IS NULL OR Note                     LIKE    '%' + @Note + '%')";

            SQL += " AND    (@FullName          IS NULL     OR FullName         LIKE    '%' + @FullName + '%')";

            if (searchEntity.ExcludeID > 0)
            {
                SQL += " AND ID <> " + searchEntity.ExcludeID.ToString();
            }

            if (searchEntity.IsShortNameExactMatch == "Y")
            {
                SQL += " AND      (@ShortName   IS NULL     OR ShortName        =       '' + @ShortName + '')";
            }
            else
            {
                SQL += " AND      (@ShortName   IS NULL     OR ShortName        LIKE    '%' + @ShortName + '%')";
            }
           
            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
               
                CreateParameter("FullName", (object)searchEntity.FullName ?? DBNull.Value, true),
                CreateParameter("ShortName", (object)searchEntity.ShortName ?? DBNull.Value, true)
            };

            results = GetRecords<Author>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public List<Author> SearchFolderItems(AuthorSearch searchEntity)
        {
            List<Author> results = new List<Author>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgta.* " +
                " FROM vw_GRINGlobal_Taxonomy_Author vgta " +
                " JOIN app_user_item_list auil " +
                " ON vgta.ID = auil.id_number " +
                " WHERE auil.id_type = 'taxonomy_author' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Author>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public int Update(Author entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Author>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Author_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }
        
        protected void BuildInsertUpdateParameters(Author entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_author_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("short_name", String.IsNullOrEmpty(entity.ShortName) ? DBNull.Value : (object)entity.ShortName, true);
            AddParameter("full_name", String.IsNullOrEmpty(entity.FullName) ? DBNull.Value : (object)entity.FullName, true);
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
    }
}
