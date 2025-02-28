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
    public class CitationSearch: SearchEntityBase
    {
        public string CitationTitle { get; set; }
        public string AuthorName { get; set; }
        public int? CitationYear { get; set; }
        // Volumr Or Page
        public string Reference { get; set; }
        public string DOIReference { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // NOTE: OMITTED
        // AccessionID
        public int AccessionID { get; set; }
        // MethodID
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesIDList { get; set; }
        public string SpeciesName { get; set; }

        // NOTE: OMITTED
        // AccessionIPRID
        // AccessionPedigreeID
        // GeneticMarkerID
        public string TypeCode { get; set; }
        public int UniqueKey { get; set; }
        public string IsAcceptedName { get; set; }
        public bool IsAcceptedNameOption { get; set; }
        public int LiteratureID { get; set; }
        public string Abbreviation { get; set; }
        public string StandardAbbreviation { get; set; }
        public string EditorAuthorName { get; set; }
        public string ReferenceTitle { get; set; }
        public string LiteratureTypeCode { get; set; }
        public string PublicationYear { get; set; }
        public string PublisherName { get; set; }
        public string PublisherLocation { get; set; }
    }
}
