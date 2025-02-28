using NLog;
using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CooperatorWizardController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Home(int cooperatorId = 0)
        {
            CooperatorWizardViewModel viewModel = new CooperatorWizardViewModel();
            return View("~/Views/Cooperator/Wizard/Index.cshtml", viewModel);
        }
        public PartialViewResult RenderCooperatorWidget(int cooperatorId = 0)
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            viewModel.Get(cooperatorId);
            return PartialView("~/Views/Cooperator/Wizard/_Step1.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult EditCooperator(CooperatorViewModel viewModel)
        {
            if (viewModel.Entity.ID == 0)
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.StatusCode = "PENDING";
                viewModel.Insert();
            }
            else
            {
                viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Update();
            }

            CooperatorWizardViewModel cooperatorWizardViewModel = new CooperatorWizardViewModel();
            cooperatorWizardViewModel.CooperatorID = viewModel.Entity.ID;
            return View("~/Views/Cooperator/Wizard/Index.cshtml", cooperatorWizardViewModel);
        }

        public PartialViewResult RenderSysUserWidget(int cooperatorId, int sysUserId = 0)
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.Entity.CooperatorID = cooperatorId;
            viewModel.Entity.UserName = "example.name"; 
            return PartialView("~/Views/Cooperator/Wizard/_Step2.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult EditSysUser(SysUserViewModel viewModel)
        {
            if (viewModel.Entity.ID == 0)
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.IsEnabled = "N";
                viewModel.Entity.SysUserName = viewModel.Entity.UserName;
                viewModel.Entity.SysUserPassword = viewModel.Entity.Password;
                viewModel.Entity.SysUserPasswordConfirm = viewModel.Entity.PasswordConfirm;
                viewModel.Insert();
            }
            else
            {
                viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Update();
            }

            CooperatorWizardViewModel cooperatorWizardViewModel = new CooperatorWizardViewModel();
            cooperatorWizardViewModel.CooperatorID = viewModel.Entity.CooperatorID;
            cooperatorWizardViewModel.SysUserID = viewModel.Entity.ID;
            return View("~/Views/Cooperator/Wizard/Index.cshtml", cooperatorWizardViewModel);

        }

        public PartialViewResult RenderSysGroupUserMapWidget(int sysUserId = 0)
        {
            SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
            //viewModel.Edit(sysGroupUserMapId);
            return PartialView("~/Views/Cooperator/Wizard/_Step3.cshtml", viewModel);

        }
    }
}