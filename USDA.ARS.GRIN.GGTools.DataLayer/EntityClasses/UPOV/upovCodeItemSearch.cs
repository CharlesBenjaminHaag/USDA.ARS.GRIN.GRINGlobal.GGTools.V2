using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer.UPOV
{
    public class upovCodeItemSearch: SearchEntityBase
    {
        public int taxonomy_species_upov_id { get; set; }
        public int upovCodeID { get; set; }
        public string upovCode { get; set; }
        public string principalBotanicalName { get; set; }
        public string otherBotanicalName { get; set; }
        public string denominationClass { get; set; }
        public string commonNameEN { get; set; }
        public string commonNameFR { get; set; }
        public string commonNameDE { get; set; }
        public string commonNameES { get; set; }
        public string commonNameZH { get; set; }
        public string commonNameJA { get; set; }
        public string commonNameKO { get; set; }
        public string commonNameVI { get; set; }
        public string commonNameTR { get; set; }
        public string hasTqForm { get; set; }
        public string note { get; set; }
    }
}
