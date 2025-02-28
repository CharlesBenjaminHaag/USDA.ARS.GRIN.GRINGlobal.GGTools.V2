using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.GOBS.DataLayer
{
    public class GOBSDatasetSearch : SearchEntityBase
    {
        public int MethodID { get; set; }
        public string MethodName { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string DatasetName { get; set; }
        public string Link { get; set; }
    }
}
