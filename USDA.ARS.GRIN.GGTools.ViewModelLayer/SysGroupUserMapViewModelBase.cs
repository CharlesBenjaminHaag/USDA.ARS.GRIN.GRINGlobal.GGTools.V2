using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysGroupUserMapViewModelBase : AppViewModelBase
    {
        private SysGroupUserMap _Entity = new SysGroupUserMap();
        private SysGroupUserMapSearch _SearchEntity = new SysGroupUserMapSearch();
        private Collection<SysGroupUserMap> _DataCollection = new Collection<SysGroupUserMap>();
        private Collection<SysGroupUserMap> _DataCollectionAvailable = new Collection<SysGroupUserMap>();
        private Collection<SysGroupUserMap> _DataCollectionUnavailable = new Collection<SysGroupUserMap>();

        public SysGroupUserMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysGroupUserMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<SysGroupUserMap> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<SysGroupUserMap> DataCollectionAvailable
        {
            get { return _DataCollectionAvailable; }
            set { _DataCollectionAvailable = value; }
        }

        public Collection<SysGroupUserMap> DataCollectionUnavailable
        {
            get { return _DataCollectionUnavailable; }
            set { _DataCollectionUnavailable = value; }
        }
    }
}
