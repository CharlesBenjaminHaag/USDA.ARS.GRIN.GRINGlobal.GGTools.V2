using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysDynamicSearch : AppEntityBase
    {
       
        public List<SysDynamicSearchCriterion> SysDynamicSearchCriteria { get; set; }

        public SysDynamicSearch() 
        { 
            this.SysDynamicSearchCriteria = new List<SysDynamicSearchCriterion>();
        }
    }
}
