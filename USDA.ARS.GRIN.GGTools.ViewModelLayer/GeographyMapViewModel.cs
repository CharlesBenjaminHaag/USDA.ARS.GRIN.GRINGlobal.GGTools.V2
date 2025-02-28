using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class GeographyMapViewModel : GeographyMapViewModelBase, IViewModel<GeographyMap>
    {
        public GeographyMapViewModel()
        {
        }

        public GeographyMapViewModel(int speciesId)
        {
            
        }

        public void Delete()
        {
            try
            {
                using (GeographyMapManager mgr = new GeographyMapManager())
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

        public GeographyMap Get(int entityId)
        {
            try
            {
                using (GeographyMapManager mgr = new GeographyMapManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
                        if (DataCollection.Count == 1)
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
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity;
        }
        
        public void GetFolderItems()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    DataCollection = new Collection<GeographyMap>(mgr.GetFolderItems(SearchEntity));
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
        
        public void GetList()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    EditCollection = mgr.Search(SearchEntity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            List<string> geographyMapIDList = new List<string>();

            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    RowsAffected = mgr.Insert(Entity);
                    Entity = mgr.Get(Entity.ID);
                    DataCollection.Add(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex; 
                }
            }
        }

        public void InsertBatch()
        {
            if (!String.IsNullOrEmpty(GeographyIDList))
            {
                string[] geographyIdList = GeographyIDList.Split(',');

                foreach (var geographyId in geographyIdList)
                {
                    Entity.ID = 0;
                    Entity.GeographyID = Int32.Parse(geographyId.ToString());
                    Insert();
                }
            }
        }
        
        public List<GeographyMap> InsertMultiple()
        {
            int geographyMapId = 0;
            string[] speciesIdList = SpeciesIDList.Split(',');
            string[] geographyIdList = GeographyIDList.Split(',');
            List<GeographyMap> geographyMaps = new List<GeographyMap>();

            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                foreach (var speciesId in speciesIdList)
                {
                    foreach (var geographyId in geographyIdList)
                    {
                        GeographyMap geographyMap = new GeographyMap();
                        geographyMap.SpeciesID = Int32.Parse(speciesId);
                        geographyMap.GeographyStatusCode = Entity.GeographyStatusCode;
                        geographyMap.GeographyID = Int32.Parse(geographyId);
                        geographyMap.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                        geographyMapId = mgr.Insert(geographyMap);

                        // Add new syn map record to a list of recs created in the current session.
                        // Note that, given that the sproc only inserts records if the taxon A/syn code/taxon B
                        // combination does not already exist, the result of the insert in those instances will
                        // be -1, vs. the new synonym map ID.
                        if (geographyMapId > 0)
                        {
                            GeographyMap geographyMapBatch = mgr.Get(geographyMapId);
                            geographyMaps.Add(geographyMapBatch);
                        }
                    }
                }
            }
            DataCollection = new Collection<GeographyMap>(geographyMaps);
            return geographyMaps;
        }
        
        public void Map()
        {
            var itemIdList = ItemIDList.Split(',');

            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                foreach (var id in itemIdList)
                {
                    GeographyMap geographyMap = new GeographyMap { ID = Entity.ID, SpeciesID = Int32.Parse(id) };
                    mgr.Insert(geographyMap);
                }
            }
        }

        public void Update()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        
        public JsonResult AddCitation(int citationId, string idList)
        {
            string[] idCollection;
            idCollection = idList.Split(',');
                
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                foreach (var id in idCollection)
                {
                    int convertedId = Int32.Parse(id);
                    mgr.AddCitation(citationId, convertedId);
                }
            }
            
            //TODO
            return null;
        }

        public void Search()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    DataCollection = new Collection<GeographyMap>(mgr.Search(SearchEntity));
                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
                    RowsAffected = mgr.RowsAffected;
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
            SearchEntity = Deserialize<GeographyMapSearch>(appUserItemListViewModel.Entity.Properties);
            Search();
        }
        
        public void SearchFolderItems()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    DataCollection = new Collection<GeographyMap>(mgr.SearchFolderItems(SearchEntity));
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

        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (CWRMapManager mgr = new CWRMapManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        int IViewModel<GeographyMap>.Insert()
        {
            throw new NotImplementedException();
        }

        int IViewModel<GeographyMap>.Update()
        {
            throw new NotImplementedException();
        }

        public List<GeographyMap> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

    }
}
