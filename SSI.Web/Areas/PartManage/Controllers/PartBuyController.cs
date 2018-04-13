using SSI.Business.PartManage;
using SSI.Entity.PartManage;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web.Areas.PartManage.Controllers
{
    public class PartBuyController : BaseController
    {
        PartBuyBLL partBuyBLL = new PartBuyBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = partBuyBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(int F_Id)
        {
            var data = partBuyBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult IndexPart()
        {
            return View();
        }

        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Part_Buy t_Part_Buy)
        {
            partBuyBLL.SubmitForm(t_Part_Buy);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(int F_Id)
        {
            partBuyBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult Export(string field, string query)
        {
            DataTable dataTable = partBuyBLL.Export(field, query);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "零件采购"), "application/ms-excel", "零件采购.xls");
        }
    }
}