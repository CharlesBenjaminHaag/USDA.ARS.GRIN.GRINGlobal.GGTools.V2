using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class LiteratureManager : GRINGlobalDataManagerBase, IManager<Literature, LiteratureSearch>
    {
        public List<Literature> GetFolderItems(LiteratureSearch searchEntity)
        {
            List<Literature> results = new List<Literature>();

            SQL = " SELECT * FROM vw_GRINGlobal_Literature_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Literature>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Literature entity)
        {
            throw new NotImplementedException();
        }

        public Literature Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(Literature entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Literature>(entity);
            SQL = "usp_GRINGlobal_Literature_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_literature_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_literature_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public List<Literature> Search(LiteratureSearch searchEntity)
        {
            List<Literature> results = new List<Literature>();

            SQL = "SELECT * FROM vw_GRINGlobal_Literature ";
            SQL += " WHERE (@StandardAbbreviation       IS NULL OR StandardAbbreviation     LIKE    '%' +   @StandardAbbreviation + '%')";

            SQL += " AND    (@ID                        IS NULL OR ID                       =       @ID)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@CreatedDate               IS NULL OR CreatedDate              =       @CreatedDate)";
            SQL += " AND    (@ModifiedByCooperatorID    IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND    (@ModifiedDate              IS NULL OR ModifiedDate             =       @ModifiedDate)";
            SQL += " AND    (@Note                      IS NULL OR Note                     LIKE    '%' + @Note + '%')";

            SQL += " AND    (@Abbreviation              IS NULL OR Abbreviation             LIKE  '%' +   @Abbreviation + '%')";
            SQL += " AND    (@LiteratureTypeCode        IS NULL OR LiteratureTypeCode       =       @LiteratureTypeCode)";
            SQL += " AND    (@ReferenceTitle            IS NULL OR ReferenceTitle           LIKE  '%' +   @ReferenceTitle + '%')";
            SQL += " AND    (@Author                    IS NULL OR EditorAuthorName         LIKE  '%' +   @Author + '%')";
            SQL += " AND    (@PublicationYear           IS NULL OR PublicationYear          LIKE  '%' +   @PublicationYear + '%')";
            
            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),

                CreateParameter("Abbreviation", (object)searchEntity.Abbreviation ?? DBNull.Value, true),
                CreateParameter("StandardAbbreviation", (object)searchEntity.StandardAbbreviation ?? DBNull.Value, true),
                CreateParameter("LiteratureTypeCode", (object)searchEntity.LiteratureTypeCode ?? DBNull.Value, true),
                CreateParameter("ReferenceTitle", (object)searchEntity.ReferenceTitle  ?? DBNull.Value, true),
                CreateParameter("Author", (object)searchEntity.EditorAuthorName  ?? DBNull.Value, true),
                CreateParameter("PublicationYear", (object)searchEntity.PublicationYear  ?? DBNull.Value, true),
            };
            results = GetRecords<Literature>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(Literature entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Literature>(entity);
            SQL = "usp_GRINGlobal_Literature_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
           
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public List<Citation> GetCitations(int speciesId)
        {
            SQL = "usp_TaxonomyLiteratureCitations_Select";
            var parameters = new List<IDbDataParameter> {
            CreateParameter("taxonomy_literature_id", (object)speciesId, false)
            };
            return GetRecords<Citation>(SQL, CommandType.StoredProcedure, parameters.ToArray());
        }
        public void BuildInsertUpdateParameters(Literature entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("literature_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("abbreviation", String.IsNullOrEmpty(entity.Abbreviation) ? DBNull.Value : (object)entity.Abbreviation, true);
            AddParameter("standard_abbreviation", String.IsNullOrEmpty(entity.StandardAbbreviation) ? DBNull.Value : (object)entity.StandardAbbreviation, true);
            AddParameter("reference_title", String.IsNullOrEmpty(entity.ReferenceTitle) ? DBNull.Value : (object)entity.ReferenceTitle, true);
            AddParameter("editor_author_name", String.IsNullOrEmpty(entity.EditorAuthorName) ? DBNull.Value : (object)entity.EditorAuthorName, true);
            AddParameter("literature_type_code", String.IsNullOrEmpty(entity.LiteratureTypeCode) ? DBNull.Value : (object)entity.LiteratureTypeCode, true);
            AddParameter("publication_year", String.IsNullOrEmpty(entity.PublicationYear) ? DBNull.Value : (object)entity.PublicationYear, true);
            AddParameter("publisher_name", String.IsNullOrEmpty(entity.PublisherName) ? DBNull.Value : (object)entity.PublisherName, true);
            AddParameter("publisher_location", String.IsNullOrEmpty(entity.PublisherLocation) ? DBNull.Value : (object)entity.PublisherLocation, true);
            AddParameter("url", String.IsNullOrEmpty(entity.URL) ? DBNull.Value : (object)entity.URL, true);
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
