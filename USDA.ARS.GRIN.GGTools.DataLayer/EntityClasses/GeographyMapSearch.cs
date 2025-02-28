using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class GeographyMapSearch : SearchEntityBase 
    {
        public string GeographyMapIDList { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public bool SpeciesIsAcceptedNameOption { get; set; }
        public int GeographyID { get; set; }
        public string GeographyDescription { get; set; }
        public string GeographyStatusCode { get; set; }
        public String GeographyStatusDescription { get; set; }
        public string Admin1 { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string IsCited { get; set; }
        public string IsValid { get; set; }
        public bool IsValidOption { get; set; }
    }
}
