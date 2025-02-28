using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer.EntityClasses
{
    public class Method : AppEntityBase
    {
        public string Name { get; set; }
        public int GeographyID { get; set; }
        public string ElevationMeters { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Uncertainty { get; set; }
        public string FormattedLocality { get; set; }
        public string GeoreferenceDatum { get; set; }
        public string GeoreferenceProtocolCode { get; set; }
        public string GeoreferenceAnnotation { get; set; }
        public string MaterialsAndMethods { get; set; }
        public string StudyReasonCode { get; set; }
    }
}
