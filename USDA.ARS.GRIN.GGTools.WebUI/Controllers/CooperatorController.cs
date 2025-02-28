using NLog;
using System;
using System.Configuration;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CooperatorController : BaseController, IController<CooperatorViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public override PartialViewResult PageMenu(string eventAction, string eventValue, string sysTableName = "", string sysTableTitle = "", int entityId = 0)
        {
            ViewBag.EventAction = eventAction;
            ViewBag.EventValue = eventValue;

            if (eventValue == "Add")
            {
                return PartialView("~/Views/Components/_DefaultAddMenu.cshtml");
            }
            else
            {
                if (eventValue == "Edit")
                {
                    return PartialView("~/Views/Cooperator/Components/_EditMenu.cshtml");
                }
                else
                {
                    if (!String.IsNullOrEmpty(sysTableName))
                    {
                        return PartialView("~/Views/Cooperator/Components/_SearchMenu.cshtml");
                    }
                    else
                    {
                        return PartialView("~/Views/Components/_DefaultMenu.cshtml");
                    }
                }
            }
        }

        public ActionResult Get(int entityId)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId, "");
                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}, {2}", entityId, viewModel.Entity.LastName, viewModel.Entity.FirstName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return View("~/Views/Cooperator/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public PartialViewResult _Get(int entityId, string environment)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId, environment);
                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}", entityId, viewModel.Entity.FullName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return PartialView("~/Views/Cooperator/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public JsonResult GetSelectListData()
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.SearchEntity.StatusCode = "ACTIVE";
                viewModel.Search();
                return Json(viewModel.DataCollection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(CooperatorViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                }

                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Insert();
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                return RedirectToAction("Edit", "Cooperator", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public ActionResult Edit(int entityId)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId, "");
                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}, {2}", entityId, viewModel.Entity.LastName, viewModel.Entity.FirstName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                
                return View("~/Views/Cooperator/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult EditBatch()
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.BatchSize = 10;
                viewModel.Entity.FirstName = "Training";
                viewModel.Entity.LastName = "User";
                viewModel.Entity.JobTitle = "Student";
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View("~/Views/Cooperator/EditBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult _Edit(CooperatorViewModel viewModel)
        {
            SysUserViewModel sysUserViewModel = new SysUserViewModel();
            try
            {
                //if (!viewModel.Validate())
                //{
                //    if (viewModel.ValidationMessages.Count > 0) return Edit(viewModel);
                //}

                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    // TODO Set default pw
                    viewModel.Entity.SysUserPassword = sysUserViewModel.GetSecurePassword("GRIN!Gl@balPa$$");
                    viewModel.Insert();
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                viewModel.Get(viewModel.Entity.ID);
                return Json(new { cooperator = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                viewModel.Entity.ID = -1;
                return Json(new { cooperator = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [GrinGlobalAuthentication]
        public ActionResult Index()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            viewModel.PageTitle = "Cooperator Search";
            viewModel.TableName = "cooperator";
            viewModel.AuthenticatedUser = AuthenticatedUser;
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult Search(CooperatorViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/Cooperator/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult Lookup(CooperatorViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView("~/Views/Cooperator/Modals/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SetStatus(CooperatorViewModel viewModel)
        {
            SMTPManager sMTPManager = new SMTPManager();
            SMTPMailMessage sMTPMailMessage = new SMTPMailMessage();

            try 
            {
                if (viewModel.Entity.ID <= 0)
                {
                    throw new IndexOutOfRangeException("Cooperator ID is set to " + viewModel.Entity.ID.ToString());
                }

                if (viewModel.EventAction == "ACTIVATE")
                {
                    CooperatorViewModel cooperatorViewModel = new CooperatorViewModel();
                    cooperatorViewModel.Get(viewModel.Entity.ID);
                    cooperatorViewModel.Entity.StatusCode = "ACTIVE";
                    cooperatorViewModel.Update();

                    SysUserViewModel sysUserViewModel = new SysUserViewModel();
                    sysUserViewModel.Get(cooperatorViewModel.Entity.SysUserID);
                    sysUserViewModel.Entity.IsEnabled = "Y";
                    sysUserViewModel.UpdateStatus();

                    WebCooperatorViewModel webCooperatorViewModel = new WebCooperatorViewModel();
                    webCooperatorViewModel.Get(cooperatorViewModel.Entity.WebCooperatorID);
                    webCooperatorViewModel.Entity.IsActive = "Y";
                    webCooperatorViewModel.Update();

                    WebUserViewModel webUserViewModel = new WebUserViewModel();
                    webUserViewModel.Get(cooperatorViewModel.Entity.WebUserID);
                    webUserViewModel.Entity.IsEnabled = "Y";
                    webUserViewModel.Update();

                    // TODO
                    // Send email 
                    EmailTemplateViewModel emailTemplateViewModel = new EmailTemplateViewModel();
                    EmailTemplate emailTemplate = emailTemplateViewModel.Get("CNA");

                    SMTPMailMessage requestorEmailMessage = new SMTPMailMessage();

                    // If there is edited email present in the viewmodel, use those
                    // fields. Otherwise, use the default template text.
                    if (!String.IsNullOrEmpty(viewModel.EmailMessage.Body))
                    {
                        requestorEmailMessage.From = viewModel.EmailMessage.From;
                        requestorEmailMessage.To = viewModel.EmailMessage.To;
                        requestorEmailMessage.Subject = viewModel.EmailMessage.Subject;
                        requestorEmailMessage.CC = viewModel.EmailMessage.CC;
                        requestorEmailMessage.Body = viewModel.EmailMessage.Body;
                        requestorEmailMessage.IsHtml = viewModel.EmailMessage.IsHtml;
                    }
                    else
                    { 
                        requestorEmailMessage.From = emailTemplate.EmailFrom;
                        requestorEmailMessage.To = cooperatorViewModel.Entity.EmailAddress;
                        requestorEmailMessage.Subject = emailTemplate.Subject;
                        requestorEmailMessage.Body = emailTemplate.Body;
                        requestorEmailMessage.IsHtml = true;
                    }

                    // Replace substitution variables with values from cooperator data
                    // fields.
                    requestorEmailMessage.Body = requestorEmailMessage.Body.Replace("[ENVIRONMENT]", AppInfo.GetDatabase());
                    requestorEmailMessage.Body = requestorEmailMessage.Body.Replace("[FIRST_NAME]", cooperatorViewModel.Entity.FirstName);
                    requestorEmailMessage.Body = requestorEmailMessage.Body.Replace("[LAST_NAME]", cooperatorViewModel.Entity.LastName);
                    requestorEmailMessage.Body = requestorEmailMessage.Body.Replace("[CURATOR_TOOL_USER_NAME]", sysUserViewModel.Entity.SysUserName);
                    requestorEmailMessage.Body = requestorEmailMessage.Body.Replace("[WEB_USER_NAME]", cooperatorViewModel.Entity.EmailAddress);
                    requestorEmailMessage.Body = requestorEmailMessage.Body.Replace("[CURATOR_TOOL_PASSWORD]", ConfigurationManager.AppSettings["DefaultSysUserPassword"]);
                    requestorEmailMessage.Body = requestorEmailMessage.Body.Replace("[CURATOR_TOOL_PASSWORD_EXPIRATION_DATE]", sysUserViewModel.Entity.SysUserPasswordExpirationDate.ToShortDateString());
                    requestorEmailMessage.Body = requestorEmailMessage.Body.Replace("[WEB_USER_PASSWORD]", ConfigurationManager.AppSettings["DefaultSysUserPassword"]);

                    sMTPManager.SendMessage(requestorEmailMessage);
                }

                return RedirectToAction("Edit","Cooperator", new { @entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _List(int siteId = 0, string sysUserIsEnabled = "Y", string statusCode = "ACTIVE", string formatCode = "", string sysGroupTag = "", bool isDetailFormat = false)
        {
            string partialViewName = "_List.cshtml";

            try
            {
                switch(formatCode)
                {
                    case "LIST":
                        partialViewName = "_List.cshtml";
                        break;
                    case "CLST":
                        partialViewName = "_ContactListWidget.cshtml";
                        break;
                    case "LWGT":
                        partialViewName = "_ListWidget.cshtml";
                        break;
                }

                if (isDetailFormat == true)
                {
                    partialViewName = "_DetailList.cshtml";
                }

                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.SearchEntity.SiteID = siteId;
                //viewModel.SearchEntity.StatusCode = statusCode;
                viewModel.SearchEntity.SysUserIsEnabled = sysUserIsEnabled;
                viewModel.Search();
                return PartialView("~/Views/Cooperator/" + partialViewName, viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListAppUserGUISettings(int cooperatorId)
        {
            try 
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.GetAppUserGUISettings(cooperatorId);
                return PartialView("~/Views/Cooperator/_ListAppUserGUISettings.cshtml",viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderSiteCuratorListWidget(int siteId)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.GetSiteCurators(siteId);
                return PartialView("~/Views/Cooperator/_ListWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult RenderLookupModal()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                return PartialView("~/Views/Cooperator/Modals/_Lookup.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult RenderWidget(int cooperatorId)
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                viewModel.SearchEntity.ID = cooperatorId;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.Search();
                return PartialView("~/Views/Cooperator/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult RenderListWidget(int siteId = 0, string sysGroupTag = "")
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                viewModel.SearchEntity.SiteID = siteId;
                viewModel.SearchEntity.SysGroupTag = sysGroupTag;
                viewModel.GetBySysGroup(sysGroupTag);
                return PartialView("~/Views/Cooperator/_SiteCuratorListWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult RenderEmailModal(int entityId)
        {
            CooperatorViewModel cooperatorViewModel = new CooperatorViewModel();
            cooperatorViewModel.Get(entityId);

            EmailTemplateViewModel emailTemplateViewModel = new EmailTemplateViewModel();
            EmailTemplate emailTemplate = emailTemplateViewModel.Get("CNA");

            SMTPMailMessage requestorEmailMessage = new SMTPMailMessage();
            requestorEmailMessage.From = emailTemplate.EmailFrom;
            requestorEmailMessage.To = cooperatorViewModel.Entity.EmailAddress;
            requestorEmailMessage.Subject = emailTemplate.Subject;

            emailTemplate.Body = emailTemplate.Body.Replace("[FIRST_NAME]", cooperatorViewModel.Entity.FirstName);
            emailTemplate.Body = emailTemplate.Body.Replace("[LAST_NAME]", cooperatorViewModel.Entity.LastName);
            emailTemplate.Body = emailTemplate.Body.Replace("[CURATOR_TOOL_USER_NAME]", cooperatorViewModel.Entity.SysUserName);
            emailTemplate.Body = emailTemplate.Body.Replace("[WEB_USER_NAME]", cooperatorViewModel.Entity.EmailAddress);
            emailTemplate.Body = emailTemplate.Body.Replace("[CURATOR_TOOL_PASSWORD]", ConfigurationManager.AppSettings["DefaultSysUserPassword"]);
            emailTemplate.Body = emailTemplate.Body.Replace("[CURATOR_TOOL_PASSWORD_EXPIRATION_DATE]", cooperatorViewModel.Entity.SysUserPasswordExpirationDate.ToShortDateString());
            emailTemplate.Body = emailTemplate.Body.Replace("[WEB_USER_PASSWORD]", ConfigurationManager.AppSettings["DefaultSysUserPassword"]);
            requestorEmailMessage.Body = emailTemplate.Body;
            requestorEmailMessage.IsHtml = true;
            cooperatorViewModel.EmailMessage = requestorEmailMessage;

            return PartialView("~/Views/Cooperator/Modals/_Email.cshtml", cooperatorViewModel);
        }
       
        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        
        public ActionResult Add()
        {
            try 
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.PageTitle = "Add Cooperator";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.Entity.StatusCode = "ACTIVE";
                return View("~/Views/Cooperator/Edit.cshtml", viewModel);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        #region Ownership

        public ActionResult EditOwnership(int cooperatorId = 0)
        {
            CooperatorOwnershipViewModel viewModel = new CooperatorOwnershipViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult EditOwnership(CooperatorOwnershipViewModel viewModel)
        {
            int result = 0;
            try 
            {
                result = viewModel.Transfer();
                return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, data = -1, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult _ListOwnership(int cooperatorId)
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            viewModel.AuthenticatedUser = AuthenticatedUser;

            try 
            {
                viewModel.GetRecordsOwned(cooperatorId);
                return PartialView("~/Views/Cooperator/Ownership/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
    }

}

        #endregion Ownership

        #region Components

        public PartialViewResult Component_PageMenu()
        {
            return PartialView("~/Views/Cooperator/Components/_EditMenu.cshtml");
        }

        public PartialViewResult Component_Widget(int cooperatorId)
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();

            try
            {
                viewModel.Get(cooperatorId);
                return PartialView("~/Views/Cooperator/Components/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #endregion
    }
}