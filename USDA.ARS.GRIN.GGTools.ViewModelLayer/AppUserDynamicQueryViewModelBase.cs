using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserDynamicQueryViewModelBase : AppViewModelBase
    {
        private AppUserDynamicQuery _Entity = new AppUserDynamicQuery();
        private AppUserDynamicQuerySearch _SearchEntity = new AppUserDynamicQuerySearch();
        private Collection<AppUserDynamicQuery> _DataCollection = new Collection<AppUserDynamicQuery>();

        public AppUserDynamicQuery Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public AppUserDynamicQuerySearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<AppUserDynamicQuery> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
    }
}
