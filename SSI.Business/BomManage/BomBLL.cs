using SSI.Business.PartManage;
using SSI.Business.ProductManage;
using SSI.Entity.BomManage;
using SSI.Entity.Manage;
using SSI.Entity.PartManage;
using SSI.Entity.ProductManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSI.Business.BomManage
{
    public class BomBLL : RepositoryFactory<T_Bom>
    {
        

        public DataTable GetGridList(GridParam gp)
        {
            StringBuilder sb = new StringBuilder(@"SELECT * FROM (SELECT T1.*,T2.F_NAME AS F_PRODUCT_NAME FROM T_BOM T1 INNER JOIN T_PRODUCT T2 ON T1.F_PRODUCT_ID=T2.F_ID WHERE T1.F_DELETE_MARK = 0) T2 WHERE 1=1");
            if (!string.IsNullOrEmpty(gp.query))
            {
                sb.Append(ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>()));
            }
            return Repository().FindTablePageBySql(sb.ToString(), ref gp);
        }


        public DataTable ExportExcel(string field, string query)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from = @"SELECT T2.F_NAME AS F_PRODUCT_NAME,T1.F_NAME,T1.F_CODE,T1.F_VERSION,T1.F_DATE,
                            CASE T1.F_ENABLE_MARK WHEN 1 THEN '有效' ELSE '无效' END F_ENABLE_MARK ,
                            T1.F_CREATE_BY,T1.F_CREATE_TIME FROM T_BOM T1 INNER JOIN T_PRODUCT T2 ON T1.F_PRODUCT_ID=T2.F_ID 
                            WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            if (!string.IsNullOrEmpty(query))
            {
                where += ConditionBuilder.GetWhereSql2(query.JsonToList<Condition>());
            }
            string orderby = "order by 创建时间 DESC";
            string sql = string.Format("{0} ({1}) T {2} {3}", select, from, where, orderby);
            return Repository().FindTableBySql(sql);
        }


        public DataTable GetBom()
        {
            string querySql = "SELECT * FROM T_BOM WHERE F_DELETE_MARK=0";
            return Repository().FindTableBySql(querySql);
        }

        public bool UpdateData(string F_Id, DataTable totalTable)
        {
            DataTable bomTable = new DataTable();
            bomTable.Columns.Add("零件编码", typeof(System.String));
            bomTable.Columns.Add("零件数量", typeof(System.Int32));

            List<T_Product> productList = new ProductBLL().GetList();
            List<T_Part> partList = new PartBLL().GetList();
            T_Product t_product=new T_Product();

            //判断产品，添加产品
            if (productList.Where(x => x.F_Name == totalTable.Rows[1]["Column2"].ToString()).ToList().Count == 0)
            {
                if (productList.Where(x => x.F_Code == totalTable.Rows[2]["Column2"].ToString()).ToList().Count == 0)
                {
                    //添加产品
                    try
                    {
                        if (!string.IsNullOrEmpty(totalTable.Rows[1]["Column2"].ToString()))
                            t_product.F_Name = totalTable.Rows[1]["Column2"].ToString();
                        else
                            throw new Exception("产品名称不能为空,请检查");
                    }
                    catch (Exception)
                    {
                        throw new Exception("产品名称异常,请检查");
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(totalTable.Rows[2]["Column2"].ToString()))
                            t_product.F_Code = totalTable.Rows[2]["Column2"].ToString();
                        else
                            throw new Exception("产品编码不能为空,请检查");
                    }
                    catch (Exception)
                    {
                        throw new Exception("产品编码异常,请检查");
                    }
                    t_product.F_Create_By = ManageProvider.Provider.Current().User.F_Account;
                    t_product.F_Create_Time = DateTime.Now;
                }
                else
                {
                    throw new Exception("产品编码重复,请检查");
                }
            }
            

            //添加零件，分解出bom详情table
            for (int i = 5; i < totalTable.Rows.Count; i++)
            {
                DataRow dr = bomTable.NewRow();
                try 
	            {
                    if (!string.IsNullOrEmpty(totalTable.Rows[i]["Column2"].ToString()))
                        if (partList.Select(x=>x.F_Code.PadRight(12,'0')==totalTable.Rows[i]["Column2"].ToString().PadRight(12, '0')).ToList().Count>0)
                            dr["零件编码"] = totalTable.Rows[i]["Column2"].ToString().PadRight(12, '0');
                        else
                            throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件编码不存在,请检查");
                    else
                        throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件编码为空,请检查");
	            }
	            catch (Exception)
	            {
                    throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件编码异常,请检查");
	            }
                try
                {
                    if (!string.IsNullOrEmpty(totalTable.Rows[i]["Column5"].ToString()))
                        dr["零件数量"] = int.Parse(totalTable.Rows[i]["Column5"].ToString());
                    else
                        throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件数量为空,请检查");
                }
                catch (Exception)
                {
                    throw new Exception("数据源第" + Convert.ToInt32(i + 2) + "行零件数量异常,请检查");
                }
                bomTable.Rows.Add(dr);//添加零件临时表                
            }
            List<T_Bom_Detail> insertBomDetailList = new List<T_Bom_Detail>();
            var query = from t in bomTable.AsEnumerable()
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
                    throw new Exception("数据源第" + Convert.ToInt32(j + 6) + "行零件编码有重复,请检查");
                }
            }//验证导入Excel是否存在重复编码

            //int InsertId = new Repository<T_Bom_Detail>().FindCountBySql("SELECT ISNULL(MAX(F_ID), 0) + 1 FROM T_BOM_DETAIL");
            for (int i = 0; i < bomTable.Rows.Count; i++)
            {
                T_Bom_Detail t_Bom_Detail = new T_Bom_Detail();
                t_Bom_Detail.F_Num = int.Parse(bomTable.Rows[i]["零件数量"].ToString());
                t_Bom_Detail.F_Part_Code = bomTable.Rows[i]["零件编码"].ToString();
                t_Bom_Detail.F_Bom_Id = F_Id;
                t_Bom_Detail.F_Enable_Mark = 1;
                t_Bom_Detail.F_Create_Time = DateTime.Now;
                t_Bom_Detail.F_Create_By = ManageProvider.Provider.Current().User.F_Account;
                insertBomDetailList.Add(t_Bom_Detail);
                //InsertId++;
            }
            using (var isOpenTrans = new Repository<T_Bom_Detail>().BeginTrans())
            {
                try
                {
                    if (t_product.F_Code != null && t_product.F_Name != null)
                    {
                        new RepositoryFactory<T_Product>().SubmitForm(t_product, isOpenTrans);//添加产品                    
                    }
                    new BomDetailBLL().Modify(F_Id, isOpenTrans);//修改详情
                    new BomDetailBLL().InsertFormBatch(insertBomDetailList, isOpenTrans);//批量插入bom详情
                    new Repository<T_Bom_Detail>().Commit();
                    return true;
                }
                catch (Exception)
                {
                    new Repository<T_Bom_Detail>().Rollback();
                    throw;
                }
                finally
                {
                    new Repository<T_Bom_Detail>().Close();
                }
            }
        }





    }
}
