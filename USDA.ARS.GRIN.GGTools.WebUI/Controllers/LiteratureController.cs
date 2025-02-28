using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class LiteratureController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Literature/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            LiteratureViewModel viewModel = new LiteratureViewModel();
            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.SearchEntity.FolderID = sysFolderId;
                viewModel.GetFolderItems();
                return PartialView(BASE_PATH + "_ListFolder.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListDynamicFolderItems(int folderId)
        {
            LiteratureViewModel viewModel = new LiteratureViewModel();

            try
            {
                viewModel.RunSearch(folderId);
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add()
        {
            try
            {
                LiteratureViewModel viewModel = new LiteratureViewModel();
                viewModel.TableName = "literature";
                viewModel.PageTitle = String.Format("Add Literature");
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                LiteratureViewModel viewModel = new LiteratureViewModel();
                viewModel.TableName = "literature";
                viewModel.TableCode = "Literature";
                viewModel.AppUserItemFolderID = appUserItemFolderId;
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Literature [{0}]", viewModel.Entity.ID);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(LiteratureViewModel viewModel)
        {
            try
            {
                viewModel.Entity.TableName = viewModel.TableName;

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
                return RedirectToAction("Edit", "Literature", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult FolderItems(int folderId)
        {
            throw new NotImplementedException();
        }
        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            LiteratureViewModel viewModel = new LiteratureViewModel();
            try
            {
                viewModel.TableName = "literature";
                viewModel.TableCode = "Literature";
                viewModel.EventAction = "Search";
                viewModel.PageTitle = "Literature Search";

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<LiteratureSearch>(appUserItemListViewModel.Entity.Properties);
                    viewModel.Search();
                }

                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public ActionResult Search(LiteratureViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();

                // Save search if attribs supplied.
                if ((viewModel.EventAction == "SEARCH") && (viewModel.EventValue == "SAVE"))
                {
                    viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                    //viewModel.SaveSearch();
                }

                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection formCollection)
        {
            string partialViewName = BASE_PATH + "/Modals/_SelectList.cshtml";
            LiteratureViewModel viewModel = new LiteratureViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["TableName"]))
                {
                    viewModel.SearchEntity.TableName = formCollection["TableName"];
                }

                if (!String.IsNullOrEmpty(formCollection["LiteratureTypeCode"]))
                {
                    viewModel.SearchEntity.LiteratureTypeCode = formCollection["LiteratureTypeCode"];
                }

                if (!String.IsNullOrEmpty(formCollection["StandardAbbreviation"]))
                {
                    viewModel.SearchEntity.StandardAbbreviation = formCollection["StandardAbbreviation"];
                }

                if (!String.IsNullOrEmpty(formCollection["Abbreviation"]))
                {
                    viewModel.SearchEntity.Abbreviation = formCollection["Abbreviation"];
                }

                if (!String.IsNullOrEmpty(formCollection["ReferenceTitle"]))
                {
                    viewModel.SearchEntity.ReferenceTitle = formCollection["ReferenceTitle"];
                }

                if (!String.IsNullOrEmpty(formCollection["EditorAuthorName"]))
                {
                    viewModel.SearchEntity.EditorAuthorName = formCollection["EditorAuthorName"];
                }

                if (!String.IsNullOrEmpty(formCollection["PublicationYear"]))
                {
                    viewModel.SearchEntity.PublicationYear = formCollection["PublicationYear"];
                }

                viewModel.Search();
                return PartialView(partialViewName, viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult RenderLookupModal(string isMultiSelect = "")
        {
            LiteratureViewModel viewModel = new LiteratureViewModel();
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                LiteratureViewModel viewModel = new LiteratureViewModel();
                viewModel.Entity.ID = Int32.Parse(GetFormFieldValue(formCollection, "EntityID"));
                viewModel.TableName = GetFormFieldValue(formCollection, "TableName");
                viewModel.Delete();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
