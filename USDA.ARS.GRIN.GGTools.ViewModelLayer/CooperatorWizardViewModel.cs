using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CooperatorWizardViewModel : CooperatorViewModelBase
    {
        public int CooperatorID { get; set; }
        public string StatusCode { get; set; }
        public int SysUserID { get; set; }
        public int SysGroupUserMapCount { get; set; }
        public int WebCooperatorID { get; set; }
        public int WebUserID { get; set; }
        public int AuthenticatedUserCooperatorSiteID { get; set; }

        public void SendNotification()
        {
            SMTPManager sMTPManager = new SMTPManager();
            SMTPMailMessage infoRequestEmailMessage = new SMTPMailMessage();
            infoRequestEmailMessage.From = "gringlobal-support@usda.gov";
            infoRequestEmailMessage.To = Entity.EmailAddress;
            infoRequestEmailMessage.Subject = "Your GRIN-Global CT Account Password Has Been Reset";

            infoRequestEmailMessage.Body = "Please store your new password securely. It will expire on " + SysUserEntity.SysUserPasswordExpirationDate.ToShortDateString() + ".";
            infoRequestEmailMessage.Body += "<br/>";

            infoRequestEmailMessage.Body += "<table>";
            infoRequestEmailMessage.Body += "<tr>";
            infoRequestEmailMessage.Body += "<td><strong>Username</strong></td><td>" + SysUserEntity.SysUserName + "</td>";
            infoRequestEmailMessage.Body += "</tr>";
            infoRequestEmailMessage.Body += "<tr>";
            infoRequestEmailMessage.Body += "<td><strong>New Password</strong></td><td>" + SysUserEntity.SysUserPlainTextPassword + "</td>";
            infoRequestEmailMessage.Body += "</tr>";
            infoRequestEmailMessage.Body += "</table>";

            infoRequestEmailMessage.IsHtml = true;
            sMTPManager.SendMessage(infoRequestEmailMessage);
        }
    }
}
