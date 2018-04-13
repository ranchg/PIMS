using System;
using System.Reflection;
using System.Web.Mvc;

namespace SSI.Web
{
    //验证Ajax提求 By 阮创 2017/11/30
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class HttpAjaxAttribute : ActionMethodSelectorAttribute
    {
        public bool Ignore { get; set; }
        public HttpAjaxAttribute(bool ignore = false)
        {
            Ignore = ignore;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            if (Ignore)
            {
                return true;
            }
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}