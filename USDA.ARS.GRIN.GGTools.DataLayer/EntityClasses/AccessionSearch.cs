using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AccessionSearch : AppEntityBase
    {
        public string AccessionPrefix { get; set; }
        public string AccessionNumber { get; set; }
        public string AccessionSuffix { get; set; }
        public int SpeciesID { get; set; }
        public string StatusCode { get; set;}
        public string InstCode { get; set; }
    }
}
