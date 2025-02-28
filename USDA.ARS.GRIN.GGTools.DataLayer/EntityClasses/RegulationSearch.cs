using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class RegulationSearch: SearchEntityBase
    {
        public int GeographyID { get; set; }
        public string GeographyDescription { get; set; }
        public string StateName { get; set; }
        public string RegulationTypeCode { get; set; }
        public string RegulationLevelCode { get; set; }
        public string Description { get; set; }
        public string URL1 { get; set; }
        public string URL2 { get; set; }
    }
}
