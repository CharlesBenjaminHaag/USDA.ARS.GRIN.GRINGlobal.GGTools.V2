using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    /// <summary>
    /// Base data manager class for all GG manager classes.
    /// </summary>
    public class GRINGlobalDataManagerBase : AppDataManagerBase
    {
        public virtual List<Cooperator> GetCooperators(string tableName)
        {
            SQL = "usp_GRINGlobal_Cooperators_Created_By_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("table_name", (object)tableName, false)
            };
            List<Cooperator> cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            RowsAffected = cooperators.Count;
            return cooperators;
        }

        public virtual List<CodeValue> GetCodeValues(string groupName)
        {
            SQL = "usp_GRINGlobal_Code_Values_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("group_name", (object)groupName, false)
            };
            List<CodeValue> codeValues = GetRecords<CodeValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return codeValues;
        }

        /// <summary>
        /// Calls a generic procedure that deletes a specified record from a specified table, identified by ID.
        /// </summary>
        /// <param name="sysTableName"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// <remarks>Requires that PK field name is [TABLE_NAME] + '_id', which is the GG standard.</remarks>
        public int Delete(string sysTableName, int entityId)
        {
            Reset(CommandType.StoredProcedure);
            SQL = "usp_GRINGlobal_Entity_Delete";

            AddParameter("table_name", sysTableName, true);
            AddParameter("entity_id", entityId == 0 ? DBNull.Value : (object)entityId, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            return RowsAffected;
        }

        #region SQL Utilities

        protected string GetCreatedDateRangeSQL(SearchEntityBase searchEntity, string sql)
        {
            // Verification date logic
            if ((searchEntity.CreatedDateFrom > DateTime.MinValue) && (searchEntity.CreatedDateTo > DateTime.MinValue))
            {
                sql += " AND CreatedDate >= '" + searchEntity.CreatedDateFrom + "' AND CreatedDate <= '" + searchEntity.CreatedDateTo + "'";
            }
            return sql;
        }

        protected string GetModifiedDateRangeSQL(SearchEntityBase searchEntity, string sql)
        {
            // Verification date logic
            if ((searchEntity.ModifiedDateFrom > DateTime.MinValue) && (searchEntity.ModifiedDateTo > DateTime.MinValue))
            {
                sql += " AND ModifiedDate >= '" + searchEntity.ModifiedDateFrom + "' AND ModifiedDate <= '" + searchEntity.ModifiedDateTo + "'";
            }
            return sql;
        }

        #endregion SQL Utilities
    }
}
