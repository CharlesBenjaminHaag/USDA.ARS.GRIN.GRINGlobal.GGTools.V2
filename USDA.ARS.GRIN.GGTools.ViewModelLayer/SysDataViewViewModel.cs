using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysDataViewViewModel : SysDataViewViewModelBase, IViewModel<SysDataView>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Get(int entityId = 0)
        {
            using (SysDataViewManager mgr = new SysDataViewManager())
            {
                Entity = mgr.Get(entityId);
            }
        }

        public void GetAll()
        {
            using (SysDataViewManager mgr = new SysDataViewManager())
            {
                DataCollection = new Collection<SysDataView>(mgr.GetAll());
            }
        }

        public void GetParameters(int entityId)
        {
            using (SysDataViewManager mgr = new SysDataViewManager())
            {
               DataCollectionParameters  = new Collection<SysDataViewParameter>(mgr.GetParameters(entityId));
            }
        }

        public void GetFields(int entityId)
        {
            using (SysDataViewManager mgr = new SysDataViewManager())
            {
                DataCollectionFields = new Collection<SysDataViewField>(mgr.GetFields(entityId));

                foreach (var field in DataCollectionFields)
                { 
                    
                }
            }
        }

        public void GetSQL(int entityId)
        {
            using (SysDataViewManager mgr = new SysDataViewManager())
            {
                SQL = mgr.GetSQL(entityId);
            }
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
      
        }

        public int Update()
        {
            throw new NotImplementedException();
        }

        SysDataView IViewModel<SysDataView>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
