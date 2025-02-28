using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysDBError
    {
        public int ErrorID { get; set; }
        public string UserName { get; set; }
        public int ErrorNumber { get; set; }
        public int ErrorState { get; set; }
        public int ErrorSeverity { get; set; }
        public int ErrorLine { get; set; }
        public string ErrorProcedure { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ErrorDateTime { get; set; }
    }
}
