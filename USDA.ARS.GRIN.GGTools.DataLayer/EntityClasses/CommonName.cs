using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CommonName: CitedAppEntityBase
    {
        public int SpeciesID { get; set; }
        [AllowHtml]
        public string SpeciesName { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public int LanguageID { get; set; }
        public string LanguageDescription { get; set; }
        public string Name { get; set; }
        public string SimplifiedName { get; set; }
        public string AlternateTranscription { get; set; }
        public string CategoryCode { get; set; }
        public Collection<Citation> Citations { get; set; }
    }
}
