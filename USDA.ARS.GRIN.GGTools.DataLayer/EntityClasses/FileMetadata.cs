using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class FileMetaData
    {
        public string SourceURL { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public DateTime LastModified { get; set; }
        public string ResultMessage { get; set; }
        public string ResultCode { get; set; }
    }
}
