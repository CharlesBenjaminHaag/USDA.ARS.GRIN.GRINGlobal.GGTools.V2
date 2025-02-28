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
    public class AuthorSearch : SearchEntityBase
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string ShortNameExpandedDiacritic { get; set; }
        public string FullNameExpandedDiacritic { get; set; }
        public string IsShortNameExactMatch { get; set; }
        public bool IsShortNameExactMatchOption { get; set; }
        public string AuthorityText { get; set; }
    }
}
