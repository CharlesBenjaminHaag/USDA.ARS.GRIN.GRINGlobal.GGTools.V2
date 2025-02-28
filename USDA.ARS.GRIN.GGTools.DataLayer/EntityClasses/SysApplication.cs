using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysApplication: AppEntityBase
    {
        public string ApplicationCode { get; set; }
        public string ApplicationTitle { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationURL { get; set; }
    }
}
