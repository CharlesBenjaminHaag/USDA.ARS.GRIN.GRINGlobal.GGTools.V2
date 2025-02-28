using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesSynonymMap : AppEntityBase
    {
        public int SpeciesAID { get; set; }
        public string SpeciesAAssembledName { get; set; }
        public string SpeciesAName { get; set; }
        public string SpeciesAAuthority { get; set; }
        public string SpeciesAIsAcceptedName { get; set; }
        public string SpeciesAAcceptedName { get; set; }
        public string SpeciesAProtologue { get; set; }
        public string SpeciesAVerifiedByCooperatorName { get; set; }
        public DateTime SpeciesANameVerifiedDate { get; set; }
        public int SpeciesBID { get; set; }
        public string SpeciesBAssembledName { get; set; }
        public string SpeciesBName { get; set; }
        public string SpeciesBIsAcceptedName { get; set; }
        public string SpeciesBAuthority { get; set; }
        public string SpeciesBAcceptedName { get; set; }
        public string SpeciesBProtologue { get; set; }
        public string SpeciesBVerifiedByCooperatorName { get; set; }
        public DateTime SpeciesBNameVerifiedDate { get; set; }

        public string SynonymCode { get; set; }
        public string SynonymDescription { get; set; }
    }
}
