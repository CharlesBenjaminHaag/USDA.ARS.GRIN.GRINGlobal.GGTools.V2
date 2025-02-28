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
    public class CWRMapSearch: SearchEntityBase
    {
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public bool SpeciesIsAcceptedNameOption { get; set; }
        public int CropForCWRID { get; set; }
        public string CropForCWRName { get; set; }
        public string CropCommonName { get; set; }
        public string IsCrop { get; set; }
        public string GenepoolCode { get; set; }
        public string IsGraftStock { get; set; }
        public string IsPotential { get; set; }
    }
}
