using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
   public class SubtribeViewModelBase : AppViewModelBase
    {
        private Subtribe _Entity = new Subtribe();
        private SubtribeSearch _SearchEntity = new SubtribeSearch();
        private Collection<Subtribe> _DataCollection = new Collection<Subtribe>();
        private Collection<FamilyMap> _DataCollectionFamilyMaps = new Collection<FamilyMap>();
        private Collection<Citation> _DataCollectionCitations = new Collection<Citation>();
        private Collection<Genus> _DataCollectionGenera = new Collection<Genus>();
        //private Collection<InfrafamilialTaxon> _DataCollectionInfrafamilialTaxa = new Collection<InfrafamilialTaxon>();
        public SubtribeViewModelBase()
        {
            //using (FamilyManager mgr = new FamilyManager())
            //{
            //    Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
            //    InfraFamilies = new SelectList(GetFamilyMaps().Where(x => x.Rank == "SUBFAMILY"), "ID", "FamilyName");
            //    TypeGenera = new SelectList(GetTypeGenera(), "ID", "Name");
            //}
        }

        public Subtribe Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public SubtribeSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<Subtribe> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<FamilyMap> DataCollectionFamilyMaps
        {
            get { return _DataCollectionFamilyMaps; }
            set { _DataCollectionFamilyMaps = value; ; }
        }

        public Collection<Citation> DataCollectionCitations
        {
            get { return _DataCollectionCitations; }
            set { _DataCollectionCitations = value; }
        }
        public Collection<Genus> DataCollectionGenera
        {
            get { return _DataCollectionGenera; }
            set { _DataCollectionGenera = value; }
        }

        //private List<FamilyMap> GetFamilyMaps()
        //{
        //    List<FamilyMap> familyMaps = new List<FamilyMap>();

        //    ObjectCache cache = MemoryCache.Default;
        //    familyMaps = cache["DATA-LIST-FAMILY-MAPS"] as List<FamilyMap>;

        //    if (familyMaps == null)
        //    {
        //        CacheItemPolicy policy = new CacheItemPolicy();
        //        using (FamilyManager mgr = new FamilyManager())
        //        {
        //            familyMaps = mgr.SearchFamilyMaps(new FamilySearch());
        //        }
        //        cache.Set("DATA-LIST-FAMILY-MAPS", familyMaps, policy);
        //    }
        //    return familyMaps;
        //}

        //public Collection<InfrafamilialTaxon> DataCollectionInfrafamilialTaxa
        //{
        //    get { return _DataCollectionInfrafamilialTaxa; }
        //    set { _DataCollectionInfrafamilialTaxa = value; }
        //}
        private List<Genus> GetTypeGenera()
        {
            List<Genus> genera = new List<Genus>();

            ObjectCache cache = MemoryCache.Default;
            genera = cache["DATA-LIST-GENERA"] as List<Genus>;

            if (genera == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (GenusManager mgr = new GenusManager())
                {
                    GenusSearch genusSearchEntity = new GenusSearch { Rank = "Genus" };
                    genera = mgr.Search(genusSearchEntity);
                }
                cache.Set("DATA-LIST-GENERA", genera, policy);
            }

            return genera;
        }
        #region Select Lists 
        public SelectList InfraFamilies { get; set; }
        public SelectList TypeGenera { get; set; }
        #endregion

    }
}
