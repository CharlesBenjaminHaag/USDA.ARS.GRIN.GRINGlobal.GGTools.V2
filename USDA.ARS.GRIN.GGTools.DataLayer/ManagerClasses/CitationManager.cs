using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CitationManager : GRINGlobalDataManagerBase, IManager<Citation, CitationSearch>
    {
        public int Insert(Citation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Citation>(entity);
            SQL = "usp_GRINGlobal_Citation_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_citation_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_citation_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        
        public int Update(Citation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Citation>(entity);
            SQL = "usp_GRINGlobal_Citation_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return RowsAffected;
        }
        public int UpdateSpeciesCitation(string tableName, int entityId, int citationId, int modifiedBy)
        {
            Reset(CommandType.StoredProcedure);
            SQL = "usp_GRINGlobal_Citation_Reference_Update";

            AddParameter("table_name", (object)tableName, true);
            AddParameter("id_value", (object)entityId, true);
            AddParameter("citation_id", citationId > 0 ? (object)citationId : DBNull.Value, true);
            AddParameter("modified_by", (object)modifiedBy, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return RowsAffected;
        }
        public int Delete(Citation entity)
        {
            throw new NotImplementedException();
        }
       
        public Citation Get(int entityId)
        {
            Citation citation = new Citation();
            SQL = "usp_GRINGlobal_Citation_Select";
            List<ReportItem> reportItems = new List<ReportItem>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("citation_id", (object)entityId, false)
            };
            citation = GetRecord<Citation>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return citation;
        }

        public List<Citation> GetSpeciesCitations(int speciesId, string tableName)
        {
            List<Citation> results = new List<Citation>();

            if (tableName.Contains("cwr"))
            {
                SQL = "SELECT * FROM vw_GRINGlobal_Citation " +
                    " WHERE SpeciesID = @SpeciesID " +
                    " AND TypeCode = 'RELATIVE' " +
                    " OR SpeciesID IN " +
                    " (SELECT SpeciesAID " +
                    " FROM vw_GRINGlobal_Taxonomy_Species_Synonym_Map " +
                    " WHERE SpeciesBID = @SpeciesID " +
                    " AND TypeCode = 'RELATIVE') ";
            }
            else
            {
                SQL = " SELECT * FROM vw_GRINGlobal_Citation " +
                        " WHERE SpeciesID = @SpeciesID " +
                        " OR " +
                        " SpeciesID IN " +
                        " (SELECT SpeciesAID " +
                        " FROM vw_GRINGlobal_Taxonomy_Species_Synonym_Map " +
                        " WHERE SpeciesBID = @SpeciesID) ";
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("SpeciesID", speciesId > 0 ? (object)speciesId : DBNull.Value, true)
            };
            results = GetRecords<Citation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public List<ReportItem> GetCitationReferenceCounts(int citationId)
        {
            SQL = "usp_GRINGlobal_Citation_Reference_Counts_Select";
            List<ReportItem> reportItems = new List<ReportItem>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("citation_id", (object)citationId, false)
            };

            reportItems = GetRecords<ReportItem>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return reportItems.Where(x=>x.Total > 0).ToList();
        }
        
        public List<Citation> Search(CitationSearch searchEntity)
        {
            List<Citation> results = new List<Citation>();

            SQL = "SELECT * FROM vw_GRINGlobal_Citation ";
            SQL += " WHERE      (@ID                            IS NULL OR ID               =       @ID)";
            SQL += " AND        (@CitationTitle                 IS NULL OR CitationTitle    LIKE    '%' + @CitationTitle + '%')";
            SQL += " AND        (@AuthorName                    IS NULL OR AuthorName       LIKE    '%' + @AuthorName + '%')";
            SQL += " AND        (@CitationYear                  IS NULL OR CitationYear     =       @CitationYear)";
            SQL += " AND        (@Reference                     IS NULL OR Reference        LIKE    '%' + @Reference + '%')";
            SQL += " AND        (@DOIReference                  IS NULL OR DOIReference     LIKE    '%' + @DOIReference + '%')";
            SQL += " AND        (@URL                           IS NULL OR URL              LIKE    '%' + @URL + '%')";
            SQL += " AND        (@Title                         IS NULL OR Title            LIKE    '%' + @Title + '%')";
            SQL += " AND        (@Description                   IS NULL OR Description      LIKE    '%' + @Description + '%')";
            // OMITTED
            // AccessionID
            // MethodID
            SQL += " AND        (@FamilyID                      IS NULL OR FamilyID         =       @FamilyID)";
            SQL += " AND        (@GenusID                       IS NULL OR GenusID          =       @GenusID)";
            SQL += " AND        (@SpeciesID                     IS NULL OR SpeciesID        =       @SpeciesID)";
            SQL += " AND        (@FamilyName                    IS NULL OR FamilyName       LIKE    '%' +       @FamilyName + '%') ";
            SQL += " AND        (@GenusName                     IS NULL OR GenusName        LIKE    '%' +       @GenusName + '%') ";
            SQL += " AND        (@SpeciesName                   IS NULL OR SpeciesName      LIKE    '%' + @SpeciesName + '%') ";
            // NOTE: OMITTED
            // AccessionIPRID
            // AccessionPedigreeID
            // GeneticMarkerID
            SQL += " AND        (@TypeCode                      IS NULL OR TypeCode         =               @TypeCode)";
            SQL += " AND        (@UniqueKey                     IS NULL OR UniqueKey        =               @UniqueKey)";
            SQL += " AND        (@IsAcceptedName                IS NULL OR IsAcceptedName   =               @IsAcceptedName)";
            SQL += " AND        (@LiteratureID                  IS NULL OR LiteratureID     =               @LiteratureID)";
            SQL += " AND        (@Abbreviation                  IS NULL OR Abbreviation     LIKE            '%' + @Abbreviation + '%')";
            //SQL += " AND        (@StandardAbbreviation        IS NULL OR StandardAbbreviation     =       @StandardAbbreviation)";
            //SQL += " AND        (@EditorAuthorName            IS NULL OR EditorAuthorName         LIKE    '%' + @EditorAuthorName + '%')";
            //SQL += " AND        (@ReferenceTitle              IS NULL OR ReferenceTitle           LIKE    '%' + @ReferenceTitle + '%')";
            //SQL += " AND        (@LiteratureTypeCode          IS NULL OR LiteratureTypeCode       =       @LiteratureTypeCode)";
            //SQL += " AND        (@PublicationYear             IS NULL OR PublicationYear          =       @PublicationYear)";
            //SQL += " AND        (@PublisherName               IS NULL OR PublisherName            LIKE    '%' + @PublisherName + '%')";
            //SQL += " AND        (@PublisherLocation           IS NULL OR PublisherLocation        LIKE    '%' + @PublisherLocation + '%')";
            SQL += " AND        (@Note                          IS NULL OR Note                     LIKE    '%' + @Note + '%')";
            SQL += " AND        (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND        (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND        (@ModifiedDate                  IS NULL OR ModifiedDate             =       @ModifiedDate)";
            SQL += " AND        (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND        (@OwnedDate                     IS NULL OR OwnedDate                =       @OwnedDate)";
            SQL += " AND        (@OwnedByCooperatorID           IS NULL OR OwnedByCooperatorID      =       @OwnedByCooperatorID)";

            if (!String.IsNullOrEmpty(searchEntity.SpeciesIDList))
            {
                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }
                SQL += " SpeciesID IN (" + searchEntity.SpeciesIDList + ")";
            }

            SQL += " ORDER BY CitationTitle ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CitationTitle", (object)searchEntity.CitationTitle ?? DBNull.Value, true),
                CreateParameter("AuthorName", (object)searchEntity.AuthorName ?? DBNull.Value, true),
                CreateParameter("CitationYear", searchEntity.CitationYear > 0 ? (object)searchEntity.CitationYear : DBNull.Value, true),
                CreateParameter("Reference", (object)searchEntity.Reference ?? DBNull.Value, true),
                CreateParameter("DOIReference", (object)searchEntity.DOIReference ?? DBNull.Value, true),
                CreateParameter("URL", (object)searchEntity.URL ?? DBNull.Value, true),
                CreateParameter("Title", (object)searchEntity.Title ?? DBNull.Value, true),
                CreateParameter("Description", (object)searchEntity.Title ?? DBNull.Value, true),
                // RESERVED
                // AccessionID
                // MethodID
                CreateParameter("FamilyID", searchEntity.FamilyID > 0 ? (object)searchEntity.FamilyID : DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("GenusID", searchEntity.GenusID > 0 ? (object)searchEntity.GenusID : DBNull.Value, true),
                CreateParameter("GenusName", (object)searchEntity.GenusName ?? DBNull.Value, true),
                CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                // NOTE: OMITTED
                // AccessionIPRID
                // AccessionPedigreeID
                // GeneticMarkerID
                CreateParameter("TypeCode", (object)searchEntity.TypeCode ?? DBNull.Value, true),
                CreateParameter("UniqueKey", searchEntity.UniqueKey > 0 ? (object)searchEntity.UniqueKey : DBNull.Value, true),
                CreateParameter("IsAcceptedName", (object)searchEntity.IsAcceptedName ?? DBNull.Value, true),
                CreateParameter("LiteratureID", searchEntity.LiteratureID > 0 ? (object)searchEntity.LiteratureID : DBNull.Value, true),
                CreateParameter("Abbreviation", (object)searchEntity.Abbreviation ?? DBNull.Value, true),
                //CreateParameter("StandardAbbreviation", (object)searchEntity.StandardAbbreviation ?? DBNull.Value, true),
                //CreateParameter("EditorAuthorName", (object)searchEntity.EditorAuthorName ?? DBNull.Value, true),
                //CreateParameter("ReferenceTitle", (object)searchEntity.ReferenceTitle ?? DBNull.Value, true),
                //CreateParameter("LiteratureTypeCode", (object)searchEntity.LiteratureTypeCode ?? DBNull.Value, true),
                //CreateParameter("PublicationYear", (object)searchEntity.PublicationYear ?? DBNull.Value, true),
                //CreateParameter("PublisherName", (object)searchEntity.PublisherName ?? DBNull.Value, true),
                //CreateParameter("PublisherLocation", (object)searchEntity.PublisherLocation ?? DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("OwnedByCooperatorID", searchEntity.OwnedByCooperatorID > 0 ? (object)searchEntity.OwnedByCooperatorID : DBNull.Value, true),
                CreateParameter("OwnedDate", searchEntity.OwnedDate > DateTime.MinValue ? (object)searchEntity.OwnedDate : DBNull.Value, true),
            };

            results = GetRecords<Citation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Citation> GetFolderItems(CitationSearch searchEntity)
        {
            List<Citation> results = new List<Citation>();

            SQL = " SELECT * FROM vw_GRINGlobal_Citation_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Citation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        public void BuildInsertUpdateParameters(Citation entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("citation_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            if (entity.AcceptedID == 0)
            {
                entity.AcceptedID = entity.ID;
            }

            if (String.IsNullOrEmpty(entity.IsAcceptedName))
            {
                entity.IsAcceptedName = "N";
            }

            // HACK: Need alternative.
            if (entity.CitationYear == null)
            {
                entity.CitationYear = 0;
            }

            AddParameter("literature_id", entity.LiteratureID == 0 ? DBNull.Value : (object)entity.LiteratureID, true);
            AddParameter("citation_title", String.IsNullOrEmpty(entity.CitationTitle) ? DBNull.Value : (object)entity.CitationTitle, true);
            AddParameter("author_name", String.IsNullOrEmpty(entity.AuthorName) ? DBNull.Value : (object)entity.AuthorName, true);
            AddParameter("citation_year", entity.CitationYear == 0 ? DBNull.Value : (object)entity.CitationYear, true);
            AddParameter("reference", String.IsNullOrEmpty(entity.Reference) ? DBNull.Value : (object)entity.Reference, true);
            AddParameter("doi_reference", String.IsNullOrEmpty(entity.DOIReference) ? DBNull.Value : (object)entity.DOIReference, true);
            AddParameter("url", String.IsNullOrEmpty(entity.URL) ? DBNull.Value : (object)entity.URL, true);
            AddParameter("title", String.IsNullOrEmpty(entity.ReferenceTitle) ? DBNull.Value : (object)entity.ReferenceTitle, true);
            AddParameter("description", String.IsNullOrEmpty(entity.ReferenceDescription) ? DBNull.Value : (object)entity.ReferenceDescription, true);
            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("taxonomy_genus_id", entity.GenusID == 0 ? DBNull.Value : (object)entity.GenusID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("type_code", String.IsNullOrEmpty(entity.TypeCode) ? DBNull.Value : (object)entity.TypeCode, true);
            AddParameter("is_accepted_name", String.IsNullOrEmpty(entity.IsAcceptedName) ? DBNull.Value : (object)entity.IsAcceptedName, true);
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
        
       

        public virtual List<Cooperator> GetOwnedByCooperators(string tableName)
        {
            SQL = "usp_GRINGlobal_Cooperators_Owned_By_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("table_name", (object)tableName, false)
            };
            List<Cooperator> cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            RowsAffected = cooperators.Count;
            return cooperators;
        }

        public Dictionary<string, string> GetTableNames()
        {
            return new Dictionary<string, string>
            {
                { "taxonomy_family", "Family" },
                { "taxonomy_genus", "Genus" },
                { "taxonomy_species", "Species" }
            };
        }
        public List<CodeValue> GetStandardAbbreviations()
        {
            List<CodeValue> codeValues = new List<CodeValue>();

            SQL = "SELECT standard_abbreviation AS Value, standard_abbreviation AS Title " + 
                  " FROM literature " +
                  " WHERE standard_abbreviation IS NOT NULL " +
                  " ORDER BY standard_abbreviation";
            codeValues = GetRecords<CodeValue>(SQL);
            RowsAffected = codeValues.Count;
            return codeValues;
        }
        public List<CodeValue> GetAbbreviations()
        {
            List<CodeValue> codeValues = new List<CodeValue>();

            SQL = "SELECT abbreviation AS Value, abbreviation AS Title " +
                  " FROM literature " +
                  " WHERE abbreviation IS NOT NULL " +
                  " ORDER BY abbreviation";
            codeValues = GetRecords<CodeValue>(SQL);
            RowsAffected = codeValues.Count;
            return codeValues;
        }
    }
}
