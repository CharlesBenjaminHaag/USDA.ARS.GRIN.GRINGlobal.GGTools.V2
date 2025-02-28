using NLog;
using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CooperatorMapController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _List(int cooperatorId = 0)
        {
            CooperatorMapViewModel viewModel = new CooperatorMapViewModel();

            try
            {
                viewModel.SearchEntity.CooperatorID = cooperatorId;
                viewModel.Search();
                return PartialView("~/Views/CooperatorMap/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(CooperatorMapViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        // GET: CooperatorMap
        public ActionResult Index()
        {
            try
            {
                CooperatorMapViewModel viewModel = new CooperatorMapViewModel();
                viewModel.PageTitle = "Cooperator Map Search";
                viewModel.TableName = "cooperator_map";
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        [HttpPost]
        public ActionResult Search(CooperatorMapViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/CooperatorMap/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
    }
}