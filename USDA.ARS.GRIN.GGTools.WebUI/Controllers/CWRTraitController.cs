using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using DataTables;
using NLog;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI
{
    [GrinGlobalAuthentication]
    public class CWRTraitController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/CWRTrait/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add(int cwrMapId = 0)
        {
            try
            {
                CWRTraitViewModel viewModel = new CWRTraitViewModel();
                viewModel.TableName = "taxonomy_cwr_trait";
                viewModel.PageTitle = "Add CWR Trait";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.CWRMapID = cwrMapId;
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
                CWRTraitViewModel viewModel = new CWRTraitViewModel();
                viewModel.TableName = "taxonomy_cwr_trait";
                viewModel.TableCode = "CWRTrait";
                viewModel.AppUserItemFolderID = appUserItemFolderId;

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.Entity.IsPotentialOption = viewModel.ToBool(viewModel.Entity.IsPotential);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit CWR Trait [{0}]: {1} {2} {3}", entityId, viewModel.Entity.CropForCWRName, viewModel.Entity.TraitClassTitle, viewModel.Entity.BreedingTypeTitle);
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add CWR Trait";
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
        public ActionResult Edit(CWRTraitViewModel viewModel)
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
                return RedirectToAction("Edit", "CWRTrait", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Index(string eventAction = "", int folderId = 0, int cwrMapId = 0, int citationId = 0)
        {
            CWRTraitViewModel viewModel = new CWRTraitViewModel();
            
            try 
            { 
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.PageTitle = "CWR Trait Search";
                viewModel.TableName = "taxonomy_cwr_trait";
                viewModel.TableCode = "CWRTrait";

                if (cwrMapId > 0)
                {
                    viewModel.SearchEntity.CWRMapID = cwrMapId;
                    viewModel.Search();
                }

                if (citationId > 0)
                {
                    viewModel.SearchEntity.CitationID = citationId;
                    viewModel.Search();
                }

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as CWRTraitViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<CWRTraitSearch>(appUserItemListViewModel.Entity.Properties);
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

        public PartialViewResult _List(int cwrMapId)
        {
            CWRTraitViewModel viewModel = new CWRTraitViewModel();
            try
            {
                viewModel.EventAction = "SEARCH";
                viewModel.SearchEntity.CWRMapID = cwrMapId;
                viewModel.Search();
                ModelState.Clear();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            CWRTraitViewModel viewModel = new CWRTraitViewModel();
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
            CWRTraitViewModel viewModel = new CWRTraitViewModel();

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
        public ActionResult Search(CWRTraitViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.EventAction = "SEARCH";
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
        
        public PartialViewResult Lookup()
        {
            CWRTraitViewModel viewModel = new CWRTraitViewModel();
            viewModel.TableName = "taxonomy_cwr_trait";
            return PartialView(BASE_PATH + "Modals/_Search.cshtml", viewModel);
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public PartialViewResult LookupNotes(FormCollection formCollection)
        {
            string partialViewName = "~/Views/CWRMap/Modals/_NoteSelectList.cshtml";
            CWRMapViewModel viewModel = new CWRMapViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.SearchEntity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["Note"]))
            {
                viewModel.SearchEntity.Note = formCollection["Note"];
            }

            viewModel.SearchNotes();
            return PartialView(partialViewName, viewModel);
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
                CWRTraitViewModel viewModel = new CWRTraitViewModel();
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
        public PartialViewResult RenderBatchEditModal(string idList)
        {
            CWRTraitViewModel viewModel = new CWRTraitViewModel();
            viewModel.SearchEntity.IDList = idList;
            viewModel.Search();

            foreach (var cwrTrait in viewModel.DataCollection)
            {
                CitationViewModel citationViewModel = new CitationViewModel();
                citationViewModel.SearchEntity.SpeciesID = cwrTrait.SpeciesID;
                citationViewModel.Search();
                cwrTrait.Citations = citationViewModel.DataCollection;
            }
            return PartialView("~/Views/Taxonomy/CWRTrait/Modals/_EditBatch.cshtml", viewModel);
        }

        public PartialViewResult RenderCloneModal(int cwrTraitId)
        {
            try
            {
                CWRTraitViewModel viewModel = new CWRTraitViewModel();
                viewModel.Get(cwrTraitId);

                return PartialView("~/Views/Taxonomy/CWRTrait/Modals/_Clone.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
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
                    var cwrMapId = keyTokens[0];
                    var citationId = keyTokens[1];

                    CWRTraitViewModel viewModel = new CWRTraitViewModel();
                    viewModel.Get(Int32.Parse(cwrMapId));
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

        [HttpPost]
        public PartialViewResult Clone(int cwrTraitId, int cwrMapId, int quantity)
        {
            CWRTraitViewModel viewModel = new CWRTraitViewModel();
            CWRTraitViewModel cloneViewModel = new CWRTraitViewModel();

            viewModel.Get(cwrTraitId);

            try
            {
                for (int i = 0; i < quantity; i++)
                {
                    cloneViewModel.Entity.ID = 0;
                    cloneViewModel.Entity.GenepoolCode = viewModel.Entity.GenepoolCode;
                    cloneViewModel.Entity.CWRMapID = cwrMapId;
                    cloneViewModel.Entity.TraitClassCode = viewModel.Entity.TraitClassCode;
                    cloneViewModel.Entity.SpeciesID = viewModel.Entity.SpeciesID;
                    cloneViewModel.Entity.BreedingTypeCode = viewModel.Entity.BreedingTypeCode;
                    cloneViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    cloneViewModel.Get(cloneViewModel.Insert());

                    //TODO GET CIT LIST HERE
                    CitationViewModel citationViewModel = new CitationViewModel();
                    citationViewModel.SearchEntity.SpeciesID = cloneViewModel.Entity.SpeciesID;
                    citationViewModel.Search();
                    cloneViewModel.Entity.Citations = citationViewModel.DataCollection;

                    cloneViewModel.DataCollectionBatch.Add(cloneViewModel.Entity);
                }
                return PartialView("~/Views/Taxonomy/CWRTrait/Modals/_CloneSelectList.cshtml", cloneViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }

        public JsonResult GetBatchEditor()
        {
            string idList = String.Empty;
            var request = System.Web.HttpContext.Current.Request;

            if (Session["CWR_TRAIT_ID_LIST"] != null)
            {
                idList = Session["CWR_TRAIT_ID_LIST"].ToString();
            }

            string[] idArray = idList.Split(',');

            try
            {
                using (SpeciesManager mgr = new SpeciesManager())
                {
                    using (var db = new Database("sqlserver", mgr.ConnectionString))
                    {
                        var editor = new Editor(db, "taxonomy_cwr_trait", "taxonomy_cwr_trait.taxonomy_cwr_trait_id").Where(q =>
                        {
                            q.Where(r =>
                            {
                                foreach (var i in idArray)
                                {
                                    r.OrWhere("taxonomy_cwr_trait.taxonomy_cwr_trait_id", i);
                                }
                            });
                        })
                        .Model<SpeciesTable>("taxonomy_cwr_trait")
                        .Model<GenusTable>("taxonomy_cwr_map")
                        .LeftJoin("taxonomy_cwr_map", "taxonomy_cwr_map.taxonomy_cwr_map_id", "=", "taxonomy_cwr_trait.taxonomy_cwr_map_id");

                        editor.Field(new Field("taxonomy_cwr_trait.taxonomy_cwr_trait_id")
                            .Validator(Validation.NotEmpty())
                        );
                         
                        editor.Field(new Field("taxonomy_cwr_trait.trait_class_code"));
                        editor.Field(new Field("taxonomy_cwr_trait.is_potential"));
                        editor.Field(new Field("taxonomy_cwr_trait.breeding_type_code"));
                        editor.Field(new Field("taxonomy_cwr_trait.breeding_usage_note"));
                        editor.Field(new Field("taxonomy_cwr_trait.ontology_trait_identifier"));
                        editor.Field(new Field("taxonomy_cwr_trait.modified_date")
                            .Set(Field.SetType.Edit));
                        editor.PreEdit += (sender, e) => editor.Field("taxonomy_cwr_trait.modified_date").SetValue(DateTime.Now);
                        editor.Process(request);

                        var response = editor.Data();

                        JsonResult jsonResult = new JsonResult();
                        jsonResult = Json(response);
                        jsonResult.MaxJsonLength = 2147483644;
                        jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                        return jsonResult;
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PostBatchEdits(string idList = "")
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                
                Session["SPECIES_ID_LIST"] = idList;
                return View("~/Views/Taxonomy/Species/EditMultiple_POC.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
    }
}
