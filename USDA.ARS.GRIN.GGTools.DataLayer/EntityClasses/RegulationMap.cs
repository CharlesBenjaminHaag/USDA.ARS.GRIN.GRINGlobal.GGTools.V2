using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class RegulationMap: AppEntityBase
    {
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public int SpeciesID { get; set; }
        public int RegulationID { get; set; }
        public string RegulationText { get; set; }
        public int GeographyID { get; set; }
        [AllowHtml]
        public string SpeciesName { get; set; }
        public string TaxonName { get; set; }
        public string RegulationTypeCode { get; set; }
        public string RegulationTypeDescription { get; set; }
        public string RegulationLevelCode { get; set; }
        public string RegulationLevelDescription { get; set; }
        public string Description { get; set; }
        public string IsExempt { get; set; }
        public bool IsExemptOption { get; set; }
    }
}
