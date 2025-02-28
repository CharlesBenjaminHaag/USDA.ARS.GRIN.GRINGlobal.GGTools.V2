using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class InventorySearch : SearchEntityBase
    {
        public int ID { get; set; }
        public int AccessionID { get; set; }
        public string AccessionAssembledName { get; set; }
        public int AccessionOwnedByCooperatorID { get; set; }
        public string AccessionOwnedByCooperatorName { get; set; }
       
      //  ,[AccessionOwnedByCooperatorEmailAddress]
      //,[InventoryNumberPart1]
      //,[InventoryNumberPart2]
      //,[InventoryNumberPart3]
      //,[AssembledName]
      //,[FormTypeCode]
      //,[InventoryMaintenancePolicyID]
      //,[IsDistributable]
      //,[c]
      //,[storage_location_part2]
      //,[storage_location_part3]
      //,[storage_location_part4]
      //,[latitude]
      //,[longitude]
      //,[IsAvailable]
      //,[web_availability_note]
      //,[AvailabilityStatusCode]
      //,[availability_status_note]
      //,[availability_start_date]
      //,[availability_end_date]
      //,[QuantityOnHand]
      //,[quantity_on_hand_unit_code]
      //,[is_auto_deducted]
      //,[distribution_default_form_code]
      //,[distribution_default_quantity]
      //,[distribution_unit_code]
      //,[distribution_critical_quantity]
      //,[regeneration_critical_quantity]
      //,[pathogen_status_code]
      //,[parent_inventory_id]
      //,[backup_inventory_id]
      //,[rootstock]
      //,[hundred_seed_weight]
      //,[pollination_method_code]
      //,[pollination_vector_code]
      //,[preservation_method_id]
      //,[regeneration_method_id]
      //,[plant_sex_code]
      //,[propagation_date]
      //,[propagation_date_code]
      //,[note]
      //,[CreatedDate]
      //,[CreatedByCooperatorID]
      //,[CreatedByCooperatorName]
      //,[ModifiedDate]
      //,[ModifiedByCooperatorID]
      //,[ModifiedByCooperatorName]
      //,[OwnedDate]
      //,[OwnedByCooperatorID]
      //,[OwnedByCooperatorName]
      //,[digital_object_identifier]
      //,[propagation_technique_code]
    }
}
