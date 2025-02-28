using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class ISTASeed: AppEntityBase
    {
        public string DisplayLetter { get; set; }
        public string DisplayName { get; set; }
        public string Rank { get; set; }
        public string DisplayNameURL { get; set; }
        public string GenusName{ get; set; }
        public int SpeciesID { get; set; }
        public int AcceptedID { get; set; }
        public string ISTASpeciesName { get; set; }
        public string ISTAAcceptedName { get; set; }
        public string SpeciesName{ get; set; }
        public string SubspeciesName { get; set; }
        public string VarietyName { get; set; }
        public string Authority { get; set; }
        public string SpeciesAuthority { get; set; }
        public string SubspeciesAuthority { get; set; }
        public string VarietyAuthority { get; set; }
        public int FamilyID { get; set; }
        public int FamilyID2 { get; set; }
        public string FamilyName { get; set; }
        public string FamilyAlternateName { get; set; }
        public string FamilyName2 { get; set; }
        public int GenusID { get; set; }
        public string NameStatus { get; set; }
        public string URL { get; set; }
        public string Comment { get; set; }
        public int UPOVCropMapID { get; set; }
        public int UPOVCodeID { get; set; }
        public int UPOVCropID { get; set; }
        public string UPOVCode { get; set; }

        public Species AcceptedSpecies { get; set; }

        public ISTASeed()
        { 
            AcceptedSpecies = new Species();
        }
    }
}
