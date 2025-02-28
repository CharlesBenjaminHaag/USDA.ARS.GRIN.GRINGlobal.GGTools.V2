using System.Web.Mvc;
using System;
using System.Web.SessionState;
using System.IO;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
   
    [GrinGlobalAuthentication]
    public class AccessionInventoryAttachmentController : BaseController, IController<AccessionInventoryAttachmentViewModel>
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
        public ActionResult Index()
        {
            AccessionInventoryAttachmentViewModel viewModel = new AccessionInventoryAttachmentViewModel();

            try
            {
                viewModel.PageTitle = "Accession Inventory Attachments";
                viewModel.SearchEntity.IsVirtualPathValid = "X";
                viewModel.SearchEntity.IsThumbnailVirtualPathValid = "X";
                viewModel.SearchEntity.StartDate = null;
                viewModel.SearchEntity.EndDate = null;

                if (Session["ACCESSION_INV_ATTACH_SEARCH"] != null)
                {
                    viewModel.SearchEntity = viewModel.Deserialize<AccessionInventoryAttachmentSearch>(Session["ACCESSION_INV_ATTACH_SEARCH"].ToString());
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        [HttpPost]
        public ActionResult Search(AccessionInventoryAttachmentViewModel viewModel)
        {
            try
            {
                switch (viewModel.EventAction)
                {
                    case "VALIDATE":
                        break;
                    case "SAVE-SEARCH":
                        //SaveSearch(viewModel);
                        break;
                }
                
                if (viewModel.EventAction == "VALIDATE")
                {
                    TempData["ACCESSION_INV_ATTACH_ID_LIST"] = viewModel.ItemIDList;
                    return RedirectToAction("FileManager", "Attachment");
                    //viewModel.ValidateURLs();
                    //FolderViewModel folderViewModel = new FolderViewModel();
                    //folderViewModel.Entity.ID = viewModel.Entity.ID;
                    //folderViewModel.Entity.FolderName = viewModel.FolderEntity.FolderName;
                    //folderViewModel.Entity.Category = "Validation";
                    //folderViewModel.Entity.TableName = "accession_inv_attach";
                    //folderViewModel.Entity.FolderType = "accession_inv_attach";
                    //folderViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    //folderViewModel.Insert();
                    //viewModel.FolderEntity = folderViewModel.Entity;
                    //return Edit("~/Views/Attachment/FileManager.cshtml", viewModel);
                }
                else
                {
                    Session["ACCESSION_INV_ATTACH_SEARCH"] = viewModel.SerializeToXml(viewModel.SearchEntity);
                    viewModel.Search();
                    ModelState.Clear();
                }
                return View("~/Views/AccessionInventoryAttachment/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        //public string SaveSearch(AccessionInventoryAttachmentViewModel viewModel)
        //{
        //    try
        //    {
        //        FolderViewModel folderViewModel = new FolderViewModel();
        //        folderViewModel.Entity.FolderName = "Accession Inventory Attachment Search";
        //        folderViewModel.Entity.Category = "Cat";
        //        folderViewModel.Entity.FolderType = "accession_inventory_attachment";
        //        folderViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
        //        folderViewModel.Insert();
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return "";
        //    }
        //}

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                AccessionInventoryAttachmentViewModel viewModel = new AccessionInventoryAttachmentViewModel();
                viewModel.PageTitle = "Edit Accession Inventory Attachment";
                viewModel.BasePath = "https://npgsweb.ars-grin.gov/gringlobal/uploads/images/";
                viewModel.Get(entityId);

                if (!viewModel.Entity.VirtualPath.Contains("http"))
                {
                    viewModel.Entity.VirtualPath = viewModel.BasePath + viewModel.Entity.VirtualPath;
                }

                if (!String.IsNullOrEmpty(viewModel.Entity.ThumbnailVirtualPath))
                {
                    if (!viewModel.Entity.ThumbnailVirtualPath.Contains("http"))
                    {
                        viewModel.Entity.ThumbnailVirtualPath = viewModel.BasePath + viewModel.Entity.ThumbnailVirtualPath;
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(AccessionInventoryAttachmentViewModel viewModel)
        {
            string uploadDir = "~/uploads/images/";
            string documentPath = String.Empty;
            string documentUrl = String.Empty;

            try
            {
                // File upload
                if (viewModel.VirtualPathUpload != null && viewModel.VirtualPathUpload.ContentLength > 0)
                {
                    documentPath = Path.Combine(Server.MapPath(uploadDir), viewModel.VirtualPathUpload.FileName);
                    documentUrl = Path.Combine(uploadDir, viewModel.VirtualPathUpload.FileName);
                    viewModel.VirtualPathUpload.SaveAs(documentPath);

                    viewModel.Entity.ContentType = viewModel.VirtualPathUpload.ContentType;
                    
                    var urlBuilder =
                       new System.UriBuilder(Request.Url.AbsoluteUri)
                       {
                           Path = Url.Content(documentUrl),
                           Query = null,
                       };

                    Uri uri = urlBuilder.Uri;
                    viewModel.Entity.VirtualPath = urlBuilder.ToString();
                }

                //Data
                viewModel.Entity.VirtualPath = documentUrl;
                viewModel.Update();

                return RedirectToAction("Edit", "AccessionInventoryAttachment", new { @entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult Validate(FormCollection formCollection)
        {
            try
            {
                AccessionInventoryAttachmentViewModel viewModel = new AccessionInventoryAttachmentViewModel();


                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.ItemIDList = formCollection["IDList"];
                }
                viewModel.ValidateAll();
                //return RedirectToAction("Index", "Attachment");
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                //return RedirectToAction("InternalServerError", "Error");
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FileManager()
        {
            try
            {
                AccessionInventoryAttachmentViewModel viewModel = new AccessionInventoryAttachmentViewModel();

                if (TempData["ACCESSION_INV_ATTACH_ID_LIST"] == null)
                {
                    throw new Exception("TempData is null.");
                }
                
                viewModel.ItemIDList = TempData["ACCESSION_INV_ATTACH_ID_LIST"].ToString();
                viewModel.TableName = "accession_inventory_attachment";
                viewModel.PageTitle = "Validate URLs";
                //viewModel.ValidateURLs();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return View("InternalServerError", "Error");
            }
        }

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