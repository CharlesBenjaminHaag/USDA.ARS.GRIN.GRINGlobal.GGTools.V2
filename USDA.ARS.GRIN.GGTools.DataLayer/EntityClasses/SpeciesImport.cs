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
    public class SpeciesImport: AppEntityBase
    {
        [Column("Taxonomy Species ID")]
        public int SpeciesID { get; set; }
        [AllowHtml]
        public bool IsAccepted { get; set; }
        public string Rank { get; set; }
        public string IsSpecificHybrid { get; set; }
        public bool IsSpecificHybridOption { get; set; }
        [Required]
        public string SpeciesName { get; set; }
        public string OriginalSpeciesName { get; set; }

        public string SynonymName { get; set; }
        public string IsAcceptedName { get; set; }
        public string Tags { get; set; }
        public int TagMapID { get; set; }

        public string NameAuthority { get; set; }
        public string OriginalNameAuthority { get; set; }

        public string SpeciesAuthority { get; set; }
        public string OriginalSpeciesAuthority { get; set; }

        public string IsSubspecificHybrid { get; set; }
        public bool IsSubSpecificHybridOption { get; set; }
        public string SubspeciesName { get; set; }
        public string SubspeciesAuthority { get; set; }
        public string IsVarietalHybrid { get; set; }
        public bool IsVarietalHybridOption { get; set; }
        public string VarietyName { get; set; }
        public string VarietyAuthority { get; set; }
        [AllowHtml]
        public string HybridParentage { get; set; }
        public string IsSubVarietalHybrid { get; set; }
        public bool IsSubvarietalHybridOption { get; set; }
        public string SubvarietyName { get; set; }
        public string SubvarietyAuthority { get; set; }
        public string IsFormaHybrid { get; set; }
        public bool IsFormaHybridOption { get; set; }
        public string FormaRankType { get; set; }
        public string FormaName { get; set; }
        public string FormaAuthority { get; set; }
        public string InfraspecificType { get; set; }
        [AllowHtml]
        public string Protologue { get; set; }
        public string OriginalProtologue { get; set; }
        public int GenusID { get; set; }
        
        [AllowHtml]
        public string GenusName { get; set; }
        public string SubgenusName { get; set; }
        public string SectionName { get; set; }
        public string SubsectionName { get; set; }
        public string FamilyName { get; set; }
        public string SubfamilyName { get; set; }
        public string TribeName { get; set; }
        public string SubtribeName { get; set; }

        public string IsNamePending { get; set; }
        public string SynonymCode { get; set; }
        public string SynonymDescription { get; set; }
        public int VerifiedByCooperatorID { get; set; }
        public string VerifiedByCooperatorName { get; set; }
        public DateTime NameVerifiedDate { get; set; }
        public string Name { get; set; }
        
        public string ProtologueVirtualPath { get; set; }
        public string AlternateName { get; set; }
        public int AccessionCount { get; set; }
        public string IsVerified { get; set; } 

        public string VerificationText
        {
            get
            {
                StringBuilder sbVerificationText = new StringBuilder();

                if (NameVerifiedDate > DateTime.MinValue)
                {
                    sbVerificationText.Append("<strong>");
                    sbVerificationText.Append("Verified by ");
                    sbVerificationText.Append("</strong>");
                    sbVerificationText.Append(VerifiedByCooperatorName);
                    sbVerificationText.Append(" on ");
                    sbVerificationText.Append(NameVerifiedDate.ToShortDateString());
                }
                return sbVerificationText.ToString();
            }
        }
        public List<Citation> Citations { get; set; }
    }
}
