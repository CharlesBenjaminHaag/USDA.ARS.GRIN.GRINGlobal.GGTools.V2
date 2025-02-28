using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class FamilyMapManager : GRINGlobalDataManagerBase, IManager<FamilyMap, FamilyMapSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(FamilyMap entity)
        {
            throw new NotImplementedException();
        }

        public FamilyMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<FamilyMap> GetSynonyms(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Family_Map_Synonyms_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@taxonomy_family_map_id", (object)entityId, false)
            };
            List<FamilyMap> familyMaps = GetRecords<FamilyMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return familyMaps;
        }
        public List<FamilyMap> GetSubdivisions(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Family_Subdivisions_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@taxonomy_family_map_id", (object)entityId, false)
            };
            List<FamilyMap> familyMaps = GetRecords<FamilyMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return familyMaps;
        }

        /// <summary>
        /// Returns a list of all genera, and synonyms and subdivisions, linked to a specified
        /// family ID.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>A collection of Genus objects</returns>
        /// <remarks>Most likely belongs in Genus manager -- to refactor.</remarks>
        public List<Genus> GetGenera(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Family_Genera_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@taxonomy_family_id", (object)entityId, false)
            };
            List<Genus> genera = GetRecords<Genus>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return genera;
        }
        public int Insert(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Family_Map_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family2_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
      
            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_map_id", -1);
            entity.FamilyID = GetParameterValue<int>("@out_taxonomy_family2_id", -1);

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public int InsertSubfamily(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Subfamily_Insert";

            BuildSubfamilyInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_subfamily_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public int InsertTribe(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Tribe_Insert";

            BuildTribeInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;

        }

        public int InsertSubtribe(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Subtribe_Insert";

            BuildSubtribeInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;

        }

        public List<FamilyMap> Search(FamilyMapSearch searchEntity)
        {
            List<FamilyMap> results = new List<FamilyMap>();

            SQL = "SELECT *, ";
            SQL += "LTRIM(RTRIM(COALESCE(FamilyName, ''))) + " +
                " CASE COALESCE(convert(nvarchar, SubfamilyName), '') " +
                " WHEN '' THEN '' ELSE '' + ' subfam. ' + LTRIM(RTRIM(SubfamilyName)) END " +
                " + CASE COALESCE(convert(nvarchar, TribeName), '') WHEN '' THEN '' ELSE '' + " +
                "' tr. ' + LTRIM(RTRIM(TribeName)) END " +
                " + CASE COALESCE(convert(nvarchar, SubtribeName), '') WHEN '' THEN '' ELSE '' + " +
                "' subtr. ' + LTRIM(RTRIM(SubtribeName)) END AS AssembledName FROM vw_GRINGlobal_Taxonomy_Family_Map ";
            SQL += " WHERE  (@ID                    IS NULL     OR  ID                  =       @ID) ";
            SQL += " AND    (@Rank                  IS NULL     OR  Rank                =       @Rank)";
            SQL += " AND    (@IsAcceptedName        IS NULL     OR  IsAcceptedName      =       @IsAcceptedName)";
            SQL += " AND    (@IsInfrafamilial       IS NULL     OR  IsInfrafamilial     =       @IsInfrafamilial)";
            SQL += " AND    (@OrderID               IS NULL     OR  OrderID             =       @OrderID)";
            SQL += " AND    (@OrderName             IS NULL     OR  OrderName           LIKE    '%' +  @OrderName + '%')";
            SQL += " AND    (@FamilyName            IS NULL     OR  FamilyName          LIKE    '%' +  @FamilyName + '%')";
            SQL += " AND    (@FamilyTypeCode        IS NULL     OR  FamilyTypeCode      =       @FamilyTypeCode)";
            SQL += " AND    (@Authority             IS NULL     OR  Authority           LIKE    '%' +  @Authority + '%')";
            SQL += " AND    (@AlternateName         IS NULL     OR  AlternateName       LIKE    '%' +  @AlternateName + '%')";
            SQL += " AND    (@SubfamilyName         IS NULL     OR  SubfamilyName       LIKE    '%' +  @SubfamilyName + '%')";
            SQL += " AND    (@TribeName             IS NULL     OR  TribeName           LIKE    '%' +  @TribeName + '%')";
            SQL += " AND    (@SubtribeName          IS NULL     OR  SubtribeName        LIKE    '%' +  @SubtribeName + '%')";
            SQL += " AND    (@CreatedByCooperatorID IS NULL     OR  CreatedByCooperatorID   =           @CreatedByCooperatorID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("Rank", (object)searchEntity.Rank ?? DBNull.Value, true),
                CreateParameter("IsAcceptedName", (object)searchEntity.IsAcceptedName ?? DBNull.Value, true),
                CreateParameter("IsInfrafamilial", (object)searchEntity.IsInfrafamilal ?? DBNull.Value, true),
                CreateParameter("OrderID", searchEntity.OrderID > 0 ? (object)searchEntity.OrderID : DBNull.Value, true),
                CreateParameter("OrderName", (object)searchEntity.OrderName ?? DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("FamilyTypeCode", (object)searchEntity.FamilyTypeCode ?? DBNull.Value, true),
                CreateParameter("AlternateName", (object)searchEntity.AlternateName ?? DBNull.Value, true),
                CreateParameter("Authority", (object)searchEntity.Authority ?? DBNull.Value, true),
                CreateParameter("SubfamilyName", (object)searchEntity.SubFamilyName ?? DBNull.Value, true),
                CreateParameter("TribeName", (object)searchEntity.TribeName ?? DBNull.Value, true),
                CreateParameter("SubtribeName", (object)searchEntity.SubTribeName ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
            };

            results = GetRecords<FamilyMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<FamilyMap> GetFolderItems(FamilyMapSearch searchEntity)
        {
            List<FamilyMap> results = new List<FamilyMap>();

            SQL = " SELECT * FROM vw_GRINGlobal_Folder_Taxonomy_Family_Map ";
            SQL += "WHERE  (@FolderID   IS NULL OR  FolderID       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<FamilyMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public int Update(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Family_Map_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public int UpdateSubfamily(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Subfamily_Update";

            BuildSubfamilyInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        public int UpdateTribe(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Tribe_Update";

            BuildTribeInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        public int UpdateSubtribe(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Subtribe_Update";

            BuildSubtribeInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
      
        public void BuildInsertUpdateParameters(FamilyMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_family_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("taxonomy_family_map_accepted_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("taxonomy_order_id", entity.OrderID == 0 ? DBNull.Value : (object)entity.OrderID, true);
            AddParameter("family_name", (object)entity.FamilyName ?? DBNull.Value, true);
            AddParameter("family_authority", (object)entity.Authority ?? DBNull.Value, true);
            AddParameter("alternate_name", (object)entity.AlternateName ?? DBNull.Value, true);
            AddParameter("family_type_code", (object)entity.FamilyTypeCode ?? DBNull.Value, true);
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

        public void BuildSubfamilyInsertUpdateParameters(FamilyMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_family_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("taxonomy_family_map_accepted_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("subfamily_name", (object)entity.SubfamilyName ?? DBNull.Value, true);
            AddParameter("authority", (object)entity.Authority ?? DBNull.Value, true);
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

        public void BuildTribeInsertUpdateParameters(FamilyMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_family_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("taxonomy_family_map_accepted_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("taxonomy_subfamily_id", entity.SubfamilyID == 0 ? DBNull.Value : (object)entity.SubfamilyID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("tribe_name", (object)entity.TribeName ?? DBNull.Value, true);
            AddParameter("authority", (object)entity.Authority ?? DBNull.Value, true);
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
        public void BuildSubtribeInsertUpdateParameters(FamilyMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_family_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("taxonomy_family_map_accepted_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("taxonomy_subfamily_id", entity.SubfamilyID == 0 ? DBNull.Value : (object)entity.SubfamilyID, true);
            AddParameter("taxonomy_tribe_id", entity.TribeID == 0 ? DBNull.Value : (object)entity.TribeID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("subtribe_name", (object)entity.SubtribeName ?? DBNull.Value, true);
            AddParameter("authority", (object)entity.Authority ?? DBNull.Value, true);
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
