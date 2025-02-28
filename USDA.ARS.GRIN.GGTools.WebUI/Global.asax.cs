using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Log.Error(exception, exception.Message);
          
            //var httpContext = ((HttpApplication)sender).Context;
            //httpContext.Response.Clear();
            //httpContext.ClearError();

            //if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            //{
            //    return;
            //}

            //var routeData = new RouteData();
            //routeData.Values.Add("controller", "ErrorPage");
            //routeData.Values.Add("action", "Error");
            //routeData.Values.Add("exception", exception);

            //if (exception.GetType() == typeof(HttpException))
            //{
            //    routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
            //}
            //else
            //{
            //    routeData.Values.Add("statusCode", 500);
            //}
            //ExecuteErrorController(httpContext, exception as HttpException);
        }

        protected void Session_Start()
        {
            Session["USER-SESSION-START"] = DateTime.Now.ToShortTimeString();
        }

        protected void InitializeCachedData()
        {
            //USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer.GeographyViewModel geographyViewModel = new Taxonomy.ViewModelLayer.GeographyViewModel();
            //System.Runtime.Caching.ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
            //using (geographyViewModel)
            //{
            //    List<Ge>
            //    codeValues = mgr.GetStandardAbbreviations();
            //}
            //cache.Set("DATA-LIST-STANDARD-ABBREVIATIONS", codeValues, policy);
        }
    }
}
