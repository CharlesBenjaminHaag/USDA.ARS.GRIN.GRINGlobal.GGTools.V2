using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesSearch: SearchEntityBase
    {
        public int AcceptedNameID { get; set; }
        public int NomenNumber { get; set; }
        public bool IsAccepted { get; set; }
        public string Rank { get; set; }
        public string IsSpecificHybrid { get; set; }
        //public bool IsSpecificHybridOption { get; set; }
        public string SpeciesName { get; set; }
        public string SpeciesEpithet { get; set; }
        public string FullName { get; set; }
        public string IsAcceptedName { get; set; }
        public string Tags { get; set; }
        public int TagMapID { get; set; }
        public string NameAuthority { get; set; }
        public string SpeciesAuthority { get; set; }
        public string IsSubspecificHybrid { get; set; }
        //public bool IsSubSpecificHybridOption { get; set; }
        public string SubspeciesName { get; set; }
        public string SubspeciesAuthority { get; set; }
        public string IsVarietalHybrid { get; set; }
        public bool IsVarietalHybridOption { get; set; }
        public string VarietyName { get; set; }
        public string VarietyAuthority { get; set; }
        public string IsSubVarietalHybrid { get; set; }
        //public bool IsSubvarietalHybridOption { get; set; }
        public string SubvarietyName { get; set; }
        public string SubvarietyAuthority { get; set; }
        public string IsFormaHybrid { get; set; }
        //public bool IsFormaHybridOption { get; set; }
        public string FormaRankType { get; set; }
        public string FormaName { get; set; }
        public string FormaAuthority { get; set; }
        public string InfraspecificType { get; set; }
        public string Protologue { get; set; }
        public bool ProtologueIsNull { get; set; }
        public string ProtologueVirtualPath { get; set; }
        public bool ProtologueVirtualPathIsNull { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public string GenusHybridCode { get; set; }
        public string SubGenusName { get; set; }
        public string FamilyName { get; set; }
        public string IsNamePending { get; set; }
        public string SynonymCode { get; set; }
        public string SynonymDescription { get; set; }
        public int VerifiedByCooperatorID { get; set; }
        public string VerifiedByCooperatorName { get; set; }
        public DateTime? NameVerifiedDate { get; set; }
        public DateTime NameVerifiedDateFrom { get; set; }
        public DateTime NameVerifiedDateTo { get; set; }
        public string IsVerified { get; set; }
        public bool IsNameVerifiedDateOption { get; set; }
        public string Name { get; set; }
        
        public string AlternateName { get; set; }
        public int AccessionCount { get; set; }
        public string IsLinkedToAccessions { get; set; }
        public string CommonFertilizationCode { get; set; }
        public string LifeFormCode { get; set; }
        public int Priority1SiteID { get; set; }
        public int Priority2SiteID { get; set; }
        public string RestrictionCode { get; set; }
        public string HybridParentage { get; set; }
        public string IsWebVisible { get; set; }

        public string GetSynonyms { get; set; }
        public string GetConspecific { get; set; }
        public string GetCommonNames { get; set; }
        public string GetCitations { get; set; }
    }
}
