using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SynchController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Index(int entityId)
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            viewModel.SearchEntity.ID = entityId;
            viewModel.Search();
            return View("~/Views/Cooperator/Synch/Index.cshtml", viewModel);
        }
        public PartialViewResult _RenderCooperatorEdit(int entityId, string environment = "")
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.EventValue = environment;
                viewModel.Get(entityId);
                return PartialView("~/Views/Cooperator/Synch/_EditCooperator.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _RenderSysUserEdit(int entityId, string environment = "")
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                viewModel.EventValue = environment;
                viewModel.Get(entityId);
                return PartialView("~/Views/Cooperator/Synch/_EditSysUser.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _RenderWebCooperatorEdit(int entityId, string environment = "")
        {
            try
            {
                WebCooperatorViewModel viewModel = new WebCooperatorViewModel();
                viewModel.EventValue = environment;
                viewModel.Get(entityId);
                return PartialView("~/Views/Cooperator/Synch/_EditWebCooperator.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _RenderWebUserEdit(int entityId, string environment = "")
        {
            try
            {
                WebUserViewModel viewModel = new WebUserViewModel();
                viewModel.EventValue = environment;
                viewModel.Get(entityId);
                return PartialView("~/Views/Cooperator/Synch/_EditWebUser.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}