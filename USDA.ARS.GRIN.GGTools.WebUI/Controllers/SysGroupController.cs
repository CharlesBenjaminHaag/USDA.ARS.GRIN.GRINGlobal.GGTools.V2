using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SysGroupController : BaseController, IController<SysGroup>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        #region Explorer

        public ActionResult Explorer()
        {
            try
            {
                return View("~/Views/SysGroup/Explorer/Index.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult Explorer_ListSysGroups()
        {
            try
            {
                SysGroupViewModel viewModel = new SysGroupViewModel();
                viewModel.Search();
                return PartialView("~/Views/SysGroup/Explorer/_ListSysGroups.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Explorer_Detail(int sysGroupId)
        {
            SysGroupViewModel viewModel = new SysGroupViewModel();
            try
            {
                viewModel.SearchEntity.ID = sysGroupId;
                viewModel.Get(sysGroupId);
                
                return PartialView("~/Views/SysGroup/Explorer/_Detail.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Explorer_Filter(string searchText) 
        {
            try
            {
                SysGroupViewModel viewModel = new SysGroupViewModel();
                viewModel.SearchEntity.GroupTitle = searchText;
                viewModel.Search();
                return PartialView("~/Views/SysGroup/Explorer/_ListSysGroups.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #endregion

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

        public ActionResult Edit(SysGroupUserMap viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: SysGroupUserMap
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(SysGroupUserMap viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult _List()
        {
            SysGroupViewModel viewModel = new SysGroupViewModel();            
            try
            { 
                viewModel.Search();
                return PartialView("~/Views/SysGroup/_SelectListSimple.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _SelectList(int sysUserId = 0, int sysGroupId = 0)
        {
            SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
            try
            {
                viewModel.SearchEntity.SysUserID = sysUserId;
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

        public ActionResult Search(SysGroup viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(SysGroup viewModel)
        {
            throw new NotImplementedException();
        }
        
    }
}