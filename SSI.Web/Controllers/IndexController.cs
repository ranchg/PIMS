using SSI.Business.SystemManage;
using SSI.Entity.Manage;
using SSI.Entity.SystemManage;
using SSI.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web.Controllers
{
    public class IndexController : Controller
    {
        UserBLL userBLL = new UserBLL();

        [AuthIndex]
        public ActionResult Index()
        {
            return View();
        }
        [AuthIndex]
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult Default()
        {
            return View();
        }

        [HttpPost]
        [HttpAjax]
        public ActionResult GetClientData()
        {
            var data = new
            {
                user = ManageProvider.Provider.Current(),
                menu = GetMenu(ManageProvider.Provider.Current().Menus, 1)
            };
            return Content(data.ToJson());
        }

        public ActionResult FormUserInfo()
        {
            return View();
        }

        [AuthIndex]
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitFormUserInfo(T_User t_User, HttpPostedFileBase F_Head_Icon_File)
        {
            if (F_Head_Icon_File != null)
            {
                t_User.F_Head_Icon = UploadHelper.ImageSaveSquare(F_Head_Icon_File, "UserHeadIcon", 200);
            }
            t_User.F_Id = ManageProvider.Provider.Current().User.F_Id;
            t_User.F_Online_Mark = ManageProvider.Provider.Current().User.F_Online_Mark;
            t_User.F_System_Mark = ManageProvider.Provider.Current().User.F_System_Mark;
            t_User.F_Enable_Mark = ManageProvider.Provider.Current().User.F_Enable_Mark;
            t_User.F_Delete_Mark = ManageProvider.Provider.Current().User.F_Delete_Mark;
            userBLL.SubmitForm(t_User);
            userBLL.AddProvider(t_User);
            return Content(new JsonMessage { state = "success", msg = "操作成功"}.ToJson());
        }

        public ActionResult FormUserPassword()
        {
            return View();
        }

        [AuthIndex]
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitFormUserPassword(string F_Password, string F_New_Password, string F_Confirm_New_Password)
        {
            if (ManageProvider.Provider.Current().User.F_Password == Md5Helper.MD5(F_Password, 0x20).ToUpper())
            {
                T_User t_User = ManageProvider.Provider.Current().User;
                t_User.F_Password = Md5Helper.MD5(F_New_Password, 0x20).ToUpper();
                userBLL.SubmitForm(t_User);
                userBLL.AddProvider(t_User);
                return Content(new JsonMessage { state = "success", msg = "操作成功" }.ToJson());
            }
            else
            {
                return Content(new JsonMessage { state = "warning", msg = "原密码错误" }.ToJson());
            }
        }

        [AuthIndex]
        [HttpPost]
        [HttpAjax]
        public ActionResult Logout()
        {
            ManageProvider.Provider.EmptyCurrent();
            Session.Abandon();
            Session.Clear();
            return Content(new JsonMessage { state = "success", msg = "退出成功", data = new { url = "/Login/Index" } }.ToJson());
        }
        private object GetMenu(List<T_Menu> menus, int parentId)
        {
            return menus.Where(e => e.F_Parent_Id == parentId).Select(e => new { id = e.F_Id, text = e.F_Name, icon = e.F_Icon, url = e.F_Target, children = GetMenu(menus, e.F_Id) });
        }
    }
}