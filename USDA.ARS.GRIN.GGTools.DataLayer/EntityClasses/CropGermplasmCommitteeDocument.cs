using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class CropGermplasmCommitteeDocument : AppEntityBase
    {
        public int CropGermplasmCommitteeID { get; set; }
        public string CommitteeName { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryDescription { get; set; }
    }
}
