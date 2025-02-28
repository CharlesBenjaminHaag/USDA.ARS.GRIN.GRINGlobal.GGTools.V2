using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CommonNameViewModel : CommonNameViewModelBase, IViewModel<CommonName>
    {
        public void Delete()
        {
            try
            {
                using (SpeciesManager mgr = new SpeciesManager())
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

        public CommonName Get(int entityId)
        {
            try
            {
                using (CommonNameManager mgr = new CommonNameManager())
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
        public void GetFolderItems()
        {
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    DataCollection = new Collection<CommonName>(mgr.GetFolderItems(SearchEntity));
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
        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    SetSimplifiedName();
                    RowsAffected = mgr.Insert(Entity);
                    Entity = mgr.Get(Entity.ID);
                    DataCollection.Add(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public int Update()
        {
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    SetSimplifiedName();
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
                return RowsAffected;
            }
        }

        public void Search()
        {
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    DataCollection = new Collection<CommonName>(mgr.Search(SearchEntity));
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
        public void RunSearch(int appUserItemFolderId)
        {
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            appUserItemListViewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
            appUserItemListViewModel.Search();
            SearchEntity = Deserialize<CommonNameSearch>(appUserItemListViewModel.Entity.Properties);
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
        public void SearchFolderItems()
        {
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    DataCollection = new Collection<CommonName>(mgr.SearchFolderItems(SearchEntity));
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

        public List<CommonName> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public override bool Validate()
        {
            bool validated = true;

            if ((Entity.SpeciesID == 0) && (Entity.GenusID == 0))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please identify either a species or genus in reference to this common name." });
            }

            if (String.IsNullOrEmpty(Entity.Name))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The name is required." });
            }

            //TODO CHECK DUPES
            //SearchEntity.Name = Entity.Name;
            //SearchEntity.IsNameExactMatch = "Y";
            //SearchEntity.ExcludeID = Entity.ID;
            //Search();
            //if (DataCollection.Count > 0)
            //{
            //    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The name " + Entity.Name + " already exists." });            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }

    }
}
