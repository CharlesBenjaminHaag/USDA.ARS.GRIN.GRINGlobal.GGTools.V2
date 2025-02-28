using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GRINGlobal.DTO
{
    public class DatasetMarkerField
    {
        public int dataset_id { get; set; }
        public int method_id { get; set; }
        public string method { get; set; }
        public int type_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public DateTime valid_from { get; set; }
    }
}
