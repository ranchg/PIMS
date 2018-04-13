using SSI.Entity.Manage;
using SSI.Utilities;
using System;
using System.Web.Mvc;

namespace SSI.Web
{
    //验证首页 By 阮创 2017/11/30
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthIndexAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public AuthIndexAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (ManageProvider.Provider.IsOverdue())
            {
                CookieHelper.WriteCookie("login_error", "overdue");
                filterContext.Result = new ContentResult() { Content = "<script>top.location.href = '/Login/Index';</script>" };
            }
            else
            {
                LogApp.Write(LogApp.Result.Success);
            }
        }
    }
}