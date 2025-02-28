using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult PageMenu(string eventAction, string eventValue, string sysTableName = "", string sysTableTitle = "")
        {
            return null;
        }

            [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ErrorLogViewModel viewModel = new ErrorLogViewModel();
                viewModel.PageTitle = "Error Logs";
                viewModel.TableCode = "sys_db_error";
                viewModel.Search();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        [HttpGet]
        public ActionResult _Error()
        {
            var exception = Server.GetLastError();
            return PartialView();
        }

        [HttpGet]
        public ActionResult InternalServerError()
        {
            var exception = Server.GetLastError();
            return View();
        }

        [HttpGet]
        public ActionResult SystemError()
        {
            var exception = Server.GetLastError();
            return View();
        }

        [HttpGet]
        public ActionResult _InternalServerError()
        {
            var exception = Server.GetLastError();
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View("~/Views/Error/Error404.cshtml");
        }
    }
}
