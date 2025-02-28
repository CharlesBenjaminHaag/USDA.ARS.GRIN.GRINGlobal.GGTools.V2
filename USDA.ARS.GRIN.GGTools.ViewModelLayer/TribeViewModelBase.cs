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
   public class TribeViewModelBase : AppViewModelBase
    {
        private Tribe _Entity = new Tribe();
        private TribeSearch _SearchEntity = new TribeSearch();
        private Collection<Tribe> _DataCollection = new Collection<Tribe>();
        private Collection<FamilyMap> _DataCollectionInfrafamilies = new Collection<FamilyMap>();
        private Collection<Citation> _DataCollectionCitations = new Collection<Citation>();
        private Collection<Genus> _DataCollectionGenera = new Collection<Genus>();
        //private Collection<InfrafamilialTaxon> _DataCollectionInfrafamilialTaxa = new Collection<InfrafamilialTaxon>();
        public TribeViewModelBase()
        {
            TableName = "just_tribe";
            //using (FamilyManager mgr = new FamilyManager())
            //{
            //    Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
            //    InfraFamilies = new SelectList(GetInfraFamilies().Where(x => x.Rank == "SUBFAMILY"), "ID", "FamilyName");
            //    TypeGenera = new SelectList(GetTypeGenera(), "ID", "Name");
            //}
        }

        public Tribe Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public TribeSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<Tribe> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<FamilyMap> DataCollectionInfraFamilies
        {
            get { return _DataCollectionInfrafamilies; }
            set { _DataCollectionInfrafamilies = value; ; }
        }

        //public Collection<InfrafamilialTaxon> DataCollectionInfrafamilialTaxa
        //{
        //    get { return _DataCollectionInfrafamilialTaxa; }
        //    set { _DataCollectionInfrafamilialTaxa = value; }
        //}
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

        //private List<FamilyMap> GetInfraFamilies()
        //{
        //    List<FamilyMap> infraFamilies = new List<FamilyMap>();

        //    ObjectCache cache = MemoryCache.Default;
        //    infraFamilies = cache["DATA-LIST-FAMILY-MAPS"] as List<FamilyMap>;

        //    if (infraFamilies == null)
        //    {
        //        CacheItemPolicy policy = new CacheItemPolicy();
        //        using (FamilyManager mgr = new FamilyManager())
        //        {
        //            infraFamilies = mgr.SearchFamilyMaps(new FamilySearch());
        //        }
        //        cache.Set("DATA-LIST-FAMILY-MAPS", infraFamilies, policy);
        //    }
        //    return infraFamilies;
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
