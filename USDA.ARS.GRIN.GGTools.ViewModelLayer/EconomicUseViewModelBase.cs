using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class EconomicUseViewModelBase : AppViewModelBase
    {
        private string _SpeciesIDList;
        private EconomicUse _Entity = new EconomicUse();
        private EconomicUseSearch _SearchEntity = new EconomicUseSearch();
        private EconomicUsageTypeSearch _EconomicUsageTypeSearchEntity = new EconomicUsageTypeSearch();
        private Collection<EconomicUse> _DataCollection = new Collection<EconomicUse>();
        private Collection<EconomicUsageType> _DataCollectionEconomicUsageTypes = new Collection<EconomicUsageType>();
        private Collection<Citation> _DataCollectionCitations = new Collection<Citation>();

        public EconomicUseViewModelBase()
        {
            TableName = "taxonomy_use";
            TableCode = "Taxonomy Use";
            using (EconomicUseManager mgr = new EconomicUseManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                EconomicUsageCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_USAGE"), "Value", "Title");
                PlantPartCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_PLANT_PART"), "Value", "Title");
                EconomicUsageTypes = new SelectList(mgr.GetEconomicUsageTypes(), "UsageType", "AssembledName");
            }
        }
        public string SpeciesIDList
        {
            get { return _SpeciesIDList; }
            set { _SpeciesIDList = value; }
        }
        public EconomicUseViewModelBase(int speciesId)
        {
            TableName = "taxonomy_use";

            using (EconomicUseManager mgr = new EconomicUseManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                EconomicUsageCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_USAGE"), "Value", "Title");
                PlantPartCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_PLANT_PART"), "Value", "Title");
                //Citations = new SelectList(mgr.GetAvailableCitations(speciesId), "ID", "CitationText");
            }
        }

        public EconomicUse Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public EconomicUseSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public EconomicUsageTypeSearch EconomicUsageTypeSearchEntity
        {
            get { return _EconomicUsageTypeSearchEntity; }
            set { _EconomicUsageTypeSearchEntity = value; }
        }

        public Collection<EconomicUse> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<EconomicUsageType> DataCollectionEconomicUsageTypes
        {
            get { return _DataCollectionEconomicUsageTypes; }
            set { _DataCollectionEconomicUsageTypes = value; }
        }
        public Collection<Citation> DataCollectionCitations 
        {
            get { return _DataCollectionCitations; }
            set { _DataCollectionCitations = value; }
        }

        #region Select Lists
        public SelectList EconomicUsageCodes { get; set; }
        public SelectList EconomicUsageTypes { get; set; }
        public SelectList PlantPartCodes { get; set; }
        public SelectList Citations { get; set; }        
        #endregion
    }
}
