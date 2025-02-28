using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.GOBS.DataLayer
{
    public class GOBSDatasetInventory : AppEntityBase
    {
        public int MethodID { get; set; }
        public string MethodName { get; set; }
        public int GOBSDatasetID { get; set; }
        public string GOBSDatasetName { get; set; }
        public int InventoryID { get; set; }
        public string InventoryName { get; set; }
    }
}
