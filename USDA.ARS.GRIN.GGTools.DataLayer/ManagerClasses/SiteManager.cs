using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SiteManager : GRINGlobalDataManagerBase, IManager<Site, SiteSearch>
    {
        public void BuildInsertUpdateParameters()
        {
         
        }

        public int Delete(Site entity)
        {
            throw new NotImplementedException();
        }

        public Site Get(int siteId)
        {
            SQL = "usp_GRINGlobal_Site_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("site_id", (object)siteId, false)
            };
            Site site = GetRecord<Site>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return site;
        }

        public int Insert(Site entity)
        {
            throw new NotImplementedException();
        }

        public List<Site> Search(SiteSearch searchEntity)
        {
            List<Site> results = new List<Site>();

            SQL = " SELECT * FROM vw_GRINGlobal_Site";
            SQL += " WHERE (@ID                     IS NULL     OR ID                       =       @ID)";
            SQL += " AND (@ShortName                IS NULL     OR ShortName                LIKE    '%' + @ShortName + '%')";
            SQL += " AND (@LongName                 IS NULL     OR LongName                 LIKE    '%' + @LongName + '%')";
           
            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("ShortName", (object)searchEntity.ShortName ?? DBNull.Value, true),
                CreateParameter("LongName", (object)searchEntity.LongName ?? DBNull.Value, true),
            };

            results = GetRecords<Site>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(Site entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Site>(entity);
            SQL = "usp_GRINGlobal_Site_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            RowsAffected = ExecuteNonQuery();

            return RowsAffected;
        }

        protected virtual void BuildInsertUpdateParameters(Site entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("site_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("@site_long_name", (object)entity.LongName ?? DBNull.Value, true);
            AddParameter("@provider_identifier", (object)entity.ProviderIdentifier ?? DBNull.Value, true);
            AddParameter("@is_internal", (object)entity.IsInternal ?? DBNull.Value, true);
            AddParameter("@is_distribution_site", (object)entity.IsDistributionSite ?? DBNull.Value, true);
            AddParameter("@type_code", (object)entity.TypeCode ?? DBNull.Value, true);
            AddParameter("@fao_institute_number", (object)entity.FAOInstituteNumber ?? DBNull.Value, true);
            AddParameter("@cooperator_id", entity.CooperatorID == 0 ? DBNull.Value : (object)entity.CooperatorID, true);
            AddParameter("@address_line1", (object)entity.PrimaryAddress1 ?? DBNull.Value, true);
            AddParameter("@address_line2", (object)entity.PrimaryAddress2 ?? DBNull.Value, true);
            AddParameter("@address_line3", (object)entity.PrimaryAddress3 ?? DBNull.Value, true);
            AddParameter("@city", (object)entity.City ?? DBNull.Value, true);
            AddParameter("@primary_phone", (object)entity.PrimaryPhone ?? DBNull.Value, true);
            AddParameter("@email_address", (object)entity.EmailAddress ?? DBNull.Value, true);
            AddParameter("@primary_url", (object)entity.PrimaryURL ?? DBNull.Value, true);
            AddParameter("@secondary_url", (object)entity.SecondaryURL ?? DBNull.Value, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);

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
