using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class ImportViewModel : AppViewModelBase
    {
        public string ImportFileName { get; set; }
        public HttpPostedFileBase DocumentUpload { get; set; }
        public DataTable DataCollectionDataTable { get; set; }
        public Collection<CodeValue> DataCollectionFields { get; set; }
         
        public string SysTableName { get; set; }
        public SelectList SysTables { get; set; }
        
        public bool LoadSpeciesOnly { get; set; }
        
        public ImportViewModel()
        {
            DataCollectionDataTable = new DataTable();
            DataCollectionFields = new Collection<CodeValue>();
        }

        public SysTableField GetColumnInfo(string sysTableName, string sysTableFieldName)
        {
            SysTableField sysTableField = new SysTableField();

            using (SysTableManager mgr = new SysTableManager())
            {
                sysTableField = mgr.GetSysTableField(sysTableName, sysTableFieldName);
            }
            return sysTableField;
        }

        public override bool Validate()
        {
            bool validated = true;

            if (DocumentUpload == null)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please select a correctly-formatted Excel (.xls, .xlx) file to upload." });
            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }
    }
}
