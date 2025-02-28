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
    public class SysDynamicQuery : AppEntityBase
    {
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string DataSource { get; set; }
        [AllowHtml]
        public string SQLStatement { get; set; }
        public string IsFavorite { get; set; }
        public bool IsFavoriteOption { get; set; }
        public List<SysDynamicQueryCriterion> SysDynamicQueryCriteria { get; set; }
    }
}
