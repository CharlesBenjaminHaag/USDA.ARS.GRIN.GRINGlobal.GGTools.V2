using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CooperatorOwnershipViewModel: ViewModelBase
    {
        private int _DonorCooperatorID;
        private int _RecipientCooperatorID;
        private string _SysTableNameList;
        private Collection<ReportItem> _DataCollectionSourceCooperatorRecords = new Collection<ReportItem>();
        private Collection<ReportItem> _DataCollectionTargetCooperatorRecords = new Collection<ReportItem>();

        public CooperatorOwnershipViewModel()
        {
            
        }

        public int DonorCooperatorID 
        {
            get { return _DonorCooperatorID; }
            set { _DonorCooperatorID = value; }
        }

        public int RecipientCooperatorID
        {
            get { return _RecipientCooperatorID; }
            set { _RecipientCooperatorID = value; }
        }

        public string SysTableNameList
        {
            get { return _SysTableNameList; }
            set { _SysTableNameList = value; }
        }

        public SelectList SourceCooperators { get; set; }
        
        public SelectList TargetCooperators { get; set; }

        public Collection<ReportItem> DataCollectionSourceCooperatorRecords
        {
            get { return _DataCollectionSourceCooperatorRecords; }
            set { _DataCollectionSourceCooperatorRecords = value; }
        }
        
        public Collection<ReportItem> DataCollectionTargetCooperatorRecords
        {
            get { return _DataCollectionTargetCooperatorRecords; }
            set { _DataCollectionTargetCooperatorRecords = value; }
        }
        
        public void GetSiteCooperators(int siteId)
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                SourceCooperators = new SelectList(mgr.Search(new CooperatorSearch { SiteID = siteId }), "ID", "FullName");
                TargetCooperators = new SelectList(mgr.Search(new CooperatorSearch { SiteID = siteId }), "ID", "FullName");
            }
        }

        public int Transfer()
        {
            string[] sysTableNameArray;
            //TODO iterate through table list and call transfer for each 
            using (CooperatorManager mgr = new CooperatorManager())
            {
                sysTableNameArray = SysTableNameList.Split(',');
                foreach (var tableName in sysTableNameArray)
                {
                    mgr.TransferOwnership(DonorCooperatorID, RecipientCooperatorID, tableName);
                }
                
            }
            return sysTableNameArray.Length;
        }
    }
}
