using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class Report
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string SQL { get; set; }
        public List<Species> ResultSet { get; set; } 
    }
}
