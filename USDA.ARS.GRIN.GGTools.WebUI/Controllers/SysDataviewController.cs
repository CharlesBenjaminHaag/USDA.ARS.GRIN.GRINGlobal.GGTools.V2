using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SysDataviewController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            SysDataViewViewModel viewModel = null;
            try
            {
                viewModel = new SysDataViewViewModel();
                return View("~/Views/SysDataview/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
            return View(viewModel);
        }

        public PartialViewResult GetSidebar()
        {
            try
            {
                return PartialView("~/Views/Shared/Sidebars/_MainSidebarSysDataView.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult GetMenuList()
        {
            try
            {
                SysDataViewViewModel viewModel = new SysDataViewViewModel();
                viewModel.GetAll();
                return PartialView("~/Views/SysDataView/Components/_MenuList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult GetParameterList(int sysDataViewId)
        {
            try
            {
                SysDataViewViewModel viewModel = new SysDataViewViewModel();
                viewModel.GetParameters(sysDataViewId);
                return PartialView("~/Views/SysDataView/Components/_ParameterList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult GetFieldList(int sysDataViewId)
        {
            try
            {
                SysDataViewViewModel viewModel = new SysDataViewViewModel();
                viewModel.GetFields(sysDataViewId);
               //TODO
                return PartialView("~/Views/SysDataView/Components/_FieldList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult GetSQL(int sysDataViewId)
        {
            try
            {
                SysDataViewViewModel viewModel = new SysDataViewViewModel();
                viewModel.GetSQL(sysDataViewId);
                //TODO
                return PartialView("~/Views/SysDataView/Components/_FieldList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}