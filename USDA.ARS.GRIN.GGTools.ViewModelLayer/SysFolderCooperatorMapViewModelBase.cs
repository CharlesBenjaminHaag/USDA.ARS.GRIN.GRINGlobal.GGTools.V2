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
    public class SysFolderCooperatorMapViewModelBase: AuthenticatedViewModelBase
    {
        private SysFolderCooperatorMap _Entity = new SysFolderCooperatorMap();
        private SysFolderCooperatorMapSearch _SearchEntity = new SysFolderCooperatorMapSearch();
        private Collection<Cooperator> _DataCollectionMapped = new Collection<Cooperator>();
        private Collection<Cooperator> _DataCollectionNonMapped = new Collection<Cooperator>();
     
        public SysFolderCooperatorMapViewModelBase()
        {
           
        }

        public SysFolderCooperatorMapViewModelBase(int cooperatorId)
        {
            
        }

        public SysFolderCooperatorMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysFolderCooperatorMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Cooperator> DataCollectionMapped
        {
            get { return _DataCollectionMapped; }
            set { _DataCollectionMapped = value; }
        }

        public Collection<Cooperator> DataCollectionNonMapped
        {
            get { return _DataCollectionNonMapped; }
            set { _DataCollectionNonMapped = value; }
        }
    }
}
