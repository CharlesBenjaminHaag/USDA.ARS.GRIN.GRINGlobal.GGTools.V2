using NLog;
using System;
using System.Configuration;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult New()
        {
            ViewBag.PageTitle = "Curator Tool Account Request: Introduction";
            return View("~/Views/Account/Request/Introduction.cshtml");
        }
      
        //public ActionResult Step(int seq)
        //{ 
        //    switch(seq)
        //    {
        //        case 1:
        //            return RedirectToAction("Details", "Account");

        //        default:
        //            return Edit("~/Views/Account/Request/Introduction.cshtml");
        //    }
        //}

        public ActionResult Details()
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                return View("~/Views/Account/Request/Details.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Details(CooperatorViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    return View("~/Views/Account/Request/Details.cshtml", viewModel);
                }

                SiteViewModel siteViewModel = new SiteViewModel();
                siteViewModel.Get(viewModel.Entity.SiteID);

                viewModel.Entity.SiteID = siteViewModel.Entity.ID;
                viewModel.Entity.SiteName = siteViewModel.Entity.LongName;
                viewModel.Entity.AddressLine1 = siteViewModel.Entity.PrimaryAddress1;
                viewModel.Entity.AddressLine2 = siteViewModel.Entity.PrimaryAddress2;
                viewModel.Entity.AddressLine3 = siteViewModel.Entity.PrimaryAddress3;
                viewModel.Entity.City = siteViewModel.Entity.City;
                viewModel.Entity.StateName = siteViewModel.Entity.State;
                viewModel.Entity.GeographyID = siteViewModel.Entity.GeographyID;
                viewModel.Entity.PostalIndex = siteViewModel.Entity.PostalIndex;

                Session["NEW_ACCOUNT"] = viewModel;
                return RedirectToAction("Confirm", "Account");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
       
        public ActionResult Confirm()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                if (Session["NEW_ACCOUNT"] != null)
                {
                    viewModel = Session["NEW_ACCOUNT"] as CooperatorViewModel;
                }
                return View("~/Views/Account/Request/Confirm.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Confirm(CooperatorViewModel viewModel)
        {
            try
            {
                viewModel.Entity.StatusCode = "PENDING";
                viewModel.Entity.CreatedByCooperatorID = 48;
                viewModel.Insert();
                return View("~/Views/Account/Request/Final.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public ActionResult Final()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            return View("~/Views/Account/Request/Final.cshtml", viewModel);
        }
        [HttpPost]
        public ActionResult Final(CooperatorViewModel viewModel)
        {
            const string DEFAULT_COOPERATOR_STATUS = "PENDING";
            const string DEFAULT_WEB_COOPERATOR_IS_ACTIVE = "Y";
            const int DEFAULT_ADMIN_SYS_USER_ID = 48;
            const int DEFAULT_ADMIN_WEB_USER_ID = 1;
            const int SYS_GROUP_ID_ALLUSERS = 2;
            const int SYS_GROUP_ID_CTUSERS = 3;
            const int SYS_GROUP_ID_WEBTOOLS = 6;
            const string SYS_GROUP_TAG_GGTOOLS_MANAGE_COOPERATOR = "GGTOOLS_MANAGE_COOPERATOR";
            string defaultSysUserPassword = String.Empty;
            SMTPManager sMTPManager = new SMTPManager();
            SMTPMailMessage sMTPMailMessage = new SMTPMailMessage();
            SMTPMailMessage sMTPMailMessageCooperator = new SMTPMailMessage();
            SMTPMailMessage sMTPMailMessageCopy = new SMTPMailMessage();
            EmailTemplateViewModel emailTemplateViewModel = new EmailTemplateViewModel();
            CooperatorViewModel sessionCooperatorViewModel = null;
            CooperatorViewModel approvalListCooperatorViewModel = new CooperatorViewModel();
            SysUserViewModel sysUserViewModel = new SysUserViewModel();
            SysUserViewModel verificationSysUserViewModel = new SysUserViewModel();
            SysGroupUserMapViewModel sysGroupUserMapViewModel = new SysGroupUserMapViewModel();
            WebCooperatorViewModel webCooperatorViewModel = new WebCooperatorViewModel();
            WebUserViewModel webUserViewModel = new WebUserViewModel();

            defaultSysUserPassword = ConfigurationManager.AppSettings["DefaultSysUserPassword"];

            if (Session["NEW_ACCOUNT"] != null)
            {
                // Cooperator
                sessionCooperatorViewModel = Session["NEW_ACCOUNT"] as CooperatorViewModel;
                sessionCooperatorViewModel.Entity.StatusCode = DEFAULT_COOPERATOR_STATUS;
                sessionCooperatorViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                sessionCooperatorViewModel.Entity.WebCooperatorID = 0;
                sessionCooperatorViewModel.Entity.Organization = "Agricultural Research Service";
                sessionCooperatorViewModel.Entity.OrganizationAbbrev = "ARS";
                sessionCooperatorViewModel.Entity.CategoryCode = "UARS";
                sessionCooperatorViewModel.Insert();

                // Sys user
                if (sessionCooperatorViewModel.Entity.ID > 0)
                {
                    // Check for existing acct.
                    verificationSysUserViewModel.SearchEntity.UserName = sessionCooperatorViewModel.Entity.FirstName.ToLower() + "." + sessionCooperatorViewModel.Entity.LastName.ToLower();
                    verificationSysUserViewModel.Search();

                    if (verificationSysUserViewModel.Entity.ID > 0)
                    {
                        sysUserViewModel.Entity.UserName = verificationSysUserViewModel.Entity.UserName + DateTime.Now.Date.ToString();
                    }
                    else
                    {
                        sysUserViewModel.Entity.UserName = sessionCooperatorViewModel.Entity.FirstName.ToLower() + "." + sessionCooperatorViewModel.Entity.LastName.ToLower();
                    }

                    sysUserViewModel.Entity.SysUserName = sysUserViewModel.Entity.UserName;                   
                    sysUserViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                    sysUserViewModel.Entity.Password = sysUserViewModel.GetSecurePassword(defaultSysUserPassword);
                    sysUserViewModel.Entity.SysUserPassword = sysUserViewModel.Entity.Password;
                    sysUserViewModel.Entity.CooperatorID = sessionCooperatorViewModel.Entity.ID;
                    sysUserViewModel.Entity.IsEnabled = "N";
                    sysUserViewModel.Insert();
                }

                // Groups
                if (sysUserViewModel.Entity.ID > 0)
                {
                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.SysGroupID = SYS_GROUP_ID_ALLUSERS;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                    sysGroupUserMapViewModel.Insert();

                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.SysGroupID = SYS_GROUP_ID_CTUSERS;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                    sysGroupUserMapViewModel.Insert();

                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.SysGroupID = SYS_GROUP_ID_WEBTOOLS;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                    sysGroupUserMapViewModel.Insert();
                }

                // Web coop
                webCooperatorViewModel.Entity.FirstName = sessionCooperatorViewModel.Entity.FirstName;
                webCooperatorViewModel.Entity.LastName = sessionCooperatorViewModel.Entity.LastName;
                webCooperatorViewModel.Entity.JobTitle = sessionCooperatorViewModel.Entity.JobTitle;
                webCooperatorViewModel.Entity.Address1 = sessionCooperatorViewModel.Entity.AddressLine1;
                webCooperatorViewModel.Entity.Address2 = sessionCooperatorViewModel.Entity.AddressLine2;
                webCooperatorViewModel.Entity.Address3 = sessionCooperatorViewModel.Entity.AddressLine3;
                webCooperatorViewModel.Entity.City = sessionCooperatorViewModel.Entity.City;
                webCooperatorViewModel.Entity.GeographyID = sessionCooperatorViewModel.Entity.GeographyID;
                webCooperatorViewModel.Entity.PostalCode = sessionCooperatorViewModel.Entity.PostalIndex;
                webCooperatorViewModel.Entity.Organization = sessionCooperatorViewModel.Entity.Organization;
                webCooperatorViewModel.Entity.OrganizationAbbrev = sessionCooperatorViewModel.Entity.OrganizationAbbrev;
                webCooperatorViewModel.Entity.CategoryCode = sessionCooperatorViewModel.Entity.CategoryCode;
                webCooperatorViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_WEB_USER_ID;
                webCooperatorViewModel.Entity.IsActive = DEFAULT_WEB_COOPERATOR_IS_ACTIVE;
                webCooperatorViewModel.Insert();

                if (webCooperatorViewModel.Entity.ID > 0)
                {
                    sessionCooperatorViewModel.Get(sessionCooperatorViewModel.Entity.ID);
                    sessionCooperatorViewModel.Entity.WebCooperatorID = webCooperatorViewModel.Entity.ID;
                    sessionCooperatorViewModel.Update();
                }

                // Web user
                webUserViewModel.SearchEntity.WebUserName = sessionCooperatorViewModel.Entity.EmailAddress;
                webUserViewModel.Search();
                if (webUserViewModel.DataCollection.Count > 0)
                {
                    webUserViewModel.Entity.WebCooperatorID = webCooperatorViewModel.Entity.ID;
                    webUserViewModel.Entity.ModifiedByCooperatorID = 48;
                    webUserViewModel.Update();
                }
                else
                {
                    webUserViewModel.Entity.WebUserName = sessionCooperatorViewModel.Entity.EmailAddress;
                    webUserViewModel.Entity.WebUserPassword = sysUserViewModel.GetSecurePassword(defaultSysUserPassword);
                    webUserViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_WEB_USER_ID;
                    webUserViewModel.Entity.WebCooperatorID = webCooperatorViewModel.Entity.ID;
                    webUserViewModel.Entity.IsEnabled = "N";
                    webUserViewModel.Insert();
                }

                //************************************ 1. SEND APPROVER EMAILS ******************************************************

                // Send an email to each user in the GGTOOLS_MANAGE_COOPERATOR group.
                approvalListCooperatorViewModel.SearchEntity.SysGroupTag = SYS_GROUP_TAG_GGTOOLS_MANAGE_COOPERATOR;
                approvalListCooperatorViewModel.Search();

                // Edit template.
                emailTemplateViewModel.SearchEntity.CategoryCode = "CNR";
                emailTemplateViewModel.Search();

                if(emailTemplateViewModel.DataCollection.Count == 0)
                {
                    throw new IndexOutOfRangeException("Curator tool request email template not found.");
                }

                foreach (var result in approvalListCooperatorViewModel.DataCollection)
                {
                    sMTPMailMessage.From = emailTemplateViewModel.Entity.EmailFrom;
                    sMTPMailMessage.To = result.EmailAddress;
                    sMTPMailMessage.Subject = emailTemplateViewModel.Entity.Subject;
                    sMTPMailMessage.Body = emailTemplateViewModel.Entity.Body;

                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[ENVIRONMENT]", AppInfo.GetDatabase());
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[FIRST_NAME]", sessionCooperatorViewModel.Entity.FirstName);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[LAST_NAME]", sessionCooperatorViewModel.Entity.LastName);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[EMAIL_ADDRESS]", sessionCooperatorViewModel.Entity.EmailAddress);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[PRIMARY_PHONE]", sessionCooperatorViewModel.Entity.PrimaryPhone);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[JOB_TITLE]", sessionCooperatorViewModel.Entity.JobTitle);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[SITE_LONG_NAME]", sessionCooperatorViewModel.Entity.SiteName);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[ADDRESS_1]", sessionCooperatorViewModel.Entity.AddressLine1);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[ADDRESS_2]", sessionCooperatorViewModel.Entity.AddressLine2);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[ADDRESS_3]", sessionCooperatorViewModel.Entity.AddressLine3);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[CITY]", sessionCooperatorViewModel.Entity.City);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[STATE]", sessionCooperatorViewModel.Entity.StateName);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[POSTAL_CODE]", sessionCooperatorViewModel.Entity.PostalIndex);
                    sMTPMailMessage.Body = sMTPMailMessage.Body.Replace("[NOTE]", viewModel.RequestorNotes);

                    sMTPManager.SendMessage(sMTPMailMessage);
                }

                // If the user has requested a copy, generate and send a duplicate of the request
                // to the specified email address,
                if (!String.IsNullOrEmpty(viewModel.RequestorEmailAddress))
                {
                    sMTPMailMessageCopy = sMTPMailMessage;
                    sMTPMailMessageCopy.To = viewModel.RequestorEmailAddress;
                    sMTPMailMessageCopy.Subject = "Your Curator Tool Account Request";
                    sMTPMailMessageCopy.Body = "You submitted the following GRIN-Global Curator Tool account request on " + DateTime.Today.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString() + ": <br>" + sMTPMailMessageCopy.Body;
                    sMTPManager.SendMessage(sMTPMailMessageCopy);
                }

                //************************************ 2. SEND REQUESTOR EMAILS ******************************************************

                // Edit template.
                //emailTemplateViewModel.SearchEntity.CategoryCode = "CNA";
                //emailTemplateViewModel.Search();

                //sMTPMailMessageCooperator.From = emailTemplateViewModel.Entity.EmailFrom;
                
                ////DEBUG
                //sMTPMailMessageCooperator.To = sessionCooperatorViewModel.Entity.EmailAddress;
                //sMTPMailMessageCooperator.Subject = emailTemplateViewModel.Entity.Subject;
                //sMTPMailMessageCooperator.Body = emailTemplateViewModel.Entity.Body;
                //sMTPMailMessageCooperator.Body = sMTPMailMessageCooperator.Body.Replace("[FIRST_NAME]", sessionCooperatorViewModel.Entity.FirstName);
                //sMTPMailMessageCooperator.Body = sMTPMailMessageCooperator.Body.Replace("[LAST_NAME]", sessionCooperatorViewModel.Entity.LastName);
                //sMTPMailMessageCooperator.Body = sMTPMailMessageCooperator.Body.Replace("[CURATOR_TOOL_USER_NAME]", sysUserViewModel.Entity.SysUserName);
                //sMTPMailMessageCooperator.Body = sMTPMailMessageCooperator.Body.Replace("[CURATOR_TOOL_PASSWORD]", defaultSysUserPassword);
                //sMTPMailMessageCooperator.Body = sMTPMailMessageCooperator.Body.Replace("[CURATOR_TOOL_PASSWORD_EXPIRATION_DATE]", sysUserViewModel.Entity.SysUserPasswordExpirationDate.ToShortDateString());
                //sMTPMailMessageCooperator.Body = sMTPMailMessageCooperator.Body.Replace("[WEB_USER_NAME]", webUserViewModel.Entity.WebUserName);
                //sMTPManager.SendMessage(sMTPMailMessageCooperator);
            }
            return View("~/Views/Account/Request/ThankYou.cshtml");
        }
        public PartialViewResult GetSite(int entityId)
        {
            try
            {
                SiteViewModel viewModel = new SiteViewModel();
                viewModel.Get(entityId);
                return PartialView("~/Views/Site/_EditAddress.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderBatchEditModal()
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                return PartialView("~/Views/Cooperator/Modals/_EditBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        #region Training roster
        public PartialViewResult RenderTrainingRosterEditModal()
        {
            try
            {
                TrainingRosterViewModel viewModel = new TrainingRosterViewModel();
                return PartialView("~/Views/Account/Modals/_TrainingRosterEdit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderTrainingRosterWidget()
        {
            try
            {
                TrainingRosterViewModel viewModel = new TrainingRosterViewModel();
                return PartialView("~/Views/Account/_TrainingRosterWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        [HttpPost]
        public JsonResult CreateTrainingRoster(TrainingRosterViewModel viewModel)
        {
            try
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { succsss = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}