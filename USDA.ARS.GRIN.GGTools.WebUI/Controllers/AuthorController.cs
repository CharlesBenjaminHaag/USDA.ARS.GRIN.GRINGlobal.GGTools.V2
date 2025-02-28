using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer.UPOV;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    public class ApiReponseContainer
    { 
        
    }

    [GrinGlobalAuthentication]
    public class AuthorController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Author/";
        
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            AuthorViewModel viewModel = new AuthorViewModel();
            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.TableName = "taxonomy_author";
                viewModel.GetFolderItems(sysFolderId);
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
            AuthorViewModel viewModel = new AuthorViewModel();
            viewModel.TableName = "taxonomy_author";

            SysFolderViewModel sysFolderViewModel = new SysFolderViewModel();
           
            try 
            {
                sysFolderViewModel.GetProperties(folderId);
                if (sysFolderViewModel.SysFolderPropertiesEntity != null)
                {
                    viewModel.SearchEntity = viewModel.Deserialize<AuthorSearch>(sysFolderViewModel.SysFolderPropertiesEntity.Properties);
                }

                viewModel.RunSearch(folderId);
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
       
        public PartialViewResult _ListReferences(string shortName)
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.SearchEntity.ShortName = shortName;
                viewModel.GetReferences();
                return PartialView("~/Views/Taxonomy/Author/_ListReferences.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public ActionResult Index(string eventAction = "", int folderId = 0, string sysTableName = "", string sysTableTitle = "")
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.PageTitle = "Author Search";
                viewModel.TableName = "taxonomy_author";

                //SetPageTitle();

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as AuthorViewModel;
                    viewModel.Search();
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<AuthorSearch>(appUserItemListViewModel.Entity.Properties);
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
        
        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.TableName = "taxonomy_author";
                viewModel.TableCode = "Author";
                viewModel.EventValue = "Edit";
                viewModel.AppUserItemFolderID = appUserItemFolderId;

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.OriginalShortName = viewModel.Entity.ShortName;                                        viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Author [{0}]: {1}", entityId, viewModel.Entity.FullName);
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add Author";
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
        public ActionResult Edit(AuthorViewModel viewModel)
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

                if ((viewModel.EventAction == "SAVE") && (viewModel.EventValue == "REF"))
                {   
                    viewModel.UpdateReferences(viewModel.OriginalShortName, viewModel.Entity.ShortName);
                }

                return RedirectToAction("Edit", "Author", new { entityId = viewModel.Entity.ID });
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
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        [HttpPost]
        public ActionResult Search(AuthorViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.EventAction = "SEARCH";
                viewModel.SearchEntity.IsShortNameExactMatch = viewModel.FromBool(viewModel.SearchEntity.IsShortNameExactMatchOption);
                viewModel.Search();
                ModelState.Clear();

                // Save search if attribs supplied.
                if ((viewModel.EventAction == "SEARCH") && (viewModel.EventValue == "SAVE"))
                {
                    SysFolderViewModel sysFolderViewModel = new SysFolderViewModel();
                    sysFolderViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    sysFolderViewModel.Entity.Title = viewModel.EventInfo;
                    sysFolderViewModel.Entity.Description = viewModel.EventNote;
                    sysFolderViewModel.Entity.TableName = viewModel.TableName;
                    sysFolderViewModel.Entity.Properties = viewModel.SerializeToXml<AuthorSearch>(viewModel.SearchEntity);
                    sysFolderViewModel.Entity.TypeCode = "DYN";
                    sysFolderViewModel.Insert();
                }
                viewModel.TableName = "taxonomy_author";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public PartialViewResult _Search(AuthorViewModel viewModel)
        {
            try
            {
                viewModel.EventAction = "SEARCH";
                viewModel.SearchEntity.IsShortNameExactMatch = viewModel.FromBool(viewModel.SearchEntity.IsShortNameExactMatchOption);
                viewModel.Search();
                return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", viewModel);
            }
        }
        
        [HttpPost]
        public PartialViewResult Lookup(FormCollection formCollection)
        {
            AuthorViewModel viewModel = new AuthorViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.SearchEntity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["AuthorLookupFullName"]))
            {
                viewModel.SearchEntity.FullName = formCollection["AuthorLookupFullName"];
            }

            if (!String.IsNullOrEmpty(formCollection["AuthorLookupShortName"]))
            {
                viewModel.SearchEntity.ShortName = formCollection["AuthorLookupShortName"];
            }

            if (!String.IsNullOrEmpty(formCollection["AuthorLookupIsExactMatch"]))
            {
                viewModel.SearchEntity.IsShortNameExactMatch = formCollection["AuthorLookupIsExactMatch"];
            }
            viewModel.Search();
            return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", viewModel);
        }
        
        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        
        public PartialViewResult RenderLookupModal()
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                return PartialView("~/Views/Taxonomy/Author/Modals/_Lookup.cshtml",viewModel);
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
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.TableName = "taxonomy_author";
                viewModel.PageTitle = "Add Author";
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
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
                AuthorViewModel viewModel = new AuthorViewModel();
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
