using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CodeValueViewModel : CodeValueViewModelBase
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Get(int entityId)
        {
            using (CodeValueManager mgr = new CodeValueManager())
            {
                try
                {
                    Entity = mgr.Get(entityId);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }

            //using (SysTableManager sysTableManager = new SysTableManager())
            //{
            //    DataCollectionSysTableFields = new Collection<SysTableField>(sysTableManager.GetSysTableFieldsByGroupName(Entity.GroupName));
            //}

            //if (DataCollectionSysTableFields.Count >= 1)
            //{
            //    using (SysDynamicQueryManager sysDynamicQueryManager = new SysDynamicQueryManager()) 
            //    {
            //        SysDynamicQuerySearch sysDynamicQuerySearch = new SysDynamicQuerySearch();
            //        sysDynamicQuerySearch.SQLStatement = "SELECT * FROM " + DataCollectionSysTableFields[0].SysTableName + " WHERE " + DataCollectionSysTableFields[0].FieldName + " = '" + Entity.GroupName + "'";
            //        DataCollectionDataTable = sysDynamicQueryManager.Search(sysDynamicQuerySearch);
            //    }
            //}
        }

        public void GetRelatedSysTableField()
        {
            using (SysTableManager mgr = new SysTableManager())
            {
                mgr.GetSysTableFieldsByGroupName(Entity.GroupName);
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (CodeValueManager mgr = new CodeValueManager())
            {
                try
                {
                    Entity.ID = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }

        public void Search()
        {
            using (CodeValueManager mgr = new CodeValueManager())
            {
                try
                {
                    DataCollection = new Collection<CodeValue>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public List<CodeValue> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (CodeValueManager mgr = new CodeValueManager())
            {
                try
                {
                    Entity.ID = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }
    }
}
