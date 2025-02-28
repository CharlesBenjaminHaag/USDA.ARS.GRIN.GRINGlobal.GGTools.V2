using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AccessionInventoryAttachment: AppEntityBase
    {
        public int InventoryID { get; set; }
        public string InventoryText { get; set; }
        public string VirtualPath { get; set; }
        public string ThumbnailVirtualPath { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string AttachmentDescription { get; set; }
        public string AttachmentDescriptionCode { get; set; }
        public string ContentType { get; set; }
        public string CategoryCode { get; set; }
        public string CopyrightInformation { get; set; }
        public int CooperatorID { get; set; }
        
        public DateTime AttachDate { get; set; }
        public string AttachDateCode { get; set; }
        public string IsVirtualPathValid { get; set; }
        public string IsThumbnailVirtualPathValid { get; set; }
        public DateTime ValidatedDate { get; set; }
    }
}
