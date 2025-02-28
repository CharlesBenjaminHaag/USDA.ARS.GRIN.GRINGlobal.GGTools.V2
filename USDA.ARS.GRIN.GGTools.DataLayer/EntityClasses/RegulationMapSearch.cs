using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class RegulationMapSearch : SearchEntityBase
    {
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public string TaxonName { get; set; }
        public int RegulationID { get; set; }
        public int GeographyID { get; set; }
        public string Description { get; set; }
        public string IsExempt { get; set; }
        public string SpeciesIDList { get; set; }
    }
}
