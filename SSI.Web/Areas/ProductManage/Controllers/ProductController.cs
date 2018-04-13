using SSI.Business.ProductManage;
using SSI.Entity.ProductManage;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace SSI.Web.Areas.ProductManage.Controllers
{
    public class ProductController : BaseController
    {
        ProductBLL ProductBLL = new ProductBLL();
        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="gp"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetGridList(GridParam gp)
        {
            var data = new
            {
                rows = ProductBLL.GetGridList(gp),
                total = gp.total
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 导出操作
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthAction]
        public FileContentResult ExportExcel(string field)
        {
            DataTable dataTable = ProductBLL.ExportExcel(field);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "产品"), "application/ms-excel", "产品.xls");
        }
        /// <summary>
        /// 根据F_Id获取数据
        /// </summary>
        /// <param name="F_Id"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(int F_Id)
        {
            var data = ProductBLL.GetForm(F_Id);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 修改或添加操作
        /// </summary>
        /// <param name="t_Product"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Product t_Product)
        {
            ProductBLL.SubmitForm(t_Product);
            return Success("操作成功");
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="F_Id"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(int F_Id)
        {
            ProductBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        /// <summary>
        /// 启用操作
        /// </summary>
        /// <param name="F_Id"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(int F_Id)
        {
            T_Product t_Product = ProductBLL.GetForm(F_Id);
            t_Product.F_Enable_Mark = 1;
            ProductBLL.SubmitForm(t_Product);
            return Success("操作成功");
        }

        /// <summary>
        /// 不启用操作
        /// </summary>
        /// <param name="F_Id"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(int F_Id)
        {
            T_Product t_Product = ProductBLL.GetForm(F_Id);
            t_Product.F_Enable_Mark = 0;
            ProductBLL.SubmitForm(t_Product);
            return Success("操作成功");
        }
        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public FileContentResult DownloadData()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("名称", typeof(string));
            dataTable.Columns.Add("编码", typeof(string));
            dataTable.Columns.Add("规格", typeof(string));
            dataTable.Columns.Add("单位", typeof(string));
            return File(ExcelHelper.ExportToContent(dataTable), "application/ms-excel", "产品模板.xls");
        }
        /// <summary>
        /// 导入模板
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthAction]
        public ActionResult UpdateData(HttpPostedFileBase file)
        {
            DataTable dataTable = ExcelHelper.ImportToDataTable(file.InputStream);
            List<T_Product> list = new List<T_Product>();
            foreach (DataRow dr in dataTable.Rows)
            {
                T_Product t_Product = new T_Product();
                t_Product.F_Name = dr["名称"].ToString().Trim();
                t_Product.F_Code = dr["编码"].ToString().Trim();
                t_Product.F_Spec = dr["规格"].ToString().Trim();
                t_Product.F_Unit = dr["单位"].ToString().Trim();
                t_Product.F_Enable_Mark = 1;
                list.Add(t_Product);
            }
            ProductBLL.InsertFormBatch(list);
            return Success("操作成功");
        }

        [HttpGet]
        public ActionResult IndexProduct()
        {
            return View();
        }
    }
}