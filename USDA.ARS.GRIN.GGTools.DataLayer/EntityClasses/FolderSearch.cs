using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class FolderSearch : AppEntityBase
    {
        public int EntityID { get; set; }
        public string Title { get; set; }
        public string FolderType { get; set; }
        public string FolderTypeDescription { get; set; }
        public string Category { get; set; }
        public bool IsShared { get; set; }
        public string IsFavorite { get; set; }
        public string CategoryList { get; set; }
        public string TypeList { get; set; }
        public int SharedWithCooperatorID { get; set; }
    }
}

