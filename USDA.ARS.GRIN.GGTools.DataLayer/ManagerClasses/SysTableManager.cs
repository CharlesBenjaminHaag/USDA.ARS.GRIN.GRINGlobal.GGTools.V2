using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using System.Security.Policy;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysTableManager : GRINGlobalDataManagerBase, IManager<SysTable, SysTableSearch>
    {
        public void BuildInsertUpdateParameters()
        {
         
        }

        public int Delete(SysTable entity)
        {
            throw new NotImplementedException();
        }

        public SysTable Get(int sysTableId)
        {
            SQL = "usp_GRINGlobal_SysTable_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("site_id", (object)sysTableId, false)
            };
            SysTable sysTable = GetRecord<SysTable>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return sysTable;
        }

        public List<SysTable> GetSysTablesTaxonomy(bool loadChildData = false)
        {
            SQL = "usp_GRINGlobal_Sys_Tables_Taxonomy_Select";
            List<SysTable> sysTables = GetRecords<SysTable>(SQL, CommandType.StoredProcedure);

            if (loadChildData)
            {
                foreach(var sysTable in sysTables) 
                {
                    sysTable.SysTableFields = GetSysTableFields(sysTable.SysTableName);
                }
            }

            return sysTables;
        }
        
        public List<SysTable> GetSysTables(int sysUserId, string databaseAreaCode)
        {
            SQL = "usp_GRINGlobal_GOBS_Tables_Select";
          
            List<SysTable> sysTables = GetRecords<SysTable>(SQL, CommandType.StoredProcedure);

            return sysTables;
        }

        public List<SysTableField> GetSysTableFields(string sysTableName)
        {
            SQL = "usp_GRINGlobal_Sys_Table_Fields_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@sys_table_name", (object)sysTableName, false)
            };
            List<SysTableField> sysTableFields = GetRecords<SysTableField>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return sysTableFields;
        }

        public List<SysTableField> GetSysTableFieldsByGroupName(string groupName)
        {
            SQL = "usp_GRINGlobal_Sys_Table_Field_By_Group_Name_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@group_name", (object)groupName, false)
            };
            List<SysTableField> sysTableFields = GetRecords<SysTableField>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return sysTableFields;
        }

        public SysTableField GetSysTableField(string sysTableName, string sysFieldName)
        {
            SQL = "usp_GRINGlobal_Sys_Table_Field_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@table_name", (object)sysTableName, false),
                CreateParameter("@field_name", (object)sysFieldName, false)
            };
            SysTableField sysTableField = GetRecord<SysTableField>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return sysTableField;
        }

        public SysTableField GetSysTablePrimaryKeyField(string sysFieldName)
        {
            SQL = "usp_GRINGlobal_Sys_Table_Primary_Key_Field_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@field_name", (object)sysFieldName, false)
            };
            SysTableField sysTableField = GetRecord<SysTableField>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return sysTableField;
        }

        public CodeValue GetRecordCount(string sysTableName, int ownedByCooperatorId = 0)
        {
            SQL = "SELECT 'Records:' AS CodeTitle, CONVERT(NVARCHAR, COUNT(*)) + ' Records' AS Value FROM " + sysTableName;
            CodeValue codeValue = GetRecord<CodeValue>(SQL);
            return codeValue;
        }

        public int Insert(SysTable entity)
        {
            throw new NotImplementedException();
        }

        public List<SysTable> Search(SysTableSearch searchEntity)
        {
            List<SysTable> results = new List<SysTable>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Table";
            SQL += " WHERE  (@ID                    IS NULL     OR ID                   =       @ID)";
            SQL += " AND    (@SysTag                IS NULL     OR SysTag               =       @SysTag)";
            SQL += " AND    (@DatabaseAreaCode      IS NULL     OR DatabaseAreaCode     =       @DatabaseAreaCode)";
            SQL += " AND    (@SysTableName          IS NULL     OR SysTableName         =       @SysTableName)";
            //SQL += " AND SysTableTitle IS NOT NULL ";
            //SQL += " AND TableName <> 'taxonomy_family'";
            //SQL += " UNION ";
            //SQL += " SELECT ID, DatabaseAreaCode, TableName, TableTitle, TableCode FROM vw_GRINGlobal_Sys_Table";
            //SQL += " WHERE TableName IN ('citation','literature','geography')"; 
            SQL += " ORDER BY SysTag, SysTableTitle ";

             var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("SysTag", (object)searchEntity.SysTag ?? DBNull.Value, true),
                CreateParameter("DatabaseAreaCode", (object)searchEntity.DatabaseAreaCode ?? DBNull.Value, true),
                CreateParameter("SysTableName", (object)searchEntity.TableName ?? DBNull.Value, true),
            };

            results = GetRecords<SysTable>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(SysTable entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<SysTable>(entity);
            SQL = "usp_GRINGlobal_Sys_Table_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            RowsAffected = ExecuteNonQuery();

            return RowsAffected;
        }

        public int TransferOwnership(int id, string sysTableName, int ownedByCooperatorId)
        {
            Reset(CommandType.StoredProcedure);
            SQL = "usp_GRINGlobal_Sys_Table_Ownership_Transfer";

            AddParameter("table_name", (object)sysTableName ?? DBNull.Value, true);
            AddParameter("owned_by", ownedByCooperatorId == 0 ? ownedByCooperatorId : (object)ownedByCooperatorId, true);
            AddParameter("id", id == 0 ? id : (object)id, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            return RowsAffected;
        }

        protected virtual void BuildInsertUpdateParameters(SysTable entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("site_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

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
