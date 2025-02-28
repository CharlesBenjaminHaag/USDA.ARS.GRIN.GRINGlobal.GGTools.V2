using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class SysGroupSearch : SearchEntityBase
    {
        public string GroupTag { get; set; }
        public string GroupTitle { get; set; }
        public string GroupDescription { get; set; }
        public string IsEnabled { get; set; }
    }
}
