using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.GOBS.ViewModelLayer;
using NLog;
using System.Security.Permissions;
using DataTables;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class GOBSController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDatasets(AuthenticatedUser.CooperatorID);
            return View(viewModel);
        }

        #region Dataset

        public PartialViewResult GetDatasets()
        {
            SysDynamicQueryViewModel viewModel = new SysDynamicQueryViewModel();
            viewModel.SearchEntity.SQLStatement = "SELECT * FROM get_gobs_dataset";
            viewModel.Search();
            return PartialView("~/Views/SysDynamicQuery/_SearchResultsList.cshtml", viewModel);
        }

        //public ViewResult GetDataset(string objectType)
        //{
        //    GOBSViewModel viewModel = new GOBSViewModel();
        //    SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();
        //    sysDynamicQueryViewModel.SearchEntity.SQLStatement = "SELECT * FROM get_" + objectType;
        //    sysDynamicQueryViewModel.Search();

        //    //viewModel.DataCollectionDataTable = sysDynamicQueryViewModel.DataCollectionDataTable;
        //    return Edit("~/Views/GOBS/Edit.cshtml", viewModel);
        //}

        public ActionResult AddDataset()
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            return View("~/Views/GOBS/EditDataset.cshtml", viewModel);
        }

        public ActionResult EditDataset(int datasetId)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDataset(AuthenticatedUser.CooperatorID, datasetId);

            if (viewModel.DatasetEntity.authorized == 1)
            {
                ViewBag.PageTitle = "Edit Dataset";
            }
            else
            {
                ViewBag.PageTitle = "Edit Dataset";
            }

            return View("~/Views/GOBS/EditDataset.cshtml", viewModel);
        }
       
        public PartialViewResult GetAll()
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDatasets(AuthenticatedUser.CooperatorID);
            return PartialView("~/Views/GOBS/_ListDatasets.cshtml", viewModel);
        }

        public PartialViewResult GetDatasetDetailEditor(int dataSetId)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            //viewModel.GetDataset(dataSetId);
            viewModel.TableTitle = "GOBS Dataset";
            return PartialView("~/Views/GOBS/_EditDataset.cshtml", viewModel);
        }

        #endregion Dataset

        #region Dataset Marker

        public ActionResult AddDatasetMarker()
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            return View("~/Views/GOBS/EditDatasetMarker.cshtml", viewModel);
        }

        public ActionResult EditDatasetMarker(int datasetMarkerId)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDataSetMarker(AuthenticatedUser.CooperatorID, datasetMarkerId);

            if (viewModel.DatasetEntity.authorized == 1)
            {
                ViewBag.PageTitle = "Edit Dataset Marker";
            }
            else
            {
                ViewBag.PageTitle = "Edit Dataset Marker";
            }

            return View("~/Views/GOBS/EditDatasetMarker.cshtml", viewModel);
        }

        #endregion

        #region Dataset Inventory

        public PartialViewResult GetDatasetInventoryEditor(int entityId)
        {
            //TODO
            return null;
        }

        #endregion Dataset Inventory

        #region Dataset Value

        public PartialViewResult GetDatasetValueEditor(int entityId)
        {
            //TODO
            return null;
        }

        #endregion Dataset Value

        #region Dataset Field
        public PartialViewResult GetDataSetFields()
        {
            //TODO
            return null;
        }
        #endregion Dataset Field

        #region Dataset Marker

        public PartialViewResult GetDatasetMarkerEditor(int entityId)
        {
            //TODO
            return null;
        }

        #endregion Dataset Marker

        #region Dataset Marker Field

        public PartialViewResult GetDatasetMarkerFieldEditor(int entityId)
        {
            //TODO
            return null;
        }

        #endregion Dataset Marker Field

        #region Dataset Marker Value
        
        public PartialViewResult GetDatasetMarkerValueEditor(int entityId)
        {
            //TODO
            return null;
        }

        #endregion Dataset Marker Value

        #region Report Value

        #endregion Report Value

        #region Report Trait

        #endregion Report Trait

        public PartialViewResult RenderSidebar()
        {
            return PartialView("~/Views/Shared/Sidebars/_MainSidebarGOBS.cshtml");
        }
    }
}