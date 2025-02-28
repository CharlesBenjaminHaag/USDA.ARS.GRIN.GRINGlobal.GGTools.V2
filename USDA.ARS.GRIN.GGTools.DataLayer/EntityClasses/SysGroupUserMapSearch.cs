using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysGroupUserMapSearch
    {
        public int SysUserID { get; set; }
        public int SysGroupID { get; set; }
        public string GroupTag { get; set; }
        public string IsAvailable { get; set; }
    }
}
