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
    public class FamilyViewModelBase: AppViewModelBase
    {
        private bool _IsWebVisibleSelector;
        private Family _Entity = new Family();
        private Family _ParentEntity = new Family();
        private FamilySearch _SearchEntity = new FamilySearch();
        private Collection<Family> _DataCollection = new Collection<Family>();
        private Collection<Family> _DataCollectionSynonyms = new Collection<Family>();
        private Collection<Family> _DataCollectionSubdivisions = new Collection<Family>();
        private Collection<Classification> _DataCollectionClassification = new Collection<Classification>();

        public FamilyViewModelBase()
        {
            using (FamilyManager mgr = new FamilyManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_family"), "ID", "FullName");
                FamilyTypes = new SelectList(GetFamilyTypes(), "Value", "Title");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
            }

            using (ClassificationManager classificationMgr = new ClassificationManager())
            {
                Orders = new SelectList(classificationMgr.Search(new ClassificationSearch()),"ID","OrderName");
            }
        }

        public Family Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        
        public Family ParentEntity
        {
            get { return _ParentEntity; }
            set { _ParentEntity = value; }
        }

        public FamilySearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        
        public Collection<Family> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        
        public Collection<Family> DataCollectionSynonyms
        {
            get { return _DataCollectionSynonyms; }
            set { _DataCollectionSynonyms = value; }
        }
        
        public Collection<Family> DataCollectionSubdivisions
        {
            get { return _DataCollectionSubdivisions; }
            set { _DataCollectionSubdivisions = value; }
        }

        public Collection<Classification> DataCollectionClassifications
        {
            get { return _DataCollectionClassification; }
            set { _DataCollectionClassification = value; }
        }

        public SelectList FamilyTypes { get; set; }

        public SelectList Orders { get; set; }

        private List<CodeValue> GetFamilyTypes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();

            ObjectCache cache = MemoryCache.Default;
            codeValues = cache["TAXONOMY_FAMILY_TYPE"] as List<CodeValue>;

            if (codeValues == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (CodeValueManager mgr = new CodeValueManager())
                {
                    codeValues = mgr.GetCodeValues("TAXONOMY_FAMILY_TYPE");
                }
                cache.Set("TAXONOMY_FAMILY_TYPE", codeValues, policy);
            }
            return codeValues;
        }

        public bool IsWebVisibleSelector
        {
            get { return _IsWebVisibleSelector; }
            set { _IsWebVisibleSelector = value; }
        }
    }
}
