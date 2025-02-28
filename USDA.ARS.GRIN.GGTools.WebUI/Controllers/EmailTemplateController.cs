using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class EmailTemplateController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int sysFolderId)
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
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }
        public PartialViewResult View(int entityId)
        {
            EmailTemplateViewModel viewModel = new EmailTemplateViewModel();
            try
            {
                viewModel.PageTitle = String.Format("Edit Email Template [{0}]", entityId);
                viewModel.Get(entityId);
                return PartialView("~/Views/EmailTemplate/_View.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }

        public ActionResult Edit(int entityId)
        {
            EmailTemplateViewModel viewModel = new EmailTemplateViewModel();
            try
            {
                viewModel.PageTitle = String.Format("Edit Email Template [{0}]", entityId);
                viewModel.Get(entityId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult Save(EmailTemplateViewModel viewModel)
        {
            try
            {
                viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Update();

                viewModel.Get(viewModel.Entity.ID);
                return PartialView("~/Views/EmailTemplate/","EmailTemplate");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }

        // GET: EmailTemplate
        public ActionResult Index()
        {
            EmailTemplateViewModel viewModel = new EmailTemplateViewModel();
            
            try 
            {
                viewModel.Search();
                viewModel.PageTitle = "Email Templates";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }

        }

        public ActionResult Search(EmailTemplateViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        //public ActionResult EmailTemplateHome()
        //{
        //    TempData["context"] = "Email Templates";
        //    try
        //    {
        //        GRINGlobalService grinGlobalService = new GRINGlobalService(this.AuthenticatedUserSession.Environment);
        //        EmailTemplateHomeViewModel emailTemplateHomeViewModel = new EmailTemplateHomeViewModel();
        //        emailTemplateHomeViewModel.EmailTemplates = grinGlobalService.GetEmailTemplates();
        //        if (emailTemplateHomeViewModel.EmailTemplates.Count() > 0)
        //        {
        //            emailTemplateHomeViewModel.CurrentID = emailTemplateHomeViewModel.EmailTemplates.First().ID;
        //        }
        //        return Edit("~/Views/GRINGlobal/WebOrder/Email/Index.cshtml", emailTemplateHomeViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, ex.Message);
        //        return PartialView("~/Views/Error/_Error.cshtml");
        //    }
        //}

        //public PartialViewResult _EmailTemplateView(int id)
        //{
        //    TempData["context"] = "Edit Email Template";
        //    GRINGlobalService grinGlobalService = new GRINGlobalService(this.AuthenticatedUserSession.Environment);
        //    EmailTemplateEditViewModel emailTemplateEditViewModel = new EmailTemplateEditViewModel();
        //    EmailTemplate emailTemplate = new EmailTemplate();

        //    try
        //    {
        //        emailTemplate = grinGlobalService.GetEmailTemplate(id);
        //        if (emailTemplate == null)
        //        {
        //            throw new NullReferenceException(String.Format("No email template found for id {0}", id));
        //        }
        //        emailTemplateEditViewModel.ID = emailTemplate.ID;
        //        emailTemplateEditViewModel.Title = emailTemplate.Title;
        //        emailTemplateEditViewModel.SenderAddress = emailTemplate.From;
        //        emailTemplateEditViewModel.Subject = emailTemplate.Subject;
        //        emailTemplateEditViewModel.Body = emailTemplate.Body;
        //        return PartialView("~/Views/GRINGlobal/WebOrder/Email/_Detail.cshtml", emailTemplateEditViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, ex.Message);
        //        return PartialView("~/Views/Error/_Error.cshtml");
        //    }
        //}

        //public ActionResult EmailTemplateEdit(int id)
        //{
        //    TempData["context"] = "Edit Email Template";
        //    GRINGlobalService grinGlobalService = new GRINGlobalService(this.AuthenticatedUserSession.Environment);
        //    EmailTemplateEditViewModel emailTemplateEditViewModel = new EmailTemplateEditViewModel();
        //    EmailTemplate emailTemplate = new EmailTemplate();

        //    try
        //    {
        //        emailTemplate = grinGlobalService.GetEmailTemplate(id);
        //        if (emailTemplate == null)
        //        {
        //            throw new NullReferenceException(String.Format("No email template found for id {0}", id));
        //        }
        //        emailTemplateEditViewModel.ID = emailTemplate.ID;
        //        emailTemplateEditViewModel.Title = emailTemplate.Title;
        //        emailTemplateEditViewModel.SenderAddress = emailTemplate.From;
        //        emailTemplateEditViewModel.Subject = emailTemplate.Subject;
        //        emailTemplateEditViewModel.Body = emailTemplate.Body;
        //        emailTemplateEditViewModel.CreatedDate = emailTemplate.CreatedDate;
        //        emailTemplateEditViewModel.CreatedByCooperatorID = emailTemplate.CreatedByCooperatorID;
        //        emailTemplateEditViewModel.CreatedByCooperatorName = emailTemplate.CreatedByCooperatorName;
        //        emailTemplateEditViewModel.ModifiedDate = emailTemplate.ModifiedDate;
        //        emailTemplateEditViewModel.ModifiedByCooperatorID = emailTemplate.ModifiedByCooperatorID;
        //        emailTemplateEditViewModel.ModifiedByCooperatorName = emailTemplate.ModifiedByCooperatorName;
        //        return Edit("~/Views/GRINGlobal/WebOrder/Email/Edit.cshtml", emailTemplateEditViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, ex.Message);
        //        return RedirectToAction("InternalServerError", "Error");
        //    }
        //}

        //[HttpPost]
        //public ActionResult EmailTemplateEdit(EmailTemplateEditViewModel emailTemplateEditViewModel)
        //{
        //    GRINGlobalService grinGlobalService = new GRINGlobalService(this.AuthenticatedUserSession.Environment);
        //    EmailTemplate emailTemplate = new EmailTemplate();
        //    ResultContainer resultContainer = new ResultContainer();
        //    try
        //    {
        //        emailTemplate.ID = emailTemplateEditViewModel.ID;
        //        emailTemplate.Title = emailTemplateEditViewModel.Title;
        //        emailTemplate.Subject = emailTemplateEditViewModel.Subject;
        //        emailTemplate.From = emailTemplateEditViewModel.SenderAddress;
        //        emailTemplate.To = emailTemplateEditViewModel.RecipientAddress;
        //        emailTemplate.Body = emailTemplateEditViewModel.Body;
        //        resultContainer = grinGlobalService.UpdateEmailTemplate(emailTemplate);
        //        return RedirectToAction("EmailTemplateEdit", "WebOrder", new { id = emailTemplateEditViewModel.ID });
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, ex.Message);
        //        return RedirectToAction("InternalServerError", "Error");
        //    }
        //}
    }
}