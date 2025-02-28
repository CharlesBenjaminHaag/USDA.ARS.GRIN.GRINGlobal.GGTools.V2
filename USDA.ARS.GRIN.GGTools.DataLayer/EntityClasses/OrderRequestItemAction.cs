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
    public class OrderRequestItemAction : AppEntityBase
    {
        public int ID { get; set; }
        public int OrderRequestID { get; set; }
        public int SequenceNumber { get; set; }
        public int AccessionID { get; set; }
        public string AccessionName { get; set; }
        public int InventoryID { get; set; }
        public string InventoryName { get; set; }
        public string InventoryQuantityOnHandUnitCode { get; set; }
        public string InventoryAvailabilityStatusCode { get; set; }
        public int SiteID { get; set; }
        public string SiteLongName { get; set; }
        public string Name { get; set; }
        public string ExternalTaxonomy { get; set; }
        public string GeographyName { get; set; }
        public int QuantityShipped { get; set; }
        public string QuantityShippedUnitCode { get; set; }
        public string DistributionFormCode { get; set; }
        public int StatusCode { get; set; }
        public DateTime StatusDate { get; set; }
    }
}
