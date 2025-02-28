using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class ErrorLog
    {
        public string Source { get; set; }
        public string Application { get; set; }
        public int ID { get; set; }
        public string Code { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorDetail2 { get; set; }
        public string ErrorDetail3 { get; set; }
        public string ErrorDetail4 { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
