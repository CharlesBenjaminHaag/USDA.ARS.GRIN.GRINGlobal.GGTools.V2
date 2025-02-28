using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysGroupUserMap: AppEntityBase
    {
        public int SysGroupID { get; set; }
        public int SysUserID { get; set; }
        public int CooperatorID { get; set; }
        public string GroupTag { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SysUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SiteLongName { get; set; }
        public string SysUserIsEnabled { get; set; }
    }
}
