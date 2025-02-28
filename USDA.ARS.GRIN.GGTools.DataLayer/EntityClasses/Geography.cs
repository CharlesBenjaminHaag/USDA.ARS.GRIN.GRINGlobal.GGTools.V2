using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class Geography : AppEntityBase
    {
        public int RegionID { get; set; }
        public string RegionText { get; set; }
        public string CountryCode { get; set; }
        public string Admin1 { get; set; }
        public string Admin1TypeCode { get; set; }
        public string Admin1TypeDescription { get; set; }
        public string Admin1Abbrev { get; set; }
        public string Admin2 { get; set; }
        public string Admin2TypeCode { get; set; }
        public string Admin2TypeDescription { get; set; }
        public string Admin2Abbrev { get; set; }
        public DateTime ChangedDate { get; set; }
        public string CountryDescription { get; set; }
        public string IsValid { get; set; }
        public bool IsValidOption { get; set; }
        public string IsRegionMapped { get; set; }
        public string GeographyText { get; set; }
        public string Continent { get; set; }
        public string ContinentAbbreviation { get; set; }
        public string SubContinent { get; set; }
        public string SubContinentAbbreviation { get; set; }
    }
}
