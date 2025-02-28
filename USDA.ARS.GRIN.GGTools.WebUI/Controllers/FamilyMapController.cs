using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class FamilyMapController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/FamilyMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add(int orderId = 0, int familyMapId = 0, string rank="") 
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();
            try 
            {
                viewModel.Entity.Rank = viewModel.ToTitleCase(String.IsNullOrEmpty(rank) ? "family" : rank);
                viewModel.PageTitle = "Add " + viewModel.Entity.Rank;
                viewModel.TableName = "taxonomy_family_map";
                viewModel.Entity.OrderID = orderId;
                viewModel.Entity.ParentID = familyMapId;
                viewModel.Entity.IsAccepted = true;
                viewModel.Entity.IsAcceptedName = "Y";

                if (familyMapId > 0)
                {
                    FamilyMapViewModel familyMapViewModel = new FamilyMapViewModel();
                    familyMapViewModel.SearchEntity.ID = familyMapId;
                    familyMapViewModel.Search();
                    viewModel.Entity.FamilyID = familyMapViewModel.Entity.FamilyID;
                    viewModel.Entity.FamilyName = familyMapViewModel.Entity.FamilyName;
                    viewModel.Entity.SubfamilyID = familyMapViewModel.Entity.SubfamilyID;
                    viewModel.Entity.SubfamilyName = familyMapViewModel.Entity.SubfamilyName;
                    viewModel.Entity.TribeID = familyMapViewModel.Entity.TribeID;
                    viewModel.Entity.TribeName = familyMapViewModel.Entity.TribeName;
                    viewModel.Entity.SubtribeID = familyMapViewModel.Entity.SubtribeID;
                    viewModel.Entity.SubtribeName = familyMapViewModel.Entity.SubtribeName;
                }

                // If the family being added is an infraramilial rank, retrieve the map record of the parent
                // rank.
                //if (rank.ToUpper() != "FAMILY")
                //{
                //    FamilyMapViewModel familyMapViewModel = new FamilyMapViewModel();
                //    familyMapViewModel.SearchEntity.ID = familyMapId;
                //    familyMapViewModel.Search();
                //    viewModel.Entity.ParentID = familyMapViewModel.Entity.ID;
                //    viewModel.Entity.FamilyID = familyMapViewModel.Entity.FamilyID;
                //    viewModel.Entity.FamilyName = familyMapViewModel.Entity.FamilyName;
                //    viewModel.Entity.SubfamilyID = familyMapViewModel.Entity.SubfamilyID;
                //    viewModel.Entity.SubfamilyName = familyMapViewModel.Entity.SubfamilyName;
                //    viewModel.Entity.TribeID = familyMapViewModel.Entity.TribeID;
                //    viewModel.Entity.TribeName = familyMapViewModel.Entity.TribeName;
                //    viewModel.Entity.SubtribeID = familyMapViewModel.Entity.SubtribeID;
                //    viewModel.Entity.SubtribeName = familyMapViewModel.Entity.SubtribeName;

                    

                //    // TODO
                //    // Populate select lists of families and sub-familial data related to the
                //    // parent family.


                //}

                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                FamilyMapViewModel viewModel = new FamilyMapViewModel();

                if (entityId > 0)
                {
                    viewModel.TableName = "taxonomy_family_map";
                    viewModel.TableCode = "FamilyMap";
                    viewModel.AppUserItemFolderID = appUserItemFolderId;

                    viewModel.Get(entityId);
                    viewModel.PageTitle = String.Format("Edit {0} [{1}]", viewModel.Entity.AssembledName, viewModel.Entity.ID);
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Species");
                }
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult RenderLookupModal()
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(FamilyMapViewModel viewModel)
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
                return RedirectToAction("Edit", "FamilyMap", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        //public PartialViewResult FolderItems(int folderId)
        //{
        //    try
        //    {
        //        FamilyMapViewModel viewModel = new FamilyMapViewModel();
        //        viewModel.EventAction = "SEARCH";
        //        viewModel.EventValue = "FOLDER";
        //        viewModel.SearchEntity.FolderID = sysFolderId;
        //        viewModel.GetFolderItems();
        //        ModelState.Clear();
        //        return PartialView(BASE_PATH + "List.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return null;
        //    }
        //}

        public ActionResult Index(string eventAction = "", int folderId = 0, int orderId = 0, string rank="")
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.PageTitle = "Family Search";
                viewModel.TableName = "taxonomy_family_map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                if (orderId > 0)
                {
                    viewModel.SearchEntity.OrderID = orderId;
                    viewModel.SearchEntity.Rank = rank;
                    viewModel.Search();
                }

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as FamilyMapViewModel;
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<FamilyMapSearch>(appUserItemListViewModel.Entity.Properties);
                    viewModel.Search();
                }

                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(FamilyMapViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
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

        public PartialViewResult _List(int orderId=0, string rank="")
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.SearchEntity.OrderID = orderId;
                viewModel.SearchEntity.Rank = rank;
                viewModel.Search();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

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
        public PartialViewResult _ListDynamicFolderItems(int folderId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.RunSearch(folderId);
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListGenera(int familyId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.GetGenera(familyId);
                return PartialView(BASE_PATH + "_ListGenera.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSynonyms(int familyId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.GetSynonyms(familyId);
                return PartialView(BASE_PATH + "_ListSynonyms.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSubdivisions(int familyId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.GetSubdivisions(familyId);
                return PartialView(BASE_PATH + "_ListSubdivisions.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection formCollection)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            if (!String.IsNullOrEmpty(formCollection["FamilyName"]))
            {
                viewModel.SearchEntity.FamilyName = formCollection["FamilyName"];
            }

            if (!String.IsNullOrEmpty(formCollection["IsAcceptedName"]))
            {
                viewModel.SearchEntity.IsAcceptedName = formCollection["IsAcceptedName"];
            }

            viewModel.Search();
            return PartialView(BASE_PATH + "Modals/_SelectList.cshtml", viewModel);
        }

        private string GetEditPartialViewName(string rank)
        {
            switch (rank.ToUpper())
            {
                case "FAMILY":
                    return "_EditFamily.cshtml";
                case "SUBFAMILY":
                    return "_EditSubfamily.cshtml";
                case "TRIBE":
                    return "_EditTribe.cshtml";
                case "SUBTRIBE":
                    return "_EditSubtribe.cshtml";
                default:
                    return "_EditFamily.cshtml";
            }
        }

        [HttpPost]
        //public PartialViewResult AddInfrafamilialTaxa(FormCollection formCollection)
        //{
        //    InfrafamilyViewModel viewModel = new InfrafamilyViewModel();

        //    if (!String.IsNullOrEmpty(formCollection["FamilyID"]))
        //    {
        //        viewModel.FamilyID = Int32.Parse(formCollection["FamilyID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["NumSubfamilies"]))
        //    {
        //        viewModel.SubfamilyCount = Int32.Parse(formCollection["NumSubfamilies"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["NumTribes"]))
        //    {
        //        viewModel.TribeCount = Int32.Parse(formCollection["NumTribes"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["NumSubtribes"]))
        //    {
        //        viewModel.SubtribeCount = Int32.Parse(formCollection["NumSubtribes"]);
        //    }
        //    viewModel.Edit();
        //    return PartialView("~/Views/FamilyMap/Modals/_InfrafamilialTaxonEditGrid.cshtml", viewModel);
        //}

        public JsonResult GenerateInfrafamilialTaxa(FormCollection formCollection)
        {
            string[] keyTokens;
            int familyId = 0;
            string infraRank = String.Empty;
            string infraRankName = String.Empty;
            FamilyMapViewModel viewModel = new FamilyMapViewModel();
            //SubfamilyViewModel subfamilyViewModel = null;
            //TribeViewModel tribeViewModel = new TribeViewModel();
            //SubtribeViewModel subtribeViewModel = new SubtribeViewModel();

            foreach (var key in formCollection.AllKeys)
            {
                if (key == "FamilyID")
                {
                    familyId = Int32.Parse(formCollection[key]);
                }
                else
                {
                    keyTokens = key.Split('_');
                    infraRank = keyTokens[0];
                    infraRankName = formCollection[key];
                }

                //switch(infraRank)
                //{
                //    case "SUBFAMILY":
                //        subfamilyViewModel = new SubfamilyViewModel();
                //        subfamilyViewModel.Entity.FamilyID = familyId;
                //        subfamilyViewModel.Entity.SubfamilyName = infraRankName;
                //        subfamilyViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //        subfamilyViewModel.Insert();
                //        break;
                //    case "TRIBE":
                //        tribeViewModel.Entity.FamilyID = familyId;
                //        tribeViewModel.Entity.TribeName = infraRankName;
                //        tribeViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //        tribeViewModel.Insert();
                //        break;
                //    case "SUBTRIBE":
                //        subtribeViewModel.Entity.FamilyID = familyId;
                //        subtribeViewModel.Entity.SubtribeName = infraRankName;
                //        subtribeViewModel.Insert();
                //        break;
                //}
            }
            return null;
        }
       

        [HttpPost]
        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
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
                FamilyViewModel viewModel = new FamilyViewModel();
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