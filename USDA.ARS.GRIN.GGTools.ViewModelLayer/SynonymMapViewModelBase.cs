using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Caching;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SynonymMapViewModelBase : AppViewModelBase
    {
        private int _Col1Width;
        private int _Col3Width;
        private int _SpeciesID;
        private string _SpeciesName;
        private string _SpeciesIDListSubject;
        private string _SpeciesIDListPredicate;
        private SpeciesSynonymMap _Entity = new SpeciesSynonymMap();
        private SpeciesSynonymMapSearch _SearchEntity = new SpeciesSynonymMapSearch();
        private Collection<Species> _DataCollectionSpeciesSubjects = new Collection<Species>();
        private Collection<Species> _DataCollectionSpeciesPredicates = new Collection<Species>();
        private Collection<SpeciesSynonymMap> _DataCollection = new Collection<SpeciesSynonymMap>();
        private List<SpeciesSynonymMap> _DataCollectionBatch = new List<SpeciesSynonymMap>();

        public SynonymMapViewModelBase()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_species_synonym_map"), "ID", "FullName");
                SynonymCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_SPECIES_QUALIFIER"), "Value", "Title");
            }
        }

        public int Col1Width 
        {
            get { return _Col1Width; }
            set { _Col1Width = value; }
        }
        public int Col3Width
        {
            get { return _Col3Width; }
            set { _Col3Width = value; }
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

        public string SpeciesIDListSubject
        {
            get { return _SpeciesIDListSubject; }
            set { _SpeciesIDListSubject = value; }
        }

        public string SpeciesIDListPredicate
        {
            get { return _SpeciesIDListPredicate; }
            set { _SpeciesIDListPredicate = value; }
        }

        public SpeciesSynonymMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public SpeciesSynonymMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Species> DataCollectionSubject
        {
            get { return _DataCollectionSpeciesSubjects; }
            set { _DataCollectionSpeciesSubjects = value; }
        }
        public Collection<Species> DataCollectionPredicate
        {
            get { return _DataCollectionSpeciesPredicates; }
            set { _DataCollectionSpeciesPredicates = value; }
        }
        public Collection<SpeciesSynonymMap> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public List<SpeciesSynonymMap> DataCollectionBatch { 
            get { return _DataCollectionBatch; }
            set { _DataCollectionBatch = value; }
        }
        public SelectList SynonymCodes { get; set; }
    }
}
