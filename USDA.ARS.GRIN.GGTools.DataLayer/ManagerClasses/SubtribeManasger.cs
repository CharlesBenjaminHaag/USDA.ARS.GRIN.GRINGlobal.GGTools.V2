using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SubtribeManager : AppDataManagerBase, IManager<Subtribe, SubtribeSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public void BuildInsertUpdateParameters(Subtribe entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_subtribe_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("taxonomy_tribe_id", entity.TribeID == 0 ? DBNull.Value : (object)entity.TribeID, true);
            AddParameter("taxonomy_subfamily_id", entity.SubfamilyID == 0 ? DBNull.Value : (object)entity.SubfamilyID, true);
            AddParameter("accepted_infra_family_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("subtribe_name", String.IsNullOrEmpty(entity.TribeName) ? DBNull.Value : (object)entity.TribeName, true);
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

        public int Delete(Subtribe entity)
        {
            throw new NotImplementedException();
        }

        public Subtribe Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(Subtribe entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Subtribe>(entity);
            SQL = "usp_GGTools_Taxon_Subtribe_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_subtribe_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_subtribe_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public List<Subtribe> Search(SubtribeSearch searchEntity)
        {
            List<Subtribe> results = new List<Subtribe>();

            SQL = " SELECT * FROM vw_GGTools_Taxon_Subtribes";
            SQL += " WHERE (@TribeName                  IS NULL OR  TribeName              LIKE         @TribeName + '%')";
            SQL += " AND (@SubfamilyName              IS NULL OR  SubfamilyName          LIKE         @SubfamilyName + '%')";
            SQL += " AND (@FamilyName                 IS NULL OR  FamilyName             LIKE         @FamilyName + '%')";
            SQL += " AND (@Name                    IS NULL OR   Name                     LIKE         @Name + '%')";
            SQL += " AND  (@CreatedByCooperatorID   IS NULL OR   CreatedByCooperatorID    =            @CreatedByCooperatorID)";
            SQL += " AND  (@ID                      IS NULL OR   ID                       =            @ID)";
   
            var parameters = new List<IDbDataParameter> {
                CreateParameter("Name", (object)searchEntity.SubtribeName ?? DBNull.Value, true),
                CreateParameter("TribeName", (object)searchEntity.TribeName ?? DBNull.Value, true),
                CreateParameter("SubfamilyName", (object)searchEntity.SubfamilyName ?? DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("FamilyID", searchEntity.FamilyID > 0 ? (object)searchEntity.FamilyID : DBNull.Value, true),
            };

            results = GetRecords<Subtribe>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Subtribe> SearchFolderItems(SubtribeSearch searchEntity)
        {
            List<Subtribe> results = new List<Subtribe>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgti.* " +
                " FROM vw_GGTools_Taxon_InfraFamilies vgti " +
                " JOIN app_user_item_list auil " +
                " ON vgti.ID = auil.id_number " +
                " WHERE auil.id_type = 'just_subtribe' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Subtribe>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public int Update(Subtribe entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Subtribe>(entity);

            SQL = "usp_GGTools_Taxon_Subtribe_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            return RowsAffected;
        }

    
       
    }
}
