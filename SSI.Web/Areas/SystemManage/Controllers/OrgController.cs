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
            return Content(data.Select(e => new
            {
                F_Id = int.Parse(e.F_Id),
                F_Parent_Id = int.Parse(e.F_Parent_Id),
                F_Name = e.F_Name,
                F_Manager = e.F_Manager,
                F_Phone = e.F_Phone,
                F_Icon = e.F_Icon,
                F_Type_Mark = e.F_Type_Mark,
                F_Enable_Mark = e.F_Enable_Mark,
                F_Create_Time = e.F_Create_Time
            }).ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Id)
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
        public ActionResult DeleteForm(string F_Id)
        {
            orgBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(string F_Id)
        {
            T_Org t_Org = orgBLL.GetForm(F_Id);
            t_Org.F_Enable_Mark = 1;
            orgBLL.SubmitForm(t_Org);
            return Success("操作成功");
        }


        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(string F_Id)
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
            var data = GetOrgTree(new OrgBLL().GetList(), "0");
            return Content(data.ToJson());
        }

        private object GetOrgTree(List<T_Org> orgs, string parentId)
        {
            return orgs.Where(e => e.F_Parent_Id == parentId).Select(e => new { id = e.F_Id, name = e.F_Name, open = true, children = GetOrgTree(orgs, e.F_Id) });
        }
    }
}