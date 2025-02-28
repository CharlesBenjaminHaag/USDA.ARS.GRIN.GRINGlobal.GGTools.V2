using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class ReportViewModel
    {
        public string ReportTitle { get; set; }
        public string ReportDescription { get; set; }
        public string ReportPartialViewName { get; set; }

        public System.Data.DataTable ReportDataTable = new System.Data.DataTable();

        public void GetReport(string code)
        {
            ReportPartialViewName = "~/Views/Reports/Partial/_DynamicList.cshtml";
            switch (code)
            {
                case "WORORD":
                    ReportTitle = "Web Order Troubleshooting";
                    ReportDescription = "Cross-references web order requests and related order requests to identify inconsistent statuses.";
                    //ReportPartialViewName = "~/Views/Reports/Partial/_OrderTroubleshootingList.cshtml";
                    break;
                case "NRRPOFF":
                    ReportTitle = "NRR Past Offenders";
                    break;
                case "NRRPREV":
                    ReportTitle = "NRR Prevention";
                    ReportDescription = "A list of all Web Order Requests that were flagged as NRRs, and then rejected and cancelled.";
                    break;
                case "WOSTAT":
                    ReportTitle = "Web Order Statistics (2022)";
                    break;
            }
            System.Data.DataTable dtReport = new System.Data.DataTable();
            using (ReportManager mgr = new ReportManager())
            {
                ReportDataTable = mgr.GetReport(code);
            }
        }
    }
}
