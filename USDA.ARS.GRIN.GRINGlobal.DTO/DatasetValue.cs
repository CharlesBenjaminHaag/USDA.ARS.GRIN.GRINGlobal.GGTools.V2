using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GRINGlobal.DTO
{
    public class DatasetValue: DTOBase
    {
        public int dataset_value_id { get; set; }
        public int method_id { get; set; }
        public string method { get; set; }
        public int dataset_id { get; set; }
        public string dataset { get; set; }
        public int dataset_field_id { get; set; }
        public string field { get; set; }
        public string value { get; set; }
        public DateTime valid_from { get; set; }
    }
}
