using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class RegulationMapViewModelBase: AppViewModelBase
    {
        private RegulationMap  _Entity = new RegulationMap();
        private RegulationMapSearch _SearchEntity = new RegulationMapSearch();
        private Collection<RegulationMap> _DataCollection = new Collection<RegulationMap>();
        private Collection<Cooperator> _DataCollectionCooperators = new Collection<Cooperator>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();
        public RegulationMapViewModelBase()
        {
            TableName = "taxonomy_regulation_map";
            using (RegulationMapManager mgr = new RegulationMapManager())
            {
                TableNames = new SelectList(mgr.GetTableNames(), "Key", "Value");
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                RegulationTypeCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_NOXIOUS_TYPE"),"Value","Title");
                RegulationLevelCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_NOXIOUS_LEVEL"), "Value", "Title");
            }

            using (RegulationManager regulationManager = new RegulationManager())
            {
                Regulations = new SelectList(regulationManager.Search(new RegulationSearch()), "ID", "AssembledName");
            }

            using (GeographyManager geographyManager = new GeographyManager())
            {
                States = new SelectList(geographyManager.GetStates(), "ID", "Admin1"); 
            }
        }

        public RegulationMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public RegulationMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<RegulationMap> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<CodeValue> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }


        #region Select Lists
        public SelectList TableNames { get; set; }
        public SelectList Regulations { get; set; }
        public SelectList States { get; set; }
        public SelectList RegulationTypeCodes { get; set; }
        public SelectList RegulationLevelCodes { get; set; }
        #endregion
    }
}
