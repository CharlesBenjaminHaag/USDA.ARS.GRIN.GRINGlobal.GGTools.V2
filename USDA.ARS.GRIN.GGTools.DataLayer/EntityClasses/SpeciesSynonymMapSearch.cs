using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesSynonymMapSearch : SearchEntityBase
    {
        public int SpeciesAID { get; set; }
        public string SpeciesAName { get; set; }
        public bool SpeciesAIsAcceptedOption { get; set; }
        public int SpeciesBID { get; set; }
        public string SpeciesBName { get; set; }
        public bool SpeciesBIsAcceptedOption { get; set; }
        public string SynonymCode { get; set; }
        public string SynonymDescription { get; set; }
        public string ItemIDList { get; set; }
    }
}
