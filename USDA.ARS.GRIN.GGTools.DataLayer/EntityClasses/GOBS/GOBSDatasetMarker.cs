using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.GOBS.DataLayer
{
    public class GOBSDatasetMarker : AppEntityBase
    {
        public int MethodID { get; set; }
        public string MethodName { get; set; }
        public int DatasetID { get; set; }
        public string DatasetName { get; set; }
        public string IsTrait { get; set; }
    }
}
