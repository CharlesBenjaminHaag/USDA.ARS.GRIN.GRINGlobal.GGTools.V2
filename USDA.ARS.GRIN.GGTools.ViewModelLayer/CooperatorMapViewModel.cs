using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CooperatorMapViewModel : CooperatorMapViewModelBase, IViewModel<CooperatorMap>
    {
        
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public CooperatorMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
            using (CooperatorMapManager mgr = new CooperatorMapManager())
            {
                try
                {
                    DataCollection = new Collection<CooperatorMap>(mgr.Search(SearchEntity));
                    if (DataCollection.Count() == 1)
                    {
                        Entity = DataCollection[0];
                    }

                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
