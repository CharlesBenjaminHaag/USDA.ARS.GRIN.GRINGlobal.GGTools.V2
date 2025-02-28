using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USDA.ARS.GRIN.GRINGlobal.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "GRIN-Global API (Beta)";
            @TempData["PAGE_CONTEXT"] = "API (Beta)";
            return View();
        }
    }
}
