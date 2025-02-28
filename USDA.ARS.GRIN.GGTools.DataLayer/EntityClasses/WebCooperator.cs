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
    public class WebCooperator: AppEntityBase
    {
        public int WebUserID { get; set; }
        public string WebUserName { get; set; }
        public string IsActive { get; set; }
        public DateTime WebUserCreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string JobTitle { get; set; }
        public string OrganizationRegionCode { get; set; }
        public string PrimaryPhone { get; set; }
        public string Organization { get; set; }
        public string OrganizationAbbrev { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public int GeographyID { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CategoryCode { get; set; }
        public string DisciplineCode { get; set; }
        public int TotalOrders { get; set; }
        public int TotalOrderItems { get; set; }
        public string VettedStatusCode { get; set; }
        public int ModifiedByWebUserID { get; set; }
        
    }
}
