using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer.OBSOLETE
{
    public class AppUserDynamicQuery : AppEntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DataSource { get; set; }
        public string QuerySyntax { get; set; }
    }
}
