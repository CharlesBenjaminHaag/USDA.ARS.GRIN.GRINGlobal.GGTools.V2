using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CropForCWRViewModel: CropForCWRViewModelBase, IViewModel<CropForCWRViewModel>
    {
        public CropForCWR Get(int entityId)
        {
            try
            {
                using (CropForCWRManager mgr = new CropForCWRManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
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
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                try
                {
                    DataCollection = new Collection<CropForCWR>(mgr.GetFolderItems(SearchEntity));
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
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                try
                {
                    DataCollection = new Collection<CropForCWR>(mgr.Search(SearchEntity));
                    Entity = DataCollection[0];
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
                return 0;
            }
        }
        public void RunSearch(int appUserItemFolderId)
        {
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            appUserItemListViewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
            appUserItemListViewModel.Search();
            SearchEntity = Deserialize<CropForCWRSearch>(appUserItemListViewModel.Entity.Properties);
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
        public void HandleRequest()
        {
           
        }

        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public void Insert()
        {
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                try
                {
                    RowsAffected = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void Update()
        {
            using (CropForCWRManager mgr = new CropForCWRManager())
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

        public void Delete()
        {
            try
            {
                using (CropForCWRManager mgr = new CropForCWRManager())
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

        void IViewModel<CropForCWRViewModel>.Search()
        {
            throw new NotImplementedException();
        }

        CropForCWRViewModel IViewModel<CropForCWRViewModel>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        int IViewModel<CropForCWRViewModel>.Insert()
        {
            throw new NotImplementedException();
        }

        int IViewModel<CropForCWRViewModel>.Update()
        {
            throw new NotImplementedException();
        }

        public List<CropForCWRViewModel> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
