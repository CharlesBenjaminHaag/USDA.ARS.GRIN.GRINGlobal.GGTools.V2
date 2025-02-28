using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class Family : AppEntityBase
    {
        public string IsAcceptedName { get; set; }
        public int TypeGenusID { get; set; }
        public string TypeGenusName { get; set; }
        public int ClassificationID { get; set; }
        public string ClassificationName { get; set; }
        public string SuprafamilyRankCode { get; set; }
        public string SuprafamilyRankName { get; set; }
        public string FamilyName { get; set; }
        public string FamilyAuthority { get; set; }
        public string AlternateName { get; set; }
        public string SubfamilyName { get; set; }
        public string TribeName { get; set; }
        public string SubtribeName { get; set; }
        public string FamilyTypeCode { get; set; }
        public string FamilyTypeDescription { get; set; }
    }
}
