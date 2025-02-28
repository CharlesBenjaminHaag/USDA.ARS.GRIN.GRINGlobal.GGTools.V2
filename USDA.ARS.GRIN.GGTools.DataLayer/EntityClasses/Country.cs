using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class Country: AppEntityBase
    {
        public string CountryCode { get; set; }
        public string CountryDescription { get; set; }
    }
}
