
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Http;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GRINGlobal.API.Controllers
{
    public class AccessionController : ApiController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public async Task<IHttpActionResult> Get(string piNumber = "", string instCode = "", int offset = 0, int limit = 0, string mcpdFormat = "")
        {
            try
            {
                if (mcpdFormat.ToUpper() != "Y" && mcpdFormat.ToUpper() != "N")
                {
                    return BadRequest("The mcpdFormat flag must be either Y or N.");
                }
                
                AccessionViewModel viewModel = new AccessionViewModel();

                if (mcpdFormat.ToUpper() == "Y")
                {
                    viewModel.Export(offset, limit);
                    if (viewModel.DataCollectionMCPD.Count == 0)
                    {
                        return NotFound();
                    }
                    return Ok(viewModel.DataCollectionMCPD);
                }
                else
                {
                    viewModel.SearchEntity.AccessionNumber = piNumber;
                    viewModel.SearchEntity.InstCode = instCode;
                    viewModel.Search();
                    if (viewModel.DataCollectionMCPD.Count == 0)
                    {
                        return NotFound();
                    }
                    return  Ok(viewModel.DataCollectionMCPD);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return InternalServerError();
            }

        }

      

    }
}