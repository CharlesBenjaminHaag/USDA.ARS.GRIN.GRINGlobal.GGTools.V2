using System;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AccessionViewModelBase: AppViewModelBase
    {
        private Accession _Entity = new Accession();
        private AccessionSearch _SearchEntity = new AccessionSearch();
        private Collection<Accession> _DataCollection = new Collection<Accession>();
        private Collection<AccessionMCPD> _DataCollectionMCPD = new Collection<AccessionMCPD>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();

        public AccessionViewModelBase()
        {
             
        }

        public Accession Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public AccessionSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Accession> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<AccessionMCPD> DataCollectionMCPD
        {
            get { return _DataCollectionMCPD; }
            set { _DataCollectionMCPD = value; }
        }
    }
}
