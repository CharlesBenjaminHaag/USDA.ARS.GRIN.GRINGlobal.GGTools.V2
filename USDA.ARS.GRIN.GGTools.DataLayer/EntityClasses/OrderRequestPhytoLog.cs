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
    public class OrderRequestPhytoLog : AppEntityBase
    {
        public int OrderRequestID { get; set; }
        public string ActionNameCode { get; set; }

    }
}
