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
    public class SysTagController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: SysTag
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Save(SysTagViewModel viewModel)
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
                return Json(new { success = true, data = viewModel.Entity.ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(int sysTagId)
        {
            try
            {
                //TODO
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
        }

        #region Components
        
        public PartialViewResult Component_SelectList()
        {
            SysTagViewModel viewModel = new SysTagViewModel();
            viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.Search();
            return PartialView("~/Views/SysTag/Components/_SelectList.cshtml", viewModel);
        }

        /// <summary>
        /// Retrieves a list of all tags assigned to a given table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public PartialViewResult Component_SysTagsByTable(string tableName, int idNumber)
        {
            try
            {
                SysTagViewModel viewModel = new SysTagViewModel();
                viewModel.SearchEntity.TableName = tableName;
                viewModel.SearchEntity.IDNumber = idNumber;
                viewModel.Search();
                return PartialView("~/Views/SysTag/Components/_SysTagsByTable.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Component_Editor(string tableName, int idNumber)
        {
            try
            {
                SysTagViewModel viewModel = new SysTagViewModel();
                viewModel.TableName = tableName;
                viewModel.Entity.IDNumber = idNumber;
                return PartialView("~/Views/SysTag/Components/_Editor.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        /// <summary>
        /// Retrieves a list of tags from which the user can select one. List excludes tags already assigned to a given
        /// table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public PartialViewResult Component_Select(string tableName, int idNumber)
        {
            try
            {
                SysTagViewModel viewModel = new SysTagViewModel();

                return PartialView("~/Views/SysTag/Components/_Select.cshtml", viewModel);
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