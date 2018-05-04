using SSI.Entity.EquipmentManage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.SystemManage
{
    public class EquipmentBLL : RepositoryFactory<T_Equipment>
    {
        //获取表格列表 By 阮创 2017/11/30
        public DataTable GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from = @"
                SELECT
	                T1.*, T2.F_OPER_TIME F_PREV_OPER_TIME,
	                (
		                CASE
		                WHEN T2.F_OPER_TIME IS NULL THEN
			                GETDATE()
		                ELSE
			                T2.F_OPER_TIME + T1.F_PERIOD
		                END
	                ) F_NEXT_OPER_TIME
                FROM
	                (
		                SELECT
			                T.*
		                FROM
			                T_EQUIPMENT T
		                WHERE
			                T.F_DELETE_MARK = 0
	                ) T1
                LEFT JOIN (
	                SELECT
		                T.F_EQUIPMENT_ID,
		                MAX (T.F_OPER_TIME) F_OPER_TIME
	                FROM
		                T_MAINTENANCE T
	                WHERE
		                T.F_DELETE_MARK = 0
	                GROUP BY
		                T.F_EQUIPMENT_ID
                ) T2 ON T2.F_EQUIPMENT_ID = T1.F_ID";
            if (!string.IsNullOrEmpty(gp.query))
            {
                where += ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindTablePageBySql(sql, ref gp);
        }

        //获取列表 By 阮创 2017/11/30
        public List<T_Equipment> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_EQUIPMENT T1
             WHERE T1.F_DELETE_MARK = 0
             ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }

        public DataTable Export(string field, string query)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from = @"
                SELECT
	                T1.*, T2.F_OPER_TIME F_PREV_OPER_TIME,
	                (
		                CASE
		                WHEN T2.F_OPER_TIME IS NULL THEN
			                GETDATE()
		                ELSE
			                T2.F_OPER_TIME + T1.F_PERIOD
		                END
	                ) F_NEXT_OPER_TIME
                FROM
	                (
		                SELECT
			                T.*
		                FROM
			                T_EQUIPMENT T
		                WHERE
			                T.F_DELETE_MARK = 0
	                ) T1
                LEFT JOIN (
	                SELECT
		                T.F_EQUIPMENT_ID,
		                MAX (T.F_OPER_TIME) F_OPER_TIME
	                FROM
		                T_MAINTENANCE T
	                WHERE
		                T.F_DELETE_MARK = 0
	                GROUP BY
		                T.F_EQUIPMENT_ID
                ) T2 ON T2.F_EQUIPMENT_ID = T1.F_ID";
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
