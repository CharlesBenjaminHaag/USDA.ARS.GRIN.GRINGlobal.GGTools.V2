using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
   public class CommonNameLanguageViewModelBase : AppViewModelBase
    {
        // Batch edit; refactor.  -- CBH 6.29.23
        public bool BatchEditIsSimplifiedNameOptionRequested;
        public bool BatchEditIsSetCountryOptionRequested;
        public string BatchEditCountryCode;

        private CommonNameLanguage _Entity = new CommonNameLanguage();
        private CommonNameLanguageSearch _SearchEntity = new CommonNameLanguageSearch();
        private Collection<CommonNameLanguage> _DataCollection = new Collection<CommonNameLanguage>();

        public CommonNameLanguageViewModelBase()
        {
            TableName = "taxonomy_common_name_language";
            using (CommonNameManager mgr = new CommonNameManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                Countries = new SelectList(mgr.GetCodeValues("GEOGRAPHY_COUNTRY_CODE"),"Value","Title");
            }
            
        }

        public CommonNameLanguage Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public CommonNameLanguageSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<CommonNameLanguage> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public SelectList Countries { get; set; }
        protected void SetSimplifiedName()
        {
            Entity.LanguageSimplifiedName = Entity.LanguageName.Replace("-", "").Replace("'", "").Replace(" ", "");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Aacute;", "A");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&aacute;", "a");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Eacute;", "E");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&eacute;", "e");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Iacute;", "I");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&iacute;", "i");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Oacute;", "O");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&oacute;", "o");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Uacute;", "U");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&uacute;", "u");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&yacute;", "y");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&abreve;", "a");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&gbreve;", "g");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&#301;", "i");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Ccaron;", "C");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ccaron;", "c");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Ecaron;", "E");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ecaron;", "e");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Rcaron;", "R");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&rcaron;", "r");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Scaron;", "S");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&scaron;", "s");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Zcaron;", "Z");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&zcaron;", "z");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Ccedil;", "C");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ccedil;", "c");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Scedil;", "S");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&scedil;", "s");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&acirc;", "a");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ecirc;", "e");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Icirc;", "I");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&icirc;", "i");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ocirc;", "o");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&scirc;", "s");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ucirc;", "u");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&agrave;", "a");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&egrave;", "e");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&igrave;", "i");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ograve;", "o");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Aring;", "A");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&aring;", "a");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Oslash;", "O");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&oslash;", "o");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&aelig;", "ae");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&oelig;", "oe");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&szlig;", "ss");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&atilde;", "a");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Ntilde;", "N");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ntilde;", "n");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&otilde;", "o");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Auml;", "A");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&auml;", "a");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&euml;", "e");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&iuml;", "i");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Ouml;", "O");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&ouml;", "o");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&Uuml;", "U");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.Replace("&uuml;", "u");
            Entity.LanguageSimplifiedName = Entity.LanguageSimplifiedName.ToUpper();
        }
    }
}
