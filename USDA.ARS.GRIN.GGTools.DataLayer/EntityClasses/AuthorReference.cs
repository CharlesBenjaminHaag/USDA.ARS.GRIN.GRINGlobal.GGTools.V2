using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AuthorReference
    {
        public string TableTitle { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldTitle { get; set; }
        public int TaxonID { get; set; }
        public string TaxonAssembledName { get; set; }
        public string TaxonAuthority { get; set; }
    }
}
