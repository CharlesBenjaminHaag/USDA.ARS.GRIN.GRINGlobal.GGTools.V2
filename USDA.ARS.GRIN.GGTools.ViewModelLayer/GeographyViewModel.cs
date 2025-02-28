using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Caching;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class GeographyViewModel : GeographyViewModelBase, IViewModel<GeographyViewModel>
    {
        public void InitializeCachedData()
        {
            ObjectCache cache = MemoryCache.Default;
            List<Region> continents = new List<Region>();
          
            using (GeographyManager mgr = new GeographyManager())
            {
                continents = mgr.GetContinents();
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            cache.Set("DATA-LIST-GEOGRAPHY-CONTINENTS", continents, policy);
        }
        public void GetFolderItems()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                try
                {
                    DataCollection = new Collection<Geography>(mgr.GetFolderItems(SearchEntity));
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

        public Geography Get(int entityId)
        {
            try
            {
                using (GeographyManager mgr = new GeographyManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
                        if (RowsAffected == 1)
                        {
                            Entity = DataCollection[0];
                            Entity.IsValidOption = ToBool(Entity.IsValid);
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

        //public List<Region> GetRegions()
        //{
        //    List<Region> regions = new List<Region>();

        //    ObjectCache cache = MemoryCache.Default;
        //    regions = cache["DATA-LIST-GEOGRAPHY-REGIONS"] as List<Region>;

        //    if (regions == null)
        //    {
        //        CacheItemPolicy policy = new CacheItemPolicy();
        //        using (GeographyManager mgr = new GeographyManager())
        //        {
        //            regions = mgr.GetRegions();
        //        }
        //        cache.Set("DATA-LIST-GEOGRAPHY-REGIONS", regions, policy);
        //    }
        //    return regions;
        //}

        public void GetContinents()
        {
            List<Region> regions = new List<Region>();
            using (GeographyManager mgr = new GeographyManager())
            {
                regions = mgr.GetContinents();
            }
            DataCollectionContinents = new Collection<Region>(regions);

            //ObjectCache cache = MemoryCache.Default;
            //regions = cache["DATA-LIST-GEOGRAPHY-CONTINENTS"] as List<Region>;

            //if (regions == null)
            //{
            //    CacheItemPolicy policy = new CacheItemPolicy();
            //    using (GeographyManager mgr = new GeographyManager())
            //    {
            //        regions = mgr.GetContinents();
            //    }
            //    cache.Set("DATA-LIST-GEOGRAPHY-CONTINENTS", regions, policy);
            //}

        }

        public void GetSubContinents()
        {
            List<Region> regions = new List<Region>();
            using (GeographyManager mgr = new GeographyManager())
            {
                regions = mgr.GetSubContinents(SearchEntity.ContinentNameList);
            }
            DataCollectionSubContinents = new Collection<Region>(regions);
            // TOD: SEE WHERE WE CAN USE THIS. -CBH, 4/5/23
            //ObjectCache cache = MemoryCache.Default;
            //regions = cache["DATA-LIST-GEOGRAPHY-SUBCONTINENTS"] as List<Region>;

            //if (regions == null)
            //{
            //    CacheItemPolicy policy = new CacheItemPolicy();
            //    using (GeographyManager mgr = new GeographyManager())
            //    {
            //        regions = mgr.GetSubContinents();
            //    }
            //    cache.Set("DATA-LIST-GEOGRAPHY-SUBCONTINENTS", regions, policy);
            //}

        }

        public void GetCountries()
        {
            List<Country> countries = new List<Country>();
            try
            {
                using (GeographyManager mgr = new GeographyManager())
                {
                    countries = mgr.GetCountries(SearchEntity.ContinentNameList, SearchEntity.SubContinentNameList);
                }
                DataCollectionCountries = new Collection<Country>(countries);
                //ObjectCache cache = MemoryCache.Default;
                //countries = cache["DATA-LIST-GEOGRAPHY-COUNTRIES"] as List<Country>;

                //if (countries == null)
                //{
                //    CacheItemPolicy policy = new CacheItemPolicy();
                //    using (GeographyManager mgr = new GeographyManager())
                //    {
                //        countries = mgr.GetCountries();
                //    }
                //    cache.Set("DATA-LIST-GEOGRAPHY-COUNTRIES", countries, policy);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Geography> GetAdministrativeUnits()
        {
            List<Geography> geographies = new List<Geography>();
            using (GeographyManager mgr = new GeographyManager())
            {
                geographies = mgr.GetGeographies(SearchEntity.SubContinentIDList, SearchEntity.CountryCodeList);
            }
            DataCollection = new Collection<Geography>(geographies);
            return geographies;

            //ObjectCache cache = MemoryCache.Default;
            //geographies = cache["DATA-LIST-GEOGRAPHY-GEOGRAPHIES"] as List<Geography>;

            //if (geographies == null)
            //{
            //    CacheItemPolicy policy = new CacheItemPolicy();
            //    using (GeographyManager mgr = new GeographyManager())
            //    {
            //        geographies = mgr.GetGeographies();
            //    }
            //    cache.Set("DATA-LIST-GEOGRAPHY-GEOGRAPHIES", geographies, policy);
            //}
        }

        public List<Geography> GetGeographyCountryAdmins()
        {
            List<Geography> geographies = new List<Geography>();
            using (GeographyManager mgr = new GeographyManager())
            {
                geographies = mgr.GetGeographyCountryAdmins();
            }
            DataCollection = new Collection<Geography>(geographies);
            return geographies;
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }


        public void Insert()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                try
                {
                    Entity.IsValid = FromBool(Entity.IsValidOption);
                    RowsAffected = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void Update()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                try
                {
                    Entity.IsValid = FromBool(Entity.IsValidOption);
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void Search()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                try
                {
                    DataCollection = new Collection<Geography>(mgr.Search(SearchEntity));
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
            SearchEntity = Deserialize<GeographySearch>(appUserItemListViewModel.Entity.Properties);
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
            using (CWRMapManager mgr = new CWRMapManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        GeographyViewModel IViewModel<GeographyViewModel>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        int IViewModel<GeographyViewModel>.Insert()
        {
            throw new NotImplementedException();
        }

        int IViewModel<GeographyViewModel>.Update()
        {
            throw new NotImplementedException();
        }

        public List<GeographyViewModel> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
