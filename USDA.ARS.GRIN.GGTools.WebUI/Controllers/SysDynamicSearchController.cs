using NLog;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysDynamicSearchController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: DynamicQuery
        public ActionResult Index()
        {
            SysDynamicSearchViewModel viewModel = new SysDynamicSearchViewModel();

            // Load list of tables
            

            return View(viewModel);
        }

        /// <summary>
        /// Adds a new row to the search grid
        /// </summary>
        /// <returns></returns>
        public JsonResult AddSearchCriterion()
        {
            return null;
        }
        
        
    }
}