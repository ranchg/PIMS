using SSI.Business.ProductManage;
using SSI.Entity.ProductManage;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SSI.Web.Areas.ProductManage.Controllers
{
    public class ProductMakeController : BaseController
    {
        ProductMakeBLL ProductMakeBLL = new ProductMakeBLL();
        delegate void SetCusumeNum(T_Product_Make t_Product_Make);
        [HttpGet]
        [AuthAction]
        public virtual ActionResult IndexProduct()
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
                rows = GetProductMakeList(ProductMakeBLL.GetGridList(gp)),
                total = gp.total
            };
            return Content(data.ToJson());
        }

        private object GetProductMakeList(List<T_Product_Make> ProductMakes)
        {
            return ProductMakes.Select(e => new
            {
                F_Id = e.F_Id,
                F_Product_Name = new ProductBLL().GetForm(e.F_Product_Id).F_Name,
                F_Quantity = e.F_Quantity,
                F_Make_Date = e.F_Make_Date,
                F_Is_Read = e.F_Is_Read,
                F_Enable_Mark = e.F_Enable_Mark,
                F_Create_Time = e.F_Create_Time
            });
        }

        [HttpPost]
        [AuthAction]
        public FileContentResult ExportExcel(string field)
        {
            DataTable dataTable = ProductMakeBLL.ExportExcel(field);
            return File(ExcelHelper.ExportToContent(dataTable, sheetTitle: "产品生产"), "application/ms-excel", "产品生产.xls");
        }

        [HttpGet]
        [HttpAjax]
        public ActionResult GetForm(string F_Id)
        {
            T_Product_Make t_Product_Make = ProductMakeBLL.GetForm(F_Id);
            return Content(new
            {
                F_Quantity = t_Product_Make.F_Quantity,
                F_Make_Date = t_Product_Make.F_Make_Date.Value.ToString("yyyy-MM-dd"),
                F_Product_Name = new ProductBLL().GetForm(t_Product_Make.F_Product_Id).F_Name,
                F_Product_Id = t_Product_Make.F_Product_Id
            }.ToJson());
        }

        [HttpPost]
        [HttpAjax]
        public ActionResult SubmitForm(T_Product_Make t_Product_Make)
        {
            ProductMakeBLL.SubmitForm(t_Product_Make);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DeleteForm(string F_Id)
        {
            ProductMakeBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult EnableForm(string F_Id)
        {
            T_Product_Make t_Product_Make = ProductMakeBLL.GetForm(F_Id);
            t_Product_Make.F_Enable_Mark = 1;
            ProductMakeBLL.SubmitForm(t_Product_Make);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult DisableForm(string F_Id)
        {
            T_Product_Make t_Product_Make = ProductMakeBLL.GetForm(F_Id);
            t_Product_Make.F_Enable_Mark = 0;
            ProductMakeBLL.SubmitForm(t_Product_Make);
            return Success("操作成功");
        }

        [HttpPost]
        [HttpAjax]
        [AuthAction]
        public ActionResult CountPart(string F_Id)
        {
            ProductMakeBLL.CountPart(F_Id);
            return Success("操作成功");
        }
        [HttpGet]
        [HttpAjax]
        [AuthAction]
        public ActionResult GetProductList(GridParam gp)
        {
            List<T_Product> list = new ProductBLL().GetGridList(gp).ToList();
            var data = new
            {
                rows = list,
                total = gp.total
            };
            return Content(data.ToJson());
        }
    }
}