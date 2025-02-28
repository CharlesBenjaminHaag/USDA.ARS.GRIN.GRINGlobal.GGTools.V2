using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class ErrorLogViewModelBase :AppViewModelBase
    {
        private ErrorLog _Entity = new ErrorLog();
        private ErrorLogSearch _SearchEntity = new ErrorLogSearch();
        private Collection<ErrorLog> _DataCollection = new Collection<ErrorLog>();

        public ErrorLog Entity
        {
            get { return this._Entity; }
            set { this._Entity = value; }
        }

        public ErrorLogSearch SearchEntity
        {
            get { return this._SearchEntity; }
            set { this._SearchEntity = value; }
        }
        
        public Collection<ErrorLog> DataCollection
        {
            get { return this._DataCollection; }
            set { this._DataCollection = value; }
        }

    }
}
