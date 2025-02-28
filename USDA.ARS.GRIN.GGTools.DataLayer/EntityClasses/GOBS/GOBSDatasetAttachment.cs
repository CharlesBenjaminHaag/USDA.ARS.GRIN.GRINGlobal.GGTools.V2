using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class GOBSDatasetAttachment: AppEntityBase
    {
        public int GOBSDatasetID { get; set; }
        public string InventoryText { get; set; }
        public string VirtualPath { get; set; }
        public string ThumbnailVirtualPath { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionCode { get; set; }
        public string ContentType { get; set; }
        public string CategoryCode { get; set; }
        public string CopyrightInformation { get; set; }
        public int CooperatorID { get; set; }
        public DateTime AttachDate { get; set; }
        public string AttachDateCode { get; set; }
    }
}
