using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetMenu(string eventAction, string eventValue, int entityId = 0)
        {
            MenuViewModel viewModel = new MenuViewModel();
            string viewName = "~/Views/Components/_DefaultMenu.cshtml"; 
            viewModel.EntityID = entityId;
            viewModel.EventAction = eventAction;
            viewModel.EventValue = eventValue;

            eventAction = eventAction.ToLower();
            eventValue = eventValue.ToLower();

            //// Determine whether user can edit the table being accessed.
            SysUser authenticatedUser = AuthenticatedUser;
            //if (authenticatedUser.CanEdit(eventValue)) { }

            // Handle "Index" (home) pages
            if (eventValue == "index")
            {
                switch (eventAction)
                {
                    case "home":
                    case "taxonomy":
                        viewName = "~/Views/Components/_DefaultMenu.cshtml";
                        break;
                    case "species":
                        viewName = "~/Views/Taxonomy/Species/Components/_SearchMenu.cshtml";
                        break;
                    case "geographymap":
                        viewName = "~/Views/Taxonomy/GeographyMap/Components/_SearchMenu.cshtml";
                        break;
                    default:
                        viewName = "~/Views/Components/_DefaultSearchMenu.cshtml";
                        break;
                }
            }



            if ((eventValue == "add") || (eventValue == "edit"))
            {
                if ((eventAction == "family") || (eventAction == "genus") || (eventAction == "species"))
                {
                    // Load context-based menu for the current taxon type.
                    viewName = "~/Views/Taxonomy/" + eventAction + "/Components/_EditMenu.cshtml";
                }
                else
                {
                    if (eventAction == "classification")
                    {
                        viewName = "~/Views/Taxonomy/Order/Components/_EditMenu.cshtml";
                    }
                    else 
                    {
                        if (eventAction == "sysfolder")
                        {
                            viewName = "~/Views/Components/_DefaultMenu.cshtml";
                        }
                        else
                        {
                            viewName = "~/Views/Components/_DefaultEditMenu.cshtml";
                        }
                    }
                }
            }
            return PartialView(viewName, viewModel);
        }
    }
}