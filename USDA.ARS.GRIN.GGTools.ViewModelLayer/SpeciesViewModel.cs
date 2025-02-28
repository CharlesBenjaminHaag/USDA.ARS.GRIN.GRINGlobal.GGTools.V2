using System;
using System.Web.Mvc;
//using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Data.Common;
using System.Data;
using Microsoft.Win32;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SpeciesViewModel : SpeciesViewModelBase, IViewModel<Species>
    {
      
        public void Delete()
        {
            try
            {
                using (SpeciesManager mgr = new SpeciesManager())
                {
                    mgr.Delete(TableName, Entity.ID);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public Species Get(int entityId)
        {
            DataCollection = new Collection<Species>();
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    Entity = mgr.Get(entityId);
                    Entity.IsSpecificHybridOption = ToBool(Entity.IsSpecificHybrid);
                    Entity.IsSubSpecificHybridOption = ToBool(Entity.IsSubspecificHybrid);
                    Entity.IsVarietalHybridOption = ToBool(Entity.IsVarietalHybrid);
                    Entity.IsSubvarietalHybridOption = ToBool(Entity.IsSubVarietalHybrid);
                    Entity.IsFormaHybridOption = ToBool(Entity.IsFormaHybrid);
                    Entity.IsWebVisibleOption = ToBool(Entity.IsWebVisible);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }

            return Entity;
        }
       
        public void GetConspecific()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollection = new Collection<Species>(mgr.GetConspecific(SearchEntity.ID));
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public void GetSynonyms()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollection = new Collection<Species>(mgr.GetSynonyms(SearchEntity.ID));
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public void GetAutonym(string genusName, string speciesName, string rank)
        {
            List<Species> speciesList = new List<Species>();
            using (SpeciesManager mgr = new SpeciesManager())
            {
                speciesList = mgr.GetInfraspecificAutonym(genusName, speciesName, rank);
                if (speciesList.Count == 1)
                {
                    AutonymEntity = speciesList[0];
                }
            }
        }

        public int Insert()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    Entity.IsSpecificHybrid = FromBool(Entity.IsSpecificHybridOption);
                    Entity.IsSubspecificHybrid = FromBool(Entity.IsSubSpecificHybridOption);
                    Entity.IsVarietalHybrid = FromBool(Entity.IsVarietalHybridOption);
                    Entity.IsSubVarietalHybrid = FromBool(Entity.IsSubvarietalHybridOption);
                    Entity.IsFormaHybrid = FromBool(Entity.IsFormaHybridOption);
                    Entity.IsWebVisible = FromBool(Entity.IsWebVisibleOption);

                    SetSpeciesName();
                    SetSpeciesNameAuthority();
                    HandleAccessions();

                    RowsAffected = mgr.Insert(Entity);
                    
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public void Search()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollection = new Collection<Species>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }

                    Entity.IsSpecificHybridOption = ToBool(Entity.IsSpecificHybrid);
                    Entity.IsAccepted = ToBool(Entity.IsAcceptedName);
                    Entity.IsWebVisibleOption = ToBool(Entity.IsWebVisible);

                    if (SearchEntity.GetCommonNames == "Y")
                    {
                        foreach (var species in DataCollection)
                        {
                            using (CommonNameManager commonNameMgr = new CommonNameManager())
                            {
                                species.CommonNames = new Collection<CommonName>(commonNameMgr.Search(new CommonNameSearch { SpeciesID = species.ID }));
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public List<Species> SearchNames(int genusId, string speciesName)
        {
            try
            {
                using (SpeciesManager mgr = new SpeciesManager())
                {
                    return mgr.SearchNames(genusId, speciesName);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public void RunSearch(int appUserItemFolderId)
        {
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            appUserItemListViewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
            appUserItemListViewModel.Search();
            SearchEntity = Deserialize<SpeciesSearch>(appUserItemListViewModel.Entity.Properties);
            Search();
        }

        //public void SaveSearch()
        //{
        //    AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
        //    appUserItemFolderViewModel.Entity.FolderName = SearchEntity.SearchTitle;
        //    appUserItemFolderViewModel.Entity.Description = SearchEntity.SearchDescription;
        //    appUserItemFolderViewModel.Entity.Category = "";
        //    appUserItemFolderViewModel.Entity.FolderType = "DYNAMIC";
        //    appUserItemFolderViewModel.Entity.DataType = TableName;
        //    appUserItemFolderViewModel.Entity.CreatedByCooperatorID = AuthenticatedUserCooperatorID;
        //    appUserItemFolderViewModel.Insert();

        //    if (appUserItemFolderViewModel.Entity.ID <= 0)
        //    {
        //        throw new IndexOutOfRangeException("Error adding new folder.");
        //    }

        //    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
        //    appUserItemListViewModel.Entity.AppUserItemFolderID = appUserItemFolderViewModel.Entity.ID;
        //    appUserItemListViewModel.Entity.CooperatorID = AuthenticatedUserCooperatorID;
        //    appUserItemListViewModel.Entity.TabName = "GGTools Taxon Editor";
        //    appUserItemListViewModel.Entity.ListName = appUserItemFolderViewModel.Entity.FolderName;
        //    appUserItemListViewModel.Entity.IDNumber = 0;
        //    appUserItemListViewModel.Entity.IDType = "FOLDER";
        //    appUserItemListViewModel.Entity.SortOrder = 0;
        //    appUserItemListViewModel.Entity.Title = appUserItemFolderViewModel.Entity.FolderName;
        //    appUserItemListViewModel.Entity.Description = "Added in GGTools Taxonomy Editor";
        //    appUserItemListViewModel.Entity.CreatedByCooperatorID = AuthenticatedUserCooperatorID;
        //    appUserItemListViewModel.Entity.Properties = SerializeToXml(SearchEntity);
        //    appUserItemListViewModel.Insert();
        //}
        
        public void GetFolderItems()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollection = new Collection<Species>(mgr.GetFolderItems(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void SearchProtologues(string protologue)
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollectionProtologues = new Collection<CodeValue>(mgr.SearchProtologues(protologue));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void SearchProtologueVirtualPaths(string protologueVirtualPath)
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollectionProtologues = new Collection<CodeValue>(mgr.SearchProtologues(protologueVirtualPath));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public int Update()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    Entity.IsSpecificHybrid = FromBool(Entity.IsSpecificHybridOption);
                    Entity.IsSubspecificHybrid = FromBool(Entity.IsSubSpecificHybridOption);
                    Entity.IsVarietalHybrid = FromBool(Entity.IsVarietalHybridOption);
                    Entity.IsSubVarietalHybrid = FromBool(Entity.IsSubvarietalHybridOption);
                    Entity.IsFormaHybrid = FromBool(Entity.IsFormaHybridOption);
                    Entity.IsWebVisible = FromBool(Entity.IsWebVisibleOption);

                    SetSpeciesName();
                    SetSpeciesNameAuthority();
                    HandleAccessions();

                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;
        }

        public void SetVerificationStatus(string statusCode) 
        {
            Entity = Get(Entity.ID);
            switch (statusCode)
            {
                case "Y":
                    Entity.VerifiedByCooperatorID = Entity.CreatedByCooperatorID;
                    Entity.NameVerifiedDate = DateTime.Now;
                    break;
                case "N":
                    Entity.VerifiedByCooperatorID = 0;
                    Entity.NameVerifiedDate = DateTime.MinValue;
                    break;
            }
            Update();
            Get(Entity.ID);
        }
        
        public override bool Validate()
        {
            bool validated = true;

            Entity.IsSpecificHybrid = FromBool(Entity.IsSpecificHybridOption);
            Entity.IsWebVisible = FromBool(Entity.IsWebVisibleOption);

            // Verify that species epithet has been provided.
            switch (Entity.Rank.ToUpper())
                {
                    case "SPECIES":
                        if (String.IsNullOrEmpty(Entity.SpeciesName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The species epithet is required." });
                        }
                        break;
                    case "SUBSPECIES":
                        if (String.IsNullOrEmpty(Entity.SubspeciesName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The subspecies epithet is required." });
                        }
                        break;
                    case "VARIETY":
                        if (String.IsNullOrEmpty(Entity.VarietyName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The variety epithet is required." });
                        }
                        break;
                    case "SUBVARIETY":
                        if (String.IsNullOrEmpty(Entity.SubvarietyName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The subvariety epithet is required." });
                        }
                        break;
                    case "FORM":
                        if (String.IsNullOrEmpty(Entity.FormaName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The forma epithet is required." });
                        }
                        if (String.IsNullOrEmpty(Entity.FormaRankType))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The forma rank type is required." });
                        }
                        break;
                }

            // Verify that genus has been provided.
            if (Entity.GenusID == 0)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The genus is required." });
            }

            // Verify that a synonym code has been supplied if an accepted name has been
            // identified.
            if (Entity.IsAcceptedName == "N")
            {
                if (Entity.AcceptedID == 0)
                {
                    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must select an accepted species." });
                }

                if (String.IsNullOrEmpty(Entity.SynonymCode))
                {
                    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must select a synonym code." });
                }
            }

            // Verify that author name(s) exist in author table.
            if (!String.IsNullOrEmpty(GetAuthority()))
            {
                if (!String.IsNullOrEmpty(ValidateAuthority()))
                {
                    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = String.Format("The author {0} does not exist in the Author table.", Entity.SpeciesAuthority) }); ;
                }
            }

            SetSpeciesName();
            SetSpeciesNameAuthority();

            // Ensure that species does not validate uniqueness constraint.
            SpeciesViewModel speciesValidationViewModel = new SpeciesViewModel();
            speciesValidationViewModel.SearchEntity.GenusID = Entity.GenusID;
            speciesValidationViewModel.SearchEntity.Name = Entity.Name;
            speciesValidationViewModel.SearchEntity.NameAuthority= Entity.NameAuthority;
            speciesValidationViewModel.SearchEntity.Protologue = Entity.Protologue;
            speciesValidationViewModel.SearchEntity.SynonymCode = Entity.SynonymCode;
            speciesValidationViewModel.SearchEntity.ExcludeID = Entity.ID;
            speciesValidationViewModel.Search();

            if (speciesValidationViewModel.DataCollection.Count > 0)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = String.Format("The species that you've attempted to create currently exists.", Entity.SpeciesAuthority) }); ;
            }


            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }

        private string GetAuthority()
        {
            switch (Entity.Rank)
            {
                case "SPECIES":
                    return Entity.SpeciesAuthority;
                case "SUBSPECIES":
                    return Entity.SubspeciesAuthority;
                case "FORMA":
                    return Entity.FormaAuthority;
                default:
                    return Entity.SpeciesAuthority;
            }
        }

        /// <summary>
        /// Parses the author string and verifies that each author listed exists in the author table.
        /// </summary>
        /// <returns></returns>
        
        public string ValidateAuthority()
        {
            string authority = GetAuthority();

            authority = Regex.Replace(authority, @"\(|\)|,|\?| ex | non |sensu | et al\.", "&");
            string[] authorityList = authority.Split('&');

            if (authorityList.Length > 0)
            {
                foreach (string auth in authorityList)
                {
                    string authClean = auth.Trim();
                    if (!String.IsNullOrEmpty(authClean))
                    {
                        // TODO: Look up author

                        AuthorViewModel authorViewModel = new AuthorViewModel();
                        authorViewModel.SearchEntity.ShortName = authClean;
                        authorViewModel.SearchEntity.IsShortNameExactMatch = "Y";
                        authorViewModel.Search();
                        if (authorViewModel.DataCollection.Count == 0)
                        {
                            return authClean;
                        }
                    }
                }
            }
            return "";
        }
        
        /// <summary>
        /// If accepted status changes:
        /// 1. Look for related accessions
        /// 2. Add annotations for each
        /// 3. Point each to new accepted name
        /// 4. Generate/send email to each accession owner
        /// </summary>
        /// <returns></returns>
        
        public int HandleAccessions()
        {
            int retVal = 0;
            List<string> accessionOwnerEmailAddresses = new List<string>();

            try
            {
                if ((Entity.ID > 0) && (Entity.AccessionCount > 0))
                {
                    if (Entity.ID != Entity.AcceptedID)
                    {
                        // Get all accessions linked to the new species name.
                        AccessionViewModel accessionViewModel = new AccessionViewModel();
                        accessionViewModel.SearchEntity.SpeciesID = Entity.ID;
                        accessionViewModel.Search();
                        if (accessionViewModel.DataCollection.Count > 0)
                        {
                            // Add annotation record(s)
                            AccessionInvAnnotationViewModel accessionInvAnnotationViewModel = new AccessionInvAnnotationViewModel();
                            accessionInvAnnotationViewModel.Entity.SpeciesID = Entity.AcceptedID;
                            accessionInvAnnotationViewModel.Entity.OldSpeciesID = Entity.ID;
                            accessionInvAnnotationViewModel.Entity.ModifiedByCooperatorID = Entity.ModifiedByCooperatorID;
                            accessionInvAnnotationViewModel.Insert();

                            // TODO
                            // Re-assign accessions to accepted name
                            AccessionViewModel accessionViewModel1 = new AccessionViewModel();
                            accessionViewModel1.Entity.SpeciesID = Entity.ID;
                            accessionViewModel1.Entity.NewSpeciesID = Entity.AcceptedID;
                            accessionViewModel1.Entity.ModifiedByCooperatorID = Entity.ModifiedByCooperatorID;
                            accessionViewModel1.UpdateBySpecies();

                            //TODO Get email list
                            foreach (var accession in accessionViewModel.DataCollection)
                            {
                                if (FindString(accessionOwnerEmailAddresses, accession.OwnedByCooperatorEmailAddress) == -1)
                                {
                                    accessionOwnerEmailAddresses.Add(accession.OwnedByCooperatorEmailAddress);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        static int FindString(List<string> stringList, string searchString)
        {
            // Loop through each string in the list
            for (int i = 0; i < stringList.Count; i++)
            {
                // Check if the current string matches the search string
                if (stringList[i] == searchString)
                {
                    // Return the index where the string was found
                    return i;
                }
            }

            // Return -1 if the string was not found
            return -1;
        }

        public void SetSpeciesName()
        {
            GenusViewModel genusViewModel = new GenusViewModel();
            genusViewModel.Get(Entity.GenusID);
            if (genusViewModel.Entity == null)
            {
                throw new Exception("Genus not found.");
            }
            Entity.GenusName = genusViewModel.Entity.Name;

            if (genusViewModel.Entity.HybridCode == "+")
                Entity.GenusName = "+ " + Entity.GenusName;

            if (genusViewModel.Entity.HybridCode == "X")
                Entity.GenusName = "X " + Entity.GenusName;

            // TODO SPEC NAME
            Entity.Name = Entity.GenusName + " " + Entity.SpeciesName;

            if (Entity.IsSpecificHybrid == "Y")
            {
                Entity.Name = "x " + Entity.SpeciesName;
            }
            if (Entity.IsSubspecificHybrid == "Y")
            {
                Entity.Name = "x " + Entity.SubspeciesName;
            }
            if (Entity.IsVarietalHybrid == "Y")
            {
                Entity.Name = "x " + Entity.VarietyName;
            }
            if (Entity.IsSubVarietalHybrid == "Y")
            {
                Entity.Name = "x " + Entity.SubvarietyName;
            }
            if (Entity.IsFormaHybrid == "Y")
            {
                Entity.Name = "x " + Entity.FormaName;
            }

            // Handle infraspecific names.
            if (!String.IsNullOrEmpty(Entity.FormaName))
            {
                Entity.Name += Entity.FormaRankType + " " + Entity.FormaName;
            }
            else
            {
                if (!String.IsNullOrEmpty(Entity.SubvarietyName))
                {
                    Entity.Name += " subvar. " + Entity.SubvarietyName;
                    Entity.NameAuthority = Entity.SubvarietyAuthority;
                }
                else
                {
                    if (!String.IsNullOrEmpty(Entity.VarietyName))
                    {
                        if (Entity.IsVarietalHybrid == "Y")
                        {
                            Entity.Name += " nothovar. " + Entity.VarietyName;
                        }
                        else
                        {
                            Entity.Name += " var. " + Entity.VarietyName;
                        }
                        Entity.NameAuthority = Entity.VarietyAuthority;
                    }
                    else 
                    {
                        if (!String.IsNullOrEmpty(Entity.SubspeciesName))
                        {
                            if (Entity.IsSubspecificHybrid == "Y")
                            {
                                Entity.Name += " nothosubsp. " + Entity.VarietyName;
                            }
                            else
                            {
                                Entity.Name += " subsp. " + Entity.VarietyName;
                            }
                            Entity.NameAuthority = Entity.SubspeciesAuthority;
                        }
                        else
                        {
                            Entity.Name = Entity.GenusName + " " + Entity.SpeciesName;
                        }
                    }
                }
            }



        }
        
        public void SetSpeciesNameAuthority()
        {
            switch (Entity.Rank.ToUpper())
            {
                case "SUBSPECIES":
                    Entity.NameAuthority = Entity.SubspeciesAuthority;
                    break;
                case "VARIETY":
                    Entity.NameAuthority = Entity.VarietyAuthority;
                    break;
                case "SUBVARIETY":
                    Entity.NameAuthority = Entity.FormaAuthority;
                    break;
                case "FORMA":
                    Entity.NameAuthority = Entity.FormaAuthority;
                    break;
                default:
                    Entity.NameAuthority = Entity.SpeciesAuthority;
                    break;
            }
        }
        
        public int CompareNames(string s, string t)
        {

            int n = s.Length; //length of s

            int m = t.Length; //length of t

            int[,] d = new int[n + 1, m + 1]; // matrix

            int cost; // cost

            // Step 1

            if (n == 0) return m;

            if (m == 0) return n;

            // Step 2

            for (int i = 0; i <= n; d[i, 0] = i++) ;

            for (int j = 0; j <= m; d[0, j] = j++) ;

            // Step 3

            for (int i = 1; i <= n; i++)
            {

                //Step 4

                for (int j = 1; j <= m; j++)
                {

                    // Step 5

                    cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);

                    // Step 6

                    d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                              d[i - 1, j - 1] + cost);

                }

            }


            // Step 7


            return d[n, m];

        }

        #region Reports
        //public void GetReportList()
        //{
        //    using (SpeciesManager mgr = new SpeciesManager())
        //    {
        //        DataCollectionReports = new Collection<CodeValue>(mgr.GetReportList());
        //    }
        //}

        //public void GetReport(string reportCode)
        //{
        //    Report.Title = reportCode.Replace("_", " ");

        //    //DEMO ONLY
        //    switch (reportCode.Replace("_",""))
        //    {
        //        case "MissingAutonymForm":
        //            Report.SQL = "SELECT distinct genus_name, species_name " +
        //                        " FROM taxonomy_species ts " +
        //                        " JOIN taxonomy_genus tg ON tg.taxonomy_genus_id = ts.taxonomy_genus_id " +
        //                        " WHERE ts.taxonomy_species_id = ts.current_taxonomy_species_id " +
        //                        " AND forma_name IS NOT NULL " +
        //                        " AND NOT EXISTS(SELECT* FROM taxonomy_species ts2 " +
        //                        " JOIN taxonomy_genus tg2 ON tg2.taxonomy_genus_id = ts2.taxonomy_genus_id " +
        //                        " WHERE genus_name = tg.genus_name AND species_name = ts.species_name AND forma_name = ts.species_name AND taxonomy_species_id = current_taxonomy_species_id) " +
        //                        " ORDER BY genus_name, species_name";

        //            break;
        //        case "MissingBasionym":
        //            Report.SQL = "select taxonomy_species_id, name, name_authority from taxonomy_species ts where ts.name_authority like '%(%)%' AND ts.taxonomy_species_id = ts.current_taxonomy_species_id AND NOT EXISTS(SELECT * FROM taxonomy_species WHERE current_taxonomy_species_id = ts.taxonomy_species_id AND synonym_code = 'B') ORDER BY name";
        //            break;
        //        case "NoSpeciesAuthor":
        //            Report.SQL = "@taxonomy_species.species_authority IS NULL AND ( @taxonomy_species.species_name NOT LIKE  'spp.') AND @taxonomy_species.species_authority IS NULL AND ( @taxonomy_species.species_name NOT LIKE  'hybr.' )";
        //            break;
        //        case "UnverifiedNodulation":
        //            Report.SQL = "select distinct t.name from taxonomy_species t join citation c on t.taxonomy_species_id = c.taxonomy_species_id where t.verifier_cooperator_id is null and c.type_code = 'NODULATION'";
        //            break;
        //    }

        //    using (SpeciesManager mgr = new SpeciesManager())
        //    {
        //        Report.ResultSet = mgr.GetReport(reportCode);
        //    }
        //}

        #endregion
    }
}
