using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SpeciesViewModelBase: AppViewModelBase
    {
        //private bool _IsBasionymNeededOption;
        //private bool _IsAutonymNeededOption;
        //private string _IsBasionymNeeded = String.Empty;
        //private string _IsAutonymNeeded = String.Empty;

        private string _PageTitle = String.Empty;
        private string _IsMultiSelect;
        private int _SpeciesID;
        private string _SubspeciesUrl;
        private string _VarietyUrl;
        private string _SubvarietyUrl;
        private string _FormUrl;
        private Report _Report = new Report();
        private Species _Entity = new Species();
        private Species _AutonymEntity = new Species();
        private Species _ParentEntity = new Species();
        private Species _Autonym = new Species();
        private SpeciesSearch _SearchEntity = new SpeciesSearch();
        private Collection<Species> _DataCollection = new Collection<Species>();
        private Collection<Species> _DataCollectionSynonyms = new Collection<Species>();
        private Collection<Species> _DataCollectionImport = new Collection<Species>();
        private Collection<CodeValue> _DataCollectionProtologues = new Collection<CodeValue>();
        private Collection<CodeValue> _DataCollectionProtologueVirtualPaths = new Collection<CodeValue>();
        private Collection<CodeValue> _DataCollectionReports = new Collection<CodeValue>();
        
        public SpeciesViewModelBase()
        {
            TableName = "taxonomy_species";

            Entity.IsSpecificHybrid = "N";
            Entity.IsSubspecificHybrid = "N";
            Entity.IsVarietalHybrid = "N";
            Entity.IsSubVarietalHybrid = "N";
            Entity.IsFormaHybrid = "N";

            using (SpeciesManager mgr = new SpeciesManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                VerifiedByCooperators = new SelectList(mgr.GetVerifiedByCooperators(), "ID", "FullName");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                TimeFrameOptions = new SelectList(mgr.GetCodeValues("TAXONOMY_SEARCH_TIME_FRAME"), "Value", "Title");
                SynonymCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_SPECIES_QUALIFIER"), "Value", "Title");
                FormaRankTypes = new SelectList(mgr.GetCodeValues("TAXONOMY_FORMA_RANK_TYPE"), "Value", "Title");
            }
        }

        public int SpeciesID
        {
            get { return _SpeciesID; }
            set { _SpeciesID = value; }
        }
        public string SubspeciesUrl
        {
            get
            { return _SubspeciesUrl; }
            set
            { _SubspeciesUrl = value; } 
        }
        public string VarietyUrl
        {
            get
            { return _VarietyUrl; }
            set
            { _VarietyUrl = value; }
        }

        public string SubvarietyUrl
        {
            get
            { return _SubvarietyUrl; }
            set
            { _SubvarietyUrl = value; }
        }
        public string FormUrl
        {
            get
            { return _FormUrl; }
            set
            { _FormUrl = value; }
        }

        public Report Report
        {
            get { return _Report; }
            set { _Report = value; }
        }
        public string IsMultiSelect
        {
            get { return _IsMultiSelect; }
            set { _IsMultiSelect = value; }
        }
        public Species Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        
        public Species ParentEntity
        {
            get { return _ParentEntity; }
            set { _ParentEntity = value; }
        }

        public Species AutonymEntity
        {
            get { return _AutonymEntity; }
            set { _AutonymEntity = value; }
        }

        public SpeciesSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Species> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<Species> DataCollectionSynonyms
        {
            get { return _DataCollectionSynonyms; }
            set { _DataCollectionSynonyms = value; }
        }

        public Collection<Species> DataCollectionImport
        {
            get { return _DataCollectionImport; }
            set { _DataCollectionImport = value; }
        }

        public Collection<CodeValue> DataCollectionProtologues
        {
            get { return _DataCollectionProtologues; }
            set { _DataCollectionProtologues = value; }
        }

        public Collection<CodeValue> DataCollectionProtologueVirtualPaths
        {
            get { return _DataCollectionProtologueVirtualPaths; }
            set { _DataCollectionProtologueVirtualPaths = value; }
        }

        public Collection<CodeValue> DataCollectionReports
        {
            get { return _DataCollectionReports; }
            set { _DataCollectionReports = value; }
        }
         
        public new string PageTitle
        {
            get {
                if (Entity.ID > 0)
                {
                    _PageTitle = String.Format("Edit Species [{0}]", Entity.ID);
                }
                else
                {
                    if (String.IsNullOrEmpty(Entity.SynonymCode))
                    {
                        _PageTitle = "Add Species";
                    }
                    else
                    {
                        _PageTitle = "Add SubTaxon";
                    }
                }
                return _PageTitle;
            }
        }

        #region Select Lists
        public SelectList TableNames { get; set; }
        public SelectList SynonymCodes { get; set; }
        public SelectList Folders { get; set; }
        public SelectList FilterOptions { get; set; }
        public SelectList Tags { get; set; }
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
        public SelectList FormaRankTypes { get; set; }
        public SelectList VerifiedByCooperators { get; set; }        

        #endregion

        //public string GetPageTitle()
        //{
        //    if (Entity.ID > 0)
        //    {
        //        _PageTitle = String.Format("Edit Species [{0}]", Entity.ID);
        //    }
        //    else
        //    {
        //        if (String.IsNullOrEmpty(Entity.SynonymCode))
        //        {
        //            _PageTitle = "Add Species";
        //        }
        //        else
        //        {
        //            _PageTitle = "Add SubTaxon";
        //        }
        //    }
        //    return _PageTitle;
        //}

        
    }
}
