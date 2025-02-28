using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class TaxonomyExplorerViewModel
    {
        public string EventAction = String.Empty;
        public string EventValue = String.Empty;
        public Collection<FamilyMap> DataCollectionFamily = new Collection<FamilyMap>();
        public Collection<Genus> DataCollectionGenus = new Collection<Genus>();
        public Collection<Species> DataCollectionSpecies = new Collection<Species>();
        public SpeciesViewModel SpeciesViewModel = new SpeciesViewModel();
    }
}
