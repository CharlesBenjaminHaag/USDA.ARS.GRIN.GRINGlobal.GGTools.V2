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
    public class ClassificationController :  BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Order/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            ClassificationViewModel viewModel = new ClassificationViewModel();
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
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add()
        {
            ClassificationViewModel viewModel = new ClassificationViewModel();

            try
            { 
                viewModel.TableName = "taxonomy_classification";
                viewModel.PageTitle = "Add Order";
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        //[HttpPost]
        //public JsonResult MapFamilies(FormCollection formCollection)
        //{
        //    ClassificationViewModel viewModel = new ClassificationViewModel();
        //    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
        //    if (!String.IsNullOrEmpty(formCollection["ID"]))
        //    {
        //        viewModel.Entity.ID = Int32.Parse(formCollection["ID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["IDList"]))
        //    {
        //        viewModel.ItemIDList = formCollection["IDList"];
        //    }
        //    //viewModel.MapFamilies();
        //    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                ClassificationViewModel viewModel = new ClassificationViewModel();
                viewModel.TableName = "taxonomy_classification";
                viewModel.TableCode = "Classification";
                viewModel.Get(entityId);
                viewModel.EventAction = "Edit";
                viewModel.PageTitle = String.Format(viewModel.EventAction + " " + viewModel.TableCode + String.Format(" [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.OrderName));
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(ClassificationViewModel viewModel)
        {
            try
            {
                //REFACTOR CBH 11/9/21
                viewModel.Entity.TableName = viewModel.TableName;

                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                }

                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Entity.ID = viewModel.Insert();
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }

                return RedirectToAction("Edit", "Classification", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        // GET: Order
        public ActionResult Index(string sysTableName = "")
        {
            ClassificationViewModel viewModel = new ClassificationViewModel();

            try
            { 
                viewModel.PageTitle = "Order Search";
                viewModel.TableName = "taxonomy_classification";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        [HttpPost]
        public ActionResult Search(ClassificationViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
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
                    sysFolderViewModel.Entity.Properties = viewModel.SerializeToXml<ClassificationSearch>(viewModel.SearchEntity);
                    sysFolderViewModel.Entity.TypeCode = "DYN";
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
        [HttpPost]
        public PartialViewResult Lookup(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Order/Modals/_SelectList.cshtml";
            ClassificationViewModel viewModel = new ClassificationViewModel();

            if (!String.IsNullOrEmpty(formCollection["LookupOrderName"]))
            {
                viewModel.SearchEntity.Name = formCollection["LookupOrderName"];
            }
            viewModel.Search();
            return PartialView(partialViewName, viewModel);
        }

        //public PartialViewResult FolderItems(int folderId)
        //{
        //    try
        //    {
        //        ClassificationViewModel viewModel = new ClassificationViewModel();
        //        viewModel.EventAction = "SEARCH";
        //        viewModel.EventValue = "FOLDER";
        //        viewModel.SearchEntity.FolderID = sysFolderId;
        //        viewModel.SearchFolderItems();
        //        ModelState.Clear();
        //        return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        [HttpPost]
        public PartialViewResult LookupNotes(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Classification/Modals/_NoteSelectList.cshtml";
            ClassificationViewModel viewModel = new ClassificationViewModel();

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
                ClassificationViewModel viewModel = new ClassificationViewModel();
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