using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class SysPermissionSearch : SearchEntityBase
    {
        public int SysGroupID { get; set; }
        public string GroupTag { get; set; }
        public string PermissionTag { get; set; }
        public string IsMember { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SysDataViewID { get; set; }
        public int SysTableID { get; set; }
        public int SysUserID { get; set; }
        
        public string TableTitle { get; set; }
        public string IsEnabled { get; set; }
        public string CreatePermission { get; set; }
        public string ReadPermission { get; set; }
        public string UpdatePermission { get; set; }
        public string DeletePermission { get; set; }
        public string CreatePermissionTitle { get; set; }
        public string ReadPermissionTitle { get; set; }
        public string UpdatePermissionTitle { get; set; }
        public string DeletePermissionTitle { get; set; }
    }
}
