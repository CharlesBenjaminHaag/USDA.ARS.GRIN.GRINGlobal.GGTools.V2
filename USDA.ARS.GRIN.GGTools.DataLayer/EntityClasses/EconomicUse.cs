using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class EconomicUse: AppEntityBase
    {
        public int SpeciesID { get; set; }
        [AllowHtml]
        public string SpeciesName { get; set; }
        public string Name { get; set; }
        public int EconomicUsageTypeID { get; set; }
        public string EconomicUsageType { get; set; }
        public string PlantPartCode { get; set; }
        public string PlantPartDescription { get; set; }
        public string CitationText { get; set; }
        public string Abbreviation { get; set; }
        public Collection<Citation> Citations { get; set; }
    }
}
