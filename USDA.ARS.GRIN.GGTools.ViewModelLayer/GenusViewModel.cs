using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;
using System.Linq;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class GenusViewModel : GenusViewModelBase, IViewModel<Genus>
    {
        public GenusViewModel()
        { }
        
        public void Delete()
        {
            try
            {
                using (GenusManager mgr = new GenusManager())
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
        
        public Genus Get(int entityId)
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    DataCollection = new Collection<Genus>(mgr.Search(SearchEntity));
                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
                    Entity.IsAccepted = ToBool(Entity.IsAcceptedName);
                    Entity.IsWebVisibleOption = ToBool(Entity.IsWebVisible);

                    if (Entity.Rank != "GENUS")
                    {
                        GetParentGenus();
                    }

                    //TODO
                    // If not accepted, retrieve accepted-fam data.
                    

                        if (Entity.IsAcceptedName == "N")
                    {
                        Genus genusAccepted = new Genus();
                        genusAccepted = mgr.Get(Entity.AcceptedID);
                        Entity.AcceptedName = genusAccepted.AssembledName;
                    }

                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return Entity;
        }
        
        public void GetFolderItems()
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollection = new Collection<Genus>(mgr.GetFolderItems(SearchEntity));
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
        
        public void GetSynonyms(int entityId)
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollectionSynonyms = new Collection<Genus>(mgr.GetSynonyms(entityId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        
        public void GetSubdivisions(int entityId)
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    Genus genus = mgr.Get(entityId);
                    DataCollectionSubdivisions = new Collection<Genus>(mgr.GetSubdivisions(genus.Name));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        
        public void GetParentGenus()
        {
            using (GenusManager mgr = new GenusManager())
            {
                TopRankGenusEntity = mgr.GetGenus(Entity.Name);
                Entity.ParentID = TopRankGenusEntity.ID;
                Entity.ParentName = TopRankGenusEntity.Name;
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            try
            {
                using (GenusManager mgr = new GenusManager())
                {
                    Entity.ID = mgr.Insert(Entity);
                }

                if (IsTypeGenus == "Y")
                {
                    using (FamilyMapManager familyMapManager = new FamilyMapManager())
                    {
                        FamilyMapSearch familyMapSearch = new FamilyMapSearch { ID = Entity.FamilyID };
                        FamilyMap familyMap = new FamilyMap();
                        familyMap = familyMapManager.Search(familyMapSearch).FirstOrDefault();

                        familyMap.TypeGenusID = Entity.ID;
                        familyMapManager.Update(familyMap);
                    }
                }
                return RowsAffected;
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public void Search()
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollection = new Collection<Genus>(mgr.Search(SearchEntity));
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
        public void RunSearch(int appUserItemFolderId)
        {
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            appUserItemListViewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
            appUserItemListViewModel.Search();
            SearchEntity = Deserialize<GenusSearch>(appUserItemListViewModel.Entity.Properties);
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
        public void SearchFolderItems()
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollection = new Collection<Genus>(mgr.SearchFolderItems(SearchEntity));
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

        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (GenusManager mgr = new GenusManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public int Update()
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public string GetPageTitle()
        {
            string pageTitle = String.Empty;
            switch (Entity.Rank.ToUpper())
            {
                case "GENUS":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.Name);
                    break;
                case "SUBGENUS":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SubgenusName);
                    break;
                case "SECTION":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SectionName);
                    break;
                case "SUBSECTION":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SubsectionName);
                    break;
                case "SERIES":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SeriesName);
                    break;
                case "SUBSERIES":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SubseriesName);
                    break;
                default:
                    pageTitle = String.Format("Edit Genus [{0}]: {1}", Entity.ID, Entity.Name);
                    break;
            }
            //DEBUG
            pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.AssembledName);

            return pageTitle;
        }
    }
}
