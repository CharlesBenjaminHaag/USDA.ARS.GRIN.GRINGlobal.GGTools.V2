using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class OrderRequestAttachment : AppEntityBase
    {
        public int OrderRequestID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public string CategoryCode { get; set; }
        public string IsWebVisible { get; set; }
        public string VirtualPath { get; set; }
        public string ThumbnailVirtualPath { get; set; }
    }
}
