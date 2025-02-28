using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AccessionInventoryAttachmentManager : GRINGlobalDataManagerBase, IManager<AccessionInventoryAttachment, AccessionInventoryAttachmentSearch>
    {
        public void BuildInsertUpdateParameters(AccessionInventoryAttachment entity)
        {
         //    int,
         //    int,
         //   @virtual_path nvarchar(255),
	        //@thumbnail_virtual_path nvarchar(255) ,
	        //@sort_order int,
         //   @title nvarchar(500) ,
	        //@description nvarchar(500) ,
	        //@description_code nvarchar(20) ,
	        //@content_type nvarchar(100) ,
	        //@category_code nvarchar(20) ,
	        //@copyright_information nvarchar(100) ,
	        //@attach_cooperator_id int ,
         //   @is_web_visible nvarchar(1) ,
	        //@attach_date datetime2(7) ,
	        //@attach_date_code nvarchar(20) ,
	        //@note nvarchar(max) ,
	        //@created_date datetime2(7) ,
	        //@created_by int ,
         //   @modified_date datetime2(7) ,
	        //@modified_by int ,
         //   @owned_date datetime2(7) ,
	        //@owned_by int ,
         //   @ nvarchar(1) ,
	        //@ nvarchar(1) ,
	        //@ datetime2(7)

            if (entity.ID > 0)
            {
                AddParameter("@accession_inv_attach_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("is_virtual_path_valid", (object)entity.IsVirtualPathValid ?? DBNull.Value, true);
            AddParameter("is_thumbnail_virtual_path_valid", (object)entity.IsThumbnailVirtualPathValid ?? DBNull.Value, true);
           
            //AddParameter("inventory_id", entity.InventoryID == 0 ? DBNull.Value : (object)entity.InventoryID, true);
            //AddParameter("is_specific_hybrid", entity.IsSpecificHybridOption == true ? "Y" : (object)"N", false);
            //AddParameter("name_authority", (object)entity.NameAuthority ?? DBNull.Value, true);
            //AddParameter("species_authority", (object)entity.SpeciesAuthority ?? DBNull.Value, true);
            //AddParameter("is_subspecific_hybrid", entity.IsSubspecificHybrid == null ? "N" : (object)entity.IsSubspecificHybrid, false);
            //AddParameter("subspecies_name", (object)entity.SubspeciesName ?? DBNull.Value, true);
            //AddParameter("subspecies_authority", (object)entity.SubspeciesAuthority ?? DBNull.Value, true);
            //AddParameter("is_varietal_hybrid", entity.IsVarietalHybrid == null ? "N" : (object)entity.IsVarietalHybrid, false);
            //AddParameter("variety_name", (object)entity.VarietyName ?? DBNull.Value, true);
            //AddParameter("variety_authority", (object)entity.VarietyAuthority ?? DBNull.Value, true);
            //AddParameter("is_subvarietal_hybrid", entity.IsSubVarietalHybrid == null ? "N" : (object)entity.IsSubVarietalHybrid, false);
            //AddParameter("subvariety_name", (object)entity.SubvarietyName ?? DBNull.Value, true);
            //AddParameter("subvariety_authority", (object)entity.SubvarietyAuthority ?? DBNull.Value, true);
            //AddParameter("is_forma_hybrid", entity.IsFormaHybrid == null ? "N" : (object)entity.IsFormaHybrid, false);
            //AddParameter("forma_rank_type", (object)entity.FormaRankType ?? DBNull.Value, true);
            //AddParameter("forma_name", (object)entity.FormaName ?? DBNull.Value, true);
            //AddParameter("forma_authority", (object)entity.FormaAuthority ?? DBNull.Value, true);
            //AddParameter("taxonomy_genus_id", (object)entity.GenusID ?? DBNull.Value, true);
            //AddParameter("synonym_code", (object)entity.SynonymCode ?? DBNull.Value, true);
            //AddParameter("verifier_cooperator_id", entity.VerifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.VerifiedByCooperatorID, true);

            //if (entity.NameVerifiedDate == DateTime.MinValue)
            //    AddParameter("name_verified_date", DBNull.Value, true);
            //else
            //    AddParameter("name_verified_date", (object)entity.NameVerifiedDate, true);

            //AddParameter("species_name", (object)entity.SpeciesName ?? DBNull.Value, true);
            //AddParameter("protologue", (object)entity.Protologue ?? DBNull.Value, true);
            //AddParameter("protologue_virtual_path", (object)entity.ProtologueVirtualPath ?? DBNull.Value, true);
            //AddParameter("note", (object)entity.Note ?? DBNull.Value, true);

            //if (entity.ID > 0)
            //{
            //    AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            //}
            //else
            //{
            //    AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            //}
        }

        public int Delete(AccessionInventoryAttachment entity)
        {
            throw new NotImplementedException();
        }

        public AccessionInventoryAttachment Get(int entityId)
        {
            AccessionInventoryAttachment result = new AccessionInventoryAttachment();

            //SQL = "usp_GGTools_GRINGlobal_AccessionInventoryAttachment_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("accession_inv_attach_id", entityId > 0 ? (object)entityId : DBNull.Value, true),
            };

            result = GetRecord<AccessionInventoryAttachment>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return result;
        }

        public int Insert(AccessionInventoryAttachment entity)
        {
            throw new NotImplementedException();
        }

        public int InsertValidation(AccessionInventoryAttachment entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<AccessionInventoryAttachment>(entity);

            //SQL = "usp_GGTools_GRINGlobal_AccessionInventoryAttachmentValidation_Insert";

            AddParameter("accession_inv_attach_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            AddParameter("is_virtual_path_valid", String.IsNullOrEmpty(entity.IsVirtualPathValid) ? DBNull.Value : (object)entity.IsVirtualPathValid, true);
            AddParameter("is_thumbnail_virtual_path_valid", String.IsNullOrEmpty(entity.IsThumbnailVirtualPathValid) ? DBNull.Value : (object)entity.IsThumbnailVirtualPathValid, true);
            AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_accession_inv_attach_validation_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            
            RowsAffected = ExecuteNonQuery();
            entity.ID = GetParameterValue<int>("@out_accession_inv_attach_validation_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }

        public List<AccessionInventoryAttachment> Search(AccessionInventoryAttachmentSearch searchEntity)
        {
            List<AccessionInventoryAttachment> results = new List<AccessionInventoryAttachment>();

            //SQL = "SELECT * FROM vw_GGTools_GRINGlobal_AccessionInventory_Attachments ";
            //SQL += " WHERE  (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID        =     @CreatedByCooperatorID)";
            //SQL += " AND    (@OwnedByCooperatorID           IS NULL OR OwnedByCooperatorID          =     @OwnedByCooperatorID)";
            //SQL += " AND    (@InventoryText                 IS NULL OR InventoryText                LIKE  '%' + @InventoryText + '%')";
            //SQL += " AND    (@Title                         IS NULL OR Title                        =     @Title)";
            //SQL += " AND    (@AttachmentDescription         IS NULL OR AttachmentDescription        =     @AttachmentDescription)";
            //SQL += " AND    (@AttachmentDescriptionCode     IS NULL OR AttachmentDescriptionCode    =     @AttachmentDescriptionCode)";
            //SQL += " AND    (@ContentType                   IS NULL OR ContentType                  =     @ContentType)";
            //SQL += " AND    (@CategoryCode                  IS NULL OR CategoryCode                 =     @CategoryCode)";
            //SQL += " AND    (@IsWebVisible                  IS NULL OR IsWebVisible                 =     @IsWebVisible)"; 
            //SQL += " AND    (@IsVirtualPathValid            IS NULL OR IsVirtualPathValid           =     @IsVirtualPathValid)"; 
            //SQL += " AND    (@IsThumbnailVirtualPathValid   IS NULL OR IsThumbnailVirtualPathValid  =     @IsThumbnailVirtualPathValid)";
            //SQL += " AND    (@IsValidated                   IS NULL OR IsValidated                  =     @IsValidated)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("OwnedByCooperatorID", searchEntity.OwnedByCooperatorID > 0 ? (object)searchEntity.OwnedByCooperatorID : DBNull.Value, true),
                CreateParameter("OwnedByCooperatorSiteID", searchEntity.OwnedByCooperatorSiteID > 0 ? (object)searchEntity.OwnedByCooperatorSiteID : DBNull.Value, true),
                CreateParameter("InventoryText", (object)searchEntity.InventoryText ?? DBNull.Value, true),
                CreateParameter("Title", (object)searchEntity.Title ?? DBNull.Value, true),
                CreateParameter("AttachmentDescription", (object)searchEntity.AttachmentDescription ?? DBNull.Value, true),
                CreateParameter("AttachmentDescriptionCode", (object)searchEntity.AttachnmentDescriptionCode ?? DBNull.Value, true),
                CreateParameter("ContentType", (object)searchEntity.ContentType ?? DBNull.Value, true),
                CreateParameter("CategoryCode", (object)searchEntity.CategoryCode ?? DBNull.Value, true),
                CreateParameter("IsWebVisible", (object)searchEntity.IsWebVisible ?? DBNull.Value, true),
                CreateParameter("IsVirtualPathValid", (object)searchEntity.IsVirtualPathValid ?? DBNull.Value, true),
                CreateParameter("IsThumbnailVirtualPathValid", (object)searchEntity.IsThumbnailVirtualPathValid ?? DBNull.Value, true),
                CreateParameter("IsValidated", (object)searchEntity.IsValidated ?? DBNull.Value, true)
            };

            switch (searchEntity.TimeFrame)
            {
                case "TDY":
                    SQL += " AND (CONVERT(date, CreatedDate) = CONVERT(date, GETDATE()))";
                    break;
                case "3DY":
                    SQL += " AND CreatedDate >= DATEADD(day,-3, GETDATE())";
                    break;
                case "7DY":
                    SQL += " AND CreatedDate >= DATEADD(day,-7, GETDATE())";
                    break;
                case "30D":
                    SQL += " AND CreatedDate >= DATEADD(day,-30, GETDATE())";
                    break;
                case "60D":
                    SQL += " AND CreatedDate >= DATEADD(day,-60, GETDATE())";
                    break;
                case "90D":
                    SQL += " AND CreatedDate >= DATEADD(day,-90, GETDATE())";
                    break;
            }

            results = GetRecords<AccessionInventoryAttachment>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(AccessionInventoryAttachment entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<AccessionInventoryAttachment>(entity);

            //SQL = "usp_GGTools_GRINGlobal_AccessionInventoryAttachment_Update";

            BuildInsertUpdateParameters(entity);
            //AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }
        
       
        public List<AccessionInventoryAttachment> SearchFolderItems(AccessionInventoryAttachmentSearch searchEntity)
        {
            List<AccessionInventoryAttachment> results = new List<AccessionInventoryAttachment>();

            //SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
            //    " auil.list_name AS ListName, " +
            //    " auil.app_user_item_folder_id AS FolderID, " +
            //    " vgtf.* " +
            //    " FROM vw_GGTools_GRINGlobal_AccessionInventoryAttachments vgga " +
            //    " JOIN app_user_item_list auil " +
            //    " ON vgga.ID = auil.id_number " +
            //    " WHERE auil.id_type = 'accession_inventory_attachment' ";
            //SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<AccessionInventoryAttachment>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public List<CodeValue> GetTimeFrameOptions()
        {
            List<CodeValue> timeFrameOptions = new List<CodeValue>();
            timeFrameOptions.Add(new CodeValue { Value = "TDY", Title = "Today" });
            timeFrameOptions.Add(new CodeValue { Value = "3DY", Title = "Within the last 3 days" });
            timeFrameOptions.Add(new CodeValue { Value = "7DY", Title = "Within the last 7 days" });
            timeFrameOptions.Add(new CodeValue { Value = "30D", Title = "Within the last 30 days" });
            timeFrameOptions.Add(new CodeValue { Value = "60D", Title = "Within the last 60 days" });
            return timeFrameOptions;
        }

        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
    }
}
