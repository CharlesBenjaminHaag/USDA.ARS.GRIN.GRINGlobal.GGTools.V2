using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;
using DataTables;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class GenusController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Genus/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            GenusViewModel viewModel = new GenusViewModel();
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
            GenusViewModel viewModel = new GenusViewModel();

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

        public ActionResult Add(string eventAction="", string eventValue="", int familyId = 0, int genusId = 0, string isType="", string rank = "genus")
        {
            GenusViewModel viewModel = new GenusViewModel();
            viewModel.EventAction = eventAction;
            viewModel.EventValue = eventValue;
            viewModel.TableName = "taxonomy_genus";
            viewModel.TableCode = "Genus";
            viewModel.PageTitle = String.Format("Add {0}", viewModel.ToTitleCase(rank));
            viewModel.Entity.IsAcceptedName = "Y";
            viewModel.Entity.Rank = rank.ToUpper();
            viewModel.Entity.IsAccepted = true;
            viewModel.Entity.IsAcceptedName = "Y";
            viewModel.Entity.IsWebVisible = "Y";
            viewModel.Entity.IsWebVisibleOption = true;
            viewModel.Entity.FamilyID = familyId;
            viewModel.IsTypeGenus = isType;

            if (familyId > 0)
            {
                FamilyViewModel familyViewModel = new FamilyViewModel();
                familyViewModel.Get(familyId);
                viewModel.Entity.FamilyID = familyViewModel.Entity.ID;
                viewModel.Entity.FamilyName = familyViewModel.Entity.FamilyName;
            }

            if (genusId > 0)
            {
                GenusViewModel genusViewModel = new GenusViewModel();
                genusViewModel.SearchEntity.ID = genusId;
                genusViewModel.Search();
                viewModel.Entity.ParentID = genusViewModel.Entity.ID;
                viewModel.Entity.Name = genusViewModel.Entity.Name;
                viewModel.Entity.FamilyID = genusViewModel.Entity.FamilyID;
                viewModel.Entity.FamilyName = genusViewModel.Entity.FamilyName;
                viewModel.Entity.Name = genusViewModel.Entity.Name;
                viewModel.Entity.SubgenusName = genusViewModel.Entity.SubgenusName;
                viewModel.Entity.SectionName = genusViewModel.Entity.SectionName;
                viewModel.Entity.SubsectionName = genusViewModel.Entity.SubsectionName;
                viewModel.Entity.SeriesName = genusViewModel.Entity.SeriesName;
                viewModel.Entity.SubseriesName = genusViewModel.Entity.SubseriesName;
                viewModel.Entity.FamilyID = genusViewModel.Entity.FamilyID;
                viewModel.Entity.FamilyName = genusViewModel.Entity.FamilyName;
                viewModel.Entity.FamilyName = genusViewModel.Entity.FamilyAssembledName;
            }
            return View(BASE_PATH + "Edit.cshtml", viewModel);
        }

        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                GenusViewModel viewModel = new GenusViewModel();
                viewModel.AppUserItemFolderID = appUserItemFolderId;
                viewModel.Get(entityId);
                viewModel.PageTitle = viewModel.GetPageTitle();
                viewModel.TableName = "taxonomy_genus";
                viewModel.TableCode = "Genus";
                viewModel.Entity.IsWebVisibleOption = viewModel.ToBool(viewModel.Entity.IsWebVisible);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(GenusViewModel viewModel)
        {
            try
            {
                // TODO: Refactor.
                viewModel.Entity.IsWebVisible = viewModel.FromBool(viewModel.Entity.IsWebVisibleOption);

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

                if (viewModel.IsTypeGenus == "Y")
                {
                    FamilyMapViewModel familyMapViewModel = new FamilyMapViewModel();
                    familyMapViewModel.Get(viewModel.Entity.FamilyID);
                    familyMapViewModel.Entity.TypeGenusID = viewModel.Entity.ID;
                    familyMapViewModel.Update();
                }

                return RedirectToAction("Edit", "Genus", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                
                  Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public virtual ActionResult Index(string eventAction = "", int folderId = 0)
        {
            GenusViewModel viewModel = new GenusViewModel();

            try
            {
                viewModel.PageTitle = "Genus Search";
                viewModel.TableName = "taxonomy_genus";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as GenusViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<GenusSearch>(appUserItemListViewModel.Entity.Properties);
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

        [HttpPost]
        public ActionResult Search(GenusViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.PageTitle = "Genus Home";
                ViewBag.Title = viewModel.PageTitle;
                viewModel.Search();
                ModelState.Clear();

                // Save search if attribs supplied.
                if ((viewModel.EventAction == "SEARCH") && (viewModel.EventValue == "SAVE"))
                {
                    SysFolderViewModel sysFolderViewModel = new SysFolderViewModel();
                    sysFolderViewModel.Entity.Title = viewModel.EventInfo;
                    sysFolderViewModel.Entity.Description = viewModel.EventNote;
                    sysFolderViewModel.Entity.TypeCode = "DYN";
                    sysFolderViewModel.Entity.Properties = sysFolderViewModel.SerializeToXml<GenusSearch>(viewModel.SearchEntity);
                    sysFolderViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    sysFolderViewModel.Insert();
                }

                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _List(string eventValue = "", int familyId = 0, string genusName = "")
        {
            GenusViewModel viewModel = new GenusViewModel();
            try
            {
                viewModel.SearchEntity.FamilyID = familyId;
                viewModel.SearchEntity.Name = genusName;
                viewModel.Search();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        
        public PartialViewResult _ListSpecies(int genusId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                //viewModel.GetGenera(genusId);
                return PartialView("~/Views/Taxonomy/Genus/_ListSpecies.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSynonyms(int genusId)
        {
            GenusViewModel viewModel = new GenusViewModel();

            try
            {
                viewModel.GetSynonyms(genusId);
                return PartialView(BASE_PATH + "_ListSynonyms.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSubdivisions(int genusId)
        {
            GenusViewModel viewModel = new GenusViewModel();

            try
            {
                viewModel.GetSubdivisions(genusId);
                return PartialView(BASE_PATH + "_ListSubdivisions.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderLookupModal()
        {
            GenusViewModel viewModel = new GenusViewModel();
            
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult Lookup(GenusViewModel viewModel)
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

        public ActionResult Search(SpeciesViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            GenusViewModel viewModel = new GenusViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["FolderID"]))
                {
                    viewModel.SearchEntity.FolderID = Int32.Parse(formCollection["FolderID"].ToString());
                }
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        [HttpPost]
        public PartialViewResult LookupNotes(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Genus/Modals/_NoteSelectList.cshtml";
            GenusViewModel viewModel = new GenusViewModel();

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

        private string GetPartialViewName(string rank)
        {
            //NOTE Refactor somehow. (CBH 12/14/21)
            switch (rank.ToUpper())
            {
                case "GENUS":
                    return "_GenusRankDetail.cshtml";
                case "SUBGENUS":
                    return "_SubgenusRankDetail.cshtml";
                case "SECTION":
                    return "_SectionRankDetail.cshtml";
                case "SUBSECTION":
                    return "_SubsectionRankDetail.cshtml";
                case "SERIES":
                    return "_SeriesRankDetail.cshtml";
                case "SUBSERIES":
                    return "_SubseriesRankDetail.cshtml";
                default:
                    return "";
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
                GenusViewModel viewModel = new GenusViewModel();
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

        #region Batch Edit

        [HttpPost]
        public ActionResult GetBatchEditor(GenusViewModel viewModel)
        {
            try
            {
                Session["GENUS_ID_LIST"] = viewModel.ItemIDList;
                return View("~/Views/Taxonomy/Genus/EditBatch.cshtml", viewModel);
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

            if (Session["GENUS_ID_LIST"] != null)
            {
                idList = Session["GENUS_ID_LIST"].ToString();
            }

            string[] idArray = idList.Split(',');

            try
            {
                using (SpeciesManager mgr = new SpeciesManager())
                {
                    using (var db = new Database("sqlserver", mgr.ConnectionString))
                    {
                        var editor = new Editor(db, "taxonomy_genus", "taxonomy_genus.taxonomy_genus_id").Where(q =>
                        {
                            q.Where(r =>
                            {
                                foreach (var i in idArray)
                                {
                                    r.OrWhere("taxonomy_genus.taxonomy_genus_id", i);
                                }
                            });
                        })
                        .Model<GenusTable>("taxonomy_genus")
                        .Model<FamilyTable>("taxonomy_family")
                        .LeftJoin("taxonomy_family", "taxonomy_genus.taxonomy_family_id", "=", "taxonomy_family.taxonomy_family_id");

                        editor.Field(new Field("taxonomy_genus.taxonomy_genus_id")
                            .Validator(Validation.NotEmpty())
                        );
                        editor.Field(new Field("taxonomy_genus.hybrid_code"));
                        editor.Field(new Field("taxonomy_genus.genus_name"));
                        editor.Field(new Field("taxonomy_genus.genus_authority"));
                        editor.Field(new Field("taxonomy_genus.subgenus_name"));
                        editor.Field(new Field("taxonomy_genus.section_name"));
                        editor.Field(new Field("taxonomy_genus.subsection_name"));
                        editor.Field(new Field("taxonomy_genus.series_name"));
                        editor.Field(new Field("taxonomy_genus.subseries_name"));
                        editor.Field(new Field("taxonomy_genus.note"));
                        editor.Field(new Field("taxonomy_genus.modified_date")
                            .Set(Field.SetType.Edit));
                        editor.PreEdit += (sender, e) => editor.Field("taxonomy_genus.modified_date").SetValue(DateTime.Now);
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
        public JsonResult SaveBatch(int genusId = 0, int familyId = 0)
        {
            try
            {
                GenusViewModel viewModel = new GenusViewModel();
                viewModel.Get(genusId);
                if (viewModel.DataCollection.Count == 0)
                {
                    throw new IndexOutOfRangeException("No genus found for id " + genusId);
                }

                if (familyId > 0)
                {
                    viewModel.Entity.FamilyID = familyId;
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


        #endregion
    }
}
