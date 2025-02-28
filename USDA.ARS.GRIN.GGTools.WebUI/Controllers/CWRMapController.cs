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
    [ValidateInput(false)]
    public class CWRMapController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/CWRMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        [HttpPost]
        public ActionResult Index(CWRMapViewModel vm)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Index(string eventAction = "", int folderId = 0, int cropForCwrId = 0)
        {
            CWRMapViewModel viewModel = new CWRMapViewModel();

            try
            {
                viewModel.PageTitle = "CWR Map Search";
                viewModel.TableName = "taxonomy_cwr_map";
                viewModel.TableCode = "CWRMap";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                if (cropForCwrId > 0)
                {
                    viewModel.SearchEntity.CropForCWRID = cropForCwrId;
                    viewModel.Search();
                }

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as CWRMapViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<CWRMapSearch>(appUserItemListViewModel.Entity.Properties);
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

        public ActionResult Edit(int entityId = 0, int appUserItemFolderId = 0)
        {
            try
            {
                CWRMapViewModel viewModel = new CWRMapViewModel();
                viewModel.TableName = "taxonomy_cwr_map";
                viewModel.TableCode = "CWRMap";
                viewModel.AppUserItemFolderID = appUserItemFolderId;

                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit CWR Map [{0}]: {1}", entityId, viewModel.Entity.AssembledName);
                
                // Load species citations
                
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(CWRMapViewModel viewModel)
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
                return RedirectToAction("Edit", "CWRMap", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Map(int cropForCwrId = 0)
        {
            CWRMapViewModel viewModel = new CWRMapViewModel();
            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.Entity.CropForCWRID = cropForCwrId;
            viewModel.TableName = "taxonomy_cwr_map";

            try 
            {
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Map(CWRMapViewModel viewModel)
        {
            try
            {
                //TODO
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }


        [HttpPost]
        public ActionResult Search(CWRMapViewModel viewModel)
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

        //[HttpPost]
        //public PartialViewResult Add(FormCollection formCollection)
        //{
        //    CWRMapViewModel viewModel = new CWRMapViewModel();
        //    FolderViewModel folderViewModel = new FolderViewModel();
        //    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

        //    if (!String.IsNullOrEmpty(formCollection["IDList"]))
        //    {
        //        viewModel.Entity.ItemIDList = formCollection["IDList"];
        //    }
            
            

        //    if (!String.IsNullOrEmpty(formCollection["CropForCWRID"]))
        //    {
        //        viewModel.Entity.CropForCWRID = Int32.Parse(formCollection["CropForCWRID"]);
        //    }
        //    if (!String.IsNullOrEmpty(formCollection["GenepoolCode"]))
        //    {
        //        viewModel.Entity.GenepoolCode = formCollection["GenepoolCode"];
        //    }
        //    if (!String.IsNullOrEmpty(formCollection["CropCommonName"]))
        //    {
        //        viewModel.Entity.CropCommonName = formCollection["CropCommonName"];
        //    }

        //    // FOLDER
        //    if (!String.IsNullOrEmpty(formCollection["FolderTitle"]))
        //    {
        //        folderViewModel.Entity.FolderName = formCollection["FolderTitle"];
        //    }
        //    if (!String.IsNullOrEmpty(formCollection["FolderCategory"]))
        //    {
        //        folderViewModel.Entity.Category = formCollection["FolderCategory"];
        //    }
        //    if (!String.IsNullOrEmpty(formCollection["FolderDescription"]))
        //    {
        //        folderViewModel.Entity.Description = formCollection["FolderDescription"];
        //    }
        //    viewModel.InsertBatch();

        //    // Retrieve newly-added records.
        //    viewModel.EventAction = "SEARCH";
        //    viewModel.SearchEntity.CropForCWRID = viewModel.Entity.CropForCWRID; ;
        //    viewModel.Search();
        //    ModelState.Clear();
        //    return PartialView("~/Views/CWRMap/_SelectList.cshtml", viewModel);
        //}

        [HttpPost]
        public PartialViewResult AddBatch(FormCollection formCollection)
        {
            CWRMapViewModel viewModel = new CWRMapViewModel();
            List<CWRMap> batchedMaps = new List<CWRMap>();
            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

            if (!String.IsNullOrEmpty(formCollection["Entity.IsCrop"]))
            {
                viewModel.Entity.IsCrop = formCollection["Entity.IsCrop"];
            }

            if (!String.IsNullOrEmpty(formCollection["Entity.IsGraftstock"]))
            {
                viewModel.Entity.IsGraftstock = formCollection["Entity.IsGraftstock"];
            }

            if (!String.IsNullOrEmpty(formCollection["Entity.IsPotential"]))
            {
                viewModel.Entity.IsPotential = formCollection["Entity.IsPotential"];
            }

            if (!String.IsNullOrEmpty(formCollection["SpeciesIDList"]))
            {
                viewModel.SpeciesIDList = formCollection["SpeciesIDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["CropForCWRIDList"]))
            {
                viewModel.CropForCWRIDList = formCollection["CropForCWRIDList"];
            }

            viewModel.InsertMultiple();

            // Add each generated batch to the session-stored list.
            if (Session["CWR-MAPS"] != null)
            {
                batchedMaps = Session["CWR-MAPS"] as List<CWRMap>;
            }
            batchedMaps.AddRange(viewModel.DataCollection);
            Session["CWR-MAPS"] = batchedMaps;
            viewModel.DataCollectionBatch = batchedMaps;

            return PartialView("~/Views/Taxonomy/CWRMap/_ListBatch.cshtml", viewModel);
        }

        public ActionResult Add(int cropForCwrId = 0, int speciesId = 0)
        {
            try
            {
                CWRMapViewModel viewModel = new CWRMapViewModel();
                viewModel.TableName = "taxonomy_cwr_map";
                viewModel.PageTitle = "Add CWR Map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.CropForCWRID = cropForCwrId;

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

//        [HttpPost]
//        public JsonResult AddCitation(FormCollection coll)
//        {
//#pragma warning disable CS0219 // The variable 'citationId' is assigned but its value is never used
//            int citationId = 0;
//#pragma warning restore CS0219 // The variable 'citationId' is assigned but its value is never used
//            string entityIdList = String.Empty;
//            CWRMapViewModel viewModel = new CWRMapViewModel();
//            CitationViewModel citationViewModel = new CitationViewModel();
//            citationViewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
//            citationViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

//            try
//            {
//                if (!String.IsNullOrEmpty(coll["IDList"]))
//                {
//                    entityIdList = coll["IDList"];
//                }

//                if (!String.IsNullOrEmpty(coll["EntityID"]))
//                {
//                    viewModel.Entity.ID = Int32.Parse(coll["EntityID"]);
//                }

//                if (!String.IsNullOrEmpty(coll["CitationAuthorName"]))
//                {
//                    citationViewModel.Entity.CitationAuthorName = coll["CitationAuthorName"];
//                }

//                if (!String.IsNullOrEmpty(coll["CitationYear"]))
//                {
//                    citationViewModel.Entity.CitationYear = Int32.Parse(coll["CitationYear"]);
//                }

//                if (!String.IsNullOrEmpty(coll["VolumePage"]))
//                {
//                    citationViewModel.Entity.VolumeOrPage = coll["VolumePage"];
//                }

//                if (!String.IsNullOrEmpty(coll["DOIReference"]))
//                {
//                    citationViewModel.Entity.DOIReference = coll["DOIReference"];
//                }

//                if (!String.IsNullOrEmpty(coll["ReferenceTitle"]))
//                {
//                    citationViewModel.Entity.ReferenceTitle = coll["ReferenceTitle"];
//                }

//                if (!String.IsNullOrEmpty(coll["Description"]))
//                {
//                    citationViewModel.Entity.Description = coll["Description"];
//                }
//                citationViewModel.Insert();
//                return Json( new { citationId = citationViewModel.Entity.ID }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex);
//                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
//            }
//        }

        //public PartialViewResult FolderItems(int folderId)
        //{
        //    try
        //    {
        //        CWRMapViewModel viewModel = new CWRMapViewModel();
        //        viewModel.EventAction = "SEARCH";
        //        viewModel.EventValue = "FOLDER";
        //        viewModel.SearchEntity.FolderID = sysFolderId;
        //        viewModel.SearchFolderItems();
        //        ModelState.Clear();
        //        return PartialView("~/Views/CWRMap/_List.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        public PartialViewResult _List(int cropForCwrId = 0, int speciesId = 0)
        {
            CWRMapViewModel viewModel = new CWRMapViewModel();
            try
            {
                viewModel.EventAction = "SEARCH";
                viewModel.SearchEntity.CropForCWRID = cropForCwrId;
                viewModel.SearchEntity.SpeciesID = speciesId;
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
            CWRMapViewModel viewModel = new CWRMapViewModel();
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
            CWRMapViewModel viewModel = new CWRMapViewModel();

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
                CWRMapViewModel viewModel = new CWRMapViewModel();
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
            CWRMapViewModel viewModel = new CWRMapViewModel();
            viewModel.SearchEntity.IDList = idList;
            viewModel.Search();

            foreach (var cwrMap in viewModel.DataCollection)
            {
                CitationViewModel citationViewModel = new CitationViewModel();
                citationViewModel.SearchEntity.SpeciesID = cwrMap.SpeciesID;
                citationViewModel.Search();
                cwrMap.Citations = citationViewModel.DataCollection;
            }
            return PartialView("~/Views/Taxonomy/CWRMap/Modals/_EditBatch.cshtml", viewModel);
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

                    CWRMapViewModel viewModel = new CWRMapViewModel();
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

        public ActionResult AddBatch()
        {
            try
            {
                CWRMapViewModel viewModel = new CWRMapViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "EditBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
    }
}
