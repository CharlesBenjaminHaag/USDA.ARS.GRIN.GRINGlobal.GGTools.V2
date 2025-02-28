using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysTableField
    {
        public int ID { get; set; }
        public int SysTableID { get; set; }
        public string  SysTableName  { get; set; }
        public string SysTableTitle { get; set; }
        public string DatabaseAreaCode { get; set; }
        public string FieldName { get; set; }
        public string FieldTitle { get; set; }
        public int FieldOrdinal { get; set; }
        public string FieldPurpose { get; set; }
        public string FieldType { get; set; }
        public string DefaultValue { get; set; }
        public string IsPrimaryKey { get; set; }
        public string IsForeignKey { get; set; }
        public int ForeignKeyTableFieldID { get; set; }
        public string ForeignKeyDataviewName { get; set; }
        public string IsNullable { get; set; }
        public string GUIHint { get; set; }
        public string IsReadOnly { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public int NumericPrecision { get; set; }
        public int NumericScale { get; set; }
        public string IsAutoIncrement { get; set; }
        public string GroupName { get; set; }
    }
}
