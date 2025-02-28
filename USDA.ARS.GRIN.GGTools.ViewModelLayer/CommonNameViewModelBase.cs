using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CommonNameViewModelBase: AppViewModelBase
    {
        private string _SpeciesIDList;
        private string _DetailPartialViewName;
        private string _EditPartialViewName;
        private string _ListPartialViewName;
        private CommonName _Entity = new CommonName();
        private Species _SpeciesEntity = new Species();
        private Genus _GenusEntity = new Genus();
        private CommonNameSearch _SearchEntity = new CommonNameSearch();
        private Collection<CommonName> _DataCollection = new Collection<CommonName>();

        public CommonNameViewModelBase()
        {
            TableName = "taxonomy_common_name";
            using (CommonNameManager mgr = new CommonNameManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                //Countries = new SelectList(mgr.GetCodeValues("GEOGRAPHY_COUNTRY_CODE", "Code", "CodeTitle"));
            }

            using (CommonNameLanguageManager commonNameLanguageMgr = new CommonNameLanguageManager())
            {
                CommonNameLanguages = new SelectList(commonNameLanguageMgr.Search(new CommonNameLanguageSearch()),"ID","AssembledName");
            }
        }
        public string SpeciesIDList
        {
            get { return _SpeciesIDList; }
            set { _SpeciesIDList = value; }
        }
        public string EditPartialViewName
        {
            get { return _EditPartialViewName; }
            set { _EditPartialViewName = value; }
        }
        public string DetailPartialViewName
        {
            get { return _DetailPartialViewName; }
            set { _DetailPartialViewName = value; }
        }
        public string ListPartialViewName
        {
            get { return _ListPartialViewName; }
            set { _ListPartialViewName = value; }
        }

        public CommonName Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public Genus GenusEntity
        {
            get { return _GenusEntity; }
            set { _GenusEntity = value; }
        }

        public Species SpeciesEntity 
        { 
            get { return _SpeciesEntity; }
            set { _SpeciesEntity = value; }
        }

        public CommonNameSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<CommonName> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        protected void SetSimplifiedName()
        {
            Entity.SimplifiedName = Entity.Name.Replace("-", "").Replace("'", "").Replace(" ", "");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Aacute;", "A");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&aacute;", "a");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Eacute;", "E");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&eacute;", "e");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Iacute;", "I");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&iacute;", "i");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Oacute;", "O");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&oacute;", "o");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Uacute;", "U");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&uacute;", "u");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&yacute;", "y");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&abreve;", "a");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&gbreve;", "g");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&#301;", "i");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Ccaron;", "C");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ccaron;", "c");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Ecaron;", "E");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ecaron;", "e");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Rcaron;", "R");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&rcaron;", "r");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Scaron;", "S");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&scaron;", "s");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Zcaron;", "Z");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&zcaron;", "z");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Ccedil;", "C");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ccedil;", "c");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Scedil;", "S");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&scedil;", "s");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&acirc;", "a");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ecirc;", "e");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Icirc;", "I");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&icirc;", "i");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ocirc;", "o");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&scirc;", "s");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ucirc;", "u");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&agrave;", "a");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&egrave;", "e");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&igrave;", "i");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ograve;", "o");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Aring;", "A");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&aring;", "a");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Oslash;", "O");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&oslash;", "o");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&aelig;", "ae");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&oelig;", "oe");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&szlig;", "ss");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&atilde;", "a");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Ntilde;", "N");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ntilde;", "n");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&otilde;", "o");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Auml;", "A");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&auml;", "a");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&euml;", "e");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&iuml;", "i");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Ouml;", "O");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&ouml;", "o");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&Uuml;", "U");
            Entity.SimplifiedName = Entity.SimplifiedName.Replace("&uuml;", "u");
            Entity.SimplifiedName = Entity.SimplifiedName.ToUpper();
        }

        #region Select Lists
        
        public SelectList Countries { get; set; }
        public SelectList CommonNameLanguages { get; set; }

        #endregion
    }
}
