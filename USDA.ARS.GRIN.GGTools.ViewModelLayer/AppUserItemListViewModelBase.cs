using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserItemListViewModelBase : AppViewModelBase
    {
        private AppUserItemList _Entity = new AppUserItemList();
        private AppUserItemListSearch _SearchEntity = new AppUserItemListSearch();
        private Collection<AppUserItemList> _DataCollection = new Collection<AppUserItemList>();
        private Collection<AppUserItemList> _DataCollectionTabs = new Collection<AppUserItemList>();
        private Collection<AppEntityRecord> _DataCollectionLists = new Collection<AppEntityRecord>();
        private Collection<SysTable> _DataCollectionSysTables = new Collection<SysTable>();

        public AppUserItemListViewModelBase()
        {
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("app_user_item_list"), "ID", "FullName");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
            }
        }
        
        public AppUserItemList Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public AppUserItemListSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<AppUserItemList> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<AppUserItemList> DataCollectionTabs
        {
            get { return _DataCollectionTabs; }
            set { _DataCollectionTabs = value; }
        }

        public Collection<AppEntityRecord> DataCollectionLists
        {
            get { return _DataCollectionLists; }
            set { _DataCollectionLists = value; }
        }
        public Collection<SysTable> DataCollectionSysTables
        {
            get { return _DataCollectionSysTables; }
            set { _DataCollectionSysTables = value; }

        }
    }
}
