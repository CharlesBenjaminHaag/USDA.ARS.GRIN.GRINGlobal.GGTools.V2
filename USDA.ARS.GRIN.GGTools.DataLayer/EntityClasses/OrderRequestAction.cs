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
    public class OrderRequestAction : AppEntityBase
    {
        public int OrderRequestID { get; set; }
        public string ActionNameCode { get; set; }
        public DateTime StartedDate { get; set; }
        public string StartedDateCode { get; set; }
        public DateTime CompletedDate { get; set; }
        public string CompletedDateCode { get; set; }
        public string ActionInformation { get; set; }
        public decimal ActionCost { get; set; }
        public int CooperatorID { get; set; }
    }
}
