using SSI.Business.BomManage;
using SSI.Business.PartManage;
using SSI.Business.ProductManage;
using SSI.Entity.BomManage;
using SSI.Entity.Manage;
using SSI.Entity.PartManage;
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
        public ActionResult GetForm(int F_Id)
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
        public ActionResult DeleteForm(int F_Id)
        {
            bomBLL.DeleteForm(F_Id);
            return Success("操作成功");
        }

        public ActionResult GetBomDetails(int F_Id)
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
        public ActionResult UpdateData(int F_Id,HttpPostedFileBase file)
        {
            
            DataTable dataTable = ExcelHelper.ImportToDataTable(file.InputStream);
            List<T_Bom_Detail> list = new List<T_Bom_Detail>();
            DataTable dtOne = new PartBLL().GetPart();

            var query = from t in dataTable.AsEnumerable()
                        group t by new { t1 = t.Field<string>("零件编码") } into m
                        select new
                        {
		                    pob = m.Key.t1,
                            rowcount = m.Count()
                        };
            int j = 0;
            foreach (var q in query)
            {
                j++;
                if (q.rowcount > 1)
                {
                    throw new Exception("数据源第" + Convert.ToInt32(j + 1) + "行零件编码有重复,请检查");
                }
            }

            for (int i=0; i<dataTable.Rows.Count;i++)
            {
                DataRow[] drOne;
                if (!string.IsNullOrEmpty(dataTable.Rows[i]["零件编码"].ToString()))
	            {
                    try
                    {
                        drOne = dtOne.Select("F_CODE='" + dataTable.Rows[i]["零件编码"].ToString().PadRight(20, '0') + "'");                        
                    }
                    catch (Exception)
                    {
                        throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件编码异常,请检查");
                    }
	            }
                else
                {
                    throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件编码为空,请检查");
                }
                if (drOne.Length!=0)
                {
                    T_Bom_Detail t_Bom_Detail = new T_Bom_Detail();
                    try
                    {
                        t_Bom_Detail.F_Num = int.Parse(dataTable.Rows[i]["零件数量"].ToString());
                    }
                    catch (Exception)
                    {
                        throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件数量异常,请检查");
                    }
                    t_Bom_Detail.F_Part_Id = int.Parse(drOne[0]["F_ID"].ToString());
                    t_Bom_Detail.F_Bom_Id = F_Id;
                    t_Bom_Detail.F_Create_Time = DateTime.Now;
                    t_Bom_Detail.F_Create_By = ManageProvider.Provider.Current().User.F_Account;
                    list.Add(t_Bom_Detail);
                }
                else
                {
                    throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件编码不存在,请检查");
                }
                
            }
            var isOpenTrans = new Repository<T_Bom_Detail>().BeginTrans();
            try
            {
                new BomDetailBLL().Modify(F_Id, isOpenTrans);
                new BomDetailBLL().InsertFormBatch(list, isOpenTrans);
                new Repository<T_Bom_Detail>().Commit();
                return Success("操作成功");
            }
            catch (Exception)
            {
                new Repository<T_Bom_Detail>().Rollback();
                throw;
            }            
        }
    }
}