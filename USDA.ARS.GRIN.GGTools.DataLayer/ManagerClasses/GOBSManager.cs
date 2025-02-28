using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GRINGlobal.DTO;



namespace USDA.ARS.GRIN.GGTools.GOBS.DataLayer
{
    public class GOBSManager : GRINGlobalDataManagerBase
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        #region Dataset

        public Dataset GetDataset(int cooperatorId, int datasetId)
        {
            SQL = "gobs.get_all_datasets";

            Dataset dataset = new Dataset();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false)
            };

            dataset = GetRecord<Dataset>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            dataset.DatasetValues = GetDatasetValues(cooperatorId, datasetId);
            dataset.DatasetMarkers = GetDatasetMarkers(cooperatorId, dataset.dataset_id);
            dataset.DatasetMarkerValues = GetDatasetMarkerValues(cooperatorId, datasetId);
            dataset.DatasetInventories = GetDatasetInventories(cooperatorId, datasetId);
            dataset.ReportValues = GetReportValuesByDataset(cooperatorId, dataset.dataset_id);
            //rpt traits
            return dataset;
        }

        public List<Dataset> GetDatasets(int cooperatorId)
        {
            SQL = "gobs.get_all_datasets";

            List<Dataset> datasets = new List<Dataset>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false)
            };

            datasets = GetRecords<Dataset>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            foreach (var dataset in datasets)
            {
                dataset.DatasetValues = GetDatasetValues(cooperatorId, dataset.dataset_id);
                dataset.DatasetMarkers = GetDatasetMarkers(cooperatorId, dataset.dataset_id);
                dataset.DatasetMarkerValues = GetDatasetMarkerValues(cooperatorId, dataset.dataset_id);
                dataset.DatasetInventories = GetDatasetInventories(cooperatorId, dataset.dataset_id);
                dataset.ReportValues = GetReportValuesByDataset(cooperatorId, dataset.dataset_id);
            }
            return datasets;
        }

        public int DeleteDataset(GOBSDataset entity)
        {
            throw new NotImplementedException();
        }

        public int InsertDataset(Dataset entity)
        {
            throw new NotImplementedException();
        }

        public List<GOBSDataset> SearchDatasets(GOBSDatasetSearch searchEntity)
        {
            SQL = "usp_GRINGlobal_GOBSDataset_Search";
            List<GOBSDataset> GOBSDatasets = new List<GOBSDataset>();



            return GOBSDatasets;
        }

        public int UpdateDataset(Dataset entity)
        {


            return RowsAffected;
        }

        #endregion Dataset

        #region Dataset Value

        public List<DatasetValue> GetDatasetValues (int cooperatorId, int datasetId)
        {
            SQL = "gobs.usp_get_dataset_values";

            List<DatasetValue> datasetValues = new List<DatasetValue>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("dataset_id", (object)datasetId, false),
            };

            datasetValues = GetRecords<DatasetValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return datasetValues;
        }

        #endregion Dataset Value

        #region Dataset Marker

        public List<DatasetMarker> GetDatasetMarkers(int cooperatorId, int datasetId)
        {
            SQL = "gobs.get_all_dataset_markers";

            List<DatasetMarker> datasetMarkers = new List<DatasetMarker>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("dataset_id", (object)datasetId, false),
            };

            datasetMarkers = GetRecords<DatasetMarker>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            foreach (var datasetMarker in datasetMarkers)
            {
                datasetMarker.DatasetMarkerValues = GetDatasetMarkerValues(cooperatorId, datasetMarker.marker_id);
            }

            return datasetMarkers;
        }

        public DatasetMarker GetDatasetMarker(int cooperatorId, int datasetMarkerId)
        {
            SQL = "gobs.usp_get_dataset_marker";

            DatasetMarker datasetMarker = new DatasetMarker();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("dataset_marker_id", (object)datasetMarkerId, false),
            };

            datasetMarker = GetRecord<DatasetMarker>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return datasetMarker;
        }

        #endregion Dataset Marker

        #region Dataset Marker Value

        public List<DatasetMarkerValue> GetDatasetMarkerValues(int cooperatorId, int datasetMarkerId = 0, int datasetMarkerFieldId = 0)
        {
            SQL = "gobs.get_all_dataset_marker_values";

            List<DatasetMarkerValue> datasetMarkerValues = new List<DatasetMarkerValue>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("marker_id", (object)datasetMarkerId, false),
            };

            datasetMarkerValues = GetRecords < DatasetMarkerValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return datasetMarkerValues;
        }

        #endregion Dataset Marker Value

        #region Dataset Attachment
        
        public GOBSDatasetAttachment GetDatasetAttachment(int entityId)
        {
            GOBSDatasetAttachment entity = new GOBSDatasetAttachment();
            SQL = "SELECT * FROM gobs.get_dataset_attach WHERE dataset_attach_id = @EntityID";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("EntityID", (object)entityId, false)
            };

            entity = GetRecord<GOBSDatasetAttachment>(SQL, CommandType.Text, parameters.ToArray());

            return entity;
        }
        #endregion Dataset Attachment

        #region Dataset Field
        
        public GOBSDatasetField GetDatasetField(int entityId)
        {
            GOBSDatasetField entity = new GOBSDatasetField();
            SQL = "SELECT * FROM gobs.get_dataset_field WHERE dataset_field_id = @EntityID";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("EntityID", (object)entityId, false)
            };

            entity = GetRecord<GOBSDatasetField>(SQL, CommandType.Text, parameters.ToArray());

            return entity;
        }

        public List<Dataset> GetDatasetFields(int cooperatorId)
        {
            SQL = "gobs.get_all_dataset_Fields";

            List<Dataset> datasetFields = new List<Dataset>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("dataset_id", (object)cooperatorId, false)
            };

            datasetFields = GetRecords<Dataset>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return datasetFields;
        }

        #endregion Dataset Field

        #region Dataset Inventory

        public GOBSDatasetInventory GetDatasetInventory(int entityId)
        {
            GOBSDatasetInventory entity = new GOBSDatasetInventory();
            SQL = "SELECT * FROM gobs.get_dataset_inventory WHERE dataset_inventory_id = @EntityID";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("EntityID", (object)entityId, false)
            };

            entity = GetRecord<GOBSDatasetInventory>(SQL, CommandType.Text, parameters.ToArray());

            return entity;
        }

        public List<DatasetInventory> GetDatasetInventories(int cooperatorId, int datasetId)
        {
            SQL = "gobs.usp_get_dataset_inventories";

            List<DatasetInventory> datasetInventories = new List<DatasetInventory>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("dataset_id", (object)datasetId, false),
            };

            datasetInventories = GetRecords<DatasetInventory>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return datasetInventories;
        }

        #endregion Dataset Inventory

        #region Report Value

        public List<ReportValue> GetReportValuesByDataset(int cooperatorId, int datasetId)
        {
            SQL = "gobs.get_all_report_values_by_dataset";

            List<ReportValue> reportValues = new List<ReportValue>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("dataset_id", (object)datasetId, false),
            };

            reportValues = GetRecords<ReportValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            return reportValues;
        }

        #endregion Report Value
    }
}
