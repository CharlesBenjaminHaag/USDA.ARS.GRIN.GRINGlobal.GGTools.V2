using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class ReportsController : BaseController
    {
        // GET: Reports
        public ActionResult Index(string reportCode = "")
        {
            ReportViewModel viewModel = new ReportViewModel();
            viewModel.GetReport(reportCode);
            return View(viewModel);
        }
    }
}