using SSI.Business.BomManage;
using SSI.Business.PartManage;
using SSI.Business.ProductManage;
using SSI.Entity.BomManage;
using SSI.Entity.Manage;
using SSI.Entity.PartManage;
using SSI.Entity.ProductManage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web.Areas.BomManage.Controllers
{
    public class BomController : BaseController
    {
        BomBLL bomBLL = new BomBLL();

        [HttpGet]
        [AuthAction]
        public ActionResult Detail()
        {
            return View();
        }

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = bomBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Id)
        {
            var data = bomBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Bom t_Bom)
        {
            bomBLL.SubmitForm(t_Bom);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(string F_Id)
        {
            bomBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        public ActionResult GetBomDetails(string F_Id)
        {
            List<T_Bom_Detail> list = new Repository<T_Bom_Detail>().FindList(string.Format("AND F_BOM_ID={0}", F_Id));
            var returnJson = new
            {
                Msg = "warning"
            };  
            if (list.Count > 0) return Json(returnJson,JsonRequestBehavior.AllowGet);
            else return DeleteForm(F_Id);
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult ExportExcel(string field, string query)
        {
            DataTable dataTable = bomBLL.ExportExcel(field, query);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "BOM信息"), "application/ms-excel", "BOM信息.xls");
        }

        [HttpPost]
        public FileContentResult DownloadData()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("零件编码", typeof(string));
            dataTable.Columns.Add("零件数量", typeof(decimal));
            return File(ExcelHelper.ExportToContent(dataTable), "application/ms-excel", "BOM明细.xls");
        }

        [HttpPost]
        public ActionResult UpdateData(string F_Id,HttpPostedFileBase file)
        {
            if (bomBLL.UpdateData(F_Id, ExcelHelper.ImportToDataTable(file.InputStream)))
                return Success("导入成功");
            else
                return Warning("导入失败");
        }
    }
}