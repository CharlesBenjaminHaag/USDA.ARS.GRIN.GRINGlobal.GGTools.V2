using System;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysTagSearch: SearchEntityBase
    {
        public string TagText { get; set; }
        public string TagFormatString { get; set; }
        
        public string PrimaryKeyFieldName { get; set; }
        public int IDNumber { get; set; }
    }
}
