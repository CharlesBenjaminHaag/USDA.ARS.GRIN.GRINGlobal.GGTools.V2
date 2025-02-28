using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SiteViewModel : SiteViewModelBase, IViewModel<Site>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Site Get(int entityId)
        {
            using (SiteManager mgr = new SiteManager())
            {
                try
                {
                    Entity = mgr.Get(entityId);
                    Entity.IsInternalOption =   ToBool(Entity.IsInternal);
                    Entity.IsDistributionSiteOption = ToBool(Entity.IsDistributionSite);
                    RowsAffected = mgr.RowsAffected;
                    return Entity;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void GetSiteActiveUsers(int siteId)
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                try
                {
                    DataCollectionSiteCooperators = new Collection<Cooperator>(mgr.GetSiteActiveUsers(siteId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
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
            using (SiteManager mgr = new SiteManager())
            {
                try
                {
                    DataCollection = new Collection<Site>(mgr.Search(SearchEntity));
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
            using (SiteManager mgr = new SiteManager())
            {
                try
                {
                    Entity.IsInternal = FromBool(Entity.IsInternalOption);
                    Entity.IsDistributionSite = FromBool(Entity.IsDistributionSiteOption);
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;

        }
    }
}
