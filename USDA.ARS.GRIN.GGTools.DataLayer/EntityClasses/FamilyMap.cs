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
    public class FamilyMap : AppEntityBase
    {
        public int OrderID { get; set; }
        public string OrderName { get; set; }
        public int TypeGenusID { get; set; }
        public string TypeGenusName { get; set; }
        public bool IsAccepted { get; set; }
        public string IsAcceptedName { get; set; }
        public string Rank { get; set; }
        public int FamilyID { get; set; }
        public int LegacyFamilyID { get; set; }
        public string FamilyName { get; set; }
        public string AlternateName { get; set; }
        public string FamilyTypeCode { get; set; }
        public string FamilyTypeDescription { get; set; }
        public string Authority { get; set; }
        public int SubfamilyID { get; set; }
        public string SubfamilyName { get; set; }
        public int TribeID { get; set; }
        public string TribeName { get; set; }
        public int SubtribeID { get; set; }
        public string SubtribeName { get; set; }
        
    }
}
