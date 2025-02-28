using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public PartialViewResult SetNavigation(string controller, string action)
        {
            string actionString = controller;

            if (!String.IsNullOrEmpty(action))
            {
                actionString += action;
            }
            
            string partialViewName = String.Empty;
            switch (actionString)
            {
                case "SpeciesEdit":
                    partialViewName = "~/Views/Taxonomy/Species/Partial/_MenuEdit.cshtml";
                    break;
                default:
                    partialViewName = "~/Views/Shared/Partial/_MenuIndex.cshtml";
                    break;
            }
            return PartialView(partialViewName);
        }
    }
}