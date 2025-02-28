using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class ExplorationViewModelBase : AppViewModelBase
    {
        private string _SpeciesIDList;
        private Exploration _Entity = new Exploration();
        private ExplorationSearch _SearchEntity = new ExplorationSearch();
        private Collection<Exploration> _DataCollection = new Collection<Exploration>();
        private Collection<ExplorationMap> _DataCollectionExplorationMaps = new Collection<ExplorationMap>();

        public ExplorationViewModelBase()
        {
            TableName = "taxonomy_use";
            TableCode = "Taxonomy Use";
            using (ExplorationManager mgr = new ExplorationManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                
            }
        }
        public string SpeciesIDList
        {
            get { return _SpeciesIDList; }
            set { _SpeciesIDList = value; }
        }
        public ExplorationViewModelBase(int speciesId)
        {
            TableName = "exploration";

            using (ExplorationManager mgr = new ExplorationManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                
            }
        }

        public Exploration Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        
        public ExplorationSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
       
        public Collection<Exploration> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<ExplorationMap> DataCollectionExplorationMaps
        {
            get { return _DataCollectionExplorationMaps; }
            set { _DataCollectionExplorationMaps = value; }
        }

        #region Select Lists

        #endregion
    }
}
