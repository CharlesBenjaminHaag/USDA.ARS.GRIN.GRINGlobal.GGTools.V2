using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class CooperatorMap: AppEntityBase
    {
        public int CooperatorGroupID { get; set; }
        public int CooperatorID { get; set; }
        public string CooperatorName { get; set; }
        public string CooperatorGroupName { get; set; }
        public string IsActive { get; set; }
        public int SiteID { get; set; }
        public string SiteShortName { get; set; }
        public string SiteLongName { get; set; }
        public string CategoryCode { get; set; }
        public string GroupTag { get; set; }
    }
}
