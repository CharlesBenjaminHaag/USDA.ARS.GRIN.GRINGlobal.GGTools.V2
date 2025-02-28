using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
   public class ClassificationViewModelBase : AppViewModelBase
    {
        private Classification _Entity = new Classification();
        private ClassificationSearch _SearchEntity = new ClassificationSearch();
        private Collection<Classification> _DataCollection = new Collection<Classification>();
        //private Collection<Family> _DataCollectionFamilies = new Collection<Family>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();

        public ClassificationViewModelBase()
        {
            using (FamilyMapManager mgr = new FamilyMapManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_classification"), "ID", "FullName");
            }
        }

        public Classification Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public ClassificationSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<Classification> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        //public Collection<Family> DataCollectionFamilies
        //{
        //    get { return _DataCollectionFamilies; }
        //    set { _DataCollectionFamilies = value; }
        //}
        public Collection<CodeValue> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }
    }
}
