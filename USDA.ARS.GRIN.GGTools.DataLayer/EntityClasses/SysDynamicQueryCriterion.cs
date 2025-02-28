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
    public class SysDynamicQueryCriterion : AppEntityBase
    {
        public string SysTableFieldName { get; set; }
        public string SysTableFieldTitle { get; set; }
        public string ComparisonOperatorValue { get; set; }
        public string ComparisonOperatorTitle { get; set; }
        public string SearchCriterionValue { get; set; }
    }
}
