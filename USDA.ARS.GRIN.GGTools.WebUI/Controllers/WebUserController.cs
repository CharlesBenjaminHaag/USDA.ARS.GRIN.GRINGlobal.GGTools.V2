using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class WebUserController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _Get(int entityId, string environment = "")
        {
            try
            {
                WebUserViewModel viewModel = new WebUserViewModel();
                viewModel.Get(entityId);
                return PartialView("~/Views/WebUser/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Copy(WebUserViewModel viewModel)
        {
             
            viewModel.Copy();
            return _Get(viewModel.Entity.ID);
        }
        public PartialViewResult Save(WebUserViewModel viewModel)
        {
            try
            {
                //if (!viewModel.Validate())
                //{
                //    if (viewModel.ValidationMessages.Count > 0) return Edit(viewModel);
                //}

                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Insert();
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    //viewModel.Update();
                }
                return _Get(viewModel.Entity.ID, "");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        // ******************************************************************************
        // BEGIN OLD CODE
        // ******************************************************************************

        [HttpPost]
        public PartialViewResult Add(FormCollection formCollection)
        {
            //SysUserViewModel viewModel = new SysUserViewModel();

            //viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            //if (!String.IsNullOrEmpty(formCollection["CooperatorID"]))
            //{
            //    viewModel.Entity.CooperatorID = Int32.Parse(formCollection["CooperatorID"]);
            //}

            //if (!String.IsNullOrEmpty(formCollection["SysUserName"]))
            //{
            //    viewModel.Entity.UserName = formCollection["SysUserName"];
            //}

            //if (!String.IsNullOrEmpty(formCollection["SysUserPassword"]))
            //{
            //    viewModel.Entity.Password = formCollection["SysUserPassword"];
            //    viewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.Password;
            //}
            //viewModel.Insert();

            //// Retrieve new account
            SysUserViewModel confirmationViewModel = new SysUserViewModel();
            //confirmationViewModel.SearchEntity.ID = viewModel.Entity.ID;
            //confirmationViewModel.Search();
            //confirmationViewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.SysUserPlainTextPassword;
            //confirmationViewModel.SendNotification("N");
            return PartialView("~/Views/SysUser/_Widget.cshtml", confirmationViewModel);
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
        throw new NotImplementedException();
        }
        public PartialViewResult _Edit(int entityId, string environment = "")
        {
            try
            {
                WebUserViewModel viewModel = new WebUserViewModel();
                viewModel.Get(entityId, environment);
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        [HttpPost]
            public PartialViewResult ResetPassword(FormCollection formCollection)
            {
                //SysUserViewModel viewModel = new SysUserViewModel();

                //viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //if (!String.IsNullOrEmpty(formCollection["SysUserID"]))
                //{
                //    viewModel.Entity.SysUserID = Int32.Parse(formCollection["SysUserID"]);
                //}

                //if (!String.IsNullOrEmpty(formCollection["SysUserName"]))
                //{
                //    viewModel.Entity.UserName = formCollection["SysUserName"];
                //}

                //if (!String.IsNullOrEmpty(formCollection["SysUserPassword"]))
                //{
                //    viewModel.Entity.Password = formCollection["SysUserPassword"];
                //    viewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.Password;
                //}
                //viewModel.UpdatePassword();

                //// Retrieve updated account
                SysUserViewModel confirmationViewModel = new SysUserViewModel();
                //confirmationViewModel.SearchEntity.ID = viewModel.Entity.SysUserID;
                //confirmationViewModel.Search();
                //confirmationViewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.SysUserPlainTextPassword;
                //confirmationViewModel.SendNotification("P");
                return PartialView("~/Views/SysUser/_Widget.cshtml", confirmationViewModel);
            }

            [HttpPost]
            public ActionResult Edit(SysUserViewModel viewModel)
            {
                throw new NotImplementedException();
            }

            // GET: SysUser
            public ActionResult Index()
            {
                // Insert sys user, linked to this coooperator.
                SysUserViewModel sysUserViewModel = new SysUserViewModel();
                //sysUserViewModel.Entity = new SysUser { CooperatorID = viewModel.Entity.ID, UserName = viewModel.Entity.SysUserName, Password = viewModel.Entity.SysUserPassword };
                //sysUserViewModel.Insert();

                return View();
            }

            public ActionResult Search(SysUserViewModel viewModel)
            {
                throw new NotImplementedException();
            }

            public PartialViewResult RenderWidget(int entityId)
            {
                WebUserViewModel viewModel = new WebUserViewModel();

                if (entityId > 0)
                {
                    viewModel.SearchEntity.ID = entityId;
                    viewModel.Search();
                }
                return PartialView("~/Views/WebUser/_Widget.cshtml", viewModel);

            }
            public PartialViewResult RenderGroupWidget(int entityId)
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                if (entityId > 0)
                {
                    viewModel.SearchEntity.ID = entityId;
                    viewModel.GetGroups(entityId);
                }
                return PartialView("~/Views/SysUser/Group/_Widget.cshtml", viewModel);
            }

            public PartialViewResult RenderEditModal()
            {
                return PartialView("~/Views/SysUser/Modals/_Edit.cshtml");
            }
            public PartialViewResult RenderPasswordResetModal()
            {
                return PartialView("~/Views/SysUser/Modals/_ResetPassword.cshtml");
            }
        
    }
}