using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.Common.DataLayer
{
    public interface IAnnotated<T>
    {
        List<T> SearchNotes(string searchText);
    }
}
