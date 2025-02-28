using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class LiteratureViewModelBase : AppViewModelBase
    {
        private string _PageTitle = String.Empty;
        private Literature _Entity = new Literature();
        private LiteratureSearch _SearchEntity = new LiteratureSearch();
        private Collection<Literature> _DataCollection = new Collection<Literature>();
        private Collection<Citation> _DataCollectionCitations = new Collection<Citation>();

        public LiteratureViewModelBase()
        {
            TableName = "literature";
            using (LiteratureManager mgr = new LiteratureManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                LiteratureTypes = new SelectList(mgr.GetCodeValues("LITERATURE_TYPE"), "Value", "Title");
            }
        }

        public Literature Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public LiteratureSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<Literature> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<Citation> DataCollectionCitations
        {
            get { return _DataCollectionCitations; }
            set { _DataCollectionCitations = value; }
        }
        public SelectList LiteratureTypes { get; set; }
    }
}
