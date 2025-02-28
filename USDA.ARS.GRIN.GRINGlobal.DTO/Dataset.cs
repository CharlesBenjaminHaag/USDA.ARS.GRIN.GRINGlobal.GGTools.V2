using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GRINGlobal.DTO
{
    public class Dataset: DTOBase
    {
        public int dataset_id { get; set; }
        public int method_id { get; set; }
        public string method { get; set; }
        public int type_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public DateTime valid_from { get; set; }

        public List<DatasetValue> DatasetValues { get; set; }
        public List<DatasetMarker> DatasetMarkers { get; set; }
        public List<DatasetMarkerValue> DatasetMarkerValues { get; set; }
        public List<DatasetInventory> DatasetInventories { get; set; }
        public List<ReportValue> ReportValues { get; set; }
        public List<ReportTrait> ReportTraits { get; set; }
    }
}
