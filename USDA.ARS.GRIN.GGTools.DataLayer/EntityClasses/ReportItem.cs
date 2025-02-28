using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class ReportItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Label2 { get; set; }
        public int Total { get; set; }
        public decimal ItemPercentage { get; set; }
        public string CSSClass { 
            get
            { 
                switch(Title)
                {
                    case "NRR_FLAG":
                        return "bg-red";
                    case "CANCELED":
                        return "bg-yellow";
                    case "ACCEPTED":
                        return "bg-green";
                    case "SUBMITTED":
                        return "bg-purple";
                    default:
                        return "";
                }
            }
        }
        public string IconName
        {
            get
            {
                switch (Title)
                {
                    case "NRR_FLAG":
                        return "fa-warning";
                    case "CANCELED":
                        return "fa-thumbs-down";
                    case "ACCEPTED":
                        return "fa-thumbs-up";
                    case "SUBMITTED":
                        return "fa-info";
                    default:
                        return "";
                }
            }
        }
    }
}
