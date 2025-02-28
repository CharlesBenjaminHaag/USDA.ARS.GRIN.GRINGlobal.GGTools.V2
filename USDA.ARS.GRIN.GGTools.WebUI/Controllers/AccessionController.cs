using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class AccessionController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: Accession
        public ActionResult Index(int speciesId = 0)
        {
            try
            { 
                AccessionViewModel viewModel = new AccessionViewModel();

                if (speciesId > 0)
                { 
                    viewModel.SearchEntity.SpeciesID = speciesId;
                    viewModel.Search();
                }
                viewModel.PageTitle = "Accessions";
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Edit(int entityId = 0, int appUserItemFolderId = 0)
        {
            try
            {
                AccessionViewModel viewModel = new AccessionViewModel();
                viewModel.TableName = "accession";
                viewModel.TableCode = "Accession";
                viewModel.PageTitle = String.Format("Edit Accession [{0}]", entityId);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                //viewModel.Edit(entityId);
                return View("~/Views/Accession/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _List(int speciesId = 0)
        {
            try
            {
                AccessionViewModel viewModel = new AccessionViewModel();
                viewModel.SearchEntity.SpeciesID = speciesId;
                viewModel.Search();
                return PartialView("~/Views/Accession/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}