using SSI.Business.SystemManage;
using SSI.Entity.SystemManage;
using SSI.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSI.Web.Areas.SystemManage.Controllers
{
    public class RoleController : BaseController
    {
        RoleBLL roleBLL = new RoleBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = roleBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(int F_Id)
        {
            var data = roleBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Role t_Role)
        {
            roleBLL.SubmitForm(t_Role);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(int F_Id)
        {
            roleBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(int F_Id)
        {
            T_Role t_Role = roleBLL.GetForm(F_Id);
            t_Role.F_Enable_Mark = 1;
            roleBLL.SubmitForm(t_Role);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(int F_Id)
        {
            T_Role t_Role = roleBLL.GetForm(F_Id);
            t_Role.F_Enable_Mark = 0;
            roleBLL.SubmitForm(t_Role);
            return Success("操作成功");
        }

        [HttpGet]
        [AuthAction]
        public ActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitAuthorize(int F_Id, string Actions)
        {
            new RoleActionBLL().DeleteByRoleId(F_Id);
            new RoleActionBLL().InsertFormBatch(Actions.JsonToList<int>().Select(e => new T_Role_Action { F_Role_Id = F_Id, F_Action_Id = e, F_Enable_Mark = 1 }).ToList());
            return Success("操作成功");
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetAuthorize(int F_Id)
        {
            var data = GetAuthorizeTree(new MenuBLL().GetList(), new ActionBLL().GetList(), new ActionBLL().GetListByRoleId(F_Id), 1);
            return Content(data.ToJson());
        }

        private object GetAuthorizeTree(List<T_Menu> menuList, List<T_Action> actionList, List<T_Action> roleList, int parentId)
        {
            var listObj = new List<object>();
            actionList.FindAll(e => e.F_Menu_Id == parentId).ForEach(e =>
            {
                if (roleList.Count(u => u.F_Id == e.F_Id) > 0)
                {
                    listObj.Add(new { id = e.F_Id, name = e.F_Name, isParent = false, action = true, @checked = true });
                }
                else
                {
                    listObj.Add(new { id = e.F_Id, name = e.F_Name, isParent = false, action = true });
                }
            });
            menuList.FindAll(e => e.F_Parent_Id == parentId).ForEach(e =>
            {
                listObj.Add(new { id = e.F_Id, name = e.F_Name, isParent = true, open = true, nocheck = true, children = GetAuthorizeTree(menuList, actionList, roleList, e.F_Id) });
            });
            return listObj;
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetOrg()
        {
            var data = GetOrgTree(new OrgBLL().GetList(), 1);
            return Content(data.ToJson());
        }

        private object GetOrgTree(List<T_Org> orgs, int parentId)
        {
            return orgs.Where(e => e.F_Parent_Id == parentId).Select(e => new { id = e.F_Id, name = e.F_Name, open = true, children = GetOrgTree(orgs, e.F_Id) });
        }
    }
}