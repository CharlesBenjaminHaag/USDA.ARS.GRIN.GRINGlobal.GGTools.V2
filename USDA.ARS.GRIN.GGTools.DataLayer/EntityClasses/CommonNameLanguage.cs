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
    public class CommonNameLanguage : AppEntityBase
    {
        public string LanguageName { get; set; }
        public string LanguageSimplifiedName { get; set; }
        public string LanguageTranscription { get; set; }
        public string CountryCode { get; set; }
        public string CountryDescription { get; set; }
    }
}
