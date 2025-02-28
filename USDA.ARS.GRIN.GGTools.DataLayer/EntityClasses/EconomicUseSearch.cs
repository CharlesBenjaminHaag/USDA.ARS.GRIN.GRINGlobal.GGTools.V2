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
    public class EconomicUseSearch : SearchEntityBase
    {
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public string PlantPartCode { get; set; }
        public string PlantPartDescription { get; set; }
    }
}
