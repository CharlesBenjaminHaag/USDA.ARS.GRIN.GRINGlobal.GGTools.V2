using NLog;
using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class WebOrderController : BaseController, IController<WebOrderRequestViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // *********************************************************************************
        // EXPLORER
        // *********************************************************************************
        public ActionResult Explorer()
        {
            return View("~/Views/WebOrder/Explorer/Index2.cshtml");
        }

        public PartialViewResult ExplorerSelectList(int entityId = 0)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.SearchEntity.ID = entityId;
                viewModel.Search();
                if (viewModel.RowsAffected == 0)
                {
                    return PartialView("~/Views/Error/RecordNotFound.cshtml");
                }
                else
                {
                    return PartialView("~/Views/WebOrder/Explorer/_List.cshtml");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult ExplorerDetail(int entityId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.Get(entityId);

                if (viewModel.RowsAffected == 0)
                {
                    return PartialView("~/Views/Error/RecordNotFound.cshtml");
                }
                else
                {
                    return PartialView("~/Views/WebOrder/Explorer/_Detail.cshtml", viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult ExplorerTimeline(int entityId)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();

            try
            {
                viewModel.SearchEntity.ID = entityId;
                viewModel.GetWebOrderRequestActions();
                return PartialView("~/Views/WebOrder/Explorer/_Timeline.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult ExplorerNotes(int entityId)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
            return PartialView();
        }
        public PartialViewResult _ListItems(int webOrderRequestId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.GetWebOrderRequestItems(webOrderRequestId);
                return PartialView("~/Views/WebOrder/_ListItems.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public JsonResult Approve(WebOrderRequestViewModel viewModel)
        {
            viewModel.Get(viewModel.Entity.ID);
            viewModel.Entity.StatusCode = "NRR_APPROVE";
            viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            //ewModel.Update();

            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
            webOrderRequestAction.WebOrderRequestID = viewModel.Entity.ID;
            webOrderRequestAction.ActionCode = viewModel.EventAction;
            webOrderRequestAction.Note = viewModel.EventNote + "==========================================" + viewModel.ActionEmailBody;
            webOrderRequestAction.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            viewModel.InsertWebOrderRequestAction(webOrderRequestAction);

            // TODO Send internal notification

            return null;
        }
        public JsonResult Reject(WebOrderRequestViewModel viewModel)
        {
            viewModel.Get(viewModel.Entity.ID);
            viewModel.Entity.StatusCode = "NRR_REJECT";
            viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            //viewModel.Update();

            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
            webOrderRequestAction.WebOrderRequestID = viewModel.Entity.ID;
            webOrderRequestAction.ActionCode = viewModel.EventAction;
            webOrderRequestAction.Note = viewModel.EventNote + "==========================================" + viewModel.ActionEmailBody;
            webOrderRequestAction.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            viewModel.InsertWebOrderRequestAction(webOrderRequestAction);

            // TODO Send internal notification

            return null;
        }

        public PartialViewResult RenderEmailModal(int entityId, string actionCode, string actionEmailTo)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
            EmailTemplate emailTemplate = new EmailTemplate();

            switch (actionCode)
            {
                case "NRR_APPROVE":
                    viewModel.EventValue = "Approve Web Order Request";
                    //emailTemplate = viewModel.GetEmailTemplate("CAP");
                    break;
                case "NRR_REJECT":
                    viewModel.EventValue = "Reject Web Order Request";
                    //emailTemplate = viewModel.GetEmailTemplate("RRJ");
                    break;
                case "NRR_REQ":
                    viewModel.EventValue = "Request Additional Information";
                    //emailTemplate = viewModel.GetEmailTemplate("RQI");
                    break;
                case "NRR_CUR":
                    viewModel.EventValue = "Email Curators";
                    //emailTemplate = viewModel.GetEmailTemplate("CUR");
                    break;
            }

            viewModel.EventAction = actionCode;
            viewModel.Entity.ID = entityId;
            viewModel.ActionEmailTo = actionEmailTo;
            viewModel.ActionEmailSubject = emailTemplate.Subject;
            viewModel.ActionEmailFrom = "gringlobal.orders@usda.gov";
            viewModel.ActionEmailBody = emailTemplate.Body;

            // REFACTOR: Replace placeholder variables with WOR data.
            //viewModel.ActionEmailBody.Replace("@WebCooperatorFullName", viewModel.Entity.WebCooperatorFullName);
            //viewModel.ActionEmailBody = viewModel.ActionEmailBody.Replace("[ID_HERE]", entityId.ToString());
            //viewModel.ActionEmailSubject = viewModel.ActionEmailSubject.Replace("[ID_HERE]", entityId.ToString());
            return PartialView("~/Views/WebOrder/Modals/_Email.cshtml", viewModel);
        }

        public JsonResult SendEmail(WebOrderRequestViewModel viewModel)
        {
            try
            {
                viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
                viewModel.SendEmail();
                
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult RenderRejectModal(int entityId)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
            viewModel.Get(entityId);
            viewModel.EventValue = "Reject Web Order Request";

            //TODO Load appropriate template
            EmailTemplate emailTemplate = viewModel.GetEmailTemplate("CCL");
            viewModel.ActionEmailFrom = "gringlobal.orders@usda.gov";
            viewModel.ActionEmailTo = viewModel.Entity.WebCooperatorEmail;
            viewModel.ActionEmailBody = emailTemplate.Body;

            return PartialView("~/Views/WebOrder/Modals/_Email.cshtml", viewModel);
        }
        public PartialViewResult RenderNoteModal(int entityId)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
            viewModel.SearchEntity.ID = entityId;
            viewModel.GetNotes();
            return PartialView("~/Views/WebOrder/Modals/_Note.cshtml", viewModel);
        }
        [HttpPost]
        public JsonResult AddNote(int webOrderRequestId, string noteText)
        {
            try 
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.InsertWebOrderRequestActionNote(webOrderRequestId, noteText, AuthenticatedUser.WebUserID);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public ActionResult Edit(WebOrderRequestViewModel viewModel)
        {
            throw new NotImplementedException();
            //try
            //{
            //    viewModel.Entity.WebUserID = AuthenticatedUser.WebUserID;
            //    viewModel.Entity.StatusCode = viewModel.EventValue;
            //    viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            //    viewModel.Entity.Note = viewModel.EventNote;
            //    viewModel.Update();
            //    return RedirectToAction("Edit", "WebOrder", new { entityId = viewModel.Entity.ID });
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex);
            //    return RedirectToAction("InternalServerError", "Error");
            //}
        }
        //[HttpPost]
        //public JsonResult EditBatch(FormCollection formCollection)
        //{
        //    try 
        //    {
        //        if (!String.IsNullOrEmpty(formCollection["IDList"]))
        //        {
        //           var DEBUG = formCollection["IDList"];
        //        }
        //        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult Index()
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.PageTitle = "Dashboard";
                return View("~/Views/WebOrder/Dashboard.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Search(int id=0, string status="", string requestorName="", string emailAddress="", string organization="", string intendedUseCode="")
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();

            try
            {
                if (id > 0)
                {
                    viewModel.SearchEntity.ID = id;
                    viewModel.Search();
                }

                if (!String.IsNullOrEmpty(status))
                {
                    viewModel.SearchEntity.StatusCode = status;
                    viewModel.Search();
                }

                if (!String.IsNullOrEmpty(organization))
                {
                    viewModel.SearchEntity.WebCooperatorOrganization = organization;
                    viewModel.Search();
                }

                if (!String.IsNullOrEmpty(emailAddress))
                {
                    viewModel.SearchEntity.WebCooperatorEmailAddress = emailAddress;
                    viewModel.Search();
                }

                if (!String.IsNullOrEmpty(intendedUseCode))
                {
                    viewModel.SearchEntity.IntendedUseCode = intendedUseCode;
                    viewModel.Search();
                }
                return View("~/Views/WebOrder/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(WebOrderRequestViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/WebOrder/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult _Lookup(FormCollection formCollection)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["TimeFrame"]))
                {
                    viewModel.SearchEntity.TimeFrame = formCollection["TimeFrame"];
                }

                if (!String.IsNullOrEmpty(formCollection["SelectedStatusList"]))
                {
                    viewModel.SearchEntity.StatusList = formCollection["SelectedStatusList"];
                }

                if (!String.IsNullOrEmpty(formCollection["SelectedWebUserList"]))
                {
                    viewModel.SearchEntity.WebUserList = formCollection["SelectedWebUserList"];
                }

                viewModel.Search();
                return PartialView("~/Views/WebOrder/Explorer/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public JsonResult AddWebOrderRequestAction(FormCollection formCollection)
        {
            return null;
        }
        public PartialViewResult _RenderNoteWidget(int webOrderRequestId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.SearchEntity.ID = webOrderRequestId;
                viewModel.GetNotes();
                return PartialView("~/Views/WebOrder/Explorer/_NoteWidget.cshtml", viewModel);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //[HttpPost]
        //public JsonResult AddNote(WebOrderRequestViewModel viewModel)
        //{
        //    try
        //    {
        //        viewModel.InsertWebOrderRequestActionNote(viewModel.Entity.ID, viewModel.Entity.Note, AuthenticatedUser.WebUserID);
        //        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}