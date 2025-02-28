using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;


namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AccessionInventoryAttachmentViewModelBase: AppViewModelBase
    {
        private int _VirtualPathErrors;
        private int _VirtualPathErrorPercentage;
        private int _ThumbnailVirtualPathErrors;
        private int _ThumbnailVirtualPathErrorPercentage;
        private string _BasePath;
        private string _ValidateVirtualPath;
        private string _ValidateThumbnailVirtualPath;
        private Folder _FolderEntity;
        private AccessionInventoryAttachment _Entity = new AccessionInventoryAttachment();
        private AccessionInventoryAttachmentSearch _SearchEntity = new AccessionInventoryAttachmentSearch();
        private Collection<AccessionInventoryAttachment> _DataCollection = new Collection<AccessionInventoryAttachment>();
      
        public AccessionInventoryAttachmentViewModelBase()
        {
            using (AccessionInventoryAttachmentManager mgr = new AccessionInventoryAttachmentManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_family_map"), "ID", "FullName");
                AttachmentDescriptionCodes = new SelectList(mgr.GetCodeValues("ATTACH_DESCRIPTION_CODE"), "Value", "Title");
                CategoryCodes = new SelectList(mgr.GetCodeValues("ATTACH_CATEGORY"), "Value", "Title");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                ValidationStatusOptions = new SelectList(mgr.GetValidationStatusOptions(), "Key", "Value");
                TimeFrameOptions = new SelectList(mgr.GetTimeFrameOptions(), "Value","Title");
            }

            using (CooperatorManager cooperatorManager = new CooperatorManager())
            {
                Sites = new SelectList(cooperatorManager.GetSites(),"ID","DisplayName");            
            }
        }

        public int VirtualPathErrors
        {
            get { return _VirtualPathErrors; }
            set { _VirtualPathErrors = value; }
        }

        public int VirtualPathErrorPercentage
        {
            get { return _VirtualPathErrorPercentage; }
            set { _VirtualPathErrorPercentage = value; }
        }
        public int ThumbnailVirtualPathErrors
        {
            get { return _ThumbnailVirtualPathErrors; }
            set { _ThumbnailVirtualPathErrors = value; }
        }

        public int ThumbnailVirtualPathErrorPercentage
        {
            get { return _ThumbnailVirtualPathErrorPercentage; }
            set { _ThumbnailVirtualPathErrorPercentage = value; }
        }

        public string BasePath 
        {
            get { return _BasePath; }
            set { _BasePath = value; } 
        }
        public string ValidateVirtualPath
        {
            get { return _ValidateVirtualPath; }
            set { _ValidateVirtualPath = value; }
        }
        public string ValidateThumbnailVirtualPath
        {
            get { return _ValidateThumbnailVirtualPath; }
            set { _ValidateThumbnailVirtualPath = value; }
        }
        public Folder FolderEntity
        {
            get { return _FolderEntity; }
            set { _FolderEntity = value; }
        }

        public AccessionInventoryAttachment Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public AccessionInventoryAttachmentSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<AccessionInventoryAttachment> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
       
        public SelectList ValidationStatusOptions { get; set; }
        public SelectList AttachmentDescriptionCodes { get; set; }
        public SelectList CategoryCodes { get; set; }
        public SelectList Sites { get; set; }
    }
}
