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
    public class SubfamilySearch : SearchEntityBase
    {
        
        public string Name { get; set; }
        public string SubfamilyName { get; set; }
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }
        public int InfraID { get; set; }
        public string InfraName { get; set; }
        public string FamilyIDList { get; set; }
    }
}
