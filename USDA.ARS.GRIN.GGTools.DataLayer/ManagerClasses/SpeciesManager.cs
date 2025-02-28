using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesManager : GRINGlobalDataManagerBase, IManager<Species, SpeciesSearch>
    {
        public int Insert(Species entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Species>(entity);
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

        public int Update(Species entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Species>(entity);
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

        public int Delete(Species entity)
        {
            throw new NotImplementedException();
        }

        public Species Get(int id)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Species_Select";
            Species species = new Species();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_species_id", (object)id, false)
            };

            species = GetRecord<Species>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return species;
        }

        public List<Species> GetConspecific(int? entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Species_Conspecific_Select";
            List<Species> speciesList = new List<Species>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_species_id", (object)entityId, false)
            };

            speciesList = GetRecords<Species>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return speciesList;
        }

        public List<Species> GetSynonyms(int? entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Species_Synonyms_Select";
            List<Species> speciesList = new List<Species>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_species_id", (object)entityId, false)
            };

            speciesList = GetRecords<Species>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return speciesList;
        }

        public List<Species> GetInfraspecificAutonym(string genusName, string speciesName, string rank)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Infraspecific_Autonym_Select";
            List<Species> speciesList = new List<Species>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("genus_name", (object)genusName, false),
                CreateParameter("species_name", (object)speciesName, false),           
                CreateParameter("rank", (object)rank, false)
             };

            speciesList = GetRecords<Species>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return speciesList;
        }

        public List<Species> Search(SpeciesSearch searchEntity)
        {
            List<Species> results = new List<Species>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Species ";

            // PRIMARY
            SQL += " WHERE      ((@Name                 IS NULL OR  REPLACE(Name, ' x ', '')        LIKE    'X ' + @Name + '%')";
            SQL += " OR         (@Name                  IS NULL OR  REPLACE(Name, ' x ', '')        LIKE    '+' + @Name + '%')";
            SQL += " OR         (@Name                  IS NULL OR  REPLACE(Name, ' x ', '')        LIKE    @Name + '%')";
            SQL += " OR         (@Name                  IS NULL OR  Name                            LIKE    @Name + '%'))";

            SQL += " AND        (@IsAcceptedName        IS NULL OR  IsAcceptedName                      =       @IsAcceptedName)";
            SQL += " AND        (@SynonymCode           IS NULL OR  SynonymCode                         =       @SynonymCode)";

            // EXTENDED
            SQL += " AND        (@CreatedByCooperatorID     IS NULL OR  CreatedByCooperatorID = @CreatedByCooperatorID)";
            // TODO CR DATE
            SQL += " AND        (@ModifiedByCooperatorID    IS NULL OR  ModifiedByCooperatorID          = @ModifiedByCooperatorID)";
            // TODO MOD DATE

            SQL += " AND        (@ID                        IS NULL OR  ID = @ID) ";
            SQL += " AND        (@AcceptedID                IS NULL OR  AcceptedID                      = @AcceptedID) ";
            SQL += " AND        (@GenusID                   IS NULL OR  GenusID                         = @GenusID) ";
            SQL += " AND        (@NomenNumber               IS NULL OR  NomenNumber = @NomenNumber) ";
            SQL += " AND        (@Note                      IS NULL OR  Note                            LIKE    '%' + @Note + '%')";
            SQL += " AND        (@IsVerified                IS NULL OR  IsVerified                      =       @IsVerified)";
            SQL += " AND        (@VerifiedByCooperatorID    IS NULL OR  VerifiedByCooperatorID          = @VerifiedByCooperatorID)";

            // Verification date logic
            if ((searchEntity.NameVerifiedDateFrom > DateTime.MinValue) && (searchEntity.NameVerifiedDateTo > DateTime.MinValue))
            {
                SQL += " AND NameVerifiedDate >= '" + searchEntity.NameVerifiedDateFrom + "' AND NameVerifiedDate <= '" + searchEntity.NameVerifiedDateTo + "'";
            }

            SQL += " AND        (@SpeciesName               IS NULL OR  SpeciesName                     LIKE    '%' + @SpeciesName + '%')";
            SQL += " AND        (@SpeciesAuthority          IS NULL OR  SpeciesAuthority                LIKE    '%' + @SpeciesAuthority + '%')";
            SQL += " AND        (@NameAuthority             IS NULL OR  NameAuthority                   LIKE    '%' + @NameAuthority + '%')";
            SQL += " AND        (@IsSpecificHybrid          IS NULL OR  IsSpecificHybrid                =       @IsSpecificHybrid)";
            SQL += " AND        (@HybridParentage           IS NULL OR  HybridParentage                 LIKE    '%' + @HybridParentage + '%')";
            SQL += " And        (@AlternateName             IS NULL OR  AlternateName                   LIKE    '%' + @AlternateName + '%')";

            if (searchEntity.IsLinkedToAccessions == "Y")
            {
                SQL += " AND AccessionCount > 0";
            }

            SQL += " AND        (@IsWebVisible              IS NULL OR  IsWebVisible                    =       @IsWebVisible)";

            if (searchEntity.ProtologueVirtualPathIsNull == true)
            {
                SQL += "AND ProtologueVirtualPath IS NULL ";
            }
            else
            {
                SQL += " AND        (@Protologue                IS NULL OR  Protologue                      LIKE    '%' + @Protologue + '%')";
            }
            
            SQL += " AND        (@ProtologueVirtualPath     IS NULL OR  ProtologueVirtualPath           LIKE    '%' + @ProtologueVirtualPath + '%')";
            SQL += " AND        (@SubspeciesName            IS NULL OR  SubspeciesName                  LIKE    '%' + @SubspeciesName + '%')";
            SQL += " AND        (@SubspeciesAuthority       IS NULL OR  SubspeciesAuthority             LIKE    '%' + @SubspeciesAuthority + '%')";
            SQL += " AND        (@IsSubspecificHybrid       IS NULL OR  IsSubspecificHybrid             =       @IsSubspecificHybrid)";
            SQL += " AND        (@VarietyName               IS NULL OR  VarietyName                     LIKE    '%' + @VarietyName + '%')";
            SQL += " AND        (@VarietyAuthority          IS NULL OR  VarietyAuthority                LIKE    '%' + @VarietyAuthority + '%')";
            SQL += " AND        (@IsVarietalHybrid          IS NULL OR  IsVarietalHybrid                =       @IsVarietalHybrid)";
            SQL += " AND        (@SubvarietyName            IS NULL OR  SubvarietyName                  LIKE    '%' + @SubvarietyName + '%')";
            SQL += " AND        (@SubvarietyAuthority       IS NULL OR  SubvarietyAuthority             LIKE    '%' + @SubvarietyAuthority + '%')";
            SQL += " AND        (@IsSubvarietalHybrid       IS NULL OR  IsSubvarietalHybrid             =       @IsSubvarietalHybrid)";
            SQL += " AND        (@FormaName                 IS NULL OR  FormaName                       LIKE    '%' + @FormaName + '%')";
            SQL += " AND        (@FormaAuthority            IS NULL OR  FormaAuthority                  LIKE    '%' + @FormaAuthority + '%')";
            SQL += " AND        (@IsFormaHybrid             IS NULL OR  IsFormaHybrid                   =       @IsFormaHybrid)";
            SQL += " AND        (@FormaRankType             IS NULL OR  FormaRankType                   =       @FormaRankType)";
            SQL += " AND        (@GenusName                 IS NULL OR  GenusName                       LIKE    '%' + @GenusName + '%')";
            SQL += " AND        (@GenusHybridCode           IS NULL OR  GenusHybridCode                 =       @GenusHybridCode)";
            SQL += " AND        (@SubGenusName              IS NULL OR  SubGenusName                    LIKE    '%' + @SubGenusName + '%')";
            SQL += " AND        (@CommonFertilizationCode   IS NULL OR  CommonFertilizationCode         =       @CommonFertilizationCode)";
            SQL += " AND        (@RestrictionCode           IS NULL OR  RestrictionCode                 =       @RestrictionCode)";
         
            if (!String.IsNullOrEmpty(searchEntity.IDList))
            {
                SQL += " AND (ID IN (" + searchEntity.IDList + "))";
            }

            if (searchEntity.ExcludeID > 0)
            {
                SQL += " AND ID <> " + searchEntity.ExcludeID;
            }

            SQL = GetCreatedDateRangeSQL(searchEntity, SQL);
            SQL = GetModifiedDateRangeSQL(searchEntity, SQL);

           

            var parameters = new List<IDbDataParameter> {
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                CreateParameter("Name", (object)searchEntity.Name ?? DBNull.Value, true),
                CreateParameter("IsAcceptedName", (object)searchEntity.IsAcceptedName ?? DBNull.Value, true),
                CreateParameter("SynonymCode", (object)searchEntity.SynonymCode ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("AcceptedID", searchEntity.AcceptedNameID > 0 ? (object)searchEntity.AcceptedNameID : DBNull.Value, true),
                CreateParameter("GenusID", searchEntity.GenusID > 0 ? (object)searchEntity.GenusID : DBNull.Value, true),
                CreateParameter("NomenNumber", searchEntity.NomenNumber > 0 ? (object)searchEntity.NomenNumber : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
                CreateParameter("IsVerified", (object)searchEntity.IsVerified ?? DBNull.Value, true),
                CreateParameter("VerifiedByCooperatorID", searchEntity.VerifiedByCooperatorID > 0 ? (object)searchEntity.VerifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("SpeciesAuthority", (object)searchEntity.SpeciesAuthority ?? DBNull.Value, true),
                CreateParameter("NameAuthority", (object)searchEntity.NameAuthority ?? DBNull.Value, true),
                CreateParameter("IsSpecificHybrid", (object)searchEntity.IsSpecificHybrid ?? DBNull.Value, true),
                CreateParameter("HybridParentage", (object)searchEntity.HybridParentage ?? DBNull.Value, true),
                CreateParameter("AlternateName", (object)searchEntity.AlternateName ?? DBNull.Value, true),
                CreateParameter("IsWebVisible", (object)searchEntity.IsWebVisible ?? DBNull.Value, true),
                CreateParameter("Protologue", (object)searchEntity.Protologue ?? DBNull.Value, true),
                CreateParameter("ProtologueVirtualPath", (object)searchEntity.ProtologueVirtualPath ?? DBNull.Value, true),
                CreateParameter("SubSpeciesName", (object)searchEntity.SubspeciesName ?? DBNull.Value, true),
                CreateParameter("SubSpeciesAuthority", (object)searchEntity.SubspeciesAuthority ?? DBNull.Value, true),
                CreateParameter("IsSubSpecificHybrid", (object)searchEntity.IsSubspecificHybrid ?? DBNull.Value, true),
                CreateParameter("VarietyName", (object)searchEntity.VarietyName ?? DBNull.Value, true),
                CreateParameter("VarietyAuthority", (object)searchEntity.VarietyAuthority ?? DBNull.Value, true),
                CreateParameter("IsVarietalHybrid", (object)searchEntity.IsVarietalHybrid ?? DBNull.Value, true),
                CreateParameter("SubvarietyName", (object)searchEntity.SubvarietyName ?? DBNull.Value, true),
                CreateParameter("SubvarietyAuthority", (object)searchEntity.SubvarietyAuthority ?? DBNull.Value, true),
                CreateParameter("IsSubvarietalHybrid", (object)searchEntity.IsSubVarietalHybrid ?? DBNull.Value, true),
                CreateParameter("FormaName", (object)searchEntity.FormaName ?? DBNull.Value, true),
                CreateParameter("FormaAuthority", (object)searchEntity.FormaAuthority ?? DBNull.Value, true),
                CreateParameter("IsFormaHybrid", (object)searchEntity.IsFormaHybrid ?? DBNull.Value, true),
                CreateParameter("FormaRankType", (object)searchEntity.FormaRankType ?? DBNull.Value, true),
                CreateParameter("GenusName", (object)searchEntity.GenusName ?? DBNull.Value, true),
                CreateParameter("GenusHybridCode", (object)searchEntity.GenusHybridCode ?? DBNull.Value, true),
                CreateParameter("SubGenusName", (object)searchEntity.SubGenusName ?? DBNull.Value, true),
                CreateParameter("CommonFertilizationCode", (object)searchEntity.CommonFertilizationCode ?? DBNull.Value, true),
                CreateParameter("RestrictionCode", (object)searchEntity.RestrictionCode ?? DBNull.Value, true),
                
            };

            results = GetRecords<Species>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Species> SearchNames(int genusId, string speciesName)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Species_Matches_Select";
            List<Species> speciesList = new List<Species>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_genus_id", (object)genusId, false),
                CreateParameter("species_name", (object)speciesName, false),
            };

            speciesList = GetRecords<Species>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return speciesList;
        }

        public List<Species> GetFolderItems(SpeciesSearch searchEntity)
        {
            List<Species> results = new List<Species>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Species_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Species>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<CodeValue> SearchProtologues(string protologue)
        {
            List<CodeValue> results = new List<CodeValue>();

            // Replace whitespace with wildcard character(s).
            protologue = protologue.Replace(" ", "%");

            SQL = " SELECT DISTINCT LTRIM(RTRIM(protologue)) AS Value, '' AS Title " +
                    " FROM taxonomy_species " +
                    " WHERE protologue IS NOT NULL ";
            SQL += " AND    (@Protologue       IS NULL OR  Protologue     LIKE     '%' + @Protologue + '%' )";
            SQL += " ORDER BY Value";
            var parameters = new List<IDbDataParameter>
            {
                 CreateParameter("Protologue", (object)protologue ?? DBNull.Value, true)
            };

            results = GetRecords<CodeValue>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        
        public Dictionary<string, string> GetTableNames()
        {
            return new Dictionary<string, string>
            {
                { "taxonomy_family", "Family" },
                { "taxonomy_genus", "Genus" },
                { "taxonomy_species", "Species" }
            };
        }
        
        protected virtual void BuildInsertUpdateParameters(Species entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_species_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("current_taxonomy_species_id", entity.AcceptedID == 0 ? entity.ID : (object)entity.AcceptedID, true);
            AddParameter("is_specific_hybrid", entity.IsSpecificHybrid == null ? "N" : (object)entity.IsSpecificHybrid, false);
            AddParameter("hybrid_parentage", (object)entity.HybridParentage ?? DBNull.Value, true);
            AddParameter("name_authority", (object)entity.NameAuthority ?? DBNull.Value, true);
            AddParameter("species_authority", (object)entity.SpeciesAuthority ?? DBNull.Value, true);
            AddParameter("is_subspecific_hybrid", entity.IsSubspecificHybrid == null ? "N" : (object)entity.IsSubspecificHybrid, false);
            AddParameter("subspecies_name", (object)entity.SubspeciesName ?? DBNull.Value, true);
            AddParameter("subspecies_authority", (object)entity.SubspeciesAuthority ?? DBNull.Value, true);
            AddParameter("is_varietal_hybrid", entity.IsVarietalHybrid == null ? "N" : (object)entity.IsVarietalHybrid, false);
            AddParameter("variety_name", (object)entity.VarietyName ?? DBNull.Value, true);
            AddParameter("variety_authority", (object)entity.VarietyAuthority ?? DBNull.Value, true);
            AddParameter("is_subvarietal_hybrid",  entity.IsSubVarietalHybrid == null ? "N" : (object)entity.IsSubVarietalHybrid, false);
            AddParameter("subvariety_name", (object)entity.SubvarietyName ?? DBNull.Value, true);
            AddParameter("subvariety_authority", (object)entity.SubvarietyAuthority ?? DBNull.Value, true);
            AddParameter("is_forma_hybrid", entity.IsFormaHybrid == null ? "N" : (object)entity.IsFormaHybrid, false);
            AddParameter("forma_rank_type", (object)entity.FormaRankType ?? DBNull.Value, true);
            AddParameter("forma_name", (object)entity.FormaName ?? DBNull.Value, true);
            AddParameter("forma_authority", (object)entity.FormaAuthority ?? DBNull.Value, true);
            AddParameter("taxonomy_genus_id", (object)entity.GenusID ?? DBNull.Value, true);
            AddParameter("synonym_code", (object)entity.SynonymCode ?? DBNull.Value, true);
            AddParameter("verifier_cooperator_id", entity.VerifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.VerifiedByCooperatorID, true);
            
            if (entity.NameVerifiedDate == DateTime.MinValue)
                AddParameter("name_verified_date", DBNull.Value, true);
            else
                AddParameter("name_verified_date", (object)entity.NameVerifiedDate, true);

            AddParameter("name", (object)entity.Name ?? DBNull.Value, true);
            AddParameter("species_name", (object)entity.SpeciesName ?? DBNull.Value, true);
            AddParameter("protologue", (object)entity.Protologue ?? DBNull.Value, true);
            AddParameter("protologue_virtual_path", (object)entity.ProtologueVirtualPath ?? DBNull.Value, true);
            AddParameter("is_web_visible", entity.IsWebVisible == null ? "N" : (object)entity.IsWebVisible, false);
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
        
        public List<CodeValue> GetRanks()
        {
            List<CodeValue> ranks = new List<CodeValue>();
            ranks.Add(new CodeValue { Value="SPECIES", Title="Species" });
            ranks.Add(new CodeValue { Value = "SUBSPECIES", Title = "Subspecies" });
            ranks.Add(new CodeValue { Value = "VARIETY", Title = "Variety" });
            ranks.Add(new CodeValue { Value = "SUBVARIETY", Title = "Subvariety" });
            ranks.Add(new CodeValue { Value = "FORMA", Title = "Forma" });
            return ranks;
        }
        
        public List<Cooperator> GetVerifiedByCooperators()
        {
            SQL = "usp_GRINGlobal_Taxonomy_Verified_By_Cooperators_Select";
            List<Cooperator> cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure);
            RowsAffected = cooperators.Count;
            return cooperators;
        }
    }
}
