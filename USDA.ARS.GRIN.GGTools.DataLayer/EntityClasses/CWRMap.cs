using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CWRMap: AppEntityBase
    {
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public int CropForCWRID { get; set; }
        public string CropForCWRName { get; set; }
        public string CropCommonName { get; set; }
        public string IsCrop { get; set; }
        public bool IsCropOption { get; set; }
        public string GenepoolCode { get; set; }
        public string IsGraftstock { get; set; }
        public bool IsGraftStockOption { get; set; }
        public string IsPotential { get; set; }
        public bool IsPotentialOption { get; set; }
        [AllowHtml]
        public string CitationText { get; set; }
        public List<CWRTrait> CWRTraits { get; set; }
        public Collection<Citation> Citations { get; set; }
    }
}
