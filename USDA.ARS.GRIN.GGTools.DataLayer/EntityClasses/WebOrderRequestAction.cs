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
    public class WebOrderRequestAction : AppEntityBase
    {
        public int WebOrderRequestID { get; set; }
        public string ActionCode { get; set; }
        public string ActionTitle { get; set; }
        public string ActionDescription { get; set; }
        public DateTime ActionDate { get; set; }
        public int CreatedByWebUserID { get; set; }
        public string CreatedByWebUserName { get; set; }
        
        public int CreatedByWebCooperatorID { get; set; }
        
        public string CreatedByWebCooperatorLastName { get; set; }
        public string CreatedByWebCooperatorFirstName { get; set; }
        public int OwnedByWebUserID { get; set; }
        public string OwnedByWebCooperatorName { get; set; }

    }
}
