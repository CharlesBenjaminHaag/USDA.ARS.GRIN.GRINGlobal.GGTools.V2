using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class ClassificationManager : GRINGlobalDataManagerBase, IManager<Classification, ClassificationSearch>, IAnnotated<CodeValue>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public void BuildInsertUpdateParameters(Classification entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_order_id", (object)entity.ID, false);
            }
            AddParameter("order_name", (object)entity.OrderName ?? DBNull.Value, false);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            if (entity.ID > 0)
            {
                AddParameter("modified_by", (object)entity.ModifiedByCooperatorID ?? DBNull.Value, true);
            }
            else
            {
                AddParameter("created_by", (object)entity.CreatedByCooperatorID ?? DBNull.Value, true);
            }
        }

        public int Delete(Classification entity)
        {
            throw new NotImplementedException();
        }

        public Classification Get(int entityId)
        {
            throw new NotImplementedException();
        }
        public List<Classification> GetFolderItems(ClassificationSearch searchEntity)
        {
            List<Classification> results = new List<Classification>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Classification_Sys_Folder_Item_Map WHERE SysFolderID = @SysFolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysFolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Classification>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public int Insert(Classification entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<Classification>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Classification_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_order_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            entity.ID = GetParameterValue<int>("@out_taxonomy_order_id", -1);
            return entity.ID;
        }

        public List<Classification> Search(ClassificationSearch searchEntity)
        {
            List<Classification> results = new List<Classification>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Classification ";

            SQL += " WHERE  (@ID                        IS NULL OR  ID                      =       @ID)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR  CreatedByCooperatorID   =       @CreatedByCooperatorID)";
            SQL += " AND    (@OrderName                 IS NULL OR  OrderName               LIKE    '%' + @OrderName + '%' )";
            SQL += " AND    (@Note                      IS NULL OR  Note                    LIKE    '%' + @Note + '%' )";
            SQL += " ORDER BY OrderName ASC";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("OrderName", (object)searchEntity.Name ?? DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
            };

            results = GetRecords<Classification>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Classification> SearchFolderItems(ClassificationSearch searchEntity)
        {
            List<Classification> results = new List<Classification>();

            SQL = " SELECT vgtcn.* FROM vw_GRINGlobal_Taxonomy_Classification vgtcn JOIN vw_GRINGlobal_App_User_Item_List vgga " +
                    " ON vgtcn.ID = vgga.EntityID WHERE vgga.TableName = 'taxonomy_classification' ";
            SQL += "AND  (@FolderID                          IS NULL OR  FolderID       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Classification>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(Classification entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<Classification>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Classification_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            return entity.ID;
        }

        public int Map(int classificationId, string entityIdList)
        {
            string[] idCollection;

            idCollection = entityIdList.Split(',');
            foreach (var id in idCollection)
            {
//                InsertItem(appUserItemList);
            }
            return 0;
        }

        public List<CodeValue> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
