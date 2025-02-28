using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class WebOrderRequestItem : AppEntityBase
    {
        public int WebOrderRequestID { get; set; }
        public int SequenceNumber { get; set; }
        public int AccessionID { get; set; }
        public string PINumber { get; set; }
        public string AccessionText { get; set; }
        public string StatusCode { get; set; }
        public string TypeCode { get; set; }
        public int SiteID { get; set; }
        public string SiteLongName { get; set; }
        public string SiteShortName { get; set; }
        public string PlantName { get; set; }
        public string DistributionForm { get; set; }
        public decimal QuantityShipped { get; set; }
        public string UnitOfQuantity { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public string ItemName { get; set; }
     
    }
}
