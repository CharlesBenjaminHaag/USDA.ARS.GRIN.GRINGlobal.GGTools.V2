using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class AppUserItemFolderCooperatorMapController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: AppUserItemFolderCooperatorMap
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Add(AppUserItemFolderCooperatorMapViewModel viewModel)
        {
            try 
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.InsertBatch();
                return Json("success", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(AppUserItemFolderCooperatorMapViewModel viewModel)
        {
            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.DeleteBatch();
                return Json("success", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult _List()
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            try 
            { 
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Search();
                return PartialView("~/Views/AppUserItemFolder/_ListWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderEditModal(int appUserItemFolderId)
        {
            AppUserItemFolderCooperatorMapViewModel viewModel = new AppUserItemFolderCooperatorMapViewModel();

            try
            {
                viewModel.SearchEntity.FolderID = sysFolderId;
                viewModel.GetMapped();
                viewModel.GetNonMapped();
                return PartialView("~/Views/AppUserItemFolderCooperatorMap/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderWidget(int appUserItemFolderId)
        {
            try
            {
                AppUserItemFolderCooperatorMapViewModel viewModel = new AppUserItemFolderCooperatorMapViewModel();
                viewModel.SearchEntity.FolderID = sysFolderId;
                viewModel.GetNonMapped();
                viewModel.GetMapped();
                return PartialView("~/Views/AppUserItemFolderCooperatorMap/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}