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
            return Content(data.Select(e => new
            {
                F_Id = int.Parse(e.F_Id),
                F_Parent_Id = int.Parse(e.F_Parent_Id),
                F_Name = e.F_Name,
                F_Target = e.F_Target,
                F_Icon = e.F_Icon,
                F_Sort = e.F_Sort,
                F_Enable_Mark = e.F_Enable_Mark,
                F_Create_Time = e.F_Create_Time
            }).ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Id)
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
        public ActionResult DeleteForm(string F_Id)
        {
            menuBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(string F_Id)
        {
            T_Menu t_Menu = menuBLL.GetForm(F_Id);
            t_Menu.F_Enable_Mark = 1;
            menuBLL.SubmitForm(t_Menu);
            return Success("操作成功");
        }


        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(string F_Id)
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
            var data = GetMenuTree(new MenuBLL().GetList(), "0");
            return Content(data.ToJson());
        }

        private object GetMenuTree(List<T_Menu> menus, string parentId)
        {
            return menus.Where(e => e.F_Parent_Id == parentId).Select(e => new { id = e.F_Id, name = e.F_Name, open = true, children = GetMenuTree(menus, e.F_Id) });
        }
    }
}