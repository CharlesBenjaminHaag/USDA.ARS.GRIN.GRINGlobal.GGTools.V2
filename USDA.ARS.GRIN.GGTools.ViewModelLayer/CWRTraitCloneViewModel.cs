using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CWRTraitCloneViewModel: CWRTraitViewModel
    {
        public int Quantity { get; set; }
        public Collection<CWRTrait> DataCollectionClone { get; set; }
    }
}
