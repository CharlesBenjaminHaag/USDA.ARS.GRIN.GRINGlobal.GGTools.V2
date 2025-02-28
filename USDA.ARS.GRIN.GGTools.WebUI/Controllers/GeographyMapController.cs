using NLog;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.WebUI;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class GeographyMapController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/GeographyMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();
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
            GeographyMapViewModel viewModel = new GeographyMapViewModel();

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
        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();
            viewModel = LoadFromSession(viewModel);
            
            try
            {
                ViewBag.PageTitle = "Distribution Search";
                viewModel.TableName = "taxonomy_geography_map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as GeographyMapViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<GeographyMapSearch>(appUserItemListViewModel.Entity.Properties);
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
        public ActionResult Search(GeographyMapViewModel viewModel)
        {
            try 
            {
                Session[SessionKeyName] = viewModel;
                viewModel.SearchEntity.IsValid = viewModel.SearchEntity.IsValidOption == true ? "Y" : null;
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
                
        public PartialViewResult _List(string eventValue = "", int geographyId = 0, int speciesId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.SearchEntity.GeographyID = geographyId;
                viewModel.SearchEntity.SpeciesID = speciesId;
                viewModel.Search();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult _SelectList(int speciesId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.SearchEntity = new GeographyMapSearch { SpeciesID = speciesId };
                viewModel.Search();
                return PartialView(BASE_PATH + "_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult EditBatch()
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();

            try
            {
                viewModel.EventAction = "GeographyMap";
                viewModel.EventValue = "EditBatch";
                viewModel.Entity.GeographyID = 926;
                viewModel.Entity.GeographyDescription = "United States";
                viewModel.Entity.GeographyStatusCode = "n";
                return View("~/Views/Taxonomy/GeographyMap/EditBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
       
        public ActionResult Add(int speciesId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.TableName = "taxonomy_geography_map";
                ViewBag.PageTitle = "Add Distribution";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
               
                if (speciesId > 0)
                {
                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                    speciesViewModel.SearchEntity = new SpeciesSearch { ID = speciesId };
                    speciesViewModel.Search();
                    viewModel.Entity.SpeciesID = speciesViewModel.Entity.ID;
                    viewModel.Entity.SpeciesName = speciesViewModel.Entity.Name;
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
        public PartialViewResult AddBatch(GeographyMapViewModel viewModel)
        {
            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.InsertBatch();
                return PartialView("~/Views/Taxonomy/GeographyMap/_ListBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult Add(FormCollection formCollection)
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();
            List<GeographyMap> batchedMaps = new List<GeographyMap>();
            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

            if (!String.IsNullOrEmpty(formCollection["StatusCode"]))
            {
                viewModel.Entity.GeographyStatusCode = formCollection["StatusCode"];
            }

            if (!String.IsNullOrEmpty(formCollection["SpeciesIDList"]))
            {
                viewModel.SpeciesIDList = formCollection["SpeciesIDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["GeographyIDList"]))
            {
                viewModel.GeographyIDList = formCollection["GeographyIDList"];
            }

            viewModel.InsertMultiple();

            // Add each generated batch to the session-stored list.
            if (Session["GEOGRAPHY-MAPS"] != null)
            {
                batchedMaps = Session["GEOGRAPHY-MAPS"] as List<GeographyMap>;
            }
            batchedMaps.AddRange(viewModel.DataCollection);
            Session["GEOGRAPHY-MAPS"] = batchedMaps;
            viewModel.DataCollectionBatch = batchedMaps;

            return PartialView("~/Views/Taxonomy/GeographyMap/_ListBatch.cshtml", viewModel);
        }

        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.TableName = "taxonomy_geography_map";
                viewModel.AppUserItemFolderID = appUserItemFolderId;

                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Distribution [{0}]: {1}, {2}", entityId, viewModel.Entity.SpeciesName, viewModel.Entity.GeographyDescription);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult EditBatch(GeographyMapViewModel viewModel)
        {
            try
            {
                //viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //viewModel.InsertBatch();
                return Json(viewModel.Entity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                viewModel.LastExceptionMessage = ex.Message;
                return Json(viewModel.Entity, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(GeographyMapViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                }

                // If there is a list of geo IDs, perform an INSERT for each.
                if (!String.IsNullOrEmpty(viewModel.GeographyIDList))
                {
                    viewModel.InsertBatch();
                }
                else
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
                }
                return RedirectToAction("Edit", "GeographyMap", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public JsonResult _Save(GeographyMapViewModel viewModel)
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

                viewModel.Get(viewModel.Entity.ID);

                return Json(new { geographyMap = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { geographyMap = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Map(int entityId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.PageTitle = "Map Geographies";
                viewModel.EventValue = "species";
                viewModel.Entity.SpeciesID = entityId;
                viewModel.Entity.TableName = "taxonomy_species";
                return View(viewModel);
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
 
        public ActionResult RenderLookupModal()
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();
            return PartialView("~/Views/GeographyMap/Modals/_Lookup.cshtml", viewModel);
        }

        public PartialViewResult _ListGeography(string idList)
        {
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.SearchEntity = new GeographySearch { IDList = idList };
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Geography/Modals/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
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
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
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

        public GeographyMapViewModel LoadFromSession(GeographyMapViewModel viewModel)
        {
            string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
            if (Session[targetKey] != null)
            {
                viewModel = Session[targetKey] as GeographyMapViewModel;
            }
            return viewModel;
        }

        [HttpPost]
        public PartialViewResult RenderBatchEditModal(string idList)
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();
            viewModel.SearchEntity.IDList = idList;
            viewModel.Search();

            foreach (var cwrMap in viewModel.DataCollection)
            {
                CitationViewModel citationViewModel = new CitationViewModel();
                citationViewModel.SearchEntity.SpeciesID = cwrMap.SpeciesID;
                citationViewModel.Search();
                cwrMap.Citations = citationViewModel.DataCollection;
            }
            return PartialView("~/Views/Taxonomy/GeographyMap/Modals/_EditBatch.cshtml", viewModel);
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

                    GeographyMapViewModel viewModel = new GeographyMapViewModel();
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

        #region Components

        public PartialViewResult Component_Edit()
        {
            try {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                return PartialView("~/Views/Taxonomy/GeographyMap/Components/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #endregion
    }
}
