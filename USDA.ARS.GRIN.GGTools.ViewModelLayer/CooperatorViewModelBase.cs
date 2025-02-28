using System;
using System.Web;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.Library.Email;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer.EntityClasses;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CooperatorViewModelBase : AuthenticatedViewModelBase
    {
        private int _TotalRecordsOwned;
        private string _ResetPasswordField;
        private string _ResetPasswordConfirmField;
        private Cooperator _Entity = new Cooperator();
        private CooperatorStatus _StatusEntity = new CooperatorStatus();
        private SysUser _SysUserEntity = new SysUser();
        private CooperatorSearch _SearchEntity = new CooperatorSearch();
        private Collection<Cooperator> _DataCollection = new Collection<Cooperator>();
        private Collection<SysGroup> _DataCollectionGroups = new Collection<SysGroup>();
        private Collection<Folder> _DataCollectionFolders = new Collection<Folder>();
        private Collection<ReportItem> _DataCollectionReportItems = new Collection<ReportItem>();
        private Collection<AppUserGUISetting> _DataCollectionAppUserGUISettings = new Collection<AppUserGUISetting>();
        private SMTPMailMessage _EmailMessage = new SMTPMailMessage();
       
        public CooperatorViewModelBase()
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                TimeFrameOptions = new SelectList(mgr.GetTimeFrameOptions(), "Value", "Title");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                States = new SelectList(mgr.GetStates(), "ID", "Admin1");
                Sites = new SelectList(mgr.GetSites(), "ID", "AssembledName");
                Disciplines = new SelectList(mgr.GetCodeValues("COOPERATOR_DISCIPLINE"), "Value", "Title");
                Categories = new SelectList(mgr.GetCodeValues("COOPERATOR_CATEGORY"), "Value", "Title");
                Salutations = new SelectList(mgr.GetCodeValues("COOPERATOR_TITLE"), "Value", "Title");
                //Regions = new SelectList(mgr.GetCodeValues("ORGANIZATION_REGION"), "Value", "Title");
                Statuses = new SelectList(mgr.GetCodeValues("COOPERATOR_STATUS"), "Value", "Title");
                //Cooperators =  new SelectList(mgr.Search(new CooperatorSearch { SysUserIsEnabled = "Y" }), "ID", "FullName");
            }
        }
  
        public int TotalRecordsOwned
        {
            get { return _TotalRecordsOwned; }
            set { _TotalRecordsOwned = value; }
        }
        public string ResetPasswordField {
            get { return _ResetPasswordField; }
            set { _ResetPasswordField = value; } 
        }
        public string ResetPasswordConfirmField
        {
            get { return _ResetPasswordConfirmField; }
            set { _ResetPasswordConfirmField = value; }
        }

        public Cooperator Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public CooperatorStatus StatusEntity
        {
            get { return _StatusEntity; }
            set { _StatusEntity = value; }
        }
        public SysUser SysUserEntity
        {
            get { return _SysUserEntity; }
            set { _SysUserEntity = value; }
        }

        public CooperatorSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Cooperator> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<SysGroup> DataCollectionGroups
        {
            get { return _DataCollectionGroups; }
            set { _DataCollectionGroups = value; }
        }
        public Collection<Folder> DataCollectionFolders
        {
            get { return _DataCollectionFolders; }
            set { _DataCollectionFolders = value; }
        }

        public Collection<ReportItem> DataCollectionReportItems
        {
            get { return _DataCollectionReportItems; }
            set { _DataCollectionReportItems = value; }
        }

        public Collection<AppUserGUISetting> DataCollectionAppUserGUISettings
        {
            get { return _DataCollectionAppUserGUISettings; }
            set { _DataCollectionAppUserGUISettings = value; }
        }

        public SMTPMailMessage EmailMessage
        {
            get { return _EmailMessage; }
            set { _EmailMessage = value; }
        }

        public string IsReadOnly
        {
            get
            {
                if ((AuthenticatedUser.IsInRole("MANAGE_COOPERATOR")) ||
                    (AuthenticatedUser.IsInRole("ADMINS") ||
                    (AuthenticatedUser.CooperatorID == Entity.ID)
                    ))
                {
                    return "N";
                }
                else
                {
                    return "Y";
                }
            }
        }

        #region Select Lists

        public SelectList States { get; set; }
        public SelectList Organizations { get; set; }
        public SelectList Sites { get; set; }
        public SelectList Disciplines { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Salutations { get; set; }
        public SelectList Regions { get; set; }
        public SelectList Statuses { get; set; }

        #endregion
    }
}
