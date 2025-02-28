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
    public class CWRMapViewModelBase : AppViewModelBase
    {
        private string _CropForCWRIDList;
        private string _SpeciesIDList;
        private CWRMap _Entity = new CWRMap();
        private CWRMapSearch _SearchEntity = new CWRMapSearch();
        private Collection<CWRMap> _DataCollection = new Collection<CWRMap>();
        private Collection<Cooperator> _DataCollectionCooperators = new Collection<Cooperator>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();
        private List<CWRMap> _DataCollectionBatch = new List<CWRMap>();
        public CWRMapViewModelBase()
        {
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                try
                {
                    Cooperators = new SelectList(mgr.GetCooperators("taxonomy_cwr_map"), "ID", "FullName");
                    GenepoolCodes = new SelectList(mgr.GetCodeValues("CWR_GENEPOOL"), "Value", "Title");
                    IsCropOptions = new SelectList(mgr.GetYesNoOptions(),"Key","Value");
                    IsGraftStockOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                    IsPotentialOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                    CropsForCWR = new SelectList(GetCropsForCWR(), "ID", "CropForCWRName");
                    //Species = new SelectList(GetSpecies(), "ID", "SpeciesName");
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }
        public string CropForCWRIDList
        {
            get { return _CropForCWRIDList; }
            set { _CropForCWRIDList = value; }
        }
        public string SpeciesIDList
        {
            get { return _SpeciesIDList; }
            set { _SpeciesIDList = value; }
        }

        public CWRMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public CWRMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<CWRMap> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<CodeValue> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }

        public List<CWRMap> DataCollectionBatch
        {
            get { return _DataCollectionBatch; }
            set { _DataCollectionBatch = value; }
        }
        public SelectList GenepoolCodes { get; set; }
        public SelectList IsCropOptions { get; set; }
        public SelectList IsGraftStockOptions { get; set; }
        public SelectList IsPotentialOptions { get; set; }
        public SelectList CropsForCWR { get; set; }
        public SelectList Species { get; set; }
        private List<CropForCWR> GetCropsForCWR()
        {
            List<CropForCWR> cropForCWRs = new List<CropForCWR>();
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                cropForCWRs = mgr.Search(new CropForCWRSearch());
            }
            //ObjectCache cache = MemoryCache.Default;
            //cropForCWRs = cache["DATA-LIST-CWR-CROPS"] as List<CropForCWR>;

            //if (cropForCWRs == null)
            //{
            //    CacheItemPolicy policy = new CacheItemPolicy();
            //    using (CropForCWRManager mgr = new CropForCWRManager())
            //    {
            //        cropForCWRs = mgr.Search(new CropForCWRSearch());
            //    }
            //    cache.Set("DATA-LIST-CWR-CROPS", cropForCWRs, policy);
            //}
            return cropForCWRs;
        }
    }
}
