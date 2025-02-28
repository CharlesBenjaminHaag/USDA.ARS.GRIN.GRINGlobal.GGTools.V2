using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using System.Linq;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class WebOrderRequestViewModelBase : AppViewModelBase
    {
        private int _AuthenticatedUserWebUserID = 0;
        private int _AssignedWebUserID = 0;
        private string _WebCooperatorVettedStatusCode = String.Empty;
        private bool _SendRequestorNotification = true;
        private bool _SendInternalNotification = true;
        private string _ActionEmailCategory = String.Empty;
        private string _ActionEmailFrom = String.Empty;
        private string _ActionEmailTo = String.Empty;
        private string _ActionEmailBCC = String.Empty;
        private string _ActionEmailSubject = String.Empty;
        private string _ActionEmailBody = String.Empty;
        private string _ActionEmailBodyOriginal = String.Empty;

        private WebOrderRequest _Entity = new WebOrderRequest();
        private WebOrderRequestSearch _SearchEntity = new WebOrderRequestSearch();
        private Collection<WebOrderRequest> _DataCollection = new Collection<WebOrderRequest>();
        private Collection<WebOrderRequestItem> _DataCollectionItems = new Collection<WebOrderRequestItem>();
        private Collection<WebOrderRequestAction> _DataCollectionActions = new Collection<WebOrderRequestAction>();
        private Collection<WebOrderRequestAction> _DataCollectionNotes = new Collection<WebOrderRequestAction>();

        private Collection<WebOrderRequestActionGroup> _DataCollectionActionGroups = new Collection<WebOrderRequestActionGroup>();
        private Collection<CodeValue> _DataCollectionStatusCodes = new Collection<CodeValue>();
        private Collection<CodeValue> _DataCollectionIntendedUseCodes = new Collection<CodeValue>();
        private Collection<ReportItem> _DataCollectionReportItems = new Collection<ReportItem>();
        private Collection<EmailTemplate> _DataCollectionEmailTemplates = new Collection<EmailTemplate>();

        public WebOrderRequestViewModelBase()
        {
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                DataCollectionStatusCodes = new Collection<CodeValue>(mgr.GetCodeValues("WEB_ORDER_INTENDED_USE"));
                DataCollectionIntendedUseCodes = new Collection<CodeValue>(mgr.GetCodeValues("WEB_ORDER_REQUEST_STATUS"));

                Countries = new SelectList(mgr.GetCodeValues("GEOGRAPHY_COUNTRY_CODE"), "Value", "Title");
                Cooperators = new SelectList(mgr.GetWebCooperators(), "ID", "FullName");
                TimeFrameOptions = new SelectList(mgr.GetTimeFrameOptions(), "Value", "Title");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                IntendedUseCodes = new SelectList(mgr.GetCodeValues("WEB_ORDER_INTENDED_USE"), "Value", "Title");
                Statuses = new SelectList(mgr.GetCodeValues("WEB_ORDER_REQUEST_STATUS"), "Value", "Title");
                WebOrderRequestActions = new SelectList(mgr.GetCodeValues("WEB_ORDER_REQUEST_ACTION").Where(x =>!x.Value.Contains("FLAG")), "Value", "Title");
                WebOrderRequestStatuses = new SelectList(mgr.GetCodeValues("WEB_ORDER_REQUEST_STATUS"), "Value", "Title");
            }
            ActionEmailFrom = "gringlobal.orders@usda.gov";           
        }

        
        public WebOrderRequest Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public WebOrderRequestSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<WebOrderRequest> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<WebOrderRequestItem> DataCollectionItems
        {
            get { return _DataCollectionItems; }
            set { _DataCollectionItems = value; }
        }
        
        public Collection<WebOrderRequestAction> DataCollectionActions
        {
            get { return _DataCollectionActions; }
            set { _DataCollectionActions = value; }
        }
        
        public Collection<WebOrderRequestAction> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }
        
        public Collection<WebOrderRequestActionGroup> DataCollectionActionGroups
        {
            get { return _DataCollectionActionGroups; }
            set { _DataCollectionActionGroups = value; }
        }

        public Collection<CodeValue> DataCollectionStatusCodes
        {
            get { return _DataCollectionStatusCodes; }
            set { _DataCollectionStatusCodes = value; }
        }

        public Collection<CodeValue> DataCollectionIntendedUseCodes
        {
            get { return _DataCollectionIntendedUseCodes; }
            set { _DataCollectionIntendedUseCodes = value; }
        }

        public Collection<ReportItem> DataCollectionReportItems
        {
            get { return _DataCollectionReportItems; }
            set { _DataCollectionReportItems = value; }
        }

        public Collection<EmailTemplate> DataCollectionEmailTemplates
        {
            get { return _DataCollectionEmailTemplates; }
            set { _DataCollectionEmailTemplates = value; }
        }

        public string WebCooperatorVettedStatusCode
        {
            get { return _WebCooperatorVettedStatusCode; }
            set { _WebCooperatorVettedStatusCode = value; }
        }

        public int AuthenticatedUserWebUserID
        {
            get { return _AuthenticatedUserWebUserID; }
            set { _AuthenticatedUserWebUserID = value; }
        }

        public int AssignedWebUserID
        {
            get { return _AssignedWebUserID; }
            set { _AssignedWebUserID = value; }
        }

        public bool SendInternalNotification
        {
            get { return _SendInternalNotification; }
            set { _SendInternalNotification = value; }
        }

        public bool SendRequestorNotification
        {
            get { return _SendRequestorNotification; }
            set { _SendRequestorNotification = value; }
        }

        public string ActionEmailCategory
        { get { return _ActionEmailCategory; }
            set { _ActionEmailCategory = value; }
        }
        
        public string ActionEmailFrom
        { get { return _ActionEmailFrom; }
            set { _ActionEmailFrom = value; }
        }
        
        public string ActionEmailTo
        {
            get { return _ActionEmailTo; }
            set { _ActionEmailTo = value; }
        }
        
        public string ActionEmailBCC
        {
            get { return _ActionEmailBCC; }
            set { _ActionEmailBCC = value; }
        }
        
        public string ActionEmailSubject
        {
            get { return _ActionEmailSubject; }
            set { _ActionEmailSubject = value; }
        }
        
        [AllowHtml]
        public string ActionEmailBody
        {
            get { return _ActionEmailBody; }
            set { _ActionEmailBody = value; }
        }

        [AllowHtml]
        public string ActionEmailBodyOriginal
        {
            get { return _ActionEmailBodyOriginal; }
            set { _ActionEmailBodyOriginal = value; }
        }

        #region Select Lists

        public SelectList Statuses { get; set; }
        
        public SelectList IntendedUseCodes { get; set; }
        
        public SelectList WebOrderRequestStatuses { get; set; }

        public SelectList WebOrderRequestActions { get; set; }

        public SelectList Countries { get; set; }

        #endregion
    }
}
