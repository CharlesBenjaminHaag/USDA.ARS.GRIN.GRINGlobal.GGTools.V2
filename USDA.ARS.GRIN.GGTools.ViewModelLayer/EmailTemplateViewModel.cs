using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class EmailTemplateViewModel: EmailTemplateViewModelBase, IViewModel<EmailTemplate>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public EmailTemplate Get(string categoryCode)
        {
            using (EmailTemplateManager mgr = new EmailTemplateManager())
            {
                try
                {
                    SearchEntity.CategoryCode = categoryCode;
                    Entity = new Collection<EmailTemplate>(mgr.Search(SearchEntity))[0];
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }

            return Entity;
        }

        public EmailTemplate Get(int entityId)
        {
            using (EmailTemplateManager mgr = new EmailTemplateManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    Entity = new Collection<EmailTemplate>(mgr.Search(SearchEntity))[0];
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
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
            using (EmailTemplateManager mgr = new EmailTemplateManager())
            {
                try
                {
                    DataCollection = new Collection<EmailTemplate>(mgr.Search(SearchEntity));

                    if (DataCollection.Count == 1)
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
            using (EmailTemplateManager mgr = new EmailTemplateManager())
            {
                try
                {
                    mgr.Update(Entity);
                    RowsAffected = mgr.RowsAffected;
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
