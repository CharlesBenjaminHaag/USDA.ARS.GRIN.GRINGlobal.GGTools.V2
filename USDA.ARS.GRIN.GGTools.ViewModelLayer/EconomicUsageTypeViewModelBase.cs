using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class EconomicUsageTypeViewModelBase : AppViewModelBase
    {
        private EconomicUsageType _Entity = new EconomicUsageType();
        private EconomicUsageTypeSearch _SearchEntity = new EconomicUsageTypeSearch();
        private Collection<EconomicUsageType> _DataCollection = new Collection<EconomicUsageType>();

        public EconomicUsageTypeViewModelBase()
        {
            using (EconomicUseManager mgr = new EconomicUseManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_economic_usage_type"), "ID", "FullName");
                EconomicUsageCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_USAGE"), "Value", "Title");
            }
        }
     
        public EconomicUsageType Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public EconomicUsageTypeSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<EconomicUsageType> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        
        public SelectList EconomicUsageCodes { get; set; }
        
        public SelectList EconomicUsageTypes { get; set; }
    }
}
