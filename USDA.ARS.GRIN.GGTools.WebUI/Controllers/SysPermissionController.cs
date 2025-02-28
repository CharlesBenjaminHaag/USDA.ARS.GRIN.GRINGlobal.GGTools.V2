using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SysPermissionController : BaseController, IController<SysPermission>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Explorer()
        {
            try 
            { 
                SysPermissionViewModel viewModel = new SysPermissionViewModel();
                viewModel.Search();
                return View("~/Views/SysPermission/Explorer/Index.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            try
            {
                return PartialView("~/Views/Shared/_UnderConstruction.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(SysPermissionViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: SysPermissionUserMap
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(SysPermissionViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult _List(int sysGroupId = 0)
        {
            SysPermissionViewModel viewModel = new SysPermissionViewModel();            
            try
            {
                viewModel.SearchEntity.SysGroupID = sysGroupId;
                viewModel.Search();
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _SelectList(int sysUserId = 0, int sysGroupId = 0)
        {
            SysPermissionViewModel viewModel = new SysPermissionViewModel();
            try
            {
                //TODO
                viewModel.Search();
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Search(SysPermission viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(SysPermission viewModel)
        {
            throw new NotImplementedException();
        }
    }
}