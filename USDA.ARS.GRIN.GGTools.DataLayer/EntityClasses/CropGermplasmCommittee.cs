using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class CropGermplasmCommittee: AppEntityBase
    {
        public string Name { get; set; }
        public string RosterURL { get; set; }
    }
}
