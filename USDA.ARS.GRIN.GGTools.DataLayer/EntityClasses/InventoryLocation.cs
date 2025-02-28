using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class InventoryLocation : AppEntityBase
    {
        public int InventoryID { get; set; }
        public string DataDateCode { get; set; }
        public DateTime DataDate { get; set; }
        public int DataDateNumber { get; set; }
        public int LocationNumber { get; set; }
        public string LocationString { get; set; }
        public int SubLocationNumber { get; set; }
        public string SubLocationString { get; set; }
        public string DataType1Code { get; set; }
        public string DataType2Code { get; set; }
        public string DataType3Code { get; set; }
        public string DataType4Code { get; set; }
        public decimal NumericValue { get; set; }
        public string StringValue { get; set; }
        public string GridLocationCode { get; set; }
        public int GridLocationX { get; set; }
        public int GridLocationY { get; set; }
        public int CooperatorID { get; set; }
        public string CooperatorFullName { get; set; }
        public int MethodID { get; set; }
        public string MethodName { get; set; }
    }
}
