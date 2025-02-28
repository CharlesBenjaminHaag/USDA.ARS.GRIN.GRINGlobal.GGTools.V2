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
    public class SysGroupUserMapViewModel : SysGroupUserMapViewModelBase, IViewModel<SysGroupUserMap>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public SysGroupUserMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public void GetSysGroupUserMaps(int sysUserId)
        {
            using (SysGroupUserMapManager mgr = new SysGroupUserMapManager())
            {
               DataCollection  = new Collection<SysGroupUserMap>(mgr.GetAvailable(Entity.SysUserID));
            }
        }

        public void GetSysGroupPermissions(int sysUserId) 
        { 
        
        }

        public void GetBySysUser(int sysUserId, string isAvailable)
        {
            using (SysGroupUserMapManager mgr = new SysGroupUserMapManager())
            {
                if (isAvailable == "Y")
                {
                  DataCollectionAvailable = new Collection<SysGroupUserMap>(mgr.GetAvailable(sysUserId));
                }
                else
                {
                  DataCollectionUnavailable = new Collection<SysGroupUserMap>(mgr.GetUnavailable(sysUserId));
                }
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            try
            {
                using (SysGroupUserMapManager mgr = new SysGroupUserMapManager())
                {
                    mgr.Insert(Entity);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    
        public void Search()
        {
            using (SysGroupUserMapManager mgr = new SysGroupUserMapManager())
            {
                try
                {
                    DataCollection = new Collection<SysGroupUserMap>(mgr.Search(SearchEntity));
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

        int IViewModel<SysGroupUserMap>.Insert()
        {
            throw new NotImplementedException();
        }

        public void GetAvailableSysGroups()
        {
            using (SysGroupUserMapManager mgr = new SysGroupUserMapManager())
            {
                DataCollectionAvailable = new Collection<SysGroupUserMap>(mgr.GetAvailable(Entity.SysUserID));
            }
        }

        public void GetCurrentSysGroups()
        {
            using (SysGroupUserMapManager mgr = new SysGroupUserMapManager())
            {
                DataCollectionUnavailable = new Collection<SysGroupUserMap>(mgr.GetUnavailable(Entity.SysUserID));
            }
        }
    }
}
