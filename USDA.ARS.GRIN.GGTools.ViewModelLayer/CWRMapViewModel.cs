using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CWRMapViewModel: CWRMapViewModelBase, IViewModel<CWRMap>
    {
        public override bool Validate()
        {
            bool validated = true;

            Entity.IsCrop = FromBool(Entity.IsCropOption);

            if (Entity.CropForCWRID == 0)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please select a crop." });
            }

            if (Entity.SpeciesID == 0)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please select a species." });
            }

            if ((Entity.IsCrop == "Y") && (String.IsNullOrEmpty(Entity.CropCommonName)))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Because you have designated this map as a crop, you must specify a common name." });
            }

            if (ValidationMessages.Count > 0)
                validated = false;

            return validated;
        }
        public void HandleRequest()
        {
           
        }

        public CWRMap Get(int entityId)
        {
            try
            {
                using (CWRMapManager mgr = new CWRMapManager())
                {
                    try
                    {
                       Entity = mgr.Get(entityId);
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
        public void GetFolderItems()
        {
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    DataCollection = new Collection<CWRMap>(mgr.GetFolderItems(SearchEntity));
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
        public int Search()
        {
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    DataCollection = new Collection<CWRMap>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                        Entity.IsCropOption = ToBool(Entity.IsCrop);
                        Entity.IsGraftStockOption = ToBool(Entity.IsGraftstock);
                        Entity.IsPotentialOption = ToBool(Entity.IsPotential);
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return 0;
            }
        }
        public void RunSearch(int appUserItemFolderId)
        {
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            appUserItemListViewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
            appUserItemListViewModel.Search();
            SearchEntity = Deserialize<CWRMapSearch>(appUserItemListViewModel.Entity.Properties);
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
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    DataCollection = new Collection<CWRMap>(mgr.SearchFolderItems(SearchEntity));
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

        public void Save()
        {
            if (Entity.ID > 0)
            {
                Update();
            }
            else
            {
                Insert();
            }
        }

        public void InsertBatch()
        {
            string[] itemIDList = Entity.ItemIDList.Split(',');
            foreach (var itemId in itemIDList)
            {
                CWRMap cWRMap = new CWRMap();
                cWRMap.CropForCWRID = Entity.CropForCWRID;
                cWRMap.SpeciesID = Int32.Parse(itemId);
                cWRMap.GenepoolCode = Entity.GenepoolCode;
                cWRMap.CropCommonName = Entity.CropCommonName;
                cWRMap.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                Entity = cWRMap;
                Insert();
            }
        }
        public List<CWRMap> InsertMultiple()
        {
            int cwrMapId = 0;
            string[] speciesIdList = SpeciesIDList.Split(',');
            string[] cropForCwrIdList = CropForCWRIDList.Split(',');
            List<CWRMap> cWRMaps = new List<CWRMap>();

            using (CWRMapManager mgr = new CWRMapManager())
            {
                foreach (var speciesId in speciesIdList)
                {
                    foreach (var cropForCwrId in cropForCwrIdList)
                    {
                        CWRMap cWRMap = new CWRMap();
                        cWRMap.SpeciesID = Int32.Parse(speciesId);
                        cWRMap.CropForCWRID = Int32.Parse(cropForCwrId);
                        cWRMap.GenepoolCode = Entity.GenepoolCode;
                        cWRMap.CropCommonName = Entity.CropCommonName;
                        cWRMap.IsCrop = Entity.IsCrop;
                        cWRMap.IsGraftstock = Entity.IsGraftstock;
                        cWRMap.IsPotential = Entity.IsPotential;
                        cWRMap.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                        cwrMapId = mgr.Insert(cWRMap);

                        // Add new syn map record to a list of recs created in the current session.
                        // Note that, given that the sproc only inserts records if the taxon A/syn code/taxon B
                        // combination does not already exist, the result of the insert in those instances will
                        // be -1, vs. the new synonym map ID.
                        if (cwrMapId > 0)
                        {
                            CWRMap cwrMapBatch = mgr.Get(cwrMapId);
                            cWRMaps.Add(cwrMapBatch);
                        }
                    }
                }
            }
            DataCollection = new Collection<CWRMap>(cWRMaps);
            return cWRMaps;
        }
        public void Insert()
        {
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    RowsAffected = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }

        public void Update()
        {
            using (CWRMapManager mgr = new CWRMapManager())
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

        void IViewModel<CWRMap>.Search()
        {
            throw new NotImplementedException();
        }

        int IViewModel<CWRMap>.Insert()
        {
            throw new NotImplementedException();
        }

        int IViewModel<CWRMap>.Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            try
            {
                using (CWRMapManager mgr = new CWRMapManager())
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

        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (CWRMapManager mgr = new CWRMapManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public List<CWRMap> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
