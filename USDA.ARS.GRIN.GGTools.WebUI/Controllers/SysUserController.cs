using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysUserController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _Get(int sysUserId = 0, int cooperatorId = 0)
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                viewModel.Get(sysUserId, cooperatorId);
                viewModel.Entity.CooperatorID = cooperatorId;
                return PartialView("~/Views/SysUser/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public PartialViewResult Save(SysUserViewModel viewModel)
        //{
        //    try
        //    {
        //        //if (!viewModel.Validate())
        //        //{
        //        //    if (viewModel.ValidationMessages.Count > 0) return PartialView(viewModel);
        //        //}

        //        viewModel.Entity.Password = viewModel.Entity.SysUserPassword;
        //        viewModel.Entity.PasswordConfirm = viewModel.Entity.SysUserPasswordConfirm;

        //        if (viewModel.Entity.SysUserPassword != viewModel.Entity.SysUserPasswordConfirm)
        //        {
        //            viewModel.UserMessage = "The passwords that you have entered do not match.";
        //            viewModel.ValidationMessages.Add(new Common.Library.ValidationMessage { Message = viewModel.UserMessage });
        //            if (viewModel.ValidationMessages.Count > 0) return PartialView("~/Views/SysUser/_Edit.cshtml", viewModel);
        //        }

        //        if (viewModel.Entity.SysUserID == 0)
        //        {
        //            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
        //            viewModel.Insert();
        //        }
        //        else
        //        {
        //            viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
        //            viewModel.Update();
        //        }
        //        return _Get(viewModel.Entity.SysUserID, viewModel.Entity.CooperatorID);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        // ******************************************************************************
        // BEGIN OLD CODE
        // ******************************************************************************

        [HttpPost]
        public PartialViewResult Add(FormCollection formCollection)
        {
            SysUserViewModel viewModel = new SysUserViewModel();

            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            if (!String.IsNullOrEmpty(formCollection["CooperatorID"]))
            {
                viewModel.Entity.CooperatorID = Int32.Parse(formCollection["CooperatorID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["SysUserName"]))
            {
                viewModel.Entity.UserName = formCollection["SysUserName"];
                viewModel.Entity.SysUserName = viewModel.Entity.UserName;
            }

            if (!String.IsNullOrEmpty(formCollection["SysUserPassword"]))
            {
                viewModel.Entity.Password = formCollection["SysUserPassword"];
                viewModel.Entity.SysUserPassword = viewModel.Entity.Password;
                viewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.Password;
            }

            if (!String.IsNullOrEmpty(formCollection["SysUserPasswordConfirm"]))
            {
                viewModel.Entity.SysUserPasswordConfirm = formCollection["SysUserPasswordConfirm"];
            }

            viewModel.Insert();

            // Retrieve new account
            SysUserViewModel confirmationViewModel = new SysUserViewModel();
            confirmationViewModel.SearchEntity.ID = viewModel.Entity.ID;
            confirmationViewModel.Search();
            confirmationViewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.SysUserPlainTextPassword;
            confirmationViewModel.SendNotification("N");
            return PartialView("~/Views/SysUser/_Widget.cshtml", confirmationViewModel);
        }

        public PartialViewResult _Add(int cooperatorId)
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                viewModel.Entity.CooperatorID = cooperatorId;
                return PartialView("~/Views/SysUser/_Edit.cshtml", viewModel);
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

        public ActionResult Edit(int entityId)
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.AuthenticatedUser = AuthenticatedUser;
                    viewModel.DisplayUserName = viewModel.Entity.SysUserName;
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _Edit(int sysUserId)
        {
            try 
            { 
                SysUserViewModel viewModel = new SysUserViewModel();
                viewModel.Get(sysUserId);
                return PartialView("~/Views/SysUser/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult _Save(SysUserViewModel viewModel)
        {
            try
            {
                if (viewModel.Entity.ID > 0)
                {
                    viewModel.Update();
                }
                else
                {
                    viewModel.Insert();
                }
                return PartialView("~/Views/SysUser/_Edit.cshtml", viewModel);
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
            SysUserViewModel viewModel = new SysUserViewModel();

            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            if (!String.IsNullOrEmpty(formCollection["SysUserID"]))
            {
                viewModel.Entity.SysUserID = Int32.Parse(formCollection["SysUserID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["SysUserName"]))
            {
                viewModel.Entity.UserName = formCollection["SysUserName"];
            }

            if (!String.IsNullOrEmpty(formCollection["SysUserPassword"]))
            {
                viewModel.Entity.Password = formCollection["SysUserPassword"];
                viewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.Password;
            }
            viewModel.Update();

            // Retrieve updated account
            SysUserViewModel confirmationViewModel = new SysUserViewModel();
            confirmationViewModel.SearchEntity.ID = viewModel.Entity.SysUserID;
            confirmationViewModel.Search();
            confirmationViewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.SysUserPlainTextPassword;
            confirmationViewModel.SendNotification("P");
            return PartialView("~/Views/SysUser/_Widget.cshtml", confirmationViewModel);
        }

        [HttpPost]
        public ActionResult Edit(SysUserViewModel viewModel)
        {
            try
            {
                viewModel.AuthenticatedUser = AuthenticatedUser;
                if (!viewModel.Validate())
                {
                    return View("~/Views/SysUser/Edit.cshtml", viewModel);
                }
                viewModel.Update();
                if (viewModel.SendNotificationOption == true)
                {
                    viewModel.SendNotification("P");
                }
                return RedirectToAction("Edit", "SysUser", new {entityId = viewModel.Entity.ID});
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult _Edit(SysUserViewModel viewModel)
        {
            try
            {
                viewModel.AuthenticatedUser = AuthenticatedUser;
                if (!viewModel.Validate())
                {
                    return View("~/Views/SysUser/Edit.cshtml", viewModel);
                }
                viewModel.Update();
                if (viewModel.SendNotificationOption == true)
                {
                    viewModel.SendNotification("P");
                }
                return RedirectToAction("Edit", "SysUser", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        //public JsonResult _Save(SysUserViewModel viewModel)
        //{
        //    try
        //    {
        //        if (viewModel.Entity.ID > 0)
        //        {
        //            viewModel.Update();
        //            viewModel.SendNotification("P");
        //        }
        //        else
        //        {
        //            viewModel.Insert();
        //            viewModel.SendNotification("N");
        //        }
        //        viewModel.Edit(viewModel.Entity.ID);
        //        return Json(new { sysUser = viewModel.Entity }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        // GET: SysUser
        public ActionResult Index()
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.PageTitle = "Sys User Search";
            viewModel.TableName = "sys_user";
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
            return View(viewModel);
        }

        //public PartialViewResult _ListAvailableSysGroups(int sysUserId)
        //{
        //    SysUserViewModel viewModel = new SysUserViewModel();
        //    viewModel.GetAvailableSysGroups(sysUserId);
        //    return PartialView("~/Views/SysUser/Group/_ListAvailable.cshtml", viewModel);
        //}
        //public PartialViewResult _ListAssignedSysGroups(int sysUserId)
        //{
        //    SysUserViewModel viewModel = new SysUserViewModel();
        //    viewModel.GetAssignedSysGroups(sysUserId);
        //    return PartialView("~/Views/SysUser/Group/_ListAssigned.cshtml", viewModel);
        //}
        public PartialViewResult _ListTaxonomy()
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.GetTaxonomySysUsers();
                return PartialView("~/Views/SysUser/_ListTaxonomy.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    
        [HttpPost]
        public JsonResult AssignGroups(FormCollection formCollection)
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(formCollection["SysUserID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(formCollection["SysUserID"]);
                    viewModel.Entity.SysUserID = viewModel.Entity.ID; 
                }

                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.Entity.ItemIDList = formCollection["IDList"];
                }
                viewModel.AssignSysGroups();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UnAssignSysGroups(FormCollection formCollection)
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                if (!String.IsNullOrEmpty(formCollection["SysUserID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(formCollection["SysUserID"]);
                    viewModel.Entity.SysUserID = viewModel.Entity.ID;
                }

                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.Entity.ItemIDList = formCollection["IDList"];
                }
                viewModel.UnAssignSysGroups();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Search(SysUserViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult RenderWidget(int sysUserId) 
        {
            SysUserViewModel viewModel = new SysUserViewModel();

            if (sysUserId > 0)
            {
                viewModel.SearchEntity.ID = sysUserId;
                viewModel.Search();
                viewModel.AuthenticatedUser = AuthenticatedUser;
            }
            return PartialView("~/Views/SysUser/_Widget.cshtml", viewModel);

        }
        public PartialViewResult RenderSysGroupWidget(int sysUserId)
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            if (sysUserId > 0)
            {
                viewModel.SearchEntity.ID = sysUserId;
                viewModel.GetGroups(sysUserId);
                viewModel.AuthenticatedUser = AuthenticatedUser;
            }
            return PartialView("~/Views/SysUser/Group/_Widget.cshtml", viewModel);
        }

        public PartialViewResult RenderEditModal()
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.Entity.UserName = "";
            viewModel.Entity.Password = "";
            viewModel.Entity.PasswordConfirm = "";
            return PartialView("~/Views/SysUser/Modals/_Edit.cshtml", viewModel);
        }
        public PartialViewResult RenderPasswordResetModal()
        {
            return PartialView("~/Views/SysUser/Modals/_ResetPassword.cshtml");
        }

    }
}