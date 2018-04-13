using SSI.Business.PartManage;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web.Areas.PartManage.Controllers
{
    public class PartStockController : BaseController
    {
        PartStockBLL partStockBLL = new PartStockBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = partStockBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetPartCheckTimeList()
        {
            var data = new PartCheckTimeBLL().GetList();
            return Content(data.ToJson());
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult Export(string field, string query)
        {
            DataTable dataTable = partStockBLL.Export(field, query);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "零件实时库存"), "application/ms-excel", "零件实时库存.xls");
        }
    }
}