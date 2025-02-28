using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GRINGlobal.DTO
{
    public class DTOBase
    {
        public int controller_id { get; set; }
        public string controller_name { get; set; }
        public int authorized { get; set; }
    }
}
