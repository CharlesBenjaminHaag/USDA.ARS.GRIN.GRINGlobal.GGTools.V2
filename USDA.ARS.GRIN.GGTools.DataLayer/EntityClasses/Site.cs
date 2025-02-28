using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class Site : AppEntityBase
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        
        public string ProviderIdentifier { get; set; }
        public string OrganizationAbbrev { get; set; }
        public string IsInternal { get; set; }
        public bool IsInternalOption { get; set; }
        public string IsDistributionSite { get; set; }
        public bool IsDistributionSiteOption { get; set; }
        public string TypeCode { get; set; }
        public string FAOInstituteNumber { get; set; }
        public int CooperatorID { get; set; }
        public string PrimaryAddress1 { get; set; }
        public string PrimaryAddress2 { get; set; }
        public string PrimaryAddress3 { get; set; }
        public string City { get; set; }
        public string PostalIndex { get; set; }
        public int PrimaryAddressGeographyID { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryAddress1 { get; set; }
        public string SecondaryAddress2 { get; set; }
        public string SecondaryAddress3 { get; set; }
        public string SecondaryCity { get; set; }
        public int SecondaryAddressGeographyID { get; set; }
        public string SecondaryPostalIndex { get; set; }
        public string SecondaryPhone { get; set; }
        public string EmailAddress { get; set; }
        public int GeographyID { get; set; }
        public string State { get; set; }
        public string PrimaryURL { get; set; }
        public string SecondaryURL { get; set; }
    }
}
