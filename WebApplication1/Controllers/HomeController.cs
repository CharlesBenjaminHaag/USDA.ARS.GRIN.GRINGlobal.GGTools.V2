using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    
    public class HomeController 
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Navigate(string applicationCode)
        {
            Session["APP_CONTEXT"] = applicationCode;
            switch (applicationCode)
            {
                case "GGT-TAX":
                    return RedirectToAction("Index", "Taxonomy");
                case "GGT-NRR":
                    return RedirectToAction("Index", "WebOrderRequest");
                case "GGT-CUR":
                    return RedirectToAction("Index", "AccessionInventoryAttachment");
                case "GGT-ARM":
                    return RedirectToAction("Explorer", "Cooperator");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
    
    }
}