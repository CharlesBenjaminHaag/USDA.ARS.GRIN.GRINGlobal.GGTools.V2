using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class WebCooperatorManager : GRINGlobalDataManagerBase, IManager<WebCooperator, WebCooperatorSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        public int Delete(WebCooperator entity)
        {
            throw new NotImplementedException();
        }
        
        public WebCooperator Get(int entityId)
        {
            WebCooperator webCooperator = new WebCooperator();
           
            SQL = "usp_GRINGlobal_Web_Cooperator_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_cooperator_id", (object)entityId, false)
            };

            webCooperator = GetRecord<WebCooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            // Address(es)
            SQL = "SELECT * FROM vw_GRINGlobal_Web_User_Shipping_Address";

            // Cart(s)

            // Pref(s)

            return webCooperator;
        }

        public List<WebUserShippingAddress> GetWebUserShippingAddresses(int webUserId)
        {
            List<WebUserShippingAddress> webUserShippingAddresses = new List<WebUserShippingAddress>();

            SQL = " SELECT * FROM vw_GRINGlobal_Web_User_Shipping_Address ";
            SQL += " WHERE  (@WebUserID               IS NULL     OR     WebUserID    =            @WebUserID)";

            var parameters = new List<IDbDataParameter> {
              
                CreateParameter("WebUserID", webUserId> 0 ? (object)webUserId : DBNull.Value, true)
            };

            webUserShippingAddresses = GetRecords<WebUserShippingAddress>(SQL, parameters.ToArray());
            RowsAffected = webUserShippingAddresses.Count;
            return webUserShippingAddresses;
        }

        public List<State> GetStates()
        {
            List<State> states = new List<State>();
            SQL = "SELECT ID, Admin1 FROM vw_GRINGlobal_Geography_State ORDER BY Admin1";
            states = GetRecords<State>(SQL);
            return states;
        }
        
        public WebCooperator GetByCooperatorID(int cooperatorId)
        {
            WebCooperator webCooperator = new WebCooperator();
            SQL = "usp_GRINGlobal_Web_Cooperator_By_Cooperator_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false)
            };
            webCooperator = GetRecord<WebCooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            parameters.Clear();
            return webCooperator;
        }

        public int Insert(WebCooperator entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebCooperator>(entity);
            BuildInsertUpdateParameters(entity);

            SQL = "usp_GRINGlobal_Web_Cooperator_Insert";

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_web_cooperator_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_web_cooperator_id", -1);
            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        
        public List<WebCooperator> Search(WebCooperatorSearch searchEntity)
        {
            List<WebCooperator> results = new List<WebCooperator>();

            SQL = " SELECT * FROM vw_GRINGlobal_Web_Cooperator ";
            SQL += " WHERE (@FirstName      IS NULL     OR      FirstName          LIKE        '%' + @FirstName + '%')";
            SQL += " AND (@LastName         IS NULL     OR      LastName           LIKE        '%' + @LastName + '%')";
            SQL += " AND (@Organization     IS NULL     OR      Organization       LIKE        '%' + @Organization + '%')";
            SQL += " AND (@ID               IS NULL     OR      WebCooperatorID    =            @ID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FirstName", (object)searchEntity.FirstName ?? DBNull.Value, true),
                CreateParameter("LastName", (object)searchEntity.LastName ?? DBNull.Value, true),
                CreateParameter("Organization", (object)searchEntity.Organization ?? DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true)
            };
            
            results = GetRecords<WebCooperator>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public int Update(WebCooperator entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebCooperator>(entity);
            BuildInsertUpdateParameters(entity);

            SQL = "usp_GRINGlobal_Web_Cooperator_Update";

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            var errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());
            return RowsAffected;
        }
        
        public int Copy(int cooperatorId)
        {
            int entityId = 0;
            Reset(CommandType.StoredProcedure);
         
            SQL = "usp_GRINGlobal_Web_Cooperator_Copy";

            AddParameter("cooperator_id", cooperatorId == 0 ? DBNull.Value : (object)cooperatorId, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_web_cooperator_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entityId = GetParameterValue<int>("@out_web_cooperator_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return entityId;
        }
                
        public void BuildInsertUpdateParameters(WebCooperator entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("web_cooperator_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("title", String.IsNullOrEmpty(entity.JobTitle) ? DBNull.Value : (object)entity.JobTitle, true);
            AddParameter("last_name", String.IsNullOrEmpty(entity.LastName) ? DBNull.Value : (object)entity.LastName, true);
            AddParameter("first_name", String.IsNullOrEmpty(entity.FirstName) ? DBNull.Value : (object)entity.FirstName, true);
            AddParameter("job", String.IsNullOrEmpty(entity.JobTitle) ? DBNull.Value : (object)entity.JobTitle, true);
            AddParameter("organization", String.IsNullOrEmpty(entity.Organization) ? DBNull.Value : (object)entity.Organization, true);
            AddParameter("organization_abbrev", String.IsNullOrEmpty(entity.OrganizationAbbrev) ? DBNull.Value : (object)entity.OrganizationAbbrev, true);
            AddParameter("address_line1", String.IsNullOrEmpty(entity.Address1) ? DBNull.Value : (object)entity.Address1, true);
            AddParameter("address_line2", String.IsNullOrEmpty(entity.Address2) ? DBNull.Value : (object)entity.Address2, true);
            AddParameter("address_line3", String.IsNullOrEmpty(entity.Address3) ? DBNull.Value : (object)entity.Address3, true);
            AddParameter("city", String.IsNullOrEmpty(entity.City) ? DBNull.Value : (object)entity.City, true);
            AddParameter("postal_index", String.IsNullOrEmpty(entity.PostalCode) ? DBNull.Value : (object)entity.PostalCode, true);
            AddParameter("geography_id", entity.GeographyID == 0 ? DBNull.Value : (object)entity.GeographyID, true);
            AddParameter("primary_phone", String.IsNullOrEmpty(entity.PrimaryPhone) ? DBNull.Value : (object)entity.PrimaryPhone, true);
            AddParameter("email_address", String.IsNullOrEmpty(entity.EmailAddress) ? DBNull.Value : (object)entity.EmailAddress, true);
            AddParameter("category_code", String.IsNullOrEmpty(entity.CategoryCode) ? DBNull.Value : (object)entity.CategoryCode, true);
            AddParameter("is_active", String.IsNullOrEmpty(entity.IsActive) ? DBNull.Value : (object)entity.IsActive, true);
            AddParameter("organization_region_code", String.IsNullOrEmpty(entity.OrganizationRegionCode) ? DBNull.Value : (object)entity.OrganizationRegionCode, true);
            AddParameter("discipline_code", String.IsNullOrEmpty(entity.DisciplineCode) ? DBNull.Value : (object)entity.DisciplineCode, true);
            AddParameter("note", String.IsNullOrEmpty(entity.Note) ? DBNull.Value : (object)entity.Note, true);

            //if (entity.ID > 0)
            //{
            //    AddParameter("modified_by", entity.WebUserID == 0 ? DBNull.Value : (object)entity.WebUserID, true);
            //}
            //else
            //{
            //    AddParameter("created_by", entity.WebUserID == 0 ? DBNull.Value : (object)entity.WebUserID, true);
            //}
        }
    }
}
