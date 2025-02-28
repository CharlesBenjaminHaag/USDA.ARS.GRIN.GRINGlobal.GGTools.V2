using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.GOBS.DataLayer;
using USDA.ARS.GRIN.GRINGlobal.DTO;

namespace USDA.ARS.GRIN.GGTools.GOBS.ViewModelLayer
{
    public class GOBSViewModelBase : AppViewModelBase
    {
        private Dataset _DatasetEntity = new Dataset();
        private DatasetField _DatasetFieldEntity = new DatasetField();
        private DatasetAttach _DatasetAttachEntity = new DatasetAttach();
        private DatasetInventory _DatasetInventoryEntity = new DatasetInventory();
        private DatasetMarker _DatasetMarkerEntity = new DatasetMarker();
        private DatasetMarkerField _DatasetMarkerFieldEntity = new DatasetMarkerField();

        private Collection<Dataset> _DataCollectionDatasets = new Collection<Dataset>();
        private Collection<GOBSDataset> _DataCollection = new Collection<GOBSDataset>();
        private DataTable _DataCollectionDataTable = new DataTable();

        public GOBSViewModelBase()
        {
        }
       
        public Dataset DatasetEntity
        {
            get { return _DatasetEntity; }
            set { _DatasetEntity = value; }
        }

        public DatasetAttach DatasetAttachEntity
        {
            get { return _DatasetAttachEntity; }
            set { _DatasetAttachEntity = value; }
        }

        public DatasetField DatasetFieldEntity
        {
            get { return _DatasetFieldEntity; }
            set { _DatasetFieldEntity = value; }
        }

        public DatasetInventory DatasetInventoryEntity
        {
            get { return _DatasetInventoryEntity; }
            set { _DatasetInventoryEntity = value; }
        }

        public DatasetMarker DatasetMarkerEntity
        {
            get { return _DatasetMarkerEntity; }
            set { _DatasetMarkerEntity = value; }
        }
       
        public Collection<Dataset> DataCollectionDatasets
        {
            get { return _DataCollectionDatasets; }
            set { _DataCollectionDatasets = value; }
        }

       //public GOBSMarker Marker
       // {
       //     get { return _Marker; }
       //     set { _Marker = value; }
       // }
    }
}
