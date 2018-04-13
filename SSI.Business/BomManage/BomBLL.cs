using SSI.Entity.BomManage;
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
            StringBuilder sb = new StringBuilder(@"SELECT * FROM (SELECT T1.*,T2.F_NAME AS F_PRODUCT_NAME FROM T_BOM T1 INNER JOIN T_PRODUCT T2 ON T1.F_PRODUCT_ID=T2.F_ID WHERE T1.F_DELETE_MARK = 0) where 1=1");
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
            string sql = string.Format("{0} ({1}) {2} {3}", select, from, where, orderby);
            return Repository().FindTableBySql(sql);
        }


        public DataTable GetBom()
        {
            string querySql = "SELECT * FROM T_BOM WHERE F_DELETE_MARK=0";
            return Repository().FindTableBySql(querySql);
        }
    }
}
