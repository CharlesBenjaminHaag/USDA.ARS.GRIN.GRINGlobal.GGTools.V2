using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class Folder : AppEntityBase
    {
        public string FolderName { get; set; }
        public string FolderType { get; set; }
        public string FolderTypeDescription { get; set; }
        public string Category { get; set; }
        public string AlternateCategory { get; set; }
        public string DataType { get; set; }
        public string Properties { get; set; }
        public string Description { get; set; }
        public bool IsFavoriteOption { get; set; }
        public string IsFavoriteCode { get; set; }
        public string IsFavorite { get; set; }
        public bool IsShared { get; set; }
        public int TotalItems { get; set; }
        public string QuerySQL { get; set; }
        public int ItemCount { get; set; }
    }
}
