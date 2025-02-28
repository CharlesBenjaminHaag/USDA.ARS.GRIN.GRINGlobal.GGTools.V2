using System;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysFolderProperties: AppEntityBase
    {
        public int SysFolderID { get; set; }
        public string TypeCode { get; set; }
        public string TableName { get; set; }
        public string Properties { get; set; }
    }
}
