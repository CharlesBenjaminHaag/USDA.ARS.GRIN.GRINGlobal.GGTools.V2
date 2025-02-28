using System;
using System.Text;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.DataLayer.SqlServerClasses;

namespace USDA.ARS.GRIN.GGTools.AppLayer
{
    /// <summary>
    /// Use this class to add any standard properties used in all entity classes
    /// </summary>
    public class AppEntityBase : SqlServerEntityBase
    {
        public string EntityKey { get; set; }
        public string TableName { get; set; }
        public int FolderID { get; set; }
        //public int AppUserItemListID { get; set; }
        public int SysFolderItemMapID { get; set; }
        public int ParentID { get; set; }
        [AllowHtml]
        public string ParentName { get; set; }
        [AllowHtml]
        public string AssembledName { get; set; }
        public int ID { get; set; }
        public string ItemIDList { get; set; }
        public int AcceptedID { get; set; }
        public string AcceptedName { get; set; }
        public int CitationID { get; set; }
        [AllowHtml]
        public string Note { get; set; }
        public string IsWebVisible { get; set; }
        public bool IsWebVisibleOption { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByCooperatorID { get; set; }
        public string CreatedByCooperatorName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedByCooperatorID { get; set; }
        public string ModifiedByCooperatorName { get; set; }
        public DateTime OwnedDate { get; set; }
        public int OwnedByCooperatorID { get; set; }
        public string OwnedByCooperatorName { get; set; }
        public string OwnedByCooperatorEmailAddress { get; set; }
        public int RelatedItemCount { get; set; }
        public string RevisionHistoryText
        {
            get
            {
                StringBuilder sbRevisionHistoryText = new StringBuilder();
                sbRevisionHistoryText.Append("<strong>");
                sbRevisionHistoryText.Append("Created by ");
                sbRevisionHistoryText.Append("</strong>");
                sbRevisionHistoryText.Append(CreatedByCooperatorName);
                sbRevisionHistoryText.Append(" on ");
                sbRevisionHistoryText.Append(CreatedDate.ToShortDateString());
                sbRevisionHistoryText.Append(" at ");
                sbRevisionHistoryText.Append(CreatedDate.ToShortTimeString());

                if (ModifiedDate != DateTime.MinValue && ModifiedByCooperatorID > 0)
                {
                    sbRevisionHistoryText.Append(", <strong>");
                    sbRevisionHistoryText.Append("Last Modified by ");
                    sbRevisionHistoryText.Append("</strong>");
                    sbRevisionHistoryText.Append(ModifiedByCooperatorName);
                    sbRevisionHistoryText.Append(" on ");
                    sbRevisionHistoryText.Append(ModifiedDate.ToShortDateString());
                    sbRevisionHistoryText.Append(" at ");
                    sbRevisionHistoryText.Append(ModifiedDate.ToShortTimeString());
                }
                return sbRevisionHistoryText.ToString();
            }
        }
    }        
}
