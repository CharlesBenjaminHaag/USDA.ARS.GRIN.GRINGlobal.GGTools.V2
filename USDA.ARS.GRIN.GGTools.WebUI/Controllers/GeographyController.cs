using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.WebUI;
using NLog;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class GeographyController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Geography/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            GeographyViewModel viewModel = new GeographyViewModel();
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
            GeographyViewModel viewModel = new GeographyViewModel();

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
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.PageTitle = "Geography Search";
                viewModel.TableName = "geography";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<GeographySearch>(appUserItemListViewModel.Entity.Properties);
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

        //public ActionResult Search()
        //{
        //    try
        //    {
        //        GeographyViewModel viewModel = new GeographyViewModel();
        //        viewModel.PageTitle = "Geography Search";
        //        viewModel.Entity.TableName = "geography";
        //        viewModel.DataCollection = new System.Collections.ObjectModel.Collection<Geography>(viewModel.GetAdministrativeUnits());
        //        viewModel.Search();

                
        //        return Edit(BASE_PATH + "Index.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return RedirectToAction("InternalServerError", "Error");
        //    }
        //}

        [HttpPost]
        public ActionResult Search(GeographyViewModel viewModel)
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

        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.TableName = "geography";
                viewModel.TableCode = "Geography";
                viewModel.AppUserItemFolderID = appUserItemFolderId;

                if (entityId > 0)
                { 
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Geography [{0}]: {1}", entityId, viewModel.Entity.GeographyText);
                    //viewModel.DataCollectionCountries = new System.Collections.ObjectModel.Collection<Country>(viewModel.GetCountries(""));
                    //viewModel.Countries = new SelectList(viewModel.GetCountries(""),"CountryCode","CountryName");
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add Geography";
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
        public ActionResult Edit(GeographyViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                }

                viewModel.Entity.IsValid = viewModel.FromBool(viewModel.Entity.IsValidOption);

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
                return RedirectToAction("Edit", "Geography", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        public ActionResult Add()
        {
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.TableName = "geography";
                viewModel.PageTitle = "Add Geography";
                //viewModel.DataCollectionCountries = new System.Collections.ObjectModel.Collection<Country>(viewModel.GetCountries(""));
                //viewModel.Countries = new SelectList(viewModel.GetCountries(""),"CountryCode", "CountryName");
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public JsonResult Map(FormCollection formCollection)
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["ID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(formCollection["ID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.ItemIDList = formCollection["IDList"];
                }
                viewModel.Map();

                //TODO
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
        public PartialViewResult _ListContinents()
        {
            try {
                //TODO
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.GetContinents();
                return PartialView("~/Views/Taxonomy/Geography/Modals/_SelectListContinent.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSubContinents(string continents = "")
        {
            try
            {
                //TODO
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.SearchEntity.ContinentNameList = continents;
                viewModel.GetSubContinents();
                return PartialView("~/Views/Taxonomy/Geography/Modals/_SelectListSubContinent.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListCountries(string continents = "NULL", string subContinents = "")
        {
            GeographyViewModel viewModel = new GeographyViewModel();
            try
            {
                viewModel.SearchEntity.ContinentNameList = continents;
                viewModel.SearchEntity.SubContinentNameList = subContinents;
                viewModel.GetCountries();
                return PartialView("~/Views/Taxonomy/Geography/Modals/_SelectListCountry.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListAdministrativeUnits(string subContinents = "", string countries = "")
        {
            GeographyViewModel viewModel = new GeographyViewModel();
            viewModel.SearchEntity.SubContinentIDList = subContinents;
            viewModel.SearchEntity.CountryCodeList = countries;
            viewModel.GetAdministrativeUnits();
            return PartialView("~/Views/Taxonomy/Geography/Modals/_SelectListAdministrativeUnits.cshtml", viewModel);
        }
        
        public JsonResult GetCountryAdmins(string countryCode)
        {
            try 
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.SearchEntity.CountryCode = countryCode;
                viewModel.Search();
                return Json(viewModel.DataCollection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json("ERROR: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public PartialViewResult SearchGeographies(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Geography/_List.cshtml";
            GeographyViewModel viewModel = new GeographyViewModel();
            
            if (!String.IsNullOrEmpty(formCollection["ContinentRegionIDList"]))
            {
                viewModel.SearchEntity.ContinentIDList = formCollection["ContinentRegionIDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["SubContinentRegionIDList"]))
            {
                viewModel.SearchEntity.SubContinentIDList = formCollection["SubContinentRegionIDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["CountryCodeList"]))
            {
                viewModel.SearchEntity.CountryCodeList = formCollection["CountryCodeList"];
            }

            viewModel.Search();
            return PartialView(partialViewName, viewModel);
        }

        public ActionResult RenderLookupModal(string isLookupFormat = "Y")
        {
            GeographyViewModel viewModel = new GeographyViewModel();
            viewModel.IsLookupFormat = isLookupFormat;
            viewModel.GetContinents();
            // TODO If we have a species ID,
            // 1) Load its name/basic identifying data
            // 2) Load its existing geo maps
            //if (speciesId > 0)
            //{
            //    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
            //    speciesViewModel.Edit(speciesId);

            //    viewModel.SpeciesID = speciesViewModel.Entity.ID;
            //    viewModel.SpeciesName = speciesViewModel.Entity.AssembledName;

            //    GeographyMapViewModel geographyMapViewModel = new GeographyMapViewModel();
            //    geographyMapViewModel.SearchEntity.SpeciesID = speciesId;
            //    geographyMapViewModel.Search();
            //}

            return PartialView(BASE_PATH + "Modals/_Lookup.cshtml", viewModel);
        }

        public PartialViewResult RenderLookupSimpleModal()
        {
            GeographyViewModel viewModel = new GeographyViewModel();
            viewModel.GetGeographyCountryAdmins();
            return PartialView(BASE_PATH + "Modals/_LookupSimple.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection coll)
        {
            GeographyViewModel viewModel = new GeographyViewModel();

            if (!String.IsNullOrEmpty(coll["ContinentRegionID"]))
            {
                viewModel.SearchEntity.ContinentRegionID = Int32.Parse(coll["ContinentRegionID"]);
            }

            if (!String.IsNullOrEmpty(coll["SubContinentRegionID"]))
            {
                viewModel.SearchEntity.SubContinentRegionID = Int32.Parse(coll["SubContinentRegionID"]);
            }

            if (!String.IsNullOrEmpty(coll["CountryCode"]))
            {
                viewModel.SearchEntity.CountryCode = coll["CountryCode"];
            }

            if (!String.IsNullOrEmpty(coll["IsValid"]))
            {
                viewModel.SearchEntity.IsValid = coll["IsValid"];
            }
            viewModel.Search();
            return PartialView(BASE_PATH + "Modals/_SelectList.cshtml", viewModel);
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
                GeographyViewModel viewModel = new GeographyViewModel();
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

        #region Explorer

        public ActionResult Explorer()
        {
            GeographyViewModel viewModel = new GeographyViewModel();
            return View("~/Views/Taxonomy/Geography/Explorer/Index.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult ExplorerList(GeographyViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Geography/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #endregion Explorer
    }
}
