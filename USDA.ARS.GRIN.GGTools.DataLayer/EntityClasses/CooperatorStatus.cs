using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class CooperatorStatus: AppEntityBase
    {
        public int CooperatorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CooperatorEmailAddress { get; set; }
        public string StatusCode { get; set; }
        public DateTime CooperatorCreatedDate { get; set; }
        public string IsSysUserEnabled { get; set; }
        public string IsSysGroupEnabled { get; set; }
        public string IsWebUserEnabled { get; set; }
        public string IsWebCooperatorEnabled { get; set; }
        public int SysUserID { get; set; }
        public string SysUserName { get; set; }
        public DateTime SysUserCreatedDate { get; set; }
        public int WebCooperatorID { get; set; }
        public DateTime WebCooperatorCreatedDate { get; set; }
        public int WebUserID { get; set; }
        public string WebUserName { get; set; }
        public DateTime WebUserCreatedDate { get; set; }
    }
}
