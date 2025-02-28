using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class CooperatorMapManager : GRINGlobalDataManagerBase, IManager<CooperatorMap, CooperatorMapSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(CooperatorMap entity)
        {
            throw new NotImplementedException();
        }

        public CooperatorMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(CooperatorMap entity)
        {
            throw new NotImplementedException();
        }

        public List<CooperatorMap> Search(CooperatorMapSearch searchEntity)
        {
            List<CooperatorMap> results = new List<CooperatorMap>();
                
            SQL = " SELECT * FROM vw_GRINGlobal_Cooperator_Map";
            SQL += " WHERE  (@CooperatorGroupID         IS NULL     OR CooperatorGroupID        =       @CooperatorGroupID)";
            SQL += " AND    (@CooperatorID              IS NULL     OR CooperatorID             =       @CooperatorID)";
            SQL += " AND    (@CooperatorName            IS NULL     OR CooperatorName           LIKE    '%' + @CooperatorName + '%')";
            SQL += " AND    (@GroupTag                  IS NULL     OR GroupTag                 LIKE    '%' + @GroupTag + '%')";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL     OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@ModifiedByCooperatorID    IS NULL     OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " ORDER BY GroupTag";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CooperatorGroupID", searchEntity.CooperatorGroupID > 0 ? (object)searchEntity.CooperatorGroupID : DBNull.Value, true),
                CreateParameter("CooperatorID", searchEntity.CooperatorID > 0 ? (object)searchEntity.CooperatorID : DBNull.Value, true),
                CreateParameter("CooperatorName", (object)searchEntity.CooperatorName ?? DBNull.Value, true),
                CreateParameter("GroupTag", (object)searchEntity.GroupTag ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),                
            };

            results = GetRecords<CooperatorMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(CooperatorMap entity)
        {
            throw new NotImplementedException();
        }
    }
}
