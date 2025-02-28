using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CitationViewModel : CitationViewModelBase, IViewModel<Citation>
    {
        public void GetFolderItems()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    DataCollection = new Collection<Citation>(mgr.GetFolderItems(SearchEntity));
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
        public Citation Get(int entityId)
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    DataCollection = new Collection<Citation>(mgr.Search(SearchEntity));
                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                        Entity.IsAcceptedNameOption = ToBool(Entity.IsAcceptedName);
                        Entity.CitationID = Entity.ID;
                        Entity.AssembledName = Entity.AssembledName.TrimStart('.');
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
                return Entity;
            }
        }
        public void GetSpeciesCitations(int speciesId, string tableName)
        {
            try
            {
                using (CitationManager mgr = new CitationManager())
                {
                    DataCollection = new Collection<Citation>(mgr.GetSpeciesCitations(speciesId, tableName));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ReportItem GetCitationReferenceCounts(int citationId)
        {
            ReportItem reportItem = new ReportItem();
            try
            {
                using (CitationManager mgr = new CitationManager())
                {
                   DataCollectionReportItems = new Collection<ReportItem>(mgr.GetCitationReferenceCounts(citationId));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportItem;
        }
        public int UpdateSpeciesCitation(string tableName, int entityId, int citationId, int cooperatorId)
        {
            using (CitationManager mgr = new CitationManager())
            {
                return mgr.UpdateSpeciesCitation(tableName, entityId, citationId, cooperatorId);
            }
        }
        
        public int Insert()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    Entity.IsAcceptedName = FromBool(Entity.IsAcceptedNameOption);
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
        
        public int GetClone(int entityId)
        {
            using (CitationManager mgr = new CitationManager())
            {
                Citation clonedCitation = new Citation();

                try
                {
                    clonedCitation = mgr.Get(entityId);
                    clonedCitation.ID = 0;
                    clonedCitation.ID = mgr.Insert(clonedCitation);    
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
                return clonedCitation.ID;
            }
        }

        public void Search()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    DataCollection = new Collection<Citation>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }
        public void RunSearch(int appUserItemFolderId)
        {
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            appUserItemListViewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
            appUserItemListViewModel.Search();
            SearchEntity = Deserialize<CitationSearch>(appUserItemListViewModel.Entity.Properties);
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
        public int Update()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    Entity.IsAcceptedName = FromBool(Entity.IsAcceptedNameOption);
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
            return RowsAffected;
        }
        public override bool Validate()
        {
            bool validated = true;

            if ((Entity.FamilyID == 0) && (Entity.GenusID == 0) && (Entity.SpeciesID == 0))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must specify a taxon." });
            }

            if ((String.IsNullOrEmpty(Entity.CitationTitle)) && (String.IsNullOrEmpty(Entity.Abbreviation)))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must specify either a title, or an abbreviated literature source." });
            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }
    }
}
