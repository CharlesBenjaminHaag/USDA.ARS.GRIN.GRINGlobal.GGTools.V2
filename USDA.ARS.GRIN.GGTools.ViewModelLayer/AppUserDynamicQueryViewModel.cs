using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserDynamicQueryViewModel : AppUserDynamicQueryViewModelBase, IViewModel<AppUserDynamicQuery>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public AppUserDynamicQuery Get(int entityId)
        {
            using (AppUserDynamicQueryManager mgr = new AppUserDynamicQueryManager())
            {
                SearchEntity.ID = entityId;
                Search();
                if (DataCollection.Count == 1)
                {
                    Entity = DataCollection[0];
                }
            }
            return Entity;
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
            try
            {
                using (AppUserDynamicQueryManager mgr = new AppUserDynamicQueryManager())
                {
                    DataCollection = new Collection<AppUserDynamicQuery>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
