using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AppUserItemList : AppEntityBase
    {
        public int AppUserItemFolderID { get; set; }
        public string IsImported { get; set; }
        public int CooperatorID { get; set; }
        public int SortOrder { get; set; }
        public string TabName  { get; set; }
        public string ItemTitle { get; set; }
        public string ListName  { get; set; }
        public string Category { get; set; }
        public int IDNumber { get; set; }
        public string IDType { get; set; }
        public int EntityID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Properties { get; set; }
        public AppUserItemFolder AppUserItemFolder { get; set; }
    }
}
