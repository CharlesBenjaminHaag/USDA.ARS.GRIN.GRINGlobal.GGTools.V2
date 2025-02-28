using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class AppUserItemListController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult GetTabList()
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.GetTabList(AuthenticatedUser.CooperatorID);

                return PartialView("~/Views/AppUserItemList/Import/_TabList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult GetListsByTab(string tabName)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.GetListsByTab(AuthenticatedUser.CooperatorID, tabName);
                return PartialView("~/Views/AppUserItemList/Import/_ListsByTab.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult GetItemsByList(string listName)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.GetItemsByList(AuthenticatedUser.CooperatorID, listName);
                return PartialView("~/Views/AppUserItemList/Import/_ItemsByList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult _List(int appUserItemFolderId)
        {
            try
            {
                AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
                viewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
                viewModel.Search();
                return PartialView("~/Views/AppUserItemList/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(AppUserItemList viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: AppUserItemList
        public ActionResult Index()
        {
            try
            {
                AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public ActionResult Import()
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();

            try
            {
                //TODO Init page with
                // 1) Tabs
                // 2) Lists per tab
                // 3) Data-type groups per list
                return View("~/Views/AppUserItemList/Import/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
    }
}
       
        public PartialViewResult _List(string tabName = "", int cooperatorId = 0, int appUserItemFolderId = 0)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.SearchEntity.ListName = tabName;
                viewModel.SearchEntity.CreatedByCooperatorID = cooperatorId;
                viewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
                
                viewModel.Search();
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListByFolder(int cooperatorId = 0, int appUserItemFolderId = 0)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.GetSysTablesByAppUserItemFolder(appUserItemFolderId);
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public PartialViewResult _ListDynamic(int cooperatorId = 0, int appUserItemFolderId = 0)
        //{
        //    AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
        //    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
        //    AuthorViewModel authorViewModel = new AuthorViewModel();

        //    try
        //    {
        //        appUserItemFolderViewModel.SearchEntity.ID = appUserItemFolderId;
        //        appUserItemFolderViewModel.Search();

        //        appUserItemListViewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
        //        appUserItemListViewModel.GetDynamic();

        //        //TODO Based on folder data type, retrieve and deserialize XML from app user item list
        //        // record, and instantiate relevant viewmodel.

                
                
        //        switch (appUserItemFolderViewModel.Entity.DataType)
        //        {
        //            case "taxonomy_author":
                        
        //                break;
        //            case "citation":
                        
        //                break;
        //        }

        //        //TODO Can I refactor this? Need variable for each view model in a massive switch
        //        // statement. --CBH 10/6/23

        //        return PartialView("~/Views/AppUserItemList/_ListDynamic.cshtml", appUserItemListViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        [HttpPost]
        public ActionResult Search(AppUserItemListViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.EventAction = "SEARCH";
                viewModel.Search();
                ModelState.Clear();

                // Save search if attribs supplied.
                //if ((viewModel.EventAction == "SEARCH") && (viewModel.EventValue == "SAVE"))
                //{
                //    viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                //    //viewModel.SaveSearch();
                //}

                return View("~/Views/AppUserItemList/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult _Lookup(FormCollection formCollection)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try 
            {
                if (!String.IsNullOrEmpty(formCollection["TabName"]))
                {
                    viewModel.SearchEntity.TabName = formCollection["TabName"];
                }
                viewModel.Search();
                return PartialView("~/Views/WebOrder/Dashboard/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}