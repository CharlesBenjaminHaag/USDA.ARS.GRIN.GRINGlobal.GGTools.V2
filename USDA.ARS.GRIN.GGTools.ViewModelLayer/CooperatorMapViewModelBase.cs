using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CooperatorMapViewModelBase : AuthenticatedViewModelBase
    {
        private CooperatorMap _Entity = new CooperatorMap();
        private CooperatorMapSearch _SearchEntity = new CooperatorMapSearch();
        private Collection<CooperatorMap> _DataCollection = new Collection<CooperatorMap>();

        public CooperatorMapViewModelBase()
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                Cooperators = new SelectList(mgr.Search(new CooperatorSearch { SysUserIsEnabled = "Y" }), "ID", "FullName");
            }
        }
        public CooperatorMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public CooperatorMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<CooperatorMap> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        
    }
}
