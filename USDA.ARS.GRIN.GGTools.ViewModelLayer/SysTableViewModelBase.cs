using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysTableViewModelBase : AppViewModelBase
    {
        private SysTable _Entity = new SysTable();
        private SysTableSearch _SearchEntity = new SysTableSearch();
        private Collection<SysTable> _DataCollection = new Collection<SysTable>();
        private Collection<SysPermission> _DataCollectionPermissions = new Collection<SysPermission>();
        public SysTable Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysTableSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<SysTable> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<SysPermission> DataCollectionPermissions
        {
            get { return _DataCollectionPermissions; }
            set { _DataCollectionPermissions = value; }
        }
    }
}
