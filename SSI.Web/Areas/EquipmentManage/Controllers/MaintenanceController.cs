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
    public class MaintenanceController : BaseController
    {
        MaintenanceBLL maintenanceBLLBLL = new MaintenanceBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp, string F_Equipment_Id = null)
        {
            var data = new
            {
                rows = maintenanceBLLBLL.GetGridList(gp, F_Equipment_Id),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Id)
        {
            var data = maintenanceBLLBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Maintenance t_Maintenance)
        {
            maintenanceBLLBLL.SubmitForm(t_Maintenance);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(string F_Id)
        {
            maintenanceBLLBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpGet]
        public ActionResult IndexEquipment()
        {
            return View();
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult Export(string field, string query)
        {
            DataTable dataTable = maintenanceBLLBLL.Export(field, query);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "设备维护"), "application/ms-excel", "设备维护.xls");
        }
    }
}