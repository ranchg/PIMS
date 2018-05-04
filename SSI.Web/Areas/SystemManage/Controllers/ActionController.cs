using SSI.Business.SystemManage;
using SSI.Entity.SystemManage;
using SSI.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSI.Web.Areas.SystemManage.Controllers
{
    public class ActionController : BaseController
    {
        ActionBLL actionBLL = new ActionBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = actionBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Id)
        {
            var data = actionBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Action t_Action)
        {
            actionBLL.SubmitForm(t_Action);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(string F_Id)
        {
            actionBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(string F_Id)
        {
            T_Action t_Action = actionBLL.GetForm(F_Id);
            t_Action.F_Enable_Mark = 1;
            actionBLL.SubmitForm(t_Action);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(string F_Id)
        {
            T_Action t_Action = actionBLL.GetForm(F_Id);
            t_Action.F_Enable_Mark = 0;
            actionBLL.SubmitForm(t_Action);
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