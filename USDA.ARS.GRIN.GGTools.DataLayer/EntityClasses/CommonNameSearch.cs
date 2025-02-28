using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CommonNameSearch : SearchEntityBase
    {
        public string SpeciesIDList { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public int LanguageID { get; set; }
        public string LanguageDescription { get; set; }
        public string Name { get; set; }
        public string IsNameExactMatch { get; set; }
        public string SimplifiedName { get; set; }
        public string AlternateTranscription { get; set; }
        
    }
}
