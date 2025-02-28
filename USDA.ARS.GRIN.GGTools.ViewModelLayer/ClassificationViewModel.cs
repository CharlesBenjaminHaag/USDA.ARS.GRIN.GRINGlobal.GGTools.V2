using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class ClassificationViewModel : ClassificationViewModelBase, IViewModel<Classification>
    {
        public void Delete()
        {
            try
            {
                using (ClassificationManager mgr = new ClassificationManager())
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

        public Classification Get(int entityId)
        {
            using (ClassificationManager mgr = new ClassificationManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    Entity = new Collection<Classification>(mgr.Search(SearchEntity))[0];
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }

            //using (FamilyManager familyManager = new FamilyManager())
            //{
            //    FamilySearch familySearch = new FamilySearch();
            //    familySearch.OrderID = entityId;
            //    //DataCollectionFamilies = new Collection<Family>(familyManager.Search(familySearch));
            //}
            return Entity;
        }

        public void GetFolderItems()
        {
            using (ClassificationManager mgr = new ClassificationManager())
            {
                try
                {
                    DataCollection = new Collection<Classification>(mgr.GetFolderItems(SearchEntity));
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

        public int Insert()
        {
            using (ClassificationManager mgr = new ClassificationManager())
            {
                try
                {
                    return mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void Search()
        {
            using (ClassificationManager mgr = new ClassificationManager())
            {
                try
                {
                    DataCollection = new Collection<Classification>(mgr.Search(SearchEntity));
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
            SearchEntity = Deserialize<ClassificationSearch>(appUserItemListViewModel.Entity.Properties);
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
            using (ClassificationManager mgr = new ClassificationManager())
            {
                try
                {
                    return mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    return -1;
                }
            }
        }

        //public int MapFamilies()
        //{
        //    string[] idCollection;
        //    using (FamilyManager mgr = new FamilyManager())
        //    {
        //        try
        //        {
        //            idCollection = ItemIDList.Split(',');
        //            foreach (var id in idCollection)
        //            {
        //                Family family = new Family { ID = Int32.Parse(id), OrderID = Entity.ID, ModifiedByCooperatorID = Entity.ModifiedByCooperatorID };
        //                // TODO? CBH 3/2/22 mgr.UpdateClassification(family);
        //            }
        //            return 0;
        //        }
        //        catch (Exception ex)
        //        {
        //            PublishException(ex);
        //            return -1;
        //        }
        //    }
        //}

        public void SearchFolderItems()
        {
            using (ClassificationManager mgr = new ClassificationManager())
            {
                try
                {
                    DataCollection = new Collection<Classification>(mgr.SearchFolderItems(SearchEntity));
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
            using (ClassificationManager mgr = new ClassificationManager())
            {
//                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public List<Classification> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
