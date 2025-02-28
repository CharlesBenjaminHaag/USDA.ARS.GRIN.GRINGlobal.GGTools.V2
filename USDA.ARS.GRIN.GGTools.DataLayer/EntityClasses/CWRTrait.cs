using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CWRTrait : AppEntityBase
    {
        public int CWRMapID { get; set; }
        public string CWRMapName { get; set; }

        public string GenepoolCode { get; set; }
        public int CropForCWRID { get; set; }
        public string CropForCWRName { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public string TraitClassCode { get; set; }
        public string TraitClassTitle { get; set; }
        public string IsPotential { get; set; }
        public bool IsPotentialOption { get; set; }
        public string BreedingTypeCode { get; set; }
        public string BreedingTypeTitle { get; set; }
        public string BreedingUsageNote { get; set; }
        public string OntologyTraitIdentifier { get; set; }
        [AllowHtml]
        public string CitationText { get; set; }
        public Collection<Citation> Citations { get; set; }
    }
}
