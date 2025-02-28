using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class Region:AppEntityBase
    {
        public string Title { get; set; }
        public string Continent { get; set; }
        public string SubContinent { get; set; }
        public string ContinentAbbrev { get; set; }
        public string SubContinentAbbrev { get; set; }
        public string RegionText { get; set; }
        public string IsContinent { get; set; }
    }
}
