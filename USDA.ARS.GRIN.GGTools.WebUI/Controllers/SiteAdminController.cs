using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SiteAdminController : Controller
    {
        // GET: SiteAdmin
        public ActionResult Index()
        {
            SiteAdminViewModel viewModel = new SiteAdminViewModel();
            return View(viewModel);
        }
    }
}