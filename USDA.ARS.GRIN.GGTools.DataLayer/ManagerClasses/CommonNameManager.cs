using System;
using System.Collections.Generic;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CommonNameManager : GRINGlobalDataManagerBase, IManager<CommonName, CommonNameSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(CommonName entity)
        {
            throw new NotImplementedException();
        }

        public CommonName Get(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Common_Name_Select";
            CommonName commonName = new CommonName();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_common_name_id", (object)entityId, false)
            };

            commonName = GetRecord<CommonName>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return commonName;
        }
        
        public List<CommonName> GetFolderItems(CommonNameSearch searchEntity)
        {
            List<CommonName> results = new List<CommonName>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Common_Name_Language_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<CommonName>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual int Insert(CommonName entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CommonName>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Common_Name_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_common_name_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddStandardParameters();

            RowsAffected = ExecuteNonQuery(false, "@out_taxonomy_common_name_id");
            entity.ID = GetParameterValue<int>("@out_taxonomy_common_name_id", -1);
            
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("SQL Error " + errorNumber.ToString());
            }

            RowsAffected = entity.ID;
            return RowsAffected;
        }

        public virtual int Update(CommonName entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CommonName>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Common_Name_Update";

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

        public List<CommonName> Search(CommonNameSearch searchEntity)
        {
            List<CommonName> results = new List<CommonName>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Common_Name ";

            SQL += " WHERE  (@ID   IS NULL OR ID            =         @ID)";
            SQL += " AND    (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID        =       @CreatedByCooperatorID)";
            SQL += " AND    (@CreatedDate                   IS NULL OR CreatedDate                  =       @CreatedDate)";
            SQL += " AND    (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID       =       @ModifiedByCooperatorID)";
            SQL += " AND    (@ModifiedDate                  IS NULL OR ModifiedDate                 =       @ModifiedDate)";
            SQL += " AND    (@Note                          IS NULL OR Note                         LIKE    '%' + @Note + '%')";
            SQL += " AND    (@GenusID                       IS NULL OR  GenusID                     =       @GenusID) ";
            SQL += " AND    (@SpeciesID                     IS NULL OR  SpeciesID                   =       @SpeciesID) ";
            SQL += " AND    (@CreatedByCooperatorID         IS NULL OR  CreatedByCooperatorID       =       @CreatedByCooperatorID)";
            SQL += " AND    (@GenusName                     IS NULL OR  GenusName                   LIKE    '%' + @GenusName + '%') ";
            SQL += " AND    (@SpeciesName                   IS NULL OR  SpeciesName                 LIKE    '%' + @SpeciesName + '%') ";

            if (searchEntity.IsNameExactMatch == "Y")
            {
                SQL += " AND    (@Name                          IS NULL OR  Name                    =       @Name) ";
            }
            else
            {
                SQL += " AND    (@Name                          IS NULL OR  Name                    LIKE    '%' + @Name + '%') ";
            }
            SQL += " AND    (@LanguageDescription           IS NULL OR  LanguageDescription         LIKE    '%' + @LanguageDescription + '%') ";
            SQL += " AND    (@SimplifiedName                IS NULL OR  SimplifiedName              LIKE    '%' + @SimplifiedName + '%') ";

            if (searchEntity.ExcludeID > 0)
            {
                SQL += " AND ID <> " + searchEntity.ExcludeID;
            }

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

                CreateParameter("GenusID", searchEntity.GenusID > 0 ? (object)searchEntity.GenusID : DBNull.Value, true),
                CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
                CreateParameter("GenusName", (object)searchEntity.GenusName ?? DBNull.Value, true),
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                CreateParameter("Name", (object)searchEntity.Name ?? DBNull.Value, true),
                CreateParameter("LanguageDescription", (object)searchEntity.LanguageDescription ?? DBNull.Value, true),
                CreateParameter("SimplifiedName", (object)searchEntity.SimplifiedName ?? DBNull.Value, true),
            };

            results = GetRecords<CommonName>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }
        public List<CommonName> SearchFolderItems(CommonNameSearch searchEntity)
        {
            List<CommonName> results = new List<CommonName>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgtcn.* " +
                " FROM vw_GRINGlobal_Taxonomy_Common_Name vgtcn " +
                " JOIN app_user_item_list auil " +
                " ON vgtcn.ID = auil.id_number " +
                " WHERE auil.id_type = 'taxonomy_common_name' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<CommonName>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
      
        public void GenerateSimplifiedName()
        { 
        // get
        //    {
        //        var cname = FullName.ToUpper();
        //string cname2 = cname.Replace("-", "").Replace("'", "").Replace(" ", "");
        //cname2 = cname2.Replace("&Aacute;", "A");
        //        cname2 = cname2.Replace("&aacute;", "a");
        //        cname2 = cname2.Replace("&Eacute;", "E");
        //        cname2 = cname2.Replace("&eacute;", "e");
        //        cname2 = cname2.Replace("&Iacute;", "I");
        //        cname2 = cname2.Replace("&iacute;", "i");
        //        cname2 = cname2.Replace("&Oacute;", "O");
        //        cname2 = cname2.Replace("&oacute;", "o");
        //        cname2 = cname2.Replace("&Uacute;", "U");
        //        cname2 = cname2.Replace("&uacute;", "u");
        //        cname2 = cname2.Replace("&yacute;", "y");
        //        cname2 = cname2.Replace("&abreve;", "a");
        //        cname2 = cname2.Replace("&gbreve;", "g");
        //        cname2 = cname2.Replace("&#301;", "i");
        //        cname2 = cname2.Replace("&Ccaron;", "C");
        //        cname2 = cname2.Replace("&ccaron;", "c");
        //        cname2 = cname2.Replace("&Ecaron;", "E");
        //        cname2 = cname2.Replace("&ecaron;", "e");
        //        cname2 = cname2.Replace("&Rcaron;", "R");
        //        cname2 = cname2.Replace("&rcaron;", "r");
        //        cname2 = cname2.Replace("&Scaron;", "S");
        //        cname2 = cname2.Replace("&scaron;", "s");
        //        cname2 = cname2.Replace("&Zcaron;", "Z");
        //        cname2 = cname2.Replace("&zcaron;", "z");
        //        cname2 = cname2.Replace("&Ccedil;", "C");
        //        cname2 = cname2.Replace("&ccedil;", "c");
        //        cname2 = cname2.Replace("&Scedil;", "S");
        //        cname2 = cname2.Replace("&scedil;", "s");
        //        cname2 = cname2.Replace("&acirc;", "a");
        //        cname2 = cname2.Replace("&ecirc;", "e");
        //        cname2 = cname2.Replace("&Icirc;", "I");
        //        cname2 = cname2.Replace("&icirc;", "i");
        //        cname2 = cname2.Replace("&ocirc;", "o");
        //        cname2 = cname2.Replace("&scirc;", "s");
        //        cname2 = cname2.Replace("&ucirc;", "u");
        //        cname2 = cname2.Replace("&agrave;", "a");
        //        cname2 = cname2.Replace("&egrave;", "e");
        //        cname2 = cname2.Replace("&igrave;", "i");
        //        cname2 = cname2.Replace("&ograve;", "o");
        //        cname2 = cname2.Replace("&Aring;", "A");
        //        cname2 = cname2.Replace("&aring;", "a");
        //        cname2 = cname2.Replace("&Oslash;", "O");
        //        cname2 = cname2.Replace("&oslash;", "o");
        //        cname2 = cname2.Replace("&aelig;", "ae");
        //        cname2 = cname2.Replace("&oelig;", "oe");
        //        cname2 = cname2.Replace("&szlig;", "ss");
        //        cname2 = cname2.Replace("&atilde;", "a");
        //        cname2 = cname2.Replace("&Ntilde;", "N");
        //        cname2 = cname2.Replace("&ntilde;", "n");
        //        cname2 = cname2.Replace("&otilde;", "o");
        //        cname2 = cname2.Replace("&Auml;", "A");
        //        cname2 = cname2.Replace("&auml;", "a");
        //        cname2 = cname2.Replace("&euml;", "e");
        //        cname2 = cname2.Replace("&iuml;", "i");
        //        cname2 = cname2.Replace("&Ouml;", "O");
        //        cname2 = cname2.Replace("&ouml;", "o");
        //        cname2 = cname2.Replace("&Uuml;", "U");
        //        cname2 = cname2.Replace("&uuml;", "u");
        //        return cname2;
        }

        

        

        protected virtual void BuildInsertUpdateParameters(CommonName entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_common_name_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("taxonomy_genus_id", entity.GenusID == 0 ? DBNull.Value : (object)entity.GenusID, true);
            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("taxonomy_common_name_language_id", entity.LanguageID == 0 ? DBNull.Value : (object)entity.LanguageID, true);
            AddParameter("name", (object)entity.Name ?? DBNull.Value, false);
            AddParameter("language_description", (object)entity.LanguageDescription ?? DBNull.Value, true);
            AddParameter("simplified_name", (object)entity.SimplifiedName ?? DBNull.Value, true);
            AddParameter("alternate_transcription", (object)entity.AlternateTranscription ?? DBNull.Value, false);
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
