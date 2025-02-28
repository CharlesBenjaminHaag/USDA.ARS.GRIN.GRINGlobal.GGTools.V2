using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class WebCooperatorViewModelBase : AuthenticatedViewModelBase
    {
        private int _CooperatorID;
        private WebCooperator _Entity = new WebCooperator();
        private WebCooperatorSearch _SearchEntity = new WebCooperatorSearch();
        private Collection<WebCooperator> _DataCollection = new Collection<WebCooperator>();
        private Collection<WebUserShippingAddress> _DataCollectionWebUserShippingAddress = new Collection<WebUserShippingAddress>();

        public WebCooperatorViewModelBase()
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                Salutations = new SelectList(mgr.GetCodeValues("COOPERATOR_TITLE"), "Value", "Title");
                Categories = new SelectList(mgr.GetCodeValues("COOPERATOR_CATEGORY"), "Value", "Title");
                States = new SelectList(mgr.GetStates(), "ID", "Admin1");
            }
        }
        
        public int CooperatorID
        {
            get { return _CooperatorID; }
            set { _CooperatorID = value; }
        }
        
        public WebCooperator Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public WebCooperatorSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<WebCooperator> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        
        public Collection<WebUserShippingAddress> DataCollectionWebUserShippingAddress
        {
            get { return _DataCollectionWebUserShippingAddress; }
            set { _DataCollectionWebUserShippingAddress = value; }
        }

        #region Select Lists
        public SelectList States { get; set; }
        public SelectList Salutations { get; set; }
        public SelectList Categories { get; set; }
        #endregion
    }
}
