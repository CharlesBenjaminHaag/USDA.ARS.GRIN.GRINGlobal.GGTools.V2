using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using DataTables;
using NLog;
    
namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class RegulationMapController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/RegulationMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
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
            RegulationMapViewModel viewModel = new RegulationMapViewModel();

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
        public ActionResult Index(RegulationMapViewModel vm)
        {
            vm.Search();
            ModelState.Clear();
            return View(vm);
        }

        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            viewModel = LoadFromSession(viewModel);

            try
            {
                viewModel.PageTitle = "Regulation Map Search";
                viewModel.TableName = "taxonomy_regulation_map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<RegulationMapSearch>(appUserItemListViewModel.Entity.Properties);
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

        public PartialViewResult _List(string eventValue = "", int speciesId = 0, int regulationId = 0, int geographyId = 0)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            viewModel.SearchEntity.SpeciesID = speciesId;
            viewModel.SearchEntity.RegulationID = regulationId;
            viewModel.SearchEntity.GeographyID = geographyId;
            viewModel.Search();
            return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        }
             
        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(formCollection["EntityID"]))
                {
                    viewModel.Entity.SpeciesID = Int32.Parse(formCollection["EntityID"]);
                }
                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.Entity.ItemIDList = formCollection["IDList"];
                }
                viewModel.Insert();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Add(int regulationId = 0, int speciesId = 0, int genusId = 0)
        {
            try
            {
                RegulationMapViewModel viewModel = new RegulationMapViewModel();
                viewModel.Entity.RegulationID = regulationId;
                viewModel.Entity.SpeciesID = speciesId;
                viewModel.TableName = "taxonomy_regulation_map";
                viewModel.PageTitle = "Add Regulation Map";
                viewModel.Entity.RegulationID = regulationId;

                if (speciesId > 0)
                {
                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                    speciesViewModel.SearchEntity = new SpeciesSearch { ID = speciesId };
                    speciesViewModel.Search();
                    viewModel.Entity.SpeciesID = speciesViewModel.Entity.ID;
                    viewModel.Entity.SpeciesName = speciesViewModel.Entity.AssembledName;
                }
                else
                {
                    GenusViewModel genusViewModel = new GenusViewModel();
                    genusViewModel.SearchEntity = new GenusSearch { ID = genusId };
                    genusViewModel.Search();
                    viewModel.Entity.GenusID = genusViewModel.Entity.ID;
                    viewModel.Entity.GenusName = genusViewModel.Entity.AssembledName;
                }

                return View( BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public ActionResult Edit(int entityId = 0)
        {
            try
            {
                RegulationMapViewModel viewModel = new RegulationMapViewModel();
                viewModel.TableName = "taxonomy_regulation_map";
                viewModel.TableCode = "RegulationMap";
                viewModel.EventAction = "EDIT";
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Regulation Map [{0}]: {1}", entityId, viewModel.Entity.AssembledName);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        [HttpPost]
        public ActionResult Edit(RegulationMapViewModel viewModel)
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
                return RedirectToAction("Edit", "RegulationMap", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [ValidateInput(false)]
        public JsonResult EditBatch()
        {
            string idList = String.Empty;
            var request = System.Web.HttpContext.Current.Request;

            if (Session["REGULATION_MAP_ID_LIST"] != null)
            {
                idList = Session["REGULATION_MAP_ID_LIST"].ToString();
            }

            string[] idArray = idList.Split(',');

            try
            {
                using (RegulationMapManager mgr = new RegulationMapManager())
                {
                    using (var db = new Database("sqlserver", mgr.ConnectionString))
                    {
                        var editor = new Editor(db, "taxonomy_regulation_map", "taxonomy_regulation_map.taxonomy_regulation_map_id").Where(q =>
                        {
                            q.Where(r =>
                            {
                                foreach (var i in idArray)
                                {
                                    r.OrWhere("taxonomy_regulation_map.taxonomy_regulation_map_id", i);
                                }
                            });
                        })
                        .Model<RegulationMapTable>("taxonomy_regulation_map")
                        .Model<RegulationTable>("taxonomy_regulation")
                        .Model<SpeciesTable>("taxonomy_species")
                        .Model<GenusTable>("taxonomy_genus")
                        .Model<FamilyTable>("taxonomy_family")
                        .LeftJoin("taxonomy_regulation", "taxonomy_regulation.taxonomy_regulation_id", "=", "taxonomy_regulation_map.taxonomy_regulation_id")
                        .LeftJoin("taxonomy_family", "taxonomy_family.taxonomy_family_id", "=", "taxonomy_regulation_map.taxonomy_family_id")
                        .LeftJoin("taxonomy_genus", "taxonomy_genus.taxonomy_genus_id", "=", "taxonomy_regulation_map.taxonomy_genus_id")
                        .LeftJoin("taxonomy_species", "taxonomy_species.taxonomy_species_id", "=", "taxonomy_regulation_map.taxonomy_species_id");

                        editor.Field(new Field("taxonomy_regulation_map.taxonomy_regulation_map_id")
                            .Validator(Validation.NotEmpty())
                        );
                        editor.Field(new Field("taxonomy_species.name"));
                        editor.Field(new Field("taxonomy_genus.genus_name"));
                        editor.Field(new Field("taxonomy_family.family_name"));
                        editor.Field(new Field("taxonomy_regulation_map.taxonomy_family_id"));
                        editor.Field(new Field("taxonomy_regulation_map.taxonomy_genus_id"));
                        editor.Field(new Field("taxonomy_regulation_map.taxonomy_species_id"));
                        editor.Field(new Field("taxonomy_regulation_map.taxonomy_regulation_id"));
                        editor.Field(new Field("taxonomy_regulation_map.is_exempt"));
                        editor.Field(new Field("taxonomy_regulation_map.note"));
                        editor.Field(new Field("taxonomy_regulation_map.modified_date")
                            .Set(Field.SetType.Edit));
                        editor.PreEdit += (sender, e) => editor.Field("taxonomy_regulation_map.modified_date").SetValue(DateTime.Now);
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

        [HttpPost]
        public JsonResult SaveBatch(int regulationMapId = 0, int familyId = 0, int genusId = 0, int speciesId = 0, int regulationId = 0)
        {
            try
            {
                RegulationMapViewModel viewModel = new RegulationMapViewModel();
                viewModel.Get(regulationMapId);
                if (viewModel.DataCollection.Count == 0)
                {
                    throw new IndexOutOfRangeException("No regulation found for id " + regulationMapId);
                }

                if (familyId > 0)
                {
                    viewModel.Entity.FamilyID = familyId;
                }

                if (genusId > 0)
                {
                    viewModel.Entity.GenusID = genusId;
                }

                if (speciesId > 0)
                {
                    viewModel.Entity.SpeciesID = speciesId;
                }

                if (regulationId > 0)
                {
                    viewModel.Entity.RegulationID = regulationId;
                }
                viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Update();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetBatchEditor(RegulationMapViewModel viewModel)
        {
            try
            {
                Session["REGULATION_MAP_ID_LIST"] = viewModel.ItemIDList;
                return View("~/Views/Taxonomy/RegulationMap/EditBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult _Search(int id = 0)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            return PartialView(BASE_PATH + "_Search.cshtml", viewModel);
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Search(RegulationMapViewModel viewModel)
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

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        // ======================================================================================
        // MODALS 
        // ======================================================================================
        public ActionResult RenderLookupModal()
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            return PartialView("~/Views/RegulationMap/Modals/_Lookup.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection coll)
        {
            CitationViewModel vm = new CitationViewModel();

            //formData.append("AbbreviatedLiteratureSource", abbreviatedLiteratureSource);
            //formData.append("CitationTitle", citationTitle);
            //formData.append("EditorAuthorName", editorAuthorName);
            //formData.append("CitationYear", citationYear);

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
                RegulationMapViewModel viewModel = new RegulationMapViewModel();
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

        public RegulationMapViewModel LoadFromSession(RegulationMapViewModel viewModel)
        {
            string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
            if (Session[targetKey] != null)
            {
                viewModel = Session[targetKey] as RegulationMapViewModel;
            }
            return viewModel;
        }
    }
}
