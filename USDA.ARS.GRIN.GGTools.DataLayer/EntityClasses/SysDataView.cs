using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysDataView : AppEntityBase
    {
        public string DataViewName { get; set; }
        public string Title { get; set; }
        public string IsEnabled { get; set; }
        public string IsReadOnly { get; set; }
        public string CategoryCode { get; set; }
        public string DatabaseAreaCode { get; set; }
        public int DatabaseAreaCodeSortOrder { get; set; }
        public string ConfigurationOptions { get; set; }

        public List<SysDataViewField> SysDataViewFields { get; set; }

        public SysDataView()
        {
            SysDataViewFields = new List<SysDataViewField>();
        }
    }
}
