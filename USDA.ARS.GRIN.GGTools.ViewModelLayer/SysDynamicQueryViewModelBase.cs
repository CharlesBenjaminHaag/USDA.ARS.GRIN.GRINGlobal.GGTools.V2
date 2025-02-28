using System;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysDynamicQueryViewModelBase: AppViewModelBase
    {
        private AppUserItemFolder _AppUserItemFolderFolderEntity = new AppUserItemFolder();
        private SysDynamicQuery _Entity = new SysDynamicQuery();
        private SysDynamicQuerySearch _SearchEntity = new SysDynamicQuerySearch();
        private Collection<SysDynamicQuery> _DataCollection = new Collection<SysDynamicQuery>();
        private Collection<AppUserItemFolder> _DataCollectionAppUserItemFolders = new Collection<AppUserItemFolder>();
        private Collection<SysTable> _DataCollectionSysTables = new Collection<SysTable>();
        private DataTable _DataCollectionDataTable = new DataTable();
        public AppUserItemFolder AppUserItemFolderEntity
        {
            get { return _AppUserItemFolderFolderEntity; }
            set { _AppUserItemFolderFolderEntity = value; }
        }

        public SysDynamicQuery Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysDynamicQuerySearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<SysDynamicQuery> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<AppUserItemFolder> DataCollectionAppUserItemFolders
        {
            get { return _DataCollectionAppUserItemFolders; }
            set { _DataCollectionAppUserItemFolders = value; }
        }

        public Collection<SysTable> DataCollectionSysTables
        {
            get { return _DataCollectionSysTables; }
            set { _DataCollectionSysTables = value; }
        }

        public DataTable DataCollectionDataTable
        {
            get { return _DataCollectionDataTable; }
            set { _DataCollectionDataTable = value; }
        }
    }
}
