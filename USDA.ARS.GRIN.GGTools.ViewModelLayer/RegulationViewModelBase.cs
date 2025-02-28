using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
//using Microsoft.Extensions.Caching.Memory;
using System.Runtime.Caching;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class RegulationViewModelBase : AppViewModelBase
    {
        private Regulation _Entity = new Regulation();
        private RegulationSearch _SearchEntity = new RegulationSearch();
        private Collection<Regulation> _DataCollection = new Collection<Regulation>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();

        public RegulationViewModelBase()
        {
            TableName = "taxonomy_regulation";
            using (RegulationManager mgr = new RegulationManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                RegulationTypeCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_NOXIOUS_TYPE"), "Value", "Title");
                RegulationLevelCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_NOXIOUS_LEVEL"), "Value", "Title");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
            }

            using (GeographyManager geographyManager = new GeographyManager())
            {
                States = new SelectList(geographyManager.GetStates(),"ID","Admin1");
            }
        }

        public Regulation Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public RegulationSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<Regulation> DataCollection
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
        public SelectList RegulationTypeCodes { get; set; }
        public SelectList RegulationLevelCodes { get; set; }
        public SelectList Geographies { get; set; }
        public SelectList Countries { get; set; }
        public SelectList States { get; set; }
        #endregion

        public List<Geography> GetGeographies()
        {
            List<Geography> geographies = new List<Geography>();

            ObjectCache cache = MemoryCache.Default;
            geographies = cache["DATA-LIST-GEOGRAPHIES"] as List<Geography>;

            if (geographies == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (GeographyManager mgr = new GeographyManager())
                {
                    GeographySearch searchEntity = new GeographySearch();
                    geographies = mgr.Search(searchEntity);
                }
                cache.Set("DATA-LIST-GEOGRAPHIES", geographies, policy);
            }
            return geographies;
        }
    }
}
