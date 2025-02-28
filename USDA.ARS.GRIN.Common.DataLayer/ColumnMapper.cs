using System.Reflection;

namespace USDA.ARS.GRIN.Common.DataLayer
{
  public class ColumnMapper
  {
    public string ColumnName { get; set; }
    public PropertyInfo ColumnProperty { get; set; }
  }
}
