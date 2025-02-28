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
    public class AppUserItemFolderViewModelBase: AuthenticatedViewModelBase
    {
        private AppUserItemFolder _Entity = new AppUserItemFolder();
        private AppUserItemFolderSearch _SearchEntity = new AppUserItemFolderSearch();
        private Collection<AppUserItemFolder> _DataCollection = new Collection<AppUserItemFolder>();
        private Collection<AppUserItemFolder> _DataCollectionUserFolders = new Collection<AppUserItemFolder>();
        private Collection<AppUserItemDynamicFolder> _DataCollectionDynamicFolders = new Collection<AppUserItemDynamicFolder>();
        private Collection<ReportItem> _DataCollectionIDTypes = new Collection<ReportItem>();
        private List<AppUserItemFolder> _DataCollectionBatch = new List<AppUserItemFolder>();

        public AppUserItemFolderViewModelBase()
        {
            TableName = "app_user_item_folder";
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
            }
        }

        public AppUserItemFolderViewModelBase(int cooperatorId)
        {
            TableName = "app_user_item_folder";
            List<CodeValue> categories = new List<CodeValue>();
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");

                categories = mgr.GetCategories(cooperatorId);

                AppUserItemFolderSearch searchEntity = new AppUserItemFolderSearch();
                searchEntity.CreatedByCooperatorID = cooperatorId;
                DataCollectionUserFolders = new Collection<AppUserItemFolder>(mgr.Search(searchEntity));
                Categories = new SelectList(categories, "Value", "Title");
            }
        }

        public AppUserItemFolder Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public AppUserItemFolderSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<AppUserItemFolder> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<AppUserItemFolder> DataCollectionUserFolders
        {
            get { return _DataCollectionUserFolders; }
            set { _DataCollectionUserFolders = value; }
        }
        public Collection<AppUserItemDynamicFolder> DataCollectionDynamicFolders
        {
            get { return _DataCollectionDynamicFolders; }
            set { _DataCollectionDynamicFolders = value; }
        }
        public List<AppUserItemFolder> DataCollectionBatch
        {
            get { return _DataCollectionBatch; }
            set { _DataCollectionBatch = value; }
        }
        public Collection<ReportItem> DataCollectionIDTypes
        {
            get { return _DataCollectionIDTypes; }
            set { _DataCollectionIDTypes = value; }
        }
        public SelectList Categories { get; set; }
    }
}
