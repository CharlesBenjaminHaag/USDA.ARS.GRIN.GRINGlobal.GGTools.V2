using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class ViewModelFactory
    {
        private AuthorViewModel authorViewModel = null;
        private ClassificationViewModel classificationViewModel = null;
        private CropForCWRViewModel cropForCWRViewModel = null;
        private CWRMapViewModel cWRMapViewModel = null;
        private CWRTraitViewModel cWRTraitViewModel = null;
        private CommonNameViewModel commonNameViewModel = null;
        private CommonNameLanguageViewModel commonNameLanguageViewModel = null;
        private EconomicUsageTypeViewModel economicUsageTypeViewModel = null;
        private EconomicUseViewModel economicUseViewModel = null;
        private FamilyViewModel familyViewModel = null;
        private GenusViewModel genusViewModel = null;

        public AppViewModelBase GetViewModel(string sysTableName)
        {
            switch (sysTableName)
            {
                case "taxonomy_author":
                    return new AuthorViewModel();
                default:
                    return new AppViewModelBase();
            }
        }
    }
}
