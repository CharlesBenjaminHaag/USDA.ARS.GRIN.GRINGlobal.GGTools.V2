using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class RegulationMapViewModel : RegulationMapViewModelBase, IViewModel<RegulationMap> 
    {
        public RegulationMapViewModel()
        {
            this.TableName = "taxonomy_regulation_map";
            this.TableCode = "RegulationMap";
            //TODO SELECT LISTS
        }

        public void Delete()
        {
            try
            {
                using (RegulationMapManager mgr = new RegulationMapManager())
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

        public RegulationMap Get(int entityId)
        {
            try
            {
                using (RegulationMapManager mgr = new RegulationMapManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
                        if (RowsAffected == 1)
                        {
                            Entity = DataCollection[0];
                            Entity.IsExemptOption = Entity.IsExempt == "Y" ? true : false;
                        }    
                    }
                    catch (Exception ex)
                    {
                        PublishException(ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity;
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }
        public void Insert()
        {
            using (RegulationMapManager mgr = new RegulationMapManager())
            {
                if (!String.IsNullOrEmpty(Entity.ItemIDList))
                {
                    string[] itemIdList = Entity.ItemIDList.Split(',');
                    foreach (var itemId in itemIdList)
                    {
                        RegulationMap regulationMap = new RegulationMap();
                        regulationMap.SpeciesID = Entity.SpeciesID;
                        regulationMap.RegulationID = Int32.Parse(itemId);
                        regulationMap.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                        mgr.Insert(regulationMap);
                    }
                }
                else
                {
                    RowsAffected = mgr.Insert(Entity);
                }
            }
        }

        public void Map()
        {
            var itemIdList = ItemIDList.Split(',');

            using (RegulationMapManager mgr = new RegulationMapManager())
            {
                foreach (var id in itemIdList)
                {
                    //TODO Determine which taxon to map.
                    RegulationMap regulationMap = new RegulationMap { ID = Entity.ID, SpeciesID = Int32.Parse(id) };
                    mgr.Insert(regulationMap);
                }
            }
        }

        public void Update()
        {
            using (RegulationMapManager mgr = new RegulationMapManager())
            {
                try
                {
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }
        [HttpPost]
        public void Search()
        {
            using (RegulationMapManager mgr = new RegulationMapManager())
            {
                try
                {
                    DataCollection = new Collection<RegulationMap>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
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
            SearchEntity = Deserialize<RegulationMapSearch>(appUserItemListViewModel.Entity.Properties);
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
            using (RegulationMapManager mgr = new RegulationMapManager())
            {
                try
                {
                    DataCollection = new Collection<RegulationMap>(mgr.GetFolderItems(SearchEntity));
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
            using (CWRMapManager mgr = new CWRMapManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        int IViewModel<RegulationMap>.Insert()
        {
            throw new NotImplementedException();
        }

        public List<RegulationMap> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        int IViewModel<RegulationMap>.Update()
        {
            throw new NotImplementedException();
        }
    }
}
