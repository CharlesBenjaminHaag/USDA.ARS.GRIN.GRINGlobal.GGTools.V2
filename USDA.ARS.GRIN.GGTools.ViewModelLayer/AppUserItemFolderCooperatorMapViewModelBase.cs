using System;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserItemFolderCooperatorMapViewModelBase : AppViewModelBase
    {
        private AppUserItemFolderCooperatorMap _Entity = new AppUserItemFolderCooperatorMap();
        private AppUserItemFolderCooperatorMapSearch _SearchEntity = new AppUserItemFolderCooperatorMapSearch();
        private Collection<Cooperator> _DataCollectionNonMappedCooperators = new Collection<Cooperator>();
        private Collection<Cooperator> _DataCollectionMappedCooperators = new Collection<Cooperator>();
        public AppUserItemFolderCooperatorMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public AppUserItemFolderCooperatorMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<Cooperator> DataCollectionNonMappedCooperators
        {
            get { return _DataCollectionNonMappedCooperators; }
            set { _DataCollectionNonMappedCooperators = value; }
        }

        public Collection<Cooperator> DataCollectionMappedCooperators
        {
            get { return _DataCollectionMappedCooperators; }
            set { _DataCollectionMappedCooperators = value; }
        }
    }
}
