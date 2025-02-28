using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GRINGlobal.DTO
{
    public class ReportTrait: DTOBase
    {
        public int method_id { get; set; }
        public string method { get; set; }
        public int dataset_id { get; set; }
        public string dataset { get; set; }
        public int marker_id { get; set; }
        public string marker { get; set; }
        public int inventory_id { get; set; }
        public string inventory { get; set; }
        public int report_value_id { get; set; }
        public int individual { get; set; }
        public string value { get; set; }
    }
}
