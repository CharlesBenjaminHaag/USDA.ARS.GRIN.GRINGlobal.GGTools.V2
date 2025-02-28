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
    public class AppUserItemFolderController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            viewModel.TableName = "app_user_item_folder";
            return View(viewModel);
        }
        public ActionResult Explorer()
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            try
            { 
                viewModel.TableName = "app_user_item_folder";
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Search();
                return View("~/Views/AppUserItemFolder/Explorer/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(AppUserItemFolderViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                //viewModel.GetFolderTypes();
                //viewModel.GetFolderCategories();
                return View("~/Views/AppUserItemFolder/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult _List(int formatCode = 1, int cooperatorId = 0, string folderType = "", string isFavorite = null, string timeFrame = "", string isShared = "N")
        {
           AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();

            if (isShared == "N")
            {
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            }
            else
            {
                viewModel.SearchEntity.SharedWithCooperatorID = AuthenticatedUser.CooperatorID;
            }

            viewModel.SearchEntity.IsFavorite = isFavorite;
            viewModel.SearchEntity.TimeFrame = timeFrame;
            viewModel.SearchEntity.IsShared = isShared;
            viewModel.SearchEntity.FolderType = folderType;
            viewModel.Search();

            return PartialView("~/Views/AppUserItemFolder/_ListWidget.cshtml", viewModel);
        }
        
        public ActionResult Edit(int entityId)
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel(AuthenticatedUser.CooperatorID);
                viewModel.TableName = "app_user_item_folder";
                viewModel.TableCode = "AppUserItemFolder";
                viewModel.AuthenticatedUser = AuthenticatedUser;

                if (entityId > 0)
                {
                    viewModel.SearchEntity.ID = entityId;
                    viewModel.Get();
                    viewModel.PageTitle = String.Format("Edit Folder: {0}", viewModel.Entity.FolderName);
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Folder");
                }

                ViewBag.PageTitle = viewModel.PageTitle;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        [HttpPost]
        public PartialViewResult Add(AppUserItemFolderViewModel viewModel)
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
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Get();
                viewModel.EventAction = "ADD";
                return PartialView("~/Views/AppUserItemFolder/_Confirmation.cshtml", viewModel);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
       
        public ActionResult EditDetails(AppUserItemFolderViewModel viewModel)
        {
            try
            {
                viewModel.Entity.IsFavorite = viewModel.FromBool(viewModel.IsFavoriteSelector);
                viewModel.Update();
                return RedirectToAction("Edit", "AppUserItemFolder", new { @entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult AddItems(AppUserItemFolderViewModel viewModel)
        {
            AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
            appUserItemFolderViewModel.SearchEntity.ID = viewModel.Entity.ID;
            appUserItemFolderViewModel.Get();
            appUserItemFolderViewModel.Entity.TableName = viewModel.Entity.TableName;
            appUserItemFolderViewModel.EntityIDList = viewModel.EntityIDList;
            appUserItemFolderViewModel.InsertItems();
            return PartialView("~/Views/AppUserItemFolder/_Confirmation.cshtml", appUserItemFolderViewModel);
        }
        public PartialViewResult RenderEditModal(string sysTableName, string eventValue = "", int parentEntityId = 0)
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel(AuthenticatedUser.CooperatorID);
                viewModel.EventValue = eventValue;
                viewModel.Entity.ParentID = parentEntityId;
                if (String.IsNullOrEmpty(sysTableName))
                {
                    throw new IndexOutOfRangeException("Table name not specified.");
                }

                //viewModel.GetFolderCategories(AuthenticatedUser.CooperatorID);
                //viewModel.GetAvailableFolders(AuthenticatedUser.CooperatorID, tableName);
                return PartialView("~/Views/AppUserItemFolder/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        [HttpPost]
        public JsonResult DeleteItemByEntityID(FormCollection formCollection)
        {
            try 
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
                viewModel.DeleteItemByEntityID(Int32.Parse(GetFormFieldValue(formCollection, "AppUserItemFolderID")), Int32.Parse(GetFormFieldValue(formCollection, "IDNumber")));
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
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
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
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

                //viewModel.Get();
                viewModel.DeleteItems();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult RenderFolderItemDeleteModal(int entityId)
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
                viewModel.SearchEntity.ID = entityId;
                viewModel.Get();
                return PartialView("~/Views/AppUserItemFolder/Modals/_BatchDelete.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderRelatedFoldersMenu(string sysTableName, int entityId = 0)
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();

            try
            {
                if (String.IsNullOrEmpty(sysTableName))
                {
                    throw new IndexOutOfRangeException("Error in FolderController._RelatedFoldersMenu(): Table name not specified.");
                }

                if (entityId == 0)
                {
                    throw new IndexOutOfRangeException("Error in FolderController._RelatedFoldersMenu(): ID field not specified.");
                }

                viewModel.Entity.ID = entityId;
                viewModel.TableName = sysTableName;
                viewModel.SearchEntity.TableName = sysTableName;
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.GetRelatedFolders();
                return PartialView("~/Views/AppUserItemFolder/_RelatedFoldersMenu.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderImportModal()
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel(AuthenticatedUser.CooperatorID);
                return PartialView("~/Views/AppUserItemFolder/Modals/_Import.cshtml", viewModel);
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
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
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
                    viewModel.Entity.FolderType = "STATIC";
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

                return PartialView("~/Views/AppUserItemFolder/_Confirmation.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        /// <summary>
        /// Renders a widget that displays all folders that a given entity
        /// is currently contained in.
        /// </summary>
        /// <param name="appUserItemFolderId"></param>
        /// <returns></returns>
        /// <remarks>Used on edit page.</remarks>
        
        public PartialViewResult RenderRelatedFoldersWidget(int idNumber)
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            try
            {
                viewModel.SearchEntity.EntityID = idNumber;
                viewModel.Search();
                return PartialView("~/Views/AppUserItemFolder/_RelatedFoldersWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
     
        public PartialViewResult GetWidget(int appUserItemFolderId)
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            try
            {
                viewModel.SearchEntity.EntityID = appUserItemFolderId;
                viewModel.Search();
                return PartialView("~/Views/AppUserItemFolder/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #region Dynamic Folder

        public PartialViewResult RenderDynamicFolderEditModal()
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel(AuthenticatedUser.CooperatorID);
                return PartialView("~/Views/AppUserItemFolder/Modals/_EditDynamic.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListDynamic(int formatCode = 1, int cooperatorId = 0, string folderType = "", string dataType = "", string isFavorite = null, string timeFrame = "", string isShared = "N")
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            try
            {
                viewModel.GetDynamicFolders(cooperatorId, dataType);
                return PartialView("~/Views/AppUserItemFolder/_ListWidgetDynamic.cshtml", viewModel);
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