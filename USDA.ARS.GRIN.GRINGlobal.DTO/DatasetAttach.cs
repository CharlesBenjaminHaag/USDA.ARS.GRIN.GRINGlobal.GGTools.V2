using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace USDA.ARS.GRIN.GRINGlobal.DTO
{
    public class DatasetAttach
    {
        public string dataset_attach_id { get; set; }
          public string dataset_id { get; set; }
          public string virtual_path { get; set; }
          public string thumbnail_virtual_path { get; set; }
          public string sort_order { get; set; }
          public string title { get; set; }
          public string description { get; set; }
          public string description_code { get; set; }
          public string content_type { get; set; }
          public string category_code { get; set; }
          public string copyright_information { get; set; }
          public string attach_cooperator_id { get; set; }
          public string is_web_visible { get; set; }
          public string attach_date { get; set; }
          public string attach_date_code { get; set; }
          public string note { get; set; }
          public string owner_id { get; set; }
          public string invalidator_id { get; set; }
          public string valid_from { get; set; }
          public string valid_to { get; set; }
    }
}
