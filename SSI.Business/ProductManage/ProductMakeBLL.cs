using SSI.DataAccess;
using SSI.Entity.ProductManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SSI.Business.ProductManage
{
    public class ProductMakeBLL : RepositoryFactory<T_Product_Make>
    {
        private LogHelper log = LogFactory.GetLogger(typeof(ProductMakeBLL));
        //获取表格列表 By 阮创 2017/11/30
        public List<T_Product_Make> GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*
              FROM T_PRODUCT_MAKE T1
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(gp.search))
            {
                where += string.Format(" AND (F_NAME LIKE '%{0}%' OR F_CODE LIKE '%{0}%')", gp.search);
            }
            if (!string.IsNullOrEmpty(gp.query))
            {
                where += ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>()).Replace("F_Make_Date", "to_date(F_Make_Date,'yyyy-mm-dd')");
            }
            string sql = string.Format("{0} ({1}) {2}", select, from, where);
            return Repository().FindListPageBySql(sql, ref gp);
        }

        //获取列表 By 阮创 2017/11/30
        public List<T_Product_Make> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_PRODUCT_MAKE T1
             WHERE T1.F_DELETE_MARK = 0
            ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }

        //导出数据 By 阮创 2017/11/30
        public DataTable ExportExcel(string field)
        {
            string select = "SELECT * FROM";
            string from =
            @"SELECT T1.F_PRODUCT_ID,
                     T1.F_QUANTITY,
                     T1.F_MAKE_DATE,
                   (CASE T1.F_ENABLE_MARK
                     WHEN 1 THEN
                      '有效'
                     WHEN 0 THEN
                      '无效'
                     ELSE
                      '类型错误'
                   END) AS F_ENABLE_MARK,
                    T2.F_NAME F_PRODUCT_NAME,
                   T1.F_CREATE_TIME
              FROM T_PRODUCT_MAKE T1
                LEFT JOIN T_PRODUCT T2 ON T1.F_PRODUCT_ID=T2.F_ID
             WHERE T1.F_DELETE_MARK = 0 AND T2.F_DELETE_MARK=0 AND T2.F_ENABLE_MARK=1";
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            string sql = string.Format("{0} ({1})", select, from);
            return Repository().FindTableBySql(sql);
        }

        //导出数据 By 阮创 2017/11/30
        private DataTable GetPartInfo(int F_Product_Id, DateTime? F_Make_Date)
        {
            string sql = string.Format(@"select t1.F_ID,t2.F_NUM
from T_PART_STOCK t1
left join T_BOM_DETAIL t2 on t2.F_PART_ID=t1.F_PART_ID and t2.F_ENABLE_MARK=1 and t2.F_DELETE_MARK=0
left join T_STAT_PERIOD t3 on t3.F_ID=t1.F_STAT_PERIOD_ID and t3.F_ENABLE_MARK=1 and t3.F_DELETE_MARK=0
where t2.F_BOM_ID=(SELECT DISTINCT FIRST_VALUE(F_ID) OVER(PARTITION BY F_PRODUCT_ID ORDER BY F_DATE DESC) F_BOM_ID 
          FROM T_BOM WHERE F_PRODUCT_ID = {0} AND F_DATE<= TO_DATE('{1}', 'yyyy/mm/dd') AND F_ENABLE_MARK=1 and F_DELETE_MARK=0)
          and t3.F_START_DATE < TO_DATE('{1}', 'YYYY/MM/DD') AND F_END_DATE > TO_DATE('{1}', 'yyyy/mm/dd') and t1.F_ENABLE_MARK=1 and t1.F_DELETE_MARK=0",
                                 F_Product_Id,
                                 F_Make_Date);
            return Repository().FindTableBySql(sql);
        }

        /// <summary>
        /// 计算添加的产品生产信息所需的所有零件数量
        /// </summary>
        /// <param name="t_Product_Make"></param>
        public void CountPart(int F_Id)
        {
            try
            {
                T_Product_Make t_Product_Make = GetForm(F_Id);
                DataTable dt = GetPartInfo(t_Product_Make.F_Product_Id, t_Product_Make.F_Make_Date);
                if (dt.Rows.Count == 0)
                    throw new Exception("找不到相应的BOM明细或在统计周期内的零件库存信息！");
                foreach (DataRow dr in dt.Rows)
                {
                    int ConsumeQuantity = t_Product_Make.F_Quantity * Convert.ToInt32(dr["F_NUM"]);
                    StringBuilder str = new StringBuilder();
                    str.AppendFormat("UPDATE T_PART_STOCK SET F_MAKE_CONSUME_QUANTITY=F_MAKE_CONSUME_QUANTITY+{0} WHERE F_ID={1}", ConsumeQuantity, dr["F_ID"]);
                    Repository().ExecuteBySql(str);
                }
                Repository().ExecuteBySql(new StringBuilder("UPDATE T_PRODUCT_MAKE SET F_IS_READ=1 WHERE F_ID=" + t_Product_Make.F_Id));
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }
    }
}




