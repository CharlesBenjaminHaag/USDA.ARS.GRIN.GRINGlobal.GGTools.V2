using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysPermissionViewModelBase : AppViewModelBase
    {
        private SysPermission _Entity = new SysPermission();
        private SysPermissionSearch _SearchEntity = new SysPermissionSearch();
        private Collection<SysPermission> _DataCollection = new Collection<SysPermission>();

        public SysPermission Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysPermissionSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<SysPermission> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
    }
}
