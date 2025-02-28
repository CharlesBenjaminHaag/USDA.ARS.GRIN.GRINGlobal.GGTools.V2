using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AccessionInvAnnotationSearch : AppEntityBase
    {
        public string AnnotationTypeCode { get; set; }
        public DateTime AnnotationDate { get; set; }
        public string AccessionDateCode { get; set; }
        public int AnnotationCooperatorID { get; set; }
        public int InventoryID { get; set; }
        public int SpeciesID { get; set; }
        public int OldSpeciesID { get; set; }
    }
}
