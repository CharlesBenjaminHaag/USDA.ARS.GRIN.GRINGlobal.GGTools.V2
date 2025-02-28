using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer.UPOV;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class UPOVViewModelBase : AuthenticatedViewModelBase
    {
        private string _OriginalShortName = String.Empty;
        private upovCodeItem _Entity = new upovCodeItem();
        private upovCodeItemSearch _SearchEntity = new upovCodeItemSearch();
        private Collection<UPOVEncodedSpecies> _DataCollection = new Collection<UPOVEncodedSpecies>();
        private Collection<upovCodeItem> _DataCollectionImport = new Collection<upovCodeItem>();

        public upovCodeItem Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public upovCodeItemSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<UPOVEncodedSpecies> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<upovCodeItem> DataCollectionImport
        {
            get { return _DataCollectionImport; }
            set { _DataCollectionImport = value; }
        }
    }
}
