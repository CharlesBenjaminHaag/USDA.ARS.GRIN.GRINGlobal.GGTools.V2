using System;
using System.Web.Mvc;
using System.IO;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CropGermplasmCommitteeDocumentController : BaseController, IController<CropGermplasmCommitteeDocumentViewModel>
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
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                CropGermplasmCommitteeDocumentViewModel viewModel = new CropGermplasmCommitteeDocumentViewModel();
                viewModel.TableName = "crop_germplasm_committee_document";
                viewModel.PageTitle = String.Format("Edit CGC Document [{0}]", entityId);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.Get(entityId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(CropGermplasmCommitteeDocumentViewModel viewModel)
        {
            string uploadDir = "~USDA.ARS.GRIN.Web/documents/cgc/";
            string path = String.Empty;

            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                }

                if (viewModel.DocumentUpload != null && viewModel.DocumentUpload.ContentLength > 0)
                {
                    switch (viewModel.Entity.CategoryCode)
                    {
                        case "CVS":
                            uploadDir += "cvs";
                            break;
                        case "MIN":
                            uploadDir += "committee";
                            break;
                    }
                    var documentPath = Path.Combine(Server.MapPath(uploadDir), viewModel.DocumentUpload.FileName);
                    var documentUrl = Path.Combine(uploadDir, viewModel.DocumentUpload.FileName);

                    // Edit full document URL to be saved with record.
                    var urlBuilder =
                        new System.UriBuilder(Request.Url.AbsoluteUri)
                        {
                            Path = Url.Content(documentUrl),
                            Query = null,
                        };

                    Uri uri = urlBuilder.Uri;
                    viewModel.Entity.URL = urlBuilder.ToString();
                    viewModel.DocumentUpload.SaveAs(documentUrl);
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
                return RedirectToAction("Edit", "CropGermplasmCommitteeDocument", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: CropGermplasmCommitteeDocument
        public ActionResult Index()
        {
            CropGermplasmCommitteeDocumentViewModel viewModel = new CropGermplasmCommitteeDocumentViewModel();
            viewModel.PageTitle = "CGC Document Search";
            return View(viewModel);
        }

        public ActionResult Search(CropGermplasmCommitteeDocumentViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/CropGermplasmCommitteeDocument/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
    }
}