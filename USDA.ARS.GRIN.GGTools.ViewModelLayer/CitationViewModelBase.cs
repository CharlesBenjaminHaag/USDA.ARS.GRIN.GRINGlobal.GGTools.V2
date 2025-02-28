using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CitationViewModelBase : AppViewModelBase
    {
        public string _IsMultiSelect { get; set; }
        private int _ReferencedEntityID = 0;
        private string _SpeciesIDList = String.Empty;
        private Citation _Entity = new Citation();
        private Citation _CloneEntity = new Citation();
        private CitationSearch _SearchEntity = new CitationSearch();
        private Collection<Citation> _DataCollection = new Collection<Citation>();
        private Collection<Citation> _DataCollectionTaxon = new Collection<Citation>();
        private Collection<Cooperator> _DataCollectionCooperators = new Collection<Cooperator>();
        private Collection<Species> _DataCollectionSpecies = new Collection<Species>();
        private Collection<ReportItem> _DataCollectionReportItems = new Collection<ReportItem>();
        public CitationViewModelBase()
        {
            this.TableCode = "Citation";
            this.TableName = "citation";

            using (CitationManager mgr = new CitationManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("citation"), "ID", "FullName");
                OwnedByCooperators = new SelectList(mgr.GetOwnedByCooperators("citation"), "ID", "FullName");
                TableNames = new SelectList(mgr.GetTableNames(), "Key", "Value");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                StandardAbbreviations = new SelectList(GetStandardAbbreviations(), "Value", "Title");
                LiteratureTypes = new SelectList(mgr.GetCodeValues("LITERATURE_TYPE"), "Value", "Title");

                List<CodeValue> citationTypes = new List<CodeValue>();
                citationTypes = mgr.GetCodeValues("CITATION_TYPE");
                citationTypes.Add(new CodeValue { Value = "NULL", Title = "NULL" });
                CitationTypes = new SelectList(citationTypes, "Value", "Title");
            }
        }

        public int ReferencedEntityID
        {
            get
            { return _ReferencedEntityID; }
            set
            { _ReferencedEntityID = value; }
        }

        public string IsMultiSelect {
            get
            { return _IsMultiSelect; }
            set
            { _IsMultiSelect = value; }
        }
        public string SpeciesIDList
        {
            get
            {
                return _SpeciesIDList;
            }
            set 
            {
                _SpeciesIDList = value;
            }
        }
        public Citation Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public Citation CloneEntity
        {
            get { return _CloneEntity; }
            set { _CloneEntity = value; }
        }

        public CitationSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Citation> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<Citation> DataCollectionTaxon
        {
            get { return _DataCollectionTaxon; }
            set { _DataCollectionTaxon = value; }
        }
        public Collection<Species> DataCollectionSpecies 
        {
            get { return _DataCollectionSpecies; }
            set { _DataCollectionSpecies = value; }
        }
        public Collection<ReportItem> DataCollectionReportItems
        {
            get { return _DataCollectionReportItems; }
            set { _DataCollectionReportItems = value; }
        }
        private List<CodeValue> GetStandardAbbreviations()
        {
            List<CodeValue> codeValues = new List<CodeValue>();

            ObjectCache cache = MemoryCache.Default;
            codeValues = cache["DATA-LIST-STANDARD-ABBREVIATIONS"] as List<CodeValue>;

            if (codeValues == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (CitationManager mgr = new CitationManager())
                {
                    codeValues = mgr.GetStandardAbbreviations();
                }
                cache.Set("DATA-LIST-STANDARD-ABBREVIATIONS", codeValues, policy);
            }
            return codeValues;
        }
        public SelectList TableNames { get; set; }
        public SelectList StandardAbbreviations { get; set; }
        public SelectList LiteratureTypes { get; set; }
        public SelectList CitationTypes { get; set; }
    }
}
