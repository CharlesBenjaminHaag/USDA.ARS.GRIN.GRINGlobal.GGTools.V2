using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public interface IManager<T, U>
    {
        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        T Get(int entityId);
        List<T> Search(U searchEntity);
        void BuildInsertUpdateParameters();
    }
}
