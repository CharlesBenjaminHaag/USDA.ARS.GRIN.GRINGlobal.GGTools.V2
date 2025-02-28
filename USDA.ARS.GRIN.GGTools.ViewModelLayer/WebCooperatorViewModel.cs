using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class WebCooperatorViewModel : WebCooperatorViewModelBase, IViewModel<WebCooperator>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public WebCooperator Get(int entityId, int cooperatorId = 0)
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                if (entityId > 0)
                {
                    Entity = mgr.Get(entityId);
                }
                else
                {
                    if (cooperatorId > 0)
                    {
                        Entity = mgr.GetByCooperatorID(cooperatorId);
                    }
                }
            }
            return Entity;
        }

        public void GetWebUserShippingAddresses(int webUserId)
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                DataCollectionWebUserShippingAddress = new Collection<WebUserShippingAddress>(mgr.GetWebUserShippingAddresses(webUserId));
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                try
                {
                    Entity.ID = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }

        public void Search()
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                try
                {
                    DataCollection = new Collection<WebCooperator>(mgr.Search(SearchEntity));
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
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                try
                {
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }

        // Create a web cooperator based on an existing cooperator.
        public void Copy(int cooperatorId)
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                Entity.ID = mgr.Copy(cooperatorId);
            }
        }

        public List<WebCooperator> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public WebCooperator Get(int entityId)
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                Entity = mgr.Get(entityId);
            }
            return Entity;
        }
    }
}
