using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class ISTASeedManager : GRINGlobalDataManagerBase, IManager<ISTASeed, ISTASeedSearch>
    {
        public int Insert(ISTASeed entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<ISTASeed>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Species_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_species_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_species_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = entity.ID;
            return RowsAffected;
        }

        public int Update(ISTASeed entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<ISTASeed>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Species_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();
          
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = entity.ID;
            return RowsAffected;
        }

        public int Delete(ISTASeed entity)
        {
            throw new NotImplementedException();
        }

        public ISTASeed Get(int id)
        {
            SQL = "";
            ISTASeed species = new ISTASeed();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_species_id", (object)id, false)
            };

            species = GetRecord<ISTASeed>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return species;
        }
       
        public List<ISTASeed> Search(ISTASeedSearch searchEntity)
        {
            List<ISTASeed> results = new List<ISTASeed>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_ISTA_Report ORDER BY GenusName ASC, SpeciesName ASC";
         
            var parameters = new List<IDbDataParameter> {
            //    CreateParameter("CommonFertilizationCode", (object)searchEntity.CommonFertilizationCode ?? DBNull.Value, true),
            //    CreateParameter("RestrictionCode", (object)searchEntity.RestrictionCode ?? DBNull.Value, true),
                
            };

            results = GetRecords<ISTASeed>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Species> GetFolderItems(ISTASeedSearch searchEntity)
        {
            List<Species> results = new List<Species>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_ISTA_Seed WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Species>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
       
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        
        protected virtual void BuildInsertUpdateParameters(ISTASeed entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_species_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
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
