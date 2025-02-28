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
    public class FolderViewModelBase : AuthenticatedViewModelBase
    {
        private string _DefaultCategory = String.Empty;
        private string _PartialViewName = String.Empty;
        private AppUserItemFolder _Entity = new AppUserItemFolder(); 
        private FolderSearch _SearchEntity = new FolderSearch();

        private Collection<Cooperator> _DataCollectionAvailableCooperators = new Collection<Cooperator>();
        private Collection<Cooperator> _DataCollectionSharedCooperators = new Collection<Cooperator>();

        private Collection<AppUserItemFolder> _DataCollection = new Collection<AppUserItemFolder>();
        private Collection<AppUserItemFolder> _DataCollectionShared = new Collection<AppUserItemFolder>();
        private Collection<AppUserItemFolder> _DataCollectionAvailableFolders = new Collection<AppUserItemFolder>();
        private List<AppUserItemFolder> _DataCollectionBatch = new List<AppUserItemFolder>();

        private Collection<AppUserItemList> _DataCollectionFolderItems = new Collection<AppUserItemList>();
        private Collection<CodeValue> _DataCollectionFolderLists = new Collection<CodeValue>();
        private Collection<CodeValue> _DataCollectionFolderTypes = new Collection<CodeValue>();
        private Collection<CodeValue> _DataCollectionFolderCategories = new Collection<CodeValue>();
        public FolderViewModelBase()
        {
            using (FolderManager mgr = new FolderManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("app_user_item_folder"), "ID", "FullName");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
            }
        }
        public FolderViewModelBase(int cooperatorId, string tableName)
        {
        }

        public string IsReadOnly
        {
            get
            {
                string isReadOnly = "Y";

                if ((AuthenticatedUser.IsInRole("GGTOOLS_TAXON")) ||
                    (AuthenticatedUser.IsInRole("GGTOOLS_ADMIN")))
                {
                    isReadOnly = "N";
                }
                
                if ((AuthenticatedUser.CooperatorID == Entity.CreatedByCooperatorID))
                {
                    isReadOnly = "N";
                }
                else
                {
                    isReadOnly = "Y";
                }

                return isReadOnly;
            }
        }

        public string DefaultCategory
        {
            get { return _DefaultCategory; }
            set { _DefaultCategory = value; }
        }
        public string PartialViewName
        {
            get { return _PartialViewName; }
            set { _PartialViewName = value; }
        }

        public AppUserItemFolder Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public FolderSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<AppUserItemFolder> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<AppUserItemFolder> DataCollectionShared
        {
            get { return _DataCollectionShared; }
            set { _DataCollectionShared = value; }
        }
        public Collection<AppUserItemFolder> DataCollectionAvailableFolders
        {
            get { return _DataCollectionAvailableFolders; }
            set { _DataCollectionAvailableFolders = value; }
        }

        public List<AppUserItemFolder> DataCollectionBatch
        {
            get { return _DataCollectionBatch; }
            set { _DataCollectionBatch = value; }
        }

        public Collection<Cooperator> DataCollectionAvailableCooperators
        {
            get { return _DataCollectionAvailableCooperators; }
            set { _DataCollectionAvailableCooperators = value; }
        }

        public Collection<Cooperator> DataCollectionCurrentCooperators
        {
            get { return _DataCollectionSharedCooperators; }
            set { _DataCollectionSharedCooperators = value; }
        }
       
        public Collection<CodeValue> DataCollectionFolderTypes
        {
            get { return _DataCollectionFolderTypes; }
            set { _DataCollectionFolderTypes = value; }
        }

        public Collection<CodeValue> DataCollectionFolderCategories
        {
            get { return _DataCollectionFolderCategories; }
            set { _DataCollectionFolderCategories = value; }
        }

        public Collection<AppUserItemList> DataCollectionFolderItems
        {
            get { return _DataCollectionFolderItems; }
            set { _DataCollectionFolderItems = value; }
        }

        public void GetFolderTypes(int cooperatorID = 0)
        {
            List<CodeValue> types = new List<CodeValue>();
            using (FolderManager mgr = new FolderManager())
            {
                types = mgr.GetFolderTypes(cooperatorID);
                DataCollectionFolderTypes = new Collection<CodeValue>(types);
                Types = new SelectList(types, "Value", "Title");
            }
        }
        public void GetFolderCategories(int cooperatorID = 0)
        {
            List<CodeValue> categories = new List<CodeValue>();
            using (FolderManager mgr = new FolderManager())
            {
                categories = mgr.GetFolderCategories(cooperatorID);
                DataCollectionFolderCategories = new Collection<CodeValue>(categories);
                Categories = new SelectList(categories, "Value", "Title");
            }
        }

        #region Select Lists
        public SelectList Types { get; set; }
        public SelectList Categories { get; set; }
        public SelectList ApplicationCooperators { get; set; }
        #endregion
    }
}
