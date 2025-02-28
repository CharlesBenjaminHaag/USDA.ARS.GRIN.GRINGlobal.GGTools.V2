using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysGroupViewModel : SysGroupViewModelBase, IViewModel<SysGroup>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public SysGroup Get(int sysGroupId)
        {
            SearchEntity.ID = sysGroupId;
            Search();
            GetSysGroupUserMaps(sysGroupId);
            GetSysPermissions(sysGroupId);  
            return this.Entity;
        }

        public void GetSysGroupUserMaps(int sysGroupId)
        {
            using (SysGroupManager mgr = new SysGroupManager())
            {
               DataCollectionSysGroupUserMap = new Collection<SysGroupUserMap>(mgr.GetSysGroupUserMaps(sysGroupId));
            }
        }

        public void GetSysPermissions(int sysGroupId)
        {
            using (SysGroupManager mgr = new SysGroupManager())
            {
            DataCollectionSysPermission = new Collection<SysPermission>(mgr.GetSysPermissions(sysGroupId));
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
            using (SysGroupManager mgr = new SysGroupManager())
            {
                try
                {
                    DataCollection = new Collection<SysGroup>(mgr.Search(SearchEntity));
                    if (DataCollection.Count() == 1)
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

        public int Update()
        {
            throw new NotImplementedException();
        }

        SysGroup IViewModel<SysGroup>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
