using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;


namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class ErrorLogViewModel: ErrorLogViewModelBase
    {
        public void Search()
        {
            try
            {
                using (ErrorLogManager mgr = new ErrorLogManager())
                {
                    DataCollection = new Collection<ErrorLog>(mgr.Search(SearchEntity));
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }
    }
}
