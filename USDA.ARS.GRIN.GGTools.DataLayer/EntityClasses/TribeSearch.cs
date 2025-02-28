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
    public class TribeSearch : SearchEntityBase
    {
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public int SubfamilyID { get; set; }
        public string SubfamilyName { get; set; }
        public int TribeID { get; set; }
        public string TribeName { get; set; }
    }
}
