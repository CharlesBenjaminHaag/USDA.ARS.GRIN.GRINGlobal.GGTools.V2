using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SpeciesBatchEditViewModel: SpeciesViewModel
    {
        public string SelectedSpeciesIDList { get; set; }
        public string SelectedLiteratureIDList { get; set; }
        public string SelectedCitationIDList { get; set; }
        public string SelectedGeographyMapIDList { get; set; }
        public string SelectedEconomicUseIDList { get; set; }
        public string SelectedCommonNameIDList { get; set; }
    }
}
