using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer.OBSOLETE
{
    public partial class AppUserItemFolderCooperatorMapManager : GRINGlobalDataManagerBase
    {
        public List<Cooperator> GetMapped(int appUserItemFolderId)
        {
            List<Cooperator> cooperators = new List<Cooperator>();
            SQL = "usp_GRINGlobal_AppUserItemFolderMappedCooperators_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("app_user_item_folder_id", (object)appUserItemFolderId, false)
            };

            cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return cooperators;
        }

        public List<Cooperator> GetNonMapped(int appUserItemFolderId)
        {
            List<Cooperator> cooperators = new List<Cooperator>();
            SQL = "usp_GRINGlobal_AppUserItemFolderNonMappedCooperators_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("app_user_item_folder_id", (object)appUserItemFolderId, false)
            };

            cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return cooperators;
        }

        public virtual List<AppUserItemFolder> Search(AppUserItemFolderSearch searchEntity)
        {
            List<AppUserItemFolder> results = new List<AppUserItemFolder>();

            SQL = " SELECT * FROM vw_GRINGlobal_App_User_Item_Folder"; 
            SQL += " WHERE  (@Category                  IS NULL OR   Category                   =   @Category)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID      =   @CreatedByCooperatorID)";
            SQL += " AND    (@AppUserItemFolderID       IS NULL OR   ID                         =   @AppUserItemFolderID)";
            SQL += " AND    (@IsFavorite                IS NULL OR   IsFavorite                 =   @IsFavorite)";
            SQL += " AND    (@FolderType                IS NULL OR   FolderType                 =   @FolderType)";

            //if (searchEntity.IsShared == "Y")
            //{
            //    SQL += " AND ID IN (SELECT FolderID " +
            //            " FROM vw_GRINGlobal_App_User_Item_Folder_Cooperator_Map " +
            //            " WHERE CooperatorID = @SharedWithCooperatorID) ";
            //}

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
                CreateParameter("Category", !String.IsNullOrEmpty(searchEntity.Category) ? (object)searchEntity.Category : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                //CreateParameter("SharedWithCooperatorID", searchEntity.SharedWithCooperatorID > 0 ? (object)searchEntity.SharedWithCooperatorID : DBNull.Value, true),
                CreateParameter("IsFavorite", !String.IsNullOrEmpty(searchEntity.IsFavorite) ? (object)searchEntity.IsFavorite : DBNull.Value, true),
                CreateParameter("FolderType", !String.IsNullOrEmpty(searchEntity.FolderType) ? (object)searchEntity.FolderType : DBNull.Value, true),
                CreateParameter("AppUserItemFolderID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            };

            results = GetRecords<AppUserItemFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual int Insert(AppUserItemFolderCooperatorMap entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<AppUserItemFolderCooperatorMap>(entity);
            SQL = "usp_GRINGlobal_AppUserItemFolderCooperatorMap_Insert";

            AddParameter("cooperator_id", (object)entity.CooperatorID, false);
            AddParameter("app_user_item_folder_id", (object)entity.FolderID, false);
            AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_cooperator_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            entity.ID = GetParameterValue<int>("@out_app_user_item_folder_cooperator_map_id", -1);
            return entity.ID;
        }
        public int Delete(AppUserItemFolderCooperatorMap entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_AppUserItemFolderCooperatorMap_Insert";
            AddParameter("@app_user_item_folder_cooperator_map_id", (object)entity.ID, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }
        protected virtual void BuildInsertUpdateParameters(AppUserItemFolder entity)
        {
            
        }
    }
}
