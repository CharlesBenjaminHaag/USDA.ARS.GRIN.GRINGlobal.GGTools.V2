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
    public class Genus : AppEntityBase
    {
        public int AcceptedNameID { get; set; }
        [AllowHtml]
        public bool IsAccepted { get; set; }
        public string IsAcceptedName { get; set; }
        public string Rank { get; set; }
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public string FamilyFullName { get; set; }
        public string FamilyAssembledName { get; set; }
        //public string Rank { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string QualifyingCode { get; set; }
        public string QualifyingCodeTitle { get; set; }
        public string HybridCode { get; set; }
        public string HybridCodeTitle { get; set; }
        public bool IsSynonym { get; set; }
        public string Authority { get; set; }
        public string SubgenusName { get; set; }
        public string SectionName { get; set; }
        public string SubsectionName { get; set; }
        public string SeriesName { get; set; }
        public string SubseriesName { get; set; }
    }
}
