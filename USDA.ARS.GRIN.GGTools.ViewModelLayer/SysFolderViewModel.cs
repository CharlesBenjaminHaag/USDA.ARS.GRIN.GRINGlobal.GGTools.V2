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
    public class SysFolderViewModel: SysFolderViewModelBase
    {
        public bool IsFavoriteSelector { get; set; }
       
        public SysFolderViewModel()
        { }
        
        public SysFolderViewModel(int cooperatorId) : base(cooperatorId)
        {
        
        }

        public void Get(int entityId)
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                Entity = mgr.Get(entityId);
            }
        }

        public void GetProperties(int entityId)
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                SysFolderPropertiesEntity = mgr.GetSysFolderProperties(entityId);
            }
        }

        public void GetItems(int entityId)
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                DataCollectionSysFolderItemMaps = new Collection<SysFolderItemMap>(mgr.GetSysFolderItemMaps(entityId));
            }
        }

        public void GetCooperators(int entityId)
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                DataCollectionCooperatorMaps = new Collection<SysFolderCooperatorMap>(mgr.GetSysFolderCooperatorMaps(entityId));
            }
        }

        public void GetSysTags(string tableName, int entityId)
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                DataCollectionSysTags = new Collection<SysTag>(mgr.GetSysTags(tableName, entityId));
            }
        }

        public void GetSysTables(int sysFolderId)
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                DataCollectionSysTables = new Collection<SysTable>(mgr.GetSysTables(sysFolderId));
            }
        }

        public void Search()
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                try
                {
                    DataCollection = new Collection<SysFolder>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

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
        
        public void Insert()
        {
            try
            {
                using (SysFolderManager mgr = new SysFolderManager())
                {
                    Entity.ID = mgr.Insert(Entity);

                    // If folder is dynamic, save serialized search criteria string.
                    if ((Entity.TypeCode == "DYN") || (Entity.TypeCode == "SQL"))
                    {
                        mgr.InsertProperties(Entity);
                    }
           
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
            using (SysFolderManager mgr = new SysFolderManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public int UpdateProperties()
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                try
                {
                    RowsAffected = mgr.UpdateProperties(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
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

            using (SysFolderManager mgr = new SysFolderManager())
            {
                foreach (var itemId in itemIdList)
                {
                    mgr.DeleteItem(Int32.Parse(itemId));
                }
            }
        }
    }
}
