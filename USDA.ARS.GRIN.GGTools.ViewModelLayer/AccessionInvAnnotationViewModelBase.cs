using System;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class AccessionInvAnnotationViewModelBase: AppViewModelBase
    {
        private AccessionInvAnnotation _Entity = new AccessionInvAnnotation();
        private AccessionInvAnnotationSearch _SearchEntity = new AccessionInvAnnotationSearch();
        private Collection<AccessionInvAnnotation> _DataCollection = new Collection<AccessionInvAnnotation>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();

        public AccessionInvAnnotationViewModelBase()
        {
             
        }

        public AccessionInvAnnotation Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public AccessionInvAnnotationSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<AccessionInvAnnotation> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
    }
}
