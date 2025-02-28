using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USDA.ARS.GRIN.WebUI.Controllers
{
    public class PortalController : Controller
    {
        // GET: Portal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult AccountCreation()
        {
            return View();
        }
        public ActionResult AccountDeactivation()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginSubmit()
        {
            return View();
        }

    }
}