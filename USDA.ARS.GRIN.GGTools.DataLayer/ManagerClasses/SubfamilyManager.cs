using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SubfamilyManager : AppDataManagerBase, IManager<Subfamily, SubfamilySearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public void BuildInsertUpdateParameters(Subfamily entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_subfamily_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("accepted_infra_family_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("subfamily_name", String.IsNullOrEmpty(entity.SubfamilyName) ? DBNull.Value : (object)entity.SubfamilyName, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            AddParameter("authority", (object)entity.Authority ?? DBNull.Value, true);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            }
            else
            {
                AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            }
        }

        public int Delete(Subfamily entity)
        {
            throw new NotImplementedException();
        }

        //public Subfamily Get(int entityId)
        //{
        //    Subfamily subFamily = new Subfamily();
        //    SQL = "usp_GGTools_Taxon_Subfamily_Select";

        //    var parameters = new List<IDbDataParameter> {
        //    CreateParameter("taxonomy_subfamily_id", (object)entityId, false)            };
        //    subFamily = GetRecord<Subfamily>(SQL, CommandType.StoredProcedure, parameters.ToArray());
        //    return subFamily;
        //}

        public int Insert(Subfamily entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Subfamily>(entity);
            SQL = "usp_GGTools_Taxon_Subfamily_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_subfamily_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_subfamily_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public List<Subfamily> Search(SubfamilySearch searchEntity)
        {
            List<Subfamily> results = new List<Subfamily>();

            SQL = " SELECT * FROM vw_GGTools_Taxon_FamilyMaps";

            SQL += " WHERE  (@ID                        IS NULL OR  ID                          =         @ID)";
            SQL += " AND    (@FamilyID                  IS NULL OR  FamilyID                    =         @FamilyID)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR  CreatedByCooperatorID       =         @CreatedByCooperatorID)";
            SQL += " AND    (@FamilyName                IS NULL OR  FamilyName                  LIKE       '%' + @FamilyName + '%' )";
            SQL += " AND    (@SubfamilyName             IS NULL OR  SubfamilyName               LIKE       @SubfamilyName + '%' )";

            if (!String.IsNullOrEmpty(searchEntity.FamilyIDList))
            {
                SQL += " AND (FamilyID IN (" + searchEntity.FamilyIDList + "))";
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("FamilyID", searchEntity.FamilyID > 0 ? (object)searchEntity.FamilyID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("SubfamilyName", (object)searchEntity.SubfamilyName ?? DBNull.Value, true),
            };

            results = GetRecords<Subfamily>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Subfamily> SearchFolderItems(SubfamilySearch searchEntity)
        {
            List<Subfamily> results = new List<Subfamily>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgtf.* " +
                " FROM vw_GGTools_Taxon_InfraFamilies vgtf " +
                " JOIN app_user_item_list auil " +
                " ON vgtf.ID = auil.id_number " +
                " WHERE auil.id_type = 'just_subfamily' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Subfamily>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public List<FamilyMap> AcceptedNameSearch(string searchText)
        {
            List<FamilyMap> results = new List<FamilyMap>();

            SQL = " SELECT DISTINCT ID, InfraName FROM vw_GGTools_Taxon_InfraFamilies";
            SQL += " WHERE    (@InfraName                IS NULL OR  InfraName                  LIKE       '%' + @InfraName + '%' )";
 
            var parameters = new List<IDbDataParameter> {
                CreateParameter("InfraName", (object)searchText ?? DBNull.Value, true),
            };

            results = GetRecords<FamilyMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public int Update(Subfamily entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Subfamily>(entity);

            SQL = "usp_GGTools_Taxon_Subfamily_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }
    }
}
