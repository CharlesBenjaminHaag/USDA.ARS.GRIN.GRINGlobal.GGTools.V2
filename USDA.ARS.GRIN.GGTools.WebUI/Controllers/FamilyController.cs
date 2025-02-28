using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;
using DataTables;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class FamilyController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index(string sysTableName = "", string sysTableTitle = "")
        {
            FamilyViewModel viewModel = new FamilyViewModel();
            viewModel.TableName = sysTableName;
            viewModel.TableTitle = sysTableTitle;
            return View("~/Views/Taxonomy/Family/Index.cshtml", viewModel);
        }
        
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add(int orderId = 0) 
        {
            FamilyViewModel viewModel = new FamilyViewModel();
            try 
            {
                viewModel.Entity.ClassificationID = orderId;
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.Entity.IsWebVisible = "Y";
                viewModel.IsWebVisibleSelector = true;
                return View("~/Views/Taxonomy/Family/Edit.cshtml", viewModel);
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
                FamilyViewModel viewModel = new FamilyViewModel();
                viewModel.TableName = "taxonomy_family";
                viewModel.TableCode = "Family";
                viewModel.Get(entityId);
                return View("~/Views/Taxonomy/Family/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
      
        [HttpPost]
        public ActionResult Edit(FamilyViewModel viewModel)
        {
            try
            {
                if (viewModel.IsWebVisibleSelector == true)
                {
                    viewModel.Entity.IsWebVisible = "Y";
                }
                else
                {
                    viewModel.Entity.IsWebVisible = "N";
                }

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
                return RedirectToAction("Edit", "Family", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
      
        [HttpPost]
        public ActionResult Search(FamilyViewModel viewModel)
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
                    ////viewModel.SaveSearch();
                }

                return View("~/Views/Taxonomy/Family/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _List(int orderId = 0, string rank="")
        {
            FamilyViewModel viewModel = new FamilyViewModel();

            try
            {
                //viewModel.SearchEntity.OrderID = orderId;
                //viewModel.SearchEntity.Rank = rank;
                //viewModel.Search();
                return PartialView("_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSynonyms(int familyId)
        {
            FamilyViewModel viewModel = new FamilyViewModel();

            try
            {
                viewModel.GetSynonyms(familyId);
                return PartialView("~/Views/Taxonomy/Family/_ListSynonyms.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSubdivisions(int familyId)
        {
            FamilyViewModel viewModel = new FamilyViewModel();
            viewModel.Get(familyId);

            try
            {
                viewModel.GetSubdivisions(viewModel.Entity.FamilyName);
                return PartialView("~/Views/Taxonomy/Family/_ListSubdivisions.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            FamilyViewModel viewModel = new FamilyViewModel();
            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.TableName = "taxonomy_family";
                viewModel.GetFolderItems(sysFolderId);
                return PartialView("~/Views/Taxonomy/Family/_ListFolder.cshtml", viewModel);
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
            FamilyViewModel viewModel = new FamilyViewModel();

            if (!String.IsNullOrEmpty(formCollection["FamilyName"]))
            {
                viewModel.SearchEntity.FamilyName = formCollection["FamilyName"];
            }

            if (!String.IsNullOrEmpty(formCollection["IsAcceptedName"]))
            {
                viewModel.SearchEntity.IsAcceptedName = formCollection["IsAcceptedName"];
            }

            viewModel.Search();
            return PartialView("~/Views/Taxonomy/Family/Modals/_SelectList.cshtml", viewModel);
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

        #region Batch Edit

        [HttpPost]
        public ActionResult GetBatchEditor(FamilyViewModel viewModel)
        {
            try
            {
                Session["FAMILY_ID_LIST"] = viewModel.ItemIDList;
                return View("~/Views/Taxonomy/Family/EditBatch.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [ValidateInput(false)]
        public JsonResult EditBatch()
        {
            string idList = String.Empty;
            var request = System.Web.HttpContext.Current.Request;

            if (Session["FAMILY_ID_LIST"] != null)
            {
                idList = Session["FAMILY_ID_LIST"].ToString();
            }

            string[] idArray = idList.Split(',');

            try
            {
                using (FamilyManager mgr = new FamilyManager())
                {
                    using (var db = new Database("sqlserver", mgr.ConnectionString))
                    {
                        var editor = new Editor(db, "taxonomy_family", "taxonomy_family.taxonomy_family_id").Where(q =>
                        {
                            q.Where(r =>
                            {
                                foreach (var i in idArray)
                                {
                                    r.OrWhere("taxonomy_family.taxonomy_family_id", i);
                                }
                            });
                        })
                        .Model<FamilyTable>("taxonomy_family");

                        editor.Field(new Field("taxonomy_family.taxonomy_family_id")
                            .Validator(Validation.NotEmpty())
                        );
                        editor.Field(new Field("taxonomy_family.family_name"));
                        editor.Field(new Field("taxonomy_family.subfamily_name"));
                        editor.Field(new Field("taxonomy_family.tribe_name"));
                        editor.Field(new Field("taxonomy_family.subtribe_name"));
                        editor.Field(new Field("taxonomy_family.family_authority"));
                        editor.Field(new Field("taxonomy_family.modified_date")
                            .Set(Field.SetType.Edit));
                        editor.PreEdit += (sender, e) => editor.Field("taxonomy_family.modified_date").SetValue(DateTime.Now);
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

        #region Components

        public PartialViewResult RenderLookupModal()
        {
            FamilyViewModel viewModel = new FamilyViewModel();
            return PartialView("~/Views/Taxonomy/Family/Modals/_Lookup.cshtml", viewModel);
        }

        public PartialViewResult Component_PageMenu()
        {
            return PartialView("~/Views/Taxonomy/Family/Components/_EditMenu.cshtml");
        }

        #endregion
    }
}