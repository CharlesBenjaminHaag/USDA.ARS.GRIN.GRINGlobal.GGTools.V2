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
    public class NoteController: BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public PartialViewResult Lookup(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Shared/Modals/_NoteSelectList.cshtml";
            ReferenceViewModel viewModel = new ReferenceViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.SearchEntity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["Note"]))
            {
                viewModel.SearchEntity.SearchText = formCollection["Note"];
                viewModel.SearchEntity.SearchText = viewModel.SearchEntity.SearchText.Replace(" ", "%");
            }

            viewModel.SearchNotes();
            return PartialView(partialViewName, viewModel);
        }
    }
}