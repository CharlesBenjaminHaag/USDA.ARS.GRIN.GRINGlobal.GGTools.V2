using System.Web.Mvc;
using System;
using USDA.ARS.GRIN.GGTools.WebUI;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CommonNameLanguageController : BaseController, IController<CommonNameLanguageViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/CommonNameLanguage/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
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
        public ActionResult Add()
        {
            try
            {
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
                viewModel.TableName = "taxonomy_common_name_language";
                viewModel.PageTitle = "Add Common Name Language";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
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
        [HttpPost]
        public ActionResult Edit(CommonNameLanguageViewModel viewModel)
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
                return RedirectToAction("Edit", "CommonNameLanguage", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
                viewModel.TableName = "taxonomy_common_name_language";
                viewModel.TableCode = "CommonNameLanguage";
                viewModel.Get(entityId);
                viewModel.EventAction = "Edit";
                viewModel.PageTitle = String.Format("Edit Common Name Language [{0}]", viewModel.Entity.ID);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        [HttpPost]
        public JsonResult EditBatch(CommonNameLanguageViewModel viewModel)
        {
            // TODO

            return null;
        }
        public ActionResult Index()
        {
            try
            {
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
                ViewBag.PageTitle = "Common Name Language Search";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Search(CommonNameLanguageViewModel viewModel)
        {
            try
            {
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

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
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