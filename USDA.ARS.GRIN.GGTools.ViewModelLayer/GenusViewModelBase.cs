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
    public class GenusViewModelBase : AppViewModelBase
    {
        private string _DetailPartialViewName;
        private string _EditPartialViewName;
        private string _ListPartialViewName;
        private string _IsTypeGenus;
        private bool _IsSingleSelect { get; set; }
        private int _ActiveFolderID = 0;
        private Genus _Entity = new Genus();
        private Genus _ParentGenusEntity = new Genus();
        private FamilyMap _FamilyMapEntity = new FamilyMap();
        private GenusSearch _SearchEntity = new GenusSearch();
        private Collection<Genus> _DataCollection = new Collection<Genus>();
        private Collection<Species> _DataCollectionSpecies = new Collection<Species>();
        private Collection<Citation> _DataCollectionCitations = new Collection<Citation>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();
        private Collection<Genus> _DataCollectionSynonyms = new Collection<Genus>();
        private Collection<Genus> _DataCollectionSubdivisions = new Collection<Genus>();
        public GenusViewModelBase()
        {
            TableName = "taxonomy_genus";
            using (GenusManager mgr = new GenusManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                Folders = new SelectList(mgr.GetFolders(TableName), "ID", "Title");
                QualifyingCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_GENUS_QUALIFIER"),"Value","Title");
                HybridCodes = new SelectList(mgr.GetCodeValues("GENUS_HYBRID"), "Value", "Title");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                //Genera = new SelectList(GetGenera().Where(x => x.Rank == "GENUS"));
                //Subgenera = new SelectList(GetGenera().Where(x => x.Rank == "SUBGENUS"));
                //Sections = new SelectList(GetGenera().Where(x => x.Rank == "SECTION"));
                //Subsections = new SelectList(GetGenera().Where(x => x.Rank == "SUBSECTION"));
                //Series = new SelectList(GetGenera().Where(x => x.Rank == "SERIES"));
            }
        }
        public string EditPartialViewName
        {
            get { return _EditPartialViewName; }
            set { _EditPartialViewName = value; }
        }
        public string DetailPartialViewName
        {
            get { return _DetailPartialViewName; }
            set { _DetailPartialViewName = value; }
        }
        public string ListPartialViewName
        {
            get { return _ListPartialViewName; }
            set { _ListPartialViewName = value; }
        }

        public string IsTypeGenus
        {
            get
            { return _IsTypeGenus; }
            set
            { _IsTypeGenus = value; }
        }

        public bool IsSingleSelect
        {
            get
            { return _IsSingleSelect; }
            set
            { _IsSingleSelect = value; }
        }
        public int ActiveFolderID
        {
            get { return _ActiveFolderID; }
            set { _ActiveFolderID = value; }
        }

        public Genus Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public Genus TopRankGenusEntity
        {
            get { return _ParentGenusEntity; }
            set { _ParentGenusEntity = value; }
        }

        public FamilyMap FamilyMapEntity
        {
            get { return _FamilyMapEntity; }
            set { _FamilyMapEntity = value; }
        }

        public GenusSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Genus> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<Species> DataCollectionSpecies
        {
            get { return _DataCollectionSpecies; }
            set { _DataCollectionSpecies = value; }
        }
        public Collection<Citation> DataCollectionCitations
        {
            get { return _DataCollectionCitations; }
            set { _DataCollectionCitations = value; }
        }

        public Collection<CodeValue> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }
        public Collection<Genus> DataCollectionSynonyms
        {
            get { return _DataCollectionSynonyms; }
            set { _DataCollectionSynonyms = value; }
        }
        public Collection<Genus> DataCollectionSubdivisions
        {
            get { return _DataCollectionSubdivisions; }
            set { _DataCollectionSubdivisions = value; }
        }
        private List<Genus> GetGenera()
        {
            List<Genus> genera = new List<Genus>();

            ObjectCache cache = MemoryCache.Default;
            genera = cache["DATA-LIST-GENERA"] as List<Genus>;

            if (genera == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (GenusManager mgr = new GenusManager())
                {
                    genera = mgr.Search(new GenusSearch());
                }
                cache.Set("DATA-LIST-GENERA", genera, policy);
            }
            return genera;
        }

        protected void ResetCache()
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove("DATA-LIST-GENERA");
        }

        #region Select Lists
        
        public SelectList Folders { get; set; }
        public SelectList QualifyingCodes { get; set; }
        public SelectList HybridCodes { get; set; }
        public SelectList Genera { get; set; }
        public SelectList Subgenera { get; set; }
        public SelectList Sections { get; set; }
        public SelectList Subsections { get; set; }
        public SelectList Series { get; set; }
        public SelectList Subseries { get; set; }

        #endregion
    }
}
