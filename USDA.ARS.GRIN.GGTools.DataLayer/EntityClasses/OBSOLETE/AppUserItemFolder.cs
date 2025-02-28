using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer.OBSOLETE
{
    public partial class AppUserItemFolder : AppEntityBase
    {
        public string FolderName { get; set; }
        public string FolderType { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string DataType { get; set; }
        public string DataTypeDescription { get; set; }
        public int TotalItems { get; set; }
        public string NewCategory { get; set; }
        public string IsFavorite { get; set; }
        public bool IsFavoriteOption { get; set; }
        public string IsShared { get; set; }
    }
}