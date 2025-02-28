using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class AppUserItemFolderSearch : SearchEntityBase
    {
        public int EntityID { get; set; }
        public string FolderName { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string FolderType { get; set; }
        public string IsFavorite { get; set; }
        public string IsShared { get; set; }
        public string TimeFrame { get; set; }
        public int SharedWithCooperatorID { get; set; }
    }
}