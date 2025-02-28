using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class ExplorationSearch: SearchEntityBase
    {
        public string ExplorationNumber { get; set; }
        public string Title { get; set; }
        public DateTime BeganDate { get; set; }
        public DateTime FinishedDate { get; set; }
        public string FundingSource { get; set; }
        public decimal  FundingAmount { get; set; }
        public string TargetSpecies { get; set; }
        public string Permits { get; set; }
        public string Restrictions { get; set; }
        public int FiscalYear { get; set; }
        public int HostCooperatorID { get; set; }
        public string HostCooperatorName { get; set; }
    }
}
