using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class EconomicUsageTypeSearch: SearchEntityBase
    {
        public string EconomicUsageCode { get; set; }
        public string UsageType { get; set; }
    }
}
