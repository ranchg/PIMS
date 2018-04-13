using SSI.Business.SystemManage;
using SSI.Entity.SystemManage;
using SSI.Utilities;
using System;
using System.Web.Mvc;

namespace SSI.Web.Controllers
{
    public class LoginController : Controller
    {
        UserBLL userBLL = new UserBLL();

        [AuthLogin]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public FileContentResult GetVerifyCode()
        {
            return File(VerifyCodeHelper.Create(), @"image/jpeg");
        }

        [AuthLogin]
        [HttpPost]
        [HttpAjax]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string Account, string Password, string VerifyCode)
        {
            try
            {
                if (VerifyCodeHelper.Check(VerifyCode))
                {
                    T_User t_User = userBLL.CheckLogin(Account, Password);
                    if (t_User != null)
                    {
                        userBLL.AddProvider(t_User);
                        LogApp.Write(LogApp.Result.Success);
                        return Content(new JsonMessage { state = "success", msg = "登录成功，正在跳转中...", data = new { url = "/Index/Index" } }.ToJson());
                    }
                    else
                    {
                        throw new Exception("登录失败");
                    }
                }
                else
                {
                    throw new Exception("验证码错误");
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { state = "error", msg = ex.Message }.ToJson());
            }
        }
    }
}