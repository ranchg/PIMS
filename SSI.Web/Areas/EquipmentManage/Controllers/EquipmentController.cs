using SSI.Business.SystemManage;
using SSI.Entity.EquipmentManage;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web.Areas.EquipmentManage.Controllers
{
    public class EquipmentController : BaseController
    {
        EquipmentBLL equipmentBLL = new EquipmentBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = equipmentBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Id)
        {
            var data = equipmentBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Equipment t_Equipment)
        {
            equipmentBLL.SubmitForm(t_Equipment);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(string F_Id)
        {
            equipmentBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpGet]
        public ActionResult IndexMaintenance()
        {
            return View();
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult Export(string field, string query)
        {
            DataTable dataTable = equipmentBLL.Export(field, query);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "设备信息"), "application/ms-excel", "设备信息.xls");
        }
    }
}