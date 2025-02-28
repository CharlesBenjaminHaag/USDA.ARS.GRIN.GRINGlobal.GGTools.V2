using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.DataLayer.UPOV;
using System.Collections.Generic;
using System.Web.Mvc.Ajax;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class UPOVViewModel : UPOVViewModelBase
    {
        public int Insert()
        {
            using (UPOVManager mgr = new UPOVManager())
            {
                try
                {
                    mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public int UpdateAll()
        {
            using (UPOVManager mgr = new UPOVManager())
            {
                try
                {
                    mgr.UpdateAll();
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public int DeleteAll()
        {
            using (UPOVManager mgr = new UPOVManager())
            {
                try
                {
                    mgr.DeleteAll();
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return 0;
            }
        }

        public void Search()
        {
            using (UPOVManager mgr = new UPOVManager())
            {
                try
                {
                    DataCollection = new Collection<UPOVEncodedSpecies>(mgr.Search(SearchEntity));
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
    }
}
