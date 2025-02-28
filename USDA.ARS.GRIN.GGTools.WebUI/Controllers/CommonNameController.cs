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
    public class CommonNameController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/CommonName/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
      
        public ActionResult Add(int genusId = 0, int speciesId = 0)
        {
            try
            {
                CommonNameViewModel viewModel = new CommonNameViewModel();
                viewModel.TableName = "taxonomy_common_name";
                viewModel.PageTitle = "Add Common Name";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

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
                    viewModel.Entity.GenusName = genusViewModel.Entity.Name;
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
                CommonNameViewModel viewModel = new CommonNameViewModel();
                viewModel.TableName = "taxonomy_common_name";
                viewModel.TableCode = "CommonName";
                
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Common Name [{0}]: {1}", entityId, viewModel.Entity.AssembledName);
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add Common Name";
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
        public PartialViewResult RenderBatchEditModal(string idList)
        {
            CommonNameViewModel viewModel = new CommonNameViewModel();
            viewModel.SearchEntity.IDList = idList;
            viewModel.Search();

            foreach (var commonName in viewModel.DataCollection)
            {
                CitationViewModel citationViewModel = new CitationViewModel();
                citationViewModel.SearchEntity.SpeciesID = commonName.SpeciesID;
                citationViewModel.Search();
                commonName.Citations = citationViewModel.DataCollection;
                // TODO Edit species citations
            }

            // TODO
            // Edit list of common names based on parameter
            // Retrieve each common name and its parent species
            //    For each species, retrieve each of its citations



            return PartialView("~/Views/Taxonomy/CommonName/Modals/_EditBatch.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult BatchEdit(string keyList)
        {
            try 
            { 
                foreach (var key in keyList.Split(','))
                {
                    var keyTokens = key.Split('_');
                    var commonNameId = keyTokens[0];
                    var citationId = keyTokens[1];

                    CommonNameViewModel viewModel = new CommonNameViewModel();
                    viewModel.Get(Int32.Parse(commonNameId));
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
        public ActionResult Edit(CommonNameViewModel viewModel)
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
                return RedirectToAction("Edit", "CommonName", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _Save(CommonNameViewModel viewModel)
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
                viewModel.Entity.SpeciesID = viewModel.Entity.ID;
                return PartialView("~/Views/Taxonomy/CommonName/_Edit.cshtml", viewModel);
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
                CommonNameViewModel viewModel = new CommonNameViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as CommonNameViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<CommonNameSearch>(appUserItemListViewModel.Entity.Properties);
                    viewModel.Search();
                }

                viewModel.PageTitle = "Common Name Search";
                viewModel.TableCode = "taxonomy_common_name";
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

        public ActionResult Search(CommonNameViewModel viewModel)
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

        public PartialViewResult _List(int genusId = 0, int speciesId = 0)
        {
            CommonNameViewModel viewModel = new CommonNameViewModel();
            CommonNameSearch search = new CommonNameSearch();
            viewModel.SearchEntity.GenusID = genusId;
            viewModel.SearchEntity.SpeciesID = speciesId;
            viewModel.Search();
            return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        }
        
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            CommonNameViewModel viewModel = new CommonNameViewModel();
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
            try
            {
                CommonNameViewModel viewModel = new CommonNameViewModel();
                viewModel.RunSearch(folderId);
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult RenderEditModal(int genusId = 0, int speciesId = 0)
        {
            try
            {
                CommonNameViewModel viewModel = new CommonNameViewModel();
                viewModel.TableName = "taxonomy_common_name";
                viewModel.PageTitle = "Add Common Name";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

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
                    viewModel.Entity.GenusName = genusViewModel.Entity.Name;
                }

                return PartialView(BASE_PATH + "_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public JsonResult Add(FormCollection coll)
        {
            return null;
        }
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                CommonNameViewModel viewModel = new CommonNameViewModel();
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
