using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.DataLayer.UPOV
{
    public class UPOVEncodedSpecies
    {
        public int ID{ get; set; }
        public int UPOVCodeID { get; set; }
        public string UPOVCode { get; set; }
        public string PrincipalBotanicalName { get; set; }
        public string OtherBotanicalName { get; set; }
        public string SpeciesName { get; set; }
        public int SpeciesID { get; set; }
        public string CommonNameText { get; set; }
        public string Note { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
