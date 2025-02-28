using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SiteController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _Get(int entityId)
        {
            try
            {
                SiteViewModel viewModel = new SiteViewModel();
                viewModel.Get(entityId);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return PartialView("~/Views/Site/_EditAddress.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Save(CooperatorViewModel viewModel)
        {
            try
            {
                //if (!viewModel.Validate())
                //{
                //    if (viewModel.ValidationMessages.Count > 0) return Edit(viewModel);
                //}

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
                return _Get(viewModel.Entity.ID);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Edit(int entityId)
        {
            SiteViewModel viewModel = new SiteViewModel();
            
            try 
            { 
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.Get(entityId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(SiteViewModel viewModel)
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
                return RedirectToAction("Edit", "Site", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _Edit(int entityId)
        {
            try
            {
                SiteViewModel viewModel = new SiteViewModel();
                viewModel.Get(entityId);
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return PartialView("~/Views/Site/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Index()
        {
            SiteViewModel viewModel = new SiteViewModel();
            try 
            { 
                viewModel.PageTitle = "Site Search";
                viewModel.TableName = "site";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(SiteViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/Site/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _List()
        {
            //TODO
            return null;
        }

        public PartialViewResult Components_SiteUsersWidget(int siteId)
        {
            SiteViewModel viewModel = new SiteViewModel();
            try
            {
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.Get(siteId);
                viewModel.GetSiteActiveUsers(siteId);
                return PartialView("~/Views/Site/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderListWidget()
        {
            SiteViewModel viewModel = new SiteViewModel();
            try
            {
                viewModel.Search();
                return PartialView("~/Views/Site/_ListWidget.cshtml", viewModel);
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

        #region Components

        public PartialViewResult Component_SiteCuratorList(int siteId)
        {
            SiteViewModel viewModel = new SiteViewModel();
            try
            {
                viewModel.GetSiteActiveUsers(siteId);
                return PartialView("~/Views/Cooperator/Components/_SiteCuratorList.cshtml", viewModel);
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