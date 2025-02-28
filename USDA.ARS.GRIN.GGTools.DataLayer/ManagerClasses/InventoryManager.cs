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
    public class InventoryManager : GRINGlobalDataManagerBase
    {
        
    
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Inventory entity)
        {
            throw new NotImplementedException();
        }

        public Inventory Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(Inventory entity)
        {
            throw new NotImplementedException();
        }

        public List<Inventory> Search(InventorySearch searchEntity)
        {
            List<Inventory> results = new List<Inventory>();

            SQL = " SELECT [INTERNAL_ID], [INSTCODE],[DOI],[ACCENUMB],[SPECIES_FULL],[GENUS],[SPECIES],[SPAUTHOR] ";
            SQL += ",[SUBTAXA],[SUBTAUTHOR],[ACCEURL],[SAMPSTAT],[REMARKS],[initial_received_date],[ACQDATE],[HISTORIC] ";
            SQL += ",[COLLSITE],[GEOREFMETH],[COORDUNCERT],[DECLATITUDE],[DECLONGITUDE],[ORIGCTY],[DUPLSITE1],[DUPLSITE2] ";
            SQL += " FROM vw_GRINGlobal_Inventory_MCPD ";

            // PRIMARY
            SQL += " WHERE      (@InventoryNumber      IS NULL OR  ACCENUMB        LIKE    '%' + @InventoryNumber + '%')";
            SQL += " AND         (@InstCode              IS NULL OR  INSTCODE        LIKE    '%' + @InstCode + '%')";

            var parameters = new List<IDbDataParameter> {
                //CreateParameter("InventoryNumber", (object)searchEntity.InventoryNumber ?? DBNull.Value, true),
                //CreateParameter("InstCode", (object)searchEntity.InstCode ?? DBNull.Value, true),
            };

            results = GetRecords<Inventory>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public int Update(Inventory entity)
        {
            //Reset(CommandType.StoredProcedure);
            //Validate<Species>(entity);
            //SQL = "usp_GRINGlobal_Inventory_Update";

            ////BuildInsertUpdateParameters(entity);

            //AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            //int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            //RowsAffected = ExecuteNonQuery();

            return RowsAffected;
        }
    }
}
