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
    public class PartCheckController : BaseController
    {
        PartCheckBLL partCheckBLL = new PartCheckBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = partCheckBLL.GetGridList(gp),
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

        [HttpGet]
        public virtual ActionResult IndexPart()
        {
            return View();
        }

        [HttpPost]
        public FileContentResult Download()
        {
            string field = "[{\"ParamTitle\":\"名称\",\"ParamField\":\"F_Name\"},{\"ParamTitle\":\"编码\",\"ParamField\":\"F_Code\"},{\"ParamTitle\":\"规格\",\"ParamField\":\"F_Spec\"},{\"ParamTitle\":\"单位\",\"ParamField\":\"F_Unit\"}]";
            DataTable dataTable = new PartBLL().Export(field, string.Empty);
            dataTable.Columns.Add("数量", typeof(int));
            return File(ExcelHelper.ExportToContent(dataTable), "application/ms-excel", "盘点清单.xls");
        }

        [HttpGet]
        public ActionResult FormCheck()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitFormCheck(T_Part_Check_Time t_Part_Check_Time, HttpPostedFileBase F_Check_List)
        {
            if (F_Check_List != null)
            {
                DataTable dt_Part_Check = ExcelHelper.ImportToDataTable(F_Check_List.InputStream);
                partCheckBLL.Check(t_Part_Check_Time, dt_Part_Check);
                return Success("操作成功");
            }
            else
            {
                return Error("请上传盘点清单");
            }
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult Export(string field, string query)
        {
            DataTable dataTable = partCheckBLL.Export(field, query);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "零件盘点"), "application/ms-excel", "零件盘点.xls");
        }
    }
}