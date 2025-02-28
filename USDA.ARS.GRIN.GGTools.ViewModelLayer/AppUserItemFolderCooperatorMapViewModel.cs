using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserItemFolderCooperatorMapViewModel : AppUserItemFolderCooperatorMapViewModelBase
    {
        public void GetNonMapped()
        {
            try
            {
                using (AppUserItemFolderCooperatorMapManager mgr = new AppUserItemFolderCooperatorMapManager())
                {
                    DataCollectionNonMappedCooperators = new Collection<Cooperator>(mgr.GetNonMapped(SearchEntity.FolderID));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetMapped()
        {
            try
            {
                using (AppUserItemFolderCooperatorMapManager mgr = new AppUserItemFolderCooperatorMapManager())
                {
                    DataCollectionMappedCooperators = new Collection<Cooperator>(mgr.GetMapped(SearchEntity.FolderID));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Insert()
        {
            using (AppUserItemFolderCooperatorMapManager mgr = new AppUserItemFolderCooperatorMapManager())
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
        }

        public void InsertBatch()
        {
            using (AppUserItemFolderCooperatorMapManager mgr = new AppUserItemFolderCooperatorMapManager())
            {
                try
                {
                    string[] itemIdArray = ItemIDList.Split(',');
                    foreach (var itemId in itemIdArray)
                    {
                        Entity.CooperatorID = Int32.Parse(itemId);
                        RowsAffected = mgr.Insert(Entity);
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
            using (AppUserItemFolderCooperatorMapManager mgr = new AppUserItemFolderCooperatorMapManager())
            {
                try
                {
                    RowsAffected = mgr.Delete(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void DeleteBatch()
        {
            using (AppUserItemFolderCooperatorMapManager mgr = new AppUserItemFolderCooperatorMapManager())
            {
                try
                {
                    string[] itemIdArray = ItemIDList.Split(',');
                    foreach (var itemId in itemIdArray)
                    {
                        Entity.CooperatorID = Int32.Parse(itemId);
                        RowsAffected = mgr.Delete(Entity);
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
    }
}
