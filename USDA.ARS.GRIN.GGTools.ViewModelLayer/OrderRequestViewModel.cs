using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.OrderManagement.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.OrderManagement.DataLayer.ManagerClasses;
using System.Web.UI;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class OrderRequestViewModel : OrderRequestViewModelBase, IViewModel<OrderRequest>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Get(int entityId)
        {
            try
            {
                using (OrderRequestManager mgr = new OrderRequestManager())
                {
                    Entity = mgr.Get(entityId);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw (ex);
            }
        }

        public List<OrderRequest> GetAll() {  throw new NotImplementedException(); }

        public void GetItems(int orderRequestId)
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            { 
               DataCollectionItems = new Collection<OrderRequestItem>(mgr.GetItems(orderRequestId));
            }
        }

        public void GetActions(int orderRequestId)
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                DataCollectionAction = new Collection<OrderRequestAction>(mgr.GetActions(orderRequestId));
            }
        }

        public void GetAttachments(int orderRequestId)
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                DataCollectionAttachments = new Collection<OrderRequestAttachment>(mgr.GetAttachments(orderRequestId));
            }
        }

        public void GetPhytoLog(int orderRequestId)
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                DataCollectionPhytoLog = new Collection<OrderRequestPhytoLog>(mgr.GetPhytoLog(orderRequestId));
            }
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                try
                {
                    DataCollection = new Collection<OrderRequest>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public List<OrderRequest> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }

        OrderRequest IViewModel<OrderRequest>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
