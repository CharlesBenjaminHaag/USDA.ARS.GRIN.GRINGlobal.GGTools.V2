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
    public class SpeciesSynonymMapController : BaseController, IController<SynonymMapViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/SpeciesSynonymMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns synonym map records.
        /// </summary>
        /// <param name="speciesId"></param>
        /// <returns></returns>
        public PartialViewResult _ListSynonymsA(int speciesId)
        {
            try 
            { 
                SynonymMapViewModel viewModel = new SynonymMapViewModel();
                viewModel.SearchEntity.SpeciesBID = speciesId;
                viewModel.Search();
                return PartialView(BASE_PATH + "_ListSynonymsA.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSynonymsB(int speciesId)
        {
            try 
            { 
                SynonymMapViewModel viewModel = new SynonymMapViewModel();
                viewModel.SearchEntity.SpeciesAID = speciesId;
                viewModel.Search();
                return PartialView(BASE_PATH + "_ListSynonymsB.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        /// <summary>
        /// Returns all species that are synonyms of a specified species.
        /// </summary>
        /// <param name="speciesId"></param>
        /// <returns></returns>
        public PartialViewResult _ListSynonyms(int speciesId)
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
            viewModel.SearchEntity = new SpeciesSynonymMapSearch { SpeciesBID = speciesId };
            viewModel.Search();
            return PartialView(BASE_PATH + "_ListSynonyms.cshtml", viewModel);
        }

        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
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
        //public PartialViewResult _ListDynamicFolderItems(int folderId)
        //{
        //    AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
        //    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();

        //    try
        //    {
        //        appUserItemFolderViewModel.SearchEntity.ID = folderId;
        //        appUserItemFolderViewModel.Search();

        //        appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
        //        appUserItemListViewModel.GetDynamic();

        //        AuthorViewModel viewModel = new AuthorViewModel();
        //        viewModel.SearchEntity = viewModel.Deserialize<AuthorSearch>(appUserItemListViewModel.Entity.Properties);
        //        viewModel.Search();
        //        return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
        public ActionResult Add(int speciesId = 0, string synonymType = "")
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
            SpeciesViewModel speciesViewModel = new SpeciesViewModel();

            try
            {
                viewModel.TableName = "taxonomy_species_synonym_map";

                // Clear the session of any previously-created batches.
                if (Session["SPECIES-SYNONYM-MAPS"] != null)
                {
                    Session.Remove("SPECIES-SYNONYM-MAPS");
                }

                // If an entity (species) ID is sent via param, retrieve the species and set the corresponding 
                // VM attributes.
                if (speciesId > 0)
                {
                    speciesViewModel.SearchEntity.ID = speciesId;
                    speciesViewModel.Search();
                    viewModel.SearchEntity.SpeciesAID = speciesId;
                    viewModel.SearchEntity.SpeciesAName = speciesViewModel.DataCollection[0].Name;
                }
                else
                {
                }
                return View(BASE_PATH + "Map.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult Add(FormCollection formCollection)
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
            List<SpeciesSynonymMap> batchedSpeciesSynonymMaps = new List<SpeciesSynonymMap>();
            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

            if (!String.IsNullOrEmpty(formCollection["SynonymCode"]))
            {
                viewModel.Entity.SynonymCode = formCollection["SynonymCode"];
            }

            if (!String.IsNullOrEmpty(formCollection["SpeciesIDListSubject"]))
            {
                viewModel.SpeciesIDListSubject = formCollection["SpeciesIDListSubject"];
            }

            if (!String.IsNullOrEmpty(formCollection["SpeciesIDListPredicate"]))
            {
                viewModel.SpeciesIDListPredicate = formCollection["SpeciesIDListPredicate"];
            }

            viewModel.InsertMultiple();

            // Add each generated batch to the session-stored list.
            if (Session["SPECIES-SYNONYM-MAPS"] != null)
            {
                batchedSpeciesSynonymMaps = Session["SPECIES-SYNONYM-MAPS"] as List<SpeciesSynonymMap>;
            }
            batchedSpeciesSynonymMaps.AddRange(viewModel.DataCollection);
            Session["SPECIES-SYNONYM-MAPS"] = batchedSpeciesSynonymMaps;
            viewModel.DataCollectionBatch = batchedSpeciesSynonymMaps;

            return PartialView("~/Views/Taxonomy/SpeciesSynonymMap/_ListBatch.cshtml", viewModel);
        }

        public PartialViewResult Clear(FormCollection formCollection)
        {
            if (Session["SPECIES-SYNONYM-MAPS"] != null)
            {
                Session.Remove("SPECIES-SYNONYM-MAPS");
                SynonymMapViewModel viewModel = new SynonymMapViewModel();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            return PartialView();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public ActionResult Edit(SynonymMapViewModel viewModel)
        {
            try 
            { 
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(BASE_PATH + "Edit.cshtml", viewModel);
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
                return RedirectToAction("Edit", "SpeciesSynonymMap", new { entityId = viewModel.Entity.ID });
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
                SynonymMapViewModel viewModel = new SynonymMapViewModel();
                viewModel.EventAction = "SpeciesSynonymMap";
                viewModel.TableName = "taxonomy_species_synonym_map";
                viewModel.TableCode = "SpeciesSynonymMap";
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit [{0}]", viewModel.Entity.ID);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
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

        public ActionResult Index()
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
            try 
            {
                viewModel.EventAction = "SpeciesSynonymMap";
                viewModel.TableName = "taxonomy_species_synonym_map";
                viewModel.PageTitle = "Synonym Map Search";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Search(SynonymMapViewModel viewModel)
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

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                SynonymMapViewModel viewModel = new SynonymMapViewModel();
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