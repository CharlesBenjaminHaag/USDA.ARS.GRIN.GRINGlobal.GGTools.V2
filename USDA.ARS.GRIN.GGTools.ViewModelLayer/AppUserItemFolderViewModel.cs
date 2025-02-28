using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserItemFolderViewModel: AppUserItemFolderViewModelBase
    {
        public bool IsFavoriteSelector { get; set; }
        public AppUserItemFolderViewModel()
        { }
        public AppUserItemFolderViewModel(int cooperatorId) : base(cooperatorId)
        {}
        public void Get()
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                try
                {
                    Entity = mgr.Get(SearchEntity);
                    Entity.IsFavoriteOption = ToBool(Entity.IsFavorite);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }
        public void GetRelatedFolders()
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                DataCollection = new Collection<AppUserItemFolder>(mgr.GetRelatedFolders(SearchEntity));
            }
        }
        public void GetDynamicFolders(int cooperatorId, string dataType)
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                SearchEntity.CreatedByCooperatorID = cooperatorId;
                SearchEntity.DataType = dataType;
                DataCollectionDynamicFolders = new Collection<AppUserItemDynamicFolder>(mgr.GetDynamicFolders(SearchEntity));
            }
        }
        // Returns a list of the ID types contained in a given folder.
        public void GetIDTypes(int appItemFolderId)
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                
            }
        }
        public int Search()
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                try
                {
                    DataCollection = new Collection<AppUserItemFolder>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return mgr.RowsAffected;
            }
        }
        public int Insert()
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                try
                {
                    if (!String.IsNullOrEmpty(Entity.NewCategory))
                    {
                        Entity.Category = Entity.NewCategory;
                    }
                    Entity.ID = mgr.Insert(Entity);
                    InsertItems();
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }
        public int InsertItems()
        {
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            // If the update includes to the folder contents as well as its
            // metadata, add them here.
            if (!String.IsNullOrEmpty(EntityIDList))
            {
                foreach (var entityId in EntityIDList.Split(','))
                {
                    appUserItemListViewModel.Entity.AppUserItemFolderID = Entity.ID;
                    appUserItemListViewModel.Entity.CooperatorID = Entity.CreatedByCooperatorID;
                    appUserItemListViewModel.Entity.TabName = "GGTools Taxon Editor";
                    appUserItemListViewModel.Entity.ListName = Entity.FolderName;
                    appUserItemListViewModel.Entity.IDNumber = Int32.Parse(entityId);
                    appUserItemListViewModel.Entity.IDType = Entity.TableName.ToUpper() + "_ID";
                    appUserItemListViewModel.Entity.SortOrder = Int32.Parse(entityId);
                    appUserItemListViewModel.Entity.Title = Entity.FolderName + " " + Entity.TableName.ToUpper();
                    appUserItemListViewModel.Entity.Description = "Added in GGTools Taxonomy Editor";
                    appUserItemListViewModel.Entity.Properties = "";
                    appUserItemListViewModel.Entity.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                    appUserItemListViewModel.Insert();
                }
            }
            return RowsAffected;
        }
        public int Import()
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                try
                {
                    RowsAffected = mgr.Import(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }
        public int Update()
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                try
                {
                    if (!String.IsNullOrEmpty(Entity.NewCategory))
                    {
                        Entity.Category = Entity.NewCategory;
                    }
                    RowsAffected = mgr.Update(Entity);
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }
        public void Delete()
        {
            try
            {
                using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
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
        public void DeleteItemByEntityID(int appUserItemFolderId, int idNumber)
        {
            try
            {
                using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
                {
                    mgr.DeleteItemByEntityID(appUserItemFolderId, idNumber);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }
        public void DeleteItems()
        {
            string[] itemIdList = ItemIDList.Split(',');

            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                foreach (var itemId in itemIdList)
                {
                    var itemIdParsed = itemId.Split('-')[1];
                    mgr.DeleteItem(Int32.Parse(itemIdParsed));
                }
            }
        }
        #region Dynamic Folder

        #endregion
    }
}
