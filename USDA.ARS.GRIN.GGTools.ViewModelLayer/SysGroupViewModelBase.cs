using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysGroupViewModelBase : AppViewModelBase
    {
        private SysGroup _Entity = new SysGroup();
        private SysGroupSearch _SearchEntity = new SysGroupSearch();
        private Collection<SysGroup> _DataCollection = new Collection<SysGroup>();
        private Collection<SysGroupUserMap> _DataCollectionSysGroupUserMap = new Collection<SysGroupUserMap>();
        private Collection<SysPermission> _DataCollectionSysPermission = new Collection<SysPermission>();

        public SysGroup Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysGroupSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<SysGroup> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<SysGroupUserMap> DataCollectionSysGroupUserMap
        {
            get { return _DataCollectionSysGroupUserMap; }
            set { _DataCollectionSysGroupUserMap = value; }
        }

        public Collection<SysPermission> DataCollectionSysPermission
        {
            get { return _DataCollectionSysPermission; }
            set { _DataCollectionSysPermission = value; }
        }
    }
}
