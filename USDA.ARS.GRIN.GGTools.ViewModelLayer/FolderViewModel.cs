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
    public class FolderViewModel: FolderViewModelBase, IViewModel<Folder>
    {
        public string ItemViewName { get; set; }
        public FolderViewModel()
        { }
        public FolderViewModel(int cooperatorId, string tableName) : base(cooperatorId, tableName)
        { 
        
        }

        public void HandleRequest()
        {
        }
        public void Get()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    Entity = mgr.Get(SearchEntity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }
        public int Search()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                { 
                    DataCollection = new Collection<AppUserItemFolder>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }

                    // Get types
                    //List<string> DEBUG = DataCollection.Select(x => x.FolderTypeDescription).Distinct().ToList();
                    //foreach (var type in DEBUG)
                    //{
                    //    DataCollectionFolderTypes.Add(new CodeValue { Value = type, Title = type });
                    //}
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return mgr.RowsAffected;
            }
        }
        public void GetRelatedFolders()
        {
            using (FolderManager mgr = new FolderManager())
            {
                DataCollection = new Collection<AppUserItemFolder>(mgr.GetRelatedFolders(SearchEntity));
            }
        }
        public int GetAvailableFolders(int cooperatorId, string tableName)
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    DataCollectionAvailableFolders = new Collection<AppUserItemFolder>(mgr.GetAvailableFolders(cooperatorId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;
        }
        void IViewModel<Folder>.Search()
        {
            throw new NotImplementedException();
        }
        public AppUserItemFolder Get(int entityId)
        {
            DataCollection = new Collection<AppUserItemFolder>();
            using (FolderManager mgr = new FolderManager())
            {
                SearchEntity.ID = entityId;
                Search();
                if (RowsAffected == 1)
                {
                    Entity = DataCollection[0];
                //    DataCollectionAvailableCooperators = new Collection<Cooperator>(mgr.GetAvailableCollaborators(Entity.ID));
                //    DataCollectionCurrentCooperators = new Collection<Cooperator>(mgr.GetCurrentCollaborators(Entity.ID));
                }
            }
            return Entity;
        }
        public void GetAvailableCooperators()
        {
            using (FolderManager mgr = new FolderManager())
            {
                DataCollectionAvailableCooperators = new Collection<Cooperator>(mgr.GetAvailableCooperators(Entity.ID));
            }
        }
        public void GetCurrentCooperators()
        {
            using (FolderManager mgr = new FolderManager())
            {
                DataCollectionCurrentCooperators = new Collection<Cooperator>(mgr.GetCurrentCollaborators(Entity.ID));
            }
        }
        //public void GetFolderItems(int folderId)
        //{
        //    using (FolderManager mgr = new FolderManager())
        //    {
        //        //TODO Determine which table to load

        //        DataCollectionFolderItems = new Collection<AppUserItemList>(mgr.GetFolderItems(folderId));
        //    }
        //}
        public int Insert()
        {
            using (FolderManager mgr = new FolderManager())
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
                return RowsAffected;
            }
        }
        public int Import()
        {
            using (FolderManager mgr = new FolderManager())
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
        public int InsertFolderItems()
        {
            using (FolderManager mgr = new FolderManager())
            {
                RowsAffected = mgr.InsertItems(Entity);                
            }
            return 0;
        }
        public int InsertCollaborators()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    string[] itemIdArray = ItemIDList.Split(',');
                    foreach (var itemId in itemIdArray)
                    {
                        int cooperatorId = Int32.Parse(itemId);
                        RowsAffected = mgr.InsertCollaborator(cooperatorId, Entity.ID);
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }
        public int DeleteCollaborators()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    string[] itemIdArray = ItemIDList.Split(',');
                    foreach (var itemId in itemIdArray)
                    {
                        int cooperatorId = Int32.Parse(itemId);
                        RowsAffected = mgr.DeleteCollaborator(cooperatorId, Entity.ID);
                    }
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
            using (FolderManager mgr = new FolderManager())
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
        public void Delete()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    RowsAffected = mgr.Delete(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void DeleteItem(int appUserItemListId)
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    RowsAffected = mgr.DeleteItem(appUserItemListId);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void DeleteItems()
        {
            string[] itemIdList = ItemIDList.Split(',');

            using (FolderManager mgr = new FolderManager())
            {
                foreach (var itemId in itemIdList)
                {
                    mgr.DeleteItem(Int32.Parse(itemId));
                }
            }
        }
        Folder IViewModel<Folder>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
