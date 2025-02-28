using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class WebUserShippingAddress: AppEntityBase 
    {
        public int WebUserId { get; set; }
        public string AddressName { get; set;}
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string PostalIndex { get; set; }
        public int GeographyID { get; set; }
        public string CountryCode { get; set; }
        public string IsDefault { get; set; }
        public int CreatedByWebUserID { get; set; }
        public string CreatedByWebCooperatorName { get; set; }
        public int ModifiedByWebUserID { get; set; }
        public string ModifiedWebCooperatorName { get; set; }

    }
}
