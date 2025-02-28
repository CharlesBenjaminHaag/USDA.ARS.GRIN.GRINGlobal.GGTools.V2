using System;
using System.Web.Mvc; 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class EmailTemplate: AppEntityBase
    {
        public string CategoryCode { get; set; }
        public string Title { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string[] Recipients { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string IsHtml { get; set; }
    }
}
