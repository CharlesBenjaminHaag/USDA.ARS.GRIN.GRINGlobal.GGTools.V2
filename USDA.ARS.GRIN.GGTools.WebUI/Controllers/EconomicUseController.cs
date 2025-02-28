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
    public class EconomicUseController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/EconomicUse/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            EconomicUseViewModel viewModel = new EconomicUseViewModel();
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
            EconomicUseViewModel viewModel = new EconomicUseViewModel();

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
                EconomicUseViewModel viewModel = new EconomicUseViewModel();
                viewModel.EventAction = "Add";
                viewModel.EventValue = "EconomicUse";
                viewModel.TableName = "taxonomy_use";
                viewModel.PageTitle = "Add Economic Use";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                if (speciesId > 0)
                {
                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                    speciesViewModel.SearchEntity = new SpeciesSearch { ID = speciesId };
                    speciesViewModel.Search();
                    viewModel.Entity.SpeciesID = speciesViewModel.Entity.ID;
                    viewModel.Entity.SpeciesName = speciesViewModel.Entity.AssembledName;
                }

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
                EconomicUseViewModel viewModel = new EconomicUseViewModel();
                viewModel.TableName = "taxonomy_use";
                viewModel.TableCode = "EconomicUse";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Economic Use [{0}]: {1}", entityId, viewModel.ToTitleCase(viewModel.Entity.AssembledName));
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add  Economic Use";
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
        public ActionResult  Edit(EconomicUseViewModel viewModel)
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
                return RedirectToAction("Edit", "EconomicUse", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult RenderBatchEditModal(string idList)
        {
            EconomicUseViewModel viewModel = new EconomicUseViewModel();
            viewModel.SearchEntity.IDList = idList;
            viewModel.Search();

            foreach (var economicUse in viewModel.DataCollection)
            {
                CitationViewModel citationViewModel = new CitationViewModel();
                citationViewModel.SearchEntity.SpeciesID = economicUse.SpeciesID;
                citationViewModel.Search();
                economicUse.Citations = citationViewModel.DataCollection;
                // TODO Edit species citations
            }
            return PartialView("~/Views/Taxonomy/EconomicUse/Modals/_EditBatch.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult BatchEdit(string keyList)
        {
            try
            {
                foreach (var key in keyList.Split(','))
                {
                    var keyTokens = key.Split('_');
                    var economicUseId = keyTokens[0];
                    var citationId = keyTokens[1];

                    EconomicUseViewModel viewModel = new EconomicUseViewModel();
                    viewModel.Get(Int32.Parse(economicUseId));
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
                EconomicUseViewModel viewModel = new EconomicUseViewModel();
                ViewBag.PageTitle = "Economic Use Search";

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as EconomicUseViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<EconomicUseSearch>(appUserItemListViewModel.Entity.Properties);
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
        public ActionResult Search(EconomicUseViewModel viewModel)
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
            EconomicUseViewModel viewModel = new EconomicUseViewModel();
            try 
            { 
                viewModel.SearchEntity.SpeciesID= speciesId;
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/EconomicUse/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult RenderEditModal(int speciesId = 0)
        {
            EconomicUseViewModel viewModel = new EconomicUseViewModel();
            viewModel.Entity.SpeciesID = speciesId;
            return PartialView("~/Views/EconomicUse/Modals/_Lookup.cshtml", viewModel);
        }
        public ActionResult RenderLookupModal(int speciesId = 0)
        {
            EconomicUseViewModel viewModel = new EconomicUseViewModel();
            viewModel.Entity.SpeciesID = speciesId;
            return PartialView("~/Views/EconomicUse/Modals/_Lookup.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult Add(FormCollection coll)
        {
            return null;
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection coll)
        {
            CitationViewModel vm = new CitationViewModel();

            if (!String.IsNullOrEmpty(coll["AbbreviatedLiteratureSource"]))
            {
                vm.SearchEntity.Abbreviation = coll["AbbreviatedLiteratureSource"];
            }

            if (!String.IsNullOrEmpty(coll["CitationTitle"]))
            {
                vm.SearchEntity.CitationTitle = coll["CitationTitle"];
            }

            if (!String.IsNullOrEmpty(coll["EditorAuthorName"]))
            {
                vm.SearchEntity.EditorAuthorName = coll["EditorAuthorName"];
            }

            if (!String.IsNullOrEmpty(coll["CitationYear"]))
            {
                vm.SearchEntity.CitationYear = Int32.Parse(coll["CitationYear"]);
            }

            if (!String.IsNullOrEmpty(coll["CitationType"]))
            {
                vm.SearchEntity.TypeCode = coll["CitationType"];
            }

            if (!String.IsNullOrEmpty(coll["TaxonIDList"]))
            {
                //TODO: GET TAXON ID VALUES
            }

            if (!String.IsNullOrEmpty(coll["TableName"]))
            {
                vm.SearchEntity.TableName = coll["TableName"];
                switch (vm.SearchEntity.TableName)
                {
                    case "taxonomy_family":
                        vm.SearchEntity.FamilyName = coll["TaxonName"];
                        break;
                    case "taxonomy_genus":
                        vm.SearchEntity.GenusName = coll["TaxonName"];
                        break;
                    case "taxonomy_species":
                        vm.SearchEntity.SpeciesName = coll["TaxonName"];
                        break;
                }
            }
            vm.Search();
            return PartialView("~/Views/Citation/_SelectList.cshtml", vm);
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                EconomicUseViewModel viewModel = new EconomicUseViewModel();
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
                EconomicUsageTypeViewModel viewModel = new EconomicUsageTypeViewModel();
                viewModel.GetEconomicUsageTypes(economicUsageCode);
                return PartialView("~/Views/Taxonomy/EconomicUsageType/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}
