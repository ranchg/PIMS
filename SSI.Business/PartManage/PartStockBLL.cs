using SSI.Entity.PartManage;
using SSI.Repository;
using SSI.Utilities;
using System.Data;

namespace SSI.Business.PartManage
{
    public class PartStockBLL : RepositoryFactory<T_Part>
    {
        public DataTable GetGridList(GridParam gp, string productId = null)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1", product = "";
            if (!string.IsNullOrEmpty(productId))
            {
                product = string.Format(" HAVING T2.F_PRODUCT_ID={0}", productId);
            }
            string from = string.Format(
            @"SELECT T1.F_ID,
                   T1.F_NAME F_PART_NAME,
                   T1.F_CODE F_PART_CODE,
                   T1.F_SPEC F_PART_SPEC,
                   T1.F_UNIT F_PART_UNIT,
                   T2.F_TIME F_CHECK_TIME,
                   T2.F_QUANTITY F_CHECK_QUANTITY,
                   T3.F_QUANTITY F_CONSUME_QUANTITY,
                   T4.F_QUANTITY F_REACH_QUANTITY,
                   T5.F_QUANTITY F_ONWAY_QUANTITY,
                   NVL(T2.F_QUANTITY, 0) - NVL(T3.F_QUANTITY, 0) +
                   NVL(T4.F_QUANTITY, 0) F_AVAILABLE_QUANTITY
              FROM (SELECT T.F_ID, T.F_NAME, T.F_CODE, T.F_SPEC, T.F_UNIT
                      FROM V_PART T
                     WHERE T.F_DELETE_MARK = 0) T1
              LEFT JOIN (SELECT T2.F_PART_ID, T2.F_TIME_ID, T2.F_QUANTITY, T1.F_TIME
                           FROM (SELECT T.F_ID, T.F_TIME
                                   FROM (SELECT T.F_ID, T.F_TIME
                                           FROM T_PART_CHECK_TIME T
                                          WHERE T.F_DELETE_MARK = 0
                                          ORDER BY T.F_TIME DESC, T.F_CREATE_TIME DESC) T
                                  WHERE ROWNUM = 1) T1
                           LEFT JOIN (SELECT T.F_PART_ID, T.F_TIME_ID, T.F_QUANTITY
                                       FROM T_PART_CHECK T
                                      WHERE T.F_DELETE_MARK = 0) T2
                             ON T2.F_TIME_ID = T1.F_ID) T2
                ON T2.F_PART_ID = T1.F_ID
              LEFT JOIN (SELECT T3.F_PART_ID, SUM(T1.F_QUANTITY * T3.F_NUM) F_QUANTITY
                           FROM (SELECT T2.F_PRODUCT_ID, SUM(T2.F_QUANTITY) F_QUANTITY
                                   FROM (SELECT T.F_ID, T.F_TIME
                                           FROM (SELECT T.F_ID, T.F_TIME
                                                   FROM T_PART_CHECK_TIME T
                                                  WHERE T.F_DELETE_MARK = 0
                                                  ORDER BY T.F_TIME        DESC,
                                                           T.F_CREATE_TIME DESC) T
                                          WHERE ROWNUM = 1) T1
                                   LEFT JOIN (SELECT T.F_PRODUCT_ID,
                                                    T.F_QUANTITY,
                                                    T.F_MAKE_DATE
                                               FROM T_PRODUCT_MAKE T
                                              WHERE T.F_DELETE_MARK = 0) T2
                                     ON T2.F_MAKE_DATE >= T1.F_TIME
                                  GROUP BY T2.F_PRODUCT_ID {0}) T1
                           LEFT JOIN (SELECT T1.F_PRODUCT_ID, T1.F_VERSION, T2.F_ID
                                       FROM (SELECT T.F_PRODUCT_ID,
                                                    MAX(T.F_VERSION) F_VERSION
                                               FROM T_BOM T
                                              WHERE T.F_DELETE_MARK = 0
                                                AND T.F_DATE <= SYSDATE
                                              GROUP BY T.F_PRODUCT_ID) T1
                                       LEFT JOIN T_BOM T2
                                         ON T1.F_PRODUCT_ID = T2.F_PRODUCT_ID
                                        AND T1.F_VERSION = T2.F_VERSION) T2
                             ON T2.F_PRODUCT_ID = T1.F_PRODUCT_ID
                           LEFT JOIN (SELECT T.F_BOM_ID, T.F_PART_ID, T.F_NUM
                                       FROM T_BOM_DETAIL T
                                      WHERE T.F_DELETE_MARK = 0) T3
                             ON T3.F_BOM_ID = T2.F_ID
                          GROUP BY T3.F_PART_ID) T3
                ON T3.F_PART_ID = T1.F_ID
              LEFT JOIN (SELECT T2.F_PART_ID,
                                SUM(CASE
                                      WHEN T2.F_ATA IS NOT NULL THEN
                                       T2.F_QUANTITY
                                    END) F_QUANTITY
                           FROM (SELECT T.F_ID, T.F_TIME
                                   FROM (SELECT T.F_ID, T.F_TIME
                                           FROM T_PART_CHECK_TIME T
                                          WHERE T.F_DELETE_MARK = 0
                                          ORDER BY T.F_TIME DESC, T.F_CREATE_TIME DESC) T
                                  WHERE ROWNUM = 1) T1
                           LEFT JOIN (SELECT T.F_PART_ID, T.F_ETA, T.F_ATA, T.F_QUANTITY
                                       FROM T_PART_BUY T
                                      WHERE T.F_DELETE_MARK = 0) T2
                             ON T2.F_ATA >= T1.F_TIME
                          GROUP BY T2.F_PART_ID) T4
                ON T4.F_PART_ID = T1.F_ID
              LEFT JOIN (SELECT T.F_PART_ID,
                                SUM(CASE
                                      WHEN T.F_ATA IS NULL THEN
                                       T.F_QUANTITY
                                    END) F_QUANTITY
                           FROM T_PART_BUY T
                          WHERE T.F_DELETE_MARK = 0
                          GROUP BY T.F_PART_ID) T5
                ON T5.F_PART_ID = T1.F_ID", product);
            if (!string.IsNullOrEmpty(gp.query))
            {
                where += ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindTablePageBySql(sql, ref gp);
        }

        public DataTable Export(string field, string query, string productId = null)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1", product = "";
            if (!string.IsNullOrEmpty(productId))
            {
                product = string.Format(" HAVING T2.F_PRODUCT_ID={0}", productId);
            }
            string from = string.Format(
            @"SELECT T1.F_ID,
                   T1.F_NAME F_PART_NAME,
                   T1.F_CODE F_PART_CODE,
                   T1.F_SPEC F_PART_SPEC,
                   T1.F_UNIT F_PART_UNIT,
                   T2.F_TIME F_CHECK_TIME,
                   T2.F_QUANTITY F_CHECK_QUANTITY,
                   T3.F_QUANTITY F_CONSUME_QUANTITY,
                   T4.F_QUANTITY F_REACH_QUANTITY,
                   T5.F_QUANTITY F_ONWAY_QUANTITY,
                   NVL(T2.F_QUANTITY, 0) - NVL(T3.F_QUANTITY, 0) +
                   NVL(T4.F_QUANTITY, 0) F_AVAILABLE_QUANTITY
              FROM (SELECT T.F_ID, T.F_NAME, T.F_CODE, T.F_SPEC, T.F_UNIT
                      FROM T_PART T
                     WHERE T.F_DELETE_MARK = 0) T1
              LEFT JOIN (SELECT T2.F_PART_ID, T2.F_TIME_ID, T2.F_QUANTITY, T1.F_TIME
                           FROM (SELECT T.F_ID, T.F_TIME
                                   FROM (SELECT T.F_ID, T.F_TIME
                                           FROM T_PART_CHECK_TIME T
                                          WHERE T.F_DELETE_MARK = 0
                                          ORDER BY T.F_TIME DESC, T.F_CREATE_TIME DESC) T
                                  WHERE ROWNUM = 1) T1
                           LEFT JOIN (SELECT T.F_PART_ID, T.F_TIME_ID, T.F_QUANTITY
                                       FROM T_PART_CHECK T
                                      WHERE T.F_DELETE_MARK = 0) T2
                             ON T2.F_TIME_ID = T1.F_ID) T2
                ON T2.F_PART_ID = T1.F_ID
              LEFT JOIN (SELECT T3.F_PART_ID, SUM(T1.F_QUANTITY * T3.F_NUM) F_QUANTITY
                           FROM (SELECT T2.F_PRODUCT_ID, SUM(T2.F_QUANTITY) F_QUANTITY
                                   FROM (SELECT T.F_ID, T.F_TIME
                                           FROM (SELECT T.F_ID, T.F_TIME
                                                   FROM T_PART_CHECK_TIME T
                                                  WHERE T.F_DELETE_MARK = 0
                                                  ORDER BY T.F_TIME        DESC,
                                                           T.F_CREATE_TIME DESC) T
                                          WHERE ROWNUM = 1) T1
                                   LEFT JOIN (SELECT T.F_PRODUCT_ID,
                                                    T.F_QUANTITY,
                                                    T.F_MAKE_DATE
                                               FROM T_PRODUCT_MAKE T
                                              WHERE T.F_DELETE_MARK = 0) T2
                                     ON T2.F_MAKE_DATE >= T1.F_TIME
                                  GROUP BY T2.F_PRODUCT_ID {0}) T1
                           LEFT JOIN (SELECT T1.F_PRODUCT_ID, T1.F_VERSION, T2.F_ID
                                       FROM (SELECT T.F_PRODUCT_ID,
                                                    MAX(T.F_VERSION) F_VERSION
                                               FROM T_BOM T
                                              WHERE T.F_DELETE_MARK = 0
                                                AND T.F_DATE <= SYSDATE
                                              GROUP BY T.F_PRODUCT_ID) T1
                                       LEFT JOIN T_BOM T2
                                         ON T1.F_PRODUCT_ID = T2.F_PRODUCT_ID
                                        AND T1.F_VERSION = T2.F_VERSION) T2
                             ON T2.F_PRODUCT_ID = T1.F_PRODUCT_ID
                           LEFT JOIN (SELECT T.F_BOM_ID, T.F_PART_ID, T.F_NUM
                                       FROM T_BOM_DETAIL T
                                      WHERE T.F_DELETE_MARK = 0) T3
                             ON T3.F_BOM_ID = T2.F_ID
                          GROUP BY T3.F_PART_ID) T3
                ON T3.F_PART_ID = T1.F_ID
              LEFT JOIN (SELECT T2.F_PART_ID,
                                SUM(CASE
                                      WHEN T2.F_ATA IS NOT NULL THEN
                                       T2.F_QUANTITY
                                    END) F_QUANTITY
                           FROM (SELECT T.F_ID, T.F_TIME
                                   FROM (SELECT T.F_ID, T.F_TIME
                                           FROM T_PART_CHECK_TIME T
                                          WHERE T.F_DELETE_MARK = 0
                                          ORDER BY T.F_TIME DESC, T.F_CREATE_TIME DESC) T
                                  WHERE ROWNUM = 1) T1
                           LEFT JOIN (SELECT T.F_PART_ID, T.F_ETA, T.F_ATA, T.F_QUANTITY
                                       FROM T_PART_BUY T
                                      WHERE T.F_DELETE_MARK = 0) T2
                             ON T2.F_ATA >= T1.F_TIME
                          GROUP BY T2.F_PART_ID) T4
                ON T4.F_PART_ID = T1.F_ID
              LEFT JOIN (SELECT T.F_PART_ID,
                                SUM(CASE
                                      WHEN T.F_ATA IS NULL THEN
                                       T.F_QUANTITY
                                    END) F_QUANTITY
                           FROM T_PART_BUY T
                          WHERE T.F_DELETE_MARK = 0
                          GROUP BY T.F_PART_ID) T5
                ON T5.F_PART_ID = T1.F_ID", product);
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            if (!string.IsNullOrEmpty(query))
            {
                where += ConditionBuilder.GetWhereSql2(query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindTableBySql(sql);
        }
    }
}
