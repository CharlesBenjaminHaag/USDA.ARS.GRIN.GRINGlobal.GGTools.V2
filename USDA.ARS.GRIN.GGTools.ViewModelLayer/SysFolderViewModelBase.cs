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
    public class SysFolderViewModelBase: AuthenticatedViewModelBase
    {
        private SysFolder _Entity = new SysFolder();
        private SysFolderProperties _SysFolderPropertiesEntity = new SysFolderProperties();
        private SysTag _TagEntity = new SysTag();
        private SysFolderSearch _SearchEntity = new SysFolderSearch();
        private Collection<SysFolder> _DataCollection = new Collection<SysFolder>();
        private Collection<SysFolderItemMap> _DataCollectionSysFolderItemMaps = new Collection<SysFolderItemMap>();
        private Collection<SysFolderCooperatorMap> _DataCollectionSysFolderCooperatorMaps = new Collection<SysFolderCooperatorMap>();
        private Collection<SysTag> _DataCollectionSysTags = new Collection<SysTag>();
        private Collection<SysTable> _DataCollectionSysTables = new Collection<SysTable>();

        public SysFolderViewModelBase()
        {
            using (SysFolderManager mgr = new SysFolderManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("sys_folder"), "ID", "FullName");
            }
        }

        public SysFolderViewModelBase(int cooperatorId)
        {
            
        }

        public SysFolder Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysFolderProperties SysFolderPropertiesEntity
        {
            get { return _SysFolderPropertiesEntity; }
            set { _SysFolderPropertiesEntity = value; }
        }

        public SysFolderSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public SysTag TagEntity
        {
            get { return _TagEntity; }
            set { _TagEntity = value; }
        }

        public Collection<SysFolder> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<SysFolderItemMap> DataCollectionSysFolderItemMaps
        {
            get { return _DataCollectionSysFolderItemMaps; }
            set { _DataCollectionSysFolderItemMaps = value; }
        }

        public Collection<SysFolderCooperatorMap> DataCollectionCooperatorMaps
        {
            get { return _DataCollectionSysFolderCooperatorMaps; }
            set { _DataCollectionSysFolderCooperatorMaps = value; }
        }

        public Collection<SysTag> DataCollectionSysTags
        {
            get { return _DataCollectionSysTags; }
            set { _DataCollectionSysTags = value; }
        }

        public Collection<SysTable> DataCollectionSysTables
        {
            get { return _DataCollectionSysTables; }
            set { _DataCollectionSysTables = value; }
        }
    }
}
