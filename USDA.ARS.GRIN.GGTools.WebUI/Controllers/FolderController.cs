using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    public class FolderController : BaseController, IController<FolderViewModel>
    {
        protected static string BASE_PATH = "~/Views/Folder/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int folderId)
        {
            try
            {
                return PartialView("~/Views/Shared/_UnderConstruction.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult Explorer(string categoryCode = "", string isFavorite="")
        {
            FolderViewModel viewModel = new FolderViewModel();

            try
            {
                viewModel.PageTitle = "TurboTaxon Home";
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.GetFolderTypes(AuthenticatedUser.CooperatorID);
                viewModel.GetFolderCategories(AuthenticatedUser.CooperatorID);
                return View(BASE_PATH + "/Explorer/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
       
        [HttpPost]
        public ActionResult Search(FolderViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                //viewModel.GetFolderTypes();
                //viewModel.GetFolderCategories();
                return View("~/Views/Folder/Search.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult _Lookup(FormCollection formCollection)
        {
            FolderViewModel viewModel = new FolderViewModel();

            if (!String.IsNullOrEmpty(formCollection["Category"]))
            {
                viewModel.SearchEntity.Category = formCollection["Category"];
            }

            if (!String.IsNullOrEmpty(formCollection["CreatedByCooperatorID"]))
            {
                viewModel.SearchEntity.CreatedByCooperatorID = Int32.Parse(formCollection["CreatedByCooperatorID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["SelectedTypeList"]))
            {
                viewModel.SearchEntity.TypeList = formCollection["SelectedTypeList"];
            }

            if (!String.IsNullOrEmpty(formCollection["SelectedCategoryList"]))
            {
                viewModel.SearchEntity.CategoryList = formCollection["SelectedCategoryList"];
            }

            viewModel.Search();
            return PartialView("~/Views/Folder/Modals/_ItemList.cshtml", viewModel);
        }
        public PartialViewResult _ListExplorer(string folderCategory = "", string folderType = "")
        {
            FolderViewModel viewModel = new FolderViewModel();

            if (!String.IsNullOrEmpty(folderType))
            {
                if (folderType.Contains("default"))
                {
                    folderType = "";
                }
            }

            if (!String.IsNullOrEmpty(folderCategory))
            {
                if (folderCategory.Contains("default"))
                {
                    folderCategory = "";
                }
                
                if (folderCategory.ToLower() == "favorites")
                {
                    viewModel.SearchEntity.IsFavorite = "Y";
                }
                else
                {
                    viewModel.SearchEntity.FolderType = folderType;
                }
            }

            viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

            if (!String.IsNullOrEmpty(viewModel.SearchEntity.Category))
            {
                if (viewModel.SearchEntity.Category.ToLower() == "favorites")
                {
                    viewModel.SearchEntity.IsFavorite = "Y";
                }
                else
                {
                    viewModel.SearchEntity.Category = folderCategory;
                }
            }
            
            viewModel.SearchEntity.FolderTypeDescription = folderType;
            viewModel.Search();
            return PartialView("~/Views/Folder/_List.cshtml", viewModel);
        }
        public PartialViewResult _List(string folderType = "", int cooperatorId = 0, string timeFrame = "")
        {
            FolderViewModel viewModel = new FolderViewModel();
            viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.SearchEntity.FolderTypeDescription = folderType;
            viewModel.SearchEntity.OwnedByCooperatorID = cooperatorId;
            viewModel.Search();

            //TODO
            //Get separate list of folders shared with logged-in user.

            return PartialView("~/Views/Folder/_List.cshtml", viewModel);
        }
        public PartialViewResult _ListMyFavoriteFolders(string folderType = "", string timeFrame = "")
        {
            FolderViewModel viewModel = new FolderViewModel();

            try 
            { 
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.SearchEntity.FolderTypeDescription = folderType;
                viewModel.SearchEntity.IsFavorite = "Y";
                viewModel.Search();
                return PartialView("~/Views/Folder/_ListMyFolders.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListMyRecentFolders(string folderType = "", string timeFrame = "")
        {
            FolderViewModel viewModel = new FolderViewModel();

            try 
            { 
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.SearchEntity.FolderTypeDescription = folderType;
                viewModel.Search();
                return PartialView("~/Views/Folder/_ListMyFolders.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListMySharedFolders(string folderType = "", string timeFrame = "")
        {
            FolderViewModel viewModel = new FolderViewModel();

            try 
            { 
                viewModel.SearchEntity.SharedWithCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.SearchEntity.FolderTypeDescription = folderType;
                viewModel.SearchEntity.IsShared = true;
                viewModel.Search();
                return PartialView("~/Views/Folder/_ListMyFolders.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult _ItemList(int entityId, string eventAction)
        {
            try 
            {
                return RedirectToAction("_ListFolderItems", eventAction, new { folderId = 1 });
                //return PartialView("~/Views/Folder/Modals/_ItemList.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _RelatedFoldersList(string tableName, int entityId)
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel();
                viewModel.SearchEntity.TableName = tableName;
                viewModel.SearchEntity.EntityID = entityId;
                viewModel.GetRelatedFolders();
                return PartialView("~/Views/Folder/_RelatedFoldersList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _RelatedFoldersMenu(string tableName, int entityId = 0)
        {
            FolderViewModel viewModel = new FolderViewModel();

            try
            {
                if (String.IsNullOrEmpty(tableName))
                {
                    throw new IndexOutOfRangeException("Error in FolderController._RelatedFoldersMenu(): Table name not specified.");
                }

                if (entityId == 0)
                {
                    throw new IndexOutOfRangeException("Error in FolderController._RelatedFoldersMenu(): ID field not specified.");
                }

                viewModel.Entity.ID = entityId;
                viewModel.TableName = tableName;
                viewModel.SearchEntity.TableName = tableName;
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.GetRelatedFolders();
                return PartialView("~/Views/Folder/_RelatedFoldersMenu.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderSidebar()
        {
            FolderViewModel viewModel = new FolderViewModel();

            try
            {
                return PartialView("~/Views/Shared/Sidebars/_MainSidebarFolder.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        /// <summary>
        /// Adds a list of records of a given data type to a newly-created
        /// folder.
        /// </summary>
        /// <param name="coll"></param>
        /// <returns>A partial view containing:
        /// 1) the name of the newly-created
        /// folder
        /// 2) a link to the folder detail page.
        /// </returns>
        [HttpPost]
        public PartialViewResult Add(FormCollection coll)
        {
            FolderViewModel viewModel = new FolderViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(coll["FolderID"]);
                }

                if (!String.IsNullOrEmpty(coll["IDList"]))
                {
                    viewModel.Entity.ItemIDList = coll["IDList"];
                }

                if (!String.IsNullOrEmpty(coll["TableName"]))
                {
                    viewModel.Entity.TableName = coll["TableName"];
                }

                if (!String.IsNullOrEmpty(coll["FolderName"]))
                {
                    viewModel.Entity.FolderName = coll["FolderName"];
                }

                if (!String.IsNullOrEmpty(coll["FolderType"]))
                {
                    viewModel.Entity.FolderType = "FOLDER";
                }

                if (!String.IsNullOrEmpty(coll["NewCategory"]))
                {
                    viewModel.Entity.Category = coll["NewCategory"];
                }
                else
                {
                    if (!String.IsNullOrEmpty(coll["Category"]))
                    {
                        viewModel.Entity.Category = coll["Category"];
                    }
                }

                if (!String.IsNullOrEmpty(coll["Description"]))
                {
                    viewModel.Entity.Description = coll["Description"];
                }

                if (!String.IsNullOrEmpty(coll["IsFavorite"]))
                {
                    viewModel.Entity.IsFavorite = coll["IsFavorite"];
                }

                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Insert();
                }
                else
                {
                    viewModel.Update();
                }

                // TEST Return new folder as JSON
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Search();
                viewModel.EventAction = "ADD";
                return PartialView("~/Views/Folder/_Confirmation.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        [HttpPost]
        public PartialViewResult Import(FormCollection coll)
        {
            FolderViewModel viewModel = new FolderViewModel();
            List<AppUserItemFolder> batchedFolders = new List<AppUserItemFolder>();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(coll["FolderID"]);
                }

                if (!String.IsNullOrEmpty(coll["IDList"]))
                {
                    viewModel.Entity.ItemIDList = coll["IDList"];
                }

                if (!String.IsNullOrEmpty(coll["TableName"]))
                {
                    viewModel.Entity.TableName = coll["TableName"];
                }

                if (!String.IsNullOrEmpty(coll["FolderName"]))
                {
                    viewModel.Entity.FolderName = coll["FolderName"];
                }

                if (!String.IsNullOrEmpty(coll["FolderType"]))
                {
                    viewModel.Entity.FolderType = coll["FolderType"];
                }

                if (!String.IsNullOrEmpty(coll["NewCategory"]))
                {
                    viewModel.Entity.Category = coll["NewCategory"];
                }
                else
                {
                    if (!String.IsNullOrEmpty(coll["Category"]))
                    {
                        viewModel.Entity.Category = coll["Category"];
                    }
                }

                if (!String.IsNullOrEmpty(coll["Description"]))
                {
                    viewModel.Entity.Description = coll["Description"];
                }

                if (!String.IsNullOrEmpty(coll["IsFavorite"]))
                {
                    viewModel.Entity.IsFavorite = coll["IsFavorite"];
                }

                viewModel.Import();

                // Retrieve newly-created folder. Return confirmation
                // widget.
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Search();
                viewModel.EventAction = "ADD";

                // Add each generated folder to session-stored list.
                if (Session["IMPORTED-FOLDER-LIST"] != null)
                {
                   batchedFolders = Session["IMPORTED-FOLDER-LIST"] as List<AppUserItemFolder>;
                }
                batchedFolders.AddRange(viewModel.DataCollection);
                Session["IMPORTED-FOLDER-LIST"] = batchedFolders;
                viewModel.DataCollectionBatch = batchedFolders;

                return PartialView("~/Views/Folder/_Confirmation.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public JsonResult AddFolderItem(int folderId, int entityId, string tableName)
        {
            try
            {
                //TODO
                return Json(new { success = "DEBUG" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult Update(FormCollection coll)
        {
            FolderViewModel viewModel = new FolderViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(coll["FolderID"]);
                }

                if (!String.IsNullOrEmpty(coll["IDList"]))
                {
                    viewModel.Entity.ItemIDList = coll["IDList"];
                }

                if (!String.IsNullOrEmpty(coll["TableName"]))
                {
                    viewModel.Entity.TableName = coll["TableName"];
                    viewModel.Entity.FolderType = "STATIC";
                }

                viewModel.InsertFolderItems();

                // TEST Return new folder as JSON
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Search();
                viewModel.EventAction = "UPDATE";
                return PartialView("~/Views/Folder/_Confirmation.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #region Cooperator
        public PartialViewResult RenderFolderCooperatorWidget(int folderId)
        {
            FolderViewModel viewModel = new FolderViewModel();
            viewModel.Entity.ID = folderId;
            viewModel.GetCurrentCooperators();
            return PartialView("~/Views/Folder/Cooperator/_Widget.cshtml", viewModel);
        }

        public PartialViewResult RenderFolderCooperatorEditModal(int entityId)
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel();
                viewModel.Entity.ID = entityId;
                viewModel.GetAvailableCooperators();
                viewModel.GetCurrentCooperators();
                return PartialView("~/Views/Folder/Cooperator/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderFolderItemDeleteModal(int entityId)
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel();
                viewModel.SearchEntity.ID = entityId;
                viewModel.Get();
                return PartialView("~/Views/Folder/Modals/_BatchDelete.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }


        [HttpPost]
        public JsonResult AddCooperators(FormCollection coll)
        {
            FolderViewModel viewModel = new FolderViewModel();

            try
            {
                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(coll["FolderID"]);
                }

                if (!String.IsNullOrEmpty(coll["IDList"]))
                {
                    viewModel.ItemIDList = coll["IDList"];
                }
                //TODO
                viewModel.InsertCollaborators();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteCooperators(FormCollection coll)
        {
            FolderViewModel viewModel = new FolderViewModel();

            try
            {
                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(coll["FolderID"]);
                }

                if (!String.IsNullOrEmpty(coll["IDList"]))
                {
                    viewModel.ItemIDList = coll["IDList"];
                }
                viewModel.DeleteCollaborators();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult _ListAvailableCooperators(FormCollection formCollection)
        {
            FolderViewModel viewModel = new FolderViewModel();

            if (!String.IsNullOrEmpty(formCollection["FolderID"]))
            {
                viewModel.Entity.ID = Int32.Parse(formCollection["FolderID"]);
            }
            viewModel.GetAvailableCooperators();
            return PartialView("~/Views/Folder/Cooperator/_ListAvailable.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult _ListCurrentCooperators(FormCollection formCollection)
        {
            FolderViewModel viewModel = new FolderViewModel();

            if (!String.IsNullOrEmpty(formCollection["FolderID"]))
            {
                viewModel.Entity.ID = Int32.Parse(formCollection["FolderID"]);
            }
            viewModel.GetCurrentCooperators();
            return PartialView("~/Views/Folder/Cooperator/_ListCurrent.cshtml", viewModel);
        }

        #endregion

        [HttpPost]
        public JsonResult AddItems(FormCollection coll)
        {
            FolderViewModel viewModel = new FolderViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.SearchEntity.ID = Int32.Parse(coll["FolderID"]);
                }

                viewModel.Get(viewModel.SearchEntity.ID);

                if (!String.IsNullOrEmpty(coll["IDList"]))
                {
                    viewModel.Entity.ItemIDList = coll["IDList"];
                }
                
                viewModel.InsertFolderItems();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
     
        public ActionResult _Edit(string tableName)
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel(AuthenticatedUser.CooperatorID, tableName);

                if (String.IsNullOrEmpty(tableName))
                {
                    throw new IndexOutOfRangeException("Table name not specified.");
                }
                
                viewModel.GetFolderCategories(AuthenticatedUser.CooperatorID);
                viewModel.GetAvailableFolders(AuthenticatedUser.CooperatorID, tableName);
                return PartialView("~/Views/Folder/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel();
                viewModel.TableName = "app_user_item_folder";
                viewModel.TableCode = "Folder";
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.GetFolderCategories(AuthenticatedUser.CooperatorID);

                if (entityId > 0)
                {
                    viewModel.SearchEntity.ID = entityId;
                    viewModel.Get();
                    viewModel.PageTitle = String.Format("Edit Folder: {0}", viewModel.Entity.FolderName);
                    viewModel.TableName = viewModel.Entity.FolderType;
                    
                    viewModel.ItemViewName = "vw_GRINGlobal_Folder_" + viewModel.TableName;
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Folder");
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(FolderViewModel viewModel)
        {
            try
            {
                if (viewModel.EventAction == "DELETE")
                {
                    viewModel.Entity.TableName = viewModel.TableName;
                    viewModel.Delete();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (!viewModel.Validate())
                    {
                        if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                    }

                    if (!String.IsNullOrEmpty(viewModel.Entity.NewCategory))
                    {
                        viewModel.Entity.Category = viewModel.Entity.NewCategory;
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
                    return RedirectToAction("Edit", "Folder", new { entityId = viewModel.Entity.ID });
                }
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
                FolderViewModel viewModel = new FolderViewModel();
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
        
        public ActionResult DeleteItem(int folderId, int appUserItemListId)
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel();
                viewModel.DeleteItem(appUserItemListId);
                return RedirectToAction("Edit", "Folder", new { @entityId = folderId });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult DeleteItems(FormCollection coll)
        {
            FolderViewModel viewModel = new FolderViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.SearchEntity.ID = Int32.Parse(coll["FolderID"]);
                }

                viewModel.Get(viewModel.SearchEntity.ID);

                if (!String.IsNullOrEmpty(coll["ItemIDList"]))
                {
                    viewModel.ItemIDList = coll["ItemIDList"];
                }
                viewModel.DeleteItems();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(int entityId)
        {
            try 
            {
                FolderViewModel viewModel = new FolderViewModel();
                viewModel.Entity.ID = entityId;
                viewModel.Delete();
                return RedirectToAction("Index","Home");
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

        public ActionResult Index()
        {
            FolderViewModel viewModel = new FolderViewModel();

            try
            {
                viewModel.PageTitle = "Folder Search";
                viewModel.TableName = "app_user_item_folder";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                //viewModel.GetFolderTypes();
                viewModel.GetFolderCategories();
                return View(BASE_PATH + "Search.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Import()
        {
            FolderViewModel viewModel = new FolderViewModel();
            return View("~/Views/Folder/Import/Index.cshtml", viewModel);
        }
        public PartialViewResult RenderImportModal()
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel();

                viewModel.GetFolderCategories(AuthenticatedUser.CooperatorID);
                return PartialView("~/Views/Folder/Modals/_Import.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}
