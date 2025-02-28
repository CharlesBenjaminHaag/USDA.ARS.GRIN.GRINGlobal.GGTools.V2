using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CommonNameLanguageManager : GRINGlobalDataManagerBase, IManager<CommonNameLanguage, CommonNameLanguageSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(CommonNameLanguage entity)
        {
            throw new NotImplementedException();
        }

        public CommonNameLanguage Get(int entityId)
        {
            throw new NotImplementedException();
        }
        public List<CommonNameLanguage> GetFolderItems(CommonNameLanguageSearch searchEntity)
        {
            List<CommonNameLanguage> results = new List<CommonNameLanguage>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Common_Name_Language_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<CommonNameLanguage>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public int Insert(CommonNameLanguage entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CommonNameLanguage>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Common_Name_Language_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_common_name_language_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_common_name_language_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
            {
                throw new Exception("SQL Error " + errorNumber.ToString());
            }

            return entity.ID;
        }

        public List<CommonNameLanguage> Search(CommonNameLanguageSearch searchEntity)
        {
            List<CommonNameLanguage> results = new List<CommonNameLanguage>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Common_Name_Language ";
            SQL += " WHERE(@CreatedByCooperatorID     IS NULL OR CreatedByCooperatorID = @CreatedByCooperatorID)";
            
            SQL += " AND  (@ID   IS NULL OR ID       =         @ID)";
            SQL += " AND    (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@CreatedDate                   IS NULL OR CreatedDate              =       @CreatedDate)";
            SQL += " AND    (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND    (@ModifiedDate                  IS NULL OR ModifiedDate             =       @ModifiedDate)";
            SQL += " AND    (@Note                          IS NULL OR Note                     LIKE    '%' + @Note + '%')";

            SQL += " AND(@LanguageName              IS NULL OR LanguageName             LIKE '%' + @LanguageName + '%')";
            SQL += " AND(@LanguageSimplifiedName    IS NULL OR LanguageSimplifiedName   LIKE '%' + @LanguageSimplifiedName + '%')";
            SQL += " AND(@LanguageTranscription     IS NULL OR LanguageTranscription    LIKE '%' + @LanguageTranscription + '%')";
            SQL += " AND(@CountryCode               IS NULL OR CountryCode = @CountryCode)";

            SQL += " ORDER BY LanguageName ASC";

            var parameters = new List<IDbDataParameter>
            {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),

                CreateParameter("LanguageName", (object)searchEntity.LanguageName ?? DBNull.Value, true),
                CreateParameter("LanguageSimplifiedName", (object)searchEntity.LanguageSimplifiedName ?? DBNull.Value, true),
                CreateParameter("LanguageTranscription", (object)searchEntity.LanguageTranscription ?? DBNull.Value, true),
                CreateParameter("CountryCode", (object)searchEntity.CountryCode ?? DBNull.Value, true),
            };

            results = GetRecords<CommonNameLanguage>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;







        }

        public int Update(CommonNameLanguage entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CommonNameLanguage>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Common_Name_Language_Update";

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
        
        public int UpdateCountry(int entityId, string countryCode, int modifiedByCooperatorId)
        {
            Reset(CommandType.StoredProcedure);
            SQL = "usp_GRINGlobal_Taxonomy_Common_Name_Language_Country_Update";

            AddParameter("taxonomy_common_name_lang_id", (object)entityId, true);
            AddParameter("country_code", (object)countryCode, true);
            AddParameter("modified_by", (object)modifiedByCooperatorId, true);

            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }

        protected virtual void BuildInsertUpdateParameters(CommonNameLanguage entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_common_name_language_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("language_name", (object)entity.LanguageName ?? DBNull.Value, false);
            AddParameter("language_simplified_name", (object)entity.LanguageSimplifiedName ?? DBNull.Value, true);
            AddParameter("language_transcription", (object)entity.LanguageTranscription ?? DBNull.Value, false);
            AddParameter("country_code", (object)entity.CountryCode ?? DBNull.Value, false);
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
