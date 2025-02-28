using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer.OBSOLETE
{
    public partial class AppUserItemDynamicFolder : AppUserItemFolder
    {
        public string TableCode { get; set; }
        public string TableName { get; set; }
        public string TableTitle { get; set; }
    }
}