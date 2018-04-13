using SSI.Business.SystemManage;
using SSI.Entity.SystemManage;
using SSI.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSI.Web.Areas.SystemManage.Controllers
{
    public class MenuController : BaseController
    {
        MenuBLL menuBLL = new MenuBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = menuBLL.GetList();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(int F_Id)
        {
            var data = menuBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Menu t_Menu)
        {
            menuBLL.SubmitForm(t_Menu);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(int F_Id)
        {
            menuBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(int F_Id)
        {
            T_Menu t_Menu = menuBLL.GetForm(F_Id);
            t_Menu.F_Enable_Mark = 1;
            menuBLL.SubmitForm(t_Menu);
            return Success("操作成功");
        }


        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(int F_Id)
        {
            T_Menu t_Menu = menuBLL.GetForm(F_Id);
            t_Menu.F_Enable_Mark = 0;
            menuBLL.SubmitForm(t_Menu);
            return Success("操作成功");
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetMenu()
        {
            var data = GetMenuTree(new MenuBLL().GetList(), 0);
            return Content(data.ToJson());
        }

        private object GetMenuTree(List<T_Menu> menus, int parentId)
        {
            return menus.Where(e => e.F_Parent_Id == parentId).Select(e => new { id = e.F_Id, name = e.F_Name, open = true, children = GetMenuTree(menus, e.F_Id) });
        }
    }
}