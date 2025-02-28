using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SynonymMapViewModel: SynonymMapViewModelBase, IViewModel<SpeciesSynonymMap>
    {
        public void Delete()
        {
            try
            {
                using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
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

        public SpeciesSynonymMap Get(int entityId)
        {
            DataCollection = new Collection<SpeciesSynonymMap>();
            using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    Search();
                    if (DataCollection.Count == 0)
                    {
                        Entity = DataCollection[0];
                    }
                    return Entity;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }
        public void GetFolderItems()
        {
            using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
            {
                try
                {
                    DataCollection = new Collection<SpeciesSynonymMap>(mgr.GetFolderItems(SearchEntity));
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
        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            SpeciesSynonymMap speciesSynonymMap = new SpeciesSynonymMap();

            try
            {
                using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
                {
                    mgr.Insert(Entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SpeciesSynonymMap> InsertMultiple()
        {
            int speciesSynonymMapId = 0;
            string[] speciesIdListSubject = SpeciesIDListSubject.Split(',');
            string[] speciesIdListPredicate = SpeciesIDListPredicate.Split(',');
            List<SpeciesSynonymMap> synonymMaps = new List<SpeciesSynonymMap>();

            using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
            {
                foreach (var subjectId in speciesIdListSubject)
                {
                    foreach (var predicateId in speciesIdListPredicate)
                    {
                        SpeciesSynonymMap speciesSynonymMap = new SpeciesSynonymMap();
                        speciesSynonymMap.SpeciesAID = Int32.Parse(subjectId);
                        speciesSynonymMap.SynonymCode = Entity.SynonymCode;
                        speciesSynonymMap.SpeciesBID = Int32.Parse(predicateId);
                        speciesSynonymMap.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                        speciesSynonymMapId = mgr.Insert(speciesSynonymMap);

                        // Add new syn map record to a list of recs created in the current session.
                        // Note that, given that the sproc only inserts records if the taxon A/syn code/taxon B
                        // combination does not already exist, the result of the insert in those instances will
                        // be -1, vs. the new synonym map ID.
                        if (speciesSynonymMapId > 0)
                        {
                            SpeciesSynonymMap speciesSynonymMap1 = mgr.Get(speciesSynonymMapId);
                            synonymMaps.Add(speciesSynonymMap1);
                        }
                    }
                }
            }
            DataCollection = new Collection<SpeciesSynonymMap>(synonymMaps);
            return synonymMaps;
        }

        public void Search()
        {
            using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
            {
                try
                {
                    DataCollection = new Collection<SpeciesSynonymMap>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }

                    //String DEBUG = SerializeToXml<CitationSearch>(SearchEntity);
                    //CitationSearch DEBUG2 = Deserialize<CitationSearch>(DEBUG);

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
            SearchEntity = Deserialize<SpeciesSynonymMapSearch>(appUserItemListViewModel.Entity.Properties);
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
        public int Update()
        {
            using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
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
            }
            return RowsAffected;
        }

        int IViewModel<SpeciesSynonymMap>.Insert()
        {
            throw new NotImplementedException();
        }
    }
}
