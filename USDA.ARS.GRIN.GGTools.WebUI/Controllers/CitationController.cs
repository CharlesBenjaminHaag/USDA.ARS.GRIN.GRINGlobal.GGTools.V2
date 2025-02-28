using NLog;
using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.WebUI;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CitationController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Citation/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public override PartialViewResult PageMenu(string eventAction, string eventValue, string sysTableName = "", string sysTableTitle = "", int entityId = 0)
        {
            ViewBag.EventAction = eventAction;
            ViewBag.EventValue = eventValue;

            if (eventValue == "Edit")
            {
                return PartialView("~/Views/Taxonomy/Citation/Components/_EditMenu.cshtml");
            }
            else
            {
                if (!String.IsNullOrEmpty(sysTableName))
                {
                    return PartialView("~/Views/Components/_DefaultSearchMenu.cshtml");
                }
                else
                {
                    return PartialView("~/Views/Components/_DefaultMenu.cshtml");
                }
            }
        }

        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            CitationViewModel viewModel = new CitationViewModel();
            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.TableName = "citation";
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

        //        CitationViewModel viewModel = new CitationViewModel();
        //        viewModel.SearchEntity = viewModel.Deserialize<CitationSearch>(appUserItemListViewModel.Entity.Properties);
        //        viewModel.Search();
        //        return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
        [HttpPost]
        public ActionResult Search(CitationViewModel viewModel)
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
                    viewModel.EventValue = "";
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
        public PartialViewResult _Search(CitationViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _List(int familyId = 0, int genusId = 0, int speciesId = 0, int literatureId = 0)
        {
            CitationViewModel viewModel = new CitationViewModel();
            viewModel.SearchEntity.FamilyID = familyId;
            viewModel.SearchEntity.GenusID = genusId;
            viewModel.SearchEntity.SpeciesID = speciesId;
            viewModel.SearchEntity.LiteratureID = literatureId;
            viewModel.Search();
            return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        }

        public PartialViewResult _ListBySpecies(int speciesId)
        {
            CitationViewModel viewModel = new CitationViewModel();
            viewModel.SearchEntity.SpeciesID = speciesId;
            viewModel.Search();
            return PartialView(BASE_PATH + "Modals/_SelectListBySpecies.cshtml", viewModel);
        }
        //[HttpPost]
        //public PartialViewResult _List(FormCollection formCollection)
        //{
        //    CitationViewModel viewModel = new CitationViewModel();

        //    if (!String.IsNullOrEmpty(formCollection["FamilyID"]))
        //    {
        //        viewModel.SearchEntity.FamilyID = Int32.Parse(formCollection["FamilyID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["GenusID"]))
        //    {
        //        viewModel.SearchEntity.GenusID = Int32.Parse(formCollection["GenusID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["SpeciesID"]))
        //    {
        //        viewModel.SearchEntity.SpeciesID = Int32.Parse(formCollection["SpeciesID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["FormatCode"]))
        //    {
        //        viewModel.EventValue = formCollection["FormatCode"];
        //    }

        //    viewModel.Search();
        //    return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        //}

        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            try
            {
                CitationViewModel viewModel = new CitationViewModel();

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as CitationViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    viewModel.RunSearch(folderId);
                }

                viewModel.PageTitle = "Citation Search";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult RenderCloneWidget()
        {
            CitationViewModel viewModel = new CitationViewModel();
            try
            {
                CitationViewModel cloneViewModel = new CitationViewModel();
                return PartialView("~/Views/Taxonomy/Citation/_Clone.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderReferenceCountsWidget(int citationId)
        {
            CitationViewModel viewModel = new CitationViewModel();
            viewModel.Entity.ID = citationId;
            viewModel.GetCitationReferenceCounts(citationId);
            return PartialView("~/Views/Taxonomy/Citation/_WidgetCitationReferences.cshtml", viewModel);
        }
        
        public PartialViewResult _AddClone(int entityId = 0, string eventAction = "add", string eventValue = "")
        {
            CitationViewModel viewModel = new CitationViewModel();
            try
            {
                viewModel.SearchEntity.ID = entityId;
                viewModel.Search();

                CitationViewModel cloneViewModel = new CitationViewModel();
                cloneViewModel.Entity = viewModel.Entity;
                cloneViewModel.EventAction = eventAction;
                cloneViewModel.EventValue = eventValue;
                return PartialView("~/Views/Taxonomy/Citation/_Clone.cshtml", cloneViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
                
        public PartialViewResult _Clone(int entityId)
        {
            CitationViewModel viewModel = new CitationViewModel();
            try
            {
                CitationViewModel cloneViewModel = new CitationViewModel();
                return PartialView("~/Views/Taxonomy/Citation/_Clone.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _Edit(int entityId = 0, int familyId = 0, int genusId = 0, int speciesId = 0, string isClone = "N")
        {
            CitationViewModel viewModel = new CitationViewModel();
            try
            {
                viewModel.TableName = "citation";
                viewModel.TableCode = "Citation";
                viewModel.EventAction = "Edit";

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                }
                else
                {
                    viewModel.Entity.FamilyID = familyId;
                    viewModel.Entity.GenusID = genusId;
                    viewModel.Entity.SpeciesID = speciesId;
                }

                if (viewModel.Entity.FamilyID > 0) viewModel.EventValue = "taxonomy_family_map";
                if (viewModel.Entity.GenusID > 0) viewModel.EventValue = "taxonomy_genus";
                if (viewModel.Entity.SpeciesID > 0) viewModel.EventValue = "taxonomy_species";
                return PartialView("~/Views/Taxonomy/Citation/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        [HttpPost]
        public PartialViewResult _Edit(CitationViewModel viewModel)
        {
            try
            {
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

                // Re-retrieve new/changed cit.
                viewModel.Get(viewModel.Entity.ID);

                return PartialView("~/Views/Taxonomy/Citation/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                CitationViewModel viewModel = new CitationViewModel();
                viewModel.TableName = "citation";
                viewModel.TableCode = "Citation";
                viewModel.EventAction = "Edit";
                viewModel.AppUserItemFolderID = appUserItemFolderId;

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.PageTitle = String.Format("Edit Citation [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.AssembledName);
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Citation");
                }
                return View(BASE_PATH + "Edit.cshtml",  viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(CitationViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(BASE_PATH + "Edit.cshtml", viewModel);
                }

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
                return RedirectToAction("Edit", "Citation", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
       
        //public PartialViewResult _Save(CitationViewModel viewModel)
        //{
        //    try
        //    {
        //        viewModel.Entity.TableName = viewModel.TableName;

        //        //if (!viewModel.Validate())
        //        //{
        //        //    if (viewModel.ValidationMessages.Count > 0) return Edit(viewModel);
        //        //}

        //        if (viewModel.Entity.ID == 0)
        //        {
        //            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
        //            viewModel.Insert();
        //        }
        //        else
        //        {
        //            viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
        //            viewModel.Update();
        //        }
        //        return PartialView("~/Views/Taxonomy/Citation/_Edit.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        public ActionResult Add(int literatureId = 0, int familyMapId = 0, int genusId = 0, int speciesId = 0, string eventValue = "")
        {
            try 
            { 
                CitationViewModel viewModel = new CitationViewModel();
                viewModel.EventAction = "Add";
                viewModel.EventValue = eventValue;
                viewModel.TableName = "citation";
                viewModel.TableCode = "Citation";
                viewModel.Entity.LiteratureID = literatureId;
                viewModel.Entity.FamilyID = familyMapId;
                viewModel.Entity.GenusID = genusId;
                viewModel.Entity.SpeciesID = speciesId;
                viewModel.PageTitle = viewModel.EventAction + " " + viewModel.TableCode;
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.CreatedByCooperatorName = AuthenticatedUser.FullName;
                viewModel.Entity.CreatedDate = System.DateTime.Now;

                if (familyMapId > 0)
                {
                    viewModel.Entity.FamilyID = familyMapId;
                    FamilyMapViewModel familyMapViewModel = new FamilyMapViewModel();
                    familyMapViewModel.SearchEntity.ID = familyMapId;
                    familyMapViewModel.Search();
                    viewModel.Entity.FamilyID = familyMapViewModel.Entity.ID;
                    viewModel.Entity.FamilyName = familyMapViewModel.Entity.AssembledName;
                }

                if (genusId > 0)
                {
                    GenusViewModel genusViewModel = new GenusViewModel();
                    genusViewModel.SearchEntity.ID = genusId;
                    genusViewModel.Search();
                    viewModel.Entity.GenusID = genusViewModel.Entity.ID;
                    viewModel.Entity.GenusName = genusViewModel.Entity.AssembledName;
                }

                if (speciesId > 0)
                {
                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                    speciesViewModel.SearchEntity.ID = speciesId;
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

        public ActionResult Clone(int entityId)
        {
            CitationViewModel viewModel = new CitationViewModel();
            CitationViewModel cloneViewModel = new CitationViewModel();
            viewModel.GetClone(entityId);
            cloneViewModel.Entity = viewModel.Entity;
            return View("~/Views/Taxonomy/Citation/Edit.cshtml", cloneViewModel);
        }

        /// <summary>
        /// Creates a clone of a speficied citation, and links a specified record to that citation.
        /// </summary>
        /// <param name="tableName">The table to update</param>
        /// <param name="entityId">The ID of the record to update</param>
        /// <param name="citationId">The citation to clone</param>
        /// <returns>Success: the ID of the cloned citation
        /// Error: TBD</returns>
        /// <remarks>
        /// Relevant to tables that are child tables of taxonomy_species. These tables, unlike
        /// taxon tables, are linked to 0 or 1 citations.
        /// </remarks>
        [HttpPost]
        public JsonResult AddSpeciesCitation(string tableName, int entityId, int citationId)
        {
            try
            {
                CitationViewModel viewModel = new CitationViewModel();
                viewModel.UpdateSpeciesCitation(tableName, entityId, citationId, AuthenticatedUser.CooperatorID);
                return Json(new { citationId = citationId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = "false" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            CitationViewModel viewModel = new CitationViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.Entity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["EntityID"]))
            {
                viewModel.ReferencedEntityID = Int32.Parse(formCollection["EntityID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["LiteratureID"]))
            {
                viewModel.Entity.LiteratureID = Int32.Parse(formCollection["LiteratureID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["CitationID"]))
            {
                viewModel.Entity.CitationID = Int32.Parse(formCollection["CitationID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["IDList"]))
            {
                viewModel.Entity.ItemIDList = formCollection["IDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["EntityIDList"]))
            {
                viewModel.EntityIDList = formCollection["EntityIDList"];
            }

            //if (!String.IsNullOrEmpty(formCollection["StandardAbbreviation"]))
            //{
            //    viewModel.Entity.StandardAbbreviation = formCollection["StandardAbbreviation"];
            //}

            if (!String.IsNullOrEmpty(formCollection["CitationTitle"]))
            {
                viewModel.Entity.CitationTitle = formCollection["CitationTitle"];
            }

            //if (!String.IsNullOrEmpty(formCollection["EditorAuthorName"]))
            //{
            //    viewModel.Entity.EditorAuthorName = formCollection["EditorAuthorName"];
            //}

            if (!String.IsNullOrEmpty(formCollection["CitationYear"]))
            {
                viewModel.Entity.CitationYear = Int32.Parse(formCollection["CitationYear"]);
            }

            if (!String.IsNullOrEmpty(formCollection["DOIReference"]))
            {
                viewModel.Entity.DOIReference = formCollection["DOIReference"];
            }

            if (!String.IsNullOrEmpty(formCollection["VolumeOrPage"]))
            {
                viewModel.Entity.Reference = formCollection["VolumeOrPage"];
            }

            if (!String.IsNullOrEmpty(formCollection["Note"]))
            {
                viewModel.Entity.Note = formCollection["Note"];
            }

            switch (viewModel.Entity.TableName)
            {
                case "taxonomy_family_map":
                    viewModel.Entity.FamilyID = viewModel.ReferencedEntityID;
                    break;
                case "taxonomy_genus":
                    viewModel.Entity.GenusID = viewModel.ReferencedEntityID;
                    break;
                case "taxonomy_species":
                    viewModel.Entity.SpeciesID = viewModel.ReferencedEntityID;
                    break;
            }

            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.Insert();
            viewModel.Get(viewModel.Entity.ID);

            return Json(new { citation = viewModel.Entity }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AddBatch(string speciesIdList, string literatureIdList)
        {
            CitationViewModel viewModel = new CitationViewModel();
            string[] speciesIdListArray = speciesIdList.Split(',');
            string[] literatureIdListArray = literatureIdList.Split(',');
            try
            {
                // TODO
                // For each species in list: create a new citation linked to that 
                // species, based on the selected literature ID.
                foreach (var speciesId in speciesIdListArray)
                {
                    foreach (var literatureId in literatureIdListArray)
                    {
                        viewModel.Entity.ID = 0;
                        viewModel.Entity.SpeciesID = Int32.Parse(speciesId);
                        viewModel.Entity.LiteratureID = Int32.Parse(literatureId);
                        viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                        viewModel.Insert();
                    }
                }

                viewModel.SearchEntity.SpeciesIDList = speciesIdList;
                viewModel.Search();

                return PartialView("~/Views/Taxonomy/Citation/_ListBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult BatchEdit()
        {
            CitationViewModel viewModel = new CitationViewModel();
            return PartialView("~/Views/Taxonomy/Citation/_EditBatch.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult BatchEdit(CitationViewModel viewModel)
        {
            // TODO
            // Iterate through species ID list
            // For each species, create cit with lit ID specified in search
            // entity obj
            return PartialView("~/Views/Taxonomy/Literature/_EditBatch.cshtml", viewModel);
        }

        public PartialViewResult RenderBatchEditModal(string entityIdList)
        {
            CitationViewModel viewModel = new CitationViewModel();
            int convertedSpeciesId = 0;

            try
            {
                // TODO
                SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                
                string[] speciesIdList = entityIdList.Split(',');
                foreach (var speciesId in speciesIdList)
                {
                    convertedSpeciesId = Int32.Parse(speciesId);
                    if (convertedSpeciesId > 0)
                    {
                        speciesViewModel.Get(Int32.Parse(speciesId));
                        viewModel.DataCollectionSpecies.Add(speciesViewModel.Entity);
                        viewModel.SearchEntity.SpeciesID = Int32.Parse(speciesId);
                        viewModel.Get(convertedSpeciesId);
                    }
                }

                return PartialView("~/Views/Taxonomy/Citation/_EditBatch2.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        /// <summary>
        /// Creates a copy of a specified citation, and renders a view allowing a side-by-side
        /// comparison and edit.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        //public ActionResult Clone(int entityId)
        //{
        //    CitationViewModel viewModel = new CitationViewModel();
        //    CitationViewModel viewModelClone = new CitationViewModel();
        //    try
        //    {
        //        viewModel.Edit(entityId);
        //        viewModelClone.Entity = viewModel.Entity;
        //        viewModelClone.TableName = "citation";
        //        viewModelClone.TableCode = "Citation";
        //        viewModelClone.PageTitle = "Add Citation (Clone)";
        //        viewModelClone.Entity.FamilyID = 0;
        //        viewModelClone.Entity.FamilyName = String.Empty;
        //        viewModelClone.Entity.GenusID = 0;
        //        viewModelClone.Entity.GenusName = String.Empty;
        //        viewModelClone.Entity.SpeciesID = 0;
        //        viewModelClone.Entity.SpeciesName = String.Empty;
        //        return Edit(BASE_PATH + "/Clone.cshtml", viewModelClone);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return RedirectToAction("InternalServerError", "Error");
        //    }
        //}
        //public ActionResult Clone(int entityId, string eventAction="", string eventValue="")
        //{
        //    // Retrieve citation to be cloned.
        //    CitationViewModel viewModel = new CitationViewModel();
        //    CitationViewModel cloneViewModel = new CitationViewModel();

        //    viewModel.SearchEntity.ID = entityId;
        //    viewModel.Search();

        //    // Create copy of source citation, resetting taxon attributes.
        //    viewModel.CloneEntity = viewModel.Entity;
        //    viewModel.EventAction = eventAction;
        //    viewModel.EventValue = eventValue;
        //    viewModel.CloneEntity.FamilyID = 0;
        //    viewModel.CloneEntity.FamilyName = String.Empty;
        //    viewModel.CloneEntity.GenusID = 0;
        //    viewModel.CloneEntity.GenusName = String.Empty;
        //    viewModel.CloneEntity.SpeciesID = 0;
        //    viewModel.CloneEntity.SpeciesName = String.Empty;
        //    return Edit(BASE_PATH + "Clone.cshtml", viewModel);
        //}
        
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(int folderId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        // ======================================================================================
        // MODALS 
        // ======================================================================================
        public PartialViewResult RenderLookupModal(string tableName = "", int entityId = 0, int familyId = 0, int genusId = 0, int speciesId = 0, string isMultiSelect = "")
        {
            CitationViewModel viewModel = new CitationViewModel();
            viewModel.IsMultiSelect = isMultiSelect;

            // If the table name is not a taxon table, load species citations.
            if ((tableName != "taxonomy_species") && 
                (tableName != "taxonomy_genus") && 
                tableName != "taxonomy_family_map")
                {
                if (speciesId > 0)
                {
                    viewModel.SearchEntity.SpeciesID = speciesId;
                    viewModel.Search();
                }
            }
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }
        public PartialViewResult RenderSpeciesCitationLookupModal(int speciesId, string tableName = "", string eventAction = "", string eventValue = "")
        {
            CitationViewModel viewModel = new CitationViewModel();

            // TODO
            // Configure modal based on context.
            // eventAction = controller
            // eventValue = action

            // eventAction  eventValue  Note
            // ===========  ==========  ====
            // CommonName   Edit        Create cit for single species and common name
            // CommonName   Search      Create cit for each selected common name
            // EconomicUse  Edit
            // GeographyMap Edit

            viewModel.TableName = tableName;
            viewModel.ParentTableName = tableName;
            viewModel.EventAction = eventAction;
            viewModel.EventValue = eventValue;
            viewModel.GetSpeciesCitations(speciesId, tableName);
            return PartialView(BASE_PATH + "/Modals/_SpeciesCitationLookup.cshtml", viewModel);
        }
        public PartialViewResult RenderEditModal(int entityId)
        {
            CitationViewModel viewModel = new CitationViewModel();

            if (entityId > 0)
            {
                viewModel.Get(entityId);
            }
            return PartialView("~/Views/Taxonomy/Citation/Modals/_Edit.cshtml", viewModel);
        }

        public PartialViewResult RenderWidget(int entityId = 0)
        {
            CitationViewModel viewModel = new CitationViewModel();

            try
            {
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                }
                return PartialView("~/Views/Taxonomy/Citation/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
   
        [HttpPost]
        public PartialViewResult Lookup(FormCollection coll)
        {
            CitationViewModel vm = new CitationViewModel();

            try
            {
                //if (!String.IsNullOrEmpty(coll["SpeciesID"]))
                //{
                //    vm.SearchEntity.SpeciesID = Int32.Parse(coll["SpeciesID"]);
                //}
                if (!String.IsNullOrEmpty(coll["TypeCode"]))
                {
                    vm.SearchEntity.TypeCode = coll["TypeCode"];
                }

                if (!String.IsNullOrEmpty(coll["LiteratureTypeCode"]))
                {
                    vm.SearchEntity.LiteratureTypeCode = coll["LiteratureTypeCode"];
                }

                if (!String.IsNullOrEmpty(coll["StandardAbbreviation"]))
                {
                    vm.SearchEntity.StandardAbbreviation = coll["StandardAbbreviation"];
                }

                if (!String.IsNullOrEmpty(coll["Abbreviation"]))
                {
                    vm.SearchEntity.Abbreviation = coll["Abbreviation"];
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

                if (!String.IsNullOrEmpty(coll["IsMultiSelect"]))
                {
                    vm.IsMultiSelect = coll["IsMultiSelect"];
                }
      
                vm.Search();
                return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", vm);
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

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                CitationViewModel viewModel = new CitationViewModel();
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
