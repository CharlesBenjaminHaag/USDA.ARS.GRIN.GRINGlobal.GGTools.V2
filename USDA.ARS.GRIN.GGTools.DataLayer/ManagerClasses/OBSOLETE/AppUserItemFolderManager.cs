using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer.OBSOLETE
{
    public partial class AppUserItemFolderManager : GRINGlobalDataManagerBase
    {
        public AppUserItemFolder Get(AppUserItemFolderSearch searchEntity)
        {
            AppUserItemFolder appUserItemFolder = new AppUserItemFolder();

            SQL = "usp_GRINGlobal_AppUserItemFolder_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("app_user_item_folder_id", (object)searchEntity.ID, false)
            };

            appUserItemFolder = GetRecord<AppUserItemFolder>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            
            return appUserItemFolder;
        }
        public virtual List<AppUserItemFolder> GetRelatedFolders(AppUserItemFolderSearch searchEntity)
        {
            List<AppUserItemFolder> results = new List<AppUserItemFolder>();

            SQL = " SELECT DISTINCT FolderID, FolderName " +
                    " FROM vw_GRINGlobal_Folder_" + searchEntity.TableName + " WHERE CreatedByCooperatorID = @CreatedByCooperatorID";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID, true)
            };
            results = GetRecords<AppUserItemFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual List<AppUserItemDynamicFolder> GetDynamicFolders(AppUserItemFolderSearch searchEntity)
        {
            List<AppUserItemDynamicFolder> results = new List<AppUserItemDynamicFolder>();

            SQL = " SELECT * " +
                    " FROM vw_GRINGlobal_App_User_Item_Dynamic_Folder " +
                    " WHERE CreatedByCooperatorID = @CreatedByCooperatorID " +
                    " AND DataType = @SysTableName";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID, true),
                CreateParameter("SysTableName", searchEntity.DataType, true)

        };
            results = GetRecords<AppUserItemDynamicFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual List<AppUserItemFolder> Search(AppUserItemFolderSearch searchEntity)
        {
            List<AppUserItemFolder> results = new List<AppUserItemFolder>();

            SQL = " SELECT * FROM vw_GRINGlobal_App_User_Item_Folder";
            SQL += " WHERE  (@FolderName                IS NULL OR   FolderName             LIKE    '%' + @FolderName + '%')";
            SQL += " AND    (@Category                  IS NULL OR   Category               =       @Category)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID  =       @CreatedByCooperatorID)";
            SQL += " AND    (@AppUserItemFolderID       IS NULL OR   ID                     =       @AppUserItemFolderID)";
            SQL += " AND    (@IsFavorite                IS NULL OR   IsFavorite             =       @IsFavorite)";
           SQL += " AND    (@FolderType                IS NULL OR   FolderType             =       @FolderType)";

            if (searchEntity.IsShared == "Y")
            {
                SQL += " AND ID IN (SELECT FolderID " +
                        " FROM vw_GRINGlobal_App_User_Item_Folder_Cooperator_Map " +
                        " WHERE CooperatorID = @SharedWithCooperatorID) ";
            }

            if (searchEntity.EntityID > 0)
            {
                SQL += " AND ID IN (SELECT AppUserItemFolderID " +
                        " FROM vw_GRINGlobal_App_User_Item_List " +
                        " WHERE IDNumber = @EntityID) ";
            }

            switch (searchEntity.TimeFrame)
            {
                case "1D":
                    SQL += " AND (CONVERT(date, CreatedDate) = CONVERT(date, GETDATE()))";
                    break;
                case "3D":
                    SQL += " AND  CreatedDate >= DATEADD(day,-3, GETDATE())";
                    break;
                case "7D":
                    SQL += " AND  CreatedDate >= DATEADD(day,-7, GETDATE())";
                    break;
                case "30D":
                    SQL += " AND  CreatedDate >= DATEADD(day,-30, GETDATE())";
                    break;
                case "60D":
                    SQL += " AND  CreatedDate >= DATEADD(day,-60, GETDATE())";
                    break;
                case "90D":
                    SQL += " AND  CreatedDate >= DATEADD(day,-90, GETDATE())";
                    break;
                case "YEAR":
                    SQL += " AND  DATEPART(year, CreatedDate) = DATEPART(year, GETDATE())";
                    break;
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderName", !String.IsNullOrEmpty(searchEntity.FolderName) ? (object)searchEntity.FolderName : DBNull.Value, true),
                CreateParameter("Category", !String.IsNullOrEmpty(searchEntity.Category) ? (object)searchEntity.Category : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("SharedWithCooperatorID", searchEntity.SharedWithCooperatorID > 0 ? (object)searchEntity.SharedWithCooperatorID : DBNull.Value, true),
                CreateParameter("IsFavorite", !String.IsNullOrEmpty(searchEntity.IsFavorite) ? (object)searchEntity.IsFavorite : DBNull.Value, true),
                CreateParameter("FolderType", !String.IsNullOrEmpty(searchEntity.FolderType) ? (object)searchEntity.FolderType : DBNull.Value, true),
                CreateParameter("AppUserItemFolderID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("EntityID", searchEntity.EntityID > 0 ? (object)searchEntity.EntityID : DBNull.Value, true),
            };

            results = GetRecords<AppUserItemFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual int Insert(AppUserItemFolder entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<AppUserItemFolder>(entity);
            SQL = "usp_GRINGlobal_AppUserItemFolder_Insert";
            
            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            entity.ID = GetParameterValue<int>("@out_app_user_item_folder_id", -1);
            return entity.ID;
        }
        public int Update(AppUserItemFolder entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<AppUserItemFolder>(entity);

            SQL = "usp_GRINGlobal_AppUserItemFolder_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }
        public int Import(AppUserItemFolder entity)
        {
            int newFolderId = 0;
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<AppUserItemFolder>(entity);
            SQL = "usp_GRINGlobal_AppUserItemFolder_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            newFolderId = GetParameterValue<int>("@out_app_user_item_folder_id", -1);

            entity.ID = newFolderId;

            if (!String.IsNullOrEmpty(entity.ItemIDList))
            {
                ImportItems(entity);
            }
            return RowsAffected;
        }
        public int ImportItems(AppUserItemFolder entity)
        {
            string[] idCollection;
            //string[] idTokens;
            idCollection = entity.ItemIDList.Split(',');
            foreach (var id in idCollection)
            {
                //TODO
                //parse each element:
                //To LEFT of - character = table name/ID type
                //To RIGHT of - character = ID of entity in table
                //idTokens = id.Split('-');
                
                AppUserItemList appUserItemList = new AppUserItemList();
                appUserItemList.AppUserItemFolderID = entity.ID;
                appUserItemList.ID = Int32.Parse(id);
                appUserItemList.CreatedByCooperatorID = entity.CreatedByCooperatorID;
                ImportItem(appUserItemList);
            }
            return 0;
        }
        private int ImportItem(AppUserItemList appUserItemList)
        {
            int rowsAffected = 0;

            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_AppUserItemFolderListMap_Insert";

            AddParameter("app_user_item_folder_id", (object)appUserItemList.AppUserItemFolderID, false);
            AddParameter("app_user_item_list_id", (object)appUserItemList.ID, false);
            AddParameter("cooperator_id", (object)appUserItemList.CreatedByCooperatorID, false);
            AddParameter("created_by", (object)appUserItemList.CreatedByCooperatorID ?? DBNull.Value, true);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_list_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            rowsAffected = ExecuteNonQuery();

            RowsAffected = GetParameterValue<int>("@out_app_user_item_folder_list_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            return rowsAffected;
        }
        public int Delete(AppUserItemFolder entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_AppUserItemFolder_Delete";
            AddParameter("@app_user_item_folder_id", (object)entity.ID, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }
        public int DeleteItem(int appUserItemListId)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_AppUserItemList_Delete";
            AddParameter("@app_user_item_list_id", (object)appUserItemListId, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }
        public int DeleteItemByEntityID(int appUserItemFolderID, int idNumber)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_AppUserItemList_By_EntityID_Delete";
            AddParameter("@app_user_item_folder_id", (object)appUserItemFolderID, false);
            AddParameter("@id_number", (object)idNumber, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }
        public virtual List<CodeValue> GetCategories(int cooperatorId = 0)
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            FolderSearch searchEntity = new FolderSearch { CreatedByCooperatorID = cooperatorId };

            SQL = " SELECT DISTINCT Category AS Value, " +
                  " Category AS Title " +
                  " FROM vw_GRINGlobal_App_User_Item_Folder ";
            SQL += " WHERE (@CreatedByCooperatorID IS NULL OR CreatedByCooperatorID =  @CreatedByCooperatorID)";
            SQL += " GROUP BY Category  ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true)
            };

            codeValues = GetRecords<CodeValue>(SQL, parameters.ToArray());

            return codeValues;
        }
        public virtual List<ReportItem> GetIDTypes(int appUserItemFolderId)
        {
            List<ReportItem> reportItems = new List<ReportItem>();

            SQL = "usp_GRINGlobal_AppUserItemList_ID_Types_Select";

            var parameters = new List<IDbDataParameter> {
            CreateParameter("app_user_item_folder_id", (object)appUserItemFolderId, false)
            };
            reportItems = GetRecords<ReportItem>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return reportItems;
        }
        protected virtual void BuildInsertUpdateParameters(AppUserItemFolder entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("app_user_item_folder_id", (object)entity.ID, false);
            }

            AddParameter("folder_name", (object)entity.FolderName, false);
            AddParameter("folder_type", (object)entity.FolderType ?? DBNull.Value, true);
            AddParameter("data_type", (object)entity.DataType ?? DBNull.Value, true);
            AddParameter("category", (object)entity.Category ?? DBNull.Value, true);
            AddParameter("description", (object)entity.Description ?? DBNull.Value, true);
            AddParameter("is_favorite", (object)entity.IsFavorite ?? DBNull.Value, true);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", (object)entity.ModifiedByCooperatorID ?? DBNull.Value, true);
            }
            else 
            {
                AddParameter("created_by", (object)entity.CreatedByCooperatorID ?? DBNull.Value, true);
            }
        }
    }
}
