using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class Author: AppEntityBase
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string ShortNameExpandedDiacritic { get; set; }
        public string FullNameExpandedDiacritic { get; set; }
        public string ShortNameExactMatch { get; set; }
        public string AuthorityText { get; set; }
    }
}
