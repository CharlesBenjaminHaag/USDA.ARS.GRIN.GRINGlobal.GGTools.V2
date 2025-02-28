using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysDataViewParameter : AppEntityBase
    {
        public int SysDataViewID { get; set; }
        public string ParamName { get; set; }
        public string ParamType { get; set; }
        public string ParamValue { get; set; }
        public int SortOrder { get; set; }
    }
}
