using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.GOBS.DataLayer
{
    public class GOBSDatasetField : AppEntityBase
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string FieldName { get; set; }
        public string Validator { get; set; }
        public string Message { get; set; }
        public string IsOptional { get; set; }
    }
}
