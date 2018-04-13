using SSI.Entity.Manage;
using SSI.Utilities;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace SSI.Web
{
    //验证登录 By 阮创 2017/11/30
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public AuthLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (!ManageProvider.Provider.IsOverdue())
            {
                CookieHelper.DelCookie("login_error");
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Index", action = "Index" }));
            }
        }
    }
}