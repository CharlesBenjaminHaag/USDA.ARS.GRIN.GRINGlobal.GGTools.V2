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
    public class InfraspecificOptionsViewModel : ViewModelBase
    {
        public string SelectedRank { get; set; }
        public string SelectedRankDescription { get; set; }
        public int ParentSpeciesID { get; set; }
        public bool IsCopyProtologueRequired { get; set; }
        public bool IsCopyAuthorityRequired { get; set; }
        public bool IsCopyNoteRequired { get; set; }
        public bool IsGenerateAutonymRequired { get; set; }

        public SelectList Ranks
        {
            get
            {
                System.Collections.Generic.List<CodeValue> codeValues = new System.Collections.Generic.List<CodeValue>();
                codeValues.Add(new CodeValue { Value = "species", Title = "Species" });
                codeValues.Add(new CodeValue { Value = "subspecies", Title = "Subspecies" });
                codeValues.Add(new CodeValue { Value = "variety", Title = "Variety" });
                codeValues.Add(new CodeValue { Value = "subvariety", Title = "Subvariety" });
                codeValues.Add(new CodeValue { Value = "forma", Title = "Forma" });
                return new SelectList(codeValues, "Value", "Title");
            }
        }
    }
}
