using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserItemListViewModel : AppUserItemListViewModelBase
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public AppUserItemList Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public void GetTabList(int cooperatorId)
        {
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                DataCollectionTabs = new Collection<AppUserItemList>(mgr.GetTabList(cooperatorId));
            }
        }

        public void GetListsByTab(int cooperatorId, string tabName)
        {
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                DataCollectionLists = new Collection<AppEntityRecord>(mgr.GetListsByTab(cooperatorId, tabName));
            }
        }

        public void GetItemsByList(int cooperatorId, string listName)
        {
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                DataCollection = new Collection<AppUserItemList>(mgr.GetItemsByList(cooperatorId, listName));
            }
        }

        public void GetSysTablesByAppUserItemFolder(int appUserItemFolderId)
        {
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
               DataCollectionSysTables = new Collection<SysTable>(mgr.GetSysTablesByAppUserItemFolder(appUserItemFolderId));
            }
        }

        public void GetDynamic()
        {
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                DataCollection = new Collection<AppUserItemList>(mgr.Search(SearchEntity));
                if (DataCollection.Count == 1)
                {
                    Entity = DataCollection[0];
                }
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                mgr.Insert(Entity);
            }
            return Entity.ID;
        }

        public void Search()
        {
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                try
                {
                    DataCollection = new Collection<AppUserItemList>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
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
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                mgr.Update(Entity);
            }
        }
    }
}
