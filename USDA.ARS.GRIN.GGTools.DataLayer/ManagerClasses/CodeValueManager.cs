using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class CodeValueManager : GRINGlobalDataManagerBase, IManager<CodeValue, CodeValueSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(CodeValue entity)
        {
            throw new NotImplementedException();
        }

        public CodeValue Get(int entityId)
        {
            CodeValue codeValue = new CodeValue();
            List<CodeValue> codeValues = new List<CodeValue>();
     
            CodeValueSearch codeValueSearch = new CodeValueSearch();
            codeValueSearch.ID = entityId;
            codeValues = Search(codeValueSearch);
            if (codeValues.Count == 1)
            {
                codeValue = codeValues[0];
                codeValue.ID = codeValue.CodeValueID;
            }
            return codeValue;
        }
       
      
        
        public List<CodeValue> GetGroups()
        {
            List<CodeValue> results = new List<CodeValue>();
            SQL = "SELECT DISTINCT GroupName AS Value, GroupName AS Title FROM vw_GRINGlobal_Code_Value ORDER BY GroupName ASC";
            results = GetRecords<CodeValue>(SQL);
            RowsAffected = results.Count;
            return results;
        }
        public List<CodeValue> GetSysLangs()
        {
            List<CodeValue> results = new List<CodeValue>();
            SQL = "SELECT CONVERT(NVARCHAR, sys_lang_id) AS Value, title AS Title FROM sys_lang ORDER BY Title ASC";
            results = GetRecords<CodeValue>(SQL);
            RowsAffected = results.Count;
            return results;
        }
        public int Insert(CodeValue entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CodeValue>(entity);
            SQL = "usp_GRINGlobal_Code_Value_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_code_value_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_code_value_id", -1);
            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        public List<CodeValue> Search(CodeValueSearch searchEntity)
        {
            List<CodeValue> results = new List<CodeValue>();

            SQL = "SELECT * FROM vw_GRINGlobal_Code_Value ";
            SQL += " WHERE  (@GroupName     IS NULL              OR    GroupName    =       @GroupName) ";
            SQL += " AND    (@Code          IS NULL              OR    Code         LIKE    '%' + @Code + '%') ";
            SQL += " AND    (@CodeTitle     IS NULL              OR    CodeTitle    LIKE    '%' + @CodeTitle + '%') ";
            SQL += " AND    (@CodeValueID   IS NULL              OR    CodeValueID  =       @CodeValueID) ";

            var parameters = new List<IDbDataParameter> {
                  CreateParameter("GroupName", (object)searchEntity.GroupName ?? DBNull.Value, true),
                  CreateParameter("Code", (object)searchEntity.Value ?? DBNull.Value, true),
                  CreateParameter("CodeTitle", (object)searchEntity.Title ?? DBNull.Value, true),
                  CreateParameter("CodeValueID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            };
            results = GetRecords<CodeValue>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }
        public int Update(CodeValue entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<CodeValue>(entity);
            SQL = "usp_GRINGlobal_Code_Value_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
       
        public void BuildInsertUpdateParameters(CodeValue entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("code_value_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("group_name", String.IsNullOrEmpty(entity.GroupName) ? DBNull.Value : (object)entity.GroupName, true);
            AddParameter("code_value", String.IsNullOrEmpty(entity.Code) ? DBNull.Value : (object)entity.Code, true);
            AddParameter("title", String.IsNullOrEmpty(entity.CodeTitle) ? DBNull.Value : (object)entity.CodeTitle, true);
            AddParameter("description", String.IsNullOrEmpty(entity.CodeDescription) ? DBNull.Value : (object)entity.CodeDescription, true);
            AddParameter("sys_lang_id", entity.SysLangID == 0 ? DBNull.Value : (object)entity.SysLangID, true);

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
