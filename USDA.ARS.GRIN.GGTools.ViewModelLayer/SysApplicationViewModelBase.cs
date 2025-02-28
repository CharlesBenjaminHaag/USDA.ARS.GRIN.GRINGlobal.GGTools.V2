using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysApplicationViewModelBase : AppViewModelBase
    {
        private SysApplication _Entity = new SysApplication();
        private SysApplicationSearch _SearchEntity = new SysApplicationSearch();
        private Collection<SysApplication> _DataCollection = new Collection<SysApplication>();

        public SysApplication Entity 
        {
            get { return this._Entity; }
            set { this._Entity = value; }
        }

        public SysApplicationSearch SearchEntity
        {
            get { return this._SearchEntity; }
            set { this._SearchEntity = value; }
        }
        public Collection<SysApplication> DataCollection
        {
            get { return this._DataCollection; }
            set { this._DataCollection = value; }
        }
    }
}
