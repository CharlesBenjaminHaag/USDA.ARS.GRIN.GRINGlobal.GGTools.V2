using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public class BaseController : Controller
    {
   

        public string SessionKeyName
        {
            get 
            {
                StringBuilder sessionKeyName = new StringBuilder();
                sessionKeyName.Append(this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper());
                sessionKeyName.Append("_");
                sessionKeyName.Append(this.ControllerContext.RouteData.Values["action"].ToString().ToUpper());
                return sessionKeyName.ToString();
            }
        }

        public void SetPageTitle()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            System.Text.StringBuilder sbPageTitle = new System.Text.StringBuilder();

            if (actionName.ToUpper() != "INDEX")
            {
                sbPageTitle.Append(actionName);
                sbPageTitle.Append(" ");
            }

            sbPageTitle.Append(controllerName);

            //if (sysTableTitleParameter != null)
            //{
            //    sbPageTitle.Append(sysTableTitleParameter.ToString());
            //}
            //else
            //{
            //    sbPageTitle.Append(controllerName);
            //}
            
            ViewBag.PageTitle = sbPageTitle.ToString();
        }

        /// <summary>
        /// Handles all menu rendering.
        /// 1. By default, a menu with a "Home" link leading to the GGTools main page.
        /// </summary>
        /// <param name="eventAction"></param>
        /// <param name="eventValue"></param>
        /// <param name="sysTableName"></param>
        /// <param name="sysTableTitle"></param>
        /// <returns></returns>
        public virtual PartialViewResult PageMenu(string eventAction, string eventValue, string sysTableName = "", string sysTableTitle = "", int entityId = 0)
        {
            ViewBag.EventAction = eventAction;
            ViewBag.EventValue = eventValue;

            if (eventValue == "Add")
            {
                return PartialView("~/Views/Components/_DefaultAddMenu.cshtml");
            }
            else
            {
                if (eventValue == "Edit")
                {
                    return PartialView("~/Views/Components/_DefaultEditMenu.cshtml");
                }
                else
                {
                    if (!String.IsNullOrEmpty(sysTableName))
                    {
                        return PartialView("~/Views/Components/_DefaultSearchMenu.cshtml");
                    }
                    else
                    {
                        return PartialView("~/Views/Components/_DefaultMenu.cshtml");
                    }
                }
            }
        }

        //public class RouteInfo
        //{
        //    public RouteInfo(RouteData data)
        //    {
        //        RouteData = data;
        //    }

        //    public RouteInfo(Uri uri, string applicationPath)
        //    {
        //        RouteData = RouteTable.Routes.GetRouteData(new InternalHttpContext(uri, applicationPath));
        //    }

        //    public RouteData RouteData { get; private set; }

        //    //********************
        //    //Miscellaneous properties here to deal with routing conditionals... (e.g. "CanRedirectFromSignIn")
        //    //********************

        //    private class InternalHttpContext : HttpContextBase
        //    {
        //        private HttpRequestBase _request;

        //        public InternalHttpContext(Uri uri, string applicationPath) : base()
        //        {
        //            _request = new InternalRequestContext(uri, applicationPath);
        //        }

        //        public override HttpRequestBase Request { get { return _request; } }
        //    }

        //    private class InternalRequestContext : HttpRequestBase
        //    {
        //        private string _appRelativePath;
        //        private string _pathInfo;

        //        public InternalRequestContext(Uri uri, string applicationPath) : base()
        //        {
        //            _pathInfo = uri.Query;

        //            if (String.IsNullOrEmpty(applicationPath) || !uri.AbsolutePath.StartsWith(applicationPath, StringComparison.OrdinalIgnoreCase))
        //            {
        //                _appRelativePath = uri.AbsolutePath.Substring(applicationPath.Length);
        //            }
        //            else
        //            {
        //                _appRelativePath = uri.AbsolutePath;
        //            }
        //        }

        //        public override string AppRelativeCurrentExecutionFilePath { get { return String.Concat("~", _appRelativePath); } }
        //        public override string PathInfo { get { return _pathInfo; } }
        //    }
        //}

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public SysUser AuthenticatedUser
        {
            get
            {
                AuthenticatedUserSession authenticatedUserSession = null;

                if (Session != null)
                {
                    if (Session["AUTHENTICATED_USER_SESSION"] != null)
                    {
                        authenticatedUserSession = Session["AUTHENTICATED_USER_SESSION"] as AuthenticatedUserSession;
                    }
                }
                return authenticatedUserSession.User;
            }
        }

        [HttpGet]
        public ActionResult Refresh()
        {

            if (Session["type"] != null && Session["resulttype"] != null)
                return View();
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;

        //    //Log the error!!
        //    Log.Error(filterContext.Exception);
        //}
        protected string GetFormFieldValue(FormCollection formCollection, string fieldName)
        {
            if (formCollection.Count == 0)
            {
                return String.Empty;
            }

            if (formCollection[fieldName] == null)
            {
                return String.Empty;
            }
            else
            {
                return formCollection[fieldName];
            }
        }

        public JsonResult SetOwner(string tableName, int cooperatorID)
        {
            try
            {
                // TODO Update relevant table with new owner ID.
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
       
        //public string IsReadOnly
        //{
        //    get
        //    {
        //        if ((AuthenticatedUser.IsInRole("GGTOOLS_COOPERATOR")) ||
        //            (AuthenticatedUser.IsInRole("MANAGE_COOPERATOR")) ||
        //            (AuthenticatedUser.IsInRole("GGTOOLS_ADMIN")) ||
        //            (AuthenticatedUser.CooperatorID == Entity.ID)
        //            )
        //        {
        //            return "N";
        //        }
        //        else
        //        {
        //            return "Y";
        //        }
        //    }
        //}
    }
}