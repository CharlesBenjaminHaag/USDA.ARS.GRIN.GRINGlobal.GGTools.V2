using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class FamilyManager : GRINGlobalDataManagerBase, IManager<Family, FamilySearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Family entity)
        {
            throw new NotImplementedException();
        }

        public Family Get(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Family_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@taxonomy_family_id", (object)entityId, false)
            };
            Family family = GetRecord<Family>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return family;
        }

        public List<Family> GetSynonyms(int entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Family_Synonyms_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@taxonomy_family_id", (object)entityId, false)
            };
            List<Family> familyMaps = GetRecords<Family>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return familyMaps;
        }
        
        public List<Family> GetSubdivisions(string familyName)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Family_Subdivisions_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@family_name", (object)familyName, false)
            };
            List<Family> familyMaps = GetRecords<Family>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return familyMaps;
        }
  
        public int Insert(Family entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Family>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Family_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
           
            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_id", -1);
           
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        
        public int Update(Family entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Family>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Family_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public List<Family> Search(FamilySearch searchEntity)
        {
            List<Family> results = new List<Family>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Family ";
            SQL += " WHERE  (@ID                        IS NULL     OR  ID                      =       @ID) ";
            SQL += " AND  (@AcceptedID                  IS NULL     OR  AcceptedID              =       @AcceptedID) ";
            SQL += " AND    (@IsAcceptedName            IS NULL     OR  IsAcceptedName          =       @IsAcceptedName)";
            SQL += " AND  (@TypeGenusID                 IS NULL     OR  TypeGenusID             =       @TypeGenusID) ";
            SQL += " AND    (@TypeGenusName             IS NULL     OR  TypeGenusName           LIKE    '%' +  @TypeGenusName + '%')";
            SQL += " AND    (@ClassificationID          IS NULL     OR  ClassificationID        =       @ClassificationID)";
            SQL += " AND    (@ClassificationName        IS NULL     OR  ClassificationName      LIKE    '%' +  @ClassificationName + '%')";
            SQL += " AND    (@SuprafamilyRankCode       IS NULL     OR  SuprafamilyRankCode     LIKE    '%' +  @SuprafamilyRankCode + '%')";
            SQL += " AND    (@SuprafamilyRankName       IS NULL     OR  SuprafamilyRankName     LIKE    '%' +  @SuprafamilyRankName + '%')";
            SQL += " AND    (@FamilyName                IS NULL     OR  FamilyName              LIKE    '%' +  @FamilyName + '%')";
            SQL += " AND    (@FamilyAuthority           IS NULL     OR  FamilyAuthority         LIKE    '%' +  @FamilyAuthority + '%')";
            SQL += " AND    (@AlternateName             IS NULL     OR  AlternateName           LIKE    '%' +  @AlternateName + '%')";
            SQL += " AND    (@SubfamilyName             IS NULL     OR  SubfamilyName           LIKE    '%' +  @SubfamilyName + '%')";
            SQL += " AND    (@TribeName                 IS NULL     OR  TribeName               LIKE    '%' +  @TribeName + '%')";
            SQL += " AND    (@SubtribeName              IS NULL     OR  SubtribeName            LIKE    '%' +  @SubtribeName + '%')";
            SQL += " AND    (@FamilyTypeCode            IS NULL     OR  FamilyTypeCode          =       @FamilyTypeCode)";
            SQL += " AND    (@Note                      IS NULL     OR  Note                    LIKE    '%' +  @Note + '%')";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL     OR  CreatedByCooperatorID   =       @CreatedByCooperatorID)";
            SQL += " AND    (@ModifiedByCooperatorID    IS NULL     OR  ModifiedByCooperatorID  =       @ModifiedByCooperatorID)";
            SQL += " AND    (@OwnedByCooperatorID       IS NULL     OR  OwnedByCooperatorID     =       @OwnedByCooperatorID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("AcceptedID", searchEntity.AcceptedID > 0 ? (object)searchEntity.AcceptedID : DBNull.Value, true),
                CreateParameter("IsAcceptedName", (object)searchEntity.IsAcceptedName ?? DBNull.Value, true),
                CreateParameter("TypeGenusID", searchEntity.TypeGenusID > 0 ? (object)searchEntity.TypeGenusID : DBNull.Value, true),
                CreateParameter("TypeGenusName", (object)searchEntity.TypeGenusName ?? DBNull.Value, true),
                CreateParameter("ClassificationID", searchEntity.ClassificationID > 0 ? (object)searchEntity.ClassificationID : DBNull.Value, true),
                CreateParameter("ClassificationName", (object)searchEntity.ClassificationName ?? DBNull.Value, true),
                CreateParameter("SuprafamilyRankCode", (object)searchEntity.SuprafamilyRankCode ?? DBNull.Value, true),
                CreateParameter("SuprafamilyRankName", (object)searchEntity.SuprafamilyRankName ?? DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("FamilyAuthority", (object)searchEntity.FamilyAuthority ?? DBNull.Value, true),
                CreateParameter("AlternateName", (object)searchEntity.AlternateName ?? DBNull.Value, true),
                CreateParameter("SubfamilyName", (object)searchEntity.SubfamilyName ?? DBNull.Value, true),
                CreateParameter("TribeName", (object)searchEntity.TribeName ?? DBNull.Value, true),
                CreateParameter("SubtribeName", (object)searchEntity.SubtribeName ?? DBNull.Value, true),
                CreateParameter("FamilyTypeCode", (object)searchEntity.FamilyTypeCode ?? DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("OwnedByCooperatorID", searchEntity.OwnedByCooperatorID > 0 ? (object)searchEntity.OwnedByCooperatorID : DBNull.Value, true),

            };

            results = GetRecords<Family>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<Family> GetFolderItems(int entityId)
        {
            List<Family> results = new List<Family>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Family_Sys_Folder_Item_Map ";
            SQL += "WHERE  (@FolderID   IS NULL OR  SysFolderID       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", entityId > 0 ? (object)entityId : DBNull.Value, true)
            };
            results = GetRecords<Family>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
              
        public void BuildInsertUpdateParameters(Family entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_family_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("current_taxonomy_family_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("suprafamily_rank_code", (object)entity.SuprafamilyRankCode ?? DBNull.Value, true);
            AddParameter("suprafamily_rank_name", (object)entity.SuprafamilyRankName ?? DBNull.Value, true);
            AddParameter("family_name", (object)entity.FamilyName ?? DBNull.Value, true);
            AddParameter("family_authority", (object)entity.FamilyAuthority ?? DBNull.Value, true);
            AddParameter("alternate_name", (object)entity.AlternateName ?? DBNull.Value, true);
            AddParameter("subfamily_name", (object)entity.SubfamilyName ?? DBNull.Value, true);
            AddParameter("tribe_name", (object)entity.TribeName ?? DBNull.Value, true);
            AddParameter("subtribe_name", (object)entity.SubtribeName ?? DBNull.Value, true);
            AddParameter("family_type_code", (object)entity.FamilyTypeCode ?? DBNull.Value, true);
            AddParameter("taxonomy_classification_id", entity.ClassificationID == 0 ? DBNull.Value : (object)entity.ClassificationID, true);
             
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
