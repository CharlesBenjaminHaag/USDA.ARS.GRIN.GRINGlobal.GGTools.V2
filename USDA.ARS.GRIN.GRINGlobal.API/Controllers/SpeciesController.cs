using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Http;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GRINGlobal.API.Controllers
{
    public class SpeciesController : ApiController
    {
        public async Task<IHttpActionResult> Get(string isAccepted = null, string name = null, string commonName = null, string getSynonyms = null, string getConSpecific = null, string getGenus = null, string getCommonNames = null, string getCitations = null)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.SearchEntity.IsAcceptedName = isAccepted;
                viewModel.SearchEntity.Name = name;
                viewModel.SearchEntity.GetCommonNames = getCommonNames;
                viewModel.SearchEntity.GetConspecific = getConSpecific;
                viewModel.SearchEntity.GetCitations = getCitations;
                viewModel.Search();
                var result = viewModel.DataCollection;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("A server-level error has occurred.");
            }

        }

    }
}