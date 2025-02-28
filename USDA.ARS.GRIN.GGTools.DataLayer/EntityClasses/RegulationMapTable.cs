using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class RegulationMapTable
    {
        public int taxonomy_regulation_map_id { get; set; }
        public int taxonomy_family_id { get; set; }
        public int taxonomy_genus_id { get; set; }
        public int taxonomy_species_id { get; set; }
        public int taxonomy_regulation_id { get; set; }
        public string note { get; set; }
        public string is_exempt { get; set; }
    }
}
