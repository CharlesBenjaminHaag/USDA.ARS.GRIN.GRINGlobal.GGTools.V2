using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class FamilyMapSearch : SearchEntityBase
    {
        public int OrderID { get; set; }
        public string OrderName { get; set; }
        public string FamilyIDList { get; set; }
        public string IsAcceptedName { get; set; }
        public string IsInfrafamilal { get; set; }
        public string Authority { get; set; }
        public string FamilyTypeCode { get; set; }
        public string Rank { get; set; }
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public string AlternateName { get; set; }
        public int SubFamilyID { get; set; }
        public string SubFamilyName { get; set; }
        public int TribeID { get; set; }
        public string TribeName { get; set; }
        public int SubTribeID { get; set; }
        public string SubTribeName { get; set; }
        public int TypeGenusID { get; set; }
        public string TypeGenusName { get; set; }
        public string SuprafamilyRankDescription { get; set; }
    }
}

