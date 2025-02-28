using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public interface IViewModel<T>
    {
        void Search();
        T Get(int entityId);
        int Insert();
        int Update();
        void Delete();
    }
}
