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
    public class CropForCWRController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/CropForCWR/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
       
        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            CropForCWRViewModel viewModel = new CropForCWRViewModel();
            try
            {
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.TableName = "taxonomy_cwr_crop";
                viewModel.TableCode = "CropForCWR";

                var DEBUG = ViewBag.PageTitle;

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as CropForCWRViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<CropForCWRSearch>(appUserItemListViewModel.Entity.Properties);
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
        
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            CropForCWRViewModel viewModel = new CropForCWRViewModel();
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
        
        [HttpPost]
        public PartialViewResult _Search(FormCollection formCollection)
        {
            CropForCWRViewModel vm = new CropForCWRViewModel();

            if (!String.IsNullOrEmpty(formCollection["Name"]))
            {
                vm.SearchEntity.Name = formCollection["Name"];
                vm.EventAction = "search";
            }
            ModelState.Clear();
            return PartialView("~/Views/CropForCWR/_SelectList.cshtml", vm);
        }

        public PartialViewResult _Search()
        { return PartialView("~/Views/CropForCWR/_Search.cshtml", new CropForCWRViewModel()); }
        
        [HttpPost]
        public ActionResult Search(CropForCWRViewModel viewModel)
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
        [HttpPost]
        public PartialViewResult Lookup(CropForCWRViewModel viewModel)
        {
            string partialViewName = "~/Views/Taxonomy/CropForCWR/Modals/_SelectList.cshtml";

            try
            {
                viewModel.Search();
                return PartialView(partialViewName, viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult Add()
        {
            try
            {
                CropForCWRViewModel viewModel = new CropForCWRViewModel();
                viewModel.TableName = "taxonomy_cwr_crop";
                viewModel.PageTitle = "Add Crop For CWR";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId = 0, int appUserItemFolderId = 0)
        {
            try
            {
                CropForCWRViewModel viewModel = new CropForCWRViewModel();
                viewModel.TableName = "taxonomy_cwr_crop";
                viewModel.TableCode = "CropForCWR";
                viewModel.PageTitle = String.Format("Edit Crop For CWR [{0}]", entityId);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Get(entityId);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(CropForCWRViewModel viewModel)
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
                return RedirectToAction("Edit", "CropForCWR", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Delete(FormCollection formCollection)
        {
            try
            {
                CropForCWRViewModel viewModel = new CropForCWRViewModel();
                viewModel.Entity.ID = Int32.Parse(GetFormFieldValue(formCollection, "EntityID"));
                viewModel.TableName = GetFormFieldValue(formCollection, "TableName");
                viewModel.Delete();
                return View();
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
                CropForCWRViewModel viewModel = new CropForCWRViewModel();
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

        public PartialViewResult RenderLookupModal()
        {
            CropForCWRViewModel viewModel = new CropForCWRViewModel();
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }
    }
}
