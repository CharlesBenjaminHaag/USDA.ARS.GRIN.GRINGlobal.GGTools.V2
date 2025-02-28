using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.Common.Library.BaseClasses;

namespace USDA.ARS.GRIN.Common.DataLayer.SqlServerClasses
{
  public class SqlServerEntityBase : EntityBase
  {
    [NotMapped]
    public int RETURN_VALUE { get; set; }
  }
}
