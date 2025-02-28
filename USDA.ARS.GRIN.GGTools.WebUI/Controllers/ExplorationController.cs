using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class ExplorationController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Exploration/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            ExplorationViewModel viewModel = new ExplorationViewModel();
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
            ExplorationViewModel viewModel = new ExplorationViewModel();

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
        
        public ActionResult Add(string eventValue = "", int speciesId = 0)
        {
            try
            {
                ExplorationViewModel viewModel = new ExplorationViewModel();
                viewModel.EventAction = "Add";
                viewModel.EventValue = "Exploration";
                viewModel.TableName = "taxonomy_use";
                viewModel.PageTitle = "Add Economic Use";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

               

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

        public ActionResult Edit(int entityId)
        {
            try
            {
                ExplorationViewModel viewModel = new ExplorationViewModel();
                
                viewModel.TableName = "exploration";
                viewModel.TableCode = "Exploration";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.GetExplorationMaps(entityId);
                    viewModel.EventAction = "Edit";
                    ViewBag.PageTitle = "Edit";
                }
                else
                {
                    viewModel.EventAction = "Add";
                    ViewBag.PageTitle = "Add";
                }
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(ExplorationViewModel viewModel)
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
                return RedirectToAction("Edit", "Exploration", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult BatchEdit(string keyList)
        {
            try
            {
                foreach (var key in keyList.Split(','))
                {
                    var keyTokens = key.Split('_');
                    var ExplorationId = keyTokens[0];
                    var citationId = keyTokens[1];

                    ExplorationViewModel viewModel = new ExplorationViewModel();
                    viewModel.Get(Int32.Parse(ExplorationId));
                    viewModel.Entity.CitationID = Int32.Parse(citationId);
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                return Json("TRUE", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json("FALSE", JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            try
            {
                ExplorationViewModel viewModel = new ExplorationViewModel();
                ViewBag.PageTitle = "Exploration Search";

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as ExplorationViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<ExplorationSearch>(appUserItemListViewModel.Entity.Properties);
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

        [HttpPost]
        public ActionResult Search(ExplorationViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
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

        public PartialViewResult _List(int speciesId = 0)
        {
            ExplorationViewModel viewModel = new ExplorationViewModel();
            try 
            { 
                //viewModel.SearchEntity.SpeciesID= speciesId;
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Exploration/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        
        public ActionResult RenderLookupModal(int speciesId = 0)
        {
            ExplorationViewModel viewModel = new ExplorationViewModel();
            //viewModel.Entity.SpeciesID = speciesId;
            return PartialView("~/Views/Exploration/Modals/_Lookup.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult Add(FormCollection coll)
        {
            return null;
        }

        //[HttpPost]
        //public PartialViewResult Lookup(FormCollection coll)
        //{
        //    CitationViewModel vm = new CitationViewModel();

        //    if (!String.IsNullOrEmpty(coll["AbbreviatedLiteratureSource"]))
        //    {
        //        vm.SearchEntity.Abbreviation = coll["AbbreviatedLiteratureSource"];
        //    }

        //    if (!String.IsNullOrEmpty(coll["CitationTitle"]))
        //    {
        //        vm.SearchEntity.CitationTitle = coll["CitationTitle"];
        //    }

        //    if (!String.IsNullOrEmpty(coll["EditorAuthorName"]))
        //    {
        //        vm.SearchEntity.EditorAuthorName = coll["EditorAuthorName"];
        //    }

        //    if (!String.IsNullOrEmpty(coll["CitationYear"]))
        //    {
        //        vm.SearchEntity.CitationYear = Int32.Parse(coll["CitationYear"]);
        //    }

        //    if (!String.IsNullOrEmpty(coll["CitationType"]))
        //    {
        //        vm.SearchEntity.TypeCode = coll["CitationType"];
        //    }

        //    if (!String.IsNullOrEmpty(coll["TaxonIDList"]))
        //    {
        //        //TODO: GET TAXON ID VALUES
        //    }

        //    if (!String.IsNullOrEmpty(coll["TableName"]))
        //    {
        //        vm.SearchEntity.TableName = coll["TableName"];
        //        switch (vm.SearchEntity.TableName)
        //        {
        //            case "taxonomy_family":
        //                vm.SearchEntity.FamilyName = coll["TaxonName"];
        //                break;
        //            case "taxonomy_genus":
        //                vm.SearchEntity.GenusName = coll["TaxonName"];
        //                break;
        //            case "taxonomy_species":
        //                vm.SearchEntity.SpeciesName = coll["TaxonName"];
        //                break;
        //        }
        //    }
        //    vm.Search();
        //    return PartialView("~/Views/Citation/_SelectList.cshtml", vm);
        //}

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                ExplorationViewModel viewModel = new ExplorationViewModel();
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
        
        [HttpPost]
        public PartialViewResult RenderWidget(string economicUsageCode)
        {
            try
            {
                
                return PartialView("~/Views/Taxonomy/EconomicUsageType/_Widget.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}
