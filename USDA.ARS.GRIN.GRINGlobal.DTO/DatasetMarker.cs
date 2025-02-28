using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GRINGlobal.DTO
{
    public class DatasetMarker: DTOBase
    {
        public int dataset_marker_id { get; set; }
        public int method_id { get; set; }
        public string method { get; set; }
        public int dataset_id { get; set; }
        public string dataset { get; set; }
        public int marker_id { get; set; }
        public string marker { get; set; }
        public bool is_trait { get; set; }
        public DateTime valid_from { get; set; }

        public List<DatasetMarkerValue> DatasetMarkerValues { get; set; }
}
}
