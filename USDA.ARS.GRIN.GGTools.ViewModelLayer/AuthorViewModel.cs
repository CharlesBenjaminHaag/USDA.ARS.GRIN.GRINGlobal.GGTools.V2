using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class AuthorViewModel : AuthorViewModelBase, IViewModel<AuthorViewModel>
    {
        public void Delete()
        {
            try
            {
                using (GRINGlobalDataManagerBase mgr = new GRINGlobalDataManagerBase())
                {
                    mgr.Delete(TableName, Entity.ID);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public Author Get(int entityId)
        {
            try
            {
                using (AuthorManager mgr = new AuthorManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
                    }
                    catch (Exception ex)
                    {
                        PublishException(ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity;
        }

        public void GetFolderItems(int sysFolderId)
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                try
                {
                    DataCollection = new Collection<Author>(mgr.GetFolderItems(sysFolderId));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void GetReferences()
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                DataCollectionReferences = new Collection<AuthorReference>(mgr.GetReferences(SearchEntity.ShortName));
            }
        }
        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                try
                {
                    RowsAffected = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;
        }

        public void Search()
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                try
                {
                    //if (!String.IsNullOrEmpty(SearchEntity.AuthorityText))
                    //{
                    //    DataCollection = new Collection<Author>(mgr.SearchTaxa(SearchEntity.TableName, SearchEntity.AuthorityText));
                    //}
                    //else 
                    //{
                        DataCollection = new Collection<Author>(mgr.Search(SearchEntity));
                    //}

                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }

                    RowsAffected = mgr.RowsAffected;

                    //String DEBUG = SerializeToXml<CitationSearch>(SearchEntity);
                    //CitationSearch DEBUG2 = Deserialize<CitationSearch>(DEBUG);

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        
        public void RunSearch(int sysFolderId)
        {
            SysFolderViewModel viewModel = new SysFolderViewModel();
            viewModel.GetProperties(sysFolderId);
            SearchEntity = Deserialize<AuthorSearch>(viewModel.Entity.Properties);
            Search();
        }
        //public void SaveSearch()
        //{
        //    AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
        //    appUserItemFolderViewModel.Entity.FolderName = SearchEntity.SearchTitle;
        //    appUserItemFolderViewModel.Entity.Description = SearchEntity.SearchDescription;
        //    appUserItemFolderViewModel.Entity.Category = "";
        //    appUserItemFolderViewModel.Entity.FolderType = "DYNAMIC";
        //    appUserItemFolderViewModel.Entity.DataType = TableName;
        //    appUserItemFolderViewModel.Entity.CreatedByCooperatorID = AuthenticatedUserCooperatorID;
        //    appUserItemFolderViewModel.Insert();

        //    if (appUserItemFolderViewModel.Entity.ID <= 0)
        //    {
        //        throw new IndexOutOfRangeException("Error adding new folder.");
        //    }

        //    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
        //    appUserItemListViewModel.Entity.AppUserItemFolderID = appUserItemFolderViewModel.Entity.ID;
        //    appUserItemListViewModel.Entity.CooperatorID = AuthenticatedUserCooperatorID;
        //    appUserItemListViewModel.Entity.TabName = "GGTools Taxon Editor";
        //    appUserItemListViewModel.Entity.ListName = appUserItemFolderViewModel.Entity.FolderName;
        //    appUserItemListViewModel.Entity.IDNumber = 0;
        //    appUserItemListViewModel.Entity.IDType = "FOLDER";
        //    appUserItemListViewModel.Entity.SortOrder = 0;
        //    appUserItemListViewModel.Entity.Title = appUserItemFolderViewModel.Entity.FolderName;
        //    appUserItemListViewModel.Entity.Description = "Added in GGTools Taxonomy Editor";
        //    appUserItemListViewModel.Entity.CreatedByCooperatorID = AuthenticatedUserCooperatorID;
        //    appUserItemListViewModel.Entity.Properties = SerializeToXml(SearchEntity);
        //    appUserItemListViewModel.Insert();
        //}
        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public int Update()
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
            return RowsAffected;
        }
        public int UpdateReferences(string originalValue, string newValue)
        {
            SearchEntity.ShortName = originalValue;
            using (AuthorManager mgr = new AuthorManager())
            {
                mgr.UpdateReferences(originalValue, newValue);
            }

            return 0;
        }
        public void SearchFolderItems()
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                try
                {
                    DataCollection = new Collection<Author>(mgr.SearchFolderItems(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public override bool Validate()
        {
            bool validated = true;

            if (String.IsNullOrEmpty(Entity.ShortName))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The author short name is required." });
            }
            else
            {
                if (String.IsNullOrEmpty(Entity.ShortName.Trim()))
                {
                    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The author short name is required." });
                }
                else
                {
                    // See if author exists.
                    AuthorViewModel validationViewModel = new AuthorViewModel();
                    validationViewModel.SearchEntity.ShortName = Entity.ShortName;
                    validationViewModel.SearchEntity.IsShortNameExactMatch = "Y";
                    validationViewModel.SearchEntity.ExcludeID = Entity.ID;
                    validationViewModel.Search();
                    if (validationViewModel.RowsAffected > 0)
                    {
                        ValidationMessages.Add(new Common.Library.ValidationMessage { Message = String.Format("The author with short name {0} already exists.", Entity.ShortName) });
                    }
                }
            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }

        AuthorViewModel IViewModel<AuthorViewModel>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<AuthorViewModel> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
