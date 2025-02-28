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
    public class CWRTraitSearch: SearchEntityBase
    {
        public int CropForCWRID { get; set; }
        public int CWRMapID { get; set; }
        public string CropForCWRName { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public string TraitClassCode { get; set; }
        public string IsPotential { get; set; }
        public string BreedingTypeCode { get; set; }
        public string BreedingUsageNote { get; set; }
        public string OntologyTraitIdentifier { get; set; }
        
    }
}
