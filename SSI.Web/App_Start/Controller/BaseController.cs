using SSI.Utilities;
using System.Web.Mvc;

namespace SSI.Web
{
    //控制器基类 By 阮创 2017/11/30
    public abstract class BaseController : Controller
    {
        [HttpGet]
        [AuthAction]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AuthAction]
        public virtual ActionResult Form()
        {
            return View();
        }
        protected virtual ActionResult Success(string msg, object data = null)
        {
            return Content(new JsonMessage { state = "success", msg = msg, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string msg, object data = null)
        {
            return Content(new JsonMessage { state = "error", msg = msg, data = data }.ToJson());
        }
        protected virtual ActionResult Warning(string msg, object data = null)
        {
            return Content(new JsonMessage { state = "warning", msg = msg, data = data }.ToJson());
        }
    }
}