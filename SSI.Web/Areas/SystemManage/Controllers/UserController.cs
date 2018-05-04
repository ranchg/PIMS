using SSI.Business.SystemManage;
using SSI.Entity.SystemManage;
using SSI.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web.Areas.SystemManage.Controllers
{
    public class UserController : BaseController
    {
        UserBLL userBLL = new UserBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = userBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Id)
        {
            var data = userBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_User t_User, HttpPostedFileBase F_Head_Icon_File)
        {
            if (F_Head_Icon_File != null)
            {
                t_User.F_Head_Icon = UploadHelper.ImageSaveSquare(F_Head_Icon_File, "UserHeadIcon", 200);
            }

            userBLL.SubmitForm(t_User);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(string F_Id)
        {
            userBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(string F_Id)
        {
            T_User t_User = userBLL.GetForm(F_Id);
            t_User.F_Enable_Mark = 1;
            userBLL.SubmitForm(t_User);
            return Success("操作成功");
        }


        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(string F_Id)
        {
            T_User t_User = userBLL.GetForm(F_Id);
            t_User.F_Enable_Mark = 0;
            userBLL.SubmitForm(t_User);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult ResetForm(string F_Id)
        {
            T_User t_User = userBLL.GetForm(F_Id);
            t_User.F_Password = Md5Helper.MD5("123456", 0x20).ToUpper();
            userBLL.SubmitForm(t_User);
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
        public ActionResult SubmitAuthorize(string F_Id, string Roles)
        {
            new UserRoleBLL().DeleteByUserId(F_Id);
            new UserRoleBLL().InsertFormBatch(Roles.JsonToList<string>().Select(e => new T_User_Role { F_User_Id = F_Id, F_Role_Id = e, F_Enable_Mark = 1 }).ToList());
            return Success("操作成功");
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetAuthorize(string F_Id)
        {
            var data = GetTree(new OrgBLL().GetList(), new RoleBLL().GetList(), new RoleBLL().GetListByUserId(F_Id), "1");
            return Content(data.ToJson());
        }

        private object GetTree(List<T_Org> orgList, List<T_Role> roleList, List<T_Role> userList, string parentId)
        {
            var listObj = new List<object>();
            roleList.FindAll(e => e.F_Org_Id == parentId).ForEach(e =>
            {
                if (userList.Count(u => u.F_Id == e.F_Id) > 0)
                {
                    listObj.Add(new { id = e.F_Id, name = e.F_Name, isParent = false, role = true, @checked = true });
                }
                else
                {
                    listObj.Add(new { id = e.F_Id, name = e.F_Name, isParent = false, role = true });
                }
            });
            orgList.FindAll(e => e.F_Parent_Id == parentId).ForEach(e =>
            {
                listObj.Add(new { id = e.F_Id, name = e.F_Name, isParent = true, open = true, nocheck = true, children = GetTree(orgList, roleList, userList, e.F_Id) });
            });
            return listObj;
        }
    }
}