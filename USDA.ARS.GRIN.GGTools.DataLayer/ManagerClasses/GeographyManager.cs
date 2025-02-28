using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class GeographyManager : GRINGlobalDataManagerBase, IManager<Geography, GeographySearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Geography entity)
        {
            throw new NotImplementedException();
        }

        public Geography Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(Geography entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Geography>(entity);
            SQL = "usp_GRINGlobal_Geography_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_geography_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_geography_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = entity.ID;
            return entity.ID;
        }

        public List<Geography> Search(GeographySearch searchEntity)
        {
            List<Geography> results = new List<Geography>();
           
            SQL = " SELECT * FROM vw_GRINGlobal_Geography ";
            SQL += " WHERE  (@ID                            IS NULL OR ID                       =       @ID)";
            SQL += " AND    (@CreatedByCooperatorID         IS NULL OR  CreatedByCooperatorID   =       @CreatedByCooperatorID)";
            SQL += " AND    (@CreatedDate                   IS NULL OR  CreatedDate             =       @CreatedDate)";
            SQL += " AND    (@ModifiedByCooperatorID        IS NULL OR  ModifiedByCooperatorID  =       @ModifiedByCooperatorID)";
            SQL += " AND    (@ModifiedDate                  IS NULL OR  ModifiedDate            =       @ModifiedDate)";
            SQL += " AND    (@Note                          IS NULL OR  Note                    LIKE    '%' + @Note + '%')";
            SQL += " AND    (@CountryCode                   IS NULL OR  CountryCode             =       @CountryCode)";
            SQL += " AND    (@CountryDescription            IS NULL OR  CountryDescription      LIKE    '%' + @CountryDescription + '%')";
            SQL += " AND    (@ContinentName                 IS NULL OR  Continent               LIKE    '%' + @ContinentName + '%')";
            SQL += " AND    (@ContinentRegionID             IS NULL OR  ContinentRegionID       =       @ContinentRegionID)";
            SQL += " AND    (@SubcontinentName              IS NULL OR  Subcontinent            LIKE    '%' + @SubcontinentName + '%')";
            SQL += " AND    (@SubContinentRegionID          IS NULL OR  SubContinentRegionID    =       @SubContinentRegionID)";
            SQL += " AND    (@IsValid                       IS NULL OR  IsValid                 =       @IsValid)";
            SQL += " AND    (@IsRegionMapped                IS NULL OR  IsRegionMapped          =       @IsRegionMapped)";

            SQL += " AND    (@Admin1 IS NULL OR Admin1 COLLATE Latin1_General_CI_AI LIKE '%' + @Admin1 + '%')";
            SQL += " AND    (@Admin1Abbrev IS NULL OR Admin1Abbrev LIKE '%' + @Admin1Abbrev + '%')";
            SQL += " AND    (@Admin1TypeCode IS NULL OR Admin1TypeCode = @Admin1TypeCode)";
            SQL += " AND    (@Admin2 IS NULL OR Admin2 COLLATE Latin1_General_CI_AI LIKE '%' + @Admin2 + '%')";
            SQL += " AND    (@Admin2Abbrev IS NULL OR Admin2Abbrev LIKE '%' + @Admin2Abbrev + '%')";
            SQL += " AND    (@Admin2TypeCode IS NULL OR Admin2TypeCode = @Admin2TypeCode)";

            if (!String.IsNullOrEmpty(searchEntity.IDList))
            {
                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }
                SQL += " ID IN (" + searchEntity.IDList + ")";
            }

            if (!String.IsNullOrEmpty(searchEntity.SubContinentIDList))
            {
                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }
                SQL +=  " RegionID IN (" + searchEntity.SubContinentIDList + ")";
            }

            if (!String.IsNullOrEmpty(searchEntity.CountryCodeList))
            {
                searchEntity.CountryCodeList = String.Join(",", Array.ConvertAll(searchEntity.CountryCodeList.Split(','), z => "'" + z + "'"));

                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }

                SQL += "  CountryCode IN (" + searchEntity.CountryCodeList + ")";
            }

            //if (!String.IsNullOrEmpty(searchEntity.SubContinentIDList))
            //{
            //    whereClause += ")";
            //}

            //if (!String.IsNullOrEmpty(whereClause))
            //{
            //    SQL += whereClause;
            //}

            SQL += " ORDER BY Continent, SubContinent, CountryDescription ";

            var parameters = new List<IDbDataParameter> {
                //EXTENDED
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),
                CreateParameter("CountryDescription", (object)searchEntity.CountryDescription ?? DBNull.Value, true),
                CreateParameter("ContinentName", (object)searchEntity.ContinentName ?? DBNull.Value, true),
                CreateParameter("ContinentRegionID", searchEntity.ContinentRegionID > 0 ? (object)searchEntity.ContinentRegionID : DBNull.Value, true),
                CreateParameter("SubContinentName", (object)searchEntity.SubContinentName ?? DBNull.Value, true),
                CreateParameter("SubContinentRegionID", searchEntity.SubContinentRegionID > 0 ? (object)searchEntity.SubContinentRegionID : DBNull.Value, true),
                CreateParameter("CountryCode", (object)searchEntity.CountryCode ?? DBNull.Value, true),
                CreateParameter("IsValid", (object)searchEntity.IsValid ?? DBNull.Value, true),
                CreateParameter("IsRegionMapped", (object)searchEntity.IsRegionMapped ?? DBNull.Value, true),
                
                //EXTENDED 
                CreateParameter("Admin1", (object)searchEntity.Admin1 ?? DBNull.Value, true),
                CreateParameter("Admin1Abbrev", (object)searchEntity.Admin1Abbrev ?? DBNull.Value, true),
                CreateParameter("Admin1TypeCode", (object)searchEntity.Admin1TypeCode ?? DBNull.Value, true),
                CreateParameter("Admin2", (object)searchEntity.Admin2 ?? DBNull.Value, true),
                CreateParameter("Admin2Abbrev", (object)searchEntity.Admin2Abbrev ?? DBNull.Value, true),
                CreateParameter("Admin2TypeCode", (object)searchEntity.Admin2TypeCode ?? DBNull.Value, true),
            };

            results = GetRecords<Geography>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public List<Geography> GetFolderItems(GeographySearch searchEntity)
        {
            List<Geography> results = new List<Geography>();

            SQL = " SELECT * FROM vw_GRINGlobal_Geography_Sys_Folder_Item_Map WHERE SysFolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Geography>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
       
        public int Update(Geography entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Geography>(entity);
            SQL = "usp_GRINGlobal_Geography_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return RowsAffected;
        }

        protected virtual void BuildInsertUpdateParameters(Geography entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("geography_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("current_geography_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("country_code", (object)entity.CountryCode ?? DBNull.Value, true);
            AddParameter("adm1", (object)entity.Admin1 ?? DBNull.Value, true);
            AddParameter("adm1_type_code", (object)entity.Admin1TypeCode ?? DBNull.Value, true);
            AddParameter("adm1_abbrev", (object)entity.Admin1Abbrev ?? DBNull.Value, true);
            AddParameter("adm2", (object)entity.Admin2 ?? DBNull.Value, true);
            AddParameter("adm2_type_code", (object)entity.Admin2TypeCode ?? DBNull.Value, true);
            AddParameter("adm2_abbrev", (object)entity.Admin2Abbrev ?? DBNull.Value, true);

            if (entity.ChangedDate > DateTime.MinValue)
            {
                AddParameter("changed_date", (object)entity.ChangedDate ?? DBNull.Value, true);
            }
            else
            {
                AddParameter("changed_date", DBNull.Value, true);
            }

            AddParameter("is_valid", (object)entity.IsValid ?? DBNull.Value, false);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            
            if(entity.ID > 0)
            {
                AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            }
            else
            {
                AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            }
        }

        

        public List<Region> GetRegions()
        {
            List<Region> results = new List<Region>();

            SQL = "usp_GRINGlobal_Regions_Select";
            results = GetRecords<Region>(SQL, CommandType.StoredProcedure);
            return results;
        }

        public List<Region> GetContinents()
        {
            List<Region> results = new List<Region>();

            SQL = " SELECT * FROM vw_GRINGlobal_Geography_Continent ";
            SQL += " ORDER BY Continent ASC ";

            results = GetRecords<Region>(SQL);
            RowsAffected = results.Count;
            return results;
        }

        public List<Region> GetSubContinents(string continentList = "")
        {
            List<Region> results = new List<Region>();

            SQL = " SELECT * FROM vw_GRINGlobal_Geography_SubContinent ";

            if (!String.IsNullOrEmpty(continentList))
            {
                SQL += " WHERE Continent IN (" + continentList + ')';
                SQL += " OR Continent = '(No Region)'";
            }

            SQL += " ORDER BY SubContinent ASC ";

            results = GetRecords<Region>(SQL);
            RowsAffected = results.Count;
            return results;
        }

        public List<Country> GetCountries(string continentNameList = "", string subContinents = "", string isRegionMapped = "Y")
        {
            List<Country> results = new List<Country>();
            string sqlOperator = " WHERE ";

            SQL = " SELECT DISTINCT CountryCode, CountryDescription FROM vw_GRINGlobal_Geography WHERE IsValid = 'Y' ";

            if (isRegionMapped == "Y")
            {
                if (!String.IsNullOrEmpty(continentNameList))
                {
                    SQL += " AND Continent IN (" + continentNameList + ") ";

                }

                if (!String.IsNullOrEmpty(subContinents))
                {
                    if (SQL.Contains("WHERE"))
                    {
                        sqlOperator = " AND ";
                    }
                    else
                    {
                        sqlOperator = " WHERE ";
                    }
                    SQL += sqlOperator + " SubContinent IN (" + subContinents + ')';
                }
            }
            else
            {
                SQL += " AND RegionID = -9";
            }

            SQL += " ORDER BY CountryDescription ASC ";

            results = GetRecords<Country>(SQL);
            RowsAffected = results.Count;
            return results;
        }

        public List<Geography> GetGeographies(string subContinentRegionIdList = "", string countryList = "")
        {
            List<Geography> results = new List<Geography>();

            SQL = " SELECT * FROM vw_GRINGlobal_Geography_Administrative_Unit ";

            if (!String.IsNullOrEmpty(countryList))
            {
                SQL += " WHERE CountryCode IN (" + countryList + ')';
            }

            if (!String.IsNullOrEmpty(subContinentRegionIdList))
            {
                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }

                SQL += " RegionID IN (" + subContinentRegionIdList + ')';
            }

            SQL += " ORDER BY AssembledName ASC ";

            results = GetRecords<Geography>(SQL);
            RowsAffected = results.Count;
            return results;
        }

        public List<Geography> GetGeographyCountryAdmins()
        {
            List<Geography> results = new List<Geography>();

            SQL = "usp_GRINGlobal_Geography_CountryAdmins_Select";

            results = GetRecords<Geography>(SQL);
            RowsAffected = results.Count;
            return results;
        }

        public List<Geography> GetStates()
        {
            List<Geography> results = new List<Geography>();

            SQL = " SELECT ID, Admin1 FROM vw_GRINGlobal_Geography_State WHERE Admin1 IS NOT NULL ";
            SQL += " ORDER BY Admin1 ASC ";

            results = GetRecords<Geography>(SQL);
            RowsAffected = results.Count;
            return results;
        }
    }
}
