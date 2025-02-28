using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class EmailTemplateViewModelBase : AppViewModelBase
    {
        private EmailTemplate _Entity = new EmailTemplate();
        private EmailTemplateSearch _SearchEntity = new EmailTemplateSearch();
        private Collection<EmailTemplate> _DataCollection = new Collection<EmailTemplate>();

        public EmailTemplate Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public EmailTemplateSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<EmailTemplate> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
    }
}
