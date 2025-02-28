using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysDynamicSearchViewModel
    {
        public Collection<SysTable> DataCollectionSysTables = new Collection<SysTable>();
        public Collection<SysDynamicSearchCriterion> DataCollectionSearchCriteria = new Collection<SysDynamicSearchCriterion>();
        public SelectList SysTables;
        public string SysTableName;

        public SysDynamicSearchViewModel()
        {
            using (SysTableManager mgr = new SysTableManager())
            {
                SysTables = new SelectList(mgr.GetSysTablesTaxonomy(), "SysTableName", "SysTableTitle");
            }
        }

        public void AddSearchCriterion()
        {
            
        }

        
    }
}
