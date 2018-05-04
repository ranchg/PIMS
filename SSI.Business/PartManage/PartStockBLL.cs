using SSI.Entity.PartManage;
using SSI.Repository;
using SSI.Utilities;
using System.Data;

namespace SSI.Business.PartManage
{
    public class PartStockBLL
    {
        public DataTable GetGridList(GridParam gp, string productId = null)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from = @"
                SELECT
	                T1.*
                FROM
	                (
		                SELECT
			                T1.F_NAME F_PART_NAME,
			                T1.F_CODE F_PART_CODE,
			                T1.F_SPEC F_PART_SPEC,
			                T1.F_UNIT F_PART_UNIT,
			                T2.F_QUANTITY F_STOCK_QUANTITY,
			                T3.F_QUANTITY F_CONSUME_QUANTITY,
			                T4.F_QUANTITY F_ONWAY_QUANTITY,
			                ISNULL(T2.F_QUANTITY, 0) - ISNULL(T3.F_QUANTITY, 0) F_AVAILABLE_QUANTITY
		                FROM
			                (
				                SELECT
					                T.F_NAME,
					                T.F_CODE,
					                T.F_SPEC,
					                T.F_UNIT
				                FROM
					                V_PART T
			                ) T1
		                LEFT JOIN (
			                SELECT
				                T.F_PART_CODE,
				                T.F_QUANTITY
			                FROM
				                V_PART_STOCK T
		                ) T2 ON T2.F_PART_CODE = T1.F_CODE
		                LEFT JOIN (
			                SELECT
				                T3.F_PART_CODE,
				                SUM (T1.F_QUANTITY * T3.F_NUM) F_QUANTITY
			                FROM
				                (
					                SELECT
						                T.F_PRODUCT_ID,
						                SUM (T.F_QUANTITY) F_QUANTITY
					                FROM
						                T_PRODUCT_MAKE T
					                WHERE
						                T.F_DELETE_MARK = 0
					                AND T.F_MAKE_DATE > GETDATE()
					                GROUP BY
						                T.F_PRODUCT_ID
				                ) T1
			                LEFT JOIN (
				                SELECT
					                T1.F_PRODUCT_ID,
					                T1.F_VERSION,
					                T2.F_ID
				                FROM
					                (
						                SELECT
							                T.F_PRODUCT_ID,
							                MAX (T.F_VERSION) F_VERSION
						                FROM
							                T_BOM T
						                WHERE
							                T.F_DELETE_MARK = 0
						                AND T.F_DATE < GETDATE()
						                GROUP BY
							                T.F_PRODUCT_ID
					                ) T1
				                LEFT JOIN T_BOM T2 ON T1.F_PRODUCT_ID = T2.F_PRODUCT_ID
				                AND T1.F_VERSION = T2.F_VERSION
			                ) T2 ON T2.F_PRODUCT_ID = T1.F_PRODUCT_ID
			                LEFT JOIN (
				                SELECT
					                T.F_BOM_ID,
					                T.F_PART_CODE,
					                T.F_NUM
				                FROM
					                T_BOM_DETAIL T
				                WHERE
					                T.F_DELETE_MARK = 0
			                ) T3 ON T3.F_BOM_ID = T2.F_ID
			                GROUP BY
				                T3.F_PART_CODE
		                ) T3 ON T3.F_PART_CODE = T1.F_CODE
		                LEFT JOIN (
			                SELECT
				                T.F_PART_CODE,
				                SUM (
					                CASE
					                WHEN T.F_ATA IS NULL THEN
						                T.F_QUANTITY
					                END
				                ) F_QUANTITY
			                FROM
				                T_PART_BUY T
			                WHERE
				                T.F_DELETE_MARK = 0
			                GROUP BY
				                T.F_PART_CODE
		                ) T4 ON T4.F_PART_CODE = T1.F_CODE
	                ) T1";
            if (!string.IsNullOrEmpty(productId))
            {
                from += string.Format(@"
                    RIGHT JOIN (
	                    SELECT DISTINCT
		                    T2.F_PART_CODE
	                    FROM
		                    (
			                    SELECT
				                    T1.F_PRODUCT_ID,
				                    T1.F_VERSION,
				                    T2.F_ID
			                    FROM
				                    (
					                    SELECT
						                    T.F_PRODUCT_ID,
						                    MAX (T.F_VERSION) F_VERSION
					                    FROM
						                    T_BOM T
					                    WHERE
						                    T.F_DELETE_MARK = 0
					                    AND F_PRODUCT_ID = {0}
					                    AND T.F_DATE < GETDATE()
					                    GROUP BY
						                    T.F_PRODUCT_ID
				                    ) T1
			                    LEFT JOIN T_BOM T2 ON T1.F_PRODUCT_ID = T2.F_PRODUCT_ID
			                    AND T1.F_VERSION = T2.F_VERSION
		                    ) T1
	                    LEFT JOIN (
		                    SELECT
			                    T.F_BOM_ID,
			                    T.F_PART_CODE
		                    FROM
			                    T_BOM_DETAIL T
		                    WHERE
			                    T.F_DELETE_MARK = 0
	                    ) T2 ON T2.F_BOM_ID = T1.F_ID
                    ) T2 ON T2.F_PART_CODE = T1.F_PART_CODE", productId);
            }
            if (!string.IsNullOrEmpty(gp.query))
            {
                where += ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return new Repository<T_Part>().FindTablePageBySql(sql, ref gp);
        }

        public DataTable Export(string field, string query, string productId = null)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from = @"
                SELECT
	                T1.*
                FROM
	                (
		                SELECT
			                T1.F_NAME F_PART_NAME,
			                T1.F_CODE F_PART_CODE,
			                T1.F_SPEC F_PART_SPEC,
			                T1.F_UNIT F_PART_UNIT,
			                T2.F_QUANTITY F_STOCK_QUANTITY,
			                T3.F_QUANTITY F_CONSUME_QUANTITY,
			                T4.F_QUANTITY F_ONWAY_QUANTITY,
			                ISNULL(T2.F_QUANTITY, 0) - ISNULL(T3.F_QUANTITY, 0) F_AVAILABLE_QUANTITY
		                FROM
			                (
				                SELECT
					                T.F_NAME,
					                T.F_CODE,
					                T.F_SPEC,
					                T.F_UNIT
				                FROM
					                V_PART T
			                ) T1
		                LEFT JOIN (
			                SELECT
				                T.F_PART_CODE,
				                T.F_QUANTITY
			                FROM
				                V_PART_STOCK T
		                ) T2 ON T2.F_PART_CODE = T1.F_CODE
		                LEFT JOIN (
			                SELECT
				                T3.F_PART_CODE,
				                SUM (T1.F_QUANTITY * T3.F_NUM) F_QUANTITY
			                FROM
				                (
					                SELECT
						                T.F_PRODUCT_ID,
						                SUM (T.F_QUANTITY) F_QUANTITY
					                FROM
						                T_PRODUCT_MAKE T
					                WHERE
						                T.F_DELETE_MARK = 0
					                AND T.F_MAKE_DATE > GETDATE()
					                GROUP BY
						                T.F_PRODUCT_ID
				                ) T1
			                LEFT JOIN (
				                SELECT
					                T1.F_PRODUCT_ID,
					                T1.F_VERSION,
					                T2.F_ID
				                FROM
					                (
						                SELECT
							                T.F_PRODUCT_ID,
							                MAX (T.F_VERSION) F_VERSION
						                FROM
							                T_BOM T
						                WHERE
							                T.F_DELETE_MARK = 0
						                AND T.F_DATE < GETDATE()
						                GROUP BY
							                T.F_PRODUCT_ID
					                ) T1
				                LEFT JOIN T_BOM T2 ON T1.F_PRODUCT_ID = T2.F_PRODUCT_ID
				                AND T1.F_VERSION = T2.F_VERSION
			                ) T2 ON T2.F_PRODUCT_ID = T1.F_PRODUCT_ID
			                LEFT JOIN (
				                SELECT
					                T.F_BOM_ID,
					                T.F_PART_CODE,
					                T.F_NUM
				                FROM
					                T_BOM_DETAIL T
				                WHERE
					                T.F_DELETE_MARK = 0
			                ) T3 ON T3.F_BOM_ID = T2.F_ID
			                GROUP BY
				                T3.F_PART_CODE
		                ) T3 ON T3.F_PART_CODE = T1.F_CODE
		                LEFT JOIN (
			                SELECT
				                T.F_PART_CODE,
				                SUM (
					                CASE
					                WHEN T.F_ATA IS NULL THEN
						                T.F_QUANTITY
					                END
				                ) F_QUANTITY
			                FROM
				                T_PART_BUY T
			                WHERE
				                T.F_DELETE_MARK = 0
			                GROUP BY
				                T.F_PART_CODE
		                ) T4 ON T4.F_PART_CODE = T1.F_CODE
	                ) T1";
            if (!string.IsNullOrEmpty(productId))
            {
                from += string.Format(@"
                    RIGHT JOIN (
	                    SELECT DISTINCT
		                    T2.F_PART_CODE
	                    FROM
		                    (
			                    SELECT
				                    T1.F_PRODUCT_ID,
				                    T1.F_VERSION,
				                    T2.F_ID
			                    FROM
				                    (
					                    SELECT
						                    T.F_PRODUCT_ID,
						                    MAX (T.F_VERSION) F_VERSION
					                    FROM
						                    T_BOM T
					                    WHERE
						                    T.F_DELETE_MARK = 0
					                    AND F_PRODUCT_ID = {0}
					                    AND T.F_DATE < GETDATE()
					                    GROUP BY
						                    T.F_PRODUCT_ID
				                    ) T1
			                    LEFT JOIN T_BOM T2 ON T1.F_PRODUCT_ID = T2.F_PRODUCT_ID
			                    AND T1.F_VERSION = T2.F_VERSION
		                    ) T1
	                    LEFT JOIN (
		                    SELECT
			                    T.F_BOM_ID,
			                    T.F_PART_CODE
		                    FROM
			                    T_BOM_DETAIL T
		                    WHERE
			                    T.F_DELETE_MARK = 0
	                    ) T2 ON T2.F_BOM_ID = T1.F_ID
                    ) T2 ON T2.F_PART_CODE = T1.F_PART_CODE", productId);
            }
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            if (!string.IsNullOrEmpty(query))
            {
                where += ConditionBuilder.GetWhereSql2(query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return new Repository<T_Part>().FindTableBySql(sql);
        }
    }
}
