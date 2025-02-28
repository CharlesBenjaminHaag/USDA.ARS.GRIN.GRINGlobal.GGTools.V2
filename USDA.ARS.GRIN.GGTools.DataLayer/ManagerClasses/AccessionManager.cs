using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AccessionManager : GRINGlobalDataManagerBase
    {
        
    
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Accession entity)
        {
            throw new NotImplementedException();
        }

        public Accession Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(Accession entity)
        {
            throw new NotImplementedException();
        }

        public List<AccessionMCPD> Search(AccessionSearch searchEntity)
        {
            List<AccessionMCPD> results = new List<AccessionMCPD>();

            SQL = " SELECT [INTERNAL_ID], [INSTCODE],[DOI],[ACCENUMB],[SPECIES_FULL],[GENUS],[SPECIES],[SPAUTHOR] ";
            SQL += ",[SUBTAXA],[SUBTAUTHOR],[ACCEURL],[SAMPSTAT],[REMARKS],[initial_received_date],[ACQDATE],[HISTORIC] ";
            SQL += ",[COLLSITE],[GEOREFMETH],[COORDUNCERT],[DECLATITUDE],[DECLONGITUDE],[ORIGCTY],[DUPLSITE1],[DUPLSITE2] ";
            SQL += " FROM vw_GRINGlobal_Accession_MCPD ";

            // PRIMARY
            SQL += " WHERE      (@AccessionNumber      IS NULL OR  ACCENUMB        LIKE    '%' + @AccessionNumber + '%')";
            SQL += " AND         (@InstCode              IS NULL OR  INSTCODE        LIKE    '%' + @InstCode + '%')";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("AccessionNumber", (object)searchEntity.AccessionNumber ?? DBNull.Value, true),
                CreateParameter("InstCode", (object)searchEntity.InstCode ?? DBNull.Value, true),
            };

            results = GetRecords<AccessionMCPD>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<AccessionMCPD> Export(int offset = 0, int limit = 0)
        {
            SQL = "usp_GRINGlobal_Accessions_MCPD_Export";
            List<AccessionMCPD> accessions = new List<AccessionMCPD>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("@offset", offset, true),
                CreateParameter("@limit", limit, true),
            };
            accessions = GetRecords<AccessionMCPD>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return accessions;
        }

        public int Update(Accession entity)
        {
            //Reset(CommandType.StoredProcedure);
            //Validate<Species>(entity);
            //SQL = "usp_GRINGlobal_Accession_Update";

            ////BuildInsertUpdateParameters(entity);

            //AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            //int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            //RowsAffected = ExecuteNonQuery();

            return RowsAffected;
        }

        public int UpdateBySpecies(Accession entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Accession>(entity);
            SQL = "usp_GRINGlobal_Accession_Taxonomy_Update";

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("current_taxonomy_species_id", entity.NewSpeciesID == 0 ? DBNull.Value : (object)entity.NewSpeciesID, true);
            AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);

            RowsAffected = ExecuteNonQuery();
            
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = entity.ID;
            return entity.ID;
        }
    }
}
