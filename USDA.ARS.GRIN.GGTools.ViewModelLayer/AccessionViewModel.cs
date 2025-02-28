using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AccessionViewModel : AccessionViewModelBase, IViewModel<AccessionViewModel>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public AccessionViewModel Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
            using (AccessionManager mgr = new AccessionManager())
            {
                try
                {
                    DataCollectionMCPD = new Collection<AccessionMCPD>(mgr.Search(SearchEntity));

                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }

        public void Export(int offset = 0, int limit = 0)
        {
            using(AccessionManager mgr = new AccessionManager())
            {
                try
                {
                    DataCollectionMCPD = new Collection<AccessionMCPD>(mgr.Export(offset, limit));

                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }

        public int Update()
        {
            throw new NotImplementedException();
        }

        public int UpdateBySpecies()
        {
            using (AccessionManager mgr = new AccessionManager())
            {
                return mgr.UpdateBySpecies(Entity);
            }
        }
    }
}
