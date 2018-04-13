using SSI.Business.BomManage;
using SSI.Business.PartManage;
using SSI.Entity.BomManage;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web.Areas.BomManage.Controllers
{
    public class BomDetailController : BaseController
    {
        BomDetailBLL bomDetailBLL = new BomDetailBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = bomDetailBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(int F_Id)
        {
            var data = bomDetailBLL.getForm(F_Id);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(string F_Id, string F_Num)
        {

            bomDetailBLL.SubmitForm(F_Id, F_Num);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(int F_Id)
        {
            bomDetailBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult ExportExcel(string field, string query)
        {
            DataTable dataTable = bomDetailBLL.ExportExcel(field, query);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "BOM明细信息"), "application/ms-excel", "BOM明细信息.xls");
        }


        

        [HttpGet]
        [HttpAjax]
        public ActionResult GetBom()
        {
            var data = new BomBLL().GetBom();
            return Content(data.ToJson());
        }
        

        

        [HttpGet]
        [HttpAjax]
        public ActionResult GetDetailById(int f_id,GridParam gp)
        {
            var data = new
            {
                rows = bomDetailBLL.GetDetailById(f_id, gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }
    }
}