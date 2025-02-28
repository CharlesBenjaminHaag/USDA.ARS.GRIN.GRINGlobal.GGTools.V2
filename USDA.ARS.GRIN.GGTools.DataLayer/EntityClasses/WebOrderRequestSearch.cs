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
    public class WebOrderRequestSearch : SearchEntityBase
    {
        public int WebCooperatorID { get; set; }
        public int OwnedByWebUserID { get; set; }
        public DateTime OrderDate { get; set; }
        public string IntendedUseCode { get; set; }
        public string IntendedUseNote { get; set; }
        public string StatusCode { get; set; }
        public string SpecialInstruction { get; set; }
        public string WebCooperatorFirstName { get; set; }
        public string WebCooperatorLastName { get; set; }
        public string WebCooperatorEmailAddress { get; set; }
        public string WebCooperatorOrganization { get; set; }
        public string WebCooperatorAddressCountry { get; set; }
        public string WebCooperatorAddressCountryDescription { get; set; }
        public string TimeFrame { get; set; }
        public string StatusList { get; set; }
        public string MostRecentActionList { get; set; }
        public string MostRecentAction { get; set; }
        public string IntendedUseList { get; set; }
        public string WebUserList { get; set; }
        public int Year { get; set; }
        public string IsLocked { get; set; }
        public string HasOrders { get; set; }
    }
}
