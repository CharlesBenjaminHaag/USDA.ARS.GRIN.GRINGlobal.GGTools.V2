using System;
using System.Configuration;
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
using System.Xml.Linq;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class WebOrderRequestViewModel : WebOrderRequestViewModelBase
    {
        public string NewActionCode { get; set; }
        public string SelectedFilterTimeFrame { get; set; }
        public string SelectedFilterCurrentStatus { get; set; }
        public string SelectedFilterMostRecentAction { get; set; }
        public string SelectedFilterAssignedTo { get; set; }
        public string EmailBodyOriginal { get; set; }
        public bool IsEmailRequested { get; set; }
        public bool IsBCCRequested { get; set; }

        public WebOrderRequestViewModel()
        {

        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public WebOrderRequest Get(int entityId)
        {
            try { 
                using (WebOrderRequestManager mgr = new WebOrderRequestManager())
                {
                    Entity = mgr.Get(entityId);
                   
                    CodeValue codeValue = mgr.GetWebOrderRequestEmailAddresses(entityId);
                    Entity.EmailAddressList = codeValue.Title;

                    RowsAffected = mgr.RowsAffected; 
                    DataCollection.Add(Entity);
                }
                using (EmailTemplateManager emailTemplateMgr = new EmailTemplateManager())
                {
                    DataCollectionEmailTemplates = new Collection<EmailTemplate>(emailTemplateMgr.Search(new EmailTemplateSearch()));
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw (ex);
            }

            return Entity;
        }
        
        public void GetWebOrderRequestItems(int entityId)
        {
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                DataCollectionItems = new Collection<WebOrderRequestItem>(mgr.GetWebOrderRequestItems(entityId));
            }
        }
        
        public void GetWebOrderRequestActions()
        {
            try 
            {
                using (WebOrderRequestManager mgr = new WebOrderRequestManager())
                {
                    DataCollectionActions = new Collection<WebOrderRequestAction>(mgr.GetWebOrderRequestActions(SearchEntity.ID));

                    // Get web order request actions, grouped by day
                    var queryWebOrderRequestActionDates =
                    from action in DataCollectionActions
                    group action by action.ActionDate into webOrderRequestActionGroup
                    orderby webOrderRequestActionGroup.Key descending
                    select webOrderRequestActionGroup;

                    foreach (var group in queryWebOrderRequestActionDates)
                    {
                        WebOrderRequestActionGroup webOrderRequestActionGroup = new WebOrderRequestActionGroup { DateGroup = DateTime.Parse(group.Key.ToString()) };
                        foreach (var subGroup in group)
                        {
                            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
                            webOrderRequestAction.ID = subGroup.ID;
                            webOrderRequestAction.ActionCode = subGroup.ActionCode;
                            webOrderRequestAction.ActionTitle = subGroup.ActionTitle;
                            webOrderRequestAction.ActionDescription = subGroup.ActionDescription;
                            webOrderRequestAction.Note = subGroup.Note;
                            webOrderRequestAction.CreatedDate = subGroup.CreatedDate;
                            webOrderRequestAction.CreatedByCooperatorID = subGroup.CreatedByCooperatorID;
                            webOrderRequestAction.CreatedByCooperatorName = subGroup.CreatedByCooperatorName;
                            webOrderRequestActionGroup.WebOrderRequestActions.Add(webOrderRequestAction);
                        }
                        DataCollectionActionGroups.Add(webOrderRequestActionGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetNotes()
        {
            try
            {
                using (WebOrderRequestManager mgr = new WebOrderRequestManager())
                {
                    DataCollectionNotes = new Collection<WebOrderRequestAction>(mgr.GetWebOrderRequestActions(SearchEntity.ID).Where(x=>x.ActionCode == "NRR_NOTE").ToList());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmailTemplate GetEmailTemplate(string typeCode)
        {
            EmailTemplate emailTemplate = new EmailTemplate();
            try
            {
                using (EmailTemplateManager mgr = new EmailTemplateManager())
                {
                    EmailTemplateSearch emailTemplateSearch = new EmailTemplateSearch();
                    emailTemplateSearch.CategoryCode = typeCode;
                    DataCollectionEmailTemplates = new Collection<EmailTemplate>(mgr.Search(emailTemplateSearch));
                    if (DataCollectionEmailTemplates.Count == 1)
                    {
                        emailTemplate = DataCollectionEmailTemplates[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return emailTemplate;
        }

        public void SendEmail(string emailTo = "")
        {
            try
            {
                SMTPManager sMTPManager = new SMTPManager();
                SMTPMailMessage sMTPMailMessage = new SMTPMailMessage();
                sMTPMailMessage.To = ActionEmailTo;
                sMTPMailMessage.CC = ActionEmailBCC;
                sMTPMailMessage.From = ActionEmailFrom;
                sMTPMailMessage.Subject = ActionEmailSubject;
                sMTPMailMessage.Body = ActionEmailBody;
                sMTPManager.SendMessage(sMTPMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertWebOrderRequestActionNote(int webOrderRequestId, string actionNote, int webUserId)
        {
            int rowsAffected = 0;

            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
            webOrderRequestAction.WebOrderRequestID = webOrderRequestId;
            webOrderRequestAction.ActionCode = "NRR_NOTE";
            webOrderRequestAction.CreatedByWebUserID = webUserId;
            webOrderRequestAction.Note = actionNote;

            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                rowsAffected = mgr.InsertWebOrderRequestAction(webOrderRequestAction);
            }
            return rowsAffected;
        }
        
        public void InsertWebOrderRequestAction(WebOrderRequestAction webOrderRequestAction)
        {
            int rowsAffected = 0;

            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                rowsAffected = mgr.InsertWebOrderRequestAction(webOrderRequestAction);
            }
        }
        
        public void Search()
        {
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                try
                {
                    DataCollection = new Collection<WebOrderRequest>(mgr.Search(SearchEntity));

                    //DEBUG
                    List<int> distinctIDs = DataCollection
                    .Select(obj => obj.ID) // Select the ID attribute
                    .Distinct()            // Get distinct values
                    .ToList();

                    ItemIDList = string.Join(",", distinctIDs);

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

        public void SearchWebOrderRequestItems()
        {
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                try
                {
                    DataCollectionItems = new Collection<WebOrderRequestItem>(mgr.SearchWebOrderRequestItems(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void SearchWebOrderRequestActions()
        {
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                try
                {
                    DataCollectionActions = new Collection<WebOrderRequestAction>(mgr.SearchWebOrderRequestActions(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void Update()
        {
            // Update WOR
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                Entity.StatusCode = NewActionCode;
                Entity.Note = EventNote;
                Entity.EmailAddressList = ActionEmailTo;

                mgr.Update(Entity);

                // Add pertinent action
                if (!String.IsNullOrEmpty(NewActionCode))
                {
                    WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
                    webOrderRequestAction.WebOrderRequestID = Entity.ID;
                    webOrderRequestAction.ActionCode = NewActionCode;
                    webOrderRequestAction.Note = EventNote;
                    webOrderRequestAction.CreatedByWebUserID = Entity.WebUserID;
                    webOrderRequestAction.OwnedByWebUserID = Entity.WebUserID;
                    mgr.InsertWebOrderRequestAction(webOrderRequestAction);
                }
            }

            //TODO Send email
        }

        public void UpdateHold()
        {
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                Entity.StatusCode = NewActionCode;
                Entity.Note = EventNote;
                Entity.EmailAddressList = ActionEmailTo;
                //mgr.UpdateLock(Entity);
            }
        }

        public void UpdateLock()
        {
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                Entity.StatusCode = NewActionCode;
                Entity.Note = EventNote;
                Entity.EmailAddressList = ActionEmailTo;
                mgr.UpdateLock(Entity);
            }
        }
       
        public string GetCSSClass(string statusCode)
        {
            string cssClass = String.Empty;

            if (statusCode.Contains("NRR_FLAG"))
            {
                cssClass = "far fa-flag bg-red";
            }
            else
            {
                switch (statusCode)
                {
                    case "NRR_FLAGGED":
                        cssClass = "far fa-flag bg-yellow";
                        break;
                    case "NRR_NOTE":
                        cssClass = "far fa-comment bg-purple";
                        break;
                    case "NRR_REVIEW":
                        cssClass = "far fa-eye bg-blue";
                        break;
                    case "NRR_INFO":
                        cssClass = "far fa-envelope margin-r-5 bg-maroon";
                        break;
                    case "NRR_REJECT":
                        cssClass = "far fa-thumbs-down bg-red";
                        break;
                    case "NRR_APPROVE":
                        cssClass = "far fa-thumbs-up bg-green";
                        break;
                    case "NRR_HOLD":
                    case "NRR_HOLD_CTRY":
                        cssClass = "fas fa-lock bg-teal";
                        break;
                    case "NRR_HOLD_REL":
                    case "NRR_HOLD_CTRY_REL":
                        cssClass = "fas fa-lock-open bg-teal";
                        break;
                    default:
                        cssClass = "far fa-circle";
                        break;
                }
            }
            return cssClass;
        }
    }
}
