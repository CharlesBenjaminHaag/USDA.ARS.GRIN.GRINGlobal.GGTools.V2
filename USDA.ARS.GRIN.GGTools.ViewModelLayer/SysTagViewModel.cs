using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysTagViewModel: SysTagViewModelBase
    {
        public SysTagViewModel()
        { }
        
        public SysTagViewModel(int cooperatorId) : base(cooperatorId)
        {}
        
        public void Get()
        {
            
        }
              
        public void Search()
        {
            using (SysTagManager mgr = new SysTagManager())
            {
                DataCollection = new Collection<SysTag>(mgr.Search(SearchEntity));
            }
        }
        public void Insert()
        {
            try
            {
                using (SysTagManager mgr = new SysTagManager())
                {
                    Entity.ID = mgr.Insert(Entity);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }
        
      
        public int Update()
        {
            return 0;
            
        }
        
        public void Delete()
        {
            try
            {
         
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }
    }
}
