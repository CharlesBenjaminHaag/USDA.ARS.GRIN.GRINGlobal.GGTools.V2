using System;
using System.Web.Mvc;
//using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Data.Common;
using System.Data;
using Microsoft.Win32;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class ISTASeedViewModel : ISTASeedViewModelBase, IViewModel<ISTASeed>
    {
      
        public void Delete()
        {
            try
            {
                using (ISTASeedManager mgr = new ISTASeedManager())
                {
                    mgr.Delete(TableName, Entity.ID);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public ISTASeed Get(int entityId)
        {
            DataCollection = new Collection<ISTASeed>();
            using (ISTASeedManager mgr = new ISTASeedManager())
            {
                try
                {
                    Entity = mgr.Get(entityId);
                  
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }

            return Entity;
        }
       
        public int Insert()
        {
            using (ISTASeedManager mgr = new ISTASeedManager())
            {
                try
                {
                     RowsAffected = mgr.Insert(Entity);
                    
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public void Search()
        {
            using (ISTASeedManager mgr = new ISTASeedManager())
            {
                try
                {
                    DataCollection = new Collection<ISTASeed>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void GetFolderItems()
        {
            using (ISTASeedManager mgr = new ISTASeedManager())
            {
                try
                {
                   // DataCollection = new Collection<ISTASeed>(mgr.GetFolderItems(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public int Update()
        {
            using (ISTASeedManager mgr = new ISTASeedManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;
        }

       public override bool Validate()
        {
            bool validated = true;

           

            //if (ISTASeedValidationViewModel.DataCollection.Count > 0)
            //{
            //    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = String.Format("The ISTASeed that you've attempted to create currently exists.", Entity.ISTASeedAuthority) }); ;
            //}


            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }
        
        
    }
}
