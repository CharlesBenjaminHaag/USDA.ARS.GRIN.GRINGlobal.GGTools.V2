using System;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysFolder: AppEntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TypeCode { get; set; }
        //public string TableName { get; set; }
        public string Properties { get; set; }
        public string IsShared { get; set; }
        public string IsSharedWithMe { get; set; }
    }
}
