using SSI.Business.SystemManage;
using SSI.Entity.SystemManage;
using SSI.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSI.Web.Areas.SystemManage.Controllers
{
    public class OrgController : BaseController
    {
        OrgBLL orgBLL = new OrgBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = orgBLL.GetList();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(int F_Id)
        {
            var data = orgBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Org t_Org)
        {
            orgBLL.SubmitForm(t_Org);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(int F_Id)
        {
            orgBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(int F_Id)
        {
            T_Org t_Org = orgBLL.GetForm(F_Id);
            t_Org.F_Enable_Mark = 1;
            orgBLL.SubmitForm(t_Org);
            return Success("操作成功");
        }


        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(int F_Id)
        {
            T_Org t_Org = orgBLL.GetForm(F_Id);
            t_Org.F_Enable_Mark = 0;
            orgBLL.SubmitForm(t_Org);
            return Success("操作成功");
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetOrg()
        {
            var data = GetOrgTree(new OrgBLL().GetList(), 0);
            return Content(data.ToJson());
        }

        private object GetOrgTree(List<T_Org> orgs, int parentId)
        {
            return orgs.Where(e => e.F_Parent_Id == parentId).Select(e => new { id = e.F_Id, name = e.F_Name, open = true, children = GetOrgTree(orgs, e.F_Id) });
        }
    }
}