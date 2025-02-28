using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Caching;

/// <summary>
/// Encapsulates logic shared across entities.
/// </summary>
namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class ReferenceViewModel : AppViewModelBase
    {
        private Collection<CodeValue> _DataCollectionCodeValues = new Collection<CodeValue>();
        private ReferenceSearch _SearchEntity = new ReferenceSearch();

        public void SearchNotes()
        {
            using (ReferenceManager mgr = new ReferenceManager())
            {
                DataCollectionCodeValues = new Collection<CodeValue>();
                DataCollectionCodeValues = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity));
                RowsAffected = DataCollectionCodeValues.Count;
            }
        }

        public ReferenceSearch SearchEntity 
        {
            get { return this._SearchEntity; }
            set { this._SearchEntity = value; }
        }

        public Collection<CodeValue> DataCollectionCodeValues
        {
            get { return _DataCollectionCodeValues; }
            set { _DataCollectionCodeValues = value; }
        }
    }
}
