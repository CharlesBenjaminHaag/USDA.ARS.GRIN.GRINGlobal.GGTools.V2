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
    public class CWRTraitTable
    {
        public int taxonomy_cwr_trait_id { get; set; }
        public int taxonomy_cwr_map_id { get; set; }
        public string trait_class_code { get; set; }
        public string is_potential { get; set; }
        public string breeding_type_code { get; set; }
        public string breeding_usage_note { get; set; }
        public string ontology_trait_identifier { get; set; }
        public int citation_id { get; set; }
    }
}
