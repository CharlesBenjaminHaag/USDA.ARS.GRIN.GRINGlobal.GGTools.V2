using System.Web.Mvc;
using System;
using System.Security.Permissions;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;
using System.Collections.Generic;
using USDA.ARS.GRIN.Common.Library;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;
using DataTables;
using System.Collections.ObjectModel;
using System.Web.Configuration;
using System.Runtime.Remoting.Messaging;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    [ValidateInput(false)]
    public class SpeciesController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Species/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Handles all menu rendering.
        /// 1. By default, a menu with a "Home" link leading to the GGTools main page.
        /// </summary>
        /// <param name="eventAction"></param>
        /// <param name="eventValue"></param>
        /// <param name="sysTableName"></param>
        /// <param name="sysTableTitle"></param>
        /// <returns></returns>
        public override PartialViewResult PageMenu(string eventAction, string eventValue, string sysTableName = "", string sysTableTitle = "", int entityId = 0)
        {
            var queryParams = new Dictionary<string, string>();

            // Iterate through the query string collection
            foreach (string key in Request.QueryString)
            {
                if (key != null)
                {
                    queryParams[key] = Request.QueryString[key];
                }
            }

            // Pass the dictionary and specific parameters to the view
            ViewBag.QueryParams = queryParams;
            ViewBag.EntityID = Request.QueryString["entityId"];
            ViewBag.SynonymCode = Request.QueryString["synonymCode"];
            ViewBag.EventAction = eventAction;
            ViewBag.EventValue = eventValue;

            // Render menu based on action.
            switch(eventValue)
            {
                case "Index":
                    return PartialView("~/Views/Taxonomy" + eventAction + "/_DefaultMenu.cshtml");
                case "Add":
                    return PartialView("~/Views/Taxonomy/Species/Components/_AddMenu.cshtml");
                case "Edit":
                    return PartialView("~/Views/Taxonomy/Species/Components/_EditMenu.cshtml");
                default:
                    return PartialView("~/Views/Components/_DefaultMenu.cshtml");
            }
        }

        public PartialViewResult GetNameMatches(string genusName, string speciesName)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.SearchEntity.GenusName = genusName;
                viewModel.Search();

                foreach (var result in viewModel.DataCollection)
                {
                    result.MatchRankingLevenshtein = StringMatching.CalculateLevenshteinDistance(speciesName, result.SpeciesName);
                    result.MatchRankingDice = StringMatching.CalculateDiceCoefficient(speciesName, result.SpeciesName);
                    //result.MatchRankingHamming = StringMatching.CalculateHammingDistance(speciesName, result.SpeciesName);
                }
                return PartialView("~/Views/Taxonomy/Species/Components/_NameMatchingSelectList.cshtml",viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _List(int entityId = 0, int genusId = 0, string formatCode = "", string speciesAuthority = "")
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                viewModel.SearchEntity.GenusID = genusId;
                viewModel.SearchEntity.SpeciesAuthority = speciesAuthority;
                viewModel.Search();
                viewModel.TableName = "taxonomy_species";
                viewModel.Entity.GenusID = genusId;

                if (formatCode == "S")
                {
                    return PartialView("~/Views/Taxonomy/Species/Modals/_SelectListSimple.cshtml", viewModel);
                }
                else
                {
                    return PartialView(BASE_PATH + "_List.cshtml", viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListConspecific(int speciesId = 0)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                viewModel.SearchEntity.ID = speciesId;
                viewModel.GetConspecific();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSynonyms(int speciesId = 0)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                viewModel.SearchEntity.ID = speciesId;
                viewModel.GetSynonyms();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListFolderItems(int sysFolderId, string displayFormat = "")
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
     
            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.EventNote = "taxonomy_species";
                viewModel.SearchEntity.FolderID = sysFolderId;
                viewModel.GetFolderItems();

                return PartialView("~/Views/Taxonomy/Species/_ListFolder.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult _ListDynamicFolderItems(int folderId)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();

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
       
        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            
            viewModel.TableName = "taxonomy_species";
            viewModel.TableCode = "Species";
            viewModel.SearchEntity.SQLStatement = "SELECT * FROM vw_gringlobal_" + viewModel.TableName;

            SetPageTitle();
             
            string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
            if (Session[targetKey] != null)
            {
                viewModel = Session[targetKey] as SpeciesViewModel;
            }

            if (eventAction == "RUN_SEARCH")
            {
                AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                appUserItemListViewModel.Search();
                viewModel.SearchEntity = viewModel.Deserialize<SpeciesSearch>(appUserItemListViewModel.Entity.Properties);
                viewModel.Search();
            }

            return View(BASE_PATH + "Index.cshtml", viewModel);
        }

        public ActionResult Search(SpeciesViewModel viewModel)
        {
            try
            {
                // CHECK FOR RESET
                if (viewModel.EventAction == "RESET")
                {
                    Session.Remove(SessionKeyName);
                    return View(BASE_PATH + "Index.cshtml", viewModel);
                }
                else
                {
                    Session[SessionKeyName] = viewModel;
                    viewModel.Search();
                    ModelState.Clear();

                     

                    // Save search if attribs supplied.
                    if ((viewModel.EventAction == "Species") && (viewModel.EventValue == "SaveSearch"))
                    {
                        viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                        ////viewModel.SaveSearch();
                    }
                    viewModel.TableName = "taxonomy_species";
                    return View(BASE_PATH + "Index.cshtml", viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        /// <summary>
        /// Called when adding a synonym. Assumes a UI element that allows the user to 
        /// specify:
        /// 1) Synonym type
        /// 2) Rank (species or infraspecific)
        /// 3) A set of Boolean variables indicating which fields to copy from
        /// the "parent" species of which the synonym is being created.
        /// </summary>
        /// <param name="viewModel">An instance of the Species view model.</param>
        /// <returns>Sends the user to the Species Edit page, configured according to
        /// the synonym type and rank specified.</returns>
        [HttpPost]
        
        public ActionResult Add(SpeciesViewModel viewModel)
        {
            viewModel.TableName = "taxonomy_species";
            viewModel.EventAction = "ADD";
            
            viewModel.Entity.IsAcceptedName = "Y";
            viewModel.Entity.IsAccepted = true;
            viewModel.Entity.IsWebVisibleOption = true;

            // Obtain reference to parent species.
            if (viewModel.Entity.ParentID > 0)
            {
                SpeciesViewModel parentViewModel = new SpeciesViewModel();
                parentViewModel.SearchEntity.ID = viewModel.Entity.ParentID;
                parentViewModel.Search();

                // Add link to "parent" taxon.
                viewModel.ParentEntity = parentViewModel.Entity;
                viewModel.Entity.ParentID = parentViewModel.Entity.ID;
                viewModel.Entity.ParentName = parentViewModel.Entity.AssembledName;
                viewModel.Entity.Name = parentViewModel.Entity.Name;

                //if (viewModel.IsCopyGenusRequired == true)
                //{
                //    viewModel.Entity.GenusID = parentViewModel.Entity.GenusID;
                //    viewModel.Entity.GenusName = parentViewModel.Entity.GenusName;
                //}

                //if (viewModel.IsCopySpeciesRequired == true)
                //{
                //    viewModel.Entity.SpeciesName = parentViewModel.Entity.SpeciesName;
                //}
                
                viewModel.Entity.AssembledName = parentViewModel.Entity.AssembledName;
                viewModel.Entity.SubspeciesName = parentViewModel.Entity.SubspeciesName;
                viewModel.Entity.VarietyName = parentViewModel.Entity.VarietyName;
                viewModel.Entity.SubvarietyName = parentViewModel.Entity.SubvarietyName;
                //viewModel.Entity.Protologue = parentViewModel.Entity.Protologue;

                if (!String.IsNullOrEmpty(viewModel.Entity.SynonymCode))
                {
                    // TODO: Refactor; obtain actual syn code based on human-readable
                    //       string passed in querystring.
                    switch (viewModel.Entity.SynonymCode)
                    {
                        case "=":
                            viewModel.Entity.SynonymDescription = "Homotypic Synonym";
                            break;
                        case "A":
                            viewModel.Entity.SynonymDescription = "Autonym";
                            break;
                        case "B":
                            viewModel.Entity.SynonymDescription = "Basionym";
                            break;
                        case "S":
                            viewModel.Entity.SynonymDescription = "Heterotypic Synonym";
                            break;
                        case "I":
                            viewModel.Entity.SynonymDescription = "Invalid Synonym";
                            break;
                    }
                }
            }

            return View(BASE_PATH + "Edit.cshtml", viewModel);
        }

        public ActionResult Add(int genusId = 0, int entityId = 0, string rank = "", string synonymCode = "", string copyProtologue="false", string copyAuthority="false", string copyNote="false")
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.EventAction = "ADD";
                viewModel.EventValue = rank;
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.Entity.IsAccepted = true;
                viewModel.Entity.IsWebVisibleOption = true;
                viewModel.Entity.Rank = String.IsNullOrEmpty(rank) ? "species" : rank;
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.CreatedByCooperatorName = AuthenticatedUser.FullName;
                viewModel.Entity.CreatedDate = System.DateTime.Now;

                 

                if (genusId > 0)
                {
                    GenusViewModel topRankGenusViewModel = new GenusViewModel();
                    topRankGenusViewModel.SearchEntity.ID = genusId;
                    topRankGenusViewModel.Search();
                    //viewModel.TopRankGenusEntity = topRankGenusViewModel.Entity;
                    viewModel.Entity.GenusID = topRankGenusViewModel.Entity.ID;
                    viewModel.Entity.GenusName = topRankGenusViewModel.Entity.Name;
                }

                // If an entity ID is passed in, this represents a parent species.
                if (entityId > 0)
                {
                    SpeciesViewModel parentViewModel = new SpeciesViewModel();
                    parentViewModel.SearchEntity.ID = entityId;
                    parentViewModel.Search();

                    // Add link to "parent" taxon.
                    viewModel.ParentEntity = parentViewModel.Entity;
                    viewModel.Entity.AcceptedID = parentViewModel.Entity.ID;
                    viewModel.Entity.AcceptedName = parentViewModel.Entity.AssembledName;
                    viewModel.Entity.SynonymCode = synonymCode;
                    viewModel.Entity.IsAccepted = false;
                    viewModel.Entity.IsAcceptedName = "N";

                    viewModel.Entity.ParentID = parentViewModel.Entity.ID;
                    viewModel.Entity.ParentName = parentViewModel.Entity.AssembledName;
                    viewModel.Entity.Name = parentViewModel.Entity.Name;
                    viewModel.Entity.SpeciesName = parentViewModel.Entity.SpeciesName;
                    viewModel.Entity.AssembledName = parentViewModel.Entity.AssembledName;
                    viewModel.Entity.SubspeciesName = parentViewModel.Entity.SubspeciesName;
                    viewModel.Entity.VarietyName = parentViewModel.Entity.VarietyName;
                    viewModel.Entity.SubvarietyName = parentViewModel.Entity.SubvarietyName;
                    viewModel.Entity.GenusID = parentViewModel.Entity.GenusID;
                    viewModel.Entity.GenusName = parentViewModel.Entity.GenusName;

                    // Determine which parent species attributes to copy based on parameters.
                    if (copyProtologue == "true")
                    {
                        viewModel.Entity.Protologue = parentViewModel.Entity.Protologue;
                    }

                    viewModel.Entity.SpeciesAuthority = parentViewModel.Entity.SpeciesAuthority;
                    if (copyAuthority == "true")
                    {
                        switch (rank.ToLower())
                        {
                            case "subspecies":
                                viewModel.Entity.SubspeciesAuthority = viewModel.ParentEntity.SpeciesAuthority;
                                break;
                            case "variety":
                                viewModel.Entity.VarietyAuthority = viewModel.ParentEntity.SpeciesAuthority;
                                break;
                            case "subvariety":
                                viewModel.Entity.SubvarietyAuthority = viewModel.ParentEntity.SpeciesAuthority;
                                break;
                            case "form":
                                viewModel.Entity.FormaAuthority = viewModel.ParentEntity.SpeciesAuthority;
                                break;
                        }

                        viewModel.Entity.SubspeciesAuthority = parentViewModel.Entity.SpeciesAuthority;
                    }

                    if (copyNote == "true")
                    {
                        viewModel.Entity.Note = parentViewModel.Entity.Note;
                    }

                    // Store parent entity in session.
                    Session["PARENT-SPECIES"] = viewModel.ParentEntity;
                }

                if (!String.IsNullOrEmpty(synonymCode))
                {
                    // TODO: Refactor; obtain actual syn code based on human-readable
                    //       string passed in querystring.
                    switch (synonymCode)
                    {
                        case "=":
                            viewModel.Entity.SynonymCode = "=";
                            viewModel.Entity.SynonymDescription = "Homotypic Synonym";
                            break;
                        case "A":
                            viewModel.Entity.SynonymCode = "A";
                            viewModel.Entity.SynonymDescription = "Autonym";
                            break;
                        case "B":
                            viewModel.Entity.SynonymCode = "B";
                            viewModel.Entity.SynonymDescription = "Basionym";
                            break;
                        case "S":
                            viewModel.Entity.SynonymCode = "S";
                            viewModel.Entity.SynonymDescription = "Heterotypic Synonym";
                            break;
                        case "I":
                            viewModel.Entity.SynonymDescription = "Invalid Synonym";
                            viewModel.Entity.SynonymCode = "I";
                            break;
                    }

                  

                    ViewBag.PageTitle += viewModel.Entity.SynonymDescription;
                }
                else
                {
                    //viewModel.PageTitle = String.Format("Add {0}", viewModel.ToTitleCase(rank));
                }

                viewModel.SubspeciesUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "subspecies" });
                viewModel.VarietyUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "variety" });
                viewModel.SubvarietyUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "subvariety" });
                viewModel.FormUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "form" });

                if (!String.IsNullOrEmpty(Request.QueryString["synonymCode"]))
                {
                    viewModel.SubspeciesUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.VarietyUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.SubvarietyUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.FormUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.Entity.SynonymCode = Request.QueryString["synonymCode"];
                }

                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult AddSynonym(SynonymOptionsViewModel viewModel)
        {
            SpeciesViewModel acceptedNameViewModel = new SpeciesViewModel();
            SpeciesViewModel synonymViewModel = new SpeciesViewModel();
            acceptedNameViewModel.Get(viewModel.ParentSpeciesID);

            synonymViewModel.Entity.SpeciesName = acceptedNameViewModel.Entity.SpeciesName;
            synonymViewModel.Entity.Name = acceptedNameViewModel.Entity.Name;
            synonymViewModel.Entity.AssembledName = acceptedNameViewModel.Entity.AssembledName;
            synonymViewModel.Entity.AcceptedID = acceptedNameViewModel.Entity.ID;
            synonymViewModel.Entity.SynonymCode = viewModel.SelectedSynonymCode;
            synonymViewModel.Entity.GenusID = acceptedNameViewModel.Entity.GenusID;
            synonymViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            synonymViewModel.Entity.Rank = viewModel.SelectedRank;
            synonymViewModel.Entity.SubspeciesName = acceptedNameViewModel.Entity.SubspeciesName;
            synonymViewModel.Entity.VarietyName = acceptedNameViewModel.Entity.VarietyName;
            synonymViewModel.Entity.SubvarietyName = acceptedNameViewModel.Entity.SubvarietyName;
            synonymViewModel.Entity.TribeName = acceptedNameViewModel.Entity.TribeName;
            synonymViewModel.Entity.SubtribeName = acceptedNameViewModel.Entity.SubtribeName;
            synonymViewModel.Entity.FormaName = acceptedNameViewModel.Entity.FormaName;

            if (viewModel.IsCopyAuthorityRequired)
            {
                synonymViewModel.Entity.NameAuthority = acceptedNameViewModel.Entity.NameAuthority;
                synonymViewModel.Entity.SpeciesAuthority = acceptedNameViewModel.Entity.SpeciesAuthority;
                synonymViewModel.Entity.SubspeciesAuthority = acceptedNameViewModel.Entity.SubspeciesAuthority;
                synonymViewModel.Entity.VarietyAuthority = acceptedNameViewModel.Entity.VarietyAuthority;
                synonymViewModel.Entity.SubvarietyAuthority = acceptedNameViewModel.Entity.SubvarietyAuthority;
                synonymViewModel.Entity.TribeAuthority = acceptedNameViewModel.Entity.TribeAuthority;
                synonymViewModel.Entity.SubTribeAuthority = acceptedNameViewModel.Entity.SubTribeAuthority;
                synonymViewModel.Entity.FormaAuthority = acceptedNameViewModel.Entity.FormaAuthority;
            }

            if (viewModel.IsCopyProtologueRequired)
            {
                synonymViewModel.Entity.Protologue = acceptedNameViewModel.Entity.Protologue;
            }

            if (viewModel.IsCopyNoteRequired)
            {
                synonymViewModel.Entity.Note = acceptedNameViewModel.Entity.Note;
            }

            synonymViewModel.Insert();
            synonymViewModel.Get(synonymViewModel.Entity.ID);

            ViewBag.PageTitle = "Add " + viewModel.SelectedSynonymName;

            return RedirectToAction("Edit", "Species", new { @entityId = synonymViewModel.Entity.ID });
        }

        public ActionResult AddInfraspecific(InfraspecificOptionsViewModel viewModel)
        {
            SpeciesViewModel parentSpeciesViewModel = new SpeciesViewModel();
            parentSpeciesViewModel.Get(viewModel.ParentSpeciesID);

            SpeciesViewModel speciesViewModel = new SpeciesViewModel();
            speciesViewModel.TableName = "taxonomy_species";
            speciesViewModel.EventAction = "ADD";
            speciesViewModel.Entity.ParentID = viewModel.ParentSpeciesID;
            speciesViewModel.EventValue = viewModel.SelectedRank;
            speciesViewModel.Entity.Name = parentSpeciesViewModel.Entity.Name;
            speciesViewModel.Entity.SpeciesName = parentSpeciesViewModel.Entity.SpeciesName;
            speciesViewModel.Entity.AssembledName = parentSpeciesViewModel.Entity.AssembledName;
            speciesViewModel.Entity.GenusID = parentSpeciesViewModel.Entity.GenusID;
            speciesViewModel.Entity.GenusName = parentSpeciesViewModel.Entity.GenusName;
            speciesViewModel.Entity.IsAcceptedName = "Y";
            speciesViewModel.Entity.IsAccepted = true;
            speciesViewModel.Entity.IsWebVisibleOption = true;
            speciesViewModel.Entity.Rank = viewModel.SelectedRank;
            speciesViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            speciesViewModel.Entity.CreatedByCooperatorName = AuthenticatedUser.FullName;
            speciesViewModel.Entity.CreatedDate = System.DateTime.Now;

            if (viewModel.IsCopyAuthorityRequired)
            {
                speciesViewModel.Entity.SpeciesAuthority = parentSpeciesViewModel.Entity.SpeciesAuthority;
                speciesViewModel.Entity.NameAuthority = parentSpeciesViewModel.Entity.NameAuthority;
            }

            if (viewModel.IsCopyNoteRequired)
            {
                speciesViewModel.Entity.Note = parentSpeciesViewModel.Entity.Note;
            }

            if (viewModel.IsCopyProtologueRequired)
            {
                speciesViewModel.Entity.Protologue = parentSpeciesViewModel.Entity.Protologue;
            }

            if (viewModel.IsGenerateAutonymRequired)
            {
                speciesViewModel.EventInfo = "AUTONYM";
            }

            ViewBag.PageTitle = "Add " + speciesViewModel.ToTitleCase(viewModel.SelectedRank);

            return View("~/Views/Taxonomy/Species/Edit.cshtml", speciesViewModel);
        }

        public ActionResult AddAutonym(int entityId)
        {
            SpeciesViewModel speciesViewModel = new SpeciesViewModel();
            speciesViewModel.Get(entityId);

            SpeciesViewModel autonymViewModel = new SpeciesViewModel();
            autonymViewModel.Entity.GenusID = speciesViewModel.Entity.GenusID;
            autonymViewModel.Entity.SpeciesName = speciesViewModel.Entity.SpeciesName;
            autonymViewModel.Entity.Rank = speciesViewModel.Entity.Rank;
            autonymViewModel.Entity.SynonymCode = "A";

            switch (speciesViewModel.Entity.Rank.ToLower())
            {
                case "subspecies":
                    autonymViewModel.Entity.SubspeciesName = speciesViewModel.Entity.SpeciesName;
                    break;
                case "variety":
                    autonymViewModel.Entity.VarietyName = speciesViewModel.Entity.SpeciesName;
                    break;
                case "subvariety":
                    autonymViewModel.Entity.SubvarietyName = speciesViewModel.Entity.SpeciesName;
                    break;
                case "form":
                    autonymViewModel.Entity.FormaName = speciesViewModel.Entity.SpeciesName;
                    break;
            }
            autonymViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            autonymViewModel.Insert();
            return RedirectToAction("Edit", "Species", new { @entityId = entityId });
        }

        public PartialViewResult _Add(int genusId)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.Entity.Rank = "SPECIES";
                viewModel.Entity.IsWebVisible = "Y";
                viewModel.Entity.IsWebVisibleOption = viewModel.ToBool(viewModel.Entity.IsWebVisible);
                viewModel.Entity.IsSpecificHybrid = "N";
                viewModel.Entity.IsSpecificHybridOption = viewModel.ToBool(viewModel.Entity.IsSpecificHybrid);
                
                if (genusId > 0)
                {
                    GenusViewModel topRankGenusViewModel = new GenusViewModel();
                    topRankGenusViewModel.SearchEntity.ID = genusId;
                    topRankGenusViewModel.Search();
                    viewModel.Entity.GenusID = topRankGenusViewModel.Entity.ID;
                    viewModel.Entity.GenusName = topRankGenusViewModel.Entity.Name;
                }
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _Edit(int entityId)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.TableCode = "Species";
                viewModel.Get(entityId);
                viewModel.EventValue = viewModel.Entity.Rank.ToLower();
                viewModel.ID = entityId;
                viewModel.SpeciesID = entityId;
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public JsonResult _Save(SpeciesViewModel viewModel)
        {
            int speciesId = 0;
            try
            {
                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Insert();
                    speciesId = viewModel.Entity.ID;
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                    speciesId = viewModel.Entity.ID;
                }
                viewModel.Get(speciesId);
                viewModel.Entity.SpeciesID = speciesId;
                return Json(new { species = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { speciesId = -1 }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Retrieves a single record and returns a batch-edit-formatted partial.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult _Get(int speciesId)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.TableCode = "Species";
                viewModel.Get(speciesId);
                viewModel.EventValue = viewModel.Entity.Rank.ToLower();
                viewModel.ID = speciesId;
                viewModel.SpeciesID = speciesId;
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        /// <summary>
        /// Submits changes to a species record.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="parentId"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult _Post(SpeciesViewModel viewModel)
        {
            SpeciesBatchEditViewModel batchEditViewModel = new SpeciesBatchEditViewModel();

            try
            {
                batchEditViewModel.Get(viewModel.Entity.ID);
                batchEditViewModel.Entity.GenusID = viewModel.Entity.GenusID;
                batchEditViewModel.Entity.SpeciesName = viewModel.Entity.SpeciesName;
                batchEditViewModel.Entity.SpeciesAuthority = viewModel.Entity.SpeciesAuthority;
                batchEditViewModel.Entity.Protologue = viewModel.Entity.Protologue;
                batchEditViewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                batchEditViewModel.Update();
                batchEditViewModel.Get(viewModel.Entity.ID);
                //TODO
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", batchEditViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Edit(int entityId, int parentId = 0, string rank = "", int appUserItemFolderId = 0)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.TableCode = "Species";
                viewModel.EventAction = "EDIT";
                viewModel.Get(entityId);
                viewModel.Entity.Protologue = System.Web.HttpUtility.HtmlDecode(viewModel.Entity.Protologue);
                viewModel.EventValue = viewModel.Entity.Rank.ToUpper();
                viewModel.ID = entityId;

                SetPageTitle();
                ViewBag.PageTitle += " [" + viewModel.ID + "]: " + viewModel.Entity.AssembledName;

                ViewData["DEBUG"] = "DEBUG EDIT SPEC " + viewModel.ID;

                // If there is a rank specified, this is a change-rank operation; reload
                // page with newly-set rank to enable necessary fields.
                if (!String.IsNullOrEmpty(rank))
                {
                    viewModel.Entity.Rank = rank;
                }

                // If there is a parent ID specified, retrieve its pertinent data.
                if(parentId > 0)
                {
                    SpeciesViewModel parentViewModel = new SpeciesViewModel();
                    parentViewModel.Get(parentId);
                    viewModel.Entity.ParentID = parentViewModel.Entity.ID;
                    viewModel.Entity.ParentName = parentViewModel.Entity.Name;
                }

                viewModel.SubspeciesUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "subspecies" });
                viewModel.VarietyUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "variety" });
                viewModel.SubvarietyUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "subvariety" });
                viewModel.FormUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "form" });

                if (!String.IsNullOrEmpty(Request.QueryString["synonymCode"]))
                {
                    viewModel.SubspeciesUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.VarietyUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.SubvarietyUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.FormUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                }

                viewModel.AppUserItemFolderID = appUserItemFolderId;

                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(SpeciesViewModel viewModel)
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
                    if (viewModel.EventAction == "VERIFY")
                    {
                        if (viewModel.EventValue == "Y")
                        {
                            viewModel.Entity.VerifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                            viewModel.Entity.NameVerifiedDate = DateTime.Now;
                        }
                        else
                        {
                            viewModel.Entity.VerifiedByCooperatorID = 0;
                            viewModel.Entity.NameVerifiedDate = DateTime.MinValue;
                        }
                    }

                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }

                // If the action indicates "ADD" and a synonym code has been supplied, add
                // a map record to link the parent species, and the newly-created one.
                //if (viewModel.EventAction.ToUpper() == "ADD")
                //{
                //    if (!String.IsNullOrEmpty(viewModel.Entity.SynonymCode))
                //    {
                //        SynonymMapViewModel synonymMapViewModel = new SynonymMapViewModel();
                //        synonymMapViewModel.Entity.SpeciesAID = viewModel.Entity.ID;
                //        synonymMapViewModel.Entity.SynonymCode = viewModel.Entity.SynonymCode;
                //        synonymMapViewModel.Entity.SpeciesBID = viewModel.Entity.ParentID;
                //        synonymMapViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //        synonymMapViewModel.Insert();
                //    }
                //}

                // If parent ID is present, display its data in a navigable URL.
                if (viewModel.Entity.ParentID > 0)
                { 
                
                }

                // If the species is being saved with a non-accepted name, add a
                // synonym map record. The stored proc. will ensure that the synonym
                // does not already exist.
                if ((viewModel.Entity.ID != viewModel.Entity.AcceptedID) && (viewModel.Entity.AcceptedID > 0))
                {
                    SynonymMapViewModel synonymMapViewModel = new SynonymMapViewModel();
                    synonymMapViewModel.Entity.SpeciesAID = viewModel.Entity.ID;
                    synonymMapViewModel.Entity.SynonymCode = viewModel.Entity.SynonymCode;
                    synonymMapViewModel.Entity.SpeciesBID = viewModel.Entity.AcceptedID;
                    synonymMapViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    synonymMapViewModel.Insert();
                }

                // If the species being added is an infraspecific name, determine whether or not an autonym is needed.
                if (viewModel.Entity.Rank.ToUpper() != "SPECIES")
                {
                    viewModel.GetAutonym(viewModel.Entity.GenusName, viewModel.Entity.SpeciesName, viewModel.Entity.Rank);
                    if (viewModel.AutonymEntity.ID == 0)
                    {
                        SpeciesViewModel autonymViewModel = new SpeciesViewModel();
                        autonymViewModel.Entity.GenusID = viewModel.Entity.GenusID;
                        autonymViewModel.Entity.SpeciesName = viewModel.Entity.SpeciesName;
                        autonymViewModel.Entity.Rank = viewModel.Entity.Rank;

                        switch (viewModel.Entity.Rank)
                        {
                            case "subspecies":
                                autonymViewModel.Entity.SubspeciesName = autonymViewModel.Entity.SpeciesName;
                                break;
                            case "variety":
                                autonymViewModel.Entity.VarietyName = autonymViewModel.Entity.SpeciesName;
                                break;
                            case "subvariety":
                                autonymViewModel.Entity.SubvarietyName = autonymViewModel.Entity.SpeciesName;
                                break;
                            case "form":
                                autonymViewModel.Entity.FormaName = autonymViewModel.Entity.SpeciesName;
                                break;
                        }
                        autonymViewModel.Entity.SynonymCode = "A";
                        autonymViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                        autonymViewModel.Insert();
                    }
                }

                return RedirectToAction("Edit", "Species", new { entityId = viewModel.Entity.ID, parentId = viewModel.Entity.ParentID });
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

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
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

        public PartialViewResult SetVerificationStatus(int speciesId, string statusCode)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.Get(speciesId);

                if (statusCode == "Y")
                {
                    viewModel.Entity.VerifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Entity.NameVerifiedDate = System.DateTime.Now;
                }
                else
                {
                    viewModel.Entity.VerifiedByCooperatorID = 0;
                    viewModel.Entity.NameVerifiedDate = DateTime.MinValue;
                }

                viewModel.Update();
                viewModel.Get(speciesId);
                return PartialView("~/Views/Taxonomy/Species/_RevisionHistory.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #region Batch Edit
        
        [HttpPost]
        public ActionResult GetBatchEditor(SpeciesViewModel viewModel)
        {
            try
            {
                Session["SPECIES_ID_LIST"] = viewModel.ItemIDList;
                return View("~/Views/Taxonomy/Species/EditBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult AddBatch()
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            return View("~/Views/Taxonomy/Species/AddBatch.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult SaveBatch(SpeciesDTO speciesDTO) 
        {
            List<Species> speciesList = new List<Species>();
            SpeciesViewModel viewModel = new SpeciesViewModel();

            try
            {
                viewModel.Entity.Rank = "SPECIES";
                viewModel.Entity.GenusID = speciesDTO.GenusID;
                viewModel.Entity.GenusName = speciesDTO.GenusName;
                viewModel.Entity.SpeciesName = speciesDTO.SpeciesName;
                viewModel.Entity.SpeciesAuthority = speciesDTO.SpeciesAuthority;
                viewModel.Entity.Protologue = speciesDTO.Protologue;
                viewModel.Entity.ProtologueVirtualPath = speciesDTO.ProtologueVirtualPath;
                viewModel.Entity.Note = speciesDTO.Note;
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                //if (!viewModel.Validate())
                //{
                //    //REFACTOR
                //    List<string> messages = new List<string>();
                //    foreach(var message in viewModel.ValidationMessages)
                //    { 
                //        messages.Add(message.Message);
                //    }

                //    return Json(new { success = false, messages });
                //}

                viewModel.Get(viewModel.Insert());

                if (Session["TAXONOMY_SPECIES_BATCH"] != null)
                {
                    speciesList = Session["TAXONOMY_SPECIES_BATCH"] as List<Species>;
                }
                else
                {
                    speciesList = new List<Species>();
                }    
                speciesList.Add(viewModel.Entity);
                Session["TAXONOMY_SPECIES_BATCH"] = speciesList;

                return Json(new { success = true, viewModel.Entity });
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    success = false,
                    errorCode = 500,
                    errorMessage = ex.Message,
                    validationErrors = viewModel.ValidationMessages,
                    additionalInfo = "Additional error details if any",
                    timestamp = DateTime.Now
                };
                return Json(new { success = false, errorResponse });
            }
            
        }

        public PartialViewResult _ListBatch()
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                if (Session["TAXONOMY_SPECIES_BATCH"] != null)
                {
                    viewModel.DataCollectionImport = new Collection<Species>(Session["TAXONOMY_SPECIES_BATCH"] as List<Species>);
                }
                return PartialView("~/Views/Taxonomy/Species/_ListBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
            
        }

        [ValidateInput(false)]
        public JsonResult EditBatch()
        {
            string idList = String.Empty;
            var request = System.Web.HttpContext.Current.Request;

            if (Session["SPECIES_ID_LIST"] != null)
            {
                idList = Session["SPECIES_ID_LIST"].ToString();
            }

            string[] idArray = idList.Split(',');

            try
            {
                using (SpeciesManager mgr = new SpeciesManager())
                {
                    using (var db = new Database("sqlserver", mgr.ConnectionString))
                    {
                        var editor = new Editor(db, "taxonomy_species", "taxonomy_species.taxonomy_species_id").Where(q =>
                        {
                            q.Where(r =>
                            {
                                foreach (var i in idArray)
                                {
                                    r.OrWhere("taxonomy_species.taxonomy_species_id", i);
                                }
                            });
                        })
                        .Model<SpeciesTable>("taxonomy_species")
                        .Model<GenusTable>("taxonomy_genus")
                        .LeftJoin("taxonomy_genus", "taxonomy_genus.taxonomy_genus_id", "=", "taxonomy_species.taxonomy_genus_id");

                        editor.Field(new Field("taxonomy_species.taxonomy_species_id")
                            .Validator(Validation.NotEmpty())
                        );
                        editor.Field(new Field("taxonomy_genus.genus_name"));
                        editor.Field(new Field("taxonomy_species.species_name"));
                        editor.Field(new Field("taxonomy_species.protologue"));
                        editor.Field(new Field("taxonomy_species.protologue_virtual_path"));
                        editor.Field(new Field("taxonomy_species.name_authority"));
                        editor.Field(new Field("taxonomy_species.note"));
                        editor.Field(new Field("taxonomy_species.modified_date")
                            .Set(Field.SetType.Edit));
                        editor.PreEdit += (sender, e) => editor.Field("taxonomy_species.modified_date").SetValue(DateTime.Now);
                        editor.Process(request);

                        var response = editor.Data();

                        JsonResult jsonResult = new JsonResult();
                        jsonResult = Json(response);
                        jsonResult.MaxJsonLength = 2147483644;
                        jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                        return jsonResult;
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Modals

        public PartialViewResult RenderLookupModal()
        {
            try 
            { 
                SpeciesViewModel viewModel = new SpeciesViewModel();
                return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderParentLookupModal()
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            return PartialView(BASE_PATH + "/Modals/_LookupParent.cshtml", viewModel);
        }

        public PartialViewResult RenderInfraspecificAutonymWidget(string genusName, string speciesName, string rank)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.GetAutonym(genusName, speciesName, rank);
                return PartialView("~/Views/Taxonomy/Shared/_InfraspecificAutonymWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderEditSynonymOptionsModal()
        {
            try
            {
                SynonymOptionsViewModel viewModel = new SynonymOptionsViewModel();
                return PartialView("~/Views/Taxonomy/Species/Modals/_EditSynonymOptions.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderInfraspecificOptionsModal()
        {
            try
            {
                InfraspecificOptionsViewModel viewModel = new InfraspecificOptionsViewModel();
                return PartialView("~/Views/Taxonomy/Species/Modals/_EditInfraspecificOptions.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }


        [HttpPost]
        public PartialViewResult Lookup(SpeciesViewModel viewModel)
        {
            string partialViewName = "~/Views/Taxonomy/Species/Modals/_SelectList.cshtml";

            try
            {
                switch (viewModel.EventAction)
                {
                    case "species-a":
                        partialViewName = "~/Views/Taxonomy/SpeciesSynonymMap/_SelectListSpeciesA.cshtml";
                        break;
                    case "species-b":
                        partialViewName = "~/Views/Taxonomy/SpeciesSynonymMap/_SelectListSpeciesB.cshtml";
                        break;
                }
                viewModel.Search();
                return PartialView(partialViewName, viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult LookupParent(SpeciesViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Species/Modals/_SelectListParent.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult LookupProtologues(FormCollection formCollection)
        {
            string partialViewName = BASE_PATH + "/Modals/_SelectListProtologue.cshtml";
            SpeciesViewModel viewModel = new SpeciesViewModel();

            if (!String.IsNullOrEmpty(formCollection["Protologue"]))
            {
                viewModel.SearchEntity.Protologue = formCollection["Protologue"];
            }

            viewModel.SearchProtologues(viewModel.SearchEntity.Protologue);
            return PartialView(partialViewName, viewModel);
        }

        [HttpPost]
        public PartialViewResult LookupProtologuePaths(FormCollection formCollection)
        {
            string partialViewName = BASE_PATH + "/Modals/_SelectListProtologueVirtualPath.cshtml";
            SpeciesViewModel viewModel = new SpeciesViewModel();

            if (!String.IsNullOrEmpty(formCollection["ProtologueVirtualPath"]))
            {
                viewModel.SearchEntity.ProtologueVirtualPath = formCollection["ProtologueVirtualPath"];
            }

            viewModel.SearchProtologues(viewModel.SearchEntity.Protologue);
            return PartialView(partialViewName, viewModel);
        }

        #endregion

        #region Components

        public PartialViewResult Component_Widget(int speciesId)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            viewModel.Get(speciesId);

            try
            {
                if (speciesId > 0)
                {
                    viewModel.Get(speciesId);
                    return PartialView("~/Views/Taxonomy/Species/Components/_Widget.cshtml", viewModel);
                }
                else
                {
                    return null;
                }
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