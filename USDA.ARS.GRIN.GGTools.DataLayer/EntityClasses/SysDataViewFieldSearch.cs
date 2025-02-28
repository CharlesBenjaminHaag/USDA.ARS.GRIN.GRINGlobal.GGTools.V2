using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysDataViewFieldSearch : SearchEntityBase
    {
        public int SysDataViewID { get; set; }
        public string DataViewName { get; set; }
        public string DataViewTitle { get; set; }
        public string FieldName { get; set; }
        public int SysTableFieldID { get; set; }
        public string FieldTitle { get; set; }
        public string IsReadOnly { get; set; }
        public string IsPrimaryKey { get; set; }
        public string IsTransform { get; set; }
        public int SortOrder { get; set; }
        public string GUIHint { get; set; }
        public string ForeignKeyDataViewName { get; set; }
        public string GroupName { get; set; }
        public string TableAliasName { get; set; }
        public string IsVisible { get; set; }
        public string ConfigurationOptions { get; set; }
    }
}
