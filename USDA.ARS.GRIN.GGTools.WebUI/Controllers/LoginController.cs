using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Display the login window.
        /// </summary>
        ///    /// returns></returns>
        public ActionResult Index()
        {
            SysUserViewModel vm = new SysUserViewModel();

            ViewBag.PageTitle = "GGTools Login";

            return View(vm);
        }

        /// <summary>
        /// Edit user credentials.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SysUserViewModel viewModel) 
        {
            bool isAuthenticated = false;
            try
            {
                if (!viewModel.IsAlphaNumeric(viewModel.Entity.UserName))
                {
                    ModelState.Clear();
                    viewModel.UserMessage = String.Format("User name invalid.");
                    return View(viewModel);
                }
                isAuthenticated = viewModel.Authenticate();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                viewModel.UserMessage = ex.Message;
                return View(viewModel);
            }

            if (viewModel.Entity.IsAuthenticated)
            {
                AuthenticatedUserSession authenticatedUserSession = new AuthenticatedUserSession(viewModel.Entity);
                Session["AUTHENTICATED_USER_SESSION"] = authenticatedUserSession;
                return View("~/Views/Login/Attestation.cshtml");
            }
            else
            {
                return View(viewModel);
            }
        }

        //public ActionResult Portal()
        //{
        //    return Edit("~/Views/Login/Portal/Index.cshtml");
        //}

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        
        [HttpPost]
        public ActionResult Confirm()
        {
            return RedirectToAction("Index", "Home");
        }
        
        /// <summary>
        /// Prompts user for user name.
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestPasswordReset()
        {
            Session.Clear();
            SysUserViewModel viewModel = new SysUserViewModel();
            return View(viewModel);
        }
        
        /// <summary>
        /// User submits user name. Application then:
        /// 1) Verifies that name exists
        /// 2) Generates reset token
        /// 3) Sends token in email
        /// 4) Displays confirmation page
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestPasswordReset(SysUserViewModel viewModel)
        {
            try
            {
                viewModel.SearchEntity.UserName = viewModel.Entity.UserName;
                if (String.IsNullOrEmpty(viewModel.SearchEntity.UserName))
                {
                    viewModel.UserMessage = String.Format("Please enter your GRIN-Global user name.");
                    ModelState.Clear();
                    return View(viewModel);
                }

                if (!viewModel.IsAlphaNumeric(viewModel.Entity.UserName))
                {
                    viewModel.UserMessage = String.Format("User name invalid.");
                    ModelState.Clear();
                    return View(viewModel);
                }

                viewModel.Search();

                if (viewModel.Entity.ID == 0)
                {
                    viewModel.UserMessage = String.Format("The user <strong>{0}</strong> could not be found. Please verify that: <ul><li>You have an active GRIN-Global account.</li><li>You have entered your username exactly as you do when logging into the Curator Tool.</li></ul>", viewModel.Entity.UserName);
                    ModelState.Clear();
                    return View(viewModel);
                }

                viewModel.GeneratePasswordResetToken(viewModel.Entity.SysUserName);
                return View("~/Views/Login/Confirmation.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("SystemError", "Error");
            }
        }
        
        public ActionResult ResetPassword(string token = "")
        {
            SysUserViewModel viewModel = new SysUserViewModel();

            if (!String.IsNullOrEmpty(token))
            {
                viewModel.Entity = viewModel.ValidatePasswordResetToken(token);
                viewModel.PasswordResetToken = token;
                viewModel.Entity.PasswordResetToken = token;
                if (viewModel.Entity.ID > 0)
                {
                    return View("~/Views/Login/ResetPasswordConfirm.cshtml", viewModel);
                }
                else
                {
                    viewModel.UserMessage = "The reset token has expired.";
                    return View("~/Views/Login/Error.cshtml", viewModel);
                }
            }
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(SysUserViewModel viewModel)
        {
            if (!viewModel.Validate())
            {
                ModelState.Clear();
                return View("~/Views/Login/ResetPasswordConfirm.cshtml", viewModel);
            }

            viewModel.UpdatePassword();
            viewModel.SearchEntity.ID = viewModel.Entity.ID;
            viewModel.Search();
            if (viewModel.Entity.IsInRole("GGTOOLS_ALLUSERS"))
            {
                return RedirectToAction("Index", "Login");
            }
            else 
            {
                ViewData["PW_EXPIRATION_DATE"] = viewModel.Entity.SysUserPasswordExpirationDate.ToString();
                return View("~/Views/Login/ResetPasswordFinal.cshtml");
            }
        }
        
        /// <summary>
        /// User has entered CT ID and requested reset
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult ResetPassword(SysUserViewModel viewModel)
        //{
        //    viewModel.GeneratePasswordResetToken(viewModel.Entity.UserName);
        //    if (!String.IsNullOrEmpty(viewModel.UserMessage))
        //    {
        //        return Edit(viewModel);
        //    }
        //    else
        //    {
        //        return Edit("~/Views/Login/Confirmation.cshtml");
        //    }
        //}

        //[HttpPost]
        //public ActionResult SaveNewPassword(SysUserViewModel viewModel)
        //{
        //    if (!viewModel.Validate())
        //    {
        //        return Edit("~/Views/Login/ResetPassword.cshtml", viewModel);
        //    }
        //    viewModel.UpdatePassword();
        //    return RedirectToAction("Index", "Login");
        //}
        public ActionResult AccountRequest()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            return View("~/Views/Login/AccountRequest.cshtml", viewModel);
        }
    }
}