using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SynonymOptionsViewModel : ViewModelBase
    {
        public string SelectedSynonymCode { get; set; }
        public string SelectedSynonymName { get; set; }
        public string SelectedRank { get; set; }
        public int ParentSpeciesID { get; set; }
        public bool IsCopyProtologueRequired { get; set; }
        public bool IsCopyAuthorityRequired { get; set; }
        public bool IsCopyNoteRequired { get; set; }

        public SelectList SynonymTypeCodes {
            get 
            {
                System.Collections.Generic.List<CodeValue> codeValues = new System.Collections.Generic.List<CodeValue>();
                codeValues.Add(new CodeValue { Value = "A", Title = "Autonym" });
                codeValues.Add(new CodeValue { Value = "B", Title = "Basionym" });
                codeValues.Add(new CodeValue { Value = "S", Title = "Heterotypic synonym" });
                codeValues.Add(new CodeValue { Value = "=", Title = "Homotypic synonym" });
                codeValues.Add(new CodeValue { Value = "I", Title = "Invalid synonym" });
                return new SelectList(codeValues, "Value", "Title");
            }  
        }
        
        public SelectList Ranks
        {
            get
            {
                System.Collections.Generic.List<CodeValue> codeValues = new System.Collections.Generic.List<CodeValue>();
                codeValues.Add(new CodeValue { Value = "SPECIES", Title = "Species" });
                codeValues.Add(new CodeValue { Value = "SUBSPECIES", Title = "Subspecies" });
                codeValues.Add(new CodeValue { Value = "VARIETY", Title = "Variety" });
                codeValues.Add(new CodeValue { Value = "SUBVARIETY", Title = "Subvariety" });
                return new SelectList(codeValues, "Value", "Title");
            }
        }
    }
}
