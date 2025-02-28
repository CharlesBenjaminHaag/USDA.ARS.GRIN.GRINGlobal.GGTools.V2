using System;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysFolderSearch: SearchEntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TypeCode { get; set; }
    }
}
