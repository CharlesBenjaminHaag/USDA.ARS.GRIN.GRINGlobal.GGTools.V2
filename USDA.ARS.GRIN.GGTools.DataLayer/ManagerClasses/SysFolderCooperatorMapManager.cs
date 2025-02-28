using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysFolderCooperatorMapManager : GRINGlobalDataManagerBase
    {
        //public SysFolder Get(int entityId)
        //{
        //    SysFolder appUserItemFolder = new SysFolder();

        //    SQL = "usp_GRINGlobal_Sys_Folder_Select";

        //    var parameters = new List<IDbDataParameter> {
        //        CreateParameter("sys_folder_id", (object)entityId, false)
        //    };

        //    appUserItemFolder = GetRecord<SysFolder>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            
        //    return appUserItemFolder;
        //}
       
        public List<Cooperator> GetMappedCooperators(int sysFolderId)
        {
            List<Cooperator> sysFolderCooperatorMaps = new List<Cooperator>();

            SQL = "usp_GRINGlobal_Sys_Folder_Cooperator_Maps_Mapped_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("sys_folder_id", (object)sysFolderId, false)
            };

            sysFolderCooperatorMaps = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return sysFolderCooperatorMaps;
        }

        public List<Cooperator> GetNonMappedCooperators(int sysFolderId)
        {
            List<Cooperator> sysFolderCooperatorMaps = new List<Cooperator>();

            SQL = "usp_GRINGlobal_Sys_Folder_Cooperator_Maps_NonMapped_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("sys_folder_id", (object)sysFolderId, false)
            };

            sysFolderCooperatorMaps = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return sysFolderCooperatorMaps;
        }

        public virtual List<SysFolder> Search(SysFolderSearch searchEntity)
        {
            List<SysFolder> results = new List<SysFolder>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Folder";
            SQL += " WHERE  (@Title                     IS NULL OR   Title                  LIKE    '%' + @Title + '%')";
            SQL += " AND    (@Description               IS NULL OR   Description            LIKE    '%' + @Description + '%')";
            SQL += " AND    (@TypeCode                  IS NULL OR   TypeCode               =       @TypeCode)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID  =       @CreatedByCooperatorID)";
           
            var parameters = new List<IDbDataParameter> {
                CreateParameter("Title", !String.IsNullOrEmpty(searchEntity.Title) ? (object)searchEntity.Title : DBNull.Value, true),
                CreateParameter("Description", !String.IsNullOrEmpty(searchEntity.Description) ? (object)searchEntity.Description : DBNull.Value, true),
                CreateParameter("TypeCode", !String.IsNullOrEmpty(searchEntity.TypeCode) ? (object)searchEntity.TypeCode : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
            };

            results = GetRecords<SysFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public virtual int Insert(SysFolderCooperatorMap entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<SysFolderCooperatorMap>(entity);
            SQL = "usp_GRINGlobal_Sys_Folder_Cooperator_Map_Insert";

            AddParameter("@sys_folder_id", (object)entity.SysFolderID, false);
            AddParameter("@cooperator_id", (object)entity.CooperatorID, false);
            AddParameter("@created_by", (object)entity.CreatedByCooperatorID, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_folder_cooperator_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            entity.ID = GetParameterValue<int>("@out_sys_folder_cooperator_map_id", -1);
            return entity.ID;
        }

        public virtual int InsertItem(SysFolderItemMap sysFolderItemMap)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<SysFolderItemMap>(sysFolderItemMap);
            SQL = "usp_GRINGlobal_Sys_Folder_Item_Map_Insert";

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_sys_folder_item_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@sys_folder_id", (object)sysFolderItemMap.FolderID ?? DBNull.Value, true);
            AddParameter("@table_name", (object)sysFolderItemMap.TableName ?? DBNull.Value, true);
            AddParameter("@id_number", (object)sysFolderItemMap.IDNumber ?? DBNull.Value, true);
            AddParameter("created_by", (object)sysFolderItemMap.CreatedByCooperatorID ?? DBNull.Value, true);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            sysFolderItemMap.ID = GetParameterValue<int>("@out_sys_folder_item_map_id", -1);
            return sysFolderItemMap.ID;
        }

        //public int Update(SysFolder entity)
        //{
        //    Reset(CommandType.StoredProcedure);
        //    Validate<SysFolder>(entity);

        //    SQL = "usp_GRINGlobal_Sys_Folder_Update";

        //    BuildInsertUpdateParameters(entity);
        //    AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
        //    RowsAffected = ExecuteNonQuery();
        //    return RowsAffected;
        //}
        
        public int Delete(SysFolder entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_SysFolder_Delete";
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
        
        public int DeleteItems(int sysFolderId, int cooperatorId)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_Sys_Folder_Cooperator_Map_Delete";
            AddParameter("@sys_folder_id", (object)sysFolderId, false);
            AddParameter("@cooperator_id", (object)cooperatorId, false);
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
        
        protected virtual void BuildInsertUpdateParameters(SysFolderCooperatorMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("sys_folder_id", (object)entity.ID, false);
            }

            //AddParameter("title", (object)entity.Title, false);
            //AddParameter("description", (object)entity.Description ?? DBNull.Value, true);
            //AddParameter("type_code", (object)entity.TypeCode ?? DBNull.Value, true);

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
