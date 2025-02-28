using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer.OBSOLETE
{
    public class AppUserItemListSearch : SearchEntityBase
    {
        public int AppUserItemFolderID { get; set; }
        public string IsImported { get; set; }
        public int CooperatorID { get; set; }
        public string TabName  { get; set; }
        public string ListName  { get; set; }
        public string Category { get; set; }
        public int IDNumber { get; set; }
        public string IDType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Properties { get; set; }
    }
}
