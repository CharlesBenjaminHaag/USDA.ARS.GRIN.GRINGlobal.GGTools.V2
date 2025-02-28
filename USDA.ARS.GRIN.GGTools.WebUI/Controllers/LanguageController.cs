using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class LanguageController : BaseController, IController<CommonNameLanguageViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
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
        public ActionResult Add()
        {
            try
            {
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
                viewModel.EventAction = "Add";
                viewModel.TableName = "taxonomy_common_name_lang";
                viewModel.TableCode = "CommonNameLanguage";
                //viewModel.PageTitle = viewModel.EventAction + " " + viewModel.TableCode;
                viewModel.PageTitle = "Common Name Language Search";
                return View("~/Views/" + viewModel.TableCode + "/Edit.cshtml", viewModel);
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

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
                viewModel.PageTitle = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"] + " " + RouteData.Route.GetRouteData(this.HttpContext).Values["action"];
                viewModel.TableName = "taxonomy_common_name_lang";
                viewModel.TableCode = "CommonNameLanguage";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format(viewModel.EventAction + " " + viewModel.TableCode + String.Format(" [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.LanguageName));
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = viewModel.EventAction + " " + viewModel.TableCode;
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
        public ActionResult Edit(CommonNameLanguageViewModel viewModel)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult BatchEdit(FormCollection formCollection)
        {
            CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
            viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;

            if (!String.IsNullOrEmpty(formCollection["IDList"]))
            {
                viewModel.Entity.ItemIDList = formCollection["IDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["CountryCode"]))
            {
                viewModel.Entity.CountryCode = formCollection["CountryCode"];
            }
            viewModel.Update();
            return null;
        }

        public ActionResult Index()
        {
            CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
            viewModel.PageTitle = "Language Home";
            return View(viewModel);
        }

        public ActionResult Search(CommonNameLanguageViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/Language/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult FolderItems(int folderId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}