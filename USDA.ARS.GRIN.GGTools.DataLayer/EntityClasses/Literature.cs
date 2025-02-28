using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class Literature : AppEntityBase
    {
        public string Abbreviation { get; set; }
        public string StandardAbbreviation { get; set; }
        public string ReferenceTitle { get; set; }
        public string EditorAuthorName { get; set; }
        public string LiteratureTypeCode { get; set; }
        public string PublicationYear { get; set; }
        public string PublisherName { get; set; }
        public string PublisherLocation { get; set; }
        public string URL { get; set; }
    }
}
