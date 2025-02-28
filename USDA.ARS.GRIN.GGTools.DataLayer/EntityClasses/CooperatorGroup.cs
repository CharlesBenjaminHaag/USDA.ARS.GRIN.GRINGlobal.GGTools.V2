using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class CooperatorGroup: AppEntityBase
    {
        public string Name { get; set; }
        public string IsActive { get; set; }
        public int SiteID { get; set; }
    }
}
