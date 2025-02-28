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
    public class Tribe : AppEntityBase
    {
        public string TribeName { get; set; }
        public string SubfamilyName { get; set; }
        public int SubfamilyID { get; set; }
        public int FamilyID { get; set; }
        
        public bool IsAccepted { get; set; }
        public string FamilyName { get; set; }
        public string Authority { get; set; }
        public int TypeGenusID { get; set; }
        public string TypeGenusName { get; set; }
    }
}
