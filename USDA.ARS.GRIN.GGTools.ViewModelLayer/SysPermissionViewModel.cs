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
    public class SysPermissionViewModel : SysPermissionViewModelBase, IViewModel<SysPermission>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public SysPermission Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public void GetPermissionsByTable()
        {
            using (SysPermissionManager mgr = new SysPermissionManager())
            {
                DataCollection = new Collection<SysPermission>(mgr.GetSysPermissionsByTable(SearchEntity.SysUserID, SearchEntity.TableName));
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
            using (SysPermissionManager mgr = new SysPermissionManager())
            {
                try
                {
                    DataCollection = new Collection<SysPermission>(mgr.Search(SearchEntity));
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
    }
}
