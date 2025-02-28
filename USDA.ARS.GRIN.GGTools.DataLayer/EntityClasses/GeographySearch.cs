using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class GeographySearch: SearchEntityBase
    {
        public string IsUSOnly { get; set; }
        public string IncludeNonRegions { get; set; }
        public int ContinentRegionID { get; set; }
        public int SubContinentRegionID { get; set; }
        public string CountryCode { get; set; }
        public string CountryDescription { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string Admin1 { get; set; }
        public string Admin1Abbrev { get; set; }
        public string Admin1TypeCode { get; set; }
        public string Admin1TypeDescription { get; set; }
        public string Admin2 { get; set; }
        public string Admin2Abbrev { get; set; }
        public string Admin2TypeCode { get; set; }
        public string Admin2TypeDescription { get; set; }
        public string IsValid { get; set; }
        public string IsRegionMapped { get; set; }
        public string Title { get; set; }
        public string ContinentIDList { get; set; }
        public string ContinentName { get; set; }
        public string ContinentNameList { get; set; }
        public string SubContinentIDList { get; set; }
        public string SubContinentName { get; set; }
        public string SubContinentNameList { get; set; }
        public string CountryCodeList { get; set; }
    }
}
