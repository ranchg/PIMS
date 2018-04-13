using SSI.Business.SystemManage;
using SSI.Utilities;
using System;
using System.Data;
using System.Web.Mvc;

namespace SSI.Web.Areas.SystemManage.Controllers
{
    public class UserLogController : BaseController
    {
        UserLogBLL userLogBLL = new UserLogBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = userLogBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult Export(string field, string query)
        {
            DataTable dataTable = userLogBLL.Export(field, query);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "系统操作日志"), "application/ms-excel", "系统操作日志.xls");
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(int F_Id)
        {
            var data = userLogBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult OneWeek()
        {
            userLogBLL.Remove(7);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult OneMonth()
        {
            userLogBLL.Remove(1);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult ThreeMonth()
        {
            userLogBLL.Remove(3);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult RemoveAll()
        {
            userLogBLL.Remove(0);
            return Success("操作成功");
        }
    }
}