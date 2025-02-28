using System;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysFolderItemMap: AppEntityBase
    {
        public int SysFolderID { get; set; }
        public string SysFolderTitle { get; set; }
        public string TableTitle { get; set; }
        public int IDNumber { get; set; }
    }
}
