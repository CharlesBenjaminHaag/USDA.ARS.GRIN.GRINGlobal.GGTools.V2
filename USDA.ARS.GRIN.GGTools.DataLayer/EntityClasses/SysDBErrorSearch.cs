using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysDBErrorSearch
    {
        public int ErrorID { get; set; }
        public string UserName { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorState { get; set; }
        public string ErrorSeverity { get; set; }
        public string ErrorLine { get; set; }
        public string ErrorProcedure { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ErrorDateTime { get; set; }
    }
}
