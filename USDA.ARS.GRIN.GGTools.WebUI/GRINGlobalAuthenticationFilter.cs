using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public class GrinGlobalAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AuthenticatedUserSession authenticatedUserSession = filterContext.HttpContext.Session["AUTHENTICATED_USER_SESSION"] as AuthenticatedUserSession;
            if (authenticatedUserSession == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }

            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerDescriptor.ControllerName;

            var viewBag = filterContext.Controller.ViewBag;

            if (actionName.ToUpper() == "INDEX")
            {
                viewBag.PageTitle = controllerName;
            }
            else
            {
                var queryString = filterContext.HttpContext.Request.QueryString;
                if (!string.IsNullOrEmpty(queryString["sysTableTitle"]))
                {
                    controllerName = queryString["sysTableTitle"];
                }
                viewBag.PageTitle = actionName + " " + controllerName;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}