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
    public class GenusSearch : SearchEntityBase
    {
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string IsAcceptedName { get; set; }
        public string AcceptedName { get; set; }
        public string Rank { get; set; }
        public string QualifyingCode { get; set; }
        public string HybridCode { get; set; }
        public bool IsSynonym { get; set; }
        public string Authority { get; set; }
        public string SubgenusName { get; set; }
        public string SectionName { get; set; }
        public string SubSectionName { get; set; }
        public string SeriesName { get; set; }
        public string SubseriesName { get; set; }
        
    }
}
