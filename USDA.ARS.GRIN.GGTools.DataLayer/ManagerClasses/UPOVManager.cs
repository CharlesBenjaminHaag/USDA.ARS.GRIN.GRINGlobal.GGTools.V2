using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer.UPOV;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class UPOVManager : GRINGlobalDataManagerBase
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Author entity)
        {
            throw new NotImplementedException();
        }

        public Author Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(upovCodeItem entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<upovCodeItem>(entity);

            SQL = "usp_GRINGlobal_Taxonomy_Species_UPOV_Insert";

            AddParameter("upov_code_id", entity.upovCodeID > 0 ? (object)entity.upovCodeID : DBNull.Value, true);
            AddParameter("upov_code", (object)entity.upovCode ?? DBNull.Value, true);
            AddParameter("other_botanical_name", (object)entity.otherBotanicalName ?? DBNull.Value, true);
            AddParameter("common_name_text", (object)entity.commonNameEN ?? DBNull.Value, true);
            AddParameter("principal_botanical_name", (object)entity.principalBotanicalName ?? DBNull.Value, true);
            AddParameter("note", (object)entity.note ?? DBNull.Value, true);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_species_upov_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            entity.taxonomy_species_upov_id = GetParameterValue<int>("@out_taxonomy_species_upov_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }

        public int UpdateAll()
        {
            Reset(CommandType.StoredProcedure);
           
            SQL = "usp_GRINGlobal_Taxonomy_Species_UPOV_Update_All";

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }

        public int DeleteAll()
        {
            Reset(CommandType.StoredProcedure);
            
            SQL = "usp_GRINGlobal_Taxonomy_Species_UPOV_Delete_All";

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }
        
        public List<UPOVEncodedSpecies> Search(upovCodeItemSearch searchEntity)
        {
            List<UPOVEncodedSpecies> results = new List<UPOVEncodedSpecies>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Species_UPOV";
            //SQL += " WHERE  (@ID                IS NULL     OR  ID = @ID) ";
            //SQL += " AND    (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            //SQL += " AND    (@CreatedDate                   IS NULL OR CreatedDate              =       @CreatedDate)";
            //SQL += " AND    (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            //SQL += " AND    (@ModifiedDate                  IS NULL OR ModifiedDate             =       @ModifiedDate)";
            //SQL += " AND    (@Note                          IS NULL OR Note                     LIKE    '%' + @Note + '%')";

            //SQL += " AND    (@FullName          IS NULL     OR FullName         LIKE    '%' + @FullName + '%')";

            
           
            var parameters = new List<IDbDataParameter> {
                
            };

            results = GetRecords<UPOVEncodedSpecies>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
            
     
       
    }
}
