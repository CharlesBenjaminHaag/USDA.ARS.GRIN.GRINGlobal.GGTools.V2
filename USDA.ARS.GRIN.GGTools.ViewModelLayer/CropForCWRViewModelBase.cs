using System;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CropForCWRViewModelBase: AppViewModelBase
    {
        private CropForCWR _Entity = new CropForCWR();
        private CropForCWRSearch _SearchEntity = new CropForCWRSearch();
        private Collection<CropForCWR> _DataCollection = new Collection<CropForCWR>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();

        public CropForCWRViewModelBase()
        {
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_cwr_crop"), "ID", "FullName");
            }
        }

        public CropForCWR Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public CropForCWRSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<CropForCWR> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<CodeValue> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }

        public SelectList SpeciesCitations { get; set; }
    }
}
