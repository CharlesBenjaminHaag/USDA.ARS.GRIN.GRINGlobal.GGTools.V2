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
    public class LiteratureSearch : SearchEntityBase
    {
        public string Abbreviation { get; set; }
        public string StandardAbbreviation { get; set; }
        public string ReferenceTitle { get; set; }
        public string EditorAuthorName { get; set; }
        public string LiteratureTypeCode { get; set; }
        public string PublicationYear { get; set; }
        public string PublisherName { get; set; }
        public string URL { get; set; }
    }
}
