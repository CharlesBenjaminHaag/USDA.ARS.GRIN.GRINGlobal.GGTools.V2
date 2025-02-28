using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USDA.ARS.GRIN.GGTools.AppLayer
{
    public class SearchEntityBase
    {
        public string Environment { get; set; }
        public int FolderID { get; set; }
        public int? ID { get; set; }
        public int? ExcludeID { get; set; }
        public string IDList { get; set; }
        public string AssembledName { get; set; }
        public string TableName { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public DateTime CreatedDateFrom { get; set; }
        public DateTime CreatedDateTo { get; set; }
        public string CreatedDateText { get; set; }

        public int CreatedByCooperatorID { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime ModifiedDateFrom { get; set; }
        public DateTime ModifiedDateTo { get; set; }
        public string ModifiedDateText { get; set; }

        public int ModifiedByCooperatorID { get; set; }
        public DateTime? OwnedDate { get; set; }
        public int OwnedByCooperatorID { get; set; }
        public int OwnedByCooperatorSiteID { get; set; }
        public string DateRangeFilter { get; set; }
        public string Note { get; set; }
        public int CitationID { get; set; }

        [AllowHtml]
        public string CitationText { get; set; }
        
        [AllowHtml]
        public string SQLStatement { get; set; }
        public string SQLWhere { get; set; }
        public string SearchTitle
        { get; set; }
        
        public string SearchDescription
        { get; set; }
        
    }
}
