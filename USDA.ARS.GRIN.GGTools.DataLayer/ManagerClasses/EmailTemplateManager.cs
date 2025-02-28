using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class EmailTemplateManager: GRINGlobalDataManagerBase, IManager<EmailTemplate, EmailTemplateSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(EmailTemplate entity)
        {
            throw new NotImplementedException();
        }

        public EmailTemplate Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(EmailTemplate entity)
        {
            throw new NotImplementedException();
        }

        public List<EmailTemplate> Search(EmailTemplateSearch searchEntity)
        {
            List<EmailTemplate> results = new List<EmailTemplate>();

            SQL = " SELECT * FROM vw_GRINGlobal_Email_Template ";
            SQL += " WHERE  (@CategoryCode          IS NULL     OR      CategoryCode    =   @CategoryCode)";
            SQL += " AND    (@ID                    IS NULL     OR      ID              =   @ID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CategoryCode", (object)searchEntity.CategoryCode ?? DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true)
            };

            results = GetRecords<EmailTemplate>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(EmailTemplate entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<EmailTemplate>(entity);
            SQL = "usp_GRINGlobal_Email_Template_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            RowsAffected = ExecuteNonQuery();

            return RowsAffected;
        }
        protected virtual void BuildInsertUpdateParameters(EmailTemplate entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("email_template_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("title", (object)entity.Title ?? DBNull.Value, true);
            AddParameter("email_from", (object)entity.EmailFrom ?? DBNull.Value, true);
            AddParameter("subject", (object)entity.Subject ?? DBNull.Value, true);
            AddParameter("body", (object)entity.Body ?? DBNull.Value, true);
        
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
