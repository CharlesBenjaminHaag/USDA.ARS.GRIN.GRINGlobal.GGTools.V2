using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class MenuViewModel
    {
        public int EntityID = 0;
        public string EventAction = String.Empty;
        public string EventValue = String.Empty;
    }
}
