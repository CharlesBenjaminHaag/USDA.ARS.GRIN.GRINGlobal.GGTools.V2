using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Caching;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class GeographyMapViewModelBase: AppViewModelBase
    {
        private string _GeographyIDList;
        private string _SpeciesIDList;
        private GeographyMap _Entity = new GeographyMap();
        private GeographyMapSearch _SearchEntity = new GeographyMapSearch();
        private Species _SpeciesEntity = new Species();
        private List<GeographyMap> _BatchEditCollection = new List<GeographyMap>();
        private Collection<GeographyMap> _DataCollection = new Collection<GeographyMap>();
        private Collection<Cooperator> _DataCollectionCooperators = new Collection<Cooperator>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();
        private Collection<Geography> _DataCollectionGeographies = new Collection<Geography>();
        private Collection<Region> _DataCollectionContinents = new Collection<Region>();
        private Collection<Region> _DataCollectionSubContinents = new Collection<Region>();
        private Collection<Country> _DataCollectionCountries = new Collection<Country>();
        private Collection<State> _DataCollectionStates = new Collection<State>();
        private Collection<CodeValue> _DataCollectionMapStatuses = new Collection<CodeValue>();
        private List<GeographyMap> _EditCollection = new List<GeographyMap>();
        private List<GeographyMap> _DataCollectionBatch = new List<GeographyMap>();

        private int _CitationID;

        public GeographyMapViewModelBase()
        {
            this.TableCode = "GeographyMap";
            this.TableName = "taxonomy_geography_map";

            //using (GeographyManager geographyManager = new GeographyManager())
            //{
            //    DataCollectionContinents = new Collection<Region>(geographyManager.GetContinents());
            //}
            List<CodeValue> geographyStatusCodes = new List<CodeValue>();

            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_geography_map"), "ID", "FullName");

                geographyStatusCodes = mgr.GetCodeValues("TAXONOMY_GEOGRAPHY_STATUS");
                GeographyStatusCodes = new SelectList(geographyStatusCodes, "Value", "Title");
                GeographyStatusCodesList = geographyStatusCodes;
                
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
            }
        }
        public string GeographyIDList
        {
            get { return _GeographyIDList; }
            set { _GeographyIDList = value; }
        }
        public string SpeciesIDList
        {
            get { return _SpeciesIDList; }
            set { _SpeciesIDList = value; }
        }
        public int CitationID
        {
            get { return _CitationID; }
            set { _CitationID = value; }
        }

        public GeographyMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public GeographyMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Species SpeciesEntity
        {
            get { return _SpeciesEntity; }
            set { _SpeciesEntity = value; }
        }
        public List<GeographyMap> BatchEditDataCollection
        {
            get { return _BatchEditCollection; }
            set { _BatchEditCollection = value; }
        }
        public Collection<GeographyMap> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public List<GeographyMap> DataCollectionBatch
        {
            get { return _DataCollectionBatch; }
            set { _DataCollectionBatch = value; }
        }
        public List<GeographyMap> EditCollection 
        {
            get { return _EditCollection; }
            set { _EditCollection = value; }
        }

        public Collection<CodeValue> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }

        public Collection<Region> DataCollectionContinents
        {
            get { return _DataCollectionContinents; }
            set { _DataCollectionContinents = value; }
        }

        public Collection<Region> DataCollectionSubContinents
        {
            get { return _DataCollectionSubContinents; }
            set { _DataCollectionSubContinents = value; }
        }
        public Collection<Country> DataCollectionCountries
        {
            get { return _DataCollectionCountries; }
            set { _DataCollectionCountries = value; }
        }

        private List<Region> GetRegions()
        {
            List<Region> regions = new List<Region>();

            ObjectCache cache = MemoryCache.Default;
            regions = cache["DATA-LIST-REGIONS"] as List<Region>;

            if (regions == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (GeographyManager mgr = new GeographyManager())
                {
                    //TODO regions = mgr..Search(new FamilyMapSearch());
                }
                cache.Set("DATA-LIST-REGIONS", regions, policy);
            }
            return regions;
        }

        #region Select Lists
        public SelectList TableNames { get; set; }
        public SelectList GeographyStatusCodes { get; set; }
        public IEnumerable<CodeValue> GeographyStatusCodesList { get; set; }
        public SelectList Continents { get; set; }
        public SelectList SubContinents { get; set; }

        public SelectList Countries { get; set; }
        public SelectList States { get; set; }

        public SelectList Citations { get; set; }

        public Collection<SysUserSearchMetadata> SysUserSearchMetadataList;
        #endregion
    }
}
