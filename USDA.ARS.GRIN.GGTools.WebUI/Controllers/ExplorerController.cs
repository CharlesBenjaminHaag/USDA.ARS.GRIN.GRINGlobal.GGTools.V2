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
    public class ExplorerController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            TaxonomyExplorerViewModel viewModel = new TaxonomyExplorerViewModel();
            try
            {
                return View("~/Views/Taxonomy/Explorer/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _LookupFamily(FamilyMapViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Explorer/_ListFamily.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _LookupGenus(GenusViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Explorer/_ListGenus.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _LookupSpecies(SpeciesViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Explorer/_ListSpecies.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public PartialViewResult _ListSpeciesFolderItems(int folderId)
        //{
        //    SpeciesViewModel viewModel = new SpeciesViewModel();
        //    try
        //    {
        //        viewModel.EventAction = "FOLDER";
        //        viewModel.SearchEntity.FolderID = sysFolderId;
        //        viewModel.GetFolderItems();
        //        return PartialView("~/Views/Taxonomy/Explorer/_List.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        public PartialViewResult _ListFamily()
        {
            TaxonomyExplorerViewModel viewModel = new TaxonomyExplorerViewModel();
            FamilyMapViewModel viewModelFamily = new FamilyMapViewModel();
            try
            {
                viewModelFamily.SearchEntity.FamilyName = "Poa";
                //viewModelFamily.Search();
                viewModel.DataCollectionFamily = viewModelFamily.DataCollection;
                return PartialView("~/Views/Taxonomy/Explorer/_ListFamily.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public PartialViewResult _LookupFamily()
        //{ 
        
        //}

        public PartialViewResult _ListGenus(int familyId = 0)
        {
            GenusViewModel viewModel = new GenusViewModel();
            try
            {
                if (familyId == 0)
                {
                   //TODO
                }
                else
                {
                    viewModel.SearchEntity.FamilyID = familyId;
                }

                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Explorer/_ListGenus.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListSpecies(int genusId = 0)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                viewModel.SearchEntity.GenusID = genusId;
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Explorer/_ListSpecies.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        //public PartialViewResult _ListRelatedFolders(string tableName = "")
        //{
        //    try
        //    {
        //        FolderViewModel viewModel = new FolderViewModel();
        //        viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
        //        viewModel.SearchEntity.FolderType = tableName;
        //        viewModel.Search();
        //        return PartialView("~/Views/Taxonomy/Explorer/_ListFolders.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
    }
}