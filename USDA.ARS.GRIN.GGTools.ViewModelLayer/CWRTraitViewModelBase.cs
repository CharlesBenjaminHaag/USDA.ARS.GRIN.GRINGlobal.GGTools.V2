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
    public class CWRTraitViewModelBase: AppViewModelBase
    {
        private CWRTrait _Entity = new CWRTrait();
        private CWRTraitSearch _SearchEntity = new CWRTraitSearch();
        private Collection<CWRTrait> _DataCollection = new Collection<CWRTrait>();
        private Collection<CWRTrait> _DataCollectionBatch = new Collection<CWRTrait>();
        private Collection<Cooperator> _DataCollectionCooperators = new Collection<Cooperator>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();
        public CWRTraitViewModelBase()
        {
            using (CWRTraitManager mgr = new CWRTraitManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_cwr_trait"), "ID", "FullName");
                TraitClassCodes = new SelectList(mgr.GetCodeValues("CWR_TRAIT_CLASS"), "Value", "Title");
                BreedingTypeCodes = new SelectList(mgr.GetCodeValues("CWR_BREEDING_TYPE"), "Value", "Title");
                IsGraftStockOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                CWRMaps = new SelectList(GetCWRMaps(),"ID","AssembledName");
            }
        }

        public CWRTrait Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public CWRTraitSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<CWRTrait> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<CWRTrait> DataCollectionBatch
        {
            get { return _DataCollectionBatch; }
            set { _DataCollectionBatch = value; }
        }
        public Collection<CodeValue> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }

        private List<CWRMap> GetCWRMaps()
        {
            List<CWRMap> cwrMaps = new List<CWRMap>();

            ObjectCache cache = MemoryCache.Default;
            cwrMaps = cache["DATA-LIST-CWR-MAPS"] as List<CWRMap>;

            if (cwrMaps == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (CWRMapManager mgr = new CWRMapManager())
                {
                    cwrMaps = mgr.Search(new CWRMapSearch());
                }
                cache.Set("DATA-LIST-CWR-MAPS", cwrMaps, policy);
            }
            return cwrMaps;
        }

        #region Select Lists

        public SelectList TraitClassCodes { get; set; }
        public SelectList BreedingTypeCodes { get; set; }
        public SelectList IsGraftStockOptions { get; set; }
        public SelectList CWRMaps { get; set; }
        #endregion
    }
}
