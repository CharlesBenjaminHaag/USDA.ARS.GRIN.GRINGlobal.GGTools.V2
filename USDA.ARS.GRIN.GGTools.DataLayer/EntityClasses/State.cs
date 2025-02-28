using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class State : AppEntityBase
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string Admin1 { get; set; }
    }
}
