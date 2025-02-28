using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.GOBS.DataLayer
{
    public class GOBSMarker : AppEntityBase
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
    }
}
