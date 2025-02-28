using NLog;
using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class TaxonomyController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();
            viewModel.AuthenticatedUser = AuthenticatedUser;
            //return Edit("~/Views/Taxonomy/Home/Index.cshtml", viewModel);
            return RedirectToAction("Explorer","SysFolder");
        }
        public ActionResult Admin(int sysUserId = 0)
        {
            SysUserViewModel viewModel = new SysUserViewModel();

            try
            {
                return View("~/Views/Taxonomy/Admin/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult RenderSidebar()
        {
            return PartialView("~/Views/Shared/Sidebars/_MainSidebarTaxonomy.cshtml");
        }
    }
}