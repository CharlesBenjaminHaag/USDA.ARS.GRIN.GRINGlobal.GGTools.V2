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
    public class ISTASeedViewModelBase: AppViewModelBase
    {
       

        private string _PageTitle = String.Empty;
       
        private ISTASeed _Entity = new ISTASeed();
       
        private ISTASeedSearch _SearchEntity = new ISTASeedSearch();
        private Collection<ISTASeed> _DataCollection = new Collection<ISTASeed>();
     
        
        public ISTASeedViewModelBase()
        {
            TableName = "taxonomy_ISTASeed";

          

            using (ISTASeedManager mgr = new ISTASeedManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                         }
        }

        public int ID
        {
            get { return ID; }
            set { ID = value; }
        }
      
       

      

      
        public ISTASeed Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        
      

        public ISTASeedSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<ISTASeed> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
    
        public new string PageTitle
        {
            get {
               
                return _PageTitle;
            }
        }

    

        
    }
}
