using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class WebUserSearch: AppEntityBase 
    {
        public int WebUserID { get; set; }
        public string WebUserName { get; set; }
        public string WebUserPassword { get; set; }
        public string WebUserPlainTextPassword { get; set; }
        public string WebUserIsEnabled { get; set; }
        public DateTime WebUserCreatedDate { get; set; }
        public DateTime WebUserModifiedDate { get; set; }
        public DateTime WebUserPasswordExpirationDate { get; set; }
        public int WebCooperatorID { get; set; } 
    }
}
