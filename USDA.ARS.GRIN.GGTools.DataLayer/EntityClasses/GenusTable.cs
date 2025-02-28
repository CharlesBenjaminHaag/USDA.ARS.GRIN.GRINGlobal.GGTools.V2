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
    public class GenusTable
    {
        public int taxonomy_genus_id { get; set; }
        public string hybrid_code { get; set; }
        public string genus_name { get; set; }
        public string genus_authority { get; set; }
        public string subgenus_name { get; set; }
        public string section_name { get; set; }
        public string subsection_name { get; set; }
        public string series_name { get; set; }
        public string subseries_name { get; set; }
    }
}
