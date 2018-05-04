using SSI.Business.PartManage;
using SSI.Entity.PartManage;
using SSI.Utilities;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System;

namespace SSI.Web.Areas.PartManage.Controllers
{
    public class PartController : BaseController
    {
        PartBLL partBLL = new PartBLL();

        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = partBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Code)
        {
            var data = partBLL.GetForm(F_Code);
            return Content(data.ToJson());
        }

        //[HttpPost]
        //[HttpAjax]
        //public ActionResult SubmitForm(T_Part t_Part)
        //{
        //    List<T_Part> list = partBLL.GetList();
        //    if ((t_Part.F_Id == 0 && list.Any(e => e.F_Code == t_Part.F_Code)) ||
        //        (t_Part.F_Id != 0 && list.Any(e => e.F_Code == t_Part.F_Code) && partBLL.GetForm(t_Part.F_Id).F_Code != t_Part.F_Code))
        //    {
        //        throw new Exception(string.Format("已存在零件号：{0}", t_Part.F_Code));
        //    }
        //    partBLL.SubmitForm(t_Part);
        //    return Success("操作成功");
        //}

        //[HttpPost]
        //[HttpAjax]
        //[AuthAction]
        //public ActionResult DeleteForm(int F_Id)
        //{
        //    partBLL.DeleteForm(F_Id);
        //    return Success("操作成功");
        //}

        //[HttpPost]
        //[AuthAction]
        //public FileContentResult Download()
        //{
        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("名称", typeof(string));
        //    dataTable.Columns.Add("编码", typeof(string));
        //    dataTable.Columns.Add("规格", typeof(string));
        //    dataTable.Columns.Add("单位", typeof(string));
        //    return File(ExcelHelper.ExportToContent(dataTable), "application/ms-excel", "零件信息.xls");
        //}

        //[HttpPost]
        //[AuthAction]
        //public ActionResult Import(HttpPostedFileBase file)
        //{
        //    DataTable dataTable = ExcelHelper.ImportToDataTable(file.InputStream);
        //    List<T_Part> listOldPart = partBLL.GetList();
        //    List<T_Part> listNewPart = new List<T_Part>();
        //    foreach (DataRow dr in dataTable.Rows)
        //    {
        //        T_Part t_Part = new T_Part();
        //        t_Part.F_Name = dr["名称"].ToString();
        //        t_Part.F_Code = dr["编码"].ToString();
        //        t_Part.F_Spec = dr["规格"].ToString();
        //        t_Part.F_Unit = dr["单位"].ToString();
        //        if (listOldPart.Any(e => e.F_Code == t_Part.F_Code) || listNewPart.Any(e => e.F_Code == t_Part.F_Code))
        //        {
        //            throw new Exception(string.Format("已存在零件号：{0}", t_Part.F_Code));
        //        }
        //        listNewPart.Add(t_Part);
        //    }
        //    partBLL.InsertFormBatch(listNewPart);
        //    return Success("操作成功");
        //}

        [HttpPost]
        [AuthAction]
        public FileContentResult Export(string field, string search)
        {
            DataTable dataTable = partBLL.Export(field, search);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "零件信息"), "application/ms-excel", "零件信息.xls");
        }
    }
}