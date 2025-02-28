using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysTableSearch : SearchEntityBase
    {
        public string SysTag { get; set; }
        public string DatabaseAreaCode { get; set; }
        public string SysTableName { get; set; }
        public string SysTableTitle { get; set; }
        public string TableCode { get; set; }
        public string IsMapped { get; set; }
    }
}
