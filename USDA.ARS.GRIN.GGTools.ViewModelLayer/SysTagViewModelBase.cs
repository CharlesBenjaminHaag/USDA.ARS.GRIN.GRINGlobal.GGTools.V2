using System;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysTagViewModelBase: AuthenticatedViewModelBase
    {
        private SysTag _Entity = new SysTag();
        private SysTag _TagEntity = new SysTag();
        private SysTagSearch _SearchEntity = new SysTagSearch();
        private Collection<SysTag> _DataCollection = new Collection<SysTag>();

        public SysTagViewModelBase()
        {
           
        }

        public SysTagViewModelBase(int cooperatorId)
        {
        }

        public SysTag Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysTagSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

       

        public Collection<SysTag> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
      
    }
}
