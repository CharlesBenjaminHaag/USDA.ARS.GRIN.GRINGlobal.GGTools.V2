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
    public class SysFolderCooperatorMapViewModel: SysFolderCooperatorMapViewModelBase
    {
        public bool IsFavoriteSelector { get; set; }
       
        public SysFolderCooperatorMapViewModel()
        { }
      
        public void Get(int entityId)
        {
            
        }

        public void GetMappedCooperators(int sysFolderId)
        {
            using (SysFolderCooperatorMapManager mgr = new SysFolderCooperatorMapManager())
            {
                DataCollectionMapped = new Collection<Cooperator>(mgr.GetMappedCooperators(sysFolderId));
            }
        }

        public void GetNonMappedCooperators(int sysFolderId)
        {
            using(SysFolderCooperatorMapManager mgr = new SysFolderCooperatorMapManager())
            {
                DataCollectionNonMapped = new Collection<Cooperator>(mgr.GetNonMappedCooperators(sysFolderId));
            }
        }

        public void Search()
        {
            //using (SysFolderManager mgr = new SysFolderManager())
            //{
            //    try
            //    {
            //        DataCollection = new Collection<SysFolder>(mgr.Search(SearchEntity));
            //        RowsAffected = mgr.RowsAffected;

            //        if (DataCollection.Count == 1)
            //        {
            //            Entity = DataCollection[0];
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        PublishException(ex);
            //        throw ex;
            //    }
            //}
        }
        
        public void Insert()
        {
            try
            {
                using (SysFolderCooperatorMapManager mgr = new SysFolderCooperatorMapManager())
                {
                    Entity.ID = mgr.Insert(Entity);
                }
                InsertItems();
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }
        
        public void InsertItems()
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                if (!String.IsNullOrEmpty(ItemIDList))
                {
                    foreach (var entityId in ItemIDList.Split(','))
                    {
                        SysFolderItemMap sysFolderItemMap = new SysFolderItemMap();
                        sysFolderItemMap.FolderID = Entity.ID;
                        sysFolderItemMap.TableName = Entity.TableName;
                        sysFolderItemMap.IDNumber = Int32.Parse(entityId);
                        sysFolderItemMap.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                        mgr.InsertItem(sysFolderItemMap);
                    }
                }
            }

        }

        public void InsertItem(SysFolderItemMap sysFolderItemMap)
        { 
        
        }

        public int Update()
        {
            //using (SysFolderManager mgr = new SysFolderManager())
            //{
            //    try
            //    {
            //        RowsAffected = mgr.Update(Entity);
            //    }
            //    catch (Exception ex)
            //    {
            //        PublishException(ex);
            //        throw ex;
            //    }
                return RowsAffected;
            //}
        }
        
        public void Delete()
        {
            try
            {
                using (SysFolderManager mgr = new SysFolderManager())
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

        public void DeleteItems()
        {
            string[] itemIdList = ItemIDList.Split(',');

            using (SysFolderCooperatorMapManager mgr = new SysFolderCooperatorMapManager())
            {
                foreach (var itemId in itemIdList)
                {
                    mgr.DeleteItems(Entity.SysFolderID, Int32.Parse(itemId));
                }
            }
        }
    }
}
