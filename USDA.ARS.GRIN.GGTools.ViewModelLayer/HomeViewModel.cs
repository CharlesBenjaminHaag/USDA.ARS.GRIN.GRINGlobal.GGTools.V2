using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class HomeViewModel
    {
        public SysUser AuthenticatedUser { get; set; }
        public int CooperatorID { get; set; }
        public int SysUserID { get; set; }
        public int SiteID { get; set; }
        public string SiteShortName { get; set; }
    }
}
