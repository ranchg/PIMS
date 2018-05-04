using SSI.Entity.ProductManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.ProductManage
{
    public class ProductBLL : RepositoryFactory<T_Product>
    {
        //获取表格列表 By 阮创 2017/11/30
        public List<T_Product> GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*
              FROM T_PRODUCT T1
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(gp.search))
            {
                where += string.Format(" AND (F_NAME LIKE '%{0}%' OR F_CODE LIKE '%{0}%')", gp.search);
            }
            string sql = string.Format("{0} ({1}) T {2}", select, from, where);
            return Repository().FindListPageBySql(sql, ref gp);
        }

        //获取列表 By 阮创 2017/11/30
        public List<T_Product> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_PRODUCT T1
             WHERE T1.F_DELETE_MARK = 0
            ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }

        //导出数据 By 阮创 2017/11/30
        public DataTable ExportExcel(string field)
        {
            string select = "SELECT * FROM";
            string from =
            @"SELECT T1.F_NAME,
                     T1.F_CODE,
                     T1.F_SPEC,
                     T1.F_UNIT,
                   (CASE T1.F_ENABLE_MARK
                     WHEN 1 THEN
                      '有效'
                     WHEN 0 THEN
                      '无效'
                     ELSE
                      '类型错误'
                   END) AS F_ENABLE_MARK,
                   T1.F_CREATE_TIME
              FROM T_PRODUCT T1
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            string sql = string.Format("{0} ({1})", select, from);
            return Repository().FindTableBySql(sql);
        }
    }
}
