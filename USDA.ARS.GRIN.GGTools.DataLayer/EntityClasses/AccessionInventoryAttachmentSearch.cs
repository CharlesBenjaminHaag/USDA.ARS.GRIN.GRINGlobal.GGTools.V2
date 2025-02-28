using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AccessionInventoryAttachmentSearch : SearchEntityBase
    {
        public int InventoryID { get; set; }
        public string InventoryText { get; set; }
        public string VirtualPath { get; set; }
        public string ThumbnailVirtualPath { get; set; }
        public string SortOrder { get; set; }
        public string Title { get; set; }
        public string AttachmentDescription { get; set; }
        public string AttachnmentDescriptionCode { get; set; }
        public string ContentType { get; set; }
        public string CategoryCode { get; set; }
        public string CopyrightInformation { get; set; }
        public string CooperatorID { get; set; }
        public string IsWebVisible { get; set; }
        public string IsVirtualPathValid { get; set; }
        public string IsThumbnailVirtualPathValid { get; set; }
        public string IsValidated { get; set; }
        public string AttachDate { get; set; }
        public string AttachDateCode { get; set; }
        public string TimeFrame { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
