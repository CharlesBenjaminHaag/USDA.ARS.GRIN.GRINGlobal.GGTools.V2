using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class AppUserDynamicQueryController : BaseController, IController<AppUserDynamicQueryViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int folderId)
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

        public ActionResult Edit(AppUserDynamicQueryViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: AppUserDynamicQuery
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(AppUserDynamicQueryViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult _List(string tableName = "")
        {
            AppUserDynamicQueryViewModel viewModel = new AppUserDynamicQueryViewModel();
            try
            {
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.SearchEntity.TableName = tableName;
                viewModel.Search();
                ModelState.Clear();
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }
    }
}