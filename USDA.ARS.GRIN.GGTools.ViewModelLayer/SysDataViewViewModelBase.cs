using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysDataViewViewModelBase : AppViewModelBase
    {
        private string _SQL = String.Empty;
        private SysDataView _Entity = new SysDataView();
        private SysDataViewSearch _SearchEntity = new SysDataViewSearch();
        private Collection<CodeValue> _DataCollectionDatabaseAreaCodes = new Collection<CodeValue>();
        private Collection<SysDataView> _DataCollection = new Collection<SysDataView>();
        private Collection<SysDataViewField> _DataCollectionFields = new Collection<SysDataViewField>();
        private Collection<SysDataViewParameter> _DataCollectionParameters = new Collection<SysDataViewParameter>();
        private DataTable _DataCollectionDataTable = new DataTable();
        public List<SysDataViewParameter> _DataCollectionEditableParameters = new List<SysDataViewParameter>();

        public SysDataViewViewModelBase() 
        { 
            using (CodeValueManager mgr  = new CodeValueManager()) 
            {
                DatabaseAreaCodes = new SelectList(mgr.GetCodeValues("DATAVIEW_DATABASE_AREA"), "Value", "Title");
            }
        }

        public string SQL
        {
            get { return _SQL; }
            set { _SQL = value; }
        }

        public SysDataView Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        
        public SysDataViewSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<CodeValue> DataCollectionDatabaseAreaCodes
        {
            get { return _DataCollectionDatabaseAreaCodes; }
            set { _DataCollectionDatabaseAreaCodes = value; }
        }

        public Collection<SysDataView> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<SysDataViewField> DataCollectionFields
        {
            get { return _DataCollectionFields; }
            set { _DataCollectionFields = value; }
        }

        public Collection<SysDataViewParameter> DataCollectionParameters
        {
            get { return _DataCollectionParameters; }
            set { _DataCollectionParameters = value; }
        }

        public DataTable DataCollectionDataTable
        {
            get { return _DataCollectionDataTable; }
            set { _DataCollectionDataTable = value; }
        }

        public SelectList DatabaseAreaCodes
        { get; set; }
    }
}
