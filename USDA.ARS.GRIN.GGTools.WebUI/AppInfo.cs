using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
using System.Configuration;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public static class AppInfo
    {
        public static string GetPageTitle()
        {
            string pageTitle = System.Configuration.ConfigurationManager.AppSettings["AppTitle"];
            return pageTitle;
        }

        public static string GetPublicWebsiteURL()
        {
            string publicWebsiteUrl = System.Configuration.ConfigurationManager.AppSettings["PublicWebsiteBaseUrl"];
            return publicWebsiteUrl;
        }

        public static string GetVersion()
        {
            Version version = null;
            StringBuilder versionNumber = new StringBuilder();

            version = Assembly.GetExecutingAssembly().GetName().Version;
            versionNumber.Append(version.Major.ToString());
            versionNumber.Append(".");
            versionNumber.Append(version.Minor.ToString());
            versionNumber.Append(".");
            versionNumber.Append(version.Build.ToString());

            // TODO Store additional label in config
            versionNumber.Append(" (Beta)");

            return versionNumber.ToString();
        }

        public static SysUser GetAuthenticatedUser()
        {
            SysUser authenticatedUser = new SysUser(); 
            AuthenticatedUserSession authenticatedUserSession = System.Web.HttpContext.Current.Session["AUTHENTICATED_USER_SESSION"] as AuthenticatedUserSession;
            
            if (authenticatedUserSession != null)
                authenticatedUser = authenticatedUserSession.User;
            
            return authenticatedUser;
        }
        
        public static string GetDatabase()
        {
            string databaseName = String.Empty;
            const string SERVER_NAME_LOCAL = "localhost";
            const string SERVER_NAME_DEV = "199.133.201.148";
            const string SERVER_NAME_TEST = "199.133.201.116";
            const string SERVER_NAME_TRAINING = "199.133.201.116";
            const string SERVER_NAME_PROD = "199.133.201.116";

            const string DB_NAME_LOCAL = "gringlobal";
            const string DB_NAME_DEV = "gringlobal";
            const string DB_NAME_TEST = "gringlobal-test";
            const string DB_NAME_TRAINING = "training";
            const string DB_NAME_PROD = "gringlobal";

            USDA.ARS.GRIN.GGTools.DataLayer.CodeValueManager mgr = new CodeValueManager();
            if (mgr.ConnectionString.Contains(SERVER_NAME_DEV))
            {
                databaseName = "DEV";
            }
            else
            {
                if (mgr.ConnectionString.Contains(SERVER_NAME_PROD))
                {
                    if (mgr.ConnectionString.Contains(DB_NAME_TEST))
                    {
                        databaseName = "TEST";
                    }
                    else
                    {
                        if (mgr.ConnectionString.Contains(DB_NAME_PROD))
                        {
                            databaseName = "PRODUCTION";
                        }
                        else
                        {
                            if (mgr.ConnectionString.Contains(DB_NAME_TRAINING))
                            {
                                databaseName = "TRAINING";
                            }
                        }
                    }
                }
                else
                {
                    if (mgr.ConnectionString.Contains(DB_NAME_LOCAL))
                    {
                        databaseName = "LOCAL";
                    }
                }
            }

            if (mgr.ConnectionString.Contains("localhost"))
            {
                databaseName += " (LOCAL)";
            }

            return databaseName.ToUpper();
        }
        
        public static string GetSupportEmail()
        {
            string emailAddress = String.Empty;
            emailAddress = ConfigurationManager.AppSettings["EmailAddressSupport"];
            return emailAddress;
        }
        
        public static bool IsRunningInTestMode()
        {
            string response = System.Configuration.ConfigurationManager.AppSettings["RunInTestMode"];
            if (response == "Y")
                { return true; }
            else
            { return false; }
        }
    }
}