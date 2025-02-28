using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using System.Security;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class FamilyTable
    {
        public int taxonomy_family_id { get; set; }
        public string family_name { get; set; }
        public string subfamily_name { get; set; }
        public string tribe_name { get; set; }
        public string subtribe_name { get; set; }
        public string family_authority { get; set; }
    }
}
