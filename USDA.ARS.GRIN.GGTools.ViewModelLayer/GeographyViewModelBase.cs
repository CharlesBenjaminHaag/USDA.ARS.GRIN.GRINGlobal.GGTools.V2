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
    public class GeographyViewModelBase: AppViewModelBase
    {
        // REFACTOR. Bad idea to couple geo to species? May be valid; only use of
        // geo is to map species. -- CBH, 3/6/23
        private int _SpeciesID;
        private string _SpeciesName;
        private string _IsLookupFormat;

        private Geography _Entity = new Geography();
        private Geography _ParentEntity = new Geography();
        private GeographySearch _SearchEntity = new GeographySearch();
        private Collection<Geography> _DataCollection = new Collection<Geography>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();
        private Collection<Region> _DataCollectionContinents = new Collection<Region>();
        private Collection<Region> _DataCollectionSubContinents = new Collection<Region>();
        private Collection<Country> _DataCollectionCountries = new Collection<Country>();
        private Collection<State> _DataCollectionStates = new Collection<State>();
        private Collection<CodeValue> _DataCollectionMapStatuses = new Collection<CodeValue>();

        public GeographyViewModelBase()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("geography"), "ID", "FullName");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                Admin1Types = new SelectList(mgr.GetCodeValues("GEOGRAPHY_ADMIN1_TYPE"), "Value", "Title");
                Admin2Types = new SelectList(mgr.GetCodeValues("GEOGRAPHY_ADMIN2_TYPE"), "Value", "Title");
                //Regions = new SelectList(mgr.GetRegions(), "ID", "RegionText");
                DataCollectionContinents = new Collection<Region>(mgr.GetContinents());
                DataCollectionSubContinents = new Collection<Region>(mgr.GetSubContinents());
                DataCollectionCountries = new Collection<Country>(mgr.GetCountries());
                //Countries = new SelectList(mgr.GetCountries(), "CountryCode", "CountryDescription");
            }
        }
        public int SpeciesID
        {
            get { return _SpeciesID; }
            set { _SpeciesID = value; }
        }
        public string SpeciesName 
        { 
            get { return _SpeciesName; }
            set { _SpeciesName = value; } 
        }
        public string IsLookupFormat
        {
            get { return _IsLookupFormat; }
            set { _IsLookupFormat = value; }
        }
        public Geography Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public Geography ParentEntity
        {
            get { return _ParentEntity; }
            set { _ParentEntity = value; }
        }

        public GeographySearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Geography> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
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
        public Collection<State> DataCollectionStates
        {
            get { return _DataCollectionStates; }
            set { _DataCollectionStates = value; }
        }

        public Collection<CodeValue> DataCollectionGeographyStatuses 
        {
            get { return _DataCollectionMapStatuses; }
            set { _DataCollectionMapStatuses = value; }
        }

        public SelectList Regions { get; set; }
        //public SelectList SubContinents { get; set; }

        public SelectList Countries { get; set; }
        public SelectList States { get; set; }
        public SelectList Admin1Types { get; set; }
        public SelectList Admin2Types { get; set; }

    }
}
