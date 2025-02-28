using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class OrderRequestViewModelBase : AppViewModelBase
    {
        private OrderRequest _Entity = new OrderRequest();
        private OrderRequestSearch _SearchEntity = new OrderRequestSearch();
        private Collection<OrderRequest> _DataCollection = new Collection<OrderRequest>();
        private Collection<OrderRequestItem> _DataCollectionItems = new Collection<OrderRequestItem>();
        private Collection<OrderRequestAction> _DataCollectionActions = new Collection<OrderRequestAction>();
        private Collection<OrderRequestAttachment> _DataCollectionAttachments = new Collection<OrderRequestAttachment>();
        private Collection<OrderRequestPhytoLog> _DataCollectionPhytoLog = new Collection<OrderRequestPhytoLog>();

        public OrderRequest Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public OrderRequestSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<OrderRequest> DataCollection
        { 
            get { return _DataCollection; } 
            set { _DataCollection = value; } 
        }

        public Collection<OrderRequestItem> DataCollectionItems
        {
            get { return _DataCollectionItems; }
            set { _DataCollectionItems = value; }
        }

        public Collection<OrderRequestAction> DataCollectionAction
        {
            get { return _DataCollectionActions; }
            set { _DataCollectionActions = value; }
        }

        public Collection<OrderRequestAttachment> DataCollectionAttachments
        {
            get { return _DataCollectionAttachments; }
            set { _DataCollectionAttachments = value; }
        }

        public Collection<OrderRequestPhytoLog> DataCollectionPhytoLog
        {
            get { return _DataCollectionPhytoLog; }
            set { _DataCollectionPhytoLog = value; }
        }

        #region Select Lists

        public SelectList Statuses { get; set; }

        public SelectList IntendedUseCodes { get; set; }

        #endregion
    }
}
