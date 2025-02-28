using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AccessionMCPD : AppEntityBase
    {
       
       public int INTERNAL_ID { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string INSTCODE { get; set; }
        public string DOI { get; set; }
        public string ACCENUMB { get; set; }
        public string SPECIES_FULL { get; set; }
        public string GENUS { get; set; }
        public string SPECIES { get; set; }
        public string SPAUTHOR { get; set; }
        public string SUBTAXA { get; set; }
        public string SUBTAUTHOR { get; set; }
        public string ACCEURL { get; set; }
        public string SAMPSTAT { get; set; }
        public string REMARKS { get; set; }
        public string ACQDATE { get; set; }
        public string HISTORIC { get; set; }
        public string COLLSITE { get; set; }
        public string GEOREFMETH { get; set; }
        public int COORDUNCERT { get; set; }
        public decimal DECLATITUDE { get; set; }
        public decimal DECLONGITUDE { get; set; }
        public string ORIGCTY { get; set; }
    }
}
