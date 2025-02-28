using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysTagManager : GRINGlobalDataManagerBase
    {
        public SysTag Get(int entityId)
        {
            SysTag appUserItemFolder = new SysTag();

            SQL = "usp_GRINGlobal_SysTag_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("sys_tag_id", (object)entityId, false)
            };

            appUserItemFolder = GetRecord<SysTag>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            
            return appUserItemFolder;
        }
      
        public virtual List<SysTag> Search(SysTagSearch searchEntity)
        {
            List<SysTag> results = new List<SysTag>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Tag";
            SQL += " WHERE  (@TableName                 IS NULL OR   TableName              =    @TableName)";
            SQL += " AND    (@IDNumber                  IS NULL OR   IDNumber               =   @IDNumber)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID  =   @CreatedByCooperatorID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("TableName", !String.IsNullOrEmpty(searchEntity.TableName) ? (object)searchEntity.TableName: DBNull.Value, true),
                CreateParameter("IDNumber", searchEntity.IDNumber > 0 ? (object)searchEntity.IDNumber : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
            };

            results = GetRecords<SysTag>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public virtual int Insert(SysTag entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<SysTag>(entity);
            SQL = "usp_GRINGlobal_Sys_Tag_Insert";
            
            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_tag_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            entity.ID = GetParameterValue<int>("@out_sys_tag_id", -1);
            return entity.ID;
        }

        public virtual int InsertItem(SysTag sysTag)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<SysTag>(sysTag);
            SQL = "usp_GRINGlobal_Sys_Tag_Insert";

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_folder_item_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            //AddParameter("@sys_folder_id", (object)sysFolderItemMap.FolderID ?? DBNull.Value, true);
            //AddParameter("@table_name", (object)sysFolderItemMap.TableName ?? DBNull.Value, true);
            //AddParameter("@id_number", (object)sysFolderItemMap.IDNumber ?? DBNull.Value, true);
            //AddParameter("created_by", (object)sysFolderItemMap.CreatedByCooperatorID ?? DBNull.Value, true);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            sysTag.ID = GetParameterValue<int>("@out_sys_folder_item_map_id", -1);
            return sysTag.ID;
        }

        public int Update(SysTag entity)
        {
            //TODO
            return 0;
        }
        
        public int Delete(SysTag entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_Sys_Tag_Delete";
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

            SQL = "usp_GRINGlobal_SysList_Delete";
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

            SQL = "usp_GRINGlobal_SysList_By_EntityID_Delete";
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
        
        protected virtual void BuildInsertUpdateParameters(SysTag entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("app_user_item_folder_id", (object)entity.ID, false);
            }

            AddParameter("tag_text", (object)entity.TagText, false);
            AddParameter("tag_format_string", (object)entity.TagFormatString ?? DBNull.Value, true);
            AddParameter("table_name", (object)entity.TableName ?? DBNull.Value, true);
            AddParameter("id_number", (object)entity.IDNumber ?? DBNull.Value, true);

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
