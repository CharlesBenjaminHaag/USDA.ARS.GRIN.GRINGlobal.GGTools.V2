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
    public class SysDataViewManager : GRINGlobalDataManagerBase, IManager<SysTable, SysTableSearch>
    {
        public void BuildInsertUpdateParameters()
        {
         
        }

        public int Delete(SysTable entity)
        {
            throw new NotImplementedException();
        }

        public SysDataView Get(int entityId)
        {
            SQL = "SELECT * FROM vw_GRINGlobal_DataView WHERE ID = " + entityId;
            SysDataView sysDataView = GetRecord<SysDataView>(SQL, CommandType.Text);
            return sysDataView;
        }

        public List<SysDataView> GetAll()
        {
            SQL = "SELECT * FROM vw_GRINGlobal_Sys_DataView";
            List<SysDataView> sysDataViews = GetRecords<SysDataView>(SQL, CommandType.Text);
            return sysDataViews;
        }

        public List<SysDataViewParameter> GetParameters(int sysDataViewId)
        {
            SQL = "SELECT * FROM vw_GRINGlobal_Sys_DataView_Param WHERE SysDataViewID = @ID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@ID", (object)sysDataViewId, false)
            };
            List<SysDataViewParameter> sysDataViewParameters = GetRecords<SysDataViewParameter>(SQL, CommandType.Text, parameters.ToArray());
            return sysDataViewParameters;
        }

        public List<SysDataViewField> GetFields(int sysDataViewId)
        {
            SQL = "SELECT * FROM vw_GRINGlobal_Sys_DataView_Field WHERE SysDataViewID = @ID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@ID", (object)sysDataViewId, false)
            };
            List<SysDataViewField> sysTableFields = GetRecords<SysDataViewField>(SQL, CommandType.Text, parameters.ToArray());
            return sysTableFields;
        }

        public string GetSQL(int sysDataViewId)
        {
           SysDataViewSQL sysDataViewSQL = null;

            SQL = "SELECT sql_statement AS SQLStatement FROM sys_dataview_sql WHERE sys_dataview_id = @ID AND database_engine_tag = 'sqlserver'";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@ID", (object)sysDataViewId, false)
            };
            sysDataViewSQL = GetRecord<SysDataViewSQL>(SQL, CommandType.Text, parameters.ToArray());
            return sysDataViewSQL.SQLStatement; 
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
            SQL += " AND    (@DatabaseAreaCode      IS NULL     OR DatabaseAreaCode     =       @DatabaseAreaCode)";
            SQL += " AND    (@SysTableName          IS NULL     OR SysTableName         =       @SysTableName)";
            //SQL += " AND SysTableTitle IS NOT NULL ";
            //SQL += " AND TableName <> 'taxonomy_family'";
            //SQL += " UNION ";
            //SQL += " SELECT ID, DatabaseAreaCode, TableName, TableTitle, TableCode FROM vw_GRINGlobal_Sys_Table";
            //SQL += " WHERE TableName IN ('citation','literature','geography')"; 
            SQL += " ORDER BY SysTableTitle ";

             var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
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

        SysTable IManager<SysTable, SysTableSearch>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
