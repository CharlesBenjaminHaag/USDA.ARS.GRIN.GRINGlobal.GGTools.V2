using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CropForCWRManager : GRINGlobalDataManagerBase, IManager<CropForCWR, CropForCWRSearch>
    {
        public virtual int Insert(CropForCWR entity)
        {
            // Reset all properties for calling a stored procedure
            Reset(CommandType.StoredProcedure);

            // Attempt to validate the data, a ValidationException is thrown if validation rules fail
            Validate<CropForCWR>(entity);

            // Create SQL to call a stored procedure
            SQL = "usp_GRINGlobal_Taxonomy_Cwr_Crop_Insert";

            // Create standard insert parameters
            BuildInsertUpdateParameters(entity);
            
            // Create parameter to get IDENTITY value generated
            AddParameter("@out_taxonomy_cwr_crop_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            // Add any standard parameters
            AddStandardParameters();

            // Execute Query
            RowsAffected = ExecuteNonQuery(false, "@out_taxonomy_cwr_crop_id");

            // Get the primary key generated from the SQL Server IDENTITY
            entity.ID = GetParameterValue<int>("@out_taxonomy_cwr_crop_id", -1);

            // Get standard output parameters
            GetStandardOutputParameters();

            return RowsAffected;
        }

        public virtual int Update(CropForCWR entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CropForCWR>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_CWR_Crop_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }

        public int Delete(CropForCWR entity) 
        { 
            return 0; 
        }

        public CropForCWR Get(int entityId)
        {
            throw new NotImplementedException();
        }
        public List<CropForCWR> GetFolderItems(CropForCWRSearch searchEntity)
        {
            List<CropForCWR> results = new List<CropForCWR>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_CWR_Crop_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<CropForCWR>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual List<CropForCWR> Search(CropForCWRSearch search)
        {
            // Create SQL to search for rows
            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_CWR_Crop";
            SQL += " WHERE (@CreatedByCooperatorID IS NULL OR CreatedByCooperatorID     =       @CreatedByCooperatorID)";
            SQL += " AND   (@Name                  IS NULL OR CropForCWRName            LIKE    '%' + @Name + '%')";
            SQL += " AND   (@ID                    IS NULL OR ID                        =       @ID)";
            SQL += " ORDER BY CropForCWRName ";
                
            var parameters = new List<IDbDataParameter> {
                CreateParameter("Name", (object)search.Name ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", search.CreatedByCooperatorID > 0 ? (object)search.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ID", search.ID > 0 ? (object)search.ID : DBNull.Value, true),
            };
            List<CropForCWR> cropForCWRs = GetRecords<CropForCWR>(SQL, parameters.ToArray());
            RowsAffected = cropForCWRs.Count;
            return cropForCWRs;
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
        
        protected virtual void BuildInsertUpdateParameters(CropForCWR entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_cwr_crop_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("crop_name", (object)entity.CropForCWRName ?? DBNull.Value, true);
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
    
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
    }
}
