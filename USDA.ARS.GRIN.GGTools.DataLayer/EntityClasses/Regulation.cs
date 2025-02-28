using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class Regulation : AppEntityBase
    {
        public int  GeographyID { get; set; }
        public string GeographyText { get; set; }
        public string GeographyDescription { get; set; }
        public string RegulationText { get; set; }
        public string RegulationTypeCode { get; set; }
        public string RegulationTypeDescription { get; set; }
        public string RegulationLevelCode { get; set; }
        public string RegulationLevelDescription { get; set; }
        public string Description { get; set; }
        public string URL1 { get; set; }
        public string URL2 { get; set; }
    }
}
