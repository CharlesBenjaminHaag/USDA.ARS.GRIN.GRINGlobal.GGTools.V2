using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USDA.ARS.GRIN.Common.Library.Email
{
    public class SMTPMailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }
}
