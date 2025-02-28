using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.GOBS.DataLayer
{
    public class GOBSType : AppEntityBase
    {
        public string Name { get; set; }
        public string Validator { get; set; }
        public string Message { get; set; }
    }
}
