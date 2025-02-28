using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class RegulationManager : GRINGlobalDataManagerBase, IManager<Regulation, RegulationSearch>
    {
        public int Insert(Regulation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Regulation>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Regulation_Insert";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_regulation_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            entity.ID = GetParameterValue<int>("@out_taxonomy_regulation_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }

        public int Update(Regulation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Regulation>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Regulation_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }

        public int Delete(Regulation entity)
        {
            throw new NotImplementedException();
        }

        public Regulation Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Regulation> Search(RegulationSearch searchEntity)
        {
            List<Regulation> results = new List<Regulation>();

            SQL = "SELECT*  FROM vw_GRINGlobal_Taxonomy_Regulation ";

            // EXTENDED
            SQL += " WHERE      (@ID                            IS NULL OR ID                       =       @ID)";
            SQL += " AND        (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND        (@CreatedDate                   IS NULL OR CreatedDate              =       @CreatedDate)";
            SQL += " AND        (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND        (@ModifiedDate                  IS NULL OR ModifiedDate             =       @ModifiedDate)";
            SQL += " AND    (@Note                          IS NULL OR Note                     LIKE    '%' + @Note + '%')";

            SQL += " AND    (@GeographyID               IS NULL OR   GeographyID                =   @GeographyID)";
            SQL += " AND    (@RegulationTypeCode        IS NULL OR   RegulationTypeCode         =   @RegulationTypeCode)";
            SQL += " AND    (@RegulationLevelCode       IS NULL OR   RegulationLevelCode        =   @RegulationLevelCode)";
            SQL += " AND    (@Description               IS NULL OR   Description                =   @Description)";

            // EXTENDED
            SQL += " AND    (@URL1                      IS NULL OR URL1                     LIKE    '%' + @URL1 + '%')";
            SQL += " AND    (@URL2                      IS NULL OR URL2                     LIKE    '%' + @URL2 + '%')";

            SQL += " ORDER BY AssembledName ASC";
            
            var parameters = new List<IDbDataParameter> {
                // EXTENDED
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),

                CreateParameter("GeographyID", searchEntity.GeographyID > 0 ? (object)searchEntity.GeographyID : DBNull.Value, true),
                CreateParameter("RegulationTypeCode", (object)searchEntity.RegulationTypeCode ?? DBNull.Value, true),
                CreateParameter("RegulationLevelCode", (object)searchEntity.RegulationLevelCode ?? DBNull.Value, true),
                CreateParameter("Description", (object)searchEntity.Description ?? DBNull.Value, true),

               // EXTENDED
               CreateParameter("URL1", (object)searchEntity.URL1 ?? DBNull.Value, true),
               CreateParameter("URL2", (object)searchEntity.URL2 ?? DBNull.Value, true),
            };

            results = GetRecords<Regulation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
   
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
       
        protected virtual void BuildInsertUpdateParameters(Regulation entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_regulation_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("geography_id", entity.GeographyID == 0 ? DBNull.Value : (object)entity.GeographyID, true);
            AddParameter("regulation_type_code ", String.IsNullOrEmpty(entity.RegulationTypeCode) ? DBNull.Value : (object)entity.RegulationTypeCode, true);
            AddParameter("regulation_level_code", String.IsNullOrEmpty(entity.RegulationLevelCode) ? DBNull.Value : (object)entity.RegulationLevelCode, true);
            AddParameter("description", String.IsNullOrEmpty(entity.Description) ? DBNull.Value : (object)entity.Description, true);
            AddParameter("url_1", String.IsNullOrEmpty(entity.URL1) ? DBNull.Value : (object)entity.URL1, true);
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
        
        public List<Regulation> GetFolderItems(RegulationSearch searchEntity)
        {
            List<Regulation> results = new List<Regulation>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Regulation_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Regulation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
    }
}
