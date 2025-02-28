using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class GeographyMap: AppEntityBase
    {
        public int SpeciesID { get; set; }
        [AllowHtml]
        public string SpeciesName { get; set; }
        public int GeographyID { get; set; }
        public string GeographyDescription { get; set; }
        public string GeographyStatusCode { get; set; }
        public String GeographyStatusDescription { get; set; }
        public string Admin1 { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string IsCited { get; set; }
        public string CitationText { get; set; }
        public Collection<Citation> Citations { get; set; }
    }
}
