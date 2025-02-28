using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysFolderController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private AuthorViewModel authorViewModel = null;
        private ClassificationViewModel classificationViewModel = null;
        private CropForCWRViewModel cropForCWRViewModel = null;
        private CWRMapViewModel cWRMapViewModel = null;
        private CWRTraitViewModel cWRTraitViewModel = null;
        private CommonNameViewModel commonNameViewModel = null;
        private CommonNameLanguageViewModel commonNameLanguageViewModel = null;
        private EconomicUsageType economicUsageTypeViewModel = null;
        private EconomicUseViewModel economicUseViewModel = null;
        private FamilyViewModel familyViewModel = null;
        private GenusViewModel genusViewModel = null;

        public ActionResult Index()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return View(viewModel);
                SetPageTitle();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Explorer()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                ViewBag.PageTitle = "My Folders";
                return View("~/Views/SysFolder/Explorer/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(SysFolderViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.EventAction = "SEARCH";
                viewModel.Search();
                ModelState.Clear();
                viewModel.TableName = "taxonomy_author";
                return View("~/Views/SysFolder/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult Edit(SysFolderViewModel viewModel)
        {
            try
            {
                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Insert();


                    //TODO
                    //if type is "DYN", look for session object with folder table name

                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }

                // If there are tag attributes, add specified tag.
                //if (viewModel.IsFavoriteSelector == true)
                //{
                //    SysTagViewModel sysTagViewModel = new SysTagViewModel();
                //    sysTagViewModel.Entity.TagText = "Favorites";
                //    sysTagViewModel.Entity.TagFormatString = "";
                //    sysTagViewModel.Entity.TableName = "sys_folder";
                //    sysTagViewModel.Entity.IDNumber = viewModel.Entity.ID;
                //    sysTagViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //    sysTagViewModel.Insert();
                //}

                // Re-retrieve new folder to verify existence.
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Get(viewModel.Entity.ID);
                return PartialView("~/Views/SysFolder/Components/_Confirmation.cshtml", viewModel);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult EditDetails(SysFolderViewModel viewModel)
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

                // Re-retrieve new folder to verify existence.
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Get(viewModel.Entity.ID);
                return PartialView("~/Views/SysFolder/_Edit.cshtml", viewModel);
                

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult EditProperties(SysFolderViewModel viewModel)
        {
            SysFolderViewModel updateViewModel = new SysFolderViewModel();
            updateViewModel.Get(viewModel.Entity.ID);
            updateViewModel.Entity.Properties = viewModel.Entity.Properties;
            updateViewModel.UpdateProperties();

            viewModel.GetProperties(viewModel.Entity.ID);
            return PartialView("~/Views/SysFolder/Components/_SQLQueryFolderEditor.cshtml", viewModel);
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.TableCode = "SysFolder";
                viewModel.TableName = "sys_folder";
                viewModel.Get(entityId);
                viewModel.GetItems(entityId);
                //viewModel.GetCooperators(entityId);
                viewModel.GetSysTags("sys_folder", entityId);
                viewModel.GetSysTables(entityId);
                SetPageTitle();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
               SysFolderViewModel viewModel = new SysFolderViewModel();
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
        public JsonResult DeleteItems(FormCollection coll)
        {
            SysFolderViewModel viewModel = new SysFolderViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.SearchEntity.ID = Int32.Parse(coll["FolderID"]);
                }

                if (!String.IsNullOrEmpty(coll["ItemIDList"]))
                {
                    viewModel.ItemIDList = coll["ItemIDList"].ToString();
                }

                //viewModel.Edit();
                viewModel.DeleteItems();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult GetEditModal()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return PartialView("~/Views/SysFolder/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public PartialViewResult GetDynamicEditModal(string sysTableName, string typeCode)
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.Entity.TypeCode = typeCode;
                return PartialView("~/Views/SysFolder/Modals/_EditDynamic.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public PartialViewResult GetSQLSearchModal()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return PartialView("~/Views/SysFolder/Modals/_EditSQL.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        #region Components

        /// <summary>
        /// Retrieves an icon-formatted list of folders. Defaults to folders owned by the logged-in user.
        /// </summary>
        /// <returns></returns>

        public PartialViewResult Component_ListWithIcons(string typeCode = "")
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.SearchEntity.TypeCode = typeCode;
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Search();
                return PartialView("~/Views/SysFolder/Components/_ListWithIcons.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Component_Widget()
        {
            SysFolderViewModel viewModel = new SysFolderViewModel();
            string sysFolderId = Request.QueryString["sysFolderId"];
            try
            {
                if (!String.IsNullOrEmpty(sysFolderId))
                {
                    viewModel.Get(Int32.Parse(sysFolderId));
                    return PartialView("~/Views/SysFolder/Components/_Widget.cshtml", viewModel);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        /// <summary>
        /// Loads and runs a previously-saved search.
        /// </summary>
        /// <param name="sysFolderId">The ID of the folder containing the encoded search criteria.</param>
        /// <returns>A partial view containing the search results.</returns>
        public PartialViewResult _ListDynamicFolderItems(int sysFolderId)
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.GetProperties(sysFolderId);
                switch (viewModel.SysFolderPropertiesEntity.TableName)
                {
                    case "taxonomy_author":
                        AuthorViewModel authorViewModel = new AuthorViewModel();
                        authorViewModel.SearchEntity = authorViewModel.Deserialize<AuthorSearch>(viewModel.SysFolderPropertiesEntity.Properties);
                        authorViewModel.Search();
                        return PartialView("~/Views/Taxonomy/Author/_List.cshtml", authorViewModel);
                    case "citation":
                        CitationViewModel citationViewModel = new CitationViewModel();
                        citationViewModel.SearchEntity = citationViewModel.Deserialize<CitationSearch>(viewModel.Entity.Properties);
                        citationViewModel.Search();
                        return PartialView("~/Views/Taxonomy/Citation/_List.cshtml", citationViewModel);
                    case "taxonomy_classification":
                        ClassificationViewModel classificationViewModel = new ClassificationViewModel();
                        classificationViewModel.SearchEntity = classificationViewModel.Deserialize<ClassificationSearch>(viewModel.Entity.Properties);
                        classificationViewModel.Search();
                        return PartialView("~/Views/Taxonomy/Order/_List.cshtml", classificationViewModel);
                    case "taxonomy_common_name":
                        CommonNameViewModel commonNameViewModel = new CommonNameViewModel();
                        commonNameViewModel.SearchEntity = commonNameViewModel.Deserialize<CommonNameSearch>(viewModel.Entity.Properties);
                        commonNameViewModel.Search();
                        return PartialView("~/Views/Taxonomy/CommonName/_List.cshtml", commonNameViewModel);
                    case "taxonomy_common_name_language":
                        CommonNameLanguageViewModel commonNameLanguageViewModel = new CommonNameLanguageViewModel();
                        commonNameLanguageViewModel.SearchEntity = commonNameLanguageViewModel.Deserialize<CommonNameLanguageSearch>(viewModel.Entity.Properties);
                        commonNameLanguageViewModel.Search();
                        return PartialView("~/Views/Taxonomy/CommonNameLanguage/_List.cshtml", commonNameLanguageViewModel);
                    case "taxonomy_cwr_crop":
                        CropForCWRViewModel cropForCWRViewModel = new CropForCWRViewModel();
                        cropForCWRViewModel.SearchEntity = cropForCWRViewModel.Deserialize<CropForCWRSearch>(viewModel.Entity.Properties);
                        cropForCWRViewModel.Search();
                        return PartialView("~/Views/Taxonomy/CropForCWRViewModel/_List.cshtml", cropForCWRViewModel);
                    case "taxonomy_cwr_map":
                        CWRMapViewModel cWRMapViewModel = new CWRMapViewModel();
                        cWRMapViewModel.SearchEntity = cWRMapViewModel.Deserialize<CWRMapSearch>(viewModel.Entity.Properties);
                        cWRMapViewModel.Search();
                        return PartialView("~/Views/Taxonomy/CWRMapViewModel/_List.cshtml", cWRMapViewModel);
                    case "taxonomy_cwr_trait":
                        CWRTraitViewModel cWRTraitViewModel = new CWRTraitViewModel();
                        cWRTraitViewModel.SearchEntity = cWRTraitViewModel.Deserialize<CWRTraitSearch>(viewModel.Entity.Properties);
                        cWRTraitViewModel.Search();
                        return PartialView("~/Views/Taxonomy/CWRTraitViewModel/_List.cshtml", cWRTraitViewModel);
                    case "taxonomy_economic_usage_type":
                        EconomicUsageTypeViewModel economicUsageTypeViewModel = new EconomicUsageTypeViewModel();
                        economicUsageTypeViewModel.SearchEntity = economicUsageTypeViewModel.Deserialize<EconomicUsageTypeSearch>(viewModel.Entity.Properties);
                        economicUsageTypeViewModel.Search();
                        return PartialView("~/Views/Taxonomy/EconomicUsageTypeViewModel/_List.cshtml", economicUsageTypeViewModel);
                    case "taxonomy_use":
                        EconomicUseViewModel economicUseViewModel = new EconomicUseViewModel();
                        economicUseViewModel.SearchEntity = economicUseViewModel.Deserialize<EconomicUseSearch>(viewModel.Entity.Properties);
                        economicUseViewModel.Search();
                        return PartialView("~/Views/Taxonomy/EconomicUseViewModel/_List.cshtml", economicUseViewModel);
                    case "taxonomy_family":
                        FamilyViewModel familyViewModel = new FamilyViewModel();
                        familyViewModel.SearchEntity = familyViewModel.Deserialize<FamilySearch>(viewModel.Entity.Properties);
                        familyViewModel.Search();
                        return PartialView("~/Views/Taxonomy/FamilyViewModel/_List.cshtml", familyViewModel);
                    case "taxonomy_genus":
                        GenusViewModel genusViewModel = new GenusViewModel();
                        genusViewModel.SearchEntity = genusViewModel.Deserialize<GenusSearch>(viewModel.Entity.Properties);
                        genusViewModel.Search();
                        return PartialView("~/Views/Taxonomy/GenusViewModel/_List.cshtml", genusViewModel);
                    case "geography":
                        GeographyViewModel geographyViewModel = new GeographyViewModel();
                        geographyViewModel.SearchEntity = geographyViewModel.Deserialize<GeographySearch>(viewModel.Entity.Properties);
                        geographyViewModel.Search();
                        return PartialView("~/Views/Taxonomy/GeographyViewModel/_List.cshtml", geographyViewModel);
                    case "taxonomy_geography_map":
                        GeographyMapViewModel geographyMapViewModel = new GeographyMapViewModel();
                        geographyMapViewModel.SearchEntity = geographyMapViewModel.Deserialize<GeographyMapSearch>(viewModel.Entity.Properties);
                        geographyMapViewModel.Search();
                        return PartialView("~/Views/Taxonomy/GeographyMapViewModel/_List.cshtml", geographyMapViewModel);
                    case "literature":
                        LiteratureViewModel literatureViewModel = new LiteratureViewModel();
                        literatureViewModel.SearchEntity = literatureViewModel.Deserialize<LiteratureSearch>(viewModel.Entity.Properties);
                        literatureViewModel.Search();
                        return PartialView("~/Views/Taxonomy/Literature/_List.cshtml", literatureViewModel);
                    case "taxonomy_regulation":
                        RegulationViewModel regulationViewModel = new RegulationViewModel();
                        regulationViewModel.SearchEntity = regulationViewModel.Deserialize<RegulationSearch>(viewModel.Entity.Properties);
                        regulationViewModel.Search();
                        return PartialView("~/Views/Taxonomy/RegulationViewModel/_List.cshtml", regulationViewModel);
                    case "taxonomy_regulation_map":
                        RegulationMapViewModel regulationMapViewModel = new RegulationMapViewModel();
                        regulationMapViewModel.SearchEntity = regulationMapViewModel.Deserialize<RegulationMapSearch>(viewModel.Entity.Properties);
                        regulationMapViewModel.Search();
                        return PartialView("~/Views/Taxonomy/RegulationMap/_List.cshtml", regulationMapViewModel);
                    case "taxonomy_species":
                        SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                        speciesViewModel.SearchEntity = speciesViewModel.Deserialize<SpeciesSearch>(viewModel.Entity.Properties);
                        speciesViewModel.Search();
                        return PartialView("~/Views/Taxonomy/Species/_List.cshtml", speciesViewModel);
                    case "taxonomy_species_synonym_map":
                        SynonymMapViewModel synonymMapViewModel = new SynonymMapViewModel();
                        synonymMapViewModel.SearchEntity = synonymMapViewModel.Deserialize<SpeciesSynonymMapSearch>(viewModel.Entity.Properties);
                        synonymMapViewModel.Search();
                        return PartialView("~/Views/Taxonomy/SpeciesSynonymMap/_List.cshtml", synonymMapViewModel);
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }

        }

        public PartialViewResult _ListSQLQueryFolderItems(int sysFolderId)
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.GetProperties(sysFolderId);

                SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();
                sysDynamicQueryViewModel.SearchEntity.SQLStatement = viewModel.SysFolderPropertiesEntity.Properties;
                sysDynamicQueryViewModel.Search();
                return PartialView("~/Views/SysDynamicQuery/_SearchResultsList.cshtml", sysDynamicQueryViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public PartialViewResult Component_SysFolderCooperatorMapEditor()
        //{
        //    try
        //    {
        //        SysFolderCooperatorMapViewModel viewModel = new SysFolderCooperatorMapViewModel();
        //        viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
        //        viewModel.Search();
        //        return PartialView("~/Views/SysFolder/Components/_ListWithIcons.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        #endregion

        #region Components

        public PartialViewResult Component_SQLQueryFolderEditor(int sysFolderId)
        {
            SysFolderViewModel viewModel = new SysFolderViewModel();
            viewModel.Get(sysFolderId);
            viewModel.GetProperties(sysFolderId);

            //SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();
            //sysDynamicQueryViewModel.SearchEntity.SQLStatement = viewModel.SysFolderPropertiesEntity.Properties;
            //sysDynamicQueryViewModel.Search();
            return PartialView("~/Views/SysFolder/Components/_SQLQueryFolderEditor.cshtml", viewModel);
        }

        #endregion
    }
}