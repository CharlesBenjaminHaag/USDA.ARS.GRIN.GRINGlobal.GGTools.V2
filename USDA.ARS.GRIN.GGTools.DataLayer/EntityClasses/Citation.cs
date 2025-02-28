using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class Citation : AppEntityBase
    {
        [AllowHtml]
        public string CitationTitle { get; set; }
        public string AuthorName { get; set; }
        public int? CitationYear { get; set; }
        // Volumr Or Page
        public string Reference { get; set; }
        public string DOIReference { get; set; }
        public string URL { get; set; }
        [AllowHtml]
        public string ReferenceTitle { get; set; }
        public string ReferenceDescription { get; set; }
        public int AccessionID { get; set; }
        public int MethodID { get; set; }
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public int SpeciesID { get; set; }
        [AllowHtml]
        public string SpeciesName { get; set; }
        public string TaxonName { get; set; }
        public int AccessionIPRID { get; set; }
        public int AccessionPedigreeID { get; set; }
        public int GeneticMarkerID { get; set; }
        public string TypeCode { get; set; }
        public string CategoryCode { get; set; }
        public int UniqueKey { get; set; }
        public string IsAcceptedName { get; set; }
        public bool IsAcceptedNameOption { get; set; }
        public int LiteratureID { get; set; }
        public string Abbreviation { get; set; }
    }
}
