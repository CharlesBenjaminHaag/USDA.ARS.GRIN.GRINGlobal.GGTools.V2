using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class InventoryViewModel : InventoryViewModelBase, IViewModel<InventoryViewModel>
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
            //using (AccessionManager mgr = new AccessionManager())
            //{
            //    try
            //    {
            //        DataCollection = new Collection<AccessionMCPD>(mgr.Search(SearchEntity));

            //        if (DataCollection.Count == 1)
            //        {
            //            Entity = DataCollection[0];
            //        }
            //        RowsAffected = mgr.RowsAffected;
            //    }
            //    catch (Exception ex)
            //    {
            //        PublishException(ex);
            //        throw (ex);
            //    }
            //}
        }

        public int Update()
        {
            throw new NotImplementedException();
        }

        void IViewModel<InventoryViewModel>.Delete()
        {
            throw new NotImplementedException();
        }

        InventoryViewModel IViewModel<InventoryViewModel>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        int IViewModel<InventoryViewModel>.Insert()
        {
            throw new NotImplementedException();
        }

        void IViewModel<InventoryViewModel>.Search()
        {
            throw new NotImplementedException();
        }

        int IViewModel<InventoryViewModel>.Update()
        {
            throw new NotImplementedException();
        }
    }
}
