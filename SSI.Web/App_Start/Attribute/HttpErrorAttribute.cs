using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web
{
    //默认错误返回值 By 阮创 2017/11/30
    public class HttpErrorAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.Result = new ContentResult { Content = new JsonMessage { state = "error", msg = filterContext.Exception.Message }.ToJson() };
        }
    }
}