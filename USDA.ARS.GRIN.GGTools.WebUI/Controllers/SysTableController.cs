using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SysTableController : BaseController, IController<SysTableViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _List(string databaseAreaCode = "")
        {
            SysTableViewModel viewModel = new SysTableViewModel();
            try
            {
                viewModel.SearchEntity.DatabaseAreaCode = databaseAreaCode;
                viewModel.GetSysTablesTaxonomy(false);
                return PartialView("~/Views/SysTable/_MenuList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
      
        public PartialViewResult _ListFolderItems(int sysFolderId)
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
        public PartialViewResult RenderWidget()
        {
            SysTableViewModel viewModel = new SysTableViewModel();
            return PartialView("~/Views/SysTable/_Widget.cshtml", viewModel);
        }
        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                SysTableViewModel viewModel = new SysTableViewModel();
                viewModel.TableName = "sys_table";
                viewModel.PageTitle = String.Format("Edit Sys Table [{0}]", entityId);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Get(entityId);
                return View("~/Views/SysTable/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Edit(SysTableViewModel viewModel)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public JsonResult TransferOwnership(string idList, string sysTableName, int ownedByCooperatorId)
        {
            try 
            {
                SysTableViewModel viewModel = new SysTableViewModel();
                viewModel.TransferOwnership(idList, sysTableName, ownedByCooperatorId);
                return Json("SUCCESS", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: SysTable
        public ActionResult Index()
        {
            return View();
        }
        
        public PartialViewResult _RenderLookupModal(string tableName = "")
        {
            SysPermissionViewModel viewModel = new SysPermissionViewModel();
            viewModel.SearchEntity.SysUserID = AuthenticatedUser.SysUserID;
            viewModel.SearchEntity.TableName = tableName;
            viewModel.GetPermissionsByTable();
            return PartialView("~/Views/SysTable/Modals/_Lookup.cshtml", viewModel);
        }

        public ActionResult Search(SysTableViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        #region Components

        public PartialViewResult Component_ListWidget(string sysTag)
        {
            SysTableViewModel viewModel = new SysTableViewModel();
            try
            {
                viewModel.SearchEntity.SysTag = sysTag;
                viewModel.Search();
                return PartialView("~/Views/SysTable/Components/_ListWidget.cshtml", viewModel);
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