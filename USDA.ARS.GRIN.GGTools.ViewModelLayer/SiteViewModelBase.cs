using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SiteViewModelBase: AuthenticatedViewModelBase
    {
        private Site _Entity = new Site();
        private SiteSearch _SearchEntity = new SiteSearch();
        private Collection<Site> _DataCollection = new Collection<Site>();
        private Collection<Cooperator> _DataCollectionSiteCooperators = new Collection<Cooperator>();
        public SiteViewModelBase()
        {
            using (SiteManager mgr = new SiteManager())
            {
                Types = new SelectList(mgr.GetCodeValues("SITE_TYPE"), "Value", "Title");
            }

            using (GeographyManager mgr = new GeographyManager())
            {
                States = new SelectList(mgr.GetStates(), "ID", "Admin1");
            }
        }

        public Site Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SiteSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Site> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<Cooperator> DataCollectionSiteCooperators
        {
            get { return _DataCollectionSiteCooperators; }
            set { _DataCollectionSiteCooperators = value; }
        }
        
        public SelectList Types { get; set; }

        public SelectList States { get; set; }

        public string IsReadOnly
        {
            get
            {
                string siteManagementRoleName = "MANAGE_SITE_" + AuthenticatedUser.SiteShortName;

                if((AuthenticatedUser.IsInRole(siteManagementRoleName)) || (AuthenticatedUser.IsInRole("ADMINS")))
                {
                    return "N";
                }
                else
                {
                    return "Y";
                }
            }
        }
    }
}
